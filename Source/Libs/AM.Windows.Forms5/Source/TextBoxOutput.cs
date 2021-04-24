// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TextBoxOutput.cs -- вывод в текстовое поле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Вывод в текстовое поле.
    /// </summary>
    public sealed class TextBoxOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Текстбокс
        /// </summary>
        public TextBox TextBox { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextBoxOutput
            (
                TextBox textBox
            )
        {
            TextBox = textBox;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление текста в конец текстбокса.
        /// </summary>
        /// <param name="text"></param>
        public void AppendText
            (
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                TextBox.InvokeIfRequired
                    (
                        () => TextBox.AppendText(text)
                    )
                    .WaitFor();
            }

            TextBox.InvokeIfRequired
                (
                    () => TextBox.SelectionStart = TextBox.TextLength
                )
                .WaitFor();
        }

        #endregion

        #region AbstractOutput members

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        public override bool HaveError { get; set; }

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Clear()
        {
            HaveError = false;
            TextBox.InvokeIfRequired
                (
                    () => TextBox.Clear()
                )
                .WaitFor();

            return this;
        }

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement

            return this;
        }

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        public override AbstractOutput Write
            (
                string text
            )
        {
            AppendText(text);

            return this;
        }

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;
            AppendText(text);

            return this;
        }

        #endregion
    }
}
