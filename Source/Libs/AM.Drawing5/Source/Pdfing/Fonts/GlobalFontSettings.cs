// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Internal;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts;

/// <summary>
/// Provides functionality to specify information about the handling
/// of fonts in the current application domain.
/// </summary>
public static class GlobalFontSettings
{
    /// <summary>
    /// The name of the default font.
    /// </summary>
    public const string DefaultFontName = "PlatformDefault";

    /// <summary>
    /// Gets or sets the global font resolver for the current application domain.
    /// This static property should be set only once and before any font operation was executed by PdfSharpCore.
    /// If this is not easily to obtain, e.g. because your code is running on a web server, you must provide the
    /// same instance of your font resolver in every subsequent setting of this property.
    /// In a web application set the font resolver in Global.asax.
    /// For .NetCore Apps, if a resolver is not set before the first get operation to this property,
    /// the default Font resolver implementation: <see cref="T:PdfSharpCore.Utils.PdfSharpCore.Utils"/> is set and returned
    /// </summary>
    public static IFontResolver FontResolver
    {
        get
        {
            if (_fontResolver == null)
            {
                FontResolver = new FontResolver();
            }

            return _fontResolver;
        }
        set
        {
            // Cannot remove font resolver.
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                Lock.EnterFontFactory();

                // Ignore multiple setting e.g. in a web application.
                if (ReferenceEquals (_fontResolver, value))
                {
                    return;
                }

                if (FontFactory.HasFontSources)
                {
                    throw new InvalidOperationException ("Must not change font resolver after is was once used.");
                }

                _fontResolver = value;
            }
            finally
            {
                Lock.ExitFontFactory();
            }
        }
    }

    static IFontResolver _fontResolver;

    /// <summary>
    /// Gets or sets the default font encoding used for XFont objects where encoding is not explicitly specified.
    /// If it is not set, the default value is PdfFontEncoding.Unicode.
    /// If you are sure your document contains only Windows-1252 characters (see https://en.wikipedia.org/wiki/Windows-1252)
    /// set default encoding to PdfFontEncodingj.Windows1252.
    /// Must be set only once per app domain.
    /// </summary>
    public static PdfFontEncoding DefaultFontEncoding
    {
        get
        {
            if (!_fontEncodingInitialized)
            {
                DefaultFontEncoding = PdfFontEncoding.Unicode;
            }

            return _fontEncoding;
        }
        set
        {
            try
            {
                Lock.EnterFontFactory();
                if (_fontEncodingInitialized)
                {
                    // Ignore multiple setting e.g. in a web application.
                    if (_fontEncoding == value)
                    {
                        return;
                    }

                    throw new InvalidOperationException ("Must not change DefaultFontEncoding after is was set once.");
                }

                _fontEncoding = value;
                _fontEncodingInitialized = true;
            }
            finally
            {
                Lock.ExitFontFactory();
            }
        }
    }

    static PdfFontEncoding _fontEncoding;
    static bool _fontEncodingInitialized;
}
