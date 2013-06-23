using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS3Query
{
    // TODO: Inheritances

    public class TS3QueryResponse
    {
        public Dictionary<string, string> Parameters { get; private set; }
        public string Name { get; private set; }

        internal TS3QueryResponse(string rawLine)
        {
            var rawLineSplit = new Queue<string>(rawLine.Split(' '));

            Name = rawLineSplit.Dequeue();
            Parameters = new Dictionary<string, string>();

            while (rawLineSplit.Any())
            {
                var rawArgSplit = rawLineSplit.Dequeue().Split('=');
                var rawArgName = TS3QueryTextFilter.DecodeText(rawArgSplit.First());
                var rawArgValue = TS3QueryTextFilter.DecodeText(string.Join("=", rawArgSplit.Skip(1)));
                Parameters.Add(rawArgName, rawArgValue);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TS3QueryTextFilter.EncodeText(Name));

            foreach (var arg in Parameters)
            {
                sb.AppendFormat(" {0}={1}", TS3QueryTextFilter.EncodeText(arg.Key), TS3QueryTextFilter.EncodeText(arg.Value));
            }

            return sb.ToString();
        }
    }

    public class TS3QueryResponseEventArgs : EventArgs
    {
        public TS3QueryResponse Response { get; private set; }

        internal TS3QueryResponseEventArgs(TS3QueryResponse e)
        {
            Response = e;
        }
    }
}
