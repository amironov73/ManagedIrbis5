// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ObjectWithId.cs -- объект с уникальным целочисленным идентификатором
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Объект с уникальным целочисленным идентификатором.
    /// </summary>
    public class ObjectWithId
        : SelectableObject
    {
        #region Properties

        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        [PrimaryKey]
        public int ID { get; set; }

        #endregion

    } // class ObjectWithId

} // namespace Istu.BookSupply
