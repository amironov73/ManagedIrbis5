// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* GroupBinding.cs -- привязка студенческой группы к учебной дисциплине
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Привязка студенческой группы к учебной дисциплине.
    /// </summary>
    [Table ("group_binding")]
    public sealed class GroupBinding
        : ObjectWithId
    {
        #region Properties

        /// <summary>
        /// Идентификатор учебной дисциплины.
        /// </summary>
        public int Discipline { get; set; }

        /// <summary>
        /// Шифр группы.
        /// </summary>
        public string Group { get; set; } = null!;

        /// <summary>
        /// Семестры (битовые флаги).
        /// </summary>
        public Semester Semester { get; set; }

        #endregion

    } // class GroupBinding

} // namespace Istu.BookSupply
