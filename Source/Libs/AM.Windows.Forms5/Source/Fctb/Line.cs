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
    /// <summary>
    /// Символы, образующие строку.
    /// </summary>
    protected List<Character> chars;

    #region Properties

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
                {
                    spacesCount++;
                }
                else
                {
                    break;
                }

            return spacesCount;
        }
    }

    /// <summary>
    /// Маркер начала свертываемого региона.
    /// </summary>
    public string? FoldingStartMarker { get; set; }

    /// <summary>
    /// Маркер конца свертываемого региона.
    /// </summary>
    public string? FoldingEndMarker { get; set; }

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
    public Brush? BackgroundBrush { get; set; }

    /// <summary>
    /// Unique ID
    /// </summary>
    public int UniqueId { get; private set; }

    /// <summary>
    /// Count of needed start spaces for AutoIndent
    /// </summary>
    public int AutoIndentSpacesNeededCount { get; set; }

    /// <summary>
    /// Text of the line
    /// </summary>
    public virtual string Text
    {
        get
        {
            var sb = new StringBuilder (Count);
            foreach (var c in this)
            {
                sb.Append (c.c);
            }

            return sb.ToString();
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="uid"></param>
    internal Line
        (
            int uid
        )
    {
        UniqueId = uid;
        chars = new List<Character>();
    }

    #endregion

    #region Public methods

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
    /// Clears folding markers
    /// </summary>
    public void ClearFoldingMarkers()
    {
        FoldingStartMarker = null;
        FoldingEndMarker = null;
    }

    /// <summary>
    /// Удаление нескольких символов.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public virtual void RemoveRange (int index, int count)
    {
        if (index >= Count)
        {
            return;
        }

        chars.RemoveRange (index, Math.Min (Count - index, count));
    }

    /// <summary>
    /// Добавление множества символов.
    /// </summary>
    public virtual void AddRange
        (
            IEnumerable<Character> collection
        )
    {
        chars.AddRange (collection);
    }

    #endregion

    #region IList<T> members

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    public int IndexOf (Character item)
    {
        return chars.IndexOf (item);
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert (int index, Character item)
    {
        chars.Insert (index, item);
    }

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    public void RemoveAt (int index)
    {
        chars.RemoveAt (index);
    }

    /// <inheritdoc cref="IList{T}.this"/>
    public Character this [int index]
    {
        get => chars[index];
        set => chars[index] = value;
    }

    #endregion

    #region ICollection<T> members

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public void Add (Character item)
    {
        chars.Add (item);
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear()
    {
        chars.Clear();
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains (Character item)
    {
        return chars.Contains (item);
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo (Character[] array, int arrayIndex)
    {
        chars.CopyTo (array, arrayIndex);
    }

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count => chars.Count;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => false;

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove (Character item)
    {
        return chars.Remove (item);
    }

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<Character> GetEnumerator()
    {
        return chars.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return chars.GetEnumerator();
    }


    #endregion
}
