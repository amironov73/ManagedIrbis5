// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberHierarchy.Global
// ReSharper disable VirtualMemberNeverOverridden.Global

/* TextPrinter.cs -- абстрактный класс для вывода текста на печать.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

#endregion

#nullable enable

namespace AM.Drawing.Printing
{
    /// <summary>
    /// Абстрактный класс для вывода текста на печать.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public abstract class TextPrinter
        : Component
    {
        #region Events

        /// <summary>
        /// Возникает перед началом печати документа.
        /// </summary>
        public event PrintEventHandler? BeginPrint;

        /// <summary>
        /// Возникает после окончания печати документа.
        /// </summary>
        public event PrintEventHandler? EndPrint;

        /// <summary>
        /// Возникает при печати каждой страницы документа.
        /// </summary>
        public event PrintPageEventHandler? PrintPage;

        /// <summary>
        /// Возникает при необходимости настроить страницы перед печатью.
        /// </summary>
        public event QueryPageSettingsEventHandler? QueryPageSettings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the borders.
        /// </summary>
        public RectangleF Borders { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        public int PageNumber
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the page settings.
        /// </summary>
        public PageSettings? PageSettings { get; set; }

        /// <summary>
        /// Gets or sets the print controller.
        /// </summary>
        public PrintController? PrintController { get; set; }

        /// <summary>
        /// Gets or sets the printer settings.
        /// </summary>
        public PrinterSettings? PrinterSettings { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font TextFont { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected TextPrinter()
        {
            Borders = new RectangleF(10f, 10f, 10f, 10f);
            DocumentName = "Text document";
            TextColor = Color.Black;
            TextFont = new Font(FontFamily.GenericSerif, 12f);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Вызывается перед началом печати документа.
        /// </summary>
        protected virtual void OnBeginPrint ( object sender, PrintEventArgs e ) =>
            BeginPrint?.Invoke(this, e);

        /// <summary>
        /// Вызывается по окончании печати документа.
        /// </summary>
        protected virtual void OnEndPrint ( object sender, PrintEventArgs e ) =>
            EndPrint?.Invoke(this, e);

        /// <summary>
        /// Вызывается при печати каждой страницы документа.
        /// </summary>
        protected virtual void OnPrintPage ( object sender, PrintPageEventArgs ea ) =>
            PrintPage?.Invoke(this, ea);

        /// <summary>
        /// Called when [query page settings].
        /// </summary>
        protected virtual void OnQueryPageSettings
            (
                object sender,
                QueryPageSettingsEventArgs e
            )
        {
            ++PageNumber;
            var handler = QueryPageSettings;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Выводит на печать заданный текст.
        /// </summary>
        public virtual bool Print
            (
                string text
            )
        {
            using var document = new PrintDocument
            {
                DocumentName = DocumentName,
                OriginAtMargins = false // TODO: зачем?
            };

            if (PageSettings is not null)
            {
                document.DefaultPageSettings = PageSettings;
            }

            if (PrintController is not null)
            {
                document.PrintController = PrintController;
            }

            if (PrinterSettings is not null)
            {
                document.PrinterSettings = PrinterSettings;
            }

            document.BeginPrint += OnBeginPrint;
            document.EndPrint += OnEndPrint;
            document.PrintPage += OnPrintPage;
            document.QueryPageSettings += OnQueryPageSettings;
            PageNumber = 1;

            document.Print();

            return true;
        }

        #endregion

    } // class Text Printer

} // namespace AM.Drawing.Printing
