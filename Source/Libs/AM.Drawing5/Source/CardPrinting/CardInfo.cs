// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardInfo.cs -- описание карточки, на которую будет выведена информация
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    /// Описание карточки, на которую будет выведена информация.
    /// </summary>
    [XmlRoot("card")]
    public class CardInfo
    {
        #region Properties

        /// <summary>
        /// Ширина.
        /// </summary>
        [XmlElement("width")]
        [DisplayName("Ширина")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [XmlElement("height")]
        [DisplayName("Высота")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// Фоновый рисунок.
        /// </summary>
        [XmlElement("background")]
        [DisplayName("Фоновый рисунок")]
        [JsonPropertyName("background")]
        public string? Background { get; set; }

        /// <summary>
        /// Элементы карточки.
        /// </summary>
        [XmlArray("items")]
        [DisplayName("Элементы")]
        //[Editor(typeof(CardItemCollectionEditor), typeof(CollectionEditor))]
        [XmlArrayItem(typeof(CardLabel), ElementName = "label")]
        [XmlArrayItem(typeof(CardText), ElementName = "text")]
        [XmlArrayItem(typeof(CardPicture), ElementName = "picture")]
        [XmlArrayItem(typeof(CardBarcode), ElementName = "barcode")]
        [XmlArrayItem(typeof(CardRectangle), ElementName = "rectangle")]
        public List<CardItem> Items { get; } = new List<CardItem>();

        #endregion

        #region Public methods

        /// <summary>
        /// Карточка по умолчанию (для ИРНИТУ).
        /// </summary>
        public static CardInfo CreateDefaultCard()
        {
            var card = new CardInfo
            {
                Background = "Bottom_texture_hf.bmp",
                Width = 3508,
                Height = 2240
            };

            card.Items.AddRange
            (
                new CardItem[]
                {
                    new CardLabel
                    {
                        Font = "Segoe UI Light; 80pt; style=Bold",
                        Color = "White",
                        Left = 720,
                        Top = 50,
                        Text = "Иркутский государственный технический университет"
                    },

                    new CardLabel
                    {
                        Font = "Segoe UI; 96pt; style=Bold",
                        Color = "White",
                        Left = 900,
                        Top = 180,
                        Text = "НАУЧНО-ТЕХНИЧЕСКАЯ БИБЛИОТЕКА"
                    }
                }
            );

            return card;
        }

        /// <summary>
        /// Проверка карточки.
        /// </summary>
        public void Verify()
        {
            // Nothing to do yet
        }

        /// <summary>
        /// Сохранение карточки со всеми элементами в указанный XML-файл.
        /// </summary>
        public void SaveXml
            (
                string fileName
            )
        {
            using var stream = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(CardInfo));
            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Загрузка карточки из указанного файла.
        /// </summary>
        public static CardInfo LoadXml
            (
                string fileName
            )
        {
            using var stream = File.OpenRead (fileName);
            var serializer = new XmlSerializer (typeof (CardInfo));

            return (CardInfo) serializer.Deserialize (stream).ThrowIfNull();

        } // method LoadXml

        /// <summary>
        /// Отрисовка карточки.
        /// </summary>
        public Image Draw
            (
                DrawingContext context
            )
        {
            var bitmap = new Bitmap(Width, Height);
            using var graphics = Graphics.FromImage(bitmap);
            context.Graphics = graphics;
            if (!string.IsNullOrEmpty(Background))
            {
                var backImage = Image.FromFile(Background);
                graphics.DrawImage
                    (
                        backImage,
                        0,
                        0,
                        Width,
                        Height
                    );
            }

            foreach (var item in Items)
            {
                item.Draw(context);
            }

            context.Graphics = null;

            return bitmap;

        } // method Draw

        #endregion

    } // class CardInfo

} // namespace AM.Drawing.CardPrinting
