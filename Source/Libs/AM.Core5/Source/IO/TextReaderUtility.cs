// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TextReaderUtility.cs -- вспомогательные методы для чтения текстовых данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Вспомогательные методы для чтения текстовых данных.
    /// </summary>
    public static class TextReaderUtility
    {
        #region Public methods

        /// <summary>
        /// Open file for reading.
        /// </summary>
        public static StreamReader OpenRead
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var result = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                );

            return result;
        }

        /// <summary>
        /// Обязательное чтение строки.
        /// </summary>
        public static string RequireLine
            (
                this TextReader reader
            )
        {
            var result = reader.ReadLine();
            if (ReferenceEquals(result, null))
            {
                Magna.Error
                    (
                        nameof(TextReaderUtility)
                        + "::"
                        + nameof(RequireLine)
                        + ": "
                        + "unexpected end of stream"
                    );

                throw new ArsMagnaException
                    (
                        "Unexpected end of stream"
                    );
            }

            return result;
        }

        #endregion
    }
}
