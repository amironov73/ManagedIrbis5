// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AccessKind.cs -- значение доступа к ресурсу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Значение доступа к ресурсу.
    /// </summary>
    public static class AccessKind
    {
        #region Constants

        /// <summary>
        /// Запрет доступа.
        /// </summary>
        public const string Denied = "0";

        /// <summary>
        /// Постраничный просмотр.
        /// </summary>
        public const string PageView = "1";

        /// <summary>
        /// Скачивание.
        /// </summary>
        public const string Download = "2";

        #endregion

    } // class AccessKind

} // namespace ManagedIrbis.AccessKind
