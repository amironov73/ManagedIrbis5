﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataTableInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("table")]
    public class DataTableInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Gets the columns.
        /// </summary>
        [XmlElement("column")]
        [JsonPropertyName("columns")]
        public NonNullCollection<DataColumnInfo> Columns
            {
                get;
                private set;
            }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataTableInfo()
        {
            Columns = new NonNullCollection<DataColumnInfo>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Columns"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeColumns()
        {
            return Columns.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Columns = reader.ReadNonNullCollection<DataColumnInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            /*

            writer
                .WriteNullable(Name)
                .WriteCollection(Columns);

                */
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<DataTableInfo> verifier
                = new Verifier<DataTableInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name");
            foreach (DataColumnInfo column in Columns)
            {
                verifier.VerifySubObject(column, "Column");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
