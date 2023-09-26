// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Grammar.cs -- грамматика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;
using AM.Lexey.Barsik.Ast;
using AM.Lexey.Parsing;
using AM.Lexey.Tokenizing;

#endregion

namespace EmlOne;

/// <summary>
/// Грамматика.
/// </summary>
internal static class Grammar
{
    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    private static readonly string[] KnownTerms = new[] { "{", "}" };

    /// <summary>
    /// Порождение константного узла.
    /// </summary>
    private static readonly Parser<AtomNode> Literal = new LiteralParser().Map
        (
            x => (AtomNode) new ConstantNode (x)
        );

    /// <summary>
    /// Разбор зарезервированного слова.
    /// </summary>
    /// <param name="word"><c>null</c> означает "любое зарезервированное слово".
    /// </param>
    private static ReservedWordParser Reserved (string? word) => new (word);

    /// <summary>
    /// Разбор идентификаторов.
    /// </summary>
    private static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Терм.
    /// </summary>
    private static TermParser Term (params string[] expected) => new (expected);

    /// <summary>
    /// Программа.
    /// </summary>
    private static Parser<AM.Lexey.Eml.Ast.ProgramNode> Program = null!;

    /// <summary>
    /// Загрузка разметки.
    /// </summary>
    public static AM.Lexey.Eml.Ast.ProgramNode Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var sourceCode = File.ReadAllText (fileName);

        var tokenizer = new Tokenizer
        {
            Refiner = new StandardTokenRefiner(),
            Recognizers =
            {
                new IntegerRecognizer(),
                new WhitespaceRecognizer(),
                new TermRecognizer (KnownTerms)
            }
        };
        var tokens = tokenizer.ScanForTokens (sourceCode);
        var state = new ParseState (tokens);
        return Program.ParseOrThrow (state);
    }
}
