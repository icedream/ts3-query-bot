using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml;

namespace TS3Query.Plugins
{
    [Export(typeof(TS3QueryBotPlugin))]
    [PluginMetadata("Sound plugin", "Icedream", Description="Allows playing sounds via the bot (via a virtual audio cable).")]
    public class SoundPlugin : TS3QueryBotPlugin
    {
        Process p = null;
        XmlDocument sounds = new XmlDocument();
        XmlDocument config = new XmlDocument();

        public SoundPlugin()
        {
            config.Load(Path.Combine(ConfigFolderPath, "plugin.xml"));

            sounds.Load(Path.Combine(ConfigFolderPath, "sounds.xml"));
            Console.WriteLine("Loaded {0} sounds.", sounds.SelectNodes("/sounds/sound").Count);
        }

        [PluginCommand("play", Description = "Prints help for the play command.", TargetMode = TS3MessageTargetMode.Private)]
        public void Play(TS3BotCommand command)
        {
            var helpPlugin = Host.Plugins.Where(p => p is HelpPlugin).Single() as HelpPlugin;

            helpPlugin.Private_Help(command.Response, command.Name);
        }

        [PluginCommand("stop", Description = "Stops the current sound.", TargetMode = TS3MessageTargetMode.Private)]
        public void Stop(TS3QueryResponse response)
        {
            try
            {
                if (p == null)
                    return;
                else p.Kill();
            }
            catch { { } }
        }

        [PluginCommand("sounds", Description = "Lists all available sound aliases.", TargetMode=TS3MessageTargetMode.Private)]
        public void Sounds(TS3QueryResponse response)
        {
            Client.SendTextMessage(response.Parameters["invokerid"], "Following sounds are available:\n" + string.Join("; ", sounds.SelectNodes("/sounds/sound").OfType<XmlNode>().Select(s => s.Attributes["id"].Value).OrderBy(k => k)));
        }

        [PluginCommand("play", Description = "Plays a sound.", TargetMode = TS3MessageTargetMode.Private)]
        public void Play(TS3QueryResponse response, [PluginCommandParameter("alias or URL", Description="The sound's alias or URL to a stream/remote audio file.")]string aliasOrURL)
        {
            if (p != null && !p.HasExited)
                return;

            aliasOrURL = aliasOrURL.Replace("[URL]", "").Replace("[/URL]", "");
            Console.WriteLine(aliasOrURL);

            if (!Uri.IsWellFormedUriString(aliasOrURL, UriKind.Absolute))
            {
                // Expect this to be a sound alias
                if (sounds.SelectNodes("/sounds/sound[@id='" + aliasOrURL + "']").Count == 0)
                {
                    Client.SendTextMessage(response.Parameters["invokerid"], "This sound alias does not exist.");
                    return;
                }

                // get the sound
                var soundNode = sounds.SelectSingleNode("/sounds/sound[@id='" + aliasOrURL + "']");

                aliasOrURL = soundNode.InnerText;
            }

            p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN", "VLC", "vlc.exe"),
                    // TODO: Implement other audio device types
                    Arguments = string.Format("-I qt --no-video-on-top --no-video --play-and-stop --play-and-exit --aout=\"{2}\" --directx-audio-device=\"{1}\" \"{0}\"", aliasOrURL, config.SelectSingleNode("/settings/audio/device").InnerText, config.SelectSingleNode("/settings/audio/type").InnerText),
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false
                }
            };
            p.Start();
            p.WaitForExit();
        }
    }
}
