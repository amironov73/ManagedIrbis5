// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IVirtualAdapter.cs -- интерфейс адаптера для виртуального режима грида
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Интерфейс адаптера для виртуального режима грида.
    /// </summary>
    public interface IVirtualAdapter<T>
    {
        #region Properties

        /// <summary>
        /// Предпочитаемая порция данных.
        /// </summary>
        int PreferredPortion => 256;

        /// <summary>
        /// Общая длина данных в строках.
        /// </summary>
        int TotalLength { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение данных.
        /// </summary>
        VirtualData<T>? ReadData(int firstLine, int lineCount);

        #endregion

    } // interface IVirtialAdapter

} // namespace ManagedIrbis.WinForms.Grid
