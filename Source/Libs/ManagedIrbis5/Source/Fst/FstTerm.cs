// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* FstTerm.cs -- термин, получаемый в результате работы FST-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// Термин, получаемый в результате работы FST-скрипта.
    /// </summary>
    public sealed class FstTerm
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Текстовое значение термина.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Метка поля (берётся из FST).
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Номер повторения (начиная с 1).
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Смещение от начала поля.
        /// </summary>
        public int Offset { get; set; }

        #endregion

    } // class FstTerm

} // namespace ManagedIrbis.Fst
