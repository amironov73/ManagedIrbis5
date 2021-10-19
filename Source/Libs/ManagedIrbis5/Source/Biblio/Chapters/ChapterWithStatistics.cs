// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChapterWithStatistics.cs -- глава со статистикой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Глава со статистикой.
    /// </summary>
    public sealed class ChapterWithStatistics
        : BiblioChapter
    {
        #region Properties

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter => true;

        #endregion

        #region Private members

        private int _total;

        private void _ProcessChapter
            (
                IrbisReport report,
                BiblioChapter chapter
            )
        {
            var items = chapter.Items;
            if (!ReferenceEquals (items, null))
            {
                var count = items.Count;
                if (!chapter.IsServiceChapter)
                {
                    var text = $"{chapter.Title}\\tab\\~ {{\\b {count}}}";
                    var band = new ParagraphBand (text);
                    report.Body.Add (band);
                    _total += count;
                }
            }

            foreach (var child in chapter.Children)
            {
                _ProcessChapter (report, child);
            }

        } // method ProcessChapter

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            var log = context.Log;
            log.WriteLine ($"Begin render {this}");
            var document = context.Document;
            var processor = context.Processor.ThrowIfNull();
            var report = processor.Report.ThrowIfNull();
            var badRecords = context.BadRecords;

            RenderTitle (context);

            _total = 0;
            string text;
            if (badRecords.Count != 0)
            {
                text = $"ВНЕ РАЗДЕЛОВ:\\tab\\~ {{\\b {badRecords.Count.ToInvariantString()}}}";
                report.Body.Add (new ParagraphBand (text));
            }

            foreach (var chapter in document.Chapters)
            {
                _ProcessChapter (report, chapter);
            }

            report.Body.Add (new ParagraphBand());
            text = $"ВСЕГО:\\tab\\~ {{\\b {_total.ToInvariantString()}}}";
            report.Body.Add (new ParagraphBand (text));

            RenderChildren (context);

            log.WriteLine ("End render {0}", this);

        } // method Render

        #endregion

    } // class ChapterWithStatistics

} // namespace ManagedIrbis.Biblio
