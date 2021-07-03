// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* VirtualData.cs -- данные, предоставляемые адаптером для виртуального режима грида
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Данные, предоставляемые адаптером для виртуального режима грида.
    /// </summary>
    public sealed class VirtualData<T>
    {
        #region Properties

        /// <summary>
        /// Номер первой строки.
        /// </summary>
        public int FirstLine { get; init; }

        /// <summary>
        /// Реальное количество строк.
        /// </summary>
        public int Length { get; init; }

        /// <summary>
        /// Прочитанные строки данных.
        /// </summary>
        public T[]? Data { get; init; }

        #endregion

    } // class VirtualData

} // namespace ManagedIrbis.WinForms.Grid
