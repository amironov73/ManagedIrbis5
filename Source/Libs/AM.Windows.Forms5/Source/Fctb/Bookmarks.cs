// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Bookmarks.cs -- коллекция закладок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Коллекция закладок.
/// </summary>
public class Bookmarks
    : BookmarksBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Bookmarks
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        this.textBox = textBox;
        textBox.LineInserted += tb_LineInserted;
        textBox.LineRemoved += tb_LineRemoved;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Текстбокс.
    /// </summary>
    protected SyntaxTextBox textBox;

    /// <summary>
    /// Закладки.
    /// </summary>
    protected List<Bookmark> items = new();

    /// <summary>
    /// Счетчик.
    /// </summary>
    protected int counter;

    /// <summary>
    /// Реакция на удаленную строку.
    /// </summary>
    protected virtual void tb_LineRemoved
        (
            object? sender,
            LineRemovedEventArgs e
        )
    {
        for (var i = 0; i < Count; i++)
        {
            if (items[i].LineIndex >= e.Index)
            {
                if (items[i].LineIndex >= e.Index + e.Count)
                {
                    items[i].LineIndex = items[i].LineIndex - e.Count;
                    continue;
                }

                var was = e.Index <= 0;
                foreach (var b in items)
                    if (b.LineIndex == e.Index - 1)
                    {
                        was = true;
                    }

                if (was)
                {
                    items.RemoveAt (i);
                    i--;
                }
                else
                {
                    items[i].LineIndex = e.Index - 1;
                }

                //if (items[i].LineIndex == e.Index + e.Count - 1)
                //{
                //    items[i].LineIndex = items[i].LineIndex - e.Count;
                //    continue;
                //}
                //
                //items.RemoveAt(i);
                //i--;
            }
        }
    }

    /// <summary>
    /// Реакция на вставленную строку
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void tb_LineInserted
        (
            object? sender,
            LineInsertedEventArgs e
        )
    {
        for (var i = 0; i < Count; i++)
        {
            if (items[i].LineIndex >= e.Index)
            {
                items[i].LineIndex = items[i].LineIndex + e.Count;
            }
            else if (items[i].LineIndex == e.Index - 1 && e.Count == 1)
            {
                if (textBox[e.Index - 1].StartSpacesCount == textBox[e.Index - 1].Count)
                {
                    items[i].LineIndex = items[i].LineIndex + e.Count;
                }
            }
        }
    }

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="BookmarksBase.GetEnumerator"/>
    public override IEnumerator<Bookmark> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    #endregion

    #region BookmarksBase members

    /// <inheritdoc cref="BookmarksBase.Clear"/>
    public override void Clear()
    {
        items.Clear();
        counter = 0;
    }

    /// <inheritdoc cref="BookmarksBase.Add(Fctb.Bookmark)"/>
    public override void Add (Bookmark bookmark)
    {
        foreach (var bm in items)
        {
            if (bm.LineIndex == bookmark.LineIndex)
            {
                return;
            }
        }

        items.Add (bookmark);
        counter++;
        textBox.Invalidate();
    }

    /// <inheritdoc cref="BookmarksBase.Contains(Fctb.Bookmark)"/>
    public override bool Contains
        (
            Bookmark item
        )
    {
        return items.Contains (item);
    }

    /// <inheritdoc cref="BookmarksBase.Add(int,string)"/>
    public override void Add
        (
            int lineIndex,
            string? bookmarkName
        )
    {
        Add (new Bookmark (textBox, bookmarkName ?? "Bookmark " + counter, lineIndex));
    }

    /// <inheritdoc cref="BookmarksBase.Add(int)"/>
    public override void Add
        (
            int lineIndex
        )
    {
        Add (new Bookmark (textBox, "Bookmark " + counter, lineIndex));
    }

    /// <inheritdoc cref="BookmarksBase.Contains(int)"/>
    public override bool Contains
        (
            int lineIndex
        )
    {
        foreach (var item in items)
        {
            if (item.LineIndex == lineIndex)
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="BookmarksBase.CopyTo"/>
    public override void CopyTo
        (
            Bookmark[] array,
            int arrayIndex
        )
    {
        items.CopyTo (array, arrayIndex);
    }

    /// <inheritdoc cref="BookmarksBase.Count"/>
    public override int Count => items.Count;

    /// <inheritdoc cref="BookmarksBase.IsReadOnly"/>
    public override bool IsReadOnly => false;

    /// <inheritdoc cref="BookmarksBase.Remove(Fctb.Bookmark)"/>
    public override bool Remove
        (
            Bookmark item
        )
    {
        Sure.NotNull (item);

        textBox.Invalidate();
        return items.Remove (item);
    }

    #endregion

    /// <inheritdoc cref="BookmarksBase.Remove(int)"/>
    public override bool Remove
        (
            int lineIndex
        )
    {
        var was = false;
        for (var i = 0; i < Count; i++)
        {
            if (items[i].LineIndex == lineIndex)
            {
                items.RemoveAt (i);
                i--;
                was = true;
            }
        }

        textBox.Invalidate();

        return was;
    }

    /// <summary>
    /// Returns Bookmark by index.
    /// </summary>
    public override Bookmark GetBookmark (int i)
    {
        return items[i];
    }

    #region IDisposable members

    /// <inheritdoc cref="BookmarksBase.Dispose"/>
    public override void Dispose()
    {
        textBox.LineInserted -= tb_LineInserted;
        textBox.LineRemoved -= tb_LineRemoved;
    }

    #endregion


}
