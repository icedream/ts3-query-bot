using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace TS3Query.Plugins
{
    [Export(typeof(TS3QueryBotPlugin))]
    [PluginMetadata("Help plugin", "Icedream", Description="Provides help for all available plugins and their commands.")]
    public class HelpPlugin : TS3QueryBotPlugin
    {
        [PluginCommand("help", TargetMode = TS3MessageTargetMode.Private, Description = "Provides general help.")]
        public void Private_Help(TS3QueryResponse response)
        {
            string invokerID = response.Parameters["invokerid"];

            Client.SendTextMessage(invokerID, "You can get help for any command or plugin by typing [B]!help <plugin here>[/B] or [B]!help <command here>[/B].\nCommands from following plugins are available:\n\t" + string.Join(", ", Host.Plugins.Select(p => p.GetType().Name + " (" + p.Metadata.Name + ")")));
        }

        [PluginCommand("help", TargetMode = TS3MessageTargetMode.Private, Description = "Provides general help.")]
        public void Private_Help(TS3QueryResponse response, [PluginCommandParameter("plugin or command", Description="Provides help for a plugin or a command")] string pluginOrCommand)
        {
            if (Host.Plugins.Any(p => p.Metadata.Name.Equals(pluginOrCommand) || p.GetType().Name.Equals(pluginOrCommand)))
                Private_Help_Plugin(response, pluginOrCommand);
            else
                Private_Help_Command(response, pluginOrCommand);
        }

        private void Private_Help_Plugin(TS3QueryResponse response, string pluginName)
        {
            string invokerID = response.Parameters["invokerid"];
            var plugin = Host.Plugins.First(p => p.Metadata.Name.Equals(pluginName) || p.GetType().Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase));

            Client.SendTextMessage(invokerID, string.Format("Commands for plugin [B]{0}[/B] by {1}, Version {2}:\n\t{3}", plugin.Metadata.Name, plugin.Metadata.Author, plugin.Metadata.Version, plugin.Commands.Any() ? string.Join(", ", plugin.Commands.Select(cmd => cmd.Metadata.Name).Distinct()) : "[I]none[/I]"));
        }

        private void Private_Help_Command(TS3QueryResponse response, string command)
        {
            string invokerID = response.Parameters["invokerid"];

            List<TS3QueryBotPluginMethod> methods = new List<TS3QueryBotPluginMethod>();

            // Search for methods with this name
            foreach (TS3QueryBotPlugin plugin in Host.Plugins)
                methods.AddRange(plugin.Commands.Where(c => c.Metadata.Name.Equals(command)));

            if (!methods.Any())
            {
                Client.SendTextMessage(invokerID, string.Format("Could not find command or plugin [B]{0}[/B].", command));
                return;
            }

            if (methods.Count > 1)
            {
                StringBuilder sb = new StringBuilder("Multiple command variations are available. Type [B]!help " + command + " <variation>[/B] to get more info.\n");
                int i = 0;
                foreach (var method in methods) // TODO: Optimize with AppendFormat
                    sb.Append("Variation [B]" + (++i) + "[/B]" + (method.Metadata.Deprecated ? " (deprecated)" : "") + ": [I]" + GenerateSyntax(method) + "[/I] - " + (string.IsNullOrEmpty(method.Metadata.ShortDescription) ? method.Metadata.Description : method.Metadata.ShortDescription) + "\n");
                Client.SendTextMessage(invokerID, sb.ToString());
            }
            else
                Private_Help_Command(response, command, 1); // Take the first variation - the only one existing
        }

        private string GenerateSyntax(TS3QueryBotPluginMethod method)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("({0}) ", method.Metadata.TargetMode);
            sb.Append("!"); // prefix
            sb.Append(method.Metadata.Name); // command name
            foreach (var parameter in method.Parameters.Where(p => p.ParameterInfo.GetCustomAttributes(typeof(PluginCommandParameterAttribute), false).Any()))
                sb.AppendFormat(" [I]<{0}>[/I]", parameter.Metadata.Name);
            return sb.ToString();
        }

        [PluginCommand("help", TargetMode = TS3MessageTargetMode.Private, Description = "Gives detailed help for a specific command variation.")]
        public void Private_Help_Command(TS3QueryResponse response, [PluginCommandParameter("command", Description = "The command name.")]string command, [PluginCommandParameter("variation", Description = "The variation number.")]int variation)
        {
            string invokerID = response.Parameters["invokerid"];
            if (variation-- < 0)
            {
                Client.SendTextMessage(invokerID, "Invalid variation.");
                return;
            }

            List<TS3QueryBotPluginMethod> methods = new List<TS3QueryBotPluginMethod>();

            // Search for methods with this name
            foreach (TS3QueryBotPlugin plugin in Host.Plugins)
                methods.AddRange(plugin.Commands.Where(c => c.Metadata.Name.Equals(command)));

            if (!methods.Any())
            {
                Client.SendTextMessage(invokerID, string.Format("Could not find command or plugin [B]{0}[/B].", command));
                return;
            }

            if (variation >= methods.Count)
            {
                Client.SendTextMessage(invokerID, "This variation does not exist.");
                return;
            }

            var method = methods[variation];

            Client.SendTextMessage(invokerID, method.Metadata.Description);
            Client.SendTextMessage(invokerID, GenerateSyntax(method));
            var sb = new StringBuilder();
            var parameters = method.Parameters.Where(p => p.ParameterInfo.GetCustomAttributes(typeof(PluginCommandParameterAttribute), false).Any());
            if (parameters.Any())
            {
                sb.Append("Parameters:");
                foreach (var parameter in parameters)
                {
                    sb.AppendFormat("\n\t{2} [B]{0}[/B]\t[I]{1}[/I]", parameter.Metadata.Name, parameter.Metadata.Description, method.Metadata.Deprecated ? "[I][deprecated][/I] " : "");
                }
            }
            else sb.Append("Parameters: [I]none[/I]");
            Client.SendTextMessage(invokerID, sb.ToString());
        }
    }
}
