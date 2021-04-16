// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Barman.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    ///
    /// </summary>
    public class Barman
    {
        #region Events

        /// <summary>
        /// Occurs when [data changed].
        /// </summary>
        public event EventHandler DataChanged;

        #endregion

        #region Properties

        // /// <summary>
        // /// Gets or sets the printer.
        // /// </summary>
        // /// <value>
        // /// The printer.
        // /// </value>
        // public PrinterInfo Printer
        // {
        //     get { return _printer; }
        //     set
        //     {
        //         _printer = value;
        //         OnDataChanged();
        //     }
        // }
        //
        // /// <summary>
        // /// Gets or sets the human.
        // /// </summary>
        // /// <value>
        // /// The human.
        // /// </value>
        // public HumanInfo Human
        // {
        //     get { return _human; }
        //     set
        //     {
        //         _human = value;
        //         OnDataChanged();
        //     }
        // }

        /// <summary>
        /// Gets or sets the card.
        /// </summary>
        /// <value>
        /// The card.
        /// </value>
        public CardInfo Card
        {
            get { return _card; }
            set
            {
                _card = value;
                OnDataChanged();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Barman"/> class.
        /// </summary>
        public Barman()
        {
            //_human = new HumanInfo();
            //_printer = PrinterInfo.GetDefaultPrinterInfo();
        }

        #endregion

        #region Private members

        //private PrinterInfo _printer;

        //private HumanInfo _human;

        private CardInfo _card;

        private void OnDataChanged()
        {
            var dataChanged = DataChanged;
            if (dataChanged != null)
            {
                dataChanged(this, EventArgs.Empty);
            }
        }

        private Image _image;

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            e.Graphics.DrawImage(_image, 0, 0, e.PageBounds.Width, e.PageBounds.Height);
            e.HasMorePages = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Load(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension != null)
            {
                extension = extension.ToLowerInvariant();
            }

            switch (extension)
            {
                case ".card":
                    Card = CardInfo.LoadXml(fileName);
                    break;

                case ".printer":
                    // Printer = PrinterInfo.LoadXml(fileName);
                    break;

                case ".reader":
                    //Human = HumanInfo.LoadXml(fileName);
                    break;

                case ".irbis":
                    // Human = HumanInfo.LoadIrbis(fileName);
                    break;

                case ".iso":
                    // Human = HumanInfo.LoadIso(fileName);
                    break;

                default:
                    throw new ApplicationException("Файл неизвестного формата");
            }
        }

        /// <summary>
        /// Verifies this instance.
        /// </summary>
        public void Verify()
        {
            // if (Printer == null)
            // {
            //     throw new ApplicationException("Не задан принтер");
            // }
            //
            // if (Human == null)
            // {
            //     throw new ApplicationException("Не задан читатель");
            // }

            if (Card == null)
            {
                throw new ApplicationException("Не задана карточка");
            }

            //Printer.Verify();
            //Human.Verify();
            Card.Verify();
        }

        /// <summary>
        /// Draws the card.
        /// </summary>
        /// <returns></returns>
        public Image DrawCard()
        {
            // if (Card == null)
            // {
            //     MessageBox.Show("Не задана карта!");
            //     return null;
            // }
            //
            // if (Human == null)
            // {
            //     MessageBox.Show("Не задан читатель!");
            //     return null;
            // }

            //DrawingContext context = new DrawingContext
            //                            {
            //                                Printer = Printer,
            //                                Card = Card,
            //                                Human = Human
            //                            };

            //Image result = Card.Draw(context);

            //return result;

            // var source = new BindingSource
            // {
            //     DataSource = Human
            // };
            // var report = new CardReport2
            // {
            //     DataSource = source
            // };
            //
            // var options
            //     = new ImageExportOptions(ImageFormat.Bmp)
            //     {
            //         ExportMode = ImageExportMode.SingleFilePageByPage,
            //         Resolution = 300
            //     };

            // var stream = new MemoryStream();
            //
            // report.ExportToImage(stream, options);
            // stream.Seek(0, SeekOrigin.Begin);
            //
            // var result = Image.FromStream(stream);

//			result.Save ("result.bmp");

            // return result;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Prints the card.
        /// </summary>
        /// <param name="image">The image.</param>
        public void PrintCard(Image image)
        {
            Verify();

            /*

            _image = image;
            if (_image == null)
            {
                MessageBox.Show("Не сформировано изображение для печати!");
                return;
            }

            var document = new PrintDocument
            {
                DocumentName = "Карточка читателя"
            };
            document.PrintPage += document_PrintPage;

            document.PrinterSettings.PrinterName = Printer.Name;
            document.DefaultPageSettings.PaperSize = new PaperSize
                (
                    "Reader card",
                    Printer.PageWidth,
                    Printer.PageHeight
                );
            document.DefaultPageSettings.Landscape = Printer.Landscape;

            document.Print();

            */
        }

        /// <summary>
        /// Parses the command line.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public void ParseCommandLine(string[] arguments)
        {
            foreach (var fileName in arguments)
            {
                Load(fileName);
            }
        }

        // /// <summary>
        // /// Sets the reader info.
        // /// </summary>
        // /// <param name="reader">The reader.</param>
        // public void SetReaderInfo(ReaderInfo reader)
        // {
        //     if ((reader == null)
        //         || string.IsNullOrEmpty(reader.Ticket))
        //     {
        //         return;
        //     }
        //
        //     var human = new HumanInfo
        //     {
        //         Address = reader.Address,
        //         Barcode = reader.Barcode,
        //         Birthdate = reader.Birthdate,
        //         Cathedra = reader.Cathedra,
        //         Cathegory = reader.Category,
        //         Department = reader.Department,
        //         Email = reader.Mail,
        //         Group = reader.Group,
        //         JobTitle = reader.JobTitle,
        //         Name = reader.Name,
        //         Phone = reader.Phone,
        //         Registration = reader.Registered,
        //         Reregistration = reader.Reregistered
        //             .ToString(CultureInfo.InvariantCulture),
        //         Ticket = reader.Ticket
        //     };
        //     using (IReaderManager manager = new ReaderManager())
        //     {
        //         var photo = manager.GetPhoto(reader.Ticket);
        //         human.ActualImage = photo;
        //     }
        //
        //     Human = human;
        // }

        #endregion
    }
}
