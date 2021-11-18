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

/* Iswc.cs -- ISWC
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // https://en.wikipedia.org/wiki/International_Standard_Musical_Work_Code
    //
    // International Standard Musical Work Code (ISWC) is a unique identifier
    // for musical works, similar to ISBN for books. It is adopted
    // as international standard ISO 15707. The ISO subcommittee with
    // responsibility for the standard is TC 46/SC 9.
    //
    // Format
    //
    // Each code is composed of three parts:
    //
    // * prefix element (1 character)
    // * work identifier (9 digits)
    // * check digit (1 digit)
    //
    // Currently, the only prefix defined is "T", indicating Musical works.
    // However, additional prefixes may be defined in the future to expand
    // the available range of identifiers and/or expand the system
    // to additional types of works.
    //
    // The check digit is calculated using the Luhn algorithm.
    //
    // ISWC identifiers are commonly written the form T-123.456.789-Z.
    // The grouping is for ease of reading only; the numbers do not
    // incorporate any information about the work's region, author,
    // publisher, etc. Rather, they are simply issued in sequence.
    // These separators are not required, and no other separators are allowed.
    //
    // The first ISWC was assigned in 1995, for the song "Dancing Queen"
    // by ABBA; the code is T-000.000.001-0.
    //

    /// <summary>
    /// International Standard Musical Work Code.
    /// </summary>
    [XmlRoot ("iswc")]
    public sealed class Iswc
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [XmlElement ("identifier")]
        [JsonPropertyName ("identifier")]
        [Description ("Идентификатор")]
        [DisplayName ("Идентификатор")]
        public string? Identifier { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстового представления.
        /// </summary>
        public void ParseText
            (
                ReadOnlySpan<char> text
            )
        {
            Sure.NotEmpty (text);

            var navigator = new ValueTextNavigator(text);
            Identifier = navigator.GetRemainingText().Trim().ThrowIfEmpty().ToString();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Identifier = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Identifier);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Iswc> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Identifier);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Identifier.ToVisibleString();
        }

        #endregion
    }
}
