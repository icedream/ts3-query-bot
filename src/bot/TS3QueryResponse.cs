using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Linq;
using System.Text;

namespace TS3Query
{
    // TODO: Inheritances

    public class TS3QueryResponse
    {
        internal DateTime Timestamp = DateTime.Now;

        public List<Dictionary<string, string>> Parameters { get; private set; }
        public string Name { get; private set; }

        internal IEnumerable<T> GenerateObject<T>()
        {
            foreach (var part in Parameters)
            {
                // Generate an instance of T
                T obj = (T)Activator.CreateInstance(typeof(T));

                // Fill it with details from this parameter part
                foreach (var item in part)
                {
                    // Check if a field for this detail exists
                    var field = obj.GetType().GetField(item.Key);
                    if (field == null) // Field does not exist
                        continue; // Skip

                    // Convert parameter string to field type
                    var tc = TypeDescriptor.GetConverter(field.FieldType);
                    try
                    {
                        var fobj = tc.ConvertFromString(item.Value);
                        field.SetValue(obj, item.Value); // Inject object into instance of T
                    }
                    catch { { } }
                }

                yield return obj;
            }
        }

        internal TS3QueryResponse(string rawLine)
        {
            var rawLineSplit = new Queue<string>(rawLine.Split(' '));

            if (!rawLineSplit.First().Contains('='))
                Name = TS3QueryTextFilter.DecodeText(rawLineSplit.Dequeue());

            rawLine = string.Join(" ", rawLineSplit);
            Parameters = rawLine.Split('|').Select(i => i.Split(' ').Select(j => { var k = j.Split('='); return new KeyValuePair<string, string>(TS3QueryTextFilter.DecodeText(k[0]), k.Length > 1 ? TS3QueryTextFilter.DecodeText(k[1]) : null); }).ToDictionary(k => k.Key, k => k.Value)).ToList();

            // Fill up parameters as the server expects us to apply values from (n-1)th instance to nth instance
            // automatically. Again, a weird way of parsing things... but okay...
            if (Parameters.Count > 1)
            {
                for (int i = 1; i < Parameters.Count; i++)
                {
                    var previousInstance = Parameters[i - 1];
                    var thisInstance = Parameters[i];

                    foreach (var item in previousInstance)
                    {
                        if (!thisInstance.ContainsKey(item.Key))
                            thisInstance.Add(item.Key, item.Value); // Apply value from previous instance
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TS3QueryTextFilter.EncodeText(Name));
            sb.Append(string.Join("|", Parameters.Select(pg => string.Format(" ", pg.Select(p => TS3QueryTextFilter.EncodeText(p.Key) + "=" + TS3QueryTextFilter.EncodeText(p.Value))))));
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
