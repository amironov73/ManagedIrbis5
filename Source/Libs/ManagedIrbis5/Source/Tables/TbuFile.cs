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

/* TbuFile.cs -- файл TBU
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    //
    // Official documentation
    //
    // Файл с расширением TBU представляет из себя файл описания формы. Он содержит три секции:
    // [FormatCode]
    // [Tab]
    // [Header]
    //
    // В секции FormatCode указывается кодировка данных.
    // Обычно это кодировка WIN.
    // После указания кодировки должен следовать
    // признак логического конца секции ***** (5 звезд).
    // Таким образом в частном случае секция FormatCode
    // практически всегда имеет вид:
    // [FormatCode]
    // WIN
    // *****
    //
    // Секция Tab задает начало форматирования документа.
    // Обычно в этой секции указываются строки,
    // инициализирующие размер страницы и начало тела документа.
    // Концом этой секции считается объявление следующей секции Header.
    //
    // Секция Header содержит строки, которыми будет закрыты данные,
    // сформированные из файла с расширением SRW.
    // Обычно это команды закрытия заголовочной части формы.
    //

    //
    // Пример секции Tab:
    //
    // [Tab]
    // \paperw11907\paperh16727\margl1134\margr1134\margt567\margb1134
    // {\b\fs24
    // БИБЛИОГРАФИЧЕСКИЙ УКАЗАТЕЛЬ КНИГ, ПОСТУПИВШИХ В БИБЛИОТЕКУ
    // \par
    // }
    // {}
    // {\b\fs32 \qc
    //

    //
    // Пример секции Header:
    //
    // [Header]
    // \b0
    // }
    // {}
    // \par }
    //
    /// <summary>
    /// Файл описания формы ИРБИС64.
    /// </summary>
    public sealed class TbuFile
    {
        #region Properties

        /// <summary>
        /// FormatCode section.
        /// </summary>
        [XmlElement ("encoding")]
        [JsonPropertyName ("encoding")]
        public string? Encoding { get; set; }

        /// <summary>
        /// Table section.
        /// </summary>
        [XmlElement ("table")]
        [JsonPropertyName ("table")]
        public string? Table { get; set; }

        /// <summary>
        /// Header section.
        /// </summary>
        [XmlElement ("header")]
        [JsonPropertyName ("header")]
        public string? Header { get; set; }

        #endregion
    }
}
