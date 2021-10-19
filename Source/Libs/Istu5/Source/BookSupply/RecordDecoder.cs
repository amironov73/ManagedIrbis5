// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* RecordDecoder.cs -- декодирует библиографическую запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Декодирует библиографическую запись, превращая ее в описание книги.
    /// </summary>
    public sealed class RecordDecoder
    {
        #region Public methods

        /// <summary>
        /// Декодирование библиографической записи.
        /// </summary>
        public BookInfo DecodeRecord
            (
                Record record,
                University university
            )
        {
            var result = new BookInfo
            {
                Description = university.FormatRecord (record.Mfn),
                Record = record
            };

            return result;
        } // method DecodeRecord

        #endregion

    } // class RecordDecoder

} // namespace Istu.BookSupply
