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
    internal class JsonArray : JsonBase, IEnumerable<object>
    {
        #region Private Fields

        private readonly List<object> array = new List<object>();

        #endregion Private Fields

        #region Public Indexers

        public override object this [int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                {
                    return array[index];
                }

                return null;
            }

            set => array[index] = value;
        }

        #endregion Public Indexers

        #region Public Properties

        public override int Count => array.Count;

        public override bool IsArray => true;

        #endregion Public Properties

        #region Public Methods

        public void Add (object obj)
        {
            array.Add (obj);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public void Insert (int index, object obj)
        {
            array.Insert (index, obj);
        }

        public void Remove (int index)
        {
            array.RemoveAt (index);
        }

        public override void WriteTo (StringBuilder sb, int indent)
        {
            sb.Append ('[');

            var notFirst = false;
            foreach (var item in array)
            {
                if (notFirst)
                {
                    sb.Append (',');
                }

                if (indent > 0)
                {
                    sb.AppendLine();
                    for (var i = 0; i < indent; i++)
                    {
                        sb.Append (' ');
                    }
                }

                WriteValue (sb, item, indent);
                notFirst = true;
            }

            if (indent > 0 && notFirst)
            {
                sb.AppendLine();
                for (var i = 2; i < indent; i++)
                {
                    sb.Append (' ');
                }
            }

            sb.Append (']');
        }

        #endregion Public Methods
    }
}
