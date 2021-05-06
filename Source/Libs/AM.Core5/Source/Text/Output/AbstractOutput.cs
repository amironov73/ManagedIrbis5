// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

/* AbstractOutput.cs -- абстрактный объект текстового вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

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
        /// Текущий общий экземпляр текстового вывода.
        /// </summary>
        public static AbstractOutput Current
        {
            get => _current ??= Null;
            set => _current = value;
        }

        /// <summary>
        /// Пустой поток.
        /// </summary>
        public static AbstractOutput Null => _null ??= new NullOutput();

        /// <summary>
        /// Системная консоль.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static AbstractOutput Console => _console ??= new ConsoleOutput();

        #endregion

        #region Private members

        private static AbstractOutput? _null;
        private static AbstractOutput? _console;
        private static AbstractOutput? _current;

        #endregion

        #region Public methods

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Метод нужно переопределить в потомке.
        /// </summary>
        public abstract AbstractOutput Clear();

        /// <summary>
        /// Конфигурирование объекта.
        /// Метод нужно переопределить в потомке.
        /// </summary>
        /// <param name="configuration">Некая строка конфигурации
        /// (в общем случае зависит от конкретной реализации).
        /// </param>
        public abstract AbstractOutput Configure
            (
                string configuration
            );

        /// <summary>
        /// Вывод текста в стандартный выходной поток.
        /// Метод нужно переопределить в потомке.
        /// </summary>
        /// <param name="text">Текст для вывода.</param>
        public abstract AbstractOutput Write
            (
                string text
            );

        /// <summary>
        /// Выводит текста в стандартный поток ошибок.
        /// Например, красным цветом.
        /// Метод нужно переопределить в потомке.
        /// </summary>
        /// <param name="text">Текст для вывода.</param>
        public abstract AbstractOutput WriteError
            (
                string text
            );

        /// <summary>
        /// Форматированный вывод текста в стандартный выходной поток.
        /// </summary>
        /// <param name="format">Строка формата.</param>
        /// <param name="args">Подставляемые значения.</param>
        public AbstractOutput Write
            (
                string format,
                params object[] args
            )
        {
            return Write ( string.Format ( format, args ) );
        }

        /// <summary>
        /// Форматированный вывод текста в стандартный поток ошибок.
        /// </summary>
        /// <param name="format">Строка формата.</param>
        /// <param name="args">Подставляемые значения.</param>
        public AbstractOutput WriteError
            (
                string format,
                params object[] args
            )
        {
            return WriteError ( string.Format ( format, args ) );
        }

        /// <summary>
        /// Вывод текста в стандартный выходной поток
        /// с последующим переводом строки.
        /// </summary>
        /// <param name="text">Текст для вывода.</param>
        public AbstractOutput WriteLine
            (
                string text
            )
        {
            return Write(text).Write(Environment.NewLine);
        }

        /// <summary>
        /// Вывод форматированного текста в стандартный
        /// выходной поток с последующим переводом строки.
        /// </summary>
        /// <param name="format">Строка формата.</param>
        /// <param name="args">Подставляемые аргументы.</param>
        public AbstractOutput WriteLine
            (
                string format,
                params object[] args
            )
        {
            return Write ( string.Format ( format, args ) )
                .Write (Environment.NewLine);
        }

        /// <summary>
        /// Вывод форматированного текста в стандартный
        /// поток ошибок с последующим переводом строки.
        /// </summary>
        /// <param name="format">Строка формата.</param>
        /// <param name="args">Подставляемые аргументы.</param>
        public AbstractOutput WriteErrorLine
            (
                string format,
                params object[] args
            )
        {
            return WriteError ( string.Format ( format, args ) )
                .WriteError (Environment.NewLine);
        } // method WriteErrorLine

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            // Здесь никаких действий не нужно.
            // Вся очистка -- в потомках (если необходимо).
        } // method Dispose

        #endregion

    } // class AbstractOutput

} // namespace AM.Text.Output
