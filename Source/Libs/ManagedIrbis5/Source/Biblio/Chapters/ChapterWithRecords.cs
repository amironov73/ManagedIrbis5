// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using Newtonsoft.Json;

#endregion

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

        [CanBeNull]
        private BiblioItem _FindItem
            (
                MenuSubChapter chapter,
                Record record
            )
        {
            if (!ReferenceEquals(chapter.Items, null))
            {
                foreach (BiblioItem item in chapter.Items)
                {
                    if (ReferenceEquals(item.Record, record))
                    {
                        return item;
                    }
                }
            }

            foreach (BiblioChapter child in chapter.Children)
            {
                MenuSubChapter subChapter = child as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    BiblioItem found = _FindItem(subChapter, record);
                    if (!ReferenceEquals(found, null))
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        [CanBeNull]
        private BiblioItem _FindItem
            (
                Record record
            )
        {
            BiblioChapter rootChapter = this;
            while (!ReferenceEquals(rootChapter.Parent, null))
            {
                rootChapter = rootChapter.Parent;
            }

            foreach (BiblioChapter child in rootChapter.Children)
            {
                MenuSubChapter chapter = child as MenuSubChapter;
                if (!ReferenceEquals(chapter, null))
                {
                    BiblioItem found = _FindItem(chapter, record);
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
        [ItemNotNull]
        public string[] FormatRecords
            (
                BiblioContext context,
                int[] mfns,
                string format
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(mfns, "mfns");
            Code.NotNullNorEmpty(format, "format");

            if (mfns.Length == 0)
            {
                return StringUtility.EmptyArray;
            }

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                formatter.ParseProgram(format);
                string[] formatted = formatter.FormatRecords(mfns);
                if (formatted.Length != mfns.Length)
                {
                    throw new IrbisException();
                }

                return formatted;
            }
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
            Code.NotNull(context, "context");
            Code.NotNullNorEmpty(format, "format");

            RecordCollection records = Records
                .ThrowIfNull("Records");
            int[] mfns = records.Select(r => r.Mfn).ToArray();
            string[] result = FormatRecords(context, mfns, format);

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
            AbstractOutput log = context.Log;
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            if (Duplicates.Count != 0)
            {
                List<BiblioItem> items
                    = new List<BiblioItem>(Duplicates.Count);
                foreach (Record dublicate in Duplicates)
                {
                    BiblioItem item = _FindItem(dublicate);
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

                StringBuilder builder = new StringBuilder();
                builder.Append("См. также: {\\i ");
                bool first = true;
                foreach (BiblioItem item in items)
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

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
