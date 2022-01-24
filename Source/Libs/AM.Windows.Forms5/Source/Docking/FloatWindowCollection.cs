// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

public class FloatWindowCollection : ReadOnlyCollection<FloatWindow>
{
    internal FloatWindowCollection()
        : base (new List<FloatWindow>())
    {
    }

    internal int Add (FloatWindow fw)
    {
        if (Items.Contains (fw))
            return Items.IndexOf (fw);

        Items.Add (fw);
        return Count - 1;
    }

    internal void Dispose()
    {
        for (int i = Count - 1; i >= 0; i--)
            this[i].Close();
    }

    internal void Remove (FloatWindow fw)
    {
        Items.Remove (fw);
    }

    internal void BringWindowToFront (FloatWindow fw)
    {
        Items.Remove (fw);
        Items.Add (fw);
    }
}