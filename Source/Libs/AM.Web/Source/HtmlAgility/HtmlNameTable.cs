// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml;

#endregion

#nullable enable

namespace HtmlAgilityPack;

internal class HtmlNameTable
    : XmlNameTable
{
    #region Fields

    private readonly NameTable _nametable = new NameTable();

    #endregion

    #region Public Methods

    public override string Add (string array)
    {
        return _nametable.Add (array);
    }

    public override string Add (char[] array, int offset, int length)
    {
        return _nametable.Add (array, offset, length);
    }

    public override string? Get (string array)
    {
        return _nametable.Get (array);
    }

    public override string? Get (char[] array, int offset, int length)
    {
        return _nametable.Get (array, offset, length);
    }

    #endregion

    #region Internal Methods

    internal string GetOrAdd (string array)
    {
        var s = Get (array);
        if (s == null)
        {
            return Add (array);
        }

        return s;
    }

    #endregion
}
