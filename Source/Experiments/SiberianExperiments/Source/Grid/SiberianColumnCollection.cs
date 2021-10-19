// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianColumnCollection.cs -- дефолтная реализация коллекции колонок грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Дефолтная реализация коллекции колонок грида.
    /// </summary>
    public sealed class SiberianColumnCollection
        : List<ISiberianColumn>,
        ISiberianColumnCollection
    {
        #region List members

        /// <inheritdoc cref="ISiberianColumnCollection.this"/>
        public new ISiberianColumn this[int index] => base[index].ThrowIfNull();

        #endregion

    } // class SiberianColumnCollection

} // namespace ManagedIrbis.WinForms.Grid
