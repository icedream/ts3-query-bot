using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TS3Query.Plugins
{
    [PluginMetadata("Sound plugin", "Icedream", Description="Allows playing sounds via the bot (via a virtual audio cable).")]
    public class SoundPlugin : TS3QueryBotPlugin
    {
        Process p = null;

        public SoundPlugin()
        {
        }

        [PluginCommand("play", Description = "Plays a sound or a video.", TargetMode = TS3MessageTargetMode.Private)]
        public void Play([PluginCommandParameter("url", Description="The URL.")]string url)
        {
            if (p != null && !p.HasExited)
                return;

            p = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN", "VLC", "vlc.exe"),
                    Arguments = string.Format("-I rc -vvv --aout=aout_directx --play-and-exit --directx-audio-device=\"Line 4 (TeamSpeak Out) (Virtual Audio Cable)\" {0}", url),
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false
                }
            };
            p.WaitForExit();
        }
    }
}
