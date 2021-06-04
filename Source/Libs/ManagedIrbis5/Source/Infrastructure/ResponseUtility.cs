// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ResponseUtility.cs -- методы расширения для работы с Response
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Методы расширения для работы с <see cref="Response"/>.
    /// </summary>
    public static class ResponseUtility
    {
        #region Public methods

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood([NotNullWhen(true)] this Response? response) =>
            response is not null && response.CheckReturnCode();

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood
            (
                [NotNullWhen(true)] this Response? response,
                params int[] goodCodes
            )
            => response is not null && response.CheckReturnCode(goodCodes);

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? Transform<T>
            (
                this Response? response,
                Func<Response, T?> transformer
            )
            where T: class
            => response.IsGood() ? transformer(response) : null;

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? TransformNoCheck<T>
            (
                this Response? response,
                Func<Response, T?> transformer
            )
            where T: class
            => response is not null ? transformer(response) : null;

        #endregion

    } // class ResponseUtility

} // namespace ManagedIrbis.Infrastructure
