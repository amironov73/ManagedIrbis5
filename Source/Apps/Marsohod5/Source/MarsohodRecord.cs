// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MarsohodRecord.cs -- запись, подлежащая импорту
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis;

#endregion

#nullable enable

namespace Marsohod5
{
    /// <summary>
    /// Запись, подлежащая импорту.
    /// </summary>
    public sealed class MarsohodRecord
    {
        #region Properties

        /// <summary>
        /// Исходная запись.
        /// </summary>
        public Record? SourceRecord { get; set; }

        /// <summary>
        /// Конвертированная запись.
        /// </summary>
        public Record? ConvertedRecord { get; set; }

        /// <summary>
        /// Текущий выпуск журнала.
        /// </summary>
        public string? CurrentIssue { get; set; }

        /// <summary>
        /// Сведения о журнале.
        /// </summary>
        public MarsMagazineInfo? Magazine { get; set; }

        #endregion

    } // class MarsohodRecord

} // namespace Marsohod5
