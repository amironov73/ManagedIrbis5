// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChapterCollection.cs -- коллекция глав
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;
using System.Xml.Serialization;

using Newtonsoft.Json;

using AM;
using AM.Collections;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Коллекция глав.
/// </summary>
[PublicAPI]
public sealed class ChapterCollection
    : NonNullCollection<BiblioChapter>,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Родительская глава.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public BiblioChapter? Parent
    {
        get => _parent;
        internal set => SetParent (value);
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChapterCollection()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChapterCollection
        (
            BiblioChapter? parent
        )
    {
        Parent = parent;
    }

    #endregion

    #region Private members

    private BiblioChapter? _parent;

    internal void SetParent
        (
            BiblioChapter? parent
        )
    {
        _parent = parent;
        foreach (var chapter in this)
        {
            chapter.Parent = parent;
        }
    }

    #endregion

    #region NonNullCollection<T> members

    /// <inheritdoc cref="NonNullCollection{T}.InsertItem" />
    protected override void InsertItem
        (
            int index,
            BiblioChapter item
        )
    {
        base.InsertItem (index, item);
        item.Parent = _parent;
    }

    /// <inheritdoc cref="NonNullCollection{T}.SetItem" />
    protected override void SetItem
        (
            int index,
            BiblioChapter item
        )
    {
        base.SetItem (index, item);
        item.Parent = _parent;
    }

    /// <inheritdoc cref="Collection{T}.ClearItems" />
    protected override void ClearItems()
    {
        foreach (var chapter in this)
        {
            chapter.Parent = null;
        }

        base.ClearItems();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem" />
    protected override void RemoveItem
        (
            int index
        )
    {
        if (index >= 0 && index < Count)
        {
            var chapter = this[index];
            chapter.Parent = null;
        }

        base.RemoveItem (index);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ChapterCollection> (this, throwOnError);

        foreach (var chapter in this)
        {
            verifier.VerifySubObject (chapter);
        }

        return verifier.Result;
    }

    #endregion
}
