// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* GblException.cs -- базовый класс для исключений, связанных с глобальной корректировкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Базовый класс для исключений, связанных с глобальной корректировкой записей.
    /// </summary>
    public class GblException
        : IrbisException
    {
        #region Construciton

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public GblException() {}

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GblException (string message) : base(message) {}

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GblException (string message, Exception innerException)
            : base (message, innerException) {}

        #endregion

    } // class GblException

} // namespace ManagedIrbis.Gbl
