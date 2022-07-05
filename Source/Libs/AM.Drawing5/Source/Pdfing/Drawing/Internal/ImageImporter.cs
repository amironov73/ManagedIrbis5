// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ImageImporter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.Internal
{
    /// <summary>
    /// The class that imports images of various formats.
    /// </summary>
    internal class ImageImporter
    {
        // TODO Make a singleton!
        /// <summary>
        /// Gets the image importer.
        /// </summary>
        public static ImageImporter GetImageImporter()
        {
            return new ImageImporter();
        }

        private ImageImporter()
        {
            _importers.Add(new ImageImporterJpeg());
            _importers.Add(new ImageImporterBmp());
            // TODO: Special importer for PDF? Or dealt with at a higher level?
        }

        /// <summary>
        /// Imports the image.
        /// </summary>
        public ImportedImage ImportImage(Stream stream, PdfDocument document)
        {
            StreamReaderHelper helper = new StreamReaderHelper(stream);

            // Try all registered importers to see if any of them can handle the image.
            foreach (IImageImporter importer in _importers)
            {
                helper.Reset();
                ImportedImage image = importer.ImportImage(helper, document);
                if (image != null)
                    return image;
            }
            return null;
        }

        private readonly List<IImageImporter> _importers = new List<IImageImporter>();
    }
}
