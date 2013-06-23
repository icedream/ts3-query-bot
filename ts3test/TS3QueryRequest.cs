using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS3Query
{
    // TODO: Inheritances

    public class TS3QueryRequest
    {
        public Dictionary<string, string> Parameters { get; set; }
        public string Name { get; set; }

        public TS3QueryRequest(string name)
            : this(name, new Dictionary<string, string>())
        {
        }

        public TS3QueryRequest(string name, Dictionary<string, string> parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

        public TS3QueryRequest(string name, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            this.Name = name;
            this.Parameters = parameters.ToDictionary(i => i.Key, i => i.Value);
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

    public class TS3QueryRequestEventArgs : EventArgs
    {
        public TS3QueryRequest Request { get; private set; }

        internal TS3QueryRequestEventArgs(TS3QueryRequest e)
        {
            Request = e;
        }
    }
}
