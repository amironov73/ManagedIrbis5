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

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json
{
    internal class JsonTextReader : IDisposable
    {
        #region Private Fields

        private string jsonText;
        private Dictionary<string, string> pool = new Dictionary<string, string>();
        private int position;
        private readonly bool stringOptimization;

        #endregion Private Fields

        #region Public Properties

        public char Char
        {
            get
            {
                return jsonText[position];
            }
        }

        public bool IsEOF
        {
            get
            {
                return position >= jsonText.Length;
            }
        }

        public bool IsNotEOF
        {
            get
            {
                return position < jsonText.Length;
            }
        }

        public string JsonText
        {
            get { return jsonText; }
            set { jsonText = value; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion Public Properties

        #region Public Constructors

        public JsonTextReader(string jsonText)
        {
            stringOptimization = Config.IsStringOptimization;

            this.jsonText = jsonText;
            position = 0;
        }

        #endregion Public Constructors

        #region Public Methods

        public string Dedublicate(string value)
        {
            if (stringOptimization)
            {
                string result;
                if (pool.TryGetValue(value, out result))
                    return result;
                return pool[value] = value;
            }
            return value;
        }

        public void Dispose()
        {
            JsonText = null;
            pool.Clear();
            pool = null;
        }

        public void ReadNext()
        {
            position++;
        }

        public void SkipWhiteSpace()
        {
            while (IsNotEOF && char.IsWhiteSpace(Char))
                position++;
        }

        public Exception ThrowEOF(params char[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Unexpected end of input json, wait for");
            ArgsToStringBuilder(sb, args);
            return new FormatException(sb.ToString());
        }

        public Exception ThrowEOF(string args)
        {
            return new FormatException("Unexpected end of input json, wait for " + args);
        }

        public Exception ThrowFormat(params char[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Json text at position ").Append(Position).Append(", unexpected symbol ").Append(Char).Append(", wait for");
            ArgsToStringBuilder(sb, args);
            return new FormatException(sb.ToString());
        }

        #endregion Public Methods

        #region Internal Methods

        internal string Substring(int startPos, int len)
        {
            return JsonText.Substring(startPos, len);
        }

        private static void ArgsToStringBuilder(StringBuilder sb, char[] args)
        {
            if (args.Length > 0)
            {
                sb.Append(' ').Append(args[0]);
            }
            for (int i = 1; i < args.Length; i++)
            {
                sb.Append(", ").Append(args[i]);
            }
        }

        #endregion Internal Methods
    }
}
