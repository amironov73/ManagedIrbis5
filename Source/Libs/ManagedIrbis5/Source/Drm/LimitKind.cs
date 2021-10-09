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

/* LimitKind.cs -- единица ограничения доступа к ресурсу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Единица ограничения доступа к ресурсу.
    /// </summary>
    public static class LimitKind
    {
        #region Constants

        /// <summary>
        /// Страница.
        /// </summary>
        public const string Page = "";

        /// <summary>
        /// Процент.
        /// </summary>
        public const string Percent = "%";

        #endregion

    } // class LimitKind

} // namespace ManagedIrbis.Drm
