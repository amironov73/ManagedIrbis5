// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MenuChapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;
using ManagedIrbis.Trees;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public class MenuChapter
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        /// <summary>
        /// Leaf nodes only can contain records.
        /// </summary>
        [JsonPropertyName("leafOnly")]
        public bool LeafOnly { get; set; }

        /// <summary>
        /// Menu name.
        /// </summary>
        [JsonPropertyName("menuName")]
        public string? MenuName { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [JsonPropertyName("orderBy")]
        public string? OrderBy { get; set; }

        /// <summary>
        /// Record selector.
        /// </summary>
        [JsonPropertyName("recordSelector")]
        public string? RecordSelector { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [JsonPropertyName("search")]
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Title format.
        /// </summary>
        [JsonPropertyName("titleFormat")]
        public string? TitleFormat { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [JsonIgnore]
        public RecordCollection? Records { get; private set; }

        /// <summary>
        /// List of settings.
        /// </summary>
        [JsonPropertyName("menuSettings")]
        public List<SpecialSettings> MenuSettings { get; private set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuChapter()
        {
            MenuSettings = new List<SpecialSettings>();
        }

        #endregion

        #region Private members

        private static char[] _lineDelimiters
            = { '\r', '\n', '\u001E', '\u001F' };

        private MenuSubChapter _CreateChapter
            (
                IPftFormatter formatter,
                TreeLine item
            )
        {
            string key = item.Prefix.Trim();
            var settings = MenuSettings.FirstOrDefault
                (
                    s => s.Name == key
                );
            string value = item.Suffix;

            var record = new Record();
            record.Fields.Add(new Field { Tag = 1, Value = key });
            record.Fields.Add(new Field { Tag = 2, Value = value });
            var title = formatter.FormatRecord(record);

            string className = null;
            if (!ReferenceEquals(settings, null))
            {
                className = settings.GetSetting("type");
            }

            MenuSubChapter result;
            if (string.IsNullOrEmpty(className))
            {
                result = new MenuSubChapter();
            }
            else
            {
                if (!className.Contains("."))
                {
                    className = "ManagedIrbis.Biblio." + className;
                }
                var type = Type.GetType(className, true)
                    .ThrowIfNull("Type.GetType");
                result = (MenuSubChapter)Activator.CreateInstance(type);
            }
            result.Key = key;
            result.MainChapter = this;
            result.Title = title;
            result.Value = value;
            result.Settings = settings;

            foreach (var child in item.Children)
            {
                var subChapter
                    = _CreateChapter(formatter, child);
                result.Children.Add(subChapter);
            }

            return result;
        }

        private void _RemoveSubField
            (
                Record record,
                int tag,
                char code
            )
        {
            var fields = record.Fields.GetField(tag);
            foreach (var field in fields)
            {
                field.RemoveSubField(code);
            }
        }

        private void _BeautifyRecord
            (
                Record record
            )
        {
            // Украшаем запись согласно вкусам библиографов

            foreach (var field in record.Fields)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    field.Value = MenuSubChapter.Enhance(field.Value);
                }

                foreach (var subField in field.Subfields)
                {
                    if (!string.IsNullOrEmpty(subField.Value))
                    {
                        subField.Value = MenuSubChapter.Enhance(subField.Value);
                    }
                }
            }

            // Источник библиографической записи
            record.RemoveField(488);

            // Сведения об автографах
            record.RemoveField(391);

            var worksheet = record.FM(920);
            if (!worksheet.SameString("ASP"))
            {
                return;
            }

            // Подзаголовочные сведения журналов
            _RemoveSubField(record, 463, '7');
            //_RemoveSubField(record, 963, 'e');

            // Издательство в статьях
            _RemoveSubField(record, 463, 'g');

            // Сведения об автографах
            record.RemoveField(391);

            // Из аннотаций брать только первое повторение
            var annotations = record.Fields.GetField(331);
            for (var i = 1; i < annotations.Length; i++)
            {
                record.Fields.Remove(annotations[i]);
            }
        }

        private static readonly Regex _regex463 = new Regex(@"^№.*\((?<date>.*?)\)$");

        private void _FixDate
            (
                SubField? subField
            )
        {
            if (!ReferenceEquals(subField, null))
            {
                var value = subField.Value;
                if (!string.IsNullOrEmpty(value))
                {

                    var match = _regex463.Match(value);
                    if (match.Success)
                    {

                        var date = match.Groups["date"].Value;
                        if (!string.IsNullOrEmpty(date)
                            && date.Contains(" "))
                        {
                            subField.Value = date.Replace(" ", "\\~");
                        }
                    }
                }
            }
        }

        private void _Fix463
            (
                Record record
            )
        {
            //
            // Переделываем даты у газет, как нравится библиографам
            // из "№ 11 (5 мая)" в просто "5 мая".
            //
            // Всё, что не походит под шаблон, оставляем как есть.
            // Пусть библиографы правят сами, ручками
            //

            var fields = record.Fields.GetField(463);
            foreach (var field in fields)
            {
                var subField = field.Subfields.GetFirstSubField('h');
                _FixDate(subField);

                subField = field.Subfields.GetFirstSubField('v');
                _FixDate(subField);
            }
        }

        private static int _GetYear
            (
                Record record
            )
        {
            var result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(463, 'j');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(934);
            }
            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }

            var match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }
            return result.SafeToInt32();
        }

        private void _GatherSame
            (
                BiblioContext context
            )
        {
            //
            // Собираем вместе записи с одинаковым полем 2025
            //

            var records = Records;
            if (ReferenceEquals(records, null))
            {
                return;
            }

            var allMarked = records.Where(r => r.HaveField(2025)).ToArray();
            var grouped = allMarked.GroupBy(r => r.FM(2025));
            var toRemove = new List<Record>();
            foreach (var oneGroup in grouped)
            {
                var array = oneGroup.ToArray();
                if (array.Length == 1)
                {
                    continue;
                }

                foreach (var record in array)
                {
                    record.UserData = _GetYear(record);
                }

                array = array.OrderBy(r => (int) r.UserData).ToArray();

                var firstRecord = array[0];
                var same = new RecordCollection();
                for (var i = 1; i < array.Length; i++)
                {
                    var record = array[i];
                    record.RemoveField(200);
                    record.RemoveField(922);
                    record.RemoveField(925);
                    record.RemoveField(700);
                    record.RemoveField(701);
                    record.RemoveField(702);
                    record.RemoveField(331); // Аннотация
                    record.RemoveField(101); // Язык основного текста

                    record.Fields.Add
                        (
                            new Field { Tag = 200 }.Add('a', "То же")
                        );

                    same.Add(record);
                    toRemove.Add(record);
                }

                firstRecord.UserData = same;
            }

            foreach (var recordToRemove in toRemove)
            {
                records.Remove(recordToRemove);
                context.Records.Remove(recordToRemove);
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine("Begin gather records {0}", this);
            var badRecords = context.BadRecords;
            Records = new RecordCollection();
            Record record = null;

            try
            {
                var processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (var formatter
                    = processor.AcquireFormatter(context))
                {
                    var provider = context.Provider;
                    var records = Records
                        .ThrowIfNull("Records");

                    var searchExpression = SearchExpression
                        .ThrowIfNull("SearchExpression");
                    formatter.ParseProgram(searchExpression);
                    record = new Record();
                    searchExpression = formatter.FormatRecord(record);

                    var found = provider.Search(searchExpression);
                    log.WriteLine("Found: {0} record(s)", found.Length);

                    log.Write("Reading records");

                    for (var i = 0; i < found.Length; i++)
                    {
                        log.Write(".");
                        record = provider.ReadRecord(found[i]);
                        if (!ReferenceEquals(record, null))
                        {
                            _Fix463(record);
                            _BeautifyRecord(record);
                        }
                        records.Add(record);
                        context.Records.Add(record);
                    }

                    _GatherSame(context);

                    //// Пробуем не загружать записи,
                    //// а предоставить заглушки

                    //for (int i = 0; i < found.Length; i++)
                    //{
                    //    log.Write(".");
                    //    record = new Record
                    //    {
                    //        Mfn = found[i]
                    //    };
                    //    records.Add(record);
                    //    context.Records.Add(record);
                    //}

                    log.WriteLine(" done");

                    CleanRecords(context, records);

                    var dictionary
                        = new Dictionary<string, MenuSubChapter>();
                    Action<BiblioChapter> action = chapter =>
                    {
                        var subChapter = chapter as MenuSubChapter;
                        if (!ReferenceEquals(subChapter, null))
                        {
                            var key = subChapter.Key
                                .ThrowIfNull("subChapter.Key");
                            dictionary.Add(key, subChapter);
                        }
                    };
                    Walk(action);

                    var recordSelector = RecordSelector
                        .ThrowIfNull("RecordSelector");
                    formatter.ParseProgram(recordSelector);
                    log.Write("Distributing records");

                    var mfns = records.Select(r => r.Mfn).ToArray();
                    var formatted = formatter.FormatRecords(mfns);
                    if (formatted.Length != mfns.Length)
                    {
                        throw new IrbisException();
                    }

                    for (var i = 0; i < records.Count; i++)
                    {
                        log.Write(".");

                        record = records[i];
                        //string key
                        //    = formatter.FormatRecord(record);
                        var key = formatted[i];
                        if (string.IsNullOrEmpty(key))
                        {
                            badRecords.Add(record);
                        }
                        else
                        {
                            var keys = key.Trim()
                                .Split(_lineDelimiters)
                                .TrimLines()
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                            key = keys.FirstOrDefault();
                            if (string.IsNullOrEmpty(key))
                            {
                                badRecords.Add(record);
                            }
                            else
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(key, out subChapter))
                                {
                                    subChapter.Records.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }

                            foreach (var nextKey in keys.Skip(1))
                            {
                                MenuSubChapter subChapter;
                                if (dictionary
                                    .TryGetValue(nextKey, out subChapter))
                                {
                                    subChapter.Duplicates.Add(record);
                                }
                                else
                                {
                                    badRecords.Add(record);
                                }
                            }
                        }
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                log.WriteLine(" done");
                log.WriteLine("Bad records: {0}", badRecords.Count);

                // Do we really need this?

                foreach (var child in Children)
                {
                    child.GatherRecords(context);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format
                    (
                        "Exception: {0}",
                        exception
                    );

                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0}{1}{2}",
                            record.Mfn,
                            Environment.NewLine,
                            message
                        );
                }

                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End gather records {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.Initialize" />
        public override void Initialize
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
            try
            {
                var menuName = MenuName.ThrowIfNull("MenuName");

                var provider = context.Provider;

                var specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = provider.Database,
                        FileName = menuName
                    };
                MenuFile menu = provider.ReadMenuFile(specification);
                if (ReferenceEquals(menu, null))
                {
                    throw new IrbisException();
                }
                var tree = menu.ToTree();

                // Create Formatter

                var processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (var formatter
                    = processor.AcquireFormatter(context))
                {
                    var titleFormat = TitleFormat
                        .ThrowIfNull("TitleFormat");
                    formatter.ParseProgram(titleFormat);

                    foreach (var root in tree.Roots)
                    {
                        var chapter
                            = _CreateChapter(formatter, root);
                        Children.Add(chapter);
                    }

                    processor.ReleaseFormatter(context, formatter);
                }

                foreach (var chapter in Children)
                {
                    chapter.Initialize(context);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            var processor = context.Processor
                .ThrowIfNull("context.Processor");

            processor.Report.Body.Add(new NewPageBand());

            base.Render(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}

