﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Gsrn.cs -- GSRN
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
    // https://en.wikipedia.org/wiki/Global_Service_Relationship_Number
    //
    // The Global Service Relation Number (GSRN) is part of the GS1 system
    // of standards. It is a simple tool used to identify a service and can
    // identify services uniquely where required.
    //
    // The Global Service Relation Number (GSRN) is the GS1 Identification
    // Key used to identify the relationship between a service provider
    // and service recipient. The GSRN can identify business relationships
    // between businesses or individuals. It does not identify a business
    // or individual specifically for purposes other than the service being
    // provided and therefore does not raise privacy concerns. In simple terms
    // a company or business will issue a GSRN to a customer to identify
    // their relationship. This could be a club membership, a member
    // of a loyalty program or to identify a patient attending hospital.
    //
    // The GSRN will be the key that links to a database to identify
    // the specific detail of the service offered and the contract
    // with the individual. The GSRN may be held as a database record
    // and may be encoded in a barcode that the customer may use to identify
    // themselves to the service provider. The GSRN may be encoded
    // in a code 128, or held in an Electronic Product Code (EPC) tag
    // or used in a database. The function of a GSRN is to provide
    // an identification point which can be used to retrieve information
    // held in a database associated that particular service relation.
    //

    /// <summary>
    /// Global Service Relationship Number.
    /// </summary>
    [XmlRoot ("gsrn")]
    public sealed class Gsrn
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
            var verifier = new Verifier<Gsrn> (this, throwOnError);

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
