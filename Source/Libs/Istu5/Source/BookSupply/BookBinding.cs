// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* BookBinding.cs -- привязка книги к учебной дисциплине
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Привязка книги (учебника) к учебной дисциплине.
    /// </summary>
    [Table ("book_bindings")]
    public sealed class BookBinding
        : ObjectWithId
    {
        #region Properties

        /// <summary>
        /// Номер каталожной карточки.
        /// </summary>
        public string Card { get; set; } = null!;

        /// <summary>
        /// Номер дисциплины.
        /// </summary>
        public int Discipline { get; set; }

        #endregion

    } // class BookBinding

} // namespace Istu.BookSupply
