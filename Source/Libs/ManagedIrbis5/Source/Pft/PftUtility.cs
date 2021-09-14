// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PftUtility.cs -- полезные методы для работы с PFT-скриптами.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.ConsoleIO;
using AM.Linq;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Полезные методы для работы с PFT-скриптами.
    /// </summary>
    public static class PftUtility
    {
        #region Properties

        /// <summary>
        /// Digits.
        /// </summary>
        public static char[] Digits =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        /// <summary>
        /// Digits plus X.
        /// </summary>
        public static char[] DigitsX =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X'
        };


        /// <summary>
        /// Letters.
        /// </summary>
        public static char[] Letters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z',

            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к',
            'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц',
            'ч', 'ш', 'щ', 'ь', 'ы', 'ъ', 'э', 'ю', 'я',

            'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К',
            'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц',
            'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ', 'Э', 'Ю', 'Я',
        };

        #endregion

        #region Private members

        private static ReadOnlyMemory<char> _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            var result = new StringBuilder();

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString().AsMemory();
        }

        private static Field _ParseLine
            (
                string line
            )
        {

            var reader = new StringReader(line);
            var result = new Field
            {
                Value = _ReadTo(reader, '^').EmptyToNull()
            };

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                var code = char.ToLower((char)next);
                var text = _ReadTo(reader, '^').EmptyToNull();
                var subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                result.Subfields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        //=================================================

        /// <summary>
        /// Assign field.
        /// </summary>
        public static void AssignField
            (
                PftContext context,
                int tag,
                IndexSpecification index,
                string? value
            )
        {
            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Magna.Error
                    (
                        "PftUtility::AssignField: "
                        + "record not set"
                    );

                return;
            }

            var fields = record.Fields.GetField(tag);

            if (string.IsNullOrEmpty(value))
            {
                if (index.Kind == IndexKind.None
                    || index.Kind == IndexKind.AllRepeats)
                {
                    record.RemoveField(tag);
                }
                else
                {
                    var i = index.ComputeValue(context, fields);

                    if (i >= 0 && i < fields.Length)
                    {
                        record.Fields.Remove(fields[i]);
                    }
                }

                return;
            }

            var lines = value.SplitLines().NonEmptyLines().ToArray();
            var newFields = new List<Field>();
            foreach (var line in lines)
            {
                var field = ParseField(line);
                field.Tag = tag;
                newFields.Add(field);
            }

            if (index.Kind == IndexKind.None)
            {
                foreach (var field in fields)
                {
                    record.Fields.Remove(field);
                }

                record.Fields.AddRange(newFields);
            }
            else
            {
                var i = index.ComputeValue(context, fields);

                if (newFields.Count == 0)
                {
                    if (i >= 0 && i < fields.Length)
                    {
                        record.Fields.Remove(fields[i]);
                    }
                }
                else
                {
                    if (i >= fields.Length)
                    {
                        record.Fields.AddRange(newFields);
                    }
                    else
                    {
                        var position = record.Fields.IndexOf(fields[i]);
                        fields[i].AssignFrom(newFields[0]);

                        for (var j = 1; j < newFields.Count; j++)
                        {
                            record.Fields.Insert
                                (
                                    position + j,
                                    newFields[j]
                                );
                        }
                    }
                }
            }
        } // method AssignField

        //=================================================

        /// <summary>
        /// Assign subfield.
        /// </summary>
        public static void AssignSubField
            (
                PftContext context,
                int tag,
                IndexSpecification fieldIndex,
                char code,
                IndexSpecification subfieldIndex,
                string? value
            )
        {
            code = SubFieldCode.Normalize(code);

            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Magna.Error
                    (
                        "PftUtility::AssignSubField: "
                        + "record not set"
                    );

                return;
            }

            var fields = record.Fields.GetField(tag);

            if (ReferenceEquals(value, null))
            {
                // TODO implement properly

                return;
            }

            if (fieldIndex.Kind != IndexKind.None)
            {
                var i = fieldIndex.ComputeValue(context, fields);

                var field = fields.GetOccurrence(i);
                if (ReferenceEquals(field, null))
                {
                    return;
                }

                fields = new[] { field };
            }

            var lines = value.SplitLines()
                .NonEmptyLines()
                .ToArray();
            var newSubFields = new List<SubField>();
            foreach (var line in lines)
            {
                var subField = new SubField
                {
                    Code = code,
                    Value = line
                };
                newSubFields.Add(subField);
            }

            var current = 0;
            foreach (var field in fields)
            {
                var subfields = field.GetSubFields(code);

                if (subfieldIndex.Kind == IndexKind.None)
                {
                    foreach (var subField in subfields)
                    {
                        field.Subfields.Remove(subField);
                    }

                    if (current < newSubFields.Count)
                    {
                        var newSubField = newSubFields[current];
                        if (!ReferenceEquals(newSubField, null))
                        {
                            field.Subfields.Add(newSubField);
                        }
                    }
                    current++;
                }
                else
                {
                    var i = subfieldIndex.ComputeValue(context, subfields);

                    if (i >= subfields.Length)
                    {
                        field.Subfields.AddRange(newSubFields);
                    }
                    else
                    {
                        var position = field.Subfields.IndexOf(subfields[i]);
                        field.Subfields[i].Value = newSubFields[0].Value;

                        for (var j = 1; j < newSubFields.Count; j++)
                        {
                            field.Subfields.Insert
                                (
                                    position + j,
                                    newSubFields[j]
                                );
                        }
                    }
                }
            }

        } // method AssignSubField

        //=================================================

        /// <summary>
        /// Clone nodes.
        /// </summary>
        public static PftNodeCollection? CloneNodes
            (
                this PftNodeCollection? nodes,
                PftNode? parent
            )
        {
            PftNodeCollection? result = null;

            if (ReferenceEquals(nodes, null))
            {
                Magna.Error
                    (
                        "PftUtility::CloneNodes: "
                        + "nodes are null"
                    );
            }
            else
            {
                result = new PftNodeCollection(parent);

                foreach (var child1 in nodes)
                {
                    var child2 = (PftNode)child1.Clone();
                    result.Add(child2);
                }
            }

            return result;

        } // method CloneNodes

        //=================================================

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static int CompareStrings
            (
                string? first,
                string? second
            )
        {
            var result = string.Compare
                (
                    first,
                    second,
                    StringComparison.CurrentCultureIgnoreCase
                );

            return result;

        } // method CompareStrings

        //=================================================

        /// <summary>
        /// Compile the program.
        /// </summary>
        public static PftProgram CompileProgram
            (
                string source
            )
        {
            var result = ProgramCache.GetProgram(source);
            if (ReferenceEquals(result, null))
            {
                var lexer = new PftLexer();
                var tokens = lexer.Tokenize(source);
                var parser = new PftParser(tokens);
                result = parser.Parse();
                ProgramCache.AddProgram(source, result);
            }

            return result;

        } // method CompileProgram

        //=================================================

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubString
            (
                string? outer,
                string? inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                // Original formatter have the bug:
                //
                // if '':'' then '1' else '2' fi,
                // if 'ABC':'A' then '1' else '2' fi,
                // if '':'A' then '1' else '2' fi,
                // if 'A': '' then '1' else '2' fi
                //
                // produces 1122
                //
                // Thus not-empty string DOESNT contains empty one!
                // Bug discovered by Ivan Batrak

                return string.IsNullOrEmpty(outer);
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            outer = outer.ToLower();
            inner = inner.ToLower();

            var result = outer.Contains(inner);

            return result;

        } // method ContainsSubString

        //=================================================

        /// <summary>
        /// Whether one string contains another.
        /// </summary>
        public static bool ContainsSubStringSensitive
            (
                string? outer,
                string? inner
            )
        {
            if (string.IsNullOrEmpty(inner))
            {
                // Original formatter have the bug:
                //
                // if '':'' then '1' else '2' fi,
                // if 'ABC':'A' then '1' else '2' fi,
                // if '':'A' then '1' else '2' fi,
                // if 'A': '' then '1' else '2' fi
                //
                // produces 1122
                //
                // Thus not-empty string DOESNT contains empty one!
                // Bug discovered by Ivan Batrak

                return string.IsNullOrEmpty(outer);
            }
            if (string.IsNullOrEmpty(outer))
            {
                return false;
            }

            var result = outer.Contains(inner);

            return result;
        }

        //=================================================

        /// <summary>
        /// Extract numeric value from the input text.
        /// </summary>
        public static double ExtractNumericValue
            (
                string? input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0.0;
            }

            var match = Regex.Match
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            if (!match.Success)
            {
                return 0.0;
            }

            var value = match.Value;
            double.TryParse
                (
                    value,
                    NumberStyles.AllowDecimalPoint
                    | NumberStyles.AllowLeadingSign
                    | NumberStyles.AllowExponent
                    | NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Extract numeric values from the input text.
        /// </summary>
        public static double[] ExtractNumericValues
            (
                string? input
            )
        {
            if (string.IsNullOrEmpty(input))
            {
                return new double[0];
            }

            var result = new List<double>();
            var matches = Regex.Matches
                (
                    input,
                    "[-]?[0-9]*[\\.]?[0-9]*"
                );
            foreach (Match match in matches)
            {
                if (double.TryParse
                    (
                        match.Value,
                        NumberStyles.AllowDecimalPoint
                        | NumberStyles.AllowLeadingSign
                        | NumberStyles.AllowExponent
                        | NumberStyles.Float,
                        CultureInfo.InvariantCulture,
                        out var value
                    ))
                {
                    result.Add(value);
                }
            }

            return result.ToArray();
        }

        //=================================================

        /// <summary>
        /// Извлекает все слова на латинице и кириллице.
        /// </summary>
        public static string[] ExtractWords
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<string>();
            }

            var result = new List<string>();
            var navigator = new TextNavigator(text);
            var builder = new StringBuilder();
            char c;
            while ((c = navigator.ReadChar()) != '\0')
            {
                if (c >= 0x0041 && c < 0x005B
                    || c >= 0x0061 && c < 0x007B
                    || c >= 0x0400 && c < 0x0460)
                {
                    builder.Append(c);
                }
                else
                {
                    if (builder.Length != 0)
                    {
                        result.Add(builder.ToString());
                        builder.Clear();
                    }
                }
            }

            if (builder.Length != 0)
            {
                result.Add(builder.ToString());
            }

            return result.ToArray();
        }

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="FieldSpecification"/>'s.
        /// </summary>
        public static void FieldsToText
            (
                StringBuilder builder,
                IEnumerable<FieldSpecification> fields
            )
        {
            var first = true;
            foreach (var field in fields.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(", ");
                }
                builder.Append(field);
                first = false;
            }
        }

        //=================================================

        /// <summary>
        /// Format for data mode.
        /// </summary>
        public static string FormatDataMode
            (
                this Field field
            )
        {
            var result = FormatHeaderMode(field);

            if (!result.EndsWith(".")
                & !result.EndsWith(". ")
                & !result.EndsWith(".  "))
            {
                result = result + ".";
            }

            if (result.EndsWith("  "))
            {
                // nothing to do
            }
            else if (result.EndsWith(" "))
            {
                result = result + " ";
            }
            else
            {
                result = result + "  ";
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Format for header mode.
        /// </summary>
        public static string FormatHeaderMode
            (
                this Field field
            )
        {
            var result = new StringBuilder();

            result.Append(field.Value);
            foreach (var subField in field.Subfields)
            {
                var delimiter = ". ";
                var code = char.ToLower(subField.Code);
                switch (code)
                {
                    case 'a':
                        delimiter = "; ";
                        break;

                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                        delimiter = ", ";
                        break;
                }
                if (result.Length != 0)
                {
                    result.Append(delimiter);
                }

                var value = subField.Value;
                if (!value.IsEmpty())
                {
                    value = value
                        .Replace("><", "; ")
                        .Replace("<", string.Empty)
                        .Replace(">", string.Empty);
                }

                result.Append(value);
            }

            return result.ToString();
        }

        //=================================================

        /// <summary>
        /// Format field according to specified output mode.
        /// </summary>
        public static string FormatField
            (
                this Field field,
                PftFieldOutputMode mode,
                bool uppercase
            )
        {
            string result;

            switch (mode)
            {
                case PftFieldOutputMode.DataMode:
                    result = FormatDataMode(field);
                    break;

                case PftFieldOutputMode.HeaderMode:
                    result = FormatHeaderMode(field);
                    break;

                case PftFieldOutputMode.PreviewMode:
                    result = field.ToText();
                    break;

                default:
                    Magna.Error
                        (
                            nameof(PftUtility) + "::" + nameof(FormatField)
                            + ": unexpected data mode="
                            + mode
                        );

                    throw new ArgumentOutOfRangeException();
            }

            if (uppercase)
            {
                result = IrbisText.ToUpper(result)
                    .ThrowIfNull("IrbisText.ToUpper");
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Format value like function f() does.
        /// </summary>
        public static string FormatLikeF
            (
                double value,
                int arg2,
                int arg3
            )
        {
            var minLength = 1;
            if (arg2 < 0)
            {
                if (arg3 < 0)
                {
                    minLength = 16;
                }
            }
            else
            {
                minLength = arg2;
            }

            var useE = true;
            var decimalPoints = 0;
            if (arg3 >= 0)
            {
                useE = false;
                decimalPoints = arg3;
            }

            // ibatrak
            // IRBIS uses banker's rounding

            // f(0.5,0,0) = 0
            // f(1.5,0,0) = 2
            // f(2.5,0,0) = 2
            // f(3.5,0,0) = 4
            // f(4.5,0,0) = 4
            // f(5.5,0,0) = 6
            // f(6.5,0,0) = 6
            // f(7.5,0,0) = 8
            // f(8.5,0,0) = 8
            // f(9.5,0,0) = 10
            // f(0.05,0,1) = 0.1
            // f(0.15,0,1) = 0.1
            // f(0.25,0,1) = 0.3
            // f(0.35,0,1) = 0.3
            // f(0.45,0,1) = 0.5
            // f(0.55,0,1) = 0.6
            // f(0.65,0,1) = 0.7
            // f(0.75,0,1) = 0.8
            // f(0.85,0,1) = 0.8
            // f(0.95,0,1) = 0.9
            // f(0.005,0,2) = 0.01
            // f(0.015,0,2) = 0.02
            // f(0.025,0,2) = 0.03
            // f(0.035,0,2) = 0.04
            // f(0.045,0,2) = 0.05
            // f(0.055,0,2) = 0.06
            // f(0.065,0,2) = 0.07
            // f(0.075,0,2) = 0.08
            // f(0.085,0,2) = 0.09
            // f(0.095,0,2) = 0.10

            switch (decimalPoints)
            {
                case 0:
                    value = Math.Round
                        (
                            value,
                            decimalPoints,
                            MidpointRounding.ToEven
                        );
                    break;

                //case 1:
                //    // ReSharper disable CompareOfFloatsByEqualityOperator

                //    if (value == 0.05)
                //    {
                //        value = 0.1;
                //    }
                //    else if (value == 0.15)
                //    {
                //        value = 0.1;
                //    }
                //    else if (value == 0.25)
                //    {
                //        value = 0.3;
                //    }
                //    else if (value == 0.35)
                //    {
                //        value = 0.3;
                //    }
                //    else if (value == 0.45)
                //    {
                //        value = 0.5;
                //    }
                //    else if (value == 0.55)
                //    {
                //        value = 0.6;
                //    }
                //    else if (value == 0.65)
                //    {
                //        value = 0.7;
                //    }
                //    else if (value == 0.75)
                //    {
                //        value = 0.8;
                //    }
                //    else if (value == 0.85)
                //    {
                //        value = 0.8;
                //    }
                //    else if (value == 0.95)
                //    {
                //        value = 0.9;
                //    }
                //    break;

                //// ReSharper restore CompareOfFloatsByEqualityOperator
            }

            var format = useE
                ? string.Format("E{0}", minLength)
                : string.Format("F{0}", decimalPoints);

            var result = value.ToString
                (
                    format,
                    CultureInfo.InvariantCulture
                )
                .PadLeft
                (
                    minLength,
                    ' '
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Format term link for "*" method.
        /// </summary>
        public static bool FormatTermLink
            (
                PftContext context,
                PftNode? node,
                string? database,
                TermLink link
            )
        {
            var provider = context.Provider;
            var saveDatabase = provider.Database;
            try
            {
                if (!string.IsNullOrEmpty(database))
                {
                    provider.Database = database;
                }

                var record = provider.ReadRecord(link.Mfn);
                if (!ReferenceEquals(record, null))
                {
                    var field = record.Fields.GetField
                        (
                            link.Tag,
                            link.Occurrence - 1
                        );
                    if (!ReferenceEquals(field, null))
                    {
                        var output = FormatField
                            (
                                field,
                                context.FieldOutputMode,
                                context.UpperMode
                            );
                        if (!string.IsNullOrEmpty(output))
                        {
                            context.WriteAndSetFlag(node, output);

                            return true;
                        }
                    }
                }
            }
            finally
            {
                provider.Database = saveDatabase;
            }

            return false;
        }

        //=================================================

        /// <summary>
        /// Get array item according to specification
        /// </summary>
        public static T[] GetArrayItem<T>
            (
                PftContext context,
                T[] array,
                IndexSpecification index
            )
        {
            if (index.Kind == IndexKind.None)
            {
                return array;
            }

            var i = index.ComputeValue(context, array);

            if (i >= 0 && i < array.Length)
            {
                return new[] { array[i] };
            }

            return new T[0];
        }

        //=================================================

        /// <summary>
        /// Get count of the fields.
        /// </summary>
        public static int GetFieldCount
            (
                PftContext context,
                params int[] tags
            )
        {
            var result = 0;
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                foreach (var tag in tags)
                {
                    var count = record.Fields.GetFieldCount(tag);
                    result = Math.Max(count, result);
                }
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Get value of the field.
        /// </summary>
        public static string[] GetFieldValue
            (
                PftContext context,
                int tag,
                IndexSpecification index
            )
        {
            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return Array.Empty<string>();
            }

            var fields = record.Fields.GetField(tag);
            var result = fields.Select
                (
                    field => field.ToText()
                )
                .ToArray();

            result = GetArrayItem
                (
                    context,
                    result,
                    index
                );

            return result;
        }

        //=================================================

        /// <summary>
        /// Get value of the field.
        /// </summary>
        public static string? GetFieldValue
            (
                PftContext context,
                Field field,
                char subFieldCode,
                IndexSpecification subFieldRepeat
            )
        {
            string? result = null;

            if (subFieldCode == SubField.NoCode)
            {
                result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
            }
            else if (subFieldCode == '*')
            {
                result = field.GetValueOrFirstSubField();
            }
            else
            {
                var subFields = field.GetSubFields(subFieldCode);
                subFields = GetArrayItem
                    (
                        context,
                        subFields,
                        subFieldRepeat
                    );
                var subField = subFields.FirstOrDefault();
                if (!ReferenceEquals(subField, null))
                {
                    result = subField.Value;
                }
            }

            return result;
        }

        //=================================================

        private static readonly string[] _reservedWords =
        {
            "a",
            "abs",
            "absent",
            "all",
            "any",
            "and",
            "blank",
            "break",
            "ceil",
            "cseval",
            "div",
            "do",
            "else",
            "empty",
            "end",
            "eval",
            "f",
            "fmt",
            "false",
            "fi",
            "first",
            "floor",
            "for",
            "frac",
            "global",
            "have",
            "if",
            "l",
            "last",
            "local",
            "mdl",
            "mdu",
            "mfn",
            "mhl",
            "mhu",
            "mpl",
            "mpu",
            "not",
            "or",
            "p",
            "parallel",
            "pow",
            "present",
            "proc",
            "ravr",
            "ref",
            "rmax",
            "rmin",
            "round",
            "rsum",
            "s",
            "sign",
            "then",
            "true",
            "trunc",
            "uf",
            "unifor",
            "val",
            "while",
            "with",
            "если",
            "иначе",
            "то"
        };

        /// <summary>
        /// Get array of reserved words.
        /// </summary>
        public static string[] GetReservedWords() => _reservedWords;

        //=================================================

        /// <summary>
        /// Get value of the subfield.
        /// </summary>
        public static string[] GetSubFieldValue
            (
                PftContext context,
                int tag,
                IndexSpecification fieldIndex,
                char code,
                IndexSpecification subfieldIndex
            )
        {
            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                Magna.Error
                    (
                        "PftUtility::GetSubFieldValue: "
                        + "record not set"
                    );

                return Array.Empty<string>();
            }

            code = SubFieldCode.Normalize(code);

            var fields = record.Fields.GetField(tag);
            fields = GetArrayItem
                (
                    context,
                    fields,
                    fieldIndex
                );

            var result = fields.Select
                (
                    subField => subField.GetFirstSubFieldValue(code) ?? string.Empty
                )
                .ToArray();

            result = GetArrayItem
                (
                    context,
                    result,
                    subfieldIndex
                );

            return result;

        } // method GetSubFieldValue

        //=================================================

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public static bool IsComplexExpression
            (
                PftNode node
            )
        {
            if (node.ComplexExpression)
            {
                return true;
            }

            var children
                = node.GetDescendants<PftNode>();
            var result = children.Any(item => item.ComplexExpression);

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public static bool IsComplexExpression
            (
                IEnumerable<PftNode> nodes
            )
        {
            var result = nodes.Any(item => IsComplexExpression(item));

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node collection represents
        /// numeric or string expression.
        /// </summary>
        public static bool IsNumeric
            (
                PftContext context,
                IList<PftNode> nodes
            )
        {
            if (nodes.Count == 0
                || nodes.Count > 1)
            {
                return true;
            }

            return IsNumeric
                (
                    context,
                    nodes[0]
                );
        }

        //=================================================

        /// <summary>
        /// Heuristics: whether given node is
        /// text or numeric.
        /// </summary>
        public static bool IsNumeric
            (
                PftContext context,
                PftNode node
            )
        {
            var reference = node as PftVariableReference;
            if (!ReferenceEquals(reference, null)
                && !ReferenceEquals(reference.Name, null))
            {
                var variable
                    = context.Variables.GetExistingVariable(reference.Name);
                if (!ReferenceEquals(variable, null))
                {
                    return variable.IsNumeric;
                }

                // TODO: some heuristic?
                return false;
            }

            return node is PftNumeric;
        }

        //=================================================

        /// <summary>
        /// Получает текстовое представление нескольких <see cref="PftNode"/>.
        /// </summary>
        public static void NodesToText
            (
                StringBuilder builder,
                IEnumerable<PftNode> nodes
            )
        {
            var first = true;
            foreach (var node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node);
                first = false;
            }

        } // method NodesToText

        /// <summary>
        /// Получает текстовое представление нескольких <see cref="PftNode"/>.
        /// </summary>
        public static void NodesToText
            (
                ref ValueStringBuilder builder,
                IEnumerable<PftNode> nodes
            )
        {
            var first = true;
            foreach (var node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node.ToString());
                first = false;
            }

        } // method NodesToText

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="PftNode"/>'s.
        /// </summary>
        public static void NodesToText
            (
                string delimiter,
                StringBuilder builder,
                IEnumerable<PftNode> nodes
            )
        {
            var first = true;
            foreach (var node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(delimiter);
                }
                builder.Append(node);
                first = false;
            }
        }

        //=================================================

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static Field ParseField
            (
                string line
            )
        {
            return _ParseLine(line);
        }

        //=================================================

        /// <summary>
        /// Prepare text for <see cref="PftUnconditionalLiteral"/>,
        /// <see cref="PftConditionalLiteral"/>,
        /// <see cref="PftRepeatableLiteral"/>.
        /// </summary>
        public static string? PrepareText
            (
                string? text
            )
        {
            var result = text;

            if (!string.IsNullOrEmpty(text))
            {
                result = text
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public static bool RequiresConnection
            (
                PftNode node
            )
        {
            if (node.RequiresConnection)
            {
                return true;
            }

            var children
                = node.GetDescendants<PftNode>();
            var result = children.Any(item => item.RequiresConnection);

            return result;

        } // method RequiresConnection

        //=================================================

        /// <summary>
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public static bool RequiresConnection
            (
                IEnumerable<PftNode> nodes
            )
        {
            var result = nodes.Any(RequiresConnection);

            return result;

        } // method RequiresConnection

        //=================================================

        /// <summary>
        /// Extract substring in safe manner.
        /// </summary>
        internal static string? SafeSubString
            (
                string? text,
                int offset,
                int length
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (offset < 0)
            {
                offset = 0;
            }
            if (length <= 0)
            {
                return string.Empty;
            }
            if (offset >= text.Length)
            {
                return string.Empty;
            }

            try
            {
                checked
                {
                    if (offset + length > text.Length)
                    {
                        length = text.Length - offset;
                        if (length <= 0)
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftUtility::SafeSubString",
                        exception
                    );

                Debug.WriteLine(exception);

                throw;
            }

            string result;

            try
            {
                result = text.Substring
                    (
                        offset,
                        length
                    );
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftUtility::SafeSubString",
                        exception
                    );

                Debug.WriteLine(exception);

                ConsoleInput.WriteLine(exception.ToString());

                throw;
            }

            return result;

        } // method SafeSubString

        //=================================================

        /// <summary>
        /// Set array item according to index specification
        /// </summary>
        public static T?[] SetArrayItem<T>
            (
                PftContext context,
                T?[] array,
                IndexSpecification index,
                T? value
            )
        {
            if (index.Kind == IndexKind.None)
            {
                array = new[] { value };
            }
            else if (index.Kind == IndexKind.AllRepeats)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = value;
                }
            }
            else
            {
                var i = index.ComputeValue(context, array);

                if (i >= 0)
                {
                    if (i >= array.Length)
                    {
                        Array.Resize(ref array, i + 1);
                    }
                    array[i] = value;
                }
            }

            return array;

        } // method SetArrayItem

        /// <summary>
        /// Преобразование одной строки в текстовый литерал PFT.
        /// </summary>
        public static void TextToPft
            (
                ReadOnlySpan<char> line,
                TextWriter output
            )
        {
            const char QUOTATION_MARK = '\'';

            if (line.IsEmpty)
            {
                return;
            }

            // флаг: выводим непосредственно символы
            var inQuotation = false; // а не unifor

            foreach (var chr in line)
            {
                if (chr >= ' ' && chr != QUOTATION_MARK)
                {
                    if (!inQuotation)
                    {
                        output.Write(QUOTATION_MARK);
                        inQuotation = true;
                    }

                    output.Write (chr);
                }
                else
                {
                    // встретился символ, требующий специального представления в PFT

                    if (inQuotation)
                    {
                        // при необходимости закрываем литерал

                        output.Write(QUOTATION_MARK);
                        output.Write(',');
                        inQuotation = false;
                    }

                    output.Write ("&uf('+9F',");
                    output.Write (((int) chr).ToInvariantString());
                    output.Write (')');
                    output.Write (',');

                } // else

            } // foreach

            if (inQuotation)
            {
                output.Write (QUOTATION_MARK); // закрывающая кавычка
            }

        } // method TextToPft

        /// <summary>
        /// Преобразование текста в PFT-совместимое представление.
        /// </summary>
        public static void TextToPft
            (
                TextReader input,
                TextWriter output
            )
        {
            string? line;
            bool first = true;
            while ((line = input.ReadLine()) != null)
            {
                if (!first)
                {
                    output.WriteLine(",/");
                }

                TextToPft (line.AsSpan(), output);
                first = false;

            } // while

        } // method TextToPft

        //=================================================

        #endregion

    } // class PftUtility

} // namespace ManagedIrbis.Pft
