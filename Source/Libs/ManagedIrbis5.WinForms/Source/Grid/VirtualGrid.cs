// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* VirtualGrid.cs -- грид в виртуальном режиме
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Грид в виртуальном режиме.
    /// </summary>
    /// <typeparam name="T">Тип данных.</typeparam>
    public class VirtualGrid<T>
        : SiberianGrid
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="adapter">Адаптер.</param>
        public VirtualGrid
            (
                IVirtualAdapter<T> adapter
            )
            : base
                (
                    new SiberianColumnCollection(),
                    new VirtualRowCollection<T>(adapter)
                )
        {
            // пустое тело конструктора
        }

        #endregion
    }
}
