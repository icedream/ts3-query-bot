using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TS3Query
{
    public class TS3QueryBot
    {
        public static void Main(string[] args)
        {
            new TS3QueryBot().Run();

            Thread.Sleep(Timeout.Infinite);
        }

        public TS3Query Client { get; private set; }

        [ImportMany]
        public List<TS3QueryBotPlugin> Plugins { get; set; }

        internal TS3QueryBot()
        {
            Client = new TS3Query();
        }

        void Run()
        {
            #region Logging thingies
            #if DEBUG
            Client.Connected += (s, e) =>
            {
                Console.WriteLine("Query client connected.");
            };
            Client.Disconnected += (s, e) =>
            {
                Console.WriteLine("Query client disconnected.");
            };
            Client.QueryRequestSent += (s, e) =>
            {
                Console.WriteLine("Query request sent: {0}", e.Request.ToString());
            };
            Client.QueryResponseReceived += (s, e) =>
            {
                Console.WriteLine("Query response received: {0}", e.Response.ToString());
            };
            #endif
            #endregion

            #region Plugin loading stuff
            var pCatalog = new AggregateCatalog();
            
            // Add current path for plugins
            pCatalog.Catalogs.Add(new DirectoryCatalog(Environment.CurrentDirectory));

            // Add assembly path for plugins (if not equal to Environment.CurrentDirectory)
            var dc = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location));
            if (dc.TrimEnd(Path.DirectorySeparatorChar) != Path.GetFullPath(Environment.CurrentDirectory.TrimEnd(Path.DirectorySeparatorChar)))
            {
                pCatalog.Catalogs.Add(new DirectoryCatalog(dc));
            }

            // Compose all parts (plugins) to this instance
            var container = new CompositionContainer(pCatalog);
            try
            {
                // Tell every plugin about this instance
                container.ComposeExportedValue(this.Client); // _query
                container.ComposeExportedValue(this); // _host

                // Compose all parts (plugins loading here)
                container.ComposeParts(this);
            }
            catch (CompositionException error)
            {
                Console.Error.WriteLine("Could not load plugins. Error: {0}", error.ToString());
            }

            foreach (var plugin in Plugins)
            {
                try
                {
                    Console.WriteLine("Initializing plugin \"{0}\"...", plugin.Metadata.Name);
                    plugin.Initialize();
                }
                catch(Exception error)
                {
                    Console.Error.WriteLine("Plugin \"{0}\" encountered this error and has to be unloaded: {1}", plugin.Metadata.Name, error.ToString());
                    Plugins.Remove(plugin);
                }
            }

            Console.WriteLine("{0} plugins loaded and initialized.", this.Plugins.Count());

            #endregion

            // Actual incoming data handler
            Client.QueryResponseReceived += (s, e) =>
            {
                foreach (var plugin in Plugins)
                    plugin.HandleResponse(e.Response);

                if (TS3BotCommand.IsValidBotCommand(e.Response))
                {
                    TS3BotCommand bc = null;
                    try
                    {
                        bc = new TS3BotCommand(e.Response);
                    }
                    catch (ArgumentException error)
                    {
                        Client.Send(
                            new TS3QueryRequest(
                                "sendtextmessage",
                                new Dictionary<string, string>
                                {
                                    { "targetmode", e.Response.Parameters["targetmode"] },
                                    { "target", e.Response.Parameters["invokerid"] },
                                    { "msg", "Error: " + error }
                                }
                            )
                        );
                    }
                    Console.WriteLine("Received bot command {0} with {1} parameters, target mode is {2}.", bc.Name, bc.Parameters.Count(), bc.TargetMode);

                    foreach (var plugin in Plugins)
                    {
                        bool success = false;

                        plugin.HandleCommand(bc);
                        var commands = plugin.Commands.Where(
                            c => c.Metadata.Name == bc.Name
                                && c.Metadata.TargetMode == bc.TargetMode
                                && c.Parameters.Count(p => p.ParameterInfo.GetCustomAttributes(typeof(PluginCommandParameterAttribute), false).Any()) == bc.Parameters.Count()
                                );
                        Console.WriteLine("Plugin {0} contains {1} compatible plugin commands.", plugin.Metadata.Name, commands.Count());
                        foreach (var command in commands)
                        {
                            Console.WriteLine("Trying plugin command from {0}: {1}", plugin.Metadata.Name, command.Metadata.Name);
                            List<object> parameters = new List<object>();
                            try
                            {
                                // Parse parameters
                                int i = 0;
                                foreach (var parameter in command.Parameters)
                                {
                                    // Handle internal types
                                    if (parameter.ParameterInfo.ParameterType == typeof(TS3QueryResponse))
                                    {
                                        parameters.Add(e.Response);
                                        continue;
                                    }
                                    if (parameter.ParameterInfo.ParameterType == typeof(TS3QueryBot))
                                    {
                                        parameters.Add(this);
                                        continue;
                                    }
                                    if (parameter.ParameterInfo.ParameterType == typeof(TS3Query))
                                    {
                                        parameters.Add(this.Client);
                                        continue;
                                    }
                                    if (parameter.ParameterInfo.ParameterType == typeof(TS3BotCommand))
                                    {
                                        parameters.Add(bc);
                                        continue;
                                    }

                                    // Handle user input (this can fail for "exotic" data types. lol.)
                                    Console.WriteLine("Trying to convert \"{0}\" to {1}", bc.Parameters[i], parameter.ParameterInfo.ParameterType.Name);
                                    var tc = TypeDescriptor.GetConverter(parameter.ParameterInfo.ParameterType);
                                    if (bc.Parameters.Count() == i) // i shot out of range!
                                        throw new ArgumentOutOfRangeException("Argument count mismatch");
                                    var obj = tc.ConvertFromString(bc.Parameters[i]);
                                    i++;
                                    parameters.Add(obj);
                                }

                                // Check parsed parameters count
                                if (parameters.Count() != command.MethodInfo.GetParameters().Count())
                                    throw new ArgumentOutOfRangeException("Argument count mismatch");

                                // Invoke command
                                Console.WriteLine("... ok! Invocation running.");
                                command.MethodInfo.Invoke(plugin, parameters.ToArray());

                                // Cancel further browsing
                                success = true;
                                break;
                            }
                            catch
                            {
                                Console.WriteLine("... failed! Trying next command.");
                                continue;
                            }
                        }

                        if (success)
                            break;
                    }
                }
            };

            Client.Connect(TS3Query.DefaultEndpoint);

        }
    }
}
