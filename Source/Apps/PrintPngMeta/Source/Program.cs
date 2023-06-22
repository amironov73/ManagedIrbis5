// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using SixLabors.ImageSharp;

#endregion

#nullable enable

namespace PrintPngMeta;

internal static class Program
{
    private static void ProcessFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var image = Image.Load (fileName);

        Console.WriteLine ($"{fileName}: size = {image.Size}");
        Console.WriteLine ($"\tbits = {image.PixelType.BitsPerPixel}");

        var genericMeta = image.Metadata;
        var pngMeta = genericMeta?.GetPngMetadata();
        if (pngMeta is null)
        {
            Console.Error.WriteLine ($"{fileName}: can't get PNG metadata");
            return;
        }

        foreach (var data in pngMeta.TextData)
        {
            Console.WriteLine ($"\t{data.Keyword} = {data.Value}");
        }

        Console.WriteLine();
    }

    public static void Main
        (
            string[] args
        )
    {
        foreach (var fileName in args)
        {
            ProcessFile (fileName);
        }
    }
}
