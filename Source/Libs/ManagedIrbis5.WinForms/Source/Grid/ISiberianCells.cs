// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* ISiberianCells.cs -- список ячеек, принадлежащих строке грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Список ячеек, принадлежащих строке грида.
    /// </summary>
    public interface ISiberianCells
        : IList<SiberianCell>
    {

    } // interface ISiberianCells

} // namespace ManagedIrbis.WinForms.Grid
