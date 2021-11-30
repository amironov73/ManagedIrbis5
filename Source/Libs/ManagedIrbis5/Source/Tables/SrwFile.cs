// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SrwFile.cs -- SRW-файл - файл сортировки таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    //
    // Official documentation
    //
    // Файл сортировки имеет расширение .srw.
    // В нем задается количество заголовков в документе,
    // их содержание и форматирование.
    //
    // Файл содержит 3 секции:
    // HeaderNumber
    // HeaderFormat
    // KeyOptions
    //
    // Первые две секции отвечают за содержание
    // заголовков сортировки.
    // Поскольку сортировка может быть множественной,
    // то и для каждого уровня сортировки возможно
    // задать свой заголовок.
    // Ключи сортировки задаются в третьей секции KeyOptions.
    // Секция KeyOptions может состоять из нескольких строк.
    // Однако стоит помнить, что количество строк в этой
    // секции должно быть кратно тройке, поскольку
    // каждый ключ сортировки описывается 3-я строками:
    // длина ключа сортировки, режим сортировки
    // и формат выбора значения сортировки.
    // Длина ключа задается целым числом,
    // режим сортировки может быть 0 (единственный ключ)
    // или 1 (множественный ключ).
    // В режиме "единственный ключ" только первая строка
    // (если она есть) результата форматирования становится
    // ключом сортировки.
    // В режиме "множественный ключ" каждая строка
    // результата форматирования становится ключом сортировки.
    //
    // При написании формата заголовков могут быть использованы
    // условные поля - Vi, где i - номер ключа сортировки.
    // Форматы заголовков (если их больше одного)
    // указываются через разделитель "/".
    //

    /// <summary>
    /// Файл сортировки таблицы.
    /// </summary>
    [XmlRoot ("sorting")]
    public sealed class SrwFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Ключи сортировки.
        /// </summary>
        [XmlElement ("key")]
        [JsonPropertyName ("keys")]
        [Description ("Ключи сортировки")]
        public NonNullCollection<KeyDefinition> Keys { get; } = new ();

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            reader.ReadCollection (Keys);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteCollection (Keys);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<SrwFile> (this, throwOnError);

            // TODO implement

            verifier
                .Positive (Keys.Count);

            foreach (var key in Keys)
            {
                verifier.VerifySubObject (key);
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"Keys: {Keys.Count}";
        }

        #endregion
    }
}
