// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using AM;

using PdfSharpCore.Internal;
using PdfSharpCore.Pdf.Content.Objects;

#endregion

#nullable enable

#pragma warning disable 1591

namespace PdfSharpCore.Pdf.Content;

/// <summary>
/// Provides the functionality to parse PDF content streams.
/// </summary>
public sealed class CParser
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="page"></param>
    public CParser (PdfPage page)
    {
        _page = page;
        var content = page.Contents.CreateSingleContent();
        var bytes = content.Stream.ThrowIfNull().Value;
        _lexer = new CLexer (bytes);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="content"></param>
    public CParser (byte[] content)
    {
        _lexer = new CLexer (content);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="content"></param>
    public CParser (MemoryStream content)
    {
        _lexer = new CLexer (content.ToArray());
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="lexer"></param>
    public CParser (CLexer lexer)
    {
        _lexer = lexer;
    }

    #endregion

    public CSymbol Symbol => _lexer.Symbol;

    public CSequence ReadContent()
    {
        var sequence = new CSequence();
        ParseObject (sequence, CSymbol.Eof);

        return sequence;
    }

    /// <summary>
    /// Parses whatever comes until the specified stop symbol is reached.
    /// </summary>
    void ParseObject (CSequence sequence, CSymbol stop)
    {
        CSymbol symbol;
        while ((symbol = ScanNextToken()) != CSymbol.Eof)
        {
            if (symbol == stop)
                return;

            CString s;
            COperator op;
            switch (symbol)
            {
                case CSymbol.Comment:
                    // ignore comments
                    break;

                case CSymbol.Integer:
                    var n = new CInteger
                    {
                        Value = _lexer.TokenToInteger
                    };
                    _operands.Add (n);
                    break;

                case CSymbol.Real:
                    var r = new CReal
                    {
                        Value = _lexer.TokenToReal
                    };
                    _operands.Add (r);
                    break;

                case CSymbol.String:
                case CSymbol.HexString:
                case CSymbol.UnicodeString:
                case CSymbol.UnicodeHexString:
                    s = new CString
                    {
                        Value = _lexer.Token
                    };
                    _operands.Add (s);
                    break;

                case CSymbol.Dictionary:
                    s = new CString
                    {
                        Value = _lexer.Token,
                        CStringType = CStringType.Dictionary
                    };
                    _operands.Add (s);
                    op = CreateOperator (OpCodeName.Dictionary);

                    //_operands.Clear();
                    sequence.Add (op);

                    break;

                case CSymbol.Name:
                    var name = new CName
                    {
                        Name = _lexer.Token
                    };
                    _operands.Add (name);
                    break;

                case CSymbol.Operator:
                    op = CreateOperator();

                    //_operands.Clear();
                    sequence.Add (op);
                    break;

                case CSymbol.BeginArray:
                    var array = new CArray();
                    if (_operands.Count != 0)
                        ContentReaderDiagnostics.ThrowContentReaderException ("Array within array...");

                    ParseObject (array, CSymbol.EndArray);
                    array.Add (_operands);
                    _operands.Clear();
                    _operands.Add ((CObject)array);
                    break;

                case CSymbol.EndArray:
                    ContentReaderDiagnostics.HandleUnexpectedCharacter (']');
                    break;

#if DEBUG
                default:
                    Debug.Assert (false);
                    break;
#endif
            }
        }
    }

    COperator CreateOperator()
    {
        var name = _lexer.Token;
        var op = OpCodes.OperatorFromName (name);
        return CreateOperator (op);
    }

    COperator CreateOperator (OpCodeName nameop)
    {
        var name = nameop.ToString();
        var op = OpCodes.OperatorFromName (name);
        return CreateOperator (op);
    }

    COperator CreateOperator (COperator op)
    {
        if (op.OpCode.OpCodeName == OpCodeName.BI)
        {
            _lexer.ScanInlineImage();
        }
#if DEBUG
        if (op.OpCode.Operands != -1 && op.OpCode.Operands != _operands.Count)
        {
            if (op.OpCode.OpCodeName != OpCodeName.ID)
            {
                GetType();
                Debug.Assert (false, "Invalid number of operands.");
            }
        }
#endif
        op.Operands.Add (_operands);
        _operands.Clear();
        return op;
    }

    CSymbol ScanNextToken()
    {
        return _lexer.ScanNextToken();
    }

    CSymbol ScanNextToken (out string token)
    {
        var symbol = _lexer.ScanNextToken();
        token = _lexer.Token;
        return symbol;
    }

    /// <summary>
    /// Reads the next symbol that must be the specified one.
    /// </summary>
    CSymbol ReadSymbol (CSymbol symbol)
    {
        var current = _lexer.ScanNextToken();
        if (symbol != current)
            ContentReaderDiagnostics.ThrowContentReaderException (PSSR.UnexpectedToken (_lexer.Token));
        return current;
    }

    readonly CSequence _operands = new CSequence();
    private PdfPage? _page;
    readonly CLexer _lexer;
}
