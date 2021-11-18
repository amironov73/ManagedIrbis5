// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Gdti.cs -- GDTI
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
    // https://ru.wikipedia.org/wiki/Global_Document_Type_Identifier
    //
    // Глобально-уникальный идентификатор типа документа (GDTI) - это
    // часть системы стандартов, поддерживаемой GS1. Это простой
    // инструмент для однозначной идентификации документа по типу.
    //
    // Термин "документ" применяется в широком смысле и охватывать
    // любые официальные или частные документы, которые подразумевают
    // право (например, доказательство владения) или обязательство
    // (например, уведомления или вызова на военную службу)
    // на предъявителя. Эмитент документа обычно отвечает
    // за все сведения, содержащиеся в документе, как со штрихкодом,
    // так в представлении, рассчитанном на чтение человеком.
    // Такие документы обычно требуют для хранения соответствующей
    // информации, содержащейся в документе. Примерами документов,
    // которые могут иметь GDTI, являются налоговые требования,
    // документы, подтверждающие отгрузку, страховых полисов,
    // внутренних накладных и т. п. Компания создает GDTI, когда
    // важно сохранить запись о документе. GDTI может содержать ссылку
    // на базу данных, которая содержит мастер-копию документа.
    // GDTI может использоваться в штрихкоде формата GS1-128
    // и печататься на документе в качестве идентификатора.
    //

    //
    // https://en.wikipedia.org/wiki/Global_Document_Type_Identifier
    //
    // The Global Document Type Identifier (GDTI) is part of the GS1
    // system of standards. It is a simple tool to identify a document
    // by type and can identify documents uniquely where required.
    //
    // The term “document” is applied broadly to cover any official
    // or private papers that confer a right (e.g. a proof of ownership)
    // or obligation (e.g., notification or call for military service)
    // upon the bearer. The issuer of the document is normally responsible
    // for all the information contained upon the document, both bar coded
    // and Human Readable Interpretation. Such documents typically
    // require storage of the appropriate information contained
    // on the document. Examples of the kind of documents that
    // could have a GDTI are tax demands, proof of shipment forms,
    // insurance policies, internal invoices etc. A company or business
    // will issue a GDTI where it is important to maintain a record
    // of the document. The GDTI will provide a link to the database
    // that holds the ‘master’ copy of the document. The GDTI may be
    // produced as a GS1-128 bar code and printed on the document
    // as a method of identification or for detail or information retrieval.
    //

    /// <summary>
    /// Global Document Type Identifier.
    /// </summary>
    [XmlRoot ("gdti")]
    public sealed class Gdti
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
            var verifier = new Verifier<Gdti> (this, throwOnError);

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
