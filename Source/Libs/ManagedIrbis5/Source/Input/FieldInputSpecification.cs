// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldInputSpecification.cs -- спецификация ввода одного поля записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Input
{
    /// <summary>
    /// Спецификация ввода одного поля записи.
    /// </summary>
    [XmlRoot ("field")]
    public sealed class FieldInputSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Подсказка.
        /// </summary>
        [XmlAttribute ("hint")]
        [JsonPropertyName ("hint")]
        [Description ("Подсказка")]
        [DisplayName ("Подсказка")]
        public string? Hint { get; set; }

        /// <summary>
        /// Обязательное поле?
        /// </summary>
        [XmlAttribute ("mandatory")]
        [JsonPropertyName ("mandatory")]
        [Description ("Обязательное поле")]
        [DisplayName ("Обязательное поле")]
        public bool Mandatory { get; set; }

        /// <summary>
        /// Повторяющееся поле?
        /// </summary>
        [XmlAttribute ("repeating")]
        [JsonPropertyName ("repeating")]
        [Description ("Повторяющееся поле")]
        [DisplayName ("Повторяющееся поле")]
        public bool Repeating { get; set; }

        /// <summary>
        /// Метка поля (например "200").
        /// </summary>
        [XmlAttribute ("tag")]
        [JsonPropertyName ("tag")]
        [Description ("Метка поля")]
        [DisplayName ("Метка поля")]
        public int Tag { get; set; }

        /// <summary>
        /// Название поля (например "Заглавие").
        /// </summary>
        [XmlAttribute ("title")]
        [JsonPropertyName ("tag")]
        [Description ("Название")]
        [DisplayName ("Название")]
        public string? Title { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Hint = reader.ReadNullableString();
            Mandatory = reader.ReadBoolean();
            Repeating = reader.ReadBoolean();
            Tag = reader.ReadInt32();
            Title = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.WriteNullable (Hint)
                .Write (Mandatory);
            writer.Write (Repeating);
            writer.Write (Tag);
            writer.WriteNullable (Title);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<FieldInputSpecification> (this, throwOnError);

            verifier
                .Assert (Tag > 0)
                .NotNullNorEmpty (Title);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Tag}: {Title.ToVisibleString()}";

        #endregion

    } // class FieldInputSpecification

} // namespace ManagedIrbis.Input
