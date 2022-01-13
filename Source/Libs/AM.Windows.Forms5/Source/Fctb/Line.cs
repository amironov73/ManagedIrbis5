// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Line.cs -- строка в текстовом редакторе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System;
using System.Text;
using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Строка в текстовом редакторе
/// </summary>
public class Line
    : IList<Character>
{
    protected List<Character> chars;

    #region Properties

    public string FoldingStartMarker { get; set; }
    public string FoldingEndMarker { get; set; }

    /// <summary>
    /// Text of line was changed
    /// </summary>
    public bool IsChanged { get; set; }

    /// <summary>
    /// Time of last visit of caret in this line
    /// </summary>
    /// <remarks>This property can be used for forward/backward navigating</remarks>
    public DateTime LastVisit { get; set; }

    /// <summary>
    /// Background brush.
    /// </summary>
    public Brush BackgroundBrush { get; set; }

    /// <summary>
    /// Unique ID
    /// </summary>
    public int UniqueId { get; private set; }

    /// <summary>
    /// Count of needed start spaces for AutoIndent
    /// </summary>
    public int AutoIndentSpacesNeededCount { get; set; }

    #endregion

    internal Line (int uid)
    {
        this.UniqueId = uid;
        chars = new List<Character>();
    }


    /// <summary>
    /// Clears style of chars, delete folding markers
    /// </summary>
    public void ClearStyle (StyleIndex styleIndex)
    {
        FoldingStartMarker = null;
        FoldingEndMarker = null;
        for (var i = 0; i < Count; i++)
        {
            var c = this[i];
            c.style &= ~styleIndex;
            this[i] = c;
        }
    }

    /// <summary>
    /// Text of the line
    /// </summary>
    public virtual string Text
    {
        get
        {
            var sb = new StringBuilder (Count);
            foreach (var c in this)
                sb.Append (c.c);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Clears folding markers
    /// </summary>
    public void ClearFoldingMarkers()
    {
        FoldingStartMarker = null;
        FoldingEndMarker = null;
    }

    /// <summary>
    /// Count of start spaces
    /// </summary>
    public int StartSpacesCount
    {
        get
        {
            var spacesCount = 0;
            for (var i = 0; i < Count; i++)
                if (this[i].c == ' ')
                    spacesCount++;
                else
                    break;
            return spacesCount;
        }
    }

    public int IndexOf (Character item)
    {
        return chars.IndexOf (item);
    }

    public void Insert (int index, Character item)
    {
        chars.Insert (index, item);
    }

    public void RemoveAt (int index)
    {
        chars.RemoveAt (index);
    }

    public Character this [int index]
    {
        get { return chars[index]; }
        set { chars[index] = value; }
    }

    public void Add (Character item)
    {
        chars.Add (item);
    }

    public void Clear()
    {
        chars.Clear();
    }

    public bool Contains (Character item)
    {
        return chars.Contains (item);
    }

    public void CopyTo (Character[] array, int arrayIndex)
    {
        chars.CopyTo (array, arrayIndex);
    }

    /// <summary>
    /// Chars count
    /// </summary>
    public int Count
    {
        get { return chars.Count; }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove (Character item)
    {
        return chars.Remove (item);
    }

    public IEnumerator<Character> GetEnumerator()
    {
        return chars.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return chars.GetEnumerator() as System.Collections.IEnumerator;
    }

    public virtual void RemoveRange (int index, int count)
    {
        if (index >= Count)
            return;
        chars.RemoveRange (index, Math.Min (Count - index, count));
    }

    public virtual void TrimExcess()
    {
        chars.TrimExcess();
    }

    public virtual void AddRange (IEnumerable<Character> collection)
    {
        chars.AddRange (collection);
    }
}
