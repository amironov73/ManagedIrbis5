// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GroupingSubChapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Linq;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Группировка документов, например, в авторские комплексы.
    /// </summary>

    public class GroupingSubChapter
        : MenuSubChapter
    {
        #region Nested classes

        /// <summary>
        /// Group of books.
        /// </summary>
        public class BookGroup
            : List<BiblioItem>
        {
            #region Properties

            /// <summary>
            /// Name.
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// Group for "other" records.
            /// </summary>
            public bool OtherGroup { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Groups.
        /// </summary>
        public List<BookGroup> Groups { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GroupingSubChapter()
        {
            Groups = new List<BookGroup>();
        }

        #endregion

        #region Private members

        private static readonly char[] _lineDelimiters = { '\r', '\n' };

        private void _OrderGroup
            (
                BiblioContext context,
                BookGroup bookGroup
            )
        {
            if (!bookGroup.OtherGroup)
            {
                var settings = Settings;
                if (ReferenceEquals(settings, null))
                {
                    return;
                }
                var orderFormat = settings.GetSetting("groupedOrder");
                if (string.IsNullOrEmpty(orderFormat))
                {
                    return;
                }

                var processor = context.Processor.ThrowIfNull("context.Processor");
                using var formatter = processor.AcquireFormatter(context);
                orderFormat = processor.GetText(context, orderFormat).ThrowIfNull("orderFormat");
                formatter.ParseProgram(orderFormat);

                foreach (var item in bookGroup)
                {
                    var record = item.Record
                        .ThrowIfNull("item.Record");
                    var order = formatter.FormatRecord(record.Mfn);
                    //item.Order = RichText.Decode(order);
                    item.Order = order;
                }
            }

            var items = bookGroup
                .OrderBy(item => item.Order)
                .ToArray();
            bookGroup.Clear();
            bookGroup.AddRange(items);
        }

        #endregion

        #region Public methods

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

            var allValues = settings.GetSetting("values");
            if (string.IsNullOrEmpty(allValues))
            {
                return;
            }

            var values = allValues.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (values.Length == 0)
            {
                return;
            }

            var groupBy = settings.GetSetting("groupBy");
            if (string.IsNullOrEmpty(groupBy))
            {
                return;
            }

            var log = context.Log;
            log.WriteLine("Begin grouping {0}", this);

            var otherName = settings.GetSetting("others");
            var others = new BookGroup
            {
                Name = otherName,
                OtherGroup = true
            };

            var processor = context.Processor.ThrowIfNull("context.Processor");
            using (var formatter = processor.AcquireFormatter(context))
            {
                groupBy = processor.GetText(context, groupBy)
                    .ThrowIfNull("groupBy");
                formatter.ParseProgram(groupBy);

                foreach (var item in items)
                {
                    var record = item.Record.ThrowIfNull("item.Record");
                    var text = formatter.FormatRecord(record.Mfn);
                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    var keys = text.Trim()
                        .Split(_lineDelimiters)
                        .TrimLines()
                        .NonEmptyLines()
                        .Distinct()
                        .ToArray();
                    var found = false;
                    foreach (var key in keys)
                    {
                        var theKey = key;
                        if (theKey.IsOneOf(values))
                        {
                            var bookGroup = Groups.FirstOrDefault
                                (
                                    g => g.Name == theKey
                                );
                            if (ReferenceEquals(bookGroup, null))
                            {
                                bookGroup = new BookGroup
                                {
                                    Name = theKey
                                };
                                Groups.Add(bookGroup);
                            }
                            bookGroup.Add(item);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        others.Add(item);
                    }
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Name).ToList();
            foreach (var bookGroup in Groups)
            {
                _OrderGroup(context, bookGroup);
            }
            _OrderGroup(context, others);
            Groups.Add(others);

            log.WriteLine("End grouping {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.NumberItems" />
        public override void NumberItems
            (
                BiblioContext context
            )
        {
            foreach (var bookGroup in Groups)
            {
                foreach (var item in bookGroup)
                {
                    item.Number = ++context.ItemCount;
                }
            }
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

            RenderTitle(context);

            foreach (var bookGroup in Groups)
            {
                var name = bookGroup.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    log.WriteLine(name);
                }

                report.Body.Add(new ParagraphBand());
                var groupTitle =
                    "{\\b "
                    + name
                    + "\\b0}";
                if (!bookGroup.OtherGroup)
                {
                    groupTitle += " (автор, редактор, составитель)";
                }
                ReportBand band = new ParagraphBand(groupTitle);
                report.Body.Add(band);
                report.Body.Add(new ParagraphBand());

                for (var i = 0; i < bookGroup.Count; i++)
                {
                    log.Write(".");
                    var item = bookGroup[i];
                    var number = item.Number;
                    var description = item.Description
                        .ThrowIfNull("item.Description");

                    band = new ParagraphBand
                        (
                            number.ToInvariantString() + ") "
                        );
                    report.Body.Add(band);
                    band.Cells.Add(new SimpleTextCell(description));
                }
                log.WriteLine(" done");
            }

            RenderDuplicates(context);

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
