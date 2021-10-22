// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BindingSpecification.cs -- спецификация подшивки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Спецификация подшивки газет/журналов.
    /// </summary>
    public sealed class BindingSpecification
    {
        #region Properties

        /// <summary>
        /// Шифр журнала.
        /// </summary>
        /// <remarks>
        /// Например, "Л680583".
        /// </remarks>
        public string? MagazineIndex { get; set; }

        /// <summary>
        /// Год.
        /// </summary>
        /// <remarks>
        /// Например, "2017".
        /// </remarks>
        public string? Year { get; set; }

        /// <summary>
        /// Номер тома.
        /// </summary>
        /// <remarks>
        /// Например, "123".
        /// </remarks>
        public string? VolumeNumber { get; set; }

        /// <summary>
        /// Номера выпусков.
        /// </summary>
        /// <remarks>
        /// Например, "1-27,29-58,60-72".
        /// </remarks>
        public string? IssueNumbers { get; set; }

        /// <summary>
        /// Описание.
        /// </summary>
        /// <remarks>
        /// Например, "янв.-июнь"
        /// </remarks>
        public string? Description { get; set; }

        /// <summary>
        /// Номер подшивки.
        /// </summary>
        /// <remarks>
        /// Например, "6284".
        /// </remarks>
        public string? BindingNumber { get; set; }

        /// <summary>
        /// Инвентарный номер подшивки.
        /// </summary>
        /// <remarks>
        /// Например, "Г6284".
        /// </remarks>
        public string? Inventory { get; set; }

        /// <summary>
        /// Фонд подшивки.
        /// </summary>
        /// <remarks>
        /// Например, "ФП".
        /// </remarks>
        public string? Fond { get; set; }

        /// <summary>
        /// Номер комплекта.
        /// </summary>
        /// <remarks>
        /// Например, "1".
        /// </remarks>
        public string? Complect { get; set; }

        #endregion

    } // class BindingSpecification

} // namespace ManagedIrbis.Magazines
