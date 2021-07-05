// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianNavigationEventArgs.cs -- данные для события навигации в гриде
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Данные для события навигации в гриде.
    /// </summary>
    public sealed class SiberianNavigationEventArgs
        : CancelableEventArgs
    {
        #region Properties

        /// <summary>
        /// Новая колонка.
        /// </summary>
        public int NewColumn { get; set; }

        /// <summary>
        /// Новая строка.
        /// </summary>
        public int NewRow { get; set; }

        /// <summary>
        /// Старая колонка.
        /// </summary>
        public int OlcColumn { get; set; }

        /// <summary>
        /// Старая строка.
        /// </summary>
        public int OldRow { get; set; }

        #endregion

    } // class SiberianNavigationEventArgs

} // namespace ManagedIrbis.WinForms.Grid
