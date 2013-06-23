using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace TS3Query.Plugins
{
    [Export(typeof(TS3QueryBotPlugin))]
    [PluginMetadata("Test plugin", "Icedream")]
    public class TestPlugin : TS3QueryBotPlugin
    {
        string CurrentSCHandlerID = string.Empty;

        [PluginCommand("test", Description = "Simply does some test output.", TargetMode = TS3MessageTargetMode.Channel)]
        public void Channel_Test(
            TS3QueryResponse response)
        {
            Console.WriteLine("[TestPlugin] Channel_Test()");
            Client.Send(
                new TS3QueryRequest(
                    "sendtextmessage",
                    new Dictionary<string, string>
                    {
                        { "targetmode", "2" },
                        { "msg", response.Parameters["invokername"] + ": This is a test message. If you read this, the test works!" }
                    }
                )
            );
        }

        [PluginCommand("test", Description = "Simply does some test output.", TargetMode = TS3MessageTargetMode.Private)]
        public void Private_Test(
            TS3QueryResponse response)
        {
            Console.WriteLine("[TestPlugin] Private_Test()");
            Client.Send(
                new TS3QueryRequest(
                    "sendtextmessage",
                    new Dictionary<string, string>
                    {
                        { "targetmode", "1" },
                        { "target", response.Parameters["invokerid"] },
                        { "msg", "This is a test message. If you read this, the test works!" }
                    }
                )
            );
        }

        [PluginCommand("test", Description = "Simply does some test output.", TargetMode = TS3MessageTargetMode.Private)]
        public void Private_Test(
            TS3QueryResponse response,
            [PluginCommandParameter("value", Description = "The value to output back to you.")] bool value)
        {
            Console.WriteLine("[TestPlugin] Private_Test(number)");
            Client.Send(
                new TS3QueryRequest(
                    "sendtextmessage",
                    new Dictionary<string, string>
                    {
                        { "targetmode", "1" },
                        { "target", response.Parameters["invokerid"] },
                        { "msg", "You sent me this bool: " + value }
                    }
                )
            );
        }

        [PluginCommand("test", Description = "Simply does some test output.", TargetMode = TS3MessageTargetMode.Private)]
        public void Private_Test(
            TS3QueryResponse response,
            [PluginCommandParameter("number", Description = "The number to output back to you.")] int number)
        {
            Console.WriteLine("[TestPlugin] Private_Test(number)");
            Client.Send(
                new TS3QueryRequest(
                    "sendtextmessage",
                    new Dictionary<string, string>
                    {
                        { "targetmode", "1" },
                        { "target", response.Parameters["invokerid"] },
                        { "msg", "You sent me this number: " + number }
                    }
                )
            );
        }

        [PluginCommand("test", Description = "Simply does some test output.", TargetMode = TS3MessageTargetMode.Private)]
        public void Private_Test(
            TS3QueryResponse response,
            [PluginCommandParameter("text", Description = "The text to output back to you.")] string text)
        {
            Console.WriteLine("[TestPlugin] Private_Test(text)");
            Client.Send(
                new TS3QueryRequest(
                    "sendtextmessage",
                    new Dictionary<string, string>
                    {
                        { "targetmode", "1" },
                        { "target", response.Parameters["invokerid"] },
                        { "msg", "You sent me this message: " + text }
                    }
                )
            );
        }
    }
}
