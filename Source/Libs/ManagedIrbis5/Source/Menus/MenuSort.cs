// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MenuSort.cs -- виды сортировки ИРБИС-меню
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Виды сортировки ИРБИС-меню.
    /// </summary>
    public enum MenuSort
    {
        /// <summary>
        /// Без сортировки.
        /// </summary>
        None,

        /// <summary>
        /// Сортировка по кодам.
        /// </summary>
        ByCode,

        /// <summary>
        /// Сортировка по комментариям.
        /// </summary>
        ByComment
    }
}
