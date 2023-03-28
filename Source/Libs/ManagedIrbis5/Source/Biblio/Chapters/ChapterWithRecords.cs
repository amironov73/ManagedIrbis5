// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChapterWithRecords.cs -- глава с библиографическими записями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Глава с библиографическими записями.
/// </summary>
[PublicAPI]
public /* not sealed */ class ChapterWithRecords
    : BiblioChapter
{
    #region Properties

    /// <summary>
    /// Библиографические записи.
    /// </summary>
    public RecordCollection Records { get; }

    /// <summary>
    /// Дублеты.
    /// </summary>
    public RecordCollection Duplicates { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChapterWithRecords()
    {
        Records = new ();
        Duplicates = new ();
    }

    #endregion

    #region Private members

    private BiblioItem? _FindItem
        (
            MenuSubChapter chapter,
            Record record
        )
    {
        if (chapter.Items is not null)
        {
            foreach (var item in chapter.Items)
            {
                if (ReferenceEquals (item.Record, record))
                {
                    return item;
                }
            }
        }

        foreach (var child in chapter.Children)
        {
            if (child is MenuSubChapter subChapter)
            {
                if (_FindItem (subChapter, record) is { } found)
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
        while (rootChapter.Parent is not null)
        {
            rootChapter = rootChapter.Parent;
        }

        foreach (var child in rootChapter.Children)
        {
            var chapter = child as MenuSubChapter;
            if (!ReferenceEquals (chapter, null))
            {
                var found = _FindItem (chapter, record);
                if (!ReferenceEquals (found, null))
                {
                    return found;
                }
            }
        }

        return null;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Форматирование записей.
    /// </summary>
    public string[] FormatRecords
        (
            BiblioContext context,
            int[] mfns,
            string format
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (mfns);
        Sure.NotNull (format);

        if (mfns.Length == 0)
        {
            return Array.Empty<string>();
        }

        var processor = context.Processor.ThrowIfNull();
        using var formatter = processor.AcquireFormatter (context);
        formatter.ParseProgram (format);
        var formatted = formatter.FormatRecords (mfns);
        if (formatted.Length != mfns.Length)
        {
            throw new IrbisException();
        }

        return formatted;
    }

    /// <summary>
    /// Форматирование записей.
    /// </summary>
    public string[] FormatRecords
        (
            BiblioContext context,
            string format
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (format);

        var records = Records.ThrowIfNull();
        var mfns = records.Select (r => r.Mfn).ToArray();
        var result = FormatRecords (context, mfns, format);

        return result;
    }

    #endregion

    #region Protected members

    /// <summary>
    /// Вывод дублетов.
    /// </summary>
    protected void RenderDuplicates
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        var processor = context.Processor.ThrowIfNull();
        var report = processor.Report.ThrowIfNull();

        if (Duplicates.Count != 0)
        {
            var items = new List<BiblioItem> (Duplicates.Count);
            foreach (var dublicate in Duplicates)
            {
                var item = _FindItem (dublicate);
                if (!ReferenceEquals (item, null))
                {
                    items.Add (item);
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
                .OrderBy (x => x.Number)
                .Distinct()
                .ToList();

            var builder = StringBuilderPool.Shared.Get();
            builder.Append ("См. также: {\\i ");
            var first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (item.Number.ToInvariantString());
                first = false;
            }

            builder.Append ('}');
            var text = builder.ReturnShared();

            report.Body.Add (new ParagraphBand());
            report.Body.Add (new ParagraphBand (text));
        }
    }

    #endregion
}
