// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TextWriterUtility.cs -- вспомогательные методы для записи текстовых данных
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
    /// Вспомогательные методы для записи текстовых данных.
    /// </summary>
    public static class TextWriterUtility
    {
        #region Public methods

        /// <summary>
        /// Open file for append.
        /// </summary>
        public static StreamWriter Append
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var result = new StreamWriter
                (
                    new FileStream(fileName, FileMode.Append),
                    encoding
                );

            return result;

        } // method Append

        /// <summary>
        /// Open file for writing.
        /// </summary>
        public static StreamWriter Create
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var result = new StreamWriter
                (
                    new FileStream(fileName, FileMode.Create),
                    encoding
                );

            return result;

        } // method Create

        #endregion

    } // method TextWriterUtility

} // namespace AM.IO
