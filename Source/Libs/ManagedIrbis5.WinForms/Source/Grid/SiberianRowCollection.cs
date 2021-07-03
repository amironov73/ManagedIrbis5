// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SiberianRowCollection.cs -- коллекция строк грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Коллекция строк грида.
    /// </summary>
    public sealed class SiberianRowCollection
        : NonNullCollection<SiberianRow>,
        ISiberianRowCollection
    {
    } // class SiberianRowCollection

} // namespace ManagedIrbis.WinForms.Grid
