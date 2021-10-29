// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PrinterInfo.cs -- настройки принтера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms.Printing
{
    /// <summary>
    /// Настройки принтера.
    /// </summary>
    [XmlRoot("printer")]
    public class PrinterInfo
    {
        #region Properties

        /// <summary>
        /// Как Windows называет принтер.
        /// </summary>
        [XmlElement("name")]
        [DisplayName("Принтер")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Ширина страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("width")]
        [DisplayName("Ширина страницы")]
        [JsonPropertyName("width")]
        public int PageWidth { get; set; }

        /// <summary>
        /// Высота страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("height")]
        [DisplayName("Высота страницы")]
        [JsonPropertyName("height")]
        public int PageHeight { get; set; }

        /// <summary>
        /// Использовать портретную ориентацию?
        /// </summary>
        [XmlElement("landscape")]
        [DisplayName("Портретная ориентация")]
        [JsonPropertyName("landscape")]
        public bool Landscape { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Принтер Fargo HDP5000.
        /// </summary>
        public static PrinterInfo GetFargoHdp5000()
        {
            var fargo = new PrinterInfo
            {
                Name = "HDP5000 Card Printer",
                PageWidth = 220,
                PageHeight = 345,
                Landscape = true
            };

            return fargo;
        }

        /// <summary>
        /// Загрузка из XML-файла.
        /// </summary>
        public static PrinterInfo LoadXml
            (
                string fileName
            )
        {
            using var stream = File.OpenRead (fileName);
            var serializer = new XmlSerializer (typeof (PrinterInfo));

            return (PrinterInfo) serializer.Deserialize (stream).ThrowIfNull();
        }

        /// <summary>
        /// Сохранение в XML-файле.
        /// </summary>
        public void SaveXml
            (
                string fileName
            )
        {
            using var stream = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(PrinterInfo));
            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Верификация данных о принтере.
        /// </summary>
        public void Verify()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ApplicationException("Не задано имя принтера");
            }

            if (PageWidth <= 0)
            {
                throw new ApplicationException("Задана неверная ширина страницы");
            }

            if (PageHeight <= 0)
            {
                throw new ApplicationException("Задана неверная высота страницы");
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Name}: {PageWidth} x {PageHeight}";

        #endregion

    } // class PrinterInfo

} // namespace AM.Windows.Forms.Printing
