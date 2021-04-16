// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    [XmlRoot("card")]
    public class CardInfo
    {
        #region Properties

        [XmlElement("width")]
        [DisplayName("Ширина")]
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        public int Height { get; set; }

        [XmlElement("background")]
        [DisplayName("Фоновый рисунок")]
        public string Background { get; set; }

        [XmlArray("items")]
        [DisplayName("Элементы")]
        //[Editor(typeof(CardItemCollectionEditor), typeof(CollectionEditor))]
        [XmlArrayItem(typeof(CardLabel), ElementName = "label")]
        [XmlArrayItem(typeof(CardText), ElementName = "text")]
        [XmlArrayItem(typeof(CardPicture), ElementName = "picture")]
        [XmlArrayItem(typeof(CardBarcode), ElementName = "barcode")]
        [XmlArrayItem(typeof(CardRectangle), ElementName = "rectangle")]
        public List<CardItem> Items
        {
            get { return _items; }
        }

        #endregion

        #region Private members

        private readonly List<CardItem> _items
            = new List<CardItem>();

        #endregion

        #region Public methods

        public static CardInfo CreateDefaultCard()
        {
            CardInfo card = new CardInfo
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

        public void Verify()
        {
            // Nothing to do yet
        }

        public void SaveXml(string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                XmlSerializer serializer
                    = new XmlSerializer(typeof(CardInfo));
                serializer.Serialize(stream, this);
            }
        }

        public static CardInfo LoadXml(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                XmlSerializer serializer
                    = new XmlSerializer(typeof(CardInfo));
                return (CardInfo) serializer.Deserialize(stream);
            }
        }

        public Image Draw(DrawingContext context)
        {
            Bitmap bitmap = new Bitmap(Width, Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                context.Graphics = graphics;
                if (!string.IsNullOrEmpty(Background))
                {
                    Image backImage = Image.FromFile(Background);
                    graphics.DrawImage
                    (
                        backImage,
                        0,
                        0,
                        Width,
                        Height
                    );
                }

                foreach (CardItem item in Items)
                {
                    item.Draw(context);
                }

                context.Graphics = null;
            }

            return bitmap;
        }

        #endregion
    }
}
