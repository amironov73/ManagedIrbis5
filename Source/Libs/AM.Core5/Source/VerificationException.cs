// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* VerificationException.cs -- сигнализирует об обнаружении ошибки при верификации.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Сигнализирует об обнаружении ошибки при верификации объекта.
    /// </summary>
    public sealed class VerificationException
        : ArsMagnaException
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public VerificationException()
        {
        }

        /// <summary>
        /// Конструктор с сообщением.
        /// </summary>
        public VerificationException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Конструктор с сообщением и внутренним исключением.
        /// </summary>
        public VerificationException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }

        #endregion
    }
}
