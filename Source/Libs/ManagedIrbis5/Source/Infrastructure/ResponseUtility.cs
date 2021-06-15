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
        public static bool IsGood
            (
                [NotNullWhen(true)] this Response? response,
                bool dispose = true
            )
        {
            var result = response is not null && response.CheckReturnCode();
            if (dispose)
            {
                response?.Dispose();
            }

            return result;

        } // method IsGood

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood
            (
                [NotNullWhen(true)] this Response? response,
                bool dispose = true,
                params int[] goodCodes
            )
        {
            var result = response is not null && response.CheckReturnCode(goodCodes);
            if (dispose)
            {
                response?.Dispose();
            }

            return result;

        } // method IsGood

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static async Task<bool> IsGoodAsync
            (
                [NotNullWhen(true)] this Task<Response?> task,
                bool dispose = true
            )
        {
            var response = await task;
            var result = response is not null && response.CheckReturnCode();
            if (dispose)
            {
                response?.Dispose();
            }

            return result;

        } // method IsGoodAsync

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static async Task<bool> IsGoodAsync
            (
                [NotNullWhen(true)] this Task<Response?> task,
                bool dispose = true,
                params int[] goodCodes
            )
        {
            var response = await task;
            var result = response is not null && response.CheckReturnCode(goodCodes);
            if (dispose)
            {
                response?.Dispose();
            }

            return result;

        } // method IsGoodAsync

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? Transform<T>
            (
                this Response? response,
                Func<Response, T?> transformer
            )
            where T : class
        {
            var result = response.IsGood(false) ? transformer(response) : null;
            response?.Dispose();

            return result;

        } // method Transform

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? TransformNoCheck<T>
            (
                this Response? response,
                Func<Response, T?> transformer
            )
            where T : class
        {
            var result = response is not null ? transformer(response) : null;
            response?.Dispose();

            return result;

        } // method TransformNoCheck

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
            var result = waited.IsGood(false) ? transformer(waited) : null;
            waited?.Dispose();

            return result;

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
            var result = waited is not null ? transformer(waited) : null;
            waited?.Dispose();

            return result;

        } // method TransformNoCheckAsync

        #endregion

    } // class ResponseUtility

} // namespace ManagedIrbis.Infrastructure
