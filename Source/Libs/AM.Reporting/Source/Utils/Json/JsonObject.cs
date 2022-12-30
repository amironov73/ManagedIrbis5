// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json
{
    public class JsonObject : JsonBase, IEnumerable<KeyValuePair<string, object>>
    {
        #region Private Fields

        private readonly Dictionary<string, object> dict = new Dictionary<string, object>();

        #endregion Private Fields

        #region Public Indexers

        public override object this [string key]
        {
            get
            {
                if (dict.TryGetValue (key, out var result))
                {
                    return result;
                }

                return null;
            }
            set => dict[key] = value;
        }

        #endregion Public Indexers

        #region Public Properties

        public override int Count => dict.Count;

        public override bool IsObject => true;

        public override IEnumerable<string> Keys => dict.Keys;

        #endregion Public Properties

        #region Public Methods

        public override bool ContainsKey (string key)
        {
            return dict.ContainsKey (key);
        }

        public bool DeleteKey (string key)
        {
            return dict.Remove (key);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public override void WriteTo (StringBuilder sb, int indent)
        {
            sb.Append ('{');
            var notFirst = false;
            foreach (KeyValuePair<string, object> kv in dict)
            {
                if (notFirst)
                {
                    sb.Append (',');
                }

                if (indent > 0)
                {
                    sb.AppendLine();
                    for (var i = 0; i < indent; i++)
                        sb.Append (' ');
                }

                WriteValue (sb, kv.Key, indent);
                if (indent > 0)
                {
                    sb.Append (": ");
                }
                else
                {
                    sb.Append (':');
                }

                WriteValue (sb, kv.Value, indent);
                notFirst = true;
            }

            if (indent > 0 && notFirst)
            {
                sb.AppendLine();
                for (var i = 2; i < indent; i++)
                    sb.Append (' ');
            }

            sb.Append ('}');
        }

        #endregion Public Methods
    }
}
