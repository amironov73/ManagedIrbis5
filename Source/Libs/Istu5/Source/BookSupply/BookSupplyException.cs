// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* BookSupplyException.cs -- исключение, специфичное для подсистемы книгообеспеченности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Исключение, специфичное для подсистемы книгообеспеченности.
    /// </summary>
    public class BookSupplyException
        : ApplicationException
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчани.
        /// </summary>
        public BookSupplyException() {}

        /// <summary>
        /// Конструктор с текстом сообщения.
        /// </summary>
        public BookSupplyException
            (
                string message
            )
            : base (message) {}

        /// <summary>
        /// Конструктор с вложенным исключением.
        /// </summary>
        public BookSupplyException
            (
                string message,
                Exception innerException
            )
            : base (message, innerException) {}

        /// <summary>
        /// Конструктор, применяемый при десериализации.
        /// </summary>
        protected BookSupplyException
            (
                SerializationInfo info,
                StreamingContext context
            )
            : base(info, context) {}

        #endregion

    } // class BookSupplyException

} // namespace Istu.BookSupply
