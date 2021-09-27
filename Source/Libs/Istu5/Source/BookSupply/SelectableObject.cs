// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SelectableObject.cs -- объект, который может быть выбран (отмечен)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Объект, который может быть выбран (отмечен).
    /// </summary>
    public class SelectableObject
    {
        #region Properties

        /// <summary>
        /// Признак выбора объекта.
        /// </summary>
        [NotColumn]
        public bool Selected { get; set; }

        #endregion

    } // class SelectableObject

} // namespace Istu.BookSupply
