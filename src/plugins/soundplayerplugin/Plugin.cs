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
        XmlDocument xml = new XmlDocument();

        public SoundPlugin()
        {
        }

        public override void Initialize()
        {
            Console.WriteLine("Sound plugin initializing");

            xml.Load("sounds.xml");

            Console.WriteLine("Loaded {0} sounds.", xml.SelectNodes("/sounds/sound").Count);
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

        [PluginCommand("play", Description = "Plays a sound.", TargetMode = TS3MessageTargetMode.Private)]
        public void Play(TS3QueryResponse response, [PluginCommandParameter("alias", Description="The sound's alias.")]string alias)
        {
            if (p != null && !p.HasExited)
                return;

            if (xml.SelectNodes("/sounds/sound[@id='" + alias + "']").Count == 0)
            {
                Client.SendTextMessage(response.Parameters["invokerid"], "This sound alias does not exist.");
                return;
            }

            // get the sound
            var soundNode = xml.SelectSingleNode("/sounds/sound[@id='" + alias + "']");

            p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN", "VLC", "vlc.exe"),
                    Arguments = string.Format("-I qt --no-video-on-top --no-video --play-and-stop --play-and-exit --aout=aout_directx --directx-audio-device=\"Line 4 (TeamSpeak Out) (Virtual Audio Cable)\" \"{0}\"", soundNode.InnerText),
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
