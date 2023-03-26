// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChapterWithDictionary.cs -- глава со словарем
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using AM;
using AM.Linq;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Глова со словарем.
/// </summary>
[PublicAPI]
public class ChapterWithDictionary
    : BiblioChapter
{
    #region Properties

    /// <summary>
    /// Словарь.
    /// </summary>
    public BiblioDictionary Dictionary { get; private set; }

    /// <summary>
    /// Коллекция терминов.
    /// </summary>
    public TermCollection Terms { get; private set; }

    /// <summary>
    /// Выражение, используемое для упорядочения элементjd словаря.
    /// </summary>
    [JsonProperty ("orderBy")]
    public string? OrderByClause { get; set; }

    /// <summary>
    /// Выражение, используемое для отбора элементов словаря.
    /// </summary>
    [JsonProperty ("select")]
    public string? SelectClause { get; set; }

    /// <summary>
    /// Расширенный формат.
    /// </summary>
    [JsonProperty ("extended")]
    public string? ExtendedFormat { get; set; }

    /// <summary>
    /// Entries to exclude.
    /// </summary>
    [JsonProperty ("exclude")]
    public List<string> ExcludeList { get; private set; }

    /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
    public override bool IsServiceChapter => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChapterWithDictionary()
    {
        Dictionary = new ();
        Terms = new ();
        ExcludeList = new ();
    }

    #endregion

    #region Private members

    private static readonly char[] _charactersToTrim = { '[', ']' };

    private static readonly char[] _lineDelimiters = { '\r', '\n', '\u001F' };

    private void _ChapterToTerms
        (
            BiblioContext context,
            BiblioChapter chapter
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (chapter);

        var log = context.Log;
        var processor = context.Processor.ThrowIfNull();

        if (Settings is { } settings)
        {
            var pattern = settings.GetSetting ("chapterFilter");
            if (!string.IsNullOrEmpty (pattern))
            {
                if (chapter.Title is { } title)
                {
                    if (!Regex.IsMatch (title, pattern))
                    {
                        log.WriteLine ("Filtered");

                        return;
                    }
                }
            }
        }

        if (chapter.Items is { Count: not 0 } items)
        {
            log.WriteLine ("Gather terms from chapter {0}", chapter);

            var mfns = items.Select (i => i.Record?.Mfn ?? 0)
                .Where (mfn => mfn > 0)
                .ToArray();
            if (mfns.Length == 0)
            {
                goto DONE;
            }

            if (mfns.Length != items.Count)
            {
                throw new IrbisException();
            }

            var termCount = 0;
            using (var formatter
                   = processor.AcquireFormatter (context))
            {
                var select = SelectClause.ThrowIfNull();
                var format = processor.GetText (context, select).ThrowIfNull();
                formatter.ParseProgram (format);

                var formatted = formatter.FormatRecords (mfns);
                var formatted2 = new string[mfns.Length];

                var extendedFormat = ExtendedFormat;
                if (!string.IsNullOrEmpty (extendedFormat))
                {
                    extendedFormat = processor.GetText
                            (
                                context,
                                extendedFormat
                            )
                        .ThrowIfNull (nameof (extendedFormat));
                    formatter.ParseProgram (extendedFormat);
                    formatted2 = formatter.FormatRecords (mfns);
                }

                for (var i = 0; i < items.Count; i++)
                {
                    if (!string.IsNullOrEmpty (formatted[i]))
                    {
                        var lines = formatted[i]
                            .Split (_lineDelimiters)
                            .TrimLines()
                            .TrimLines (_charactersToTrim)
                            .NonEmptyLines()
                            .Distinct()
                            .ToArray();
                        var lines2 = new string[lines.Length];
                        if (!string.IsNullOrEmpty (formatted2[i]))
                        {
                            lines2 = formatted2[i]
                                .Split (_lineDelimiters)
                                .TrimLines()
                                .TrimLines (_charactersToTrim)
                                .NonEmptyLines()
                                .Distinct()
                                .ToArray();
                        }

                        for (var j = 0; j < lines.Length && j < lines2.Length; j++)
                        {
                            var line1 = lines[j];
                            var line2 = lines2[j];
                            if (!ExcludeList.Contains (line1))
                            {
                                var term = new BiblioTerm
                                {
                                    Title = line1,
                                    Extended = line2,
                                    Dictionary = Terms,
                                    Item = items[i]
                                };
                                Terms.Add (term);
                                termCount++;
                            }
                        }
                    }
                }
            }

            log.WriteLine (" done");
            log.WriteLine ("Term count: {0}", termCount);
        }

        DONE:

        foreach (var child in chapter.Children)
        {
            _ChapterToTerms (context, child);
        }
    }

    #endregion

    #region BiblioChapter members

    /// <inheritdoc cref="BiblioChapter.BuildDictionary" />
    public override void BuildDictionary
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin build dictionary {0}", this);

        foreach (var term in Terms)
        {
            if (term is { Title: { } title, Item: { } item })
            {
                Dictionary.Add (title, item.Number);
            }
        }

        log.WriteLine ("End build dictionary {0}", this);
    }

    /// <inheritdoc cref="BiblioChapter.GatherTerms" />
    public override void GatherTerms
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin gather terms {0}", this);

        if (Active)
        {
            try
            {
                var document = context.Document;

                foreach (var chapter in document.Chapters)
                {
                    _ChapterToTerms (context, chapter);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine ("Exception: {0}", exception);
                throw;
            }
        }


        log.WriteLine ("End gather terms {0}", this);
    }

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

        report.Body.Add (new NewPageBand());
        RenderTitle (context);

        var keys = Dictionary.Keys.ToArray();
        var items = keys.Select (CleanOrder).ToArray();
        Array.Sort (items, keys); //-V3066
        var builder = StringBuilderPool.Shared.Get();
        foreach (var key in keys)
        {
            log.Write (".");
            builder.Clear();
            var entry = Dictionary[key];
            var band = new ParagraphBand();
            report.Body.Add (band);

            var title = entry.Title;
            if (string.IsNullOrEmpty (title))
            {
                continue;
            }

            if (!string.IsNullOrEmpty (ExtendedFormat))
            {
                var maxLength = 0;
                string? longest = null;
                foreach (var term in Terms)
                {
                    if (term.Title == title)
                    {
                        var candidate = term.Extended;
                        if (!string.IsNullOrEmpty (candidate))
                        {
                            var length = candidate.Length;
                            if (length > maxLength)
                            {
                                maxLength = length;
                                longest = candidate;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty (longest)
                    && longest.StartsWith (title))
                {
                    title = longest;
                }
            }

            builder.Append (title);
            builder.Append (" {\\i ");
            var refs = entry.References
                .Where (item => item > 0)
                .ToArray();
            Array.Sort (refs);
            var first = true;
            foreach (var reference in refs)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (reference);
                first = false;
            }

            builder.Append ('}');

            var description = builder.ReturnShared();
            if (!string.IsNullOrEmpty (description))
            {
                // TODO implement properly!!!
                var encoded = RichText.Encode3 (builder.ToString(), UnicodeRange.Russian, "\\f2");
                if (!string.IsNullOrEmpty (encoded))
                {
                    band.Cells.Add (new SimpleTextCell (encoded));
                }
            }
        }

        log.WriteLine (" done");

        RenderChildren (context);

        log.WriteLine ("End render {0}", this);
    }

    #endregion

    #region IVerifiable mebers

    /// <inheritdoc cref="BiblioChapter.Verify" />
    public override bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ChapterWithDictionary> (this, throwOnError);

        verifier
            .Assert (base.Verify (throwOnError))
            .VerifySubObject (Terms);

        return verifier.Result;
    }

    #endregion
}
