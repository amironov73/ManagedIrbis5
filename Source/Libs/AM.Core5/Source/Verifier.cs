// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Verifier.cs -- работа с интерфейсом IVerifiable
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Работа с интерфейсом <see cref="IVerifiable"/>.
    /// </summary>
    public sealed class Verifier<T>
        where T : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Префикс.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Результат верификации.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Проверяемый объект.
        /// </summary>
        public T Target { get; }

        /// <summary>
        /// Выбрасывать ли исключение при верификации?
        /// </summary>
        public bool ThrowOnError { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Verifier
            (
                T target,
                bool throwOnError = true
            )
        {
            Target = target;
            ThrowOnError = throwOnError;
            Result = true;

        } // constructor

        #endregion

        #region Private members

        [ExcludeFromCodeCoverage]
        private void _Throw
            (
                string message
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty (Prefix))
                {
                    Throw (Prefix + ": " + message);
                }

                Throw (message);
            }

        } // method _Throw

        [ExcludeFromCodeCoverage]
        private void _Throw
            (
                string format,
                params object[] arguments
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty (Prefix))
                {
                    string message = Prefix + ": " + string.Format (format, arguments);
                    Throw (message);
                }

                Throw (format, arguments);
            }

        } // method _Throw

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка заданного условия.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                [CallerArgumentExpression ("condition")] string? message = null
            )
        {
            Result = Result && condition;
            _Throw (message!);

            return this;

        } // method Assert

        /// <summary>
        /// Проверка условия.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                string format,
                params object[] arguments
            )
        {
            Result = Result && condition;
            _Throw (format, arguments);

            return this;

        } // method Assert

        /// <summary>
        /// Проверка существования указанной директории.
        /// </summary>
        public Verifier<T> DirectoryExist
            (
                string path,
                [CallerArgumentExpression ("path")] string? name = null
            )
        {
            if (string.IsNullOrEmpty (path))
            {
                Result = false;
                _Throw ($"Directory '{name}': path not specified");
            }
            else if (!Directory.Exists (path))
            {
                Result = false;
                _Throw ($"Directory '{name}' is set to '{path}': path not exist");
            }

            return this;

        } // method DirectoryExist

        /// <summary>
        /// Фиксация сбоя.
        /// </summary>
        public Verifier<T> Failure()
        {
            Result = false;

            return this;

        } // method Failure

        /// <summary>
        /// Фиксация сбоя.
        /// </summary>
        public Verifier<T> Failure
            (
                string message
            )
        {
            Result = false;
            _Throw (message);

            return this;

        } // method Failure

        /// <summary>
        /// Проверка существования указанного файла.
        /// </summary>
        public Verifier<T> FileExist
            (
                string path,
                [CallerArgumentExpression ("path")] string? name = null
            )
        {
            if (string.IsNullOrEmpty (path))
            {
                Result = false;
                _Throw ($"File '{name}': path not specified");
            }
            else if (!File.Exists (path))
            {
                Result = false;
                _Throw ("File '{name}' is set to '{path}': path not exist");
            }

            return this;

        } // method FileExist

        /// <summary>
        /// Проверка на то, что указанное значение является одним из допустимых.
        /// </summary>
        public Verifier<T> IsOneOf
            (
                string? value,
                string[] allowed,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            if (!value.IsOneOf (allowed))
            {
                Result = false;
                _Throw ($"value {value} is no allowed. Allowed values are: {string.Join (',', allowed)}");
            }

            return this;

        } // method IsOneOf

        /// <summary>
        /// Проверка указателя на объект на равенство <c>null</c>.
        /// </summary>
        public Verifier<T> NotNull
            (
                object? value,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            return Assert
                (
                    value is not null,
                    name
                );

        }

        /// <summary>
        /// Проверка, что среди перечисленных строк хотя бы одна
        /// не <c>null</c> и не пустая.
        /// </summary>
        public Verifier<T> AnyNotNullNorEmpty
            (
                params string?[] values
            )
        {

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty (value))
                {
                    return this;
                }
            }

            Failure ("All string values are null or empty");

            return this;
        }

        /// <summary>
        /// Проверка, что заданная строка не <c>null</c> и не пустая.
        /// </summary>
        public Verifier<T> NotNullNorEmpty
            (
                string? value,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            return Assert (!string.IsNullOrEmpty (value), name);

        }

        /// <summary>
        /// Проверка, что заданный массив не <c>null</c> и не пустой.
        /// </summary>
        public Verifier<T> NotNullNorEmpty<V>
            (
                V[]? value,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            return Assert (!ArrayUtility.IsNullOrEmpty(value), name);

        } // method NotNullNorEmpty

        /// <summary>
        /// Проверка, что заданный список не <c>null</c> и не пустой.
        /// </summary>
        public Verifier<T> NotNullNorEmpty<V>
            (
                IReadOnlyList<V>? value,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            return Assert (value is not null && value.Count != 0, name);

        } // method NotNullNorEmpty

        /// <summary>
        /// Проверка, что заданный словарь не <c>null</c> и не пустой.
        /// </summary>
        public Verifier<T> NotNullNorEmpty<V1, V2>
            (
                IDictionary<V1, V2>? value,
                [CallerArgumentExpression ("value")] string? name = null
            )
        {
            return Assert (value is not null && value.Count != 0, name);

        } // method NotNullNorEmpty

        /// <summary>
        /// Проверка, что указанное число положительное.
        /// </summary>
        public Verifier<T> Positive
            (
                int value,
                [CallerArgumentExpression("value")] string? name = null
            )
        {
            return Assert (value > 0, name!);

        } // method Positive

        /// <summary>
        /// Проверка, что указатели на объекты совпадают.
        /// </summary>
        public Verifier<T> ReferenceEquals
            (
                object? first,
                object? second,
                string message
            )
        {
            return Assert
                (
                    ReferenceEquals (first, second),
                    message
                );

        } // method ReferenceEquals

        /// <summary>
        /// Выброс исключения.
        /// </summary>
        public void Throw()
        {
            Magna.Error (nameof (Verifier<T>) + "::" + nameof (Throw));

            throw new VerificationException();

        } // method Throw

        /// <summary>
        /// Выброс исключения.
        /// </summary>
        public void Throw
            (
                string? message
            )
        {
            if (!string.IsNullOrEmpty (message))
            {
                Magna.Error
                    (
                        nameof (Verifier<T>) + "::" + nameof (Throw)
                        + ": " + message
                    );

                throw new VerificationException (message);
            }

            Magna.Error (nameof (Verifier<T>) + "::" + nameof (Throw));

            throw new VerificationException();

        } // method Throw

        /// <summary>
        /// Выброс исключения.
        /// </summary>
        public void Throw
            (
                string format,
                params object[] arguments
            )
        {
            var message = string.Format (format, arguments);
            Throw (message);

        } // method Throw

        /// <summary>
        /// Верификация под-объекта.
        /// </summary>
        public Verifier<T> VerifySubObject
            (
                IVerifiable verifiable,
                [CallerArgumentExpression ("verifiable")] string? name = null
            )
        {
            // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
            // Пока мы на .NET 5, уберем эту проверку
            // Sure.NotNullNorEmpty (name);

            return Assert (verifiable.Verify (ThrowOnError), name);

        } // method VerifySubObject

        #endregion

    } // class Verifier

} // namespace AM
