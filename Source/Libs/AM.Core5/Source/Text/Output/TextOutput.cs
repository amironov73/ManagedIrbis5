// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TextOutput.cs -- вывод в текстовую строку.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Text.Output
{
    /// <summary>
    /// Вывод в текстовую строку.
    /// </summary>
    public sealed class TextOutput
        : AbstractOutput
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextOutput()
        {
            _builder = new StringBuilder();
        }

        #endregion

        #region Private members

        private readonly StringBuilder _builder;

        #endregion

        #region AbstractOutput members

        /// <inheritdoc cref="AbstractOutput.HaveError" />
        public override bool HaveError { get; set; }

        /// <inheritdoc cref="AbstractOutput.Clear" />
        public override AbstractOutput Clear()
        {
            _builder.Length = 0;
            HaveError = false;

            return this;
        } // method Clear

        /// <inheritdoc cref="AbstractOutput.Configure" />
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement

            return this;
        } // method Configure

        /// <inheritdoc cref="AbstractOutput.Write(string)" />
        public override AbstractOutput Write
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                _builder.Append(text);
            }

            return this;
        } // method Write

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
            if (!string.IsNullOrEmpty(text))
            {
                _builder.Append(text);
            }

            return this;
        } // method WriteError

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => _builder.ToString();

        #endregion

    } // class TextOutput

} // namespace AM.Text.Output
