// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BookInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure.Unifors;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Общая информация о книге.
    /// </summary>
    public sealed class BookInfo
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Record.
        /// </summary>
        public Record Record { get; private set; }

        /// <summary>
        /// Количество экземпляров.
        /// </summary>
        public int Amount
        {
            get { return _ExecuteScript(_amountScript).SafeToInt32(); }
        }

        /// <summary>
        /// Первый автор.
        /// </summary>
        public AuthorInfo? FirstAuthor
        {
            get
            {
                AuthorInfo? result = null;
                var field700 = Record.Fields.GetFirstField(700);
                if (!ReferenceEquals(field700, null))
                {
                    result = AuthorInfo.ParseField700(field700);
                }

                return result;
            }
        }

        /// <summary>
        /// Авторы.
        /// </summary>
        public AuthorInfo[] Authors
        {
            get { return AuthorInfo.ParseRecord(Record, AuthorInfo.AllKnownTags); }
        }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        public string Description
        {
            get
            {
                var result = Record.Description
                             ?? _ExecuteScript(_descriptionScript);
                result = UniforPlusS.DecodeTitle("1" + result);

                return result;
            }
        }

        /// <summary>
        /// Характер документа (первый из).
        /// </summary>
        public ReadOnlyMemory<char> DocumentCharacter =>
            Record.FM(900, 'c');

        /// <summary>
        /// Тип документа.
        /// </summary>
        public ReadOnlyMemory<char> DocumentType =>
            Record.FM(900, 't');

        /// <summary>
        /// Электронный ресурс?
        /// </summary>
        public bool Electronic
        {
            get
            {
                // Электронными считаются:
                // 1. Те, у которых проставлен тип документа L
                // 2. Те, у которых единица измерения r, j или o
                // 3. Те, к которым прикреплен файл, и это не обложка

                var documentType = DocumentType;
                if (documentType.IsOneOf("l", "m"))
                {
                    return true;
                }

                var measureUnit = Record.FM(215, '1').FirstChar();
                if (measureUnit.IsOneOf('r', 'j', 'o'))
                {
                    return true;
                }

                var all951 = Record.Fields.GetField(951);
                foreach (var v951 in all951)
                {
                    var v951h = v951.GetFirstSubFieldValue('h');
                    if (!v951h.IsOneOf("02", "02a", "02b"))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Экземпляры.
        /// </summary>
        public ExemplarInfo[] Exemplars
        {
            get { return ExemplarInfo.Parse(Record); }
        }

        /// <summary>
        /// Число экземпляров.
        /// </summary>
        public int ExemplarCount
        {
            get
            {
                var result = 0;
                foreach (var exemplar in Exemplars)
                {
                    var status = exemplar.Status;
                    if (status != "0"
                        && status != "1"
                        && status != "5"
                        && status != "9")
                    {
                        continue;
                    }

                    var amount = exemplar.Amount.SafeToInt32();
                    if (amount == 0)
                    {
                        amount = 1;
                    }

                    result += amount;
                }

                return result;
            }
        }

        /// <summary>
        /// Документ на иностранном языке?
        /// </summary>
        public bool Foreign
        {
            get
            {
                var languages = Languages;
                if (languages.Length == 0)
                {
                    return false;
                }

                return !languages[0].SameString("rus");
            }
        }

        /// <summary>
        /// Языки документа
        /// </summary>
        public ReadOnlyMemory<char>[] Languages =>
            Record.FMA(101);

        /// <summary>
        /// Первая ссылка на внешний ресурс.
        /// </summary>
        public ReadOnlyMemory<char> Link =>
            _ExecuteScript(_linkScript).AsMemory();

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public int Pages => CountPages(Volume);

        /// <summary>
        /// Цена, общая для всех экземпляров.
        /// </summary>
        public decimal Price
        {
            get { return Record.FM(10, 'd').SafeToDecimal(); }
        }

        /// <summary>
        /// Издательства.
        /// </summary>
        public ReadOnlyMemory<char>[] Publishers
        {
            get
            {
                var result = new List<ReadOnlyMemory<char>>();
                result.AddRange(Record.FMA(210, 'c'));
                result.AddRange(Record.FMA(461, 'g'));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Область заглавия.
        /// </summary>
        public TitleInfo Title
        {
            get
            {
                var result = TitleInfo.Parse(Record);

                return result[0];
            }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string TitleText
        {
            get { return _ExecuteScript(_titleScript); }
        }

        /// <summary>
        /// Счётчик выдач.
        /// </summary>
        public int UsageCount
        {
            get { return Record.FM(999).SafeToInt32(); }
        }

        /// <summary>
        /// Объем издания (цифры).
        /// </summary>
        public ReadOnlyMemory<char> Volume => Record.FM(215, 'a');

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        public ReadOnlyMemory<char> Worksheet => Record.FM(920);

        /// <summary>
        /// Год издания.
        /// </summary>
        public int Year
        {
            get
            {
                var record = Record;
                var result = record.FM(210, 'd');
                if (result.IsEmpty)
                {
                    result = record.FM(461, 'h');
                }
                if (result.IsEmpty)
                {
                    result = record.FM(461, 'z');
                }
                if (result.IsEmpty)
                {
                    result = record.FM(463, 'j');
                }
                if (result.IsEmpty)
                {
                    result = record.FM(934);
                }
                if (result.IsEmpty)
                {
                    return 0;
                }

                // TODO: реализовать оптимально

                var match = Regex.Match(result.ToString(), @"\d{4}");
                if (match.Success)
                {
                    result = match.Value.AsMemory();
                }

                return result.SafeToInt32();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BookInfo
            (
                ISyncProvider provider,
                Record record
            )
        {
            Provider = provider;
            Record = record;
        }

        #endregion

        #region Private members

        private static string _amountScript = "f(rsum((if p(v910) then if '0159': v910^a then '1' fi, if 'CU': v910^a then v910^1 fi, ';' fi)),0,0)";

        private static string _descriptionScript = "@brief";

        private static string _linkScript = "(if p(v951^i) then if not v951^h:'02' then v951^i, break, fi, fi)";

        private static string _titleScript = "if p(v461) then v461^c, v461^2, \" : \"v461^e, '. ' fi, v200^v\". \", v200^a, v200^b, \" : \"v200^e, &uf('!')";

        private string _ExecuteScript
            (
                string script
            )
        {
            var formatter = new PftFormatter();
            formatter.SetProvider(Provider);
            formatter.ParseProgram(script);
            var result = formatter.FormatRecord(Record);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Count pages.
        /// </summary>
        public static int CountPages
            (
                ReadOnlyMemory<char> text
            )
        {
            if (text.IsEmpty)
            {
                return 0;
            }

            text = text.Trim();
            if (text.IsEmpty)
            {
                return 0;
            }

            var result = 0;

            // TODO: реализовать оптимально

            var matches = Regex.Matches
                (
                    text.ToString(),
                    @"[ivxlcm]+",
                    RegexOptions.IgnoreCase
                );
            foreach (Match match in matches)
            {
                var value = UniforPlus9.ToArabicNumber(match.Value);
                result += value;
            }

            matches = Regex.Matches(text.ToString(), @"[0-9]+");
            foreach (Match match in matches)
            {
                var value = FastNumber.ParseInt32(match.Value);
                result += value;
            }

            return result;
        }

        #endregion
    }
}
