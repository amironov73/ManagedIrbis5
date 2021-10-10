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

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.Collections;

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
    {
        #region Properties

        /// <summary>
        /// Ключи сортировки.
        /// </summary>
        [XmlElement ("key")]
        [JsonPropertyName ("keys")]
        public NonNullCollection<KeyDefinition> Keys { get; } = new ();

        #endregion

    } // class SrwFile

} // namespace ManagedIrbis.Tables
