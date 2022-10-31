// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HeaderlessTabPageCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// Коллекция <see cref="HeaderlessTabPage"/>.
/// </summary>
public class HeaderlessTabPageCollection
    : IList<HeaderlessTabPage>
{
    /// <summary>
    ///
    /// </summary>
    protected List<HeaderlessTabPage> TabPages { get; set; }

    /// <summary>
    ///
    /// </summary>
    protected HeaderlessTabControl Owner { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tabControl"></param>
    public HeaderlessTabPageCollection
        (
            HeaderlessTabControl tabControl
        )
    {
        Sure.NotNull (tabControl);

        Owner = tabControl;
        TabPages = new List<HeaderlessTabPage>();
    }

    int IList<HeaderlessTabPage>.IndexOf
        (
            HeaderlessTabPage item
        )
    {
        Sure.NotNull (item);

        return TabPages.IndexOf (item);
    }

    void IList<HeaderlessTabPage>.Insert (int index, HeaderlessTabPage item)
    {
        throw new NotImplementedException();
    }

    void IList<HeaderlessTabPage>.RemoveAt (int index)
    {
        throw new NotImplementedException();
    }

    HeaderlessTabPage IList<HeaderlessTabPage>.this [int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    void ICollection<HeaderlessTabPage>.Add (HeaderlessTabPage item)
    {
        throw new NotImplementedException();
    }

    void ICollection<HeaderlessTabPage>.Clear()
    {
        throw new NotImplementedException();
    }

    bool ICollection<HeaderlessTabPage>.Contains (HeaderlessTabPage item)
    {
        throw new NotImplementedException();
    }

    void ICollection<HeaderlessTabPage>.CopyTo (HeaderlessTabPage[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    int ICollection<HeaderlessTabPage>.Count => throw new NotImplementedException();

    bool ICollection<HeaderlessTabPage>.IsReadOnly => throw new NotImplementedException();

    bool ICollection<HeaderlessTabPage>.Remove (HeaderlessTabPage item)
    {
        throw new NotImplementedException();
    }

    IEnumerator<HeaderlessTabPage> IEnumerable<HeaderlessTabPage>.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
