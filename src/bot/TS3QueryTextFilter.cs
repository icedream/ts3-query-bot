using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TS3Query
{
    static class TS3QueryTextFilter
    {
        static readonly string[] RawChars = new[] {
            @"\", "/", " ", "|", "\a", "\b", "\f", "\n", "\r", "\t", "\v"
        };
        static readonly string[] ReplaceChars = new[] {
            @"\\", @"\/", @"\s", @"\p", @"\a", @"\b", @"\f", @"\n", @"\r", @"\r", @"\v"
        };

        public static string DecodeText(string data)
        {
            for (int i = 0; i < RawChars.Count(); i++)
            {
                data = data.Replace(ReplaceChars[i], RawChars[i]);
            }
            return data;
        }
        public static string EncodeText(string data)
        {
            for (int i = 0; i < RawChars.Count(); i++)
            {
                data = data.Replace(RawChars[i], ReplaceChars[i]);
            }
            return data;
        }
    }
}
