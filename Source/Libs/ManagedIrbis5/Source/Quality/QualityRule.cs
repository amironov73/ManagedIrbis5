// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* QualityRule.cs -- базовый абстрактный класс для правил проверки качества
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

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Базовый абстрактный класс для правил проверки качества.
    /// </summary>
    public abstract class QualityRule
    {
        #region Properties

        /// <summary>
        /// Спецификация полей, подлежащих провеке.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public abstract string FieldSpec { get; }

        /// <summary>
        /// Провайдер.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public ISyncProvider? Provider => Context?.Provider;

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
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        private IMemoryCache? _cache;

        /// <summary>
        /// Добавление обнаруженного дефекта в список.
        /// </summary>
        protected void AddDefect
            (
                int tag,
                int damage,
                string format,
                params object[] args
            )
        {
            var defect = new FieldDefect
            {
                Field = tag,
                Damage = damage,
                Message = string.Format (format, args)
            };

            Report.ThrowIfNull (nameof (Report)).Defects.Add (defect);

        } // method AddDefect

        /// <summary>
        /// Добавление обнаруженного дефекта в список.
        /// </summary>
        protected void AddDefect
            (
                Field field,
                int damage,
                string format,
                params object[] args
            )
        {
            var defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Value = field.ToString(), // ToText
                Damage = damage,
                Message = string.Format(format, args)
            };

            Report.ThrowIfNull (nameof (Report)).Defects.Add (defect);

        } // method AddDefect

        /// <summary>
        /// Добавление обнаруженного дефекта в список.
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
            var defect = new FieldDefect
            {
                Field = field.Tag,
                Repeat = field.Repeat,
                Subfield = subfield.Code.ToString(),
                Value = subfield.Value,
                Damage = damage,
                Message = string.Format(format, args)
            };

            Report.ThrowIfNull (nameof (Report)).Defects.Add (defect);

        } // method AddDefect

        /// <summary>
        /// Начало проверки записи.
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
        /// Кеширование меню.
        /// </summary>
        protected MenuFile? CacheMenu
            (
                string name,
                MenuFile? menu
            )
        {
            if (menu is null)
            {
                if (_cache is null)
                {
                    var options = new MemoryCacheOptions();
                    _cache = new MemoryCache (options);
                }

                var specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = Provider.ThrowIfNull (nameof (Provider)).Database,
                    FileName = name
                };
                var key = specification.ToString().ToUpperInvariant();
                if (!_cache.TryGetValue (key, out menu))
                {
                    menu = Provider.ThrowIfNull (nameof(Provider)).ReadMenu (specification);
                    _cache.Set (key, menu);
                }
            }

            return menu;

        } // CacheMenu

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

            var entry = menu.GetEntrySensitive(value);

            return !ReferenceEquals(entry, null);
        }

        /// <summary>
        /// Get text at specified position in the string.
        /// </summary>
        protected static ReadOnlyMemory<char> GetTextAtPosition
            (
                string text,
                int position
            )
        {
            var length = text.Length;
            var start = Math.Max(0, position - 1);
            var stop = Math.Min(length - 1, position + 2);

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

            return text.AsMemory().Slice
                (
                    start,
                    stop - start + 1
                )
                .Trim();
        }

        /// <summary>
        /// Вывод места в строке, где пробел удвоился.
        /// </summary>
        protected static ReadOnlyMemory<char> ShowDoubleWhiteSpace
            (
                ReadOnlyMemory<char> text
            )
        {
            var position = text.Span.IndexOf
                (
                    "  ",
                    StringComparison.Ordinal
                );

            return GetTextAtPosition
                (
                    text.ToString(),
                    position
                );

        } // method ShowDoubleWhiteSpace

        /// <summary>
        /// Проверка наличия пробелов в строке.
        /// </summary>
        protected void CheckWhitespace
            (
                Field field,
                SubField subfield
            )
        {
            var text = subfield.Value;

            if (text.IsEmpty())
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

            } // if

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
            } // if

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

            } // if

            // TODO: реализовать эффективно

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
                        ShowDoubleWhiteSpace(text.AsMemory())
                    );

            } // if

        } // method CheckWhitespace

        /// <summary>
        /// Проверка наличия пробелов в строке.
        /// </summary>
        protected void CheckWhitespace
            (
                Field field
            )
        {
            var text = field.Value;
            if (!text.IsEmpty())
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
                } // if

                if (text.EndsWith(" "))
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Поле {0} оканчивается пробелом",
                            field.Tag
                        );
                } // if

                // TODO: implement

                throw new NotImplementedException();

                /*

                if (text.Span.Contains("  "))
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

                */
            } // if

            foreach (var subfield in field.Subfields)
            {
                CheckWhitespace
                    (
                        field,
                        subfield
                    );
            } // foreach

        } // method CheckWhitespace

        /// <summary>
        /// Окончание проверки записи.
        /// </summary>
        protected RuleReport EndCheck()
        {
            var result = Report.ThrowIfNull (nameof(Report));
            result.Damage = result.Defects.Sum (defect => defect.Damage);

            return result;

        } // method EndCheck

        /// <summary>
        /// Текущий рабочий лист ASP?
        /// </summary>
        protected bool IsAsp() => IrbisUtility.IsAsp (Worksheet);

        /// <summary>
        /// Текущий рабочий лист относится к книжным: PAZK, SPEC или PVK?
        /// </summary>
        protected bool IsBook() => IrbisUtility.IsBook (Worksheet);

        /// <summary>
        /// Текущий рабочий лист PAZK?
        /// </summary>
        protected bool IsPazk() => IrbisUtility.IsPazk (Worksheet);

        /// <summary>
        /// Текущий рабочий лист SPEC?
        /// </summary>
        protected bool IsSpec() => IrbisUtility.IsSpec (Worksheet);

        /// <summary>
        /// Фильтрация полей записи согласно спецификации <see cref="FieldSpec"/>
        /// для текущего правила.
        /// </summary>
        protected Field[] GetFields() => Record.ThrowIfNull (nameof (Record)).Fields
                .GetFieldBySpec (FieldSpec);

        /// <summary>
        /// Поле не должно содержать подполей.
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

        } // method MustNotContainSubfields

        /// <summary>
        /// Asserts that the field must not contain plain text value.
        /// </summary>
        protected void MustNotContainText
            (
                Field field
            )
        {
            if (!field.Value.IsEmpty())
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
            var grouped = fields.GroupBy
                (
                    f => f.Value
                        .ThrowIfNullOrEmpty()
                        .ToString()
                        .ToLowerInvariant()
                );

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
                .Where(sf => sf.Value.IsEmpty());
            foreach (var subField in selected)
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
                        .ThrowIfNullOrEmpty()
                        .ToString()
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
            foreach (var code in codes)
            {
                MustBeUniqueSubfield
                    (
                        fields,
                        code
                    );
            }

        } // method MustBeUniqueSubfield

        /// <summary>
        /// Asserts that the field must not contain whitespace.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                Field field
            )
        {
            var text = field.Value;
            if (!string.IsNullOrEmpty (text)
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

        } // method MustNotContainWhitespace

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
        /// Подполя с указанными кодами не должны содержать пробелов.
        /// </summary>
        protected void MustNotContainWhitespace
            (
                Field field,
                params char[] codes
            )
        {
            foreach (var code in codes)
            {
                var subFields = field.GetSubFields (code);
                foreach (var subField in subFields)
                {
                    MustNotContainWhitespace
                        (
                            field,
                            subField
                        );
                } // foreach
            } // foreach

        } // method MustoNotContainWhitespace

        /// <summary>
        /// Поле не должно содержать запрещенных символов.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                Field field
            )
        {
            var text = field.Value;
            if (!text.IsEmpty())
            {
                var position = RuleUtility.BadCharacterPosition(text);
                if (position >= 0)
                {
                    AddDefect
                        (
                            field,
                            3,
                            "Поле {0} содержит запрещённые символы: {1}",
                            GetTextAtPosition (text, position)
                        );
                } // if
            } // if

        } // method MustNotContainBadCharacters

        /// <summary>
        /// Подполе не должно содержать запрещенных символов.
        /// </summary>
        protected void MustNotContainBadCharacters
            (
                Field field,
                SubField subField
            )
        {
            var text = subField.Value;
            if (!text.IsEmpty())
            {
                var position = RuleUtility.BadCharacterPosition (text);
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
                            GetTextAtPosition (text, position)
                        );
                } // if
            } // if

        } // method MustNotContainBadCharacters

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка записи.
        /// </summary>
        public abstract RuleReport CheckRecord
            (
                RuleContext context
            );

        #endregion

    } // class QualityRule

} // namespace ManagedIrbis.Quality
