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
using System.Threading.Tasks;

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
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static async Task<bool> IsGoodAsync
            (
                [NotNullWhen(true)] this Task<Response?> task
            )
        {
            var response = await task;

            return response is not null && response.CheckReturnCode();

        } // method IsGoodAsync

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static async Task<bool> IsGoodAsync
            (
                [NotNullWhen(true)] this Task<Response?> task,
                params int[] goodCodes
            )
        {
            var response = await task;

            return response is not null && response.CheckReturnCode(goodCodes);

        } // method IsGoodAsync

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

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static async Task<T?> TransformAsync<T>
            (
                this Task<Response?> response,
                Func<Response, T?> transformer
            )
            where T : class
        {
            var waited = await response;

            return waited.IsGood() ? transformer(waited) : null;

        } // method TransformAsync

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static async Task<T?> TransformNoCheckAsync<T>
            (
                this Task<Response?> response,
                Func<Response, T?> transformer
            )
            where T: class
        {
            var waited = await response;

            return waited is not null ? transformer(waited) : null;

        } // method TransformNoCheckAsync

        #endregion

    } // class ResponseUtility

} // namespace ManagedIrbis.Infrastructure
