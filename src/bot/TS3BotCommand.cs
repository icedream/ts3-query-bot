using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS3Query
{
    public enum TS3MessageTargetMode : byte
    {
        Private = 1,
        Channel = 2,
        Server = 3
    }

    public class TS3BotCommand
    {
        public string Prefix { get; private set; }
        public string Name { get; private set; }
        public string[] Parameters { get; private set; }
        public TS3MessageTargetMode TargetMode { get; private set; }
        public TS3QueryResponse Response { get; private set; }

        internal static bool IsValidBotCommand(TS3QueryResponse response)
        {
            try
            {
                return response.Name == "notifytextmessage" && (
                    response.Parameters["msg"].StartsWith("!")
                    || response.Parameters["msg"].StartsWith(".")
                    );
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.ToString());
                return false;
            }
        }

        internal TS3BotCommand(TS3QueryResponse response)
        {
            if (!IsValidBotCommand(response))
                throw new InvalidOperationException("This response does not contain a bot command.");

            Response = response;

            TargetMode = (TS3MessageTargetMode)Convert.ToByte(response.Parameters["targetmode"]);

            Queue<string> args = new Queue<string>();
            string a = response.Parameters["msg"];
            while (a.Length > 0)
            {
                switch (a.First())
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        {
                            a = a.Substring(1);
                        } continue;
                    case '\'':
                    case '"':
                        {
                            var b = a.First(); // Our token
                            Console.WriteLine("=> {0}", a);
                            Console.WriteLine("Token is {0}", b);
                            var c = 1; // Start searching right after our token
                            do
                            {
                                c = a.IndexOf(b, c + 1); // Same token is where again?
                                Console.WriteLine("Same token is in {0}", c);
                                if (c < 0)
                                    throw new ArgumentException("Invalid syntax.");
                            } while (a.Substring(c - 1, 1) == @"\"); // "\" escapes token, therefore ignore that
                            var d = a.Substring(1, c - 1); // <token>[...content...]<token>
                            args.Enqueue(d);
                            a = a.Substring(c + 1);
                        } break;
                    default:
                        {
                            var c = 0; // Start searching at the beginning
                            do
                            {
                                c = a.IndexOfAny(new[] { ' ', '\r', '\n', '\t' }, c); // Same token is where again?
                                if (c < 0)
                                    c = a.Length; // End of string
                            } while (
                                c > 0 // prevent searching before beginning
                                && a.Substring(c - 1, 1) == @"\"); // "\ " not to be seen as same token
                            var d = a.Substring(0, c); // [...content...]<token>
                            args.Enqueue(d);
                            a = a.Substring(c);
                        } break;
                }
            }

            Name = args.Dequeue();
            Prefix = Name.First().ToString(); // TODO: Configurable prefix, also multi-length
            Name = Name.Substring(Prefix.Length); // cut out the prefix
            Parameters = args.ToArray();
        }
    }
}
