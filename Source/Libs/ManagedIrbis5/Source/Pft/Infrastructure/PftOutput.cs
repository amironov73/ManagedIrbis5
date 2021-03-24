// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftOutput.cs -- выходные потоки форматтера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Выходные потоки форматтера.
    /// </summary>
    public sealed class PftOutput
    {
        #region Properties

        /// <summary>
        /// Родительский буфер. Может быть <c>null</c>.
        /// </summary>
        public PftOutput? Parent { get; }

        /// <summary>
        /// Основной (обычный) поток.
        /// </summary>
        public TextBuffer Normal { get; }

        /// <summary>
        /// Поток предупреждений.
        /// </summary>
        public TextBuffer Warning { get; }

        /// <summary>
        /// Поток ошибок.
        /// </summary>
        public TextBuffer Error { get; }

        /// <summary>
        /// Накопленный текст основного потока.
        /// </summary>
        public string Text => Normal.ToString();

        /// <summary>
        /// Накопленный текст потока предупреждений.
        /// </summary>
        public string WarningText => Warning.ToString();

        /// <summary>
        /// Накопленный текст потока ошибок.
        /// </summary>
        public string ErrorText => Error.ToString();

        /// <summary>
        /// Накоплен ли текст в основном потоке?
        /// </summary>
        public bool HaveText => _HaveText(Normal);

        /// <summary>
        /// Были ли предупреждения?
        /// </summary>
        public bool HaveWarning => _HaveText(Warning);

        /// <summary>
        /// Были ли ошибки?
        /// </summary>
        public bool HaveError => _HaveText(Error);

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftOutput()
            : this(null)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftOutput
            (
               PftOutput? parent
            )
        {
            Parent = parent;
            Normal = new TextBuffer();
            Warning = new TextBuffer();
            Error = new TextBuffer();
        } // constructor

        #endregion

        #region Private members

        private static bool _HaveText
            (
               TextBuffer writer
            )
        {
            return writer.Length != 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка основного потока.
        /// </summary>
        public PftOutput ClearText()
        {
            Normal.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Очистака потока предупреждений.
        /// </summary>
        public PftOutput ClearWarning()
        {
            Warning.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Очистка потока ошибок.
        /// </summary>
        public PftOutput ClearError()
        {
            Error.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Получить (воображаемую) позицию курсора по горизонтали.
        /// </summary>
        public int GetCaretPosition()
        {
            return Normal.Column;
        }

        //=================================================

        /// <summary>
        /// Пустая ли последняя строка в основном буфере?
        /// </summary>
        public bool HaveEmptyLine()
        {
            bool result = Normal.Column == 1;

            return result;
        }

        //=================================================

        /// <summary>
        /// Предваряется явным переводом строки?
        /// </summary>
        public bool PrecededByEmptyLine()
        {
            bool result = Normal.PrecededByNewLine();

            return result;
        }

        //=================================================

        /// <summary>
        /// Временный переход к новому буферу.
        /// </summary>
        public PftOutput Push()
        {
            PftOutput result = new PftOutput(this);

            return result;
        }

        //=================================================

        /// <summary>
        /// Возврат к старому буферу с дописыванием
        /// в конец текста, накопленного в новом
        /// веременном буфере.
        /// </summary>
        public string Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                string warningText = WarningText;
                if (!string.IsNullOrEmpty(warningText))
                {
                    Parent.Warning.Write(warningText);
                }

                string errorText = ErrorText;
                if (!string.IsNullOrEmpty(errorText))
                {
                    Parent.Error.Write(errorText);
                }
            }

            return ToString();
        }

        //=================================================

        /// <summary>
        /// Удалить последнюю строку в буфере, если она пустая.
        /// </summary>
        public PftOutput RemoveEmptyLines()
        {
            Normal.RemoveEmptyLines();

            return this;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public PftOutput Write
            (
                string? format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.Write(format, arg);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write text.
        /// </summary>
        public PftOutput Write
            (
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.Write(value);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        public PftOutput WriteLine
            (
                string? format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.WriteLine(format, arg);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        public PftOutput WriteLine
            (
               string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.WriteLine(value);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        public PftOutput WriteLine()
        {
            Normal.WriteLine();

            return this;
        }

        //=================================================

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Normal.ToString();

        #endregion

    } // class PftOutput

} // namespace ManagedIrbis.Pft.Infrastructure
