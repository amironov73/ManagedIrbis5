// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AbstractOutput.cs -- абстрактный объект текстового вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text.Output
{
    /// <summary>
    /// Абстрактный объект текстового вывода.
    /// Например, консоль или текстовое окно.
    /// </summary>
    public abstract class AbstractOutput
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        public abstract bool HaveError { get; set; }

        /// <summary>
        /// Current <see cref="AbstractOutput"/>.
        /// </summary>
        public static AbstractOutput Current
        {
            get
            {
                if (ReferenceEquals(_current, null))
                {
                    _current = Null;
                }

                return _current;
            }
            set
            {
                _current = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static AbstractOutput Null =>
            _null ??= new NullOutput();

        /// <summary>
        ///
        /// </summary>
        public static AbstractOutput Console =>
            _console ??= new ConsoleOutput();

        #endregion

        #region Private members

        private static AbstractOutput? _null;

        private static AbstractOutput? _console;

        private static AbstractOutput? _current;

        #endregion

        #region Public methods

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <returns></returns>
        public abstract AbstractOutput Clear();

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract AbstractOutput Configure
            (
                string configuration
            );

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Возвращает сам объект
        /// вывода.</returns>
        public abstract AbstractOutput Write
            (
                string text
            );

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract AbstractOutput WriteError
            (
                string text
            );

        /// <summary>
        ///
        /// </summary>
        public AbstractOutput Write
            (
                string format,
                params object[] args
            )
        {
            return Write
                (
                    string.Format
                        (
                            format,
                            args
                        )
                );
        }

        /// <summary>
        ///
        /// </summary>
        public AbstractOutput WriteError
            (
                string format,
                params object[] args
            )
        {
            return WriteError
                (
                    string.Format
                        (
                            format,
                            args
                        )
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public AbstractOutput WriteLine
            (
                string text
            )
        {
            return Write
                (
                    text
                    + Environment.NewLine
                );
        }

        /// <summary>
        ///
        /// </summary>
        public AbstractOutput WriteLine
            (
                string format,
                params object[] args
            )
        {
            return Write
                (
                    string.Format
                        (
                            format,
                            args
                        )
                    + Environment.NewLine
                );
        }

        /// <summary>
        ///
        /// </summary>
        public AbstractOutput WriteErrorLine
            (
                string format,
                params object[] args
            )
        {
            return WriteError
                (
                    string.Format
                        (
                            format,
                            args
                        )
                    + Environment.NewLine
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
        }

        #endregion

    } // class AbstractOutput

} // namespace AM.Text.Output
