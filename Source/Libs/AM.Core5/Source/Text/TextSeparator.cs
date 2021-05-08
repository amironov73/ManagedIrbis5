// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

/* TextSeparator.cs -- разделяет текст на вложенный и внешний
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Разделяет текст на вложенный и внешний.
    /// </summary>
    public class TextSeparator
    {
        #region Constants

        /// <summary>
        /// Закрывающая последовательность символов по умолчанию.
        /// </summary>
        public const string DefaultClose = "%>";

        /// <summary>
        /// Открывающая последовательность символов по умолчанию.
        /// </summary>
        public const string DefaultOpen = "<%";

        #endregion

        #region Properties

        /// <summary>
        /// Действующая закрывающая последовательность символов.
        /// </summary>
        public string Close
        {
            get => new (_close);
            set
            {
                Sure.NotNullNorEmpty(value, nameof(value));

                _close = value.ToCharArray();
            }
        }

        /// <summary>
        /// Действующая открывающая последовательность символов.
        /// </summary>
        public string Open
        {
            get => new (_open);
            set
            {
                Sure.NotNullNorEmpty(value, nameof(value));

                _open = value.ToCharArray();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TextSeparator
            (
                string open = DefaultOpen,
                string close = DefaultClose
            )
        {
            Sure.NotNullNorEmpty(open, nameof(open));
            Sure.NotNullNorEmpty(close, nameof(close));

            _close = close.ToCharArray();
            _open = open.ToCharArray();
        }

        #endregion

        #region Private members

        private char[] _close;

        private char[] _open;

        //=================================================

        /// <summary>
        /// Обработка следующего куска текста.
        /// Метод должен быть переопределен в классе-потомке.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected virtual void HandleChunk
            (
                bool inner,
                string text
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        //=================================================

        /// <summary>
        /// Separate text.
        /// </summary>
        public bool SeparateText
            (
                string text
            )
        {
            return SeparateText (new StringReader(text));
        }

        //=================================================

        /// <summary>
        /// Разделение текста на внешний и внутренний.
        /// </summary>
        public bool SeparateText
            (
                TextReader reader
            )
        {
            var inner = false;
            var array = _open;
            var buffer = StringBuilderPool.Shared.Get();

            while (true)
            {
                var depth = 0;

                while (true)
                {
                    var i = reader.Read();
                    if (i < 0)
                    {
                        goto DONE;
                    }

                    var c = (char)i;
                    if (c == array[depth])
                    {
                        depth++;
                        if (depth == array.Length)
                        {
                            HandleChunk
                                (
                                    inner,
                                    buffer.ToString()
                                );

                            depth = 0;
                            buffer.Length = 0;
                            inner = !inner;
                            array = inner ? _close : _open;
                        }
                    }
                    else
                    {
                        if (depth != 0)
                        {
                            buffer.Append(array, 0, depth);
                            depth = 0;
                        }
                        buffer.Append(c);
                    }
                }
            }

            DONE:

            if (buffer.Length != 0)
            {
                HandleChunk
                    (
                        inner,
                        buffer.ToString()
                    );
            }

            StringBuilderPool.Shared.Return(buffer);

            return inner;
        }

        //=================================================

        #endregion

    } // class TextSeparator

} // namespace AM.Text
