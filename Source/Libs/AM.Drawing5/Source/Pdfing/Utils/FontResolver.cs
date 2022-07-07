// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontResolver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;

using PdfSharpCore.Internal;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;

using SixLabors.Fonts;

#endregion

#nullable enable

namespace PdfSharpCore.Utils;

/// <summary>
/// Отыскивает шрифты по описанию
/// </summary>
public class FontResolver
    : IFontResolver
{
    public string DefaultFontName => "Arial";

    private static readonly Dictionary<string, FontFamilyModel> InstalledFonts = new ();

    private static readonly string[] SSupportedFonts;

    public FontResolver()
    {
    }

    static FontResolver()
    {
        string fontDir;

        var isOSX = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform (System.Runtime.InteropServices
            .OSPlatform.OSX);
        if (isOSX)
        {
            fontDir = "/Library/Fonts/";
            SSupportedFonts = System.IO.Directory.GetFiles
                (
                    fontDir,
                    "*.ttf",
                    System.IO.SearchOption.AllDirectories
                );
            SetupFontsFiles (SSupportedFonts);
            return;
        }

        var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform (System.Runtime.InteropServices.OSPlatform.Linux);
        if (isLinux)
        {
            SSupportedFonts = LinuxSystemFontResolver.Resolve();
            SetupFontsFiles (SSupportedFonts);
            return;
        }

        var isWindows =
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform (System.Runtime.InteropServices.OSPlatform
                .Windows);
        if (isWindows)
        {
            fontDir = System.Environment.ExpandEnvironmentVariables (@"%SystemRoot%\Fonts");
            var fontPaths = new List<string>();

            var systemFontPaths =
                System.IO.Directory.GetFiles (fontDir, "*.ttf", System.IO.SearchOption.AllDirectories);
            fontPaths.AddRange (systemFontPaths);

            var appdataFontDir =
                System.Environment.ExpandEnvironmentVariables (@"%LOCALAPPDATA%\Microsoft\Windows\Fonts");
            if (System.IO.Directory.Exists (appdataFontDir))
            {
                var appdataFontPaths =
                    System.IO.Directory.GetFiles (appdataFontDir, "*.ttf", System.IO.SearchOption.AllDirectories);
                fontPaths.AddRange (appdataFontPaths);
            }

            SSupportedFonts = fontPaths.ToArray();
            SetupFontsFiles (SSupportedFonts);
            return;
        }

        throw new System.NotImplementedException
            (
                "FontResolver not implemented for this platform (PdfSharpCore.Utils.FontResolver.cs)."
            );
    }


    private readonly struct FontFileInfo
    {
        private FontFileInfo (string path, FontDescription fontDescription)
        {
            this.Path = path;
            this.FontDescription = fontDescription;
        }

        public string Path { get; }

        public FontDescription FontDescription { get; }

        public string FamilyName => this.FontDescription.FontFamilyInvariantCulture;


        public XFontStyle GuessFontStyle()
        {
            switch (this.FontDescription.Style)
            {
                case FontStyle.Bold:
                    return XFontStyle.Bold;
                case FontStyle.Italic:
                    return XFontStyle.Italic;
                case FontStyle.BoldItalic:
                    return XFontStyle.BoldItalic;
                default:
                    return XFontStyle.Regular;
            }
        }

        public static FontFileInfo Load (string path)
        {
            var fontDescription = FontDescription.LoadDescription (path);
            return new FontFileInfo (path, fontDescription);
        }
    }


    public static void SetupFontsFiles (string[] sSupportedFonts)
    {
        var tempFontInfoList = new List<FontFileInfo>();
        foreach (var fontPathFile in sSupportedFonts)
        {
            try
            {
                var fontInfo = FontFileInfo.Load (fontPathFile);
                Debug.WriteLine (fontPathFile);
                tempFontInfoList.Add (fontInfo);
            }
            catch (System.Exception e)
            {
                System.Console.Error.WriteLine (e);
            }
        }

        // Deserialize all font families
        foreach (var familyGroup in tempFontInfoList.GroupBy (info => info.FamilyName))
            try
            {
                var familyName = familyGroup.Key;
                var family = DeserializeFontFamily (familyName, familyGroup);
                InstalledFonts.Add (familyName.ToLower(), family);
            }
            catch (System.Exception e)
            {
                System.Console.Error.WriteLine (e);
            }
    }


    [SuppressMessage ("ReSharper", "PossibleMultipleEnumeration")]
    private static FontFamilyModel DeserializeFontFamily (string fontFamilyName, IEnumerable<FontFileInfo> fontList)
    {
        var font = new FontFamilyModel { Name = fontFamilyName };

        // there is only one font
        if (fontList.Count() == 1)
            font.FontFiles.Add (XFontStyle.Regular, fontList.First().Path);
        else
        {
            foreach (var info in fontList)
            {
                var style = info.GuessFontStyle();
                if (!font.FontFiles.ContainsKey (style))
                    font.FontFiles.Add (style, info.Path);
            }
        }

        return font;
    }

    public virtual byte[] GetFont (string faceFileName)
    {
        using (var ms = new System.IO.MemoryStream())
        {
            var ttfPathFile = "";
            try
            {
                ttfPathFile = SSupportedFonts.ToList().First (x => x.ToLower().Contains (
                        System.IO.Path.GetFileName (faceFileName).ToLower())
                    );

                using (System.IO.Stream ttf = System.IO.File.OpenRead (ttfPathFile))
                {
                    ttf.CopyTo (ms);
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine (e);
                throw new System.Exception ("No Font File Found - " + faceFileName + " - " + ttfPathFile);
            }
        }
    }

    public bool NullIfFontNotFound { get; set; } = false;

    public virtual FontResolverInfo ResolveTypeface (string familyName, bool isBold, bool isItalic)
    {
        if (InstalledFonts.Count == 0)
            throw new System.IO.FileNotFoundException ("No Fonts installed on this device!");

        if (InstalledFonts.TryGetValue (familyName.ToLower(), out var family))
        {
            if (isBold && isItalic)
            {
                if (family.FontFiles.TryGetValue (XFontStyle.BoldItalic, out var boldItalicFile))
                    return new FontResolverInfo (System.IO.Path.GetFileName (boldItalicFile));
            }
            else if (isBold)
            {
                if (family.FontFiles.TryGetValue (XFontStyle.Bold, out var boldFile))
                    return new FontResolverInfo (System.IO.Path.GetFileName (boldFile));
            }
            else if (isItalic)
            {
                if (family.FontFiles.TryGetValue (XFontStyle.Italic, out var italicFile))
                    return new FontResolverInfo (System.IO.Path.GetFileName (italicFile));
            }

            if (family.FontFiles.TryGetValue (XFontStyle.Regular, out var regularFile))
                return new FontResolverInfo (System.IO.Path.GetFileName (regularFile));

            return new FontResolverInfo (System.IO.Path.GetFileName (family.FontFiles.First().Value));
        }

        if (NullIfFontNotFound)
            return null;

        var ttfFile = InstalledFonts.First().Value.FontFiles.First().Value;
        return new FontResolverInfo (System.IO.Path.GetFileName (ttfFile));
    }
}
