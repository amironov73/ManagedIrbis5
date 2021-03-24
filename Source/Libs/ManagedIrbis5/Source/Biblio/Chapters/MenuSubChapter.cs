// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MenuSubChapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;
using AM.Text.Output;

using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    public class MenuSubChapter
        : ChapterWithRecords
    {
        #region Properties

        /// <summary>
        /// Key.
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Main chapter.
        /// </summary>
        public MenuChapter? MainChapter { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get
            {
                if (Children.Count == 0)
                {
                    return false;
                }

                var mainChapter = MainChapter;
                if (mainChapter is null)
                {
                    return Records.Count == 0;
                }

                return mainChapter.LeafOnly && Records.Count == 0;
            }
        } // property IsServiceChapter

        #endregion

        #region Private members

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        protected virtual string GetDescriptionFormat()
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                var subChapter = chapter as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    var settings = subChapter.Settings;
                    if (!ReferenceEquals(settings, null))
                    {
                        var result = settings.GetSetting("format");
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }

                chapter = chapter.Parent;
            }

            return MainChapter
                .ThrowIfNull("MainChapter")
                .Format
                .ThrowIfNull("MainChapter.Format");
        }

        /// <summary>
        /// Get order format from chapter hierarchy.
        /// </summary>
        protected virtual string GetOrderFormat()
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                var subChapter = chapter as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    var settings = subChapter.Settings;
                    if (!ReferenceEquals(settings, null))
                    {
                        var result = settings.GetSetting("order");
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }

                chapter = chapter.Parent;
            }

            return MainChapter
                .ThrowIfNull("MainChapter")
                .OrderBy
                .ThrowIfNull("MainChapter.Format");
        }

        internal static string Enhance
            (
                string text
            )
        {
            var result = text
                .Replace(". - ", ". – ")
                .Replace("№", "\\'B9");

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            Record? record = null;

            log.WriteLine("Begin build items {0}", this);
            Items = new ItemCollection();

            try
            {
                if (Records.Count != 0)
                {
                    var processor = context.Processor
                        .ThrowIfNull("context.Processor");
                    var mainChapter = MainChapter
                        .ThrowIfNull("MainChapter");

                    using (var formatter = processor.AcquireFormatter(context))
                    {
                        var descriptionFormat = GetDescriptionFormat();
                        descriptionFormat = processor.GetText
                            (
                                context,
                                descriptionFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(descriptionFormat);
                        //string[] formatted
                        //    = FormatRecords(context, descriptionFormat);

                        for (var i = 0; i < Records.Count; i++)
                        {
                            log.Write(".");
                            record = Records[i];
                            //string description = formatted[i]
                            //    .TrimEnd('\u001F');
                            var description = formatter.FormatRecord(record)
                                .TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(description)

                            description = BiblioUtility.AddTrailingDot(description);

                            var item = new BiblioItem
                            {
                                Chapter = this,
                                Record = record,
                                Description = description
                            };
                            Items.Add(item);

                            var same = record.UserData as RecordCollection;
                            if (!ReferenceEquals(same, null))
                            {
                                foreach (var oneRecord in same)
                                {
                                    var desc = formatter.FormatRecord(oneRecord)
                                        .TrimEnd('\u001F');
                                    desc = BiblioUtility.AddTrailingDot(desc);
                                    oneRecord.Description = desc;
                                }
                            }
                        }

                        log.WriteLine(" done");

                        //string orderFormat = mainChapter.OrderBy
                        //    .ThrowIfNull("mainChapter.OrderBy");
                        var orderFormat = GetOrderFormat();
                        orderFormat = processor.GetText
                            (
                                context,
                                orderFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(orderFormat);
                        // formatted = FormatRecords(context, orderFormat);
                        for (var i = 0; i < Items.Count; i++)
                        {
                            log.Write(".");
                            var item = Items[i];
                            record = item.Record;
                            //string order = formatted[i].TrimEnd('\u001F');
                            var order = formatter.FormatRecord(record)
                                .TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(order)

                            order = CleanOrder(order);

                            //item.Order = RichText.Decode(order);
                            item.Order = order;
                        }

                        log.WriteLine(" done");

                        Items.SortByOrder();

                        log.WriteLine("Items: {0}", Items.Count);
                    }
                }

                foreach (var chapter in Children)
                {
                    chapter.BuildItems(context);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format
                    (
                        "Exception: {0}", exception
                    );
                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0} : {1}",
                            record.Mfn,
                            message
                        );
                }

                log.WriteLine(string.Empty);
                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End build items {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine("Begin render {0}", this);

            var processor = context.Processor
                .ThrowIfNull("context.Processor");
            var report = processor.Report
                .ThrowIfNull("processor.Report");
            // ReportDriver driver = context.ReportContext.Driver;

            var showOrder =
                context.Document.CommonSettings.Value<bool?>("showOrder") ?? false;

            if (Records.Count != 0
                || Duplicates.Count != 0
                || Children.Count != 0)
            {
                RenderTitle(context);

                for (var i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    var item = Items[i];
                    var number = item.Number;
                    var description = item.Description.ThrowIfNull("item.Description");

                    description = Enhance(description);

                    ReportBand band = new ParagraphBand
                        (
                            number.ToInvariantString() + ".\\~\\~"
                        );
                    report.Body.Add(band);

                    band.Cells.Add(new SimpleTextCell
                        (
                            // TODO implement properly!!!
                            //RichText.Encode(description, UnicodeRange.Russian)
                            //RichText.Encode2(description, UnicodeRange.Russian)
                            RichText.Encode3(description, UnicodeRange.Russian, "\\f2")
                        ));

                    var record = item.Record;

                    // Для отладки: проверить упорядочение
                    if (showOrder)
                    {
                        if (!ReferenceEquals(record, null))
                        {
                            band = new ParagraphBand("MFN " + record.Mfn + " " + item.Order);
                            report.Body.Add(band);
                            report.Body.Add(new ParagraphBand());
                        }
                    }

                    if (!ReferenceEquals(record, null))
                    {
                        var sameBooks = record.UserData as RecordCollection;
                        if (!ReferenceEquals(sameBooks, null))
                        {
                            foreach (var book in sameBooks)
                            {
                                var text = book.Description;
                                text = RichText.Encode3(text, UnicodeRange.Russian, "\\f2")
                                    .ThrowIfNull();
                                band = new ParagraphBand(text);
                                report.Body.Add(band);
                                //band.Cells.Add(new SimpleTextCell(text));
                            }
                        }
                    }
                }

                log.WriteLine(" done");
            }

            RenderDuplicates(context);

            RenderChildren(context);

            log.WriteLine(string.Empty);
            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="BiblioChapter.ToString" />
        public override string ToString()
        {
            var result = base.ToString()
                         + " [:] "
                         + Records.Count.ToInvariantString();

            return result;
        }

        #endregion
    }
}
