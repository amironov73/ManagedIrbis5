// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* HtmlDocument.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using AM;

#endregion

#pragma warning disable CS0618 // obsolete types

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents a complete HTML document.
/// </summary>
public partial class HtmlDocument
{
    #region Manager

    internal static bool _disableBehaviorTagP = true;

    /// <summary>True to disable, false to enable the behavior tag p.</summary>
    public static bool DisableBehaviorTagP
    {
        get => _disableBehaviorTagP;
        set
        {
            if (value)
            {
                if (HtmlNode.ElementsFlags.ContainsKey ("p"))
                {
                    HtmlNode.ElementsFlags.Remove ("p");
                }
            }
            else
            {
                if (!HtmlNode.ElementsFlags.ContainsKey ("p"))
                {
                    HtmlNode.ElementsFlags.Add ("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
                }
            }

            _disableBehaviorTagP = value;
        }
    }

    /// <summary>Default builder to use in the HtmlDocument constructor</summary>
    public static Action<HtmlDocument>? DefaultBuilder { get; set; }

    /// <summary>Action to execute before the Parse is executed</summary>
    public Action<HtmlDocument> ParseExecuting { get; set; }

    #endregion

    #region Fields

    private int _c;
    private Crc32 _crc32;
    private HtmlAttribute _currentAttribute;
    private HtmlNode _currentNode;
    private Encoding? _declaredEncoding;
    private HtmlNode _documentnode;
    private bool _fullcomment;
    private int _index;
    internal Dictionary<string, HtmlNode> Lastnodes = new ();
    private HtmlNode _lastParentNode;
    private int _line;
    private int _lineposition, _maxlineposition;
    internal Dictionary<string, HtmlNode> Nodesid;
    private ParseState _oldstate;
    private bool _onlyDetectEncoding;
    internal Dictionary<int, HtmlNode>? Openednodes;
    private List<HtmlParseError> _parseerrors = new ();
    private string _remainder;
    private int _remainderOffset;
    private ParseState _state;
    private Encoding _streamEncoding;
    private bool _useHtmlEncodingForStream;

    /// <summary>The HtmlDocument Text. Careful if you modify it.</summary>
    public string Text;

    /// <summary>True to stay backward compatible with previous version of HAP. This option does not guarantee 100% compatibility.</summary>
    public bool BackwardCompatibility = true;

    /// <summary>
    /// Adds Debugging attributes to node. Default is false.
    /// </summary>
    public bool OptionAddDebuggingAttributes;

    /// <summary>
    /// Defines if closing for non closed nodes must be done at the end or directly in the document.
    /// Setting this to true can actually change how browsers render the page. Default is false.
    /// </summary>
    public bool OptionAutoCloseOnEnd; // close errors at the end

    /// <summary>
    /// Defines if non closed nodes will be checked at the end of parsing. Default is true.
    /// </summary>
    public bool OptionCheckSyntax = true;

    /// <summary>
    /// Defines if a checksum must be computed for the document while parsing. Default is false.
    /// </summary>
    public bool OptionComputeChecksum;

    /// <summary>
    /// Defines if SelectNodes method will return null or empty collection when no node matched the XPath expression.
    /// Setting this to true will return empty collection and false will return null. Default is false.
    /// </summary>
    public bool OptionEmptyCollection = false;

    /// <summary>True to disable, false to enable the server side code.</summary>
    public bool DisableServerSideCode = false;


    /// <summary>
    /// Defines the default stream encoding to use. Default is System.Text.Encoding.Default.
    /// </summary>
    public Encoding OptionDefaultStreamEncoding;

    /// <summary>
    /// Force to take the original comment instead of creating it
    /// </summary>
    public bool OptionXmlForceOriginalComment;

    /// <summary>
    /// Defines if source text must be extracted while parsing errors.
    /// If the document has a lot of errors, or cascading errors, parsing performance can be dramatically affected if set to true.
    /// Default is false.
    /// </summary>
    public bool OptionExtractErrorSourceText;

    // turning this on can dramatically slow performance if a lot of errors are detected

    /// <summary>
    /// Defines the maximum length of source text or parse errors. Default is 100.
    /// </summary>
    public int OptionExtractErrorSourceTextMaxLength = 100;

    /// <summary>
    /// Defines if LI, TR, TH, TD tags must be partially fixed when nesting errors are detected. Default is false.
    /// </summary>
    public bool OptionFixNestedTags; // fix li, tr, th, td tags

    /// <summary>
    /// Defines if output must conform to XML, instead of HTML. Default is false.
    /// </summary>
    public bool OptionOutputAsXml;

    /// <summary>
    /// If used together with <see cref="OptionOutputAsXml"/> and enabled, Xml namespaces in element names are preserved. Default is false.
    /// </summary>
    public bool OptionPreserveXmlNamespaces;

    /// <summary>
    /// Defines if attribute value output must be optimized (not bound with double quotes if it is possible). Default is false.
    /// </summary>
    public bool OptionOutputOptimizeAttributeValues;

    /// <summary>Defines the global attribute value quote. When specified, it will always win.</summary>
    public AttributeValueQuote? GlobalAttributeValueQuote;

    /// <summary>
    /// Defines if name must be output with it's original case. Useful for asp.net tags and attributes. Default is false.
    /// </summary>
    public bool OptionOutputOriginalCase;

    /// <summary>
    /// Defines if name must be output in uppercase. Default is false.
    /// </summary>
    public bool OptionOutputUpperCase;

    /// <summary>
    /// Defines if declared encoding must be read from the document.
    /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node.
    /// Default is true.
    /// </summary>
    public bool OptionReadEncoding = true;

    /// <summary>
    /// Defines the name of a node that will throw the StopperNodeException when found as an end node. Default is null.
    /// </summary>
    public string OptionStopperNodeName;

    /// <summary>
    /// Defines if attributes should use original names by default, rather than lower case. Default is false.
    /// </summary>
    public bool OptionDefaultUseOriginalName;

    /// <summary>
    /// Defines if the 'id' attribute must be specifically used. Default is true.
    /// </summary>
    public bool OptionUseIdAttribute = true;

    /// <summary>
    /// Defines if empty nodes must be written as closed during output. Default is false.
    /// </summary>
    public bool OptionWriteEmptyNodes;

    /// <summary>
    /// The max number of nested child nodes.
    /// Added to prevent stackoverflow problem when a page has tens of thousands of opening html tags with no closing tags
    /// </summary>
    public int OptionMaxNestedChildNodes = 0;

    #endregion

    #region Static Members

    internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";

    internal static readonly string HtmlExceptionUseIdAttributeFalse =
        "You need to set UseIdAttribute property to true to enable this feature";

    internal static readonly string HtmlExceptionClassDoesNotExist = "Class name doesn't exist";

    internal static readonly string HtmlExceptionClassExists = "Class name already exists";

    internal static readonly Dictionary<string, string[]> HtmlResetters = new ()
    {
        { "li", new[] { "ul", "ol" } },
        { "tr", new[] { "table" } },
        { "th", new[] { "tr", "table" } },
        { "td", new[] { "tr", "table" } },
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an instance of an HTML document.
    /// </summary>
    public HtmlDocument()
    {
        _crc32 = null!;
        _currentAttribute = null!;
        _currentNode = null!;
        _lastParentNode = null!;
        _remainder = null!;
        _streamEncoding = null!;
        ParseExecuting = null!;
        Nodesid = null!;
        Text = null!;
        OptionStopperNodeName = null!;

        if (DefaultBuilder != null!)
        {
            DefaultBuilder (this);
        }

        _documentnode = CreateNode (HtmlNodeType.Document, 0);
        OptionDefaultStreamEncoding = Encoding.UTF8;
    }

    #endregion

    #region Properties

    /// <summary>Gets the parsed text.</summary>
    /// <value>The parsed text.</value>
    public string ParsedText => Text;

    /// <summary>
    /// Defines the max level we would go deep into the html document. If this depth level is exceeded, and exception is
    /// thrown.
    /// </summary>
    public static int MaxDepthLevel { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets the document CRC32 checksum if OptionComputeChecksum was set to true before parsing, 0 otherwise.
    /// </summary>
    public int CheckSum => _crc32 == null! ? 0 : (int)_crc32.CheckSum;

    /// <summary>
    /// Gets the document's declared encoding.
    /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node (pre-HTML5) or the meta charset="XXXXX" html node (HTML5).
    /// </summary>
    public Encoding DeclaredEncoding => _declaredEncoding!;

    /// <summary>
    /// Gets the root node of the document.
    /// </summary>
    public HtmlNode DocumentNode => _documentnode;

    /// <summary>
    /// Gets the document's output encoding.
    /// </summary>
    public Encoding Encoding => GetOutEncoding();

    /// <summary>
    /// Gets a list of parse errors found in the document.
    /// </summary>
    public IEnumerable<HtmlParseError> ParseErrors => _parseerrors;

    /// <summary>
    /// Gets the remaining text.
    /// Will always be null if OptionStopperNodeName is null.
    /// </summary>
    public string Remainder => _remainder;

    /// <summary>
    /// Gets the offset of Remainder in the original Html text.
    /// If OptionStopperNodeName is null, this will return the length of the original Html text.
    /// </summary>
    public int RemainderOffset => _remainderOffset;

    /// <summary>
    /// Gets the document's stream encoding.
    /// </summary>
    public Encoding StreamEncoding => _streamEncoding;

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets a valid XML name.
    /// </summary>
    /// <param name="name">Any text.</param>
    /// <returns>A string that is a valid XML name.</returns>
    public static string GetXmlName (string name)
    {
        return GetXmlName (name, false, false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tagName"></param>
    public void UseAttributeOriginalName (string tagName)
    {
        foreach (var nod in this.DocumentNode.SelectNodes ("//" + tagName)!)
        {
            foreach (var attribut in nod.Attributes)
            {
                attribut.UseOriginalName = true;
            }
        }
    }

    public static string GetXmlName (string name, bool isAttribute, bool preserveXmlNamespaces)
    {
        var xmlname = string.Empty;
        var nameisok = true;
        for (var i = 0; i < name.Length; i++)
        {
            // names are lcase
            // note: we are very limited here, too much?
            if (name[i] >= 'a' && name[i] <= 'z' ||
                name[i] >= 'A' && name[i] <= 'Z' ||
                name[i] >= '0' && name[i] <= '9' ||
                (isAttribute || preserveXmlNamespaces) && name[i] == ':' ||

                //                    (name[i]==':') || (name[i]=='_') || (name[i]=='-') || (name[i]=='.')) // these are bads in fact
                name[i] == '_' || name[i] == '-' || name[i] == '.')
            {
                xmlname += name[i];
            }
            else
            {
                nameisok = false;
                var bytes = Encoding.UTF8.GetBytes (new[] { name[i] });
                for (var j = 0; j < bytes.Length; j++)
                {
                    xmlname += bytes[j].ToString ("x2");
                }

                xmlname += "_";
            }
        }

        if (nameisok)
        {
            return xmlname;
        }

        return "_" + xmlname;
    }

    /// <summary>
    /// Applies HTML encoding to a specified string.
    /// </summary>
    /// <param name="html">The input string to encode. May not be null.</param>
    /// <returns>The encoded string.</returns>
    public static string HtmlEncode (string html)
    {
        return HtmlEncodeWithCompatibility (html, true);
    }

    internal static string HtmlEncodeWithCompatibility (string html, bool backwardCompatibility = true)
    {
        Sure.NotNull (html);

        // replace & by &amp; but only once!

        var rx = backwardCompatibility
            ? new Regex ("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase)
            : new Regex ("&(?!(amp;)|(lt;)|(gt;)|(quot;)|(nbsp;)|(reg;))", RegexOptions.IgnoreCase);
        return rx.Replace (html, "&amp;").Replace ("<", "&lt;").Replace (">", "&gt;").Replace ("\"", "&quot;");
    }

    /// <summary>
    /// Determines if the specified character is considered as a whitespace character.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>true if if the specified character is considered as a whitespace character.</returns>
    public static bool IsWhiteSpace (int c)
    {
        if (c is 10 or 13 or 32 or 9)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Creates an HTML attribute with the specified name.
    /// </summary>
    /// <param name="name">The name of the attribute. May not be null.</param>
    /// <returns>The new HTML attribute.</returns>
    public HtmlAttribute CreateAttribute (string name)
    {
        Sure.NotNull (name);

        var att = CreateAttribute();
        att.Name = name;
        return att;
    }

    /// <summary>
    /// Creates an HTML attribute with the specified name.
    /// </summary>
    /// <param name="name">The name of the attribute. May not be null.</param>
    /// <param name="value">The value of the attribute.</param>
    /// <returns>The new HTML attribute.</returns>
    public HtmlAttribute CreateAttribute (string name, string value)
    {
        Sure.NotNull (name);

        var att = CreateAttribute (name);
        att.Value = value;
        return att;
    }

    /// <summary>
    /// Creates an HTML comment node.
    /// </summary>
    /// <returns>The new HTML comment node.</returns>
    public HtmlCommentNode CreateComment()
    {
        return (HtmlCommentNode)CreateNode (HtmlNodeType.Comment);
    }

    /// <summary>
    /// Creates an HTML comment node with the specified comment text.
    /// </summary>
    /// <param name="comment">The comment text. May not be null.</param>
    /// <returns>The new HTML comment node.</returns>
    public HtmlCommentNode CreateComment (string comment)
    {
        Sure.NotNull (comment);

        var c = CreateComment();
        c.Comment = comment;
        return c;
    }

    /// <summary>
    /// Creates an HTML element node with the specified name.
    /// </summary>
    /// <param name="name">The qualified name of the element. May not be null.</param>
    /// <returns>The new HTML node.</returns>
    public HtmlNode CreateElement (string name)
    {
        Sure.NotNull (name);

        var node = CreateNode (HtmlNodeType.Element);
        node.Name = name;
        return node;
    }

    /// <summary>
    /// Creates an HTML text node.
    /// </summary>
    /// <returns>The new HTML text node.</returns>
    public HtmlTextNode CreateTextNode()
    {
        return (HtmlTextNode)CreateNode (HtmlNodeType.Text);
    }

    /// <summary>
    /// Creates an HTML text node with the specified text.
    /// </summary>
    /// <param name="text">The text of the node. May not be null.</param>
    /// <returns>The new HTML text node.</returns>
    public HtmlTextNode CreateTextNode (string text)
    {
        Sure.NotNull (text);

        var t = CreateTextNode();
        t.Text = text;
        return t;
    }

    /// <summary>
    /// Detects the encoding of an HTML stream.
    /// </summary>
    /// <param name="stream">The input stream. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding (Stream stream)
    {
        return DetectEncoding (stream, false);
    }

    /// <summary>
    /// Detects the encoding of an HTML stream.
    /// </summary>
    /// <param name="stream">The input stream. May not be null.</param>
    /// <param name="checkHtml">The html is checked.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding (Stream stream, bool checkHtml)
    {
        Sure.NotNull (stream);

        _useHtmlEncodingForStream = checkHtml;

        return DetectEncoding (new StreamReader (stream));
    }


    /// <summary>
    /// Detects the encoding of an HTML text provided on a TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to feed the HTML. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding (TextReader reader)
    {
        Sure.NotNull (reader);

        _onlyDetectEncoding = true;
        if (OptionCheckSyntax)
        {
            Openednodes = new Dictionary<int, HtmlNode>();
        }
        else
        {
            Openednodes = null;
        }

        if (OptionUseIdAttribute)
        {
            Nodesid = new Dictionary<string, HtmlNode> (StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            Nodesid = null!;
        }

        var sr = reader as StreamReader;
        if (sr != null && !_useHtmlEncodingForStream)
        {
            Text = sr.ReadToEnd();
            _streamEncoding = sr.CurrentEncoding;
            return _streamEncoding;
        }

        _streamEncoding = null!;
        _declaredEncoding = null;

        Text = reader.ReadToEnd();
        _documentnode = CreateNode (HtmlNodeType.Document, 0);

        // this is almost a hack, but it allows us not to muck with the original parsing code
        try
        {
            Parse();
        }
        catch (EncodingFoundException ex)
        {
            return ex.Encoding;
        }

        return _streamEncoding;
    }


    /// <summary>
    /// Detects the encoding of an HTML text.
    /// </summary>
    /// <param name="html">The input html text. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncodingHtml (string html)
    {
        Sure.NotNull (html);

        using (var sr = new StringReader (html))
        {
            var encoding = DetectEncoding (sr);
            return encoding;
        }
    }

    /// <summary>
    /// Gets the HTML node with the specified 'id' attribute value.
    /// </summary>
    /// <param name="id">The attribute id to match. May not be null.</param>
    /// <returns>The HTML node with the matching id or null if not found.</returns>
    public HtmlNode? GetElementbyId (string id)
    {
        Sure.NotNull (id);

        if (Nodesid == null)
        {
            throw new Exception (HtmlExceptionUseIdAttributeFalse);
        }

        return Nodesid.ContainsKey (id) ? Nodesid[id] : null;
    }

    /// <summary>
    /// Loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    public void Load (Stream stream)
    {
        Load (new StreamReader (stream, OptionDefaultStreamEncoding));
    }

    /// <summary>
    /// Loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public void Load (Stream stream, bool detectEncodingFromByteOrderMarks)
    {
        Load (new StreamReader (stream, detectEncodingFromByteOrderMarks));
    }

    /// <summary>
    /// Loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public void Load (Stream stream, Encoding encoding)
    {
        Load (new StreamReader (stream, encoding));
    }

    /// <summary>
    /// Loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public void Load (Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
    {
        Load (new StreamReader (stream, encoding, detectEncodingFromByteOrderMarks));
    }

    /// <summary>
    /// Loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    /// <param name="buffersize">The minimum buffer size.</param>
    public void Load (Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
    {
        Load (new StreamReader (stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
    }


    /// <summary>
    /// Loads the HTML document from the specified TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to feed the HTML data into the document. May not be null.</param>
    public void Load (TextReader reader)
    {
        // all Load methods pass down to this one
        Sure.NotNull (reader);

        _onlyDetectEncoding = false;

        if (OptionCheckSyntax)
        {
            Openednodes = new Dictionary<int, HtmlNode>();
        }
        else
        {
            Openednodes = null;
        }

        if (OptionUseIdAttribute)
        {
            Nodesid = new Dictionary<string, HtmlNode> (StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            Nodesid = null!;
        }

        if (reader is StreamReader sr)
        {
            try
            {
                // trigger bom read if needed
                sr.Peek();
            }

            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)

                // ReSharper restore EmptyGeneralCatchClause
            {
                // void on purpose
            }

            _streamEncoding = sr.CurrentEncoding;
        }
        else
        {
            _streamEncoding = null!;
        }

        _declaredEncoding = null;

        Text = reader.ReadToEnd();
        _documentnode = CreateNode (HtmlNodeType.Document, 0);
        Parse();

        if (!OptionCheckSyntax || Openednodes == null)
        {
            return;
        }

        foreach (var node in Openednodes.Values)
        {
            if (!node._startTag) // already reported
            {
                continue;
            }

            string html;
            if (OptionExtractErrorSourceText)
            {
                html = node.OuterHtml!;
                if (html.Length > OptionExtractErrorSourceTextMaxLength)
                {
                    html = html.Substring (0, OptionExtractErrorSourceTextMaxLength);
                }
            }
            else
            {
                html = string.Empty;
            }

            AddError (
                HtmlParseErrorCode.TagNotClosed,
                node.Line, node.LinePosition,
                node._streamPosition, html,
                "End tag </" + node.Name + "> was not found");
        }

        // we don't need this anymore
        Openednodes.Clear();
    }

    /// <summary>
    /// Loads the HTML document from the specified string.
    /// </summary>
    /// <param name="html">String containing the HTML document to load. May not be null.</param>
    public void LoadHtml (string html)
    {
        Sure.NotNull (html);

        using (var sr = new StringReader (html))
        {
            Load (sr);
        }
    }

    /// <summary>
    /// Saves the HTML document to the specified stream.
    /// </summary>
    /// <param name="outStream">The stream to which you want to save.</param>
    public void Save (Stream outStream)
    {
        var sw = new StreamWriter (outStream, GetOutEncoding());
        Save (sw);
    }

    /// <summary>
    /// Saves the HTML document to the specified stream.
    /// </summary>
    /// <param name="outStream">The stream to which you want to save. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    public void Save (Stream outStream, Encoding encoding)
    {
        Sure.NotNull (outStream);
        Sure.NotNull (encoding);

        var sw = new StreamWriter (outStream, encoding);
        Save (sw);
    }


    /// <summary>
    /// Saves the HTML document to the specified StreamWriter.
    /// </summary>
    /// <param name="writer">The StreamWriter to which you want to save.</param>
    public void Save (StreamWriter writer)
    {
        Save ((TextWriter)writer);
    }

    /// <summary>
    /// Saves the HTML document to the specified TextWriter.
    /// </summary>
    /// <param name="writer">The TextWriter to which you want to save. May not be null.</param>
    public void Save (TextWriter writer)
    {
        Sure.NotNull (writer);

        DocumentNode.WriteTo (writer);
        writer.Flush();
    }

    /// <summary>
    /// Saves the HTML document to the specified XmlWriter.
    /// </summary>
    /// <param name="writer">The XmlWriter to which you want to save.</param>
    public void Save (XmlWriter writer)
    {
        DocumentNode.WriteTo (writer);
        writer.Flush();
    }

    #endregion

    #region Internal Methods

    internal HtmlAttribute CreateAttribute()
    {
        return new HtmlAttribute (this);
    }

    internal HtmlNode CreateNode (HtmlNodeType type)
    {
        return CreateNode (type, -1);
    }

    internal HtmlNode CreateNode (HtmlNodeType type, int index)
    {
        switch (type)
        {
            case HtmlNodeType.Comment:
                return new HtmlCommentNode (this, index);

            case HtmlNodeType.Text:
                return new HtmlTextNode (this, index);

            default:
                return new HtmlNode (type, this, index);
        }
    }

    internal Encoding GetOutEncoding()
    {
        // when unspecified, use the stream encoding first
        return _declaredEncoding ?? (_streamEncoding ?? OptionDefaultStreamEncoding);
    }

    internal HtmlNode? GetXmlDeclaration()
    {
        if (!_documentnode.HasChildNodes)
        {
            return null;
        }

        foreach (var node in _documentnode._childrenNodes!)
            if (node.Name == "?xml") // it's ok, names are case sensitive
            {
                return node;
            }

        return null;
    }

    internal void SetIdForNode (HtmlNode node, string id)
    {
        if (!OptionUseIdAttribute)
        {
            return;
        }

        if (Nodesid == null! || id == null!)
        {
            return;
        }

        if (node == null!)
        {
            Nodesid.Remove (id);
        }
        else
        {
            Nodesid[id] = node;
        }
    }

    internal void UpdateLastParentNode()
    {
        do
        {
            if (_lastParentNode.Closed)
            {
                _lastParentNode = _lastParentNode.ParentNode!;
            }
        } while (_lastParentNode != null! && _lastParentNode.Closed);

        if (_lastParentNode == null)
        {
            _lastParentNode = _documentnode;
        }
    }

    #endregion

    #region Private Methods

    private void AddError (HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText,
        string reason)
    {
        var err = new HtmlParseError (code, line, linePosition, streamPosition, sourceText, reason);
        _parseerrors.Add (err);
        return;
    }

    private void CloseCurrentNode()
    {
        if (_currentNode.Closed) // text or document are by def closed
        {
            return;
        }

        var error = false;
        var prev = Utilities.GetDictionaryValueOrDefault (Lastnodes, _currentNode.Name);

        // find last node of this kind
        if (prev == null)
        {
            if (HtmlNode.IsClosedElement (_currentNode.Name))
            {
                // </br> will be seen as <br>
                _currentNode.CloseNode (_currentNode);

                // add to parent node
                if (_lastParentNode != null!)
                {
                    HtmlNode? foundNode = null;
                    var futureChild = new Stack<HtmlNode>();

                    if (!_currentNode.Name.Equals ("br"))
                    {
                        for (var node = _lastParentNode.LastChild; node != null; node = node.PreviousSibling)
                        {
                            // br node never can contains other nodes.
                            if (node.Name == _currentNode.Name && !node.HasChildNodes)
                            {
                                foundNode = node;
                                break;
                            }

                            futureChild.Push (node);
                        }
                    }


                    if (foundNode != null)
                    {
                        while (futureChild.Count != 0)
                        {
                            var node = futureChild.Pop();
                            _lastParentNode.RemoveChild (node);
                            foundNode.AppendChild (node);
                        }
                    }
                    else
                    {
                        _lastParentNode.AppendChild (_currentNode);
                    }
                }
            }
            else
            {
                // node has no parent
                // node is not a closed node

                if (HtmlNode.CanOverlapElement (_currentNode.Name))
                {
                    // this is a hack: add it as a text node
                    var closenode = CreateNode (HtmlNodeType.Text, _currentNode._outerstartindex);
                    closenode._outerlength = _currentNode._outerlength;
                    ((HtmlTextNode)closenode).Text = ((HtmlTextNode)closenode).Text.ToLowerInvariant();
                    if (_lastParentNode != null!)
                    {
                        _lastParentNode.AppendChild (closenode);
                    }
                }
                else
                {
                    if (HtmlNode.IsEmptyElement (_currentNode.Name))
                    {
                        AddError (
                            HtmlParseErrorCode.EndTagNotRequired,
                            _currentNode.Line, _currentNode.LinePosition,
                            _currentNode._streamPosition, _currentNode.OuterHtml!,
                            "End tag </" + _currentNode.Name + "> is not required");
                    }
                    else
                    {
                        // node cannot overlap, node is not empty
                        AddError (
                            HtmlParseErrorCode.TagNotOpened,
                            _currentNode.Line, _currentNode.LinePosition,
                            _currentNode._streamPosition, _currentNode.OuterHtml!,
                            "Start tag <" + _currentNode.Name + "> was not found");
                        error = true;
                    }
                }
            }
        }
        else
        {
            if (OptionFixNestedTags)
            {
                if (FindResetterNodes (prev, GetResetters (_currentNode.Name)))
                {
                    AddError (
                        HtmlParseErrorCode.EndTagInvalidHere,
                        _currentNode.Line, _currentNode.LinePosition,
                        _currentNode._streamPosition, _currentNode.OuterHtml!,
                        "End tag </" + _currentNode.Name + "> invalid here");
                    error = true;
                }
            }

            if (!error)
            {
                Lastnodes[_currentNode.Name] = prev._prevWithSameName;
                prev.CloseNode (_currentNode);
            }
        }


        // we close this node, get grandparent
        if (!error)
        {
            if (_lastParentNode != null &&
                (!HtmlNode.IsClosedElement (_currentNode.Name) ||
                 _currentNode._startTag))
            {
                UpdateLastParentNode();
            }
        }
    }

    private string CurrentNodeName()
    {
        return Text.Substring (_currentNode._namestartindex, _currentNode._namelength);
    }


    private void DecrementPosition()
    {
        _index--;
        if (_lineposition == 0)
        {
            _lineposition = _maxlineposition;
            _line--;
        }
        else
        {
            _lineposition--;
        }
    }

    private HtmlNode? FindResetterNode
        (
            HtmlNode node,
            string name
        )
    {
        var resetter = Utilities.GetDictionaryValueOrDefault (Lastnodes, name);
        if (resetter == null)
        {
            return null;
        }

        if (resetter.Closed)
        {
            return null;
        }

        if (resetter._streamPosition < node._streamPosition)
        {
            return null;
        }

        return resetter;
    }

    private bool FindResetterNodes
        (
            HtmlNode node,
            string[]? names
        )
    {
        if (names == null)
        {
            return false;
        }

        for (var i = 0; i < names.Length; i++)
        {
            if (FindResetterNode (node, names[i]) != null)
            {
                return true;
            }
        }

        return false;
    }

    private void FixNestedTag
        (
            string name,
            string[]? resetters
        )
    {
        if (resetters == null)
        {
            return;
        }

        var prev = Utilities.GetDictionaryValueOrDefault (Lastnodes, _currentNode.Name);

        // if we find a previous unclosed same name node, without a resetter node between, we must close it
        if (prev == null || Lastnodes[name].Closed)
        {
            return;
        }

        // try to find a resetter node, if found, we do nothing
        if (FindResetterNodes (prev, resetters))
        {
            return;
        }

        // ok we need to close the prev now
        // create a fake closer node
        var close = new HtmlNode (prev.NodeType, this, -1);
        close._endNode = close;
        prev.CloseNode (close);
    }

    private void FixNestedTags()
    {
        // we are only interested by start tags, not closing tags
        if (!_currentNode._startTag)
        {
            return;
        }

        var name = CurrentNodeName();
        FixNestedTag (name, GetResetters (name));
    }

    private string[]? GetResetters (string name)
    {
        string[]? resetters;

        if (!HtmlResetters.TryGetValue (name, out resetters))
        {
            return null;
        }

        return resetters;
    }

    private void IncrementPosition()
    {
        if (_crc32 != null!)
        {
            // REVIEW: should we add some checksum code in DecrementPosition too?
            _crc32.AddToCRC32 (_c);
        }

        _index++;
        _maxlineposition = _lineposition;
        if (_c == 10)
        {
            _lineposition = 0;
            _line++;
        }
        else
        {
            _lineposition++;
        }
    }

    private bool IsValidTag()
    {
        var isValidTag = _c == '<' && _index < Text.Length && (Char.IsLetter (Text[_index]) || Text[_index] == '/' ||
                                                               Text[_index] == '?' || Text[_index] == '!' ||
                                                               Text[_index] == '%');
        return isValidTag;
    }

    private bool NewCheck()
    {
        if (_c != '<' || !IsValidTag())
        {
            return false;
        }

        if (_index < Text.Length)
        {
            if (Text[_index] == '%')
            {
                if (DisableServerSideCode)
                {
                    return false;
                }

                switch (_state)
                {
                    case ParseState.AttributeAfterEquals:
                        PushAttributeValueStart (_index - 1);
                        break;

                    case ParseState.BetweenAttributes:
                        PushAttributeNameStart (_index - 1, _lineposition - 1);
                        break;

                    case ParseState.WhichTag:
                        PushNodeNameStart (true, _index - 1);
                        _state = ParseState.Tag;
                        break;
                }

                _oldstate = _state;
                _state = ParseState.ServerSideCode;
                return true;
            }
        }

        if (!PushNodeEnd (_index - 1, true))
        {
            // stop parsing
            _index = Text.Length;
            return true;
        }

        _state = ParseState.WhichTag;
        if (_index - 1 <= Text.Length - 2)
        {
            if (Text[_index] == '!' || Text[_index] == '?')
            {
                PushNodeStart (HtmlNodeType.Comment, _index - 1, _lineposition - 1);
                PushNodeNameStart (true, _index);
                PushNodeNameEnd (_index + 1);
                _state = ParseState.Comment;
                if (_index < Text.Length - 2)
                {
                    if (Text[_index + 1] == '-' &&
                        Text[_index + 2] == '-')
                    {
                        _fullcomment = true;
                    }
                    else
                    {
                        _fullcomment = false;
                    }
                }

                return true;
            }
        }

        PushNodeStart (HtmlNodeType.Element, _index - 1, _lineposition - 1);
        return true;
    }

    private void Parse()
    {
        if (ParseExecuting != null!)
        {
            ParseExecuting (this);
        }

        var lastquote = 0;
        if (OptionComputeChecksum)
        {
            _crc32 = new Crc32();
        }

        Lastnodes = new Dictionary<string, HtmlNode>();
        _c = 0;
        _fullcomment = false;
        _parseerrors = new List<HtmlParseError>();
        _line = 1;
        _lineposition = 0;
        _maxlineposition = 0;

        _state = ParseState.Text;
        _oldstate = _state;
        _documentnode._innerlength = Text.Length;
        _documentnode._outerlength = Text.Length;
        _remainderOffset = Text.Length;

        _lastParentNode = _documentnode;
        _currentNode = CreateNode (HtmlNodeType.Text, 0);
        _currentAttribute = null!;

        _index = 0;
        PushNodeStart (HtmlNodeType.Text, 0, _lineposition);
        while (_index < Text.Length)
        {
            _c = Text[_index];
            IncrementPosition();

            switch (_state)
            {
                case ParseState.Text:
                    if (NewCheck())
                    {
                        continue;
                    }

                    break;

                case ParseState.WhichTag:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (_c == '/')
                    {
                        PushNodeNameStart (false, _index);
                    }
                    else
                    {
                        PushNodeNameStart (true, _index - 1);
                        DecrementPosition();
                    }

                    _state = ParseState.Tag;
                    break;

                case ParseState.Tag:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (IsWhiteSpace (_c))
                    {
                        CloseParentImplicitExplicitNode();

                        PushNodeNameEnd (_index - 1);
                        if (_state != ParseState.Tag)
                        {
                            continue;
                        }

                        _state = ParseState.BetweenAttributes;
                        continue;
                    }

                    if (_c == '/')
                    {
                        CloseParentImplicitExplicitNode();

                        PushNodeNameEnd (_index - 1);
                        if (_state != ParseState.Tag)
                        {
                            continue;
                        }

                        _state = ParseState.EmptyTag;
                        continue;
                    }

                    if (_c == '>')
                    {
                        CloseParentImplicitExplicitNode();

                        //// CHECK if parent is compatible with end tag
                        //if (IsParentIncompatibleEndTag())
                        //{
                        //    _state = ParseState.Text;
                        //    PushNodeStart(HtmlNodeType.Text, _index);
                        //    break;
                        //}

                        PushNodeNameEnd (_index - 1);
                        if (_state != ParseState.Tag)
                        {
                            continue;
                        }

                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.Tag)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                    }

                    break;

                case ParseState.BetweenAttributes:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (IsWhiteSpace (_c))
                    {
                        continue;
                    }

                    if (_c is '/' or '?')
                    {
                        _state = ParseState.EmptyTag;
                        continue;
                    }

                    if (_c == '>')
                    {
                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.BetweenAttributes)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    PushAttributeNameStart (_index - 1, _lineposition - 1);
                    _state = ParseState.AttributeName;
                    break;

                case ParseState.EmptyTag:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (_c == '>')
                    {
                        if (!PushNodeEnd (_index, true))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.EmptyTag)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    // we may end up in this state if attributes are incorrectly seperated
                    // by a /-character. If so, start parsing attribute-name immediately.
                    if (!IsWhiteSpace (_c))
                    {
                        // Just do nothing and push to next one!
                        DecrementPosition();
                        _state = ParseState.BetweenAttributes;
                        continue;
                    }
                    else
                    {
                        _state = ParseState.BetweenAttributes;
                    }

                    break;

                case ParseState.AttributeName:
                    if (NewCheck())
                    {
                        continue;
                    }

                    _currentAttribute._isFromParse = true;

                    if (IsWhiteSpace (_c))
                    {
                        PushAttributeNameEnd (_index - 1);
                        _state = ParseState.AttributeBeforeEquals;
                        continue;
                    }

                    if (_c == '=')
                    {
                        PushAttributeNameEnd (_index - 1);
                        _currentAttribute._hasEqual = true;
                        _state = ParseState.AttributeAfterEquals;
                        continue;
                    }

                    if (_c == '>')
                    {
                        PushAttributeNameEnd (_index - 1);
                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.AttributeName)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    break;

                case ParseState.AttributeBeforeEquals:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (IsWhiteSpace (_c))
                    {
                        continue;
                    }

                    if (_c == '>')
                    {
                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.AttributeBeforeEquals)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    if (_c == '=')
                    {
                        _currentAttribute._hasEqual = true;
                        _state = ParseState.AttributeAfterEquals;
                        continue;
                    }

                    // no equals, no whitespace, it's a new attrribute starting
                    _state = ParseState.BetweenAttributes;
                    DecrementPosition();
                    break;

                case ParseState.AttributeAfterEquals:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (IsWhiteSpace (_c))
                    {
                        continue;
                    }

                    if (_c is '\'' or '"')
                    {
                        _state = ParseState.QuotedAttributeValue;
                        PushAttributeValueStart (_index, _c);
                        lastquote = _c;
                        continue;
                    }

                    if (_c == '>')
                    {
                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.AttributeAfterEquals)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    PushAttributeValueStart (_index - 1);
                    _state = ParseState.AttributeValue;
                    break;

                case ParseState.AttributeValue:
                    if (NewCheck())
                    {
                        continue;
                    }

                    if (IsWhiteSpace (_c))
                    {
                        PushAttributeValueEnd (_index - 1);
                        _state = ParseState.BetweenAttributes;
                        continue;
                    }

                    if (_c == '>')
                    {
                        PushAttributeValueEnd (_index - 1);
                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        if (_state != ParseState.AttributeValue)
                        {
                            continue;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    break;

                case ParseState.QuotedAttributeValue:
                    if (_c == lastquote)
                    {
                        PushAttributeValueEnd (_index - 1);
                        _state = ParseState.BetweenAttributes;
                        continue;
                    }

                    if (_c == '<')
                    {
                        if (_index < Text.Length)
                        {
                            if (Text[_index] == '%')
                            {
                                _oldstate = _state;
                                _state = ParseState.ServerSideCode;
                                continue;
                            }
                        }
                    }

                    break;

                case ParseState.Comment:
                    if (_c == '>')
                    {
                        if (_fullcomment)
                        {
                            if ((Text[_index - 2] != '-' || Text[_index - 3] != '-')
                                &&
                                (Text[_index - 2] != '!' || Text[_index - 3] != '-' ||
                                 Text[_index - 4] != '-'))
                            {
                                continue;
                            }
                        }

                        if (!PushNodeEnd (_index, false))
                        {
                            // stop parsing
                            _index = Text.Length;
                            break;
                        }

                        _state = ParseState.Text;
                        PushNodeStart (HtmlNodeType.Text, _index, _lineposition);
                        continue;
                    }

                    break;

                case ParseState.ServerSideCode:
                    if (_c == '%')
                    {
                        if (_index < Text.Length)
                        {
                            if (Text[_index] == '>')
                            {
                                switch (_oldstate)
                                {
                                    case ParseState.AttributeAfterEquals:
                                        _state = ParseState.AttributeValue;
                                        break;

                                    case ParseState.BetweenAttributes:
                                        PushAttributeNameEnd (_index + 1);
                                        _state = ParseState.BetweenAttributes;
                                        break;

                                    default:
                                        _state = _oldstate;
                                        break;
                                }

                                IncrementPosition();
                            }
                        }
                    }
                    else if (_oldstate == ParseState.QuotedAttributeValue
                             && _c == lastquote)
                    {
                        _state = _oldstate;
                        DecrementPosition();
                    }

                    break;

                case ParseState.PcData:
                    // look for </tag + 1 char

                    // check buffer end
                    if (_currentNode._namelength + 3 <= Text.Length - (_index - 1))
                    {
                        if (string.Compare (Text.Substring (_index - 1, _currentNode._namelength + 2),
                                "</" + _currentNode.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            int c = Text[_index - 1 + 2 + _currentNode.Name.Length];
                            if (c == '>' || IsWhiteSpace (c))
                            {
                                // add the script as a text node
                                var script = CreateNode (HtmlNodeType.Text,
                                    _currentNode._outerstartindex +
                                    _currentNode._outerlength);
                                script._outerlength = _index - 1 - script._outerstartindex;
                                script._streamPosition = script._outerstartindex;
                                script.Line = _currentNode.Line;
                                script.LinePosition = _currentNode.LinePosition + _currentNode._namelength + 2;

                                _currentNode.AppendChild (script);

                                // https://www.w3schools.com/jsref/prop_node_innertext.asp
                                // textContent returns the text content of all elements, while innerText returns the content of all elements, except for <script> and <style> elements.
                                // innerText will not return the text of elements that are hidden with CSS (textContent will). ==> The parser do not support that.
                                if (_currentNode.Name.ToLowerInvariant().Equals ("script") ||
                                    _currentNode.Name.ToLowerInvariant().Equals ("style"))
                                {
                                    _currentNode._isHideInnerText = true;
                                }

                                PushNodeStart (HtmlNodeType.Element, _index - 1, _lineposition - 1);
                                PushNodeNameStart (false, _index - 1 + 2);
                                _state = ParseState.Tag;
                                IncrementPosition();
                            }
                        }
                    }

                    break;
            }
        }

        // TODO: Add implicit end here?


        // finish the current work
        if (_currentNode._namestartindex > 0)
        {
            PushNodeNameEnd (_index);
        }

        PushNodeEnd (_index, false);

        // we don't need this anymore
        Lastnodes.Clear();
    }

    // In this moment, we don't have value.
    // Potential: "\"", "'", "[", "]", "<", ">", "-", "|", "/", "\\"
    private static readonly List<string> BlockAttributes = new () { "\"", "'" };

    private void PushAttributeNameEnd (int index)
    {
        _currentAttribute._namelength = index - _currentAttribute._namestartindex;

        if (_currentAttribute.Name != null! && !BlockAttributes.Contains (_currentAttribute.Name))
        {
            _currentNode.Attributes.Append (_currentAttribute);
        }
    }

    private void PushAttributeNameStart (int index, int lineposition)
    {
        _currentAttribute = CreateAttribute();
        _currentAttribute._namestartindex = index;
        _currentAttribute.Line = _line;
        _currentAttribute._lineposition = lineposition;
        _currentAttribute._streamposition = index;
    }

    private void PushAttributeValueEnd (int index)
    {
        _currentAttribute._valuelength = index - _currentAttribute._valuestartindex;
    }

    private void PushAttributeValueStart (int index)
    {
        PushAttributeValueStart (index, 0);
    }

    private void CloseParentImplicitExplicitNode()
    {
        var hasNodeToClose = true;

        while (hasNodeToClose && !_lastParentNode.Closed)
        {
            hasNodeToClose = false;

            var forceExplicitEnd = false;

            // CHECK if parent must be implicitely closed
            if (IsParentImplicitEnd())
            {
                if (OptionOutputAsXml)
                {
                    forceExplicitEnd = true;
                }
                else
                {
                    CloseParentImplicitEnd();
                    hasNodeToClose = true;
                }
            }

            // CHECK if parent must be explicitely closed
            if (forceExplicitEnd || IsParentExplicitEnd())
            {
                CloseParentExplicitEnd();
                hasNodeToClose = true;
            }
        }
    }

    private bool IsParentImplicitEnd()
    {
        // MUST be a start tag
        if (!_currentNode._startTag)
        {
            return false;
        }

        var isImplicitEnd = false;
        var parent = _lastParentNode.Name;
        var nodeName = Text.Substring (_currentNode._namestartindex, _index - _currentNode._namestartindex - 1)
            .ToLowerInvariant();

        switch (parent)
        {
            case "a":
                isImplicitEnd = nodeName == "a";
                break;

            case "dd":
                isImplicitEnd = nodeName is "dt" or "dd";
                break;

            case "dt":
                isImplicitEnd = nodeName is "dt" or "dd";
                break;

            case "li":
                isImplicitEnd = nodeName == "li";
                break;

            case "p":
                if (DisableBehaviorTagP)
                {
                    isImplicitEnd = nodeName is "address" or "article" or "aside" or "blockquote" or "dir" or "div" or "dl" or "fieldset" or "footer" or "form" or "h1" or "h2" or "h3" or "h4" or "h5" or "h6" or "header" or "hr" or "menu" or "nav" or "ol" or "p" or "pre" or "section" or "table" or "ul";
                }
                else
                {
                    isImplicitEnd = nodeName == "p";
                }

                break;

            case "option":
                isImplicitEnd = nodeName == "option";
                break;
        }

        return isImplicitEnd;
    }

    private bool IsParentExplicitEnd()
    {
        // MUST be a start tag
        if (!_currentNode._startTag)
        {
            return false;
        }

        var isExplicitEnd = false;
        var parent = _lastParentNode.Name;
        var nodeName = Text.Substring (_currentNode._namestartindex, _index - _currentNode._namestartindex - 1)
            .ToLowerInvariant();

        switch (parent)
        {
            case "title":
                isExplicitEnd = nodeName == "title";
                break;

            case "p":
                isExplicitEnd = nodeName == "div";
                break;

            case "table":
                isExplicitEnd = nodeName == "table";
                break;

            case "tr":
                isExplicitEnd = nodeName == "tr";
                break;

            case "td":
                isExplicitEnd = nodeName is "td" or "th" or "tr";
                break;

            case "th":
                isExplicitEnd = nodeName is "td" or "th" or "tr";
                break;

            case "h1":
                isExplicitEnd = nodeName is "h2" or "h3" or "h4" or "h5";
                break;

            case "h2":
                isExplicitEnd = nodeName is "h1" or "h3" or "h4" or "h5";
                break;

            case "h3":
                isExplicitEnd = nodeName is "h1" or "h2" or "h4" or "h5";
                break;

            case "h4":
                isExplicitEnd = nodeName is "h1" or "h2" or "h3" or "h5";
                break;

            case "h5":
                isExplicitEnd = nodeName is "h1" or "h2" or "h3" or "h4";
                break;
        }

        return isExplicitEnd;
    }

    //private bool IsParentIncompatibleEndTag()
    //{
    //    // MUST be a end tag
    //    if (_currentnode._starttag) return false;

    //    bool isIncompatible = false;

    //    var parent = _lastparentnode.Name;
    //    var nodeName = Text.Substring(_currentnode._namestartindex, _index - _currentnode._namestartindex - 1);

    //    switch (parent)
    //    {
    //        case "h1":
    //            isIncompatible = nodeName == "h2" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
    //            break;
    //        case "h2":
    //            isIncompatible = nodeName == "h1" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
    //            break;
    //        case "h3":
    //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h4" || nodeName == "h5";
    //            break;
    //        case "h4":
    //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h5";
    //            break;
    //        case "h5":
    //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h4";
    //            break;
    //    }

    //    return isIncompatible;
    //}

    private void CloseParentImplicitEnd()
    {
        var close = new HtmlNode (_lastParentNode.NodeType, this, -1);
        close._endNode = close;
        close._isImplicitEnd = true;
        _lastParentNode._isImplicitEnd = true;
        _lastParentNode.CloseNode (close);
    }

    private void CloseParentExplicitEnd()
    {
        var close = new HtmlNode (_lastParentNode.NodeType, this, -1);
        close._endNode = close;
        _lastParentNode.CloseNode (close);
    }

    private void PushAttributeValueStart (int index, int quote)
    {
        _currentAttribute._valuestartindex = index;
        if (quote == '\'')
        {
            _currentAttribute.QuoteType = AttributeValueQuote.SingleQuote;
        }

        _currentAttribute.InternalQuoteType = _currentAttribute.QuoteType;

        if (quote == 0)
        {
            _currentAttribute.InternalQuoteType = AttributeValueQuote.None;
        }
    }

    private bool PushNodeEnd (int index, bool close)
    {
        _currentNode._outerlength = index - _currentNode._outerstartindex;

        if (_currentNode._nodeType is HtmlNodeType.Text or HtmlNodeType.Comment)
        {
            // forget about void nodes
            if (_currentNode._outerlength > 0)
            {
                _currentNode._innerlength = _currentNode._outerlength;
                _currentNode._innerstartindex = _currentNode._outerstartindex;
                if (_lastParentNode != null!)
                {
                    _lastParentNode.AppendChild (_currentNode);
                }
            }
        }
        else
        {
            if (_currentNode._startTag && _lastParentNode != _currentNode)
            {
                // add to parent node
                if (_lastParentNode != null!)
                {
                    _lastParentNode.AppendChild (_currentNode);
                }

                ReadDocumentEncoding (_currentNode);

                // remember last node of this kind
                var prev = Utilities.GetDictionaryValueOrDefault (Lastnodes, _currentNode.Name);

                _currentNode._prevWithSameName = prev!;
                Lastnodes[_currentNode.Name] = _currentNode;

                // change parent?
                if (_currentNode.NodeType is HtmlNodeType.Document or HtmlNodeType.Element)
                {
                    _lastParentNode = _currentNode;
                }

                if (HtmlNode.IsCDataElement (CurrentNodeName()))
                {
                    _state = ParseState.PcData;
                    return true;
                }

                if (HtmlNode.IsClosedElement (_currentNode.Name) ||
                    HtmlNode.IsEmptyElement (_currentNode.Name))
                {
                    close = true;
                }
            }
        }

        if (close || !_currentNode._startTag)
        {
            if (OptionStopperNodeName != null! && _remainder == null! &&
                string.Compare (_currentNode.Name, OptionStopperNodeName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                _remainderOffset = index;
                _remainder = Text.Substring (_remainderOffset);
                CloseCurrentNode();
                return false; // stop parsing
            }

            CloseCurrentNode();
        }

        return true;
    }

    private void PushNodeNameEnd (int index)
    {
        _currentNode._namelength = index - _currentNode._namestartindex;
        if (OptionFixNestedTags)
        {
            FixNestedTags();
        }
    }

    private void PushNodeNameStart (bool starttag, int index)
    {
        _currentNode._startTag = starttag;
        _currentNode._namestartindex = index;
    }

    private void PushNodeStart (HtmlNodeType type, int index, int lineposition)
    {
        _currentNode = CreateNode (type, index);
        _currentNode.Line = _line;
        _currentNode.LinePosition = lineposition;
        _currentNode._streamPosition = index;
    }

    private void ReadDocumentEncoding (HtmlNode node)
    {
        if (!OptionReadEncoding)
        {
            return;
        }

        // format is
        // <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />

        // when we append a child, we are in node end, so attributes are already populated
        if (node._namelength != 4) // quick check, avoids string alloc
        {
            return;
        }

        if (node.Name != "meta") // all nodes names are lowercase
        {
            return;
        }

        string? charset = null;
        var att = node.Attributes["http-equiv"];
        if (att != null)
        {
            if (string.Compare (att.Value, "content-type", StringComparison.OrdinalIgnoreCase) != 0)
            {
                return;
            }

            var content = node.Attributes["content"];
            if (content != null)
            {
                charset = NameValuePairList.GetNameValuePairsValue (content.Value, "charset");
            }
        }
        else
        {
            att = node.Attributes["charset"];
            if (att != null)
            {
                charset = att.Value;
            }
        }

        if (!string.IsNullOrEmpty (charset))
        {
            // The following check fixes the the bug described at: http://htmlagilitypack.codeplex.com/WorkItem/View.aspx?WorkItemId=25273
            if (string.Equals (charset, "utf8", StringComparison.OrdinalIgnoreCase))
            {
                charset = "utf-8";
            }

            try
            {
                _declaredEncoding = Encoding.GetEncoding (charset);
            }
            catch (ArgumentException)
            {
                _declaredEncoding = null;
            }

            if (_onlyDetectEncoding)
            {
                throw new EncodingFoundException (_declaredEncoding!);
            }

            if (_streamEncoding != null!)
            {
                if (_declaredEncoding != null)
                {
                    if (_declaredEncoding.CodePage != _streamEncoding.CodePage)
                    {
                        AddError (
                            HtmlParseErrorCode.CharsetMismatch,
                            _line, _lineposition,
                            _index, node.OuterHtml!,
                            "Encoding mismatch between StreamEncoding: " +
                            _streamEncoding.WebName + " and DeclaredEncoding: " +
                            _declaredEncoding.WebName);
                    }
                }
            }
        }
    }

    #endregion

    #region Nested type: ParseState

    private enum ParseState
    {
        Text,
        WhichTag,
        Tag,
        BetweenAttributes,
        EmptyTag,
        AttributeName,
        AttributeBeforeEquals,
        AttributeAfterEquals,
        AttributeValue,
        Comment,
        QuotedAttributeValue,
        ServerSideCode,
        PcData
    }

    #endregion
}
