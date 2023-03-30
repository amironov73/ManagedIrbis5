// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChapterWithText.cs -- глава с фиксированным текстом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text.Json.Serialization;

using AM;
using AM.Linq;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Глава с фиксированным текстом.
/// </summary>
[PublicAPI]
public sealed class ChapterWithText
    : BiblioChapter
{
    #region Properties

    /// <summary>
    /// Собственно текст, составляющий содержание главы.
    /// </summary>
    [JsonPropertyName ("text")]
    public string? Text { get; set; }

    /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
    public override bool IsServiceChapter => true;

    #endregion

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

        var processor = context.Processor.ThrowIfNull();
        var report = processor.Report.ThrowIfNull();

        RenderTitle (context);

        var text = Text;
        if (!string.IsNullOrEmpty (text))
        {
            text = processor.GetText (context, text);
            if (!string.IsNullOrEmpty (text))
            {
                var lines = text.SplitLines()
                    .NonEmptyLines()
                    .ToArray();
                foreach (var line in lines)
                {
                    ReportBand band = new ParagraphBand();
                    report.Body.Add (band);
                    band.Cells.Add (new SimpleTextCell (line));
                }
            }
        }

        RenderChildren (context);

        log.WriteLine ("End render {0}", this);
    }

    #endregion
}
