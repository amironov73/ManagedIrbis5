// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Parsing.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

public partial class HierarchyPosition
{
    /// <summary>
    /// Parses a string to a HierarchyPosition
    /// </summary>
    /// <param name="source">String to parse</param>
    /// <param name="separators">Separators between integers in string</param>
    /// <returns></returns>
    public static HierarchyPosition? TryParse
        (
            string source,
            params string[] separators
        )
    {
        var parts = source.Split (separators, StringSplitOptions.RemoveEmptyEntries);
        var values = new List<int>();

        foreach (var part in parts)
        {
            if (!int.TryParse (part, out var value))
            {
                return null;
            }

            values.Add (value);
        }

        return new HierarchyPosition (values);
    }
}
