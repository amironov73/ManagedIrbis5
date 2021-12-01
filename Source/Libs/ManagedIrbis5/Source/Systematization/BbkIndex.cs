// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkIndex.cs -- классификационный индекс ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Классификационный индекс ББК,
    /// разложенный по элементам.
    /// </summary>
    [XmlRoot ("bbk")]
    public sealed class BbkIndex
    {
        #region Properties

        /// <summary>
        /// Основной индекс.
        /// </summary>
        [XmlAttribute ("main-index")]
        [JsonPropertyName ("main-index")]
        [Description ("Основной индекс")]
        public string? MainIndex { get; set; }

        /// <summary>
        /// Территориальное типовое деление.
        /// </summary>
        [XmlAttribute ("territorial-index")]
        [JsonPropertyName ("territorial-index")]
        [Description ("Территориальное типовое деление")]
        public string? TerritorialIndex { get; set; }

        /// <summary>
        /// Специальные типовые деления.
        /// </summary>
        [XmlAttribute ("special-index")]
        [JsonPropertyName ("special-index")]
        [Description ("Специальные типовые деления")]
        public NonNullCollection<string> SpecialIndex { get; } = new ();

        /// <summary>
        /// Код социальной системы.
        /// </summary>
        [XmlAttribute ("social-index")]
        [JsonPropertyName ("social-index")]
        [Description ("Код социальной системы")]
        public string? SocialIndex { get; set; }

        /// <summary>
        /// Комбинированный индекс.
        /// </summary>
        [XmlAttribute ("combined-index")]
        [JsonPropertyName ("combined-index")]
        [Description ("Комбинированный индекс")]
        public string? CombinedIndex { get; set; }

        /// <summary>
        /// Определители.
        /// </summary>
        [XmlElement ("qualifiers")]
        [JsonPropertyName ("qualifiers")]
        [Description ("Определители")]
        public NonNullCollection<string> Qualifiers { get; } = new ();

        /// <summary>
        /// Некая хрень.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public string? Hren { get; set; }

        /// <summary>
        /// Запятая???
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public string? Comma { get; set; }

        #endregion

        #region Private members

        private static readonly char[] _allowedSymbols =
        {
            '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '.', '/'
        };

        private static readonly char[] _qualifierSymbols =
        {
            'в', 'г', 'д', 'е', 'ж', 'и', 'к', 'л',
            'м', 'н', 'п', 'р', 'с', 'т', 'у', 'ф',
            'ц', 'ю', 'я'
        };

        private static int _ParseSimple
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            for (; offset < length; offset++)
            {
                var c = text[offset];
                if (Array.IndexOf (_allowedSymbols, c) < 0)
                {
                    break;
                }

                result.Append (c);
            }

            return offset;
        }

        private static int _ParseCombined
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == ':')
                {
                    result.Append (':');
                    offset = _ParseSimple (text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_ParseCombined)
                                + ": неверный комбинированный индекс: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Неверный комбинированный индекс"
                            );
                    }
                }
            }

            return offset;
        }

        private static int _ParseTerritorial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '(')
                {
                    result.Append ('(');
                    offset++;
                    while (offset < length)
                    {
                        var c = text[offset];
                        offset++;
                        if (c == ')')
                        {
                            result.Append (')');
                            break;
                        }

                        result.Append (c);
                    }

                    if (result[^1] != ')')
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_ParseTerritorial)
                                + ": незакрытая скобка в территориальном делении: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Незакрытая скобка в территориальном делении"
                            );
                    }
                }
            }

            return offset;
        }

        static int _ParseQualifier
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                var c = text[offset];
                if (Array.IndexOf (_qualifierSymbols, c) >= 0)
                {
                    result.Append (c);
                    if (c == 'д')
                    {
                        offset++;
                        while (offset < length)
                        {
                            result.Append (text[offset]);
                            offset++;
                        }
                    }
                    else
                    {
                        offset = _ParseSimple (text, ++offset, length, result);
                    }
                }
            }

            return offset;
        }

        static int _ParseSpecial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '-')
                {
                    result.Append ('-');
                    offset = _ParseSimple (text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_ParseSpecial)
                                + ": неверный код специального типового деления: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Неверный код специального типового деления"
                            );
                    }
                }
            }

            return offset;
        }

        static int _ParseHren
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (char.IsDigit (text, offset))
                {
                    offset = _ParseSimple (text, offset, length, result);
                }
            }

            return offset;
        }

        static int _ParseComma
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == ',')
                {
                    result.Append (text[offset]);
                    offset = _ParseSimple (text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_ParseComma)
                                + ": неверно сформированный индекс: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Неверно сформированный индекс"
                            );
                    }
                }
            }

            return offset;
        }

        static int _ParseSocial
            (
                string text,
                int offset,
                int length,
                StringBuilder result
            )
        {
            if (offset < length)
            {
                if (text[offset] == '\'')
                {
                    result.Append ('\'');
                    offset = _ParseSimple (text, ++offset, length, result);

                    if (result.Length == 1)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_ParseSocial)
                                + ": неверный код социальной системы: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Неверный код социальной системы"
                            );
                    }
                }
            }

            return offset;
        }

        private static string? _EmptyToNull
            (
                StringBuilder builder
            )
        {
            return builder.Length == 0
                ? null
                : builder.ToString();
        }

        private static string? _Verify
            (
                string? text,
                int skip
            )
        {
            if (string.IsNullOrEmpty (text))
            {
                return text;
            }

            var copy = text;
            var c = copy[0];
            if (c == '(')
            {
                return text;
            }

            if (skip != 0)
            {
                copy = copy.Substring (skip);
            }

            if (copy[^1] == '.')
            {
                Magna.Error
                    (
                        nameof (BbkIndex) + "::" + nameof (_Verify)
                        + ": индекс оканчивается точкой: "
                        + text.ToVisibleString()
                    );

                throw new BbkException
                    (
                        "Индекс заканчивается точкой"
                    );
            }

            var length = copy.Length;
            if (length > 2)
            {
                if (copy[2] != '.')
                {
                    Magna.Error
                        (
                            nameof (BbkIndex) + "::" + nameof (_Verify)
                            + ": индекс должен начинаться с двузначной группы: "
                            + text.ToVisibleString()
                        );

                    throw new BbkException
                        (
                            "Индекс должен начинаться с двузначной группы"
                        );
                }
            }

            var offset = 3;
            var count = 0;

            while (offset < length)
            {
                if (copy[offset] == '.'
                    || copy[offset] == '/')
                {
                    if (count == 0)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (_Verify)
                                + ": два разделителя подряд: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Два разделителя подряд"
                            );
                    }

                    // expression count > 3 is always false
                    //if (count > 3)
                    //{
                    //    throw new BbkException
                    //        (
                    //            "Слишком длинная группа"
                    //        );
                    //}
                    count = 0;
                }
                else
                {
                    count++;
                    if (count > 3)
                    {
                        Magna.Error
                            (
                                "BbkIndex::_Verify: "
                                + "слишком длинная группа: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Слишком длинная группа"
                            );
                    }
                }

                offset++;
            }

            return text;
        }

        private static void _Dump
            (
                TextWriter writer,
                string name,
                string? value,
                string prefix
            )
        {
            if (!string.IsNullOrEmpty (value))
            {
                writer.WriteLine
                    (
                        "{0}{1}: {2}",
                        prefix,
                        name,
                        value
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстовой строки.
        /// </summary>
        public static BbkIndex Parse
            (
                string text
            )
        {
            var result = new BbkIndex();

            if (string.IsNullOrEmpty (text))
            {
                Magna.Error
                    (
                        nameof (BbkIndex) + "::" + nameof (Parse)
                        + ": empty index"
                    );

                throw new BbkException ("Пустой индекс ББК");
            }

            var length = text.Length;
            if (length < 2)
            {
                Magna.Error
                    (
                        nameof (BbkIndex) + "::" + nameof (Parse)
                        + ": less than two symbols: "
                        + text.ToVisibleString()
                    );

                throw new BbkException
                    (
                        "ББК не может содержать меньше двух символов"
                    );
            }

            if (!char.IsDigit (text, 0) || !char.IsDigit (text, 1))
            {
                Magna.Error
                    (
                        nameof (BbkIndex) + "::" + nameof (Parse)
                        + ": two first symbols must be digits: "
                        + text.ToVisibleString()
                    );

                throw new BbkException
                    (
                        "Первые два символа ББК должны быть цифрами"
                    );
            }

            var offset = 2;
            var accumulator = StringBuilderPool.Shared.Get();
            accumulator.Append (text, 0, 2);
            offset = _ParseSimple (text, offset, length, accumulator);
            result.MainIndex = _Verify (accumulator.ToString(), 0);

            try
            {
                while (offset < length)
                {
                    var previousOffset = offset;

                    // Ищем комбинированный индекс
                    accumulator.Clear();
                    offset = _ParseCombined (text, offset, length, accumulator);
                    result.CombinedIndex = _Verify (_EmptyToNull (accumulator), 1);

                    // Ищем территориальный
                    accumulator.Clear();
                    offset = _ParseTerritorial (text, offset, length, accumulator);
                    result.TerritorialIndex = _EmptyToNull (accumulator);

                    // Запятая
                    accumulator.Clear();
                    offset = _ParseComma (text, offset, length, accumulator);
                    result.Comma = _EmptyToNull (accumulator);

                    // Неведомая хрень
                    accumulator.Clear();
                    offset = _ParseHren (text, offset, length, accumulator);
                    result.Hren = _EmptyToNull (accumulator);

                    // Определитель
                    accumulator.Clear();
                    offset = _ParseQualifier (text, offset, length, accumulator);
                    var qualifier = _EmptyToNull (accumulator);
                    if (!string.IsNullOrEmpty (qualifier))
                    {
                        result.Qualifiers.Add (qualifier);
                    }

                    // Специальное типовое деление
                    accumulator.Clear();
                    offset = _ParseSpecial (text, offset, length, accumulator);
                    var specialIndex = _EmptyToNull (accumulator);
                    if (!string.IsNullOrEmpty (specialIndex))
                    {
                        result.SpecialIndex.Add (specialIndex);
                    }

                    // Код социальной системы
                    accumulator.Clear();
                    offset = _ParseSocial (text, offset, length, accumulator);
                    result.SocialIndex = _EmptyToNull (accumulator);

                    if (offset == previousOffset)
                    {
                        Magna.Error
                            (
                                nameof (BbkIndex) + "::" + nameof (Parse)
                                + "garbage found: "
                                + text.ToVisibleString()
                            );

                        throw new BbkException
                            (
                                "Нераспознанные символы начиная с '"
                                + text.Substring (offset)
                                + "'"
                            );
                    }
                }
            }
            finally
            {
                StringBuilderPool.Shared.Return (accumulator);
            }

            return result;
        }

        /// <summary>
        /// Дамп.
        /// </summary>
        public void Dump
            (
                TextWriter writer,
                string prefix
            )
        {
            Sure.NotNull (writer);

            _Dump (writer, "Основной индекс", MainIndex, prefix);
            _Dump (writer, "Комбинированный индекс", CombinedIndex, prefix);
            _Dump (writer, "Территориальное деление", TerritorialIndex, prefix);
            _Dump (writer, "Некая хрень", Hren, prefix);
            _Dump (writer, "Запятая", Comma, prefix);

            foreach (var qualifier in Qualifiers)
            {
                _Dump (writer, "Определитель", qualifier, prefix);
            }

            foreach (var specialIndex in SpecialIndex)
            {
                _Dump (writer, "Специальное деление", specialIndex, prefix);
            }

            _Dump (writer, "Социальная система ", SocialIndex, prefix);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append (MainIndex);
            builder.Append (CombinedIndex);
            builder.Append (TerritorialIndex);
            builder.Append (Hren);
            builder.Append (Comma);
            foreach (var qualifier in Qualifiers)
            {
                builder.Append (qualifier);
            }

            foreach (var index in SpecialIndex)
            {
                builder.Append (index);
            }

            builder.Append (SocialIndex);

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        #endregion
    }
}
