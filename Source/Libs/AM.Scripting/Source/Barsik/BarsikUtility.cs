// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BarsikUtility.cs -- полезные методы для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Globalization;
using System.IO;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Полезные методы для Барсика.
    /// </summary>
    static class BarsikUtility
    {
        #region Public methods

        /// <summary>
        /// Вывод на печать произвольного объекта.
        /// </summary>
        public static void PrintObject
            (
                TextWriter output,
                object? value
            )
        {
            if (value is null)
            {
                output.Write ("(null)");
                return;
            }

            if (value is bool b)
            {
                output.Write (b ? "true" : "false");
                return;
            }

            if (value is string)
            {
                output.Write (value);
                return;
            }

            var type = value.GetType();
            if (type.IsPrimitive)
            {
                if (value is IFormattable formattable)
                {
                    output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
                }
                else
                {
                    output.Write (value);
                }
                return;
            }

            switch (value)
            {
                case IDictionary dictionary:
                    PrintDictionary (output, dictionary);
                    break;

                case IEnumerable sequence:
                    PrintSequence (output, sequence);
                    break;

                case IFormattable formattable:
                    output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
                    break;

                default:
                    output.Write (value);
                    break;
            }
        }

        /// <summary>
        /// Вывод на печать словаря.
        /// </summary>
        public static void PrintDictionary
            (
                TextWriter output,
                IDictionary? dictionary
            )
        {
            if (dictionary is null)
            {
                output.Write ("(null)");
                return;
            }

            output.Write ("{");

            var first = true;
            foreach (DictionaryEntry entry in dictionary)
            {
                if (!first)
                {
                    output.Write (", ");
                }

                PrintObject (output, entry.Key);
                output.Write (": ");
                PrintObject (output, entry.Value);

                first = false;
            }

            output.Write ("}");
        }

        /// <summary>
        /// Вывод на печать последовательности.
        /// </summary>
        public static void PrintSequence
            (
                TextWriter output,
                IEnumerable? sequence
            )
        {
            if (sequence is null)
            {
                output.Write ("(null)");
                return;
            }

            if (sequence is IDictionary dictionary)
            {
                PrintDictionary (output, dictionary);
                return;
            }

            if (sequence is string)
            {
                output.Write (sequence);
                return;
            }

            var first = true;
            output.Write ("[");
            foreach (var item in sequence)
            {
                if (!first)
                {
                    output.Write (", ");
                }

                PrintObject (output, item);
                first = false;
            }
            output.Write ("]");
        }

        /// <summary>
        /// Замена в исходном тексте комментариев в стиле C/C++ на пробелы.
        /// </summary>
        public static string RemoveComments
            (
                string sourceCode
            )
        {
            if (!sourceCode.Contains ("//") && !sourceCode.Contains ("/*"))
            {
                return sourceCode;
            }

            var builder = StringBuilderPool.Shared.Get();
            builder.EnsureCapacity (sourceCode.Length);

            var navigator = new ValueTextNavigator (sourceCode);
            while (!navigator.IsEOF)
            {
                var line = navigator.ReadUntil ('/');
                builder.Append (line);
                if (navigator.IsEOF)
                {
                    break;
                }

                var first = navigator.ReadChar();
                var second = navigator.ReadChar();
                if (second == '/')
                {
                    navigator.ReadLine();
                    builder.AppendLine();
                }
                else if (second == '*')
                {
                    var commented = navigator.ReadToString ("*/");
                    builder.Append ("  "); // заменяем "/*"
                    foreach (var c1 in commented)
                    {
                        var c2 = c1 switch
                        {
                            '\r' => '\r',
                            '\n' => '\n',
                            '\t' => '\t',
                            _ => ' '
                        };
                        builder.Append (c2);
                    }

                    builder.Append ("  "); // заменяем "*/"
                }
                else
                {
                    builder.Append (first);
                }
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        /// <summary>
        /// Преобразование любого значения в логическое.
        /// </summary>
        public static bool ToBoolean
            (
                object? value
            )
        {
            return value switch
            {
                null => false,
                true => true,
                false => false,
                "true" or "True" => true,
                "false" or "False" => false,
                string text => !string.IsNullOrEmpty (text),
                sbyte sb => sb != 0,
                byte b => b != 0,
                short i16 => i16 != 0,
                int i32 => i32 != 0,
                long i64 => i64 != 0,
                float f32 => f32 != 0.0f,
                double d64 => d64 != 0.0,
                decimal d => d != 0.0m,
                IList list => list.Count != 0,
                IDictionary dictionary => dictionary.Count != 0,
                _ => true
            };
        }

        public static Type? GetCommonType
            (
                IEnumerable values
            )
        {
            Type? result = null;

            foreach (var value in values)
            {
                if (value is not null)
                {
                    var type = value.GetType();
                    if (result is not null)
                    {
                        if (type != result)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        result = type;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
