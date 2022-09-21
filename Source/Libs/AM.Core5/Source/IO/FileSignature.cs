// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

#pragma warning disable 8618 // property not initialized

/* FileSignature.cs -- сигнатура файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#endregion

#nullable enable

namespace AM.IO;

//
// https://en.wikipedia.org/wiki/List_of_file_signatures
// https://ru.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D1%81%D0%B8%D0%B3%D0%BD%D0%B0%D1%82%D1%83%D1%80_%D1%84%D0%B0%D0%B9%D0%BB%D0%BE%D0%B2
//

/// <summary>
/// Сигнатура файла.
/// </summary>
public sealed class FileSignature
{
    #region Nested classes

    /// <summary>
    /// Алиасы для расширения файла.
    /// </summary>
    private sealed class Extensions
    {
        /// <summary>
        /// Основное расширение (без точки, нижний регистр).
        /// </summary>
        public string Main { get; init; }

        /// <summary>
        /// Алиасы (без точек, нижний регистр).
        /// </summary>
        public string[] Aliases { get; init; }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Известные сигнатуры методов
    /// </summary>
    public static FileSignature[] KnownSignatures
    {
        get
        {
            _InitializeSignatures();

            return _knownSignatures!;
        }
    }

    /// <summary>
    /// Расширение файла (без точки, нижний регистр).
    /// </summary>
    public string Extension { get; init; }

    /// <summary>
    /// Смещение сигнатуры (байты от начала файла).
    /// </summary>
    public int Offset { get; init; }

    /// <summary>
    /// Магические байты.
    /// </summary>
    public byte[] Magic { get; init; }

    #endregion

    #region Constructors

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FileSignature
        (
            string extension,
            byte[] magic,
            int offset = 0
        )
    {
        Extension = extension;
        Offset = offset;
        Magic = magic;
    }

    #endregion

    #region Private members

    private static FileSignature[]? _knownSignatures;

    private static readonly Extensions[] _aliases =
    {
        new ()
        {
            Main = "zip", Aliases = new[] { "jar", "odt", "ods", "odp", "docx", "xlsx", "pptx", "vdsx", "apk", "aar" }
        },
        new () { Main = "tiff", Aliases = new[] { "tif" } },
        new () { Main = "jpeg", Aliases = new[] { "jpg" } },
        new () { Main = "wmv", Aliases = new[] { "wma" } },
        new () { Main = "bmp", Aliases = new[] { "dib" } },
        new () { Main = "midi", Aliases = new[] { "mid" } },
        new () { Main = "djvu", Aliases = new[] { "djv" } },
        new () { Main = "ogg", Aliases = new[] { "oga", "ogv" } },
        new () { Main = "doc", Aliases = new[] { "xls", "ppt", "msg" } },
        new () { Main = "mkv", Aliases = new[] { "mka", "mks", "mk3d", "webm" } }
    };

    private static void _InitializeSignatures()
    {
        if (_knownSignatures is not null)
        {
            return;
        }

        var list = new List<FileSignature>();
        var properties = typeof (FileSignature)
            .GetProperties (BindingFlags.Static | BindingFlags.Public);
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof (FileSignature))
            {
                list.Add ((FileSignature)property.GetValue (null)!);
            }
        }

        _knownSignatures = list.ToArray();
    } // method _InitializeSignatures

    /// <summary>
    /// Находим алиас для указанного расширения.
    /// </summary>
    private static string _FindAlias
        (
            string extension
        )
    {
        if (extension.StartsWith (".", StringComparison.Ordinal))
        {
            extension = extension[1..];
        }

        foreach (var outer in _aliases)
        {
            foreach (var inner in outer.Aliases)
            {
                if (inner.SameString (extension))
                {
                    return outer.Main;
                }
            }
        }

        return extension;
    }

    #endregion

    #region Known signatures

    /// <summary>
    /// Advanced Systems Format.
    /// </summary>
    public static readonly FileSignature Asf = new ("asf",
        new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 });

    /// <summary>
    /// Audio Video Interleave video format.
    /// </summary>
    public static readonly FileSignature Avi = new ("", new byte[] { 0x52, 0x49, 0x46, 0x46 });

    /// <summary>
    /// BMP file, a bitmap format used mostly in the Windows world.
    /// </summary>
    public static readonly FileSignature Bmp = new ("", new byte[] { 0x42, 0x4D });

    /// <summary>
    /// Compressed file using Bzip2 algorithm.
    /// </summary>
    public static readonly FileSignature Bz2 = new ("bz2", new byte[] { 0x42, 0x5A, 0x68 });

    /// <summary>
    /// Microsoft Cabinet file.
    /// </summary>
    public static readonly FileSignature Cab = new ("cab", new byte[] { 0x4D, 0x53, 0x43, 0x46 });

    /// <summary>
    /// Java class file.
    /// </summary>
    public static readonly FileSignature Class = new ("class", new byte[] { 0xCA, 0xFE, 0xBA, 0xBE });

    /// <summary>
    /// Debian package file.
    /// </summary>
    public static readonly FileSignature Deb = new ("deb", new byte[] { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E });

    /// <summary>
    /// Compound File Binary Format, a container format used
    /// for document by older versions of Microsoft Office.
    /// It is however an open format used by other programs as well.
    /// </summary>
    public static readonly FileSignature Doc = new ("doc",
        new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });

    /// <summary>
    /// DjVu document.
    /// </summary>
    public static readonly FileSignature Djvu = new ("djvu",
        new byte[] { 0x41, 0x54, 0x26, 0x54, 0x46, 0x4F, 0x52, 0x4D });

    /// <summary>
    /// Executable and Linkable Format.
    /// </summary>
    public static readonly FileSignature Elf = new ("elf", new byte[] { 0x7F, 0x45, 0x4C, 0x46 });

    /// <summary>
    /// DOS MZ executable file format and its descendants (including NE and PE).
    /// </summary>
    public static readonly FileSignature Exe = new ("exe", new byte[] { 0x4D, 0x5A });

    /// <summary>
    /// Free Lossless Audio Codec.
    /// </summary>
    public static readonly FileSignature Flac = new ("flac", new byte[] { 0x66, 0x4C, 0x61, 0x43 });

    /// <summary>
    /// Image file encoded in the Graphics Interchange Format (GIF).
    /// </summary>
    public static readonly FileSignature Gif87a = new ("gif", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 });

    /// <summary>
    /// Image file encoded in the Graphics Interchange Format (GIF).
    /// </summary>
    public static readonly FileSignature Gif89a = new ("gif", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 });

    /// <summary>
    /// GNU ZIP.
    /// </summary>
    public static readonly FileSignature Gz = new ("gz", new byte[] { 0x1F, 0x8B });

    /// <summary>
    /// Computer icon encoded in ICO file format.
    /// </summary>
    public static readonly FileSignature Ico = new ("ico", new byte[] { 0x00, 0x00, 0x01, 0x00 });

    /// <summary>
    /// ISO9660 CD/DVD image file.
    /// </summary>
    public static readonly FileSignature Iso = new ("iso", new byte[] { 0x43, 0x44, 0x30, 0x30, 0x31 });

    /// <summary>
    /// lzip compressed file.
    /// </summary>
    public static readonly FileSignature Lz = new ("lz", new byte[] { 0x4C, 0x5A, 0x49, 0x50 });

    /// <summary>
    /// MIDI sound file.
    /// </summary>
    public static readonly FileSignature Midi = new ("midi", new byte[] { 0x4D, 0x54, 0x68, 0x64 });

    /// <summary>
    /// Matroska media container, including WebM.
    /// </summary>
    public static readonly FileSignature Mkv = new ("", new byte[] { 0x1A, 0x45, 0xDF, 0xA3 });

    /// <summary>
    /// MP3 file with an ID3v2 container.
    /// </summary>
    public static readonly FileSignature Mp3 = new ("mp3", new byte[] { 0x49, 0x44, 0x33 });

    /// <summary>
    /// Ogg, an open source media container format.
    /// </summary>
    public static readonly FileSignature Ogg = new ("ogg", new byte[] { 0x4F, 0x67, 0x67, 0x53 });

    /// <summary>
    /// PDF document.
    /// </summary>
    public static readonly FileSignature Pdf = new ("pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 });

    /// <summary>
    /// Image encoded in the Portable Network Graphics format.
    /// </summary>
    public static readonly FileSignature Png = new ("png",
        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });

    /// <summary>
    /// PostScript document.
    /// </summary>
    public static readonly FileSignature Ps = new ("ps", new byte[] { 0x25, 0x21, 0x50, 0x53 });

    /// <summary>
    /// Photoshop Document file, Adobe Photoshop’s native file format.
    /// </summary>
    public static readonly FileSignature Psd = new ("", new byte[] { 0x38, 0x42, 0x50, 0x53 });

    /// <summary>
    /// RAR archive version 1.50 onwards.
    /// </summary>
    public static readonly FileSignature Rar = new ("rar", new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 });

    /// <summary>
    /// RAR archive version 5.0 onwards.
    /// </summary>
    public static readonly FileSignature Rar5 = new ("rar",
        new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00 });

    /// <summary>
    /// Rich Text Format.
    /// </summary>
    public static readonly FileSignature Rtf = new ("rtf", new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 });

    /// <summary>
    /// 7-Zip File Format.
    /// </summary>
    public static readonly FileSignature SevenZip = new ("7z", new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C });

    /// <summary>
    /// tar archive.
    /// </summary>
    public static readonly FileSignature Tar = new ("tar",
        new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x00, 0x30, 0x30 }, 0x101);

    /// <summary>
    /// Tagged Image File Format (big endian format)
    /// </summary>
    public static readonly FileSignature TiffBigEndian = new ("tiff", new byte[] { 0x4D, 0x4D, 0x00, 0x2A });

    /// <summary>
    /// Tagged Image File Format (little endian format)
    /// </summary>
    public static readonly FileSignature TiffLittleEndian = new ("tiff", new byte[] { 0x49, 0x49, 0x2A, 0x00 });

    /// <summary>
    /// tar archive.
    /// </summary>
    public static readonly FileSignature Ustar = new ("tar",
        new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20, 0x00 }, 0x101);

    /// <summary>
    /// Windows media video.
    /// </summary>
    public static readonly FileSignature Vmv = new ("wmv",
        new byte[] { 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C });

    /// <summary>
    /// WebAssembly binary format.
    /// </summary>
    public static readonly FileSignature Wasm = new ("wasm", new byte[] { 0x6D, 0x73, 0x61, 0x00 });

    /// <summary>
    /// Waveform Audio File Format.
    /// </summary>
    public static readonly FileSignature Wav = new ("wav", new byte[] { 0x52, 0x49, 0x46, 0x46 });

    /// <summary>
    /// ZIP file format and formats based on it, such as JAR, ODF, OOXML.
    /// </summary>
    public static readonly FileSignature Zip = new ("zip", new byte[] { 0x50, 0x4B, 0x03, 0x04 });

    /// <summary>
    /// ZIP file format and formats based on it, such as JAR, ODF, OOXML (empty archive).
    /// </summary>
    public static readonly FileSignature ZipEmpty = new ("zip", new byte[] { 0x50, 0x4B, 0x05, 0x06 });

    /// <summary>
    /// ZIP file format and formats based on it, such as JAR, ODF, OOXML (spanned archive).
    /// </summary>
    public static readonly FileSignature ZipSpanned = new ("zip", new byte[] { 0x50, 0x4B, 0x07, 0x08 });

    /// <summary>
    /// Compressed file (often tar zip) using Lempel-Ziv-Welch algorithm.
    /// </summary>
    public static readonly FileSignature Zlzw = new ("z", new byte[] { 0x1F, 0x9D });

    /// <summary>
    /// Compressed file (often tar zip) using LZH algorithm.
    /// </summary>
    public static readonly FileSignature Zlzh = new ("z", new byte[] { 0x1F, 0xA0 });

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, содержит ли указанный поток данную сигнатуру.
    /// </summary>
    public unsafe bool Check
        (
            Stream stream
        )
    {
        var savePosition = stream.Position;
        stream.Position = Offset;
        Span<byte> buffer = stackalloc byte[Magic.Length];
        var actual = stream.Read (buffer);
        stream.Position = savePosition;
        if (actual != buffer.Length)
        {
            return false;
        }

        return Utility.CompareSpans (Magic.AsSpan(), buffer) == 0;
    }

    /// <summary>
    /// Проверка, содержит ли указанный файл данную сигнатуру.
    /// </summary>
    public bool Check
        (
            string fileName
        )
    {
        using var stream = File.OpenRead (fileName);

        return Check (stream);
    }

    /// <summary>
    /// Проверка указанного файла.
    /// </summary>
    /// <returns><c>true</c> когда файл содержит соответствующую сигнатуру
    /// или является неизвестным типом файла.</returns>
    public static bool CheckFile
        (
            string fileName
        )
    {
        var extension = Path.GetExtension (fileName);
        if (string.IsNullOrEmpty (extension))
        {
            return true;
        }

        _InitializeSignatures();

        extension = extension.ToLowerInvariant();
        extension = _FindAlias (extension);

        var known = false;
        foreach (var signature in _knownSignatures!)
        {
            if (extension.SameString (signature.Extension))
            {
                if (signature.Check (fileName))
                {
                    return true;
                }

                // дадим еще одну попытку, вдруг есть альтернативная сигнатура
                // например, в GIF - версии от 87 и 89 года
                known = true;
            }
        }

        // незнакомые файлы считаем правильными
        return !known;
    }

    #endregion
}
