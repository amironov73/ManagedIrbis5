// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IsoRecordHeader.cs -- заголовок записи в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Marc;

#endregion

#nullable enable

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Заголовок записи в формате ISO 2709.
    /// </summary>
    public sealed class IsoRecordHeader
    {
        #region Properties

        /// <summary>
        /// Статус записи.
        /// </summary>
        public RecordStatus RecordStatus { get; set; }

        /// <summary>
        /// Тип записи.
        /// </summary>
        public RecordType RecordType { get; set; }

        /// <summary>
        /// Библиографический указатель.
        /// </summary>
        public MarcBibliographicalIndex BibliographicalIndex { get; set; }

        /// <summary>
        /// Уровень описания.
        /// </summary>
        /// <value></value>
        public MarcBibliographicalLevel BibliographicalLevel { get; set; }

        /// <summary>
        /// Правила каталогизации.
        /// </summary>
        public MarcCatalogingRules CatalogingRules { get; set; }

        /// <summary>
        /// Наличие связанной записи.
        /// </summary>
        public MarcRelatedRecord RelatedRecord { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode the header.
        /// </summary>
        public void Encode
            (
                byte[] bytes,
                int offset
            )
        {
            unchecked
            {
                bytes[offset] = (byte) RecordStatus;
                bytes[offset + 1] = (byte) RecordType;
                bytes[offset + 2] = (byte) BibliographicalIndex;
                bytes[offset + 3] = (byte) BibliographicalLevel;
                bytes[offset + 4] = (byte) CatalogingRules;
                bytes[offset + 5] = (byte) RelatedRecord;
            }
        }

        /// <summary>
        /// Заголовок по умолчанию.
        /// </summary>
        public static IsoRecordHeader GetDefault()
        {
            var result = new IsoRecordHeader
            {
                RecordStatus = RecordStatus.New,
                RecordType = RecordType.Text,
                BibliographicalIndex = MarcBibliographicalIndex.Monograph,
                BibliographicalLevel = MarcBibliographicalLevel.Unknown,
                CatalogingRules = MarcCatalogingRules.NotConforming,
                RelatedRecord = MarcRelatedRecord.NotRequired
            };

            return result;
        }

        /// <summary>
        /// Parse text representation.
        /// </summary>
        public static IsoRecordHeader Parse
            (
                string text
            )
        {
            var result = new IsoRecordHeader
            {
                RecordStatus = (RecordStatus) text[0],
                RecordType = (RecordType) text[1],
                BibliographicalIndex = (MarcBibliographicalIndex) text[2],
                BibliographicalLevel = (MarcBibliographicalLevel) text[3],
                CatalogingRules = (MarcCatalogingRules)text[4],
                RelatedRecord = (MarcRelatedRecord)text[5]
            };

            return result;
        }

        /// <summary>
        /// Parse binary representation.
        /// </summary>
        public static IsoRecordHeader Parse
            (
                byte[] bytes,
                int offset
            )
        {
            var result = new IsoRecordHeader
            {
                RecordStatus = (RecordStatus) bytes[offset],
                RecordType = (RecordType) bytes[offset + 1],
                BibliographicalIndex = (MarcBibliographicalIndex) bytes[offset + 2],
                BibliographicalLevel = (MarcBibliographicalLevel) bytes[offset + 3],
                CatalogingRules = (MarcCatalogingRules) bytes[offset + 4],
                RelatedRecord = (MarcRelatedRecord) bytes[offset + 5]
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new char[6];
            unchecked
            {
                result[0] = (char) RecordStatus;
                result[1] = (char) RecordType;
                result[2] = (char) BibliographicalIndex;
                result[3] = (char) BibliographicalLevel;
                result[4] = (char) CatalogingRules;
                result[5] = (char) RelatedRecord;
            }

            return new string(result);
        }

        #endregion
    }
}
