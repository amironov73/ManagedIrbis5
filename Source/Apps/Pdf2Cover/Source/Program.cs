// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

/*
    Утилита для создания обложек для PDF-файлов.
    Фактически отрисовывается первая страница.
 */

#region Using directives

using System;
using System.Drawing.Imaging;
using System.IO;

using DevExpress.Pdf;

#endregion

namespace Pdf2Cover;

/// <summary>
/// Единственный класс, содержащий всю функциональность утилиты.
/// </summary>
internal sealed class Program
{
    private static void ProcessPdf
        (
            string filename
        )
    {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument (filename);
        if (processor.Document.Pages.Count < 1)
        {
            return;
        }

        using var bitmap = processor.CreateBitmap (1, 300);
        var pageName = Path.ChangeExtension (filename, ".jpg");
        bitmap.Save (pageName);
    }

    /// <summary>
    /// Собственно точка входа в программу.
    /// </summary>
    private static int Main
        (
            string[] args
        )
    {
        try
        {
            if (args.Length != 0)
            {
                ProcessPdf (args[0]);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }

}
