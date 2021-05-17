﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* RequestStatus.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Requests
{
    /// <summary>
    /// Commonly used request statuses.
    /// </summary>
    public static class RequestStatus
    {
        #region Constants

        /// <summary>
        /// Unfulfilled request.
        /// </summary>
        public const string Unfulfilled = "0";

        /// <summary>
        /// Fulfilled request.
        /// </summary>
        public const string Fulfilled = "1";

        /// <summary>
        /// Reserved request.
        /// </summary>
        public const string Reserved = "2";

        /// <summary>
        /// Rejected request.
        /// </summary>
        public const string Rejected = "3";

        #endregion

    } // class RequestStatus

} // namespace ManagedIrbis.Requests
