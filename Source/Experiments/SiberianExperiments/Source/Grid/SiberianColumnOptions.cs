// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianColumnOptions.cs -- опции колонки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Опции колонки грида.
    /// </summary>
    [Flags]
    public enum SiberianColumnOptions
    {
        /// <summary>
        /// Данные можно редактировать.
        /// </summary>
        Editable = 0x0001,

        /// <summary>
        /// Ячейки данной колонки можно выбрать.
        /// </summary>
        Selectable = 0x0002,

        /// <summary>
        /// Ширину колонки можно менять вручную (мышкой).
        /// </summary>
        Resizeable = 0x0004,

        /// <summary>
        /// Ширина колонки может увеличиваться автоматически.
        /// </summary>
        CanGrowHorizontally = 0x0008,

        /// <summary>
        /// Ширина колонки может уменьшаться автоматически.
        /// </summary>
        CanShrinkHorizontally = 0x0010,

        /// <summary>
        /// Высота ячеек данной колонки может увеличиваться автоматически.
        /// </summary>
        CanGrowVertically = 0x0020,

        /// <summary>
        /// Высота ячеек данной колонки может уменьшаться автоматически.
        /// </summary>
        CanShrinkVertically = 0x0040

    } // enum SiberianColumnOptions

} // namespace ManagedIrbis.WinForms.Grid
