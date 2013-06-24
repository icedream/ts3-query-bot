using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.ComponentModel.Composition;
namespace TS3Query.Plugins
{
    [Export(typeof(TS3QueryBotPlugin))]
    [PluginMetadata("Welcome plugin", "Icedream", Description="Plays a sound when a user joins the channel in which the bot is.")]
    public class WelcomePlugin : TS3QueryBotPlugin
    {
        DateTime _lastWelcomePlayback = DateTime.MinValue;

        XmlDocument _config = new XmlDocument();

        string ourID = string.Empty;

        public WelcomePlugin()
        {
            _config.Load(Path.Combine(ConfigFolderPath, "plugin.xml"));
        }

        public override void Initialize()
        {
            Client.Connected += (s, e) =>
            {
                Client.Send(new TS3QueryRequest("whoami"));
            };
            Client.QueryResponseReceived += (s, e) =>
            {
                switch (e.Response.Name)
                {
                    case "notifycliententerview":
                        // "ctid" = Channel Target ID? Contains ID of channel in which the client joins.
                        if (DateTime.Now.Subtract(_lastWelcomePlayback) < TimeSpan.Parse(_config.SelectSingleNode("/settings/delay").InnerText))
                        {
                            break;
                        }

                        if (e.Response.Parameters["client_type"] == "1" /* Server query? */)
                        {
                            break;
                        }

                        var ctid = _config.SelectSingleNode("/settings/channelid").InnerText;
                        var soundplugin = Host.Plugins.Where(p => p is SoundPlugin).Single() as SoundPlugin; // TODO: Require dependency SoundPlugin

                        if (soundplugin.IsBusy)
                            break;

                        // Check if user is in configured channel
                        if (!e.Response.Parameters["ctid"].Equals(ctid))
                            break;

                        // Move to this channel
                        Client.Send(new TS3QueryRequest("clientmove", new Dictionary<string, string>
                        {
                            { "cid", ctid },
                            // TODO: Password
                            { "clid", ourID }
                        }));

                        // Play sound "welcome_sound"
                        soundplugin.Play(e.Response, _config.SelectSingleNode("/settings/sound").InnerText);


                        break;

                    default:
                        if (e.Response.Name.StartsWith("clid="))
                        {
                            ourID = e.Response.Name.Split('=').Last();
                        }
                        break;
                }
            };
        }
    }
}
