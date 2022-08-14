// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PairComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

internal sealed class PairComparer
    : IComparer<KeyValuePair<int, DataGridViewColumn>>
{
    #region IComparer<KeyValuePair<int,DataGridViewColumn>> members

    public int Compare
        (
            KeyValuePair<int, DataGridViewColumn> x,
            KeyValuePair<int, DataGridViewColumn> y
        )
    {
        return x.Key - y.Key;
    }

    #endregion
}
