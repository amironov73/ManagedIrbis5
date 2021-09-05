// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Translator.cs -- трансляция штрих-кодов в инвентарные номера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Трансляция штрих-кодов в инвентарные номера.
    /// </summary>
    [Table]
    public class Translator
    {
        #region Properties

        /// <summary>
        /// Инвентарный номер экземпляра.
        /// </summary>
        [Column (Name = "invnum"), PrimaryKey]
        public int Inventory { get; set; }

        /// <summary>
        /// Штрих-код экземпляра.
        /// </summary>
        [Column]
        public string? Barcode { get; set; }

        /// <summary>
        /// Дата привязки штрих-кода.
        /// </summary>
        [Column (Name = "whn")]
        public string? Moment { get; set; }

        /// <summary>
        /// Взято на обработку.
        /// </summary>
        [Column]
        public bool Taken { get; set; }

        /// <summary>
        /// Дополнительная информация об экземпляре.
        /// </summary>
        [Column, Nullable]
        public string? Info { get; set; }

        /// <summary>
        /// Номер оператора, выполнившего привязку.
        /// </summary>
        [Column]
        public int Operator { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [Column]
        public bool Pilot { get; set; }

        /// <summary>
        /// RFID-метка.
        /// </summary>
        [Column, Nullable]
        public string? Rfid { get; set; }

        #endregion

    } // class Translator

} // namespace Istu.OldModel
