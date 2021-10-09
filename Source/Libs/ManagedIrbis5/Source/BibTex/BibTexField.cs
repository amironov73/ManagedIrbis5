// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BibTexField.cs -- поле BibText-записи
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.BibTex
{
    //
    // Каждая запись содержит некоторый список стандартных полей
    // (можно вводить любые другие поля, которые просто игнорируются
    // стандартными программами).
    //

    /// <summary>
    /// Поле BibText-записи.
    /// </summary>
    public sealed class BibTexField
    {
        #region Properties

        /// <summary>
        /// Тег поля, см. <see cref="KnownTags"/>.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Значение поля.
        /// </summary>
        public string? Value { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Tag}={Value}";

        #endregion

    } // class BibTexField

} // namespace ManagedIbris.BibTex
