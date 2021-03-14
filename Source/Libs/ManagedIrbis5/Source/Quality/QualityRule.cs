// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* QualityRule.cs -- abstract base class for all the quality rules
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Abstract base class for all the quality rules.
    /// </summary>
    public abstract class QualityRule
    {
        #region Properties

        /// <summary>
        /// Specification of the fields to check.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public abstract string FieldSpec { get; }

        /// <summary>
        /// Клиент.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public IIrbisConnection? Connection =>
            Context?.Connection;

        /// <summary>
        /// Текущий контекст.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RuleContext? Context { get; protected set; }

        /// <summary>
        /// Текущая проверяемая запись.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Record? Record => Context?.Record;

        /// <summary>
        /// Накопленный отчёт.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public RuleReport? Report { get; protected set; }

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public string? Worksheet => Record?.FM(920);

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                int tag,
                int damage,
                string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = tag,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.ThrowIfNull("Report").Defects.Add(defect);
        }

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                Field field,
                int damage,
                string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Value = field.ToString(), // ToText
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.ThrowIfNull("Report").Defects.Add(defect);
        }

        /// <summary>
        /// Add detected defect.
        /// </summary>
        protected void AddDefect
            (
                Field field,
                SubField subfield,
                int damage,
                string format,
                params object[] args
            )
        {
            FieldDefect defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Subfield = subfield.Code.ToString(),
                Value = subfield.Value,
                Damage = damage,
                Message = string.Format(format, args)
            };
            Report.ThrowIfNull("Report").Defects.Add(defect);
        }

        /// <summary>
        /// Begin the record checking.
        /// </summary>
        protected void BeginCheck
            (
                RuleContext context
            )
        {
            Context = context;
            Report = new RuleReport();
        }

        /// <summary>
        /// Cache the menu.
        /// </summary>
        protected MenuFile CacheMenu
            (
                string name,
                MenuFile? menu
            )
        {
            /*

            menu = menu ??
                MenuFile.ReadFromServer
                (
                    Connection,
                    new FileSpecification(IrbisPath.MasterFile, name)
                );

            return menu;

            */

            return new MenuFile();
        }

        /// <summary>
        /// Check the value against the menu.
        /// </summary>
        protected bool CheckForMenu
            (
                MenuFile? menu,
                string? value
            )
        {
            if (ReferenceEquals(menu, null))
            {
                return true;
            }
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            MenuEntry entry = menu.GetEntrySensitive(value);

            return !ReferenceEquals(entry, null);
        }

        /// <summary>
        /// Get text at specified position in the string.
        /// </summary>
        protected static string GetTextAtPosition
            (
                string text,
                int position
            )
        {
            int length = text.Length;
            int start = Math.Max(0, position - 1);
            int stop = Math.Min(length - 1, position + 2);
            while (start >= 0 && text[start] == ' ')
            {
                start--;
            }
            while (start >= 0 && text[start] != ' ')
            {
                start--;
            }
            start = Math.Max(0, start);
            while (stop < length && text[stop] == ' ')
            {
                stop++;
            }
            while (stop < length && text[stop] != ' ')
            {
                stop++;
            }
            stop = Math.Min(length - 1, stop);

            return text.Substring
                (
                    start,
                    stop - start + 1
                )
                .Trim();
        }

        /// <summary>
        /// Show double whitespace in the text.
        /// </summary>
        protected static string ShowDoubleWhiteSpace
            (
                string text
            )
        {
            int position = text.IndexOf
                (
                    "  ",
                    StringComparison.Ordinal
                );

            return GetTextAtPosition
                (
                    text,
                    position
                );
        }

        /// <summary>
        /// Check the subfield for whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                Field field,
                SubField subfield
            )
        {
            string text = subfield.Value;

            if (string.IsNullOrEmpty(text))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Пустое подполе {0}^{1}",
                        field.Tag,
                        subfield.Code
                    );
                return;
            }

            if (text.StartsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} начинается с пробела",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.EndsWith(" "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} оканчивается пробелом",
                        field.Tag,
                        subfield.Code
                    );
            }

            if (text.Contains("  "))
            {
                AddDefect
                    (
                        field,
                        subfield,
                        1,
                        "Подполе {0}^{1} содержит двойной пробел: {2}",
                        field.Tag,
                        subfield.Code,
                        ShowDoubleWhiteSpace(text)
                    );
            }
        }

        /// <summary>
        /// Check for whitespace.
        /// </summary>
        protected void CheckWhitespace
            (
                Field field
            )
        {
            string text = field.Value;
            if (!string.IsNullOrEmpty(text))
            {
                if (text.StartsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} начинается с пробела",
                            field.Tag
                        );
                }
                if (text.EndsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} оканчивается пробелом",
                            field.Tag
                        );
                }
                if (text.Contains("  "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} содержит двойной пробел: {1}",
                            field.Tag,
                            ShowDoubleWhiteSpace(text)
                        );
                }
            }

            foreach (SubField subfield in field.Subfields)
            {
                CheckWhitespace
                    (
                        field,
                        subfield
                    );
            }
        }

        /// <summary>
        /// End the record checking.
        /// </summary>
        protected RuleReport EndCheck()
        {
            RuleReport result = Report.ThrowIfNull("Report");
            result.Damage = result.Defects.Sum(defect => defect.Damage);

            return result;
        }

        /// <summary>
        /// Whether the current working list is ASP?
        /// </summary>
        protected bool IsAsp()
        {
            return Worksheet.SameString("ASP");
        }

        /// <summary>
        /// Whether the current working list is book-specific:
        /// PAZK, SPEC or PVK?
        /// </summary>
        protected bool IsBook()
        {
            string worksheet = Worksheet;
            return worksheet.SameString("PAZK")
                   || worksheet.SameString("SPEC")
                   || worksheet.SameString("PVK");
        }

        /// <summary>
        /// Whether the current working list is PAZK?
        /// </summary>
        protected bool IsPazk()
        {
            return Worksheet.SameString("PAZK");
        }

        /// <summary>
        /// Whether the current working list is SPEC?
        /// </summary>
        protected bool IsSpec()
        {
            return Worksheet.SameString("SPEC");
        }

        /// <summary>
        /// Get fields of the current record for the rule
        /// according the <see cref="FieldSpec"/>.
        /// </summary>
        protected Field[] GetFields()
        {
            return Record.ThrowIfNull("Record").Fields
                .GetFieldBySpec(FieldSpec);
        }

        /// <summary>
        /// Must not contain subfields.
        /// </summary>
        protected void MustNotContainSubfields
            (
                Field field
            )
        {
            if (field.Subfields.Count != 0)
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} содержит подполя",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain plain text value.
        /// </summary>
        protected void MustNotContainText
            (
                Field field
            )
        {
            if (!string.IsNullOrEmpty(field.Value))
            {
                AddDefect
                    (
                        field,
                        20,
                        "Поле {0} должно состоять только из подполей",
                        field.Tag
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain
        /// repeatable subfields.
        /// </summary>
        protected void MustNotRepeatSubfields
            (
                Field field
            )
        {
            /*

            var grouped = field.Subfields
                .GroupBy(sf => sf.CodeString.ToLowerInvariant());
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            field,
                            20,
                            "Подполе {0}^{1} повторяется",
                            field.Tag,
                            grp.Key
                        );
                }
            }

            */
        }

        /// <summary>
        /// Asserts that the field must be unique.
        /// </summary>
        protected void MustBeUniqueField
            (
                Field[] fields
            )
        {
            var grouped = fields
                .GroupBy
                (
                    f => f.Value
                        .ThrowIfNull("field.Value")
                        .ToLowerInvariant()
                )
                ;
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            grp.First(),
                            20,
                            "Поле {0} содержит повторяющееся значение {1}",
                            grp.First().Tag,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that her subfield must be non-empty.
        /// </summary>
        protected void MustBeNonEmptySubfield
            (
                Field field,
                char code
            )
        {
            var selected = field.Subfields
                .GetSubField(new[] {code})
                .Where(sf => string.IsNullOrEmpty(sf.Value));
            foreach (SubField subField in selected)
            {
                AddDefect
                    (
                        field,
                        subField,
                        5,
                        "Подполе {0}^{1} пустое",
                        field.Tag,
                        subField.Code
                    );
            }
        }

        /// <summary>
        /// Asserts that subfields of the fields must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                Field[] fields,
                char code
            )
        {
            var grouped = fields
                .SelectMany(f => f.Subfields)
                .GetSubField(new[] {code})
                .GroupBy
                (
                    sf => sf.Value
                        .ThrowIfNull("field.Value")
                        .ToLowerInvariant()
                );
            foreach (var grp in grouped)
            {
                if (grp.Count() != 1)
                {
                    AddDefect
                        (
                            fields[0],
                            grp.First(),
                            5,
                            "Подполе {0}^{1} содержит неуникальное значение {2}",
                            fields[0].Tag,
                            grp.First().Code,
                            grp.Key
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that subfields of the fields must be unique.
        /// </summary>
        protected void MustBeUniqueSubfield
            (
                Field[] fields,
                params char[] codes
            )
        {
            foreach (char code in codes)
            {
                MustBeUniqueSubfield
                    (
                        fields,
                        code
                    );
            }
        }

        /// <summary>
        /// Asserts that the field must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                Field field
            )
        {
            /*

            string text = field.Value;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        3,
                        "Поле {0} содержит пробельные символы",
                        field.Tag
                    );
            }

            */
        }

        /// <summary>
        /// Asserts that the subfield must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                Field field,
                SubField subField
            )
        {
            /*

            string text = subField.Value;
            if (!string.IsNullOrEmpty(text)
                && text.ContainsWhitespace())
            {
                AddDefect
                    (
                        field,
                        subField,
                        3,
                        "Подполе {0}^{1} содержит пробельные символы",
                        field.Tag,
                        subField.Code
                    );
            }

            */
        }

        /// <summary>
        /// Asserts that subfields of the field must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                Field field,
                params char[] codes
            )
        {
            /*

            foreach (char code in codes)
            {
                SubField[] subFields = field.GetSubField(code);
                foreach (SubField subField in subFields)
                {
                    MustNotContainWhitespace
                        (
                            field,
                            subField
                        );
                }
            }

            */
        }

        /// <summary>
        /// Asserts that the field must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                Field field
            )
        {
            string text = field.Value;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            3,
                            "Поле {0} содержит запрещённые символы: {1}",
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        /// <summary>
        /// Asserts that the subfield must not contain bad characters.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                Field field,
                SubField subField
            )
        {
            string text = subField.Value;
            if (!string.IsNullOrEmpty(text))
            {
                int position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Подполе {0}^{1} содержит "
                            + "запрещённые символы: {2}",
                            field.Tag,
                            subField.Code,
                            GetTextAtPosition(text, position)
                        );
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        public abstract RuleReport CheckRecord
            (
                RuleContext context
            );

        #endregion

    } // class QualityRule

} // namespace ManagedIrbis.Quality
