﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

using AM.Reporting.Utils.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Allows working with JsonObject
    /// </summary>
    public abstract class JsonBase
    {
        #region Private Fields

        private static readonly NumberFormatInfo format;

        #endregion Private Fields

        #region Public Indexers

        /// <summary>
        /// Returns child object for JsonArray
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual object this [int index]
        {
            get => null;
            set { }
        }

        /// <summary>
        /// Returns child object for JsonObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object this [string key]
        {
            get => null;
            set { }
        }

        #endregion Public Indexers

        #region Public Properties

        /// <summary>
        /// Returns count of child object
        /// </summary>
        public virtual int Count => 0;

        /// <summary>
        /// Returns true if this object is JsonArray
        /// </summary>
        public virtual bool IsArray => false;

        /// <summary>
        /// Returns true if this object is JsonObject
        /// </summary>
        public virtual bool IsObject => false;

        /// <summary>
        /// Returns list of JsonObject keys
        /// </summary>
        public virtual IEnumerable<string> Keys
        {
            get { yield break; }
        }

        #endregion Public Properties

        #region Public Constructors

        static JsonBase()
        {
            format = new NumberFormatInfo
            {
                NumberGroupSeparator = string.Empty,
                NumberDecimalSeparator = "."
            };
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Pars json text string and return a new JsonBase Object
        /// </summary>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static JsonBase FromString (string jsonText)
        {
            using (var reader = new JsonTextReader (jsonText))
            {
                return FromTextReader (reader);
            }
        }

        /// <summary>
        /// returns true
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool ContainsKey (string key)
        {
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            WriteTo (sb, 0);
            return sb.ToString();
        }

        /// <summary>
        /// Serialize this object to sb
        /// </summary>
        ///
        /// <param name="sb"></param>
        /// <param name="indent">indent in space, 0 = without indent</param>
        public abstract void WriteTo (StringBuilder sb, int indent);

        #endregion Public Methods

        #region Internal Methods

        internal string ReadString (string key)
        {
            var result = this[key];
            if (result != null)
            {
                return result.ToString();
            }

            return null;
        }

        internal void WriteValue (StringBuilder sb, object item, int indent)
        {
            if (item is JsonBase jsonBase)
            {
                if (indent > 0)
                {
                    jsonBase.WriteTo (sb, indent + 2);
                }
                else
                {
                    jsonBase.WriteTo (sb, 0);
                }
            }
            else if (item is bool)
            {
                if (item.Equals (true))
                {
                    sb.Append ("true");
                }
                else
                {
                    sb.Append ("false");
                }
            }
            else if (IsNumber (item))
            {
                sb.Append (((IConvertible)item).ToString (format));
            }
            else if (item == null)
            {
                sb.Append ("null");
            }
            else
            {
                sb.Append ('"');

                foreach (var c in item.ToString())
                {
                    switch (c)
                    {
                        case '"':
                            sb.Append ("\\\"");
                            break;
                        case '\\':
                            sb.Append ("\\\\");
                            break;
                        case '/':
                            sb.Append ("\\/");
                            break;
                        case '\b':
                            sb.Append ("\\b");
                            break;
                        case '\f':
                            sb.Append ("\\f");
                            break;
                        case '\n':
                            sb.Append ("\\n");
                            break;
                        case '\r':
                            sb.Append ("\\r");
                            break;
                        case '\t':
                            sb.Append ("\\t");
                            break;
                        default:
                            sb.Append (c);
                            break;
                    }
                }

                sb.Append ('"');
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private static JsonBase FromTextReader (JsonTextReader reader)
        {
            reader.SkipWhiteSpace();
            if (reader.IsNotEOF)
            {
                switch (reader.Char)
                {
                    case '{':
                        return ReadObject (reader);

                    case '[':
                        return ReadArray (reader);

                    default:
                        throw reader.ThrowFormat ('{', '[');
                }
            }

            throw reader.ThrowEOF ('{', '[');
        }

        private static JsonArray ReadArray (JsonTextReader reader)
        {
            if (reader.Char != '[')
            {
                throw reader.ThrowFormat ('[');
            }

            var result = new JsonArray();

            reader.ReadNext();
            reader.SkipWhiteSpace();

            if (reader.IsEOF)
            {
                throw reader.ThrowEOF (']');
            }
            else if (reader.Char != ']')
            {
                while (true)
                {
                    result.Add (ReadValue (reader));
                    reader.SkipWhiteSpace();

                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF (']');
                    }
                    else if (reader.Char == ',')
                    {
                        reader.ReadNext();
                        reader.SkipWhiteSpace();
                        continue;
                    }
                    else if (reader.Char == ']')
                    {
                        break;
                    }
                    else
                    {
                        reader.ThrowFormat (',', ']');
                    }
                }
            }

            reader.ReadNext();

            return result;
        }

        private static JsonObject ReadObject (JsonTextReader reader)
        {
            if (reader.Char != '{')
            {
                throw reader.ThrowFormat ('{');
            }

            var result = new JsonObject();

            reader.ReadNext();
            reader.SkipWhiteSpace();

            if (reader.IsEOF)
            {
                throw reader.ThrowEOF ('}');
            }
            else if (reader.Char != '}')
            {
                while (true)
                {
                    var key = reader.Dedublicate (ReadValueString (reader));

                    reader.SkipWhiteSpace();

                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF (':');
                    }
                    else if (reader.Char != ':')
                    {
                        reader.ThrowFormat (':');
                    }

                    reader.ReadNext();
                    reader.SkipWhiteSpace();

                    result[key] = ReadValue (reader);
                    reader.SkipWhiteSpace();

                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('}');
                    }
                    else if (reader.Char == ',')
                    {
                        reader.ReadNext();
                        reader.SkipWhiteSpace();
                        continue;
                    }
                    else if (reader.Char == '}')
                    {
                        break;
                    }
                    else
                    {
                        reader.ThrowFormat (',', '}');
                    }
                }
            }

            reader.ReadNext();

            return result;
        }

        private static object ReadValue (JsonTextReader reader)
        {
            if (reader.IsEOF)
            {
                throw reader.ThrowEOF ('"', '[', '{', 'n', 't', 'f', '-', '0', '1', '2', '3', '4', '5', '6', '7', '8',
                    '9');
            }

            switch (reader.Char)
            {
                case '"':
                    return ReadValueString (reader);

                case '[':
                    return ReadArray (reader);

                case '{':
                    return ReadObject (reader);

                case 'n':
                    return ReadValue (reader, "null", null);

                case 't':
                    return ReadValue (reader, "true", true);

                case 'f':
                    return ReadValue (reader, "false", false);

                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadValueNumber (reader);

                default:
                    throw reader.ThrowFormat ('"', '[', '{', 'n', 't', 'f', '-', '0', '1', '2', '3', '4', '5', '6', '7',
                        '8', '9');
            }
        }

        private static object ReadValue (JsonTextReader reader, string str, object result)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF (str[i]);
                }
                else if (reader.Char != str[i])
                {
                    throw reader.ThrowFormat (str[i]);
                }

                reader.ReadNext();
            }

            return result;
        }

        private static double ReadValueNumber (JsonTextReader reader)
        {
            var startPos = reader.Position;
            if (reader.IsEOF)
            {
                throw reader.ThrowEOF ('-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            }

            if (reader.Char == '-')
            {
                reader.ReadNext();
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                }
            }

            if (reader.Char is < '0' or > '9')
            {
                throw reader.ThrowFormat ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            }

            while (true)
            {
                reader.ReadNext();
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                }

                if (reader.Char is < '0' or > '9')
                {
                    break;
                }
            }

            if (reader.Char == '.')
            {
                reader.ReadNext();
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                }

                if (reader.Char is < '0' or > '9')
                {
                    throw reader.ThrowFormat ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                }

                while (true)
                {
                    reader.ReadNext();
                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }

                    if (reader.Char is < '0' or > '9')
                    {
                        break;
                    }
                }
            }

            if (reader.Char is 'e' or 'E')
            {
                reader.ReadNext();
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF ('+', '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                }

                var signed = false;
                if (reader.Char == '+')
                {
                    reader.ReadNext();
                    signed = true;
                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }
                }
                else if (reader.Char == '-')
                {
                    reader.ReadNext();
                    signed = true;
                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }
                }

                if (reader.Char is < '0' or > '9')
                {
                    if (signed)
                    {
                        throw reader.ThrowFormat ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }
                    else
                    {
                        throw reader.ThrowFormat ('-', '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }
                }

                while (true)
                {
                    reader.ReadNext();
                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                    }

                    if (reader.Char is < '0' or > '9')
                    {
                        break;
                    }
                }
            }

            var value = reader.Substring (startPos, reader.Position - startPos);

            return double.Parse (value, format);
        }

        private static string ReadValueString (JsonTextReader reader)
        {
            if (reader.IsEOF)
            {
                throw reader.ThrowEOF ('"');
            }
            else if (reader.Char != '"')
            {
                throw reader.ThrowFormat ('"');
            }

            reader.ReadNext();

            var sb = new StringBuilder();
            while (true)
            {
                if (reader.IsEOF)
                {
                    throw reader.ThrowEOF ('"');
                }

                if (reader.Char == '"')
                {
                    break;
                }
                else if (reader.Char == '\\')
                {
                    reader.ReadNext();
                    if (reader.IsEOF)
                    {
                        throw reader.ThrowEOF ('"', '\\', '/', 'b', 'f', 'n', 'r', 't', 'u');
                    }

                    switch (reader.Char)
                    {
                        case '"':
                            sb.Append ('"');
                            break;
                        case '\\':
                            sb.Append ('\\');
                            break;
                        case '/':
                            sb.Append ('/');
                            break;
                        case 'b':
                            sb.Append ('\b');
                            break;
                        case 'f':
                            sb.Append ('\f');
                            break;
                        case 'n':
                            sb.Append ('\n');
                            break;
                        case 'r':
                            sb.Append ('\r');
                            break;
                        case 't':
                            sb.Append ('\t');
                            break;
                        case 'u':
                            var number = 0;
                            for (var i = 0; i < 4; i++)
                            {
                                reader.ReadNext();
                                if (reader.IsEOF)
                                {
                                    throw reader.ThrowEOF ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b',
                                        'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F');
                                }

                                if (reader.Char is >= '0' and <= '9')
                                {
                                    number = number * 0x10 + (int)(reader.Char - '0');
                                }
                                else if (reader.Char is >= 'a' and <= 'f')
                                {
                                    number = number * 0x10 + 10 + (int)(reader.Char - 'a');
                                }
                                else if (reader.Char is >= 'A' and <= 'F')
                                {
                                    number = number * 0x10 + 10 + (int)(reader.Char - 'A');
                                }
                                else
                                {
                                    throw reader.ThrowFormat ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a',
                                        'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F');
                                }
                            }

                            sb.Append ((char)number);

                            break;

                        default:
                            throw reader.ThrowFormat ('"', '\\', '/', 'b', 'f', 'n', 'r', 't', 'u');
                    }
                }
                else
                {
                    sb.Append (reader.Char);
                }

                reader.ReadNext();
            }

            reader.ReadNext();

            return sb.ToString();
        }

        private static bool IsNumber (object item)
        {
            return item is float or double or sbyte or byte or short or ushort or int or uint or long or ulong or decimal;
        }

        #endregion Private Methods
    }
}
