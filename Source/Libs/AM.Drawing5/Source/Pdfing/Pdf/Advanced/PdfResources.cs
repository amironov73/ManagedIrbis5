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

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a PDF resource object.
/// </summary>
public sealed class PdfResources
    : PdfDictionary
{
    // Resource management works roughly like this:
    // When the user creates an XFont and uses it in the XGraphics of a PdfPage, then at the first time
    // a PdfFont is created and cached in the document global font table. If the user creates a new
    // XFont object for an exisisting PdfFont, the PdfFont object is reused. When the PdfFont is added
    // to the resources of a PdfPage for the first time, it is added to the page local PdfResourceMap for
    // fonts and automatically associated with a local resource name.

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfResources"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    public PdfResources (PdfDocument document)
        : base (document)
    {
        Elements[Keys.ProcSet] = new PdfLiteral ("[/PDF/Text/ImageB/ImageC/ImageI]");
    }

    internal PdfResources (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Adds the specified font to this resource dictionary and returns its local resource name.
    /// </summary>
    public string AddFont (PdfFont font)
    {
        if (!_resources.TryGetValue (font, out var name))
        {
            name = NextFontName;
            _resources[font] = name;
            if (font.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (font);
            }

            Fonts.Elements[name] = font.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified image to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddImage (PdfImage image)
    {
        if (!_resources.TryGetValue (image, out var name))
        {
            name = NextImageName;
            _resources[image] = name;
            if (image.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (image);
            }

            XObjects.Elements[name] = image.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified form object to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddForm (PdfFormXObject form)
    {
        if (!_resources.TryGetValue (form, out var name))
        {
            name = NextFormName;
            _resources[form] = name;
            if (form.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (form);
            }

            XObjects.Elements[name] = form.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified graphics state to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddExtGState (PdfExtGState extGState)
    {
        if (!_resources.TryGetValue (extGState, out var name))
        {
            name = NextExtGStateName;
            _resources[extGState] = name;
            if (extGState.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (extGState);
            }

            ExtGStates.Elements[name] = extGState.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified pattern to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddPattern (PdfShadingPattern pattern)
    {
        if (!_resources.TryGetValue (pattern, out var name))
        {
            name = NextPatternName;
            _resources[pattern] = name;
            if (pattern.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (pattern);
            }

            Patterns.Elements[name] = pattern.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified pattern to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddPattern (PdfTilingPattern pattern)
    {
        if (!_resources.TryGetValue (pattern, out var name))
        {
            name = NextPatternName;
            _resources[pattern] = name;
            if (pattern.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (pattern);
            }

            Patterns.Elements[name] = pattern.Reference;
        }

        return name;
    }

    /// <summary>
    /// Adds the specified shading to this resource dictionary
    /// and returns its local resource name.
    /// </summary>
    public string AddShading (PdfShading shading)
    {
        if (!_resources.TryGetValue (shading, out var name))
        {
            name = NextShadingName;
            _resources[shading] = name;
            if (shading.Reference == null)
            {
                Owner.ThrowIfNull()._irefTable.Add (shading);
            }

            Shadings.Elements[name] = shading.Reference;
        }

        return name;
    }

    /// <summary>
    /// Gets the fonts map.
    /// </summary>
    internal PdfResourceMap Fonts =>
        _fonts ??= (PdfResourceMap) Elements.GetValue (Keys.Font, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _fonts;

    /// <summary>
    /// Gets the external objects map.
    /// </summary>
    internal PdfResourceMap XObjects =>
        _xObjects ??= (PdfResourceMap) Elements.GetValue (Keys.XObject, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _xObjects;

    // TODO: make own class
    internal PdfResourceMap ExtGStates =>
        _extGStates ??= (PdfResourceMap) Elements.GetValue (Keys.ExtGState, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _extGStates;

    // TODO: make own class
    internal PdfResourceMap ColorSpaces =>
        _colorSpaces ??= (PdfResourceMap) Elements.GetValue (Keys.ColorSpace, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _colorSpaces;

    // TODO: make own class
    internal PdfResourceMap Patterns =>
        _patterns ??= (PdfResourceMap) Elements.GetValue (Keys.Pattern, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _patterns;

    // TODO: make own class
    internal PdfResourceMap Shadings =>
        _shadings ??= (PdfResourceMap) Elements.GetValue (Keys.Shading, VCF.Create)
            .ThrowIfNull();

    PdfResourceMap? _shadings;

    // TODO: make own class
    internal PdfResourceMap Properties =>
        _properties ??= (PdfResourceMap) Elements.GetValue (Keys.Properties, VCF.Create)
            .ThrowIfNull();

    private PdfResourceMap? _properties;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextFontName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/F{_fontNumber++}"))
            {
                // пустое тело цикла
            }

            return name;
        }
    }

    int _fontNumber;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextImageName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/I{_imageNumber++}"))
            {
                // пустое тело цикла
            }

            return name;
        }
    }

    int _imageNumber;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextFormName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/Fm{_formNumber++}"))
            {
                // пустое тело цикла
            }

            return name;
        }
    }

    int _formNumber;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextExtGStateName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/GS{_extGStateNumber++}"))
            {
                // пустое тело цикла
            }

            return name;
        }
    }

    int _extGStateNumber;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextPatternName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/Pa{_patternNumber++}"))
            {
                // пустое тело цикла
            }
            return name;
        }
    }

    int _patternNumber;

    /// <summary>
    /// Gets a new local name for this resource.
    /// </summary>
    string NextShadingName
    {
        get
        {
            string name;
            while (ExistsResourceNames (name = $"/Sh{_shadingNumber++}"))
            {
                // пустое тело цикла
            }
            return name;
        }
    }

    int _shadingNumber;

    /// <summary>
    /// Check whether a resource name is already used in the context of this resource dictionary.
    /// PDF4NET uses GUIDs as resource names, but I think this weapon is to heavy.
    /// </summary>
    internal bool ExistsResourceNames (string name)
    {
        // TODO: more precise: is this page imported and is PageOptions != Replace
        // BUG:
        //if (!Owner.IsImported)
        //  return false;

        // Collect all resouce names of all imported resources.
        if (_importedResourceNames == null)
        {
            _importedResourceNames = new Dictionary<string, object>();

            if (Elements[Keys.Font] != null)
            {
                Fonts.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.XObject] != null)
            {
                XObjects.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.ExtGState] != null)
            {
                ExtGStates.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.ColorSpace] != null)
            {
                ColorSpaces.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.Pattern] != null)
            {
                Patterns.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.Shading] != null)
            {
                Shadings.CollectResourceNames (_importedResourceNames);
            }

            if (Elements[Keys.Properties] != null)
            {
                Properties.CollectResourceNames (_importedResourceNames);
            }
        }

        return _importedResourceNames.ContainsKey (name);

        // This is superfluous because PDFsharp resource names cannot be double.
        // importedResourceNames.Add(name, null);
    }

    /// <summary>
    /// All the names of imported resources.
    /// </summary>
    private Dictionary<string, object>? _importedResourceNames;

    /// <summary>
    /// Maps all PDFsharp resources to their local resource names.
    /// </summary>
    readonly Dictionary<PdfObject, string> _resources = new ();

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    public sealed class Keys
        : KeysBase
    {
        /// <summary>
        /// (Optional) A dictionary that maps resource names to graphics state
        /// parameter dictionaries.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string ExtGState = "/ExtGState";

        /// <summary>
        /// (Optional) A dictionary that maps each resource name to either the name of a
        /// device-dependent color space or an array describing a color space.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string ColorSpace = "/ColorSpace";

        /// <summary>
        /// (Optional) A dictionary that maps each resource name to either the name of a
        /// device-dependent color space or an array describing a color space.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string Pattern = "/Pattern";

        /// <summary>
        /// (Optional; PDF 1.3) A dictionary that maps resource names to shading dictionaries.
        /// </summary>
        [KeyInfo ("1.3", KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string Shading = "/Shading";

        /// <summary>
        /// (Optional) A dictionary that maps resource names to external objects.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string XObject = "/XObject";

        /// <summary>
        /// (Optional) A dictionary that maps resource names to font dictionaries.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string Font = "/Font";

        /// <summary>
        /// (Optional) An array of predefined procedure set names.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string ProcSet = "/ProcSet";

        /// <summary>
        /// (Optional; PDF 1.2) A dictionary that maps resource names to property list
        /// dictionaries for marked content.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Optional, typeof (PdfResourceMap))]
        public const string Properties = "/Properties";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        internal static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
