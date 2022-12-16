// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BiDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils;

/// <summary>
/// A simple bi-directional dictionary
/// </summary>
/// <typeparam name="T1">Key type</typeparam>
/// <typeparam name="T2">Value type</typeparam>
internal class BiDictionary<T1, T2>
    where T1: notnull
    where T2: notnull
{
    public Dictionary<T1, T2> Forward = new ();
    public Dictionary<T2, T1> Reverse = new ();

    public void Clear()
    {
        Forward.Clear();
        Reverse.Clear();
    }

    public void Add (T1 key, T2 value)
    {
        Forward.Add (key, value);
        Reverse.Add (value, key);
    }

    public bool TryGetValue (T1 key, out T2? value)
    {
        return Forward.TryGetValue (key, out value);
    }

    public bool TryGetKey (T2 value, out T1? key)
    {
        return Reverse.TryGetValue (value, out key);
    }

    public bool ContainsKey (T1 key)
    {
        return Forward.ContainsKey (key);
    }

    public bool ContainsValue (T2 value)
    {
        return Reverse.ContainsKey (value);
    }
}
