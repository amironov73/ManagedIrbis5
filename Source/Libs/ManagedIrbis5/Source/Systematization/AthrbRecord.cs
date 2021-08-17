// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthrbRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;
using AM.Linq;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Запись в базе данных ATHRB.
    /// </summary>
    public sealed class AthrbRecord
    {
        #region Properties

        /// <summary>
        /// Основной заголовок рубрики.
        /// Поле 210.
        /// </summary>
        [Field(210)]
        public AthrbHeading? MainHeading { get; set; }

        /// <summary>
        /// Связанные заголовки рубрики.
        /// Поле 510.
        /// </summary>
        [Field(510)]
        public AthrbHeading[]? LinkedHeadings { get; set; }

        /// <summary>
        /// Методические указания / описания.
        /// Поле 300.
        /// </summary>
        [Field(300)]
        public AthrbGuidelines[]? Guidelines { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static AthrbRecord Parse
            (
                Record record
            )
        {
            var result = new AthrbRecord
            {
                MainHeading = AthrbHeading.Parse(record.Fields.GetFirstField(210)),

                LinkedHeadings = record.Fields
                    .GetField(510)
                    .Select(AthrbHeading.Parse)
                    .NonNullItems()
                    .ToArray(),

                Guidelines = record.Fields
                    .GetField(300)
                    .Select(AthrbGuidelines.Parse)
                    .ToArray()
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => MainHeading.ToVisibleString();

        #endregion

    } // class AthrbRecord

} // namespace ManagedIrbis.Systematization
