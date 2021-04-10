// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ZaprInfo.cs -- информация о постоянном запросе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация о постоянном запросе в базе данных ZAPR.
    /// Поле 2.
    /// </summary>
    public sealed class ZaprInfo
    {
        #region Properties

        /// <summary>
        /// Формулировка запроса на естественном языке.
        /// Подполе a.
        /// </summary>
        [SubField('a')]
        public string? NaturalLanguage { get; set; }

        /// <summary>
        /// Полнотекстовая часть запроса.
        /// Подполе b.
        /// </summary>
        [SubField('b')]
        public string? FullTextQuery { get; set; }

        /// <summary>
        /// Библиографическая часть запроса.
        /// Подполе c.
        /// </summary>
        [SubField('c')]
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Дата создания запроса.
        /// Подполе d.
        /// </summary>
        [SubField('d')]
        public IrbisDate Date { get; set; } = new (IrbisDate.TodayText);

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record field.
        /// </summary>
        public static ZaprInfo Parse
            (
                Field field
            )
        {
            Sure.NotNull(field, nameof(field));

            var result = new ZaprInfo
            {
                NaturalLanguage = field.GetSubFieldValue('a').ToString(),
                FullTextQuery = field.GetSubFieldValue('b').ToString(),
                SearchQuery = field.GetSubFieldValue('c').ToString(),
                Date = IrbisDate.ConvertStringToDate(field.GetSubFieldValue('d'))
            };

            return result;
        } // method Parse

        #endregion

    } // class ZaprInfo

} // namespace ManagedIrbis.Fields
