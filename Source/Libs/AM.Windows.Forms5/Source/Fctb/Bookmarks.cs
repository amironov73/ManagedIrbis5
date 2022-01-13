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

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Коллекция закладок.
/// </summary>
public class Bookmarks
    : BookmarksBase
{
    protected SyntaxTextBox _textBox;
    protected List<Bookmark> items = new();
    protected int counter;

    public Bookmarks (SyntaxTextBox textBox)
    {
        this._textBox = textBox;
        textBox.LineInserted += tb_LineInserted;
        textBox.LineRemoved += tb_LineRemoved;
    }

    protected virtual void tb_LineRemoved (object sender, LineRemovedEventArgs e)
    {
        for (var i = 0; i < Count; i++)
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
                        was = true;

                if (was)
                {
                    items.RemoveAt (i);
                    i--;
                }
                else
                    items[i].LineIndex = e.Index - 1;

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

    protected virtual void tb_LineInserted (object sender, LineInsertedEventArgs e)
    {
        for (var i = 0; i < Count; i++)
            if (items[i].LineIndex >= e.Index)
            {
                items[i].LineIndex = items[i].LineIndex + e.Count;
            }
            else if (items[i].LineIndex == e.Index - 1 && e.Count == 1)
            {
                if (_textBox[e.Index - 1].StartSpacesCount == _textBox[e.Index - 1].Count)
                    items[i].LineIndex = items[i].LineIndex + e.Count;
            }
    }

    public override void Dispose()
    {
        _textBox.LineInserted -= tb_LineInserted;
        _textBox.LineRemoved -= tb_LineRemoved;
    }

    public override IEnumerator<Bookmark> GetEnumerator()
    {
        foreach (var item in items)
            yield return item;
    }

    public override void Add (int lineIndex, string bookmarkName)
    {
        Add (new Bookmark (_textBox, bookmarkName ?? "Bookmark " + counter, lineIndex));
    }

    public override void Add (int lineIndex)
    {
        Add (new Bookmark (_textBox, "Bookmark " + counter, lineIndex));
    }

    public override void Clear()
    {
        items.Clear();
        counter = 0;
    }

    public override void Add (Bookmark bookmark)
    {
        foreach (var bm in items)
            if (bm.LineIndex == bookmark.LineIndex)
                return;

        items.Add (bookmark);
        counter++;
        _textBox.Invalidate();
    }

    public override bool Contains (Bookmark item)
    {
        return items.Contains (item);
    }

    public override bool Contains (int lineIndex)
    {
        foreach (var item in items)
            if (item.LineIndex == lineIndex)
                return true;
        return false;
    }

    public override void CopyTo (Bookmark[] array, int arrayIndex)
    {
        items.CopyTo (array, arrayIndex);
    }

    public override int Count
    {
        get { return items.Count; }
    }

    public override bool IsReadOnly
    {
        get { return false; }
    }

    public override bool Remove (Bookmark item)
    {
        _textBox.Invalidate();
        return items.Remove (item);
    }

    /// <summary>
    /// Removes bookmark by line index
    /// </summary>
    public override bool Remove (int lineIndex)
    {
        var was = false;
        for (var i = 0; i < Count; i++)
            if (items[i].LineIndex == lineIndex)
            {
                items.RemoveAt (i);
                i--;
                was = true;
            }

        _textBox.Invalidate();

        return was;
    }

    /// <summary>
    /// Returns Bookmark by index.
    /// </summary>
    public override Bookmark GetBookmark (int i)
    {
        return items[i];
    }
}
