// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TextSeparator.cs -- отделяет вложенный текст от внешнего
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Отделяет вложенный текст от внешнего.
    /// </summary>
    public class TextSeparator
    {
        #region Constants

        /// <summary>
        /// Default closing sequence.
        /// </summary>
        public const string DefaultClose = "%>";

        /// <summary>
        /// Default opening sequence.
        /// </summary>
        public const string DefaultOpen = "<%";

        #endregion

        #region Properties

        /// <summary>
        /// Closing sequence.
        /// </summary>
        public string Close
        {
            get => new string(_close);
            set
            {
                Sure.NotNullNorEmpty(value, nameof(value));

                _close = value.ToCharArray();
            }
        }

        /// <summary>
        /// Nested text opening sequence.
        /// </summary>
        public string Open
        {
            get => new string(_open);
            set
            {
                Sure.NotNullNorEmpty(value, nameof(value));

                _open = value.ToCharArray();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextSeparator
            (
                string open,
                string close
            )
        {
            Sure.NotNullNorEmpty(open, nameof(open));
            Sure.NotNullNorEmpty(close, nameof(close));

            _close = close.ToCharArray();
            _open = open.ToCharArray();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextSeparator()
            : this
            (
                DefaultOpen,
                DefaultClose
            )
        {
        }

        #endregion

        #region Private members

        private char[] _close;

        private char[] _open;

        //=================================================

        /// <summary>
        /// Handle text chunk.
        /// </summary>
        /// <remarks>Must be overridden.</remarks>
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
        /// Separate text.
        /// </summary>
        public bool SeparateText
            (
                TextReader reader
            )
        {
            var inner = false;
            var array = _open;
            var buffer = new StringBuilder();

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

            return inner;
        }

        //=================================================

        #endregion
    }
}
