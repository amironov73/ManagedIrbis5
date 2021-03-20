// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* WorksheetPage.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("page")]
    public sealed class WorksheetPage
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя страницы.
        /// </summary>
        [XmlElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Элементы страницы.
        /// </summary>
        [XmlElement("item")]
        [JsonPropertyName("items")]
        public NonNullCollection<WorksheetItem> Items { get; private set; }

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
        public WorksheetPage()
        {
            Items = new NonNullCollection<WorksheetItem>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode the page.
        /// </summary>
        public void Encode
            (
                TextWriter writer
            )
        {
            foreach (WorksheetItem item in Items)
            {
                item.Encode(writer);
            }
        }

        /// <summary>
        /// Разбор потока.
        /// </summary>
        public static WorksheetPage ParseStream
            (
                TextReader reader,
                string name,
                int count
            )
        {
            var result = new WorksheetPage
            {
                Name = name
            };

            for (int i = 0; i < count; i++)
            {
                WorksheetItem item = WorksheetItem.ParseStream(reader);
                result.Items.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Items"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeItems()
        {
            return Items.Count != 0;
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
            Items = reader.ReadNonNullCollection<WorksheetItem>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.Write(Items);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<WorksheetPage> verifier
                = new Verifier<WorksheetPage>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name")
                .Assert
                    (
                        Items.Count != 0,
                        "Items.Count != 0"
                    );

            foreach (WorksheetItem item in Items)
            {
                verifier.VerifySubObject
                    (
                        item,
                        "Item"
                    );
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
