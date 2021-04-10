// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChapterWithRecords.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
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
    public class ChapterWithRecords
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Records.
        /// </summary>
        public RecordCollection Records { get; private set; }

        /// <summary>
        /// Duplicates.
        /// </summary>
        public RecordCollection Duplicates { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterWithRecords()
        {
            Records = new RecordCollection();
            Duplicates = new RecordCollection();
        }

        #endregion

        #region Private members

        private BiblioItem? _FindItem
            (
                MenuSubChapter chapter,
                Record record
            )
        {
            if (!ReferenceEquals(chapter.Items, null))
            {
                foreach (var item in chapter.Items)
                {
                    if (ReferenceEquals(item.Record, record))
                    {
                        return item;
                    }
                }
            }

            foreach (var child in chapter.Children)
            {
                var subChapter = child as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    var found = _FindItem(subChapter, record);
                    if (!ReferenceEquals(found, null))
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        private BiblioItem? _FindItem
            (
                Record record
            )
        {
            BiblioChapter rootChapter = this;
            while (!ReferenceEquals(rootChapter.Parent, null))
            {
                rootChapter = rootChapter.Parent;
            }

            foreach (var child in rootChapter.Children)
            {
                var chapter = child as MenuSubChapter;
                if (!ReferenceEquals(chapter, null))
                {
                    var found = _FindItem(chapter, record);
                    if (!ReferenceEquals(found, null))
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Format records.
        /// </summary>
        public string[] FormatRecords
            (
                BiblioContext context,
                int[] mfns,
                string format
            )
        {
            if (mfns.Length == 0)
            {
                return Array.Empty<string>();
            }

            var processor = context.Processor
                .ThrowIfNull("context.Processor");

            using var formatter = processor.AcquireFormatter(context);
            formatter.ParseProgram(format);
            var formatted = formatter.FormatRecords(mfns);
            if (formatted.Length != mfns.Length)
            {
                throw new IrbisException();
            }

            return formatted;
        }

        /// <summary>
        /// Format records.
        /// </summary>
        public string[] FormatRecords
            (
                BiblioContext context,
                string format
            )
        {
            var records = Records .ThrowIfNull(nameof(Records));
            var mfns = records.Select(r => r.Mfn).ToArray();
            var result = FormatRecords(context, mfns, format);

            return result;
        }

        /// <summary>
        /// Render duplicates.
        /// </summary>
        protected void RenderDuplicates
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            var processor = context.Processor
                .ThrowIfNull("context.Processor");
            var report = processor.Report
                .ThrowIfNull("processor.Report");

            if (Duplicates.Count != 0)
            {
                var items
                    = new List<BiblioItem>(Duplicates.Count);
                foreach (var dublicate in Duplicates)
                {
                    var item = _FindItem(dublicate);
                    if (!ReferenceEquals(item, null))
                    {
                        items.Add(item);
                    }
                    else
                    {
                        log.WriteLine
                            (
                                "Проблема с дубликатом MFN="
                                + dublicate.Mfn
                            );
                    }
                }
                items = items
                    .OrderBy(x => x.Number)
                    .Distinct()
                    .ToList();

                var builder = new StringBuilder();
                builder.Append("См. также: {\\i ");
                var first = true;
                foreach (var item in items)
                {
                    if (!first)
                    {
                        builder.Append(", ");
                    }
                    builder.Append(item.Number.ToInvariantString());
                    first = false;
                }
                builder.Append('}');

                report.Body.Add(new ParagraphBand());
                report.Body.Add(new ParagraphBand(builder.ToString()));
            }
        }


        #endregion

    }
}
