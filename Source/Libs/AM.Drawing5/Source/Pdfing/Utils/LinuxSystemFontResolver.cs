// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* LinuxSystemFontResolver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace PdfSharpCore.Utils;

/// <summary>
///
/// </summary>
public static class LinuxSystemFontResolver
{
    const string libfontconfig = "libfontconfig.so.1";

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    private static extern IntPtr FcInitLoadConfigAndFonts();

    /// <summary>
    ///
    /// </summary>
    static readonly Lazy<IntPtr> fcConfig = new Lazy<IntPtr> (FcInitLoadConfigAndFonts);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    public static extern FcPatternHandle FcPatternCreate();

    /// <summary>
    ///
    /// </summary>
    /// <param name="p"></param>
    /// <param name="obj"></param>
    /// <param name="n"></param>
    /// <param name="s"></param>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    public static extern int FcPatternGetString (IntPtr p, [MarshalAs (UnmanagedType.LPStr)] string obj, int n,
        ref IntPtr s);

    /// <summary>
    ///
    /// </summary>
    /// <param name="pattern"></param>
    [DllImport (libfontconfig)]
    public static extern void FcPatternDestroy (IntPtr pattern);

    /// <summary>
    ///
    /// </summary>
    public class FcPatternHandle
        : SafeHandle
    {
        /// <summary>
        ///
        /// </summary>
        FcPatternHandle()
            : base (IntPtr.Zero, true)
        {
            // пустое тело конструктора
        }

        /// <inheritdoc cref="SafeHandle.IsInvalid"/>
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
        protected override bool ReleaseHandle()
        {
            FcPatternDestroy (handle);
            return true;
        }
    }


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    public static extern FcObjectSetHandle FcObjectSetCreate();

    /// <summary>
    ///
    /// </summary>
    /// <param name="os"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    public static extern int FcObjectSetAdd (FcObjectSetHandle os, [MarshalAs (UnmanagedType.LPStr)] string obj);

    /// <summary>
    ///
    /// </summary>
    /// <param name="os"></param>
    [DllImport (libfontconfig)]
    public static extern void FcObjectSetDestroy (IntPtr os);

    /// <summary>
    ///
    /// </summary>
    public class FcObjectSetHandle
        : SafeHandle
    {
        /// <summary>
        ///
        /// </summary>
        FcObjectSetHandle()
            : base (IntPtr.Zero, true)
        {
            // пустое тело конструктора
        }

        /// <inheritdoc cref="SafeHandle.IsInvalid"/>
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
        protected override bool ReleaseHandle()
        {
            FcObjectSetDestroy (handle);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static FcObjectSetHandle Create (params string[] objs)
        {
            var os = FcObjectSetCreate();
            foreach (var obj in objs)
            {
                FcObjectSetAdd (os, obj);
            }

            FcObjectSetAdd (os, "");
            return os;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="config"></param>
    /// <param name="pattern"></param>
    /// <param name="os"></param>
    /// <returns></returns>
    [DllImport (libfontconfig)]
    public static extern FcFontSetHandle FcFontList (IntPtr config, FcPatternHandle pattern, FcObjectSetHandle os);

    /// <summary>
    ///
    /// </summary>
    /// <param name="fs"></param>
    [DllImport (libfontconfig)]
    public static extern void FcFontSetDestroy (IntPtr fs);

    /// <summary>
    ///
    /// </summary>
    public struct FcFontSet
    {
        /// <summary>
        ///
        /// </summary>
        public int nfont;

        /// <summary>
        ///
        /// </summary>
        public int sfont;

        /// <summary>
        ///
        /// </summary>
        public IntPtr fonts;
    }

    /// <summary>
    ///
    /// </summary>
    public sealed class FcFontSetHandle
        : SafeHandle
    {
        /// <summary>
        ///
        /// </summary>
        FcFontSetHandle()
            : base (IntPtr.Zero, true)
        {
            // пустое тело конструктора
        }

        /// <inheritdoc cref="SafeHandle.IsInvalid"/>
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
        protected override bool ReleaseHandle()
        {
            FcFontSetDestroy (handle);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public FcFontSet Read()
        {
            return Marshal.PtrToStructure<FcFontSet> (handle);
        }
    }


    static string? GetString (IntPtr handle, string obj)
    {
        var ptr = IntPtr.Zero;
        var result = FcPatternGetString (handle, obj, 0, ref ptr);
        if (result == 0)
            return Marshal.PtrToStringAnsi (ptr);
        else
            return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    static IEnumerable<string> ResolveFontConfig()
    {
        var config = fcConfig.Value;
        using (var pattern = FcPatternCreate())
        using (var os = FcObjectSetHandle.Create ("family", "style", "file"))
        using (var fs = FcFontList (config, pattern, os))
        {
            var fset = fs.Read();
            for (var index = 0; index < fset.nfont; index++)
            {
                var font = Marshal.ReadIntPtr (fset.fonts, index * Marshal.SizeOf<IntPtr>());
                var family = GetString (font, "family");
                var style = GetString (font, "style");
                var file = GetString (font, "file");

                if (family is null || style is null || file is null)
                    continue;

                yield return file;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string[] Resolve()
    {
        try
        {
            return ResolveFontConfig().Where (x => x.EndsWith (".ttf", StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine (ex.ToString());
            return ResolveFallback().Where (x => x.EndsWith (".ttf", StringComparison.OrdinalIgnoreCase)).ToArray();
        }
    }


    static IEnumerable<string> ResolveFallback()
    {
        var fontList = new List<string>();

        void AddFontsToFontList (string path)
        {
            if (!Directory.Exists (path))
                return;

            foreach (var subDir in Directory.EnumerateDirectories (path, "*", SearchOption.AllDirectories))
            {
                fontList.AddRange (Directory.EnumerateFiles (subDir, "*", SearchOption.AllDirectories));
            }
        }

        var hs = new HashSet<string> (StringComparer.OrdinalIgnoreCase);
        foreach (var path in SearchPaths())
        {
            if (hs.Contains (path))
                continue;
            hs.Add (path);
            AddFontsToFontList (path);
        }

        return fontList.ToArray();
    }

    static IEnumerable<string> SearchPaths()
    {
        var dirs = new List<string>();
        try
        {
            var confRegex = new Regex ("<dir>(?<dir>.*)</dir>", RegexOptions.Compiled);
            using (var reader = new StreamReader (File.OpenRead ("/etc/fonts/fonts.conf")))
            {
                while (reader.ReadLine() is { } line)
                {
                    var match = confRegex.Match (line);
                    if (!match.Success)
                        continue;

                    var path = match.Groups["dir"].Value.Trim();
                    if (path.StartsWith ("~"))
                    {
                        path = Environment.GetEnvironmentVariable ("HOME") + path.Substring (1);
                    }

                    dirs.Add (path);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine (ex.Message);
            Console.Error.WriteLine (ex.StackTrace);
        }

        dirs.Add ("/usr/share/fonts");
        dirs.Add ("/usr/local/share/fonts");
        dirs.Add (Environment.GetEnvironmentVariable ("HOME") + "/.fonts");
        return dirs;
    }
}
