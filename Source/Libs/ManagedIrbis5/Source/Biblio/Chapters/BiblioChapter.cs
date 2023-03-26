// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BiblioChapter.cs -- глава библиографического указателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Newtonsoft.Json;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Reports;

using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Базовый класс для глав библиографического указателя.
/// </summary>
[PublicAPI]
public class BiblioChapter
    : IAttributable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Активна ли данная глава?
    /// Главы можно временно отключать
    /// по каким-либо соображениям.
    /// </summary>
    [JsonProperty ("active")]
    public bool Active { get; set; }

    /// <inheritdoc cref="IAttributable.Attributes" />
    [JsonProperty ("attr")]
    public ReportAttributes Attributes { get; private set; }

    /// <summary>
    /// Дочерние главы (не элементы!).
    /// Дочерние главы неактивной главы не рендерятся.
    /// </summary>
    [JsonProperty ("children")]
    public ChapterCollection Children { get; private set; }

    /// <summary>
    /// Заголавие главы.
    /// </summary>
    [JsonProperty ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Родительская глава (опционально).
    /// </summary>
    [JsonIgnore]
    public BiblioChapter? Parent { get; internal set; }

    /// <summary>
    /// Элементы, составляющие данную главу
    /// (не дочерние главы!).
    /// </summary>
    [JsonIgnore]
    public ItemCollection? Items { get; protected internal set; }

    /// <summary>
    /// Предназначена ли глава для служебных целей?
    /// </summary>
    [JsonIgnore]
    public virtual bool IsServiceChapter => false;

    /// <summary>
    /// Специальные настройки, связанные с главой
    /// и ее дочерними элементами.
    /// </summary>
    [JsonProperty ("settings")]
    public SpecialSettings? Settings { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BiblioChapter()
    {
        Active = true;
        Attributes = new ReportAttributes();
        Children = new ChapterCollection (this);
        Settings = new SpecialSettings();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Очищаем ключ упорядочивания записи.
    /// </summary>
    protected internal string CleanOrder
        (
            string text
        )
    {
        Sure.NotNull (text);

        var builder = StringBuilderPool.Shared.Get();
        builder.Append (text);
        builder.Replace ("[", string.Empty);
        builder.Replace ("]", string.Empty);
        builder.Replace ("\"", string.Empty);
        builder.Replace ("«", string.Empty);
        builder.Replace ("»", string.Empty);
        builder.Replace ("...", string.Empty);

        return builder.ReturnShared();
    }

    /// <summary>
    /// Get property value.
    /// </summary>
    protected TResult? GetProperty<TChapter, TResult>
        (
            Func<TChapter, TResult> func
        )
        where TChapter : BiblioChapter
    {
        for (var chapter = this; chapter is not null; chapter = chapter.Parent)
        {
            if (chapter is TChapter subChapter)
            {
                if (func (subChapter) is { } result)
                {
                    return result;
                }
            }
        }

        return default;
    }

    /// <summary>
    /// Рендеринг дочерник глав.
    /// </summary>
    protected virtual void RenderChildren
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        foreach (var child in Children)
        {
            if (child.Active)
            {
                child.Render (context);
            }
        }
    }

    /// <summary>
    /// Render the chapter title.
    /// </summary>
    protected virtual void RenderTitle
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var processor = context.Processor.ThrowIfNull();
        var report = processor.Report.ThrowIfNull();

        if (!string.IsNullOrEmpty (Title))
        {
            var title = new ParagraphBand
            {
                StyleSpecification = @"\s1\plain\f1\fs40\sb400\sa400\b "
            };
            report.Body.Add (title);
            title.Cells.Add (new SimpleTextCell (Title));
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение словарей.
    /// </summary>
    public virtual void BuildDictionary
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin build dictionaries {0}", this);

        foreach (var child in Children)
        {
            if (child.Active)
            {
                child.BuildDictionary(context);
            }
        }

        log.WriteLine ("End build dictionaries {0}", this);
    }

    /// <summary>
    /// Построение элементов главы <see cref="BiblioItem"/>s.
    /// </summary>
    public virtual void BuildItems
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin build items {0}", this);

        foreach (var child in Children)
        {
            if (child.Active)
            {
                child.BuildItems(context);
            }
        }

        log.WriteLine ("End build items {0}", this);
    }

    /// <summary>
    /// Очистка собранных записей.
    /// </summary>
    public virtual void CleanRecords
        (
            BiblioContext context,
            RecordCollection records
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (records);

        var document = context.Document.ThrowIfNull();
        var array = (JArray?) document.CommonSettings
            .SelectToken("$.removeTags");
        var tags = Array.Empty<int>();
        if (array is not null)
        {
            tags = array.ToObject<int[]>().ThrowIfNull();
        }

        if (tags.Length == 0)
        {
            return;
        }

        foreach (var record in records)
        {
            foreach (var tag in tags)
            {
                record.RemoveField (tag);
            }
        }
    }

    /// <summary>
    /// Gather terms.
    /// </summary>
    public virtual void GatherTerms
        (
            BiblioContext context
        )
    {
        var log = context.Log;
        log.WriteLine ("Begin gather terms {0}", this);

        foreach (var child in Children)
        {
            if (child.Active)
            {
                child.GatherTerms (context);
            }
        }

        log.WriteLine ("End gather terms {0}", this);
    }

    /// <summary>
    /// Gather records.
    /// </summary>
    public virtual void GatherRecords
        (
            BiblioContext context
        )
    {
        var log = context.Log;
        log.WriteLine ("Begin gather records {0}", this);

        foreach (var child in Children)
        {
            if (child.Active)
            {
                child.GatherRecords (context);
            }
        }

        log.WriteLine ("End gather records {0}", this);
    }

    /// <summary>
    /// Initialize the chapter.
    /// </summary>
    public virtual void Initialize
        (
            BiblioContext context
        )
    {
        var log = context.Log;
        log.WriteLine ("Begin initialize {0}", this);

        foreach (var child in Children)
        {
            // Give the chapter a chance
            child.Initialize (context);
        }

        log.WriteLine ("End initialize {0}", this);
    }

    /// <summary>
    /// Number items.
    /// </summary>
    public virtual void NumberItems
        (
            BiblioContext context
        )
    {
        var items = Items;

        if (items is not null)
        {
            foreach (var item in items)
            {
                item.Number = ++context.ItemCount;
            }
        }

        foreach (var child in Children)
        {
            child.NumberItems (context);
        }
    }

    /// <summary>
    /// Render the chapter.
    /// </summary>
    public virtual void Render
        (
            BiblioContext context
        )
    {
        var log = context.Log;
        log.WriteLine ("Begin render items {0}", this);

        RenderTitle (context);
        RenderChildren (context);

        log.WriteLine ("End render items {0}", this);
    }

    /// <summary>
    /// Walk over the chapter and its children.
    /// </summary>
    public void Walk
        (
            Action<BiblioChapter> action
        )
    {
        Sure.NotNull (action);

        action (this);
        foreach (var child in Children)
        {
            child.Walk (action);
        }
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public virtual bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BiblioChapter> (this, throwOnError);

        verifier
            .NotNull (Children)
            .VerifySubObject (Children);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"{GetType().Name}: {Title.ToVisibleString()}";
    }

    #endregion
}
