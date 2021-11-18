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

/* Pii.cs -- Publisher Item Identifier
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
    // См. https://ru.wikipedia.org/wiki/Publisher_Item_Identifier
    //
    // Publisher Item Identifier (PII) — уникальный идентификатор,
    // применяемый некоторыми научными журналами для идентификации
    // научных работ. Он основан на более ранних идентификаторах
    // ISSN и ISBN, к которым добавлены символ для уточнения типа
    // публикации, номер сущности и контрольная цифра.
    //
    // Системой PII пользуются с 1996 года издатели American Chemical
    // Society, American Institute of Physics, American Physical
    // Society, Elsevier и IEEE.
    //
    // Формат
    //
    // Идентификатор PII представляет собой строку из 17 символов, состоящую из:
    //
    // Символ типа публикации: «S» означает периодическое издание
    // и код ISSN, «B» — книги и код ISBN.
    // ISSN (8 цифр) или ISBN (10 цифр)
    // для периодики добавлено 2 цифры, чтобы выровнять длину кода.
    // Часто используется 2 последние цифры года, в который произошло
    // присвоение номера PII.
    // пятизначный код, присвоенный данной работе издателем.
    // Должен быть уникален в рамках данного журнала или книги
    // контрольная цифра (0-9 или X)
    // При печати код PII может быть дополнен знаками пунктуации для
    // упрощения чтения, например, Sxxxx-xxxx(yy)iiiii-d
    // или Bx-xxx-xxxxx-x/iiiii-d.
    //

    /// <summary>
    /// Publisher Item Identifier.
    /// </summary>
    [XmlRoot ("pii")]
    public sealed class Pii
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
            var verifier = new Verifier<Pii> (this, throwOnError);

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
