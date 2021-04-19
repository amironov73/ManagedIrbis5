// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CumulatingSubChapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Сводное описание многотомного издания.
    /// </summary>
    public class CumulatingSubChapter
        : MenuSubChapter
    {
        #region Nested classes

        /// <summary>
        /// Multivolume document.
        /// </summary>
        public class Multivolume
            : List<BiblioItem>
        {
            #region Properties

            /// <summary>
            /// Header part.
            /// </summary>
            public string? Header { get; set; }

            /// <summary>
            /// Order
            /// </summary>
            public string? Order { get; set; }

            /// <summary>
            /// Item.
            /// </summary>
            public BiblioItem? Item { get; set; }

            /// <summary>
            /// Single.
            /// </summary>
            public bool Single { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Groups.
        /// </summary>
        public List<Multivolume> Groups { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CumulatingSubChapter()
        {
            Groups = new List<Multivolume>();
        }

        #endregion

        #region Private members

        //private static char[] _lineDelimiters = { '\r', '\n' };

        /// <summary>
        ///  Order the group.
        /// </summary>
        protected static void OrderGroup
            (
                Multivolume bookGroup
            )
        {
            var items = bookGroup
                .OrderBy(item => item.Order)
                .ToArray();
            bookGroup.Clear();
            bookGroup.AddRange(items);
        }

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="MenuSubChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            base.BuildItems(context);

            var items = Items;
            if (items is null)
            {
                return;
            }

            var settings = Settings;
            if (ReferenceEquals(settings, null))
            {
                return;
            }

            var generalFormat = settings.GetSetting("general");
            var orderFormat = settings.GetSetting("order");
            if (string.IsNullOrEmpty(generalFormat)
                || string.IsNullOrEmpty(orderFormat))
            {
                return;
            }

            var log = context.Log;
            log.WriteLine("Begin grouping {0}", this);

            var processor = context.Processor.ThrowIfNull("context.Processor");
            using (var formatter = processor.AcquireFormatter(context))
            {
                generalFormat = processor.GetText(context, generalFormat)
                    .ThrowIfNull("generalFormat");
                formatter.ParseProgram(generalFormat);

                foreach (var item in items)
                {
                    var record = item.Record.ThrowIfNull("item.Record");
                    var header = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(header))
                    {
                        header = header.Trim();
                    }
                    var bookGroup = Groups.FirstOrDefault
                        (
                            g => g.Header == header
                        );
                    if (ReferenceEquals(bookGroup, null))
                    {
                        bookGroup = new Multivolume
                        {
                            Header = header
                        };
                        Groups.Add(bookGroup);
                    }
                    bookGroup.Add(item);
                }

                orderFormat = processor.GetText(context, orderFormat)
                    .ThrowIfNull("orderFormat");
                formatter.ParseProgram(orderFormat);

                foreach (var bookGroup in Groups)
                {
                    var record = bookGroup.First().Record
                        .ThrowIfNull("bookGroup.Record");
                    var order = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(order))
                    {
                        order = order.Trim();
                        order = CleanOrder(order);
                    }
                    bookGroup.Order = order;
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Order).ToList();
            items.Clear();
            foreach (var bookGroup in Groups)
            {
                OrderGroup(bookGroup);
                var item = new BiblioItem
                {
                    Description = bookGroup.Header,
                    Record = new Record(), // TODO ???
                    UserData = bookGroup
                };
                bookGroup.Item = item;
                items.Add(item);
            }


            log.WriteLine("End grouping {0}", this);
        }

        /// <inheritdoc cref="MenuSubChapter.Render" />
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

            var showOrder = false;
                // TODO: implement
                /* context.Document.CommonSettings.Value<bool?>("showOrder") ?? false; */

            RenderTitle(context);

            foreach (var bookGroup in Groups)
            {
                var header = bookGroup.Header;
                if (!string.IsNullOrEmpty(header))
                {
                    log.WriteLine(header);
                }

                header = RichText.Encode3(header, UnicodeRange.Russian, "\\f2");

                report.Body.Add(new ParagraphBand());
                var item = bookGroup.Item
                    .ThrowIfNull("bookGroup.Item");
                var number = item.Number;
                ReportBand band = new ParagraphBand
                    (
                        number.ToInvariantString() + ".\\~\\~"
                        + header
                    );
                report.Body.Add(band);

                // Для отладки: проверить упорядочение
                if (showOrder)
                {
                    if (!string.IsNullOrEmpty(bookGroup.Order))
                    {
                        band = new ParagraphBand(bookGroup.Order);
                        report.Body.Add(band);
                        report.Body.Add(new ParagraphBand());
                    }
                }

                if (!bookGroup.Single)
                {
                    for (var i = 0; i < bookGroup.Count; i++)
                    {
                        log.Write(".");
                        item = bookGroup[i];
                        var description = item.Description.ThrowIfNull("item.Description");
                        description = RichText.Encode3 ( description, UnicodeRange.Russian, "\\f2" )
                            .ThrowIfNull("item.Description");
                        band = new ParagraphBand(description);
                        report.Body.Add(band);
                    }
                }

                log.WriteLine(" done");
            }

            RenderDuplicates(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion
    }
}
