// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* SyntaxException.cs -- возбуждается при обнаружении синтаксической ошибки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Возбуждается при обнаружении синтаксической ошибки.
    /// </summary>
    public class SyntaxException
        : LanguageException
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SyntaxException()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected SyntaxException
            (
                SerializationInfo info,
                StreamingContext context
            )
            : base(info, context)
        {
        }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public SyntaxException
            (
                string? message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SyntaxException
            (
                string? message,
                Exception? innerException
            )
            : base(message, innerException)
        {
        }

        #endregion

    } // class SyntaxException

} // namespace SimplestLanguage
