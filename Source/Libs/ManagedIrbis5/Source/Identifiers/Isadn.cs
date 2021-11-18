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

/* Isadn.cs -- ISADN
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
    // https://en.wikipedia.org/wiki/International_Standard_Authority_Data_Number
    //
    // The International Standard Authority Data Number (ISADN) was
    // a registry proposed by the International Federation of Library
    // Associations and Institutions (IFLA) to provide and maintain
    // unique identifiers for entities described in authority data.
    // Having such a unique number would have the benefits of being
    // language-independent and system-independent.
    //
    // Francoise Bourdon was a major proponent of such a standard,
    // proposing a structure for the ISADN and recommending that the
    // number uniquely identify authority records, rather than
    // their subjects.
    //
    // A 1989 article by Delsey described the work on the IFLA
    // Working Group on an International Authority System, spending
    // a good portion of time on conceptualizing an international
    // standard number "that will facilitate the linkage of variant
    // authorities for the same identity." Their discussion was
    // very complex in its discussion of which agencies would actually
    // assign such numbers. For example, a national library might
    // be tasked with assigning identifiers to authors within
    // its country, but this would lead to duplicate identifiers
    // for authority data that describe transnational people.
    //
    // The project was ultimately determined to be unfeasible.
    // Tillett suggested that the cluster identifiers used by the
    // Virtual International Authority File might meet the needs
    // expressed in the proposal.
    //
    // The concept of an ISADN continues to be relevant to the information
    // science community, as it could be a great help in the problem
    // of measuring an individual author's research output.
    //

    /// <summary>
    /// International Standard Authority Data Number.
    /// </summary>
    [XmlRoot ("isadn")]
    public sealed class Isadn
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
            var verifier = new Verifier<Isadn> (this, throwOnError);

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
