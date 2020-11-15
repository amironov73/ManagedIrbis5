// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Operator.cs -- оператор системы книговыдачи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Оператор системы книговыдачи.
    /// </summary>
    [Table(Name = "operators")]
    public class Operator
    {
        #region Properties

        /// <summary>
        /// Идентификатор оператор.
        /// </summary>
        [Column, Identity, PrimaryKey]
        public int ID { get; set; }

        /// <summary>
        /// Фамилия, имя, отчество оператора.
        /// </summary>
        [Column]
        public string? Name { get; set; }

        /// <summary>
        /// Комментарий в произвольной форме.
        /// </summary>
        [Column, Nullable]
        public string? Comment { get; set; }

        /// <summary>
        /// Штрих-код.
        /// </summary>
        [Column]
        public string? Barcode { get; set; }

        #endregion
    }
}
