// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Department.cs -- информация о факультете (подразделении) вуза
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Информация о факультете (подразделении) вуза.
    /// </summary>
    [Table ("departments")]
    public sealed class Department
        : ObjectWithId
    {
        #region Properties

        /// <summary>
        /// Наименование факультета.
        /// </summary>
        public string Name { get; set; } = null!;

        #endregion

    } // class Department

} // namespace Istu.BookSupply
