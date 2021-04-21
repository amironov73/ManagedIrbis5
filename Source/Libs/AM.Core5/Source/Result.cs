// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Result.cs -- обертка для возврата результата либо ошибки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Обертка для возврата результата либо ошибки.
    /// Вдохновлен Swift.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    /// <typeparam name="TError">Тип ошибки.</typeparam>
    public struct Result<TResult, TError>
    {
        #region Properties

        /// <summary>
        /// Возвращенный результат в случае успеха.
        /// </summary>
        public TResult? Value { get; private init; }

        /// <summary>
        /// Признак успешного вычисления.
        /// </summary>
        public bool Success { get; private init; }

        /// <summary>
        /// Код ошибки в случае неудачи.
        /// </summary>
        public TError? Error { get; private init; }

        #endregion

        #region Public methods

        /// <summary>
        /// Конструирование значения для успешного завершения.
        /// </summary>
        /// <param name="value">Возвращаемое значение.</param>
        /// <returns>Сконструированная структура.</returns>
        public static Result<TResult, TError> Succeed
            (
                TResult value
            )
        {
            return new ()
                {
                    Value = value,
                    Success = true,
                    Error = default
                };
        }

        /// <summary>
        /// Конструирование значения для неуспешного завершения.
        /// </summary>
        /// <param name="error">Значение для ошибки.</param>
        /// <returns>Сконструированная структура.</returns>
        public static Result<TResult, TError> Failure
            (
                TError error
            )
        {
            return new ()
                {
                    Value = default,
                    Success = false,
                    Error = error
                };
        }

        /// <summary>
        /// Конструирование значения для неуспешного завершения.
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <param name="description">Описание ошибки.</param>
        /// <typeparam name="T">Тип для кода ошибки</typeparam>
        /// <returns>Сконструированная структура.</returns>
        public static Result<TResult, ErrorInfo<T>> FailureWithInfo<T>
            (
                T code,
                string description
            )
        {
            return new ()
            {
                Value = default,
                Success = false,
                Error = new ErrorInfo<T>(code, description)
            };
        }

        /// <summary>
        /// Получение результата либо альтернативного значения.
        /// </summary>
        /// <param name="value">Альтернативное значение.</param>
        /// <returns>Вычисленное значение.</returns>
        public TResult ValueOr(TResult value)
        {
            return Success
                ? Value!
                : value;
        }

        /// <summary>
        /// Получение результата либо альтернативного значения.
        /// </summary>
        /// <param name="func">Функция для вычисления альтернативного значения.</param>
        /// <returns>Вычисленное значение.</returns>
        public TResult ValueOr(Func<TResult> func)
        {
            return Success
                ? Value!
                : func();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Неявное преобразование к булевому типу.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <returns>Признак успешного завершения.</returns>
        public static implicit operator bool
            (
                Result<TResult, TError> value
            )
        {
            return value.Success;
        }

        /// <summary>
        /// Неявное преобразование к типу результата.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <returns>Полученное значение.</returns>
        public static implicit operator TResult
            (
                Result<TResult, TError> value
            )
        {
            return value.Value!;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() =>
            Success ? $"Success: {Value}" : $"Failure: {Error}";

        #endregion
    }
}
