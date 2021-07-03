// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SiberianColumnCollection.cs -- коллекция колонок грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Коллекция колонок грида.
    /// </summary>
    public sealed class SiberianColumnCollection
        : NonNullCollection<SiberianColumn>,
        ISiberianColumnCollection
    {
    } // class SiberianColumnCollection

} // namespace ManagedIrbis.WinForms.Grid
