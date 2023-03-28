// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChapterWithErrors.cs -- глава с перечислением ошибок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Глава с перечислением ошибок.
/// </summary>
[PublicAPI]
public sealed class ChapterWithErrors
    : BiblioChapter
{
    #region BiblioChapter members

    /// <inheritdoc cref="BiblioChapter.Render" />
    public override void Render
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin render {0}", this);

        var badRecords = context.BadRecords;
        var processor = context.Processor.ThrowIfNull();
        var report = processor.Report.ThrowIfNull();

        RenderTitle (context);

        if (badRecords.Count != 0)
        {
            var title = new ParagraphBand
                (
                    "Следующие записи не входят ни в один раздел"
                );
            report.Body.Add (title);
            report.Body.Add (new ParagraphBand());

            using var formatter = processor.AcquireFormatter (context);
            var briefFormat = processor
                .GetText (context, "*brief.pft")
                .ThrowIfNull ("processor.GetText");
            formatter.ParseProgram (briefFormat);

            foreach (var record in badRecords)
            {
                log.Write (".");
                var description =
                    "MFN " + record.Mfn + " "
                    + formatter.FormatRecord (record.Mfn);
                var band
                    = new ParagraphBand (description);
                report.Body.Add (band);
                report.Body.Add (new ParagraphBand());
            }

            log.WriteLine (" done");

            processor.ReleaseFormatter (context, formatter);
        }

        RenderChildren (context);

        log.WriteLine ("End render {0}", this);
    }

    #endregion
}
