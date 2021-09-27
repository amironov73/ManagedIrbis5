// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ObjectWithCode.cs -- объект, имеющий уникальный код-строку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Объект с уникальным кодом-строкой в качестве идентификатора.
    /// </summary>
    public class ObjectWithCode
        : SelectableObject
    {
        #region Properties

        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        [PrimaryKey]
        [DisplayName ("Код")]
        public string Code { get; set; } = null!;

        #endregion

    } // class ObjectWithCode

} // namespace Istu.BookSupply
