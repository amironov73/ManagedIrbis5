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

/* Isli.cs -- ISLI
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
    // https://en.wikipedia.org/wiki/International_Standard_Link_Identifier
    //
    // The International Standard Link Identifier (ISLI), is an identifier
    // standard. ISLI is a universal identifier for links between entities
    // in the field of information and documentation. It was developed
    // by the International Organization for Standardization (ISO)
    // and published on May 15, 2015. ISO/TC 46/SC 9 is responsible
    // for the development of the ISLI standard.
    //
    // ISLI is used for identifying links between entities in the field
    // of information and documentation. A linked entity can be physical,
    // e.g. a print book or an electronic resource (text, audio, and video);
    // or something abstract, e.g. a physical position within a frame
    // of reference or the time of day.
    //
    // In the context of modern information technology, the application
    // of resources in the field of information and documentation
    // is increasingly getting diversified. Isolated content products
    // can no longer satisfy the ever-increasing user demand.
    //
    // Using a link identifier to build links between resources
    // in the field of information and documentation provides a basis
    // for a combined application of resources in the field, and supports
    // collaborative creation of content and data interoperability
    // between systems.
    //
    // The openness of the ISLI system will boost the emergence of new
    // applications in both multimedia and other fields, which increases
    // the value of the linked-resources.
    //
    // The link model of ISLI includes three elements: a source, a target,
    // and the link between them.
    //

    /// <summary>
    /// International Standard Link Identifier.
    /// </summary>
    [XmlRoot ("isli")]
    public sealed class Isli
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
            var verifier = new Verifier<Isli> (this, throwOnError);

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
