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

using System.IO;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Работа с интерфейсом <see cref="IVerifiable"/>.
    /// </summary>
    public sealed class Verifier<T>
        where T: IVerifiable
    {
        #region Properties

        /// <summary>
        /// Prefix.
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Result.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Target.
        /// </summary>
        public T Target { get; }

        /// <summary>
        /// Throw on error.
        /// </summary>
        public bool ThrowOnError { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Verifier
            (
                T target,
                bool throwOnError
            )
        {
            Target = target;
            ThrowOnError = throwOnError;
            Result = true;
        }

        #endregion

        #region Private members

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private void _Throw()
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Throw(Prefix);
                }
                Throw();
            }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private void _Throw
            (
                string message
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    Throw(Prefix + ": " + message);
                }
                Throw(message);
            }
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        private void _Throw
            (
                string format,
                params object[] arguments
            )
        {
            if (!Result && ThrowOnError)
            {
                if (!string.IsNullOrEmpty(Prefix))
                {
                    string message = Prefix + ": " + string.Format(format, arguments);
                    Throw(message);
                }
                Throw(format, arguments);
            }
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition
            )
        {
            Result = Result && condition;
            _Throw();

            return this;
        }

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                string message
            )
        {
            Result = Result && condition;
            _Throw(message);

            return this;
        }

        /// <summary>
        /// Assert.
        /// </summary>
        public Verifier<T> Assert
            (
                bool condition,
                string format,
                params object[] arguments
            )
        {
            Result = Result && condition;
            _Throw(format, arguments);

            return this;
        }

        /// <summary>
        /// Specified directory must exist.
        /// </summary>
        public Verifier<T> DirectoryExist
            (
                string path,
                string name
            )
        {
            if (string.IsNullOrEmpty(path))
            {
                Result = false;
                _Throw
                    (
                        "Directory '{0}': path not specified",
                        name
                    );
            }

            if (!Directory.Exists(path))
            {
                Result = false;
                _Throw
                    (
                        "Directory '{0}' is set to '{1}': path not exist",
                        name,
                        path
                    );
            }

            return this;
        }

        /// <summary>
        /// Specified file must exist.
        /// </summary>
        public Verifier<T> FileExist
            (
                string path,
                string name
            )
        {
            if (string.IsNullOrEmpty(path))
            {
                Result = false;
                _Throw
                    (
                        "File '{0}': path not specified",
                        name
                    );
            }

            if (!File.Exists(path))
            {
                Result = false;
                _Throw
                    (
                        "File '{0}' is set to '{1}': path not exist",
                        name,
                        path
                    );
            }

            return this;
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNull
            (
                object? value
            )
        {
            return Assert(!ReferenceEquals(value, null));
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNull
            (
                object? value,
                string name
            )
        {
            return Assert
                (
                    !ReferenceEquals(value, null),
                    name
                );
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNullNorEmpty
            (
                string? value
            )
        {
            return Assert(!string.IsNullOrEmpty(value));
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> NotNullNorEmpty
            (
                string? value,
                string name
            )
        {
            return Assert
                (
                    !string.IsNullOrEmpty(value),
                    name
                );
        }

        /// <summary>
        /// Not null?
        /// </summary>
        public Verifier<T> Positive
            (
                int value,
                string name
            )
        {
            return Assert(value > 0, name);
        }

        /// <summary>
        /// Reference equals?
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
                    ReferenceEquals(first, second),
                    message
                );
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        public void Throw()
        {
            // Log.Error(nameof(Verifier<T>) + "::" + nameof(Throw));

            throw new VerificationException();
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        public void Throw
            (
                string message
            )
        {
            //Log.Error
            //    (
            //        nameof(Verifier<T>) + "::" + nameof(Throw)
            //        + ": " + message
            //    );

            throw new VerificationException(message);
        }

        /// <summary>
        /// Throw exception.
        /// </summary>
        public void Throw
            (
                string format,
                params object[] arguments
            )
        {
            string message = string.Format
                (
                    format,
                    arguments
                );

            Throw(message);
        }

        /// <summary>
        /// Verify sub-object.
        /// </summary>
        public Verifier<T> VerifySubObject
            (
                IVerifiable verifiable
            )
        {
            Assert(verifiable.Verify(ThrowOnError));

            return this;
        }

        /// <summary>
        /// Verify sub-object.
        /// </summary>
        public Verifier<T> VerifySubObject
            (
                IVerifiable verifiable,
                string name
            )
        {
            Sure.NotNullNorEmpty(name, nameof(name));

            Assert(verifiable.Verify(ThrowOnError), name);

            return this;
        }

        #endregion
    }
}
