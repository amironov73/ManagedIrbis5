// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthrbHeading.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Заголовок рубрики в базе ATHRB.
    /// Поля 210 и 510.
    /// </summary>
    public sealed class AthrbHeading
    {
        #region Properties

        /// <summary>
        /// Основной заголовок рубрики.
        /// Подполе a.
        /// </summary>
        [SubField('a')]
        public string? Heading { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе b.
        /// </summary>
        [SubField('b')]
        public string? Code1 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе c.
        /// </summary>
        [SubField('c')]
        public string? Code2 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе d.
        /// </summary>
        [SubField('d')]
        public string? Code3 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе e.
        /// </summary>
        [SubField('e')]
        public string? Code4 { get; set; }

        /// <summary>
        /// Код рубрики.
        /// Подполе f.
        /// </summary>
        [SubField('f')]
        public string? Code5 { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static AthrbHeading? Parse
            (
                Field? field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            // TODO: реализовать эффективно

            var result = new AthrbHeading
            {
                Heading = field.GetFirstSubFieldValue('a').ToString(),
                Code1 = field.GetFirstSubFieldValue('b').ToString(),
                Code2 = field.GetFirstSubFieldValue('c').ToString(),
                Code3 = field.GetFirstSubFieldValue('d').ToString(),
                Code4 = field.GetFirstSubFieldValue('e').ToString(),
                Code5 = field.GetFirstSubFieldValue('f').ToString(),
            };

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Heading.ToVisibleString();
        }

        #endregion
    }
}
