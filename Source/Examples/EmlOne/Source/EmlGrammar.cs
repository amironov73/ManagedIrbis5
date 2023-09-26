// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

/* EmlGrammar.cs -- грамматика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.Lexey.Barsik.Ast;
using AM.Lexey.Eml.Ast;
using AM.Lexey.Parsing;
using AM.Lexey.Tokenizing;

#endregion

namespace EmlOne;

/// <summary>
/// Грамматика.
/// </summary>
internal static class EmlGrammar
{
    /// <summary>
    /// Зарезервированные слова.
    /// </summary>
    private static readonly string[] ReservedWords = { "import" };

    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    private static readonly string[] KnownTerms = { "{", "}", "=", "." };

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
    private static readonly Parser<string> Identifier =
        new IdentifierParser().SeparatedBy (Term ("."))
            .Map (it => string.Join ('.', it));

    /// <summary>
    /// Терм.
    /// </summary>
    private static TermParser Term (params string[] expected) => new (expected);

    /// <summary>
    /// Директива <c>import</c>.
    /// </summary>
    private static readonly Parser<ImportNode> Import = Parser.Chain
        (
            Reserved ("import"),
            Identifier,
            (_, name) => new ImportNode (name)
        );

    /// <summary>
    /// Секция <c>import</c>.
    /// </summary>
    private static readonly Parser<IList<ImportNode>> ImportSection = Import.Repeated();

    /// <summary>
    /// Свойство.
    /// </summary>
    private static readonly Parser<PropertyNode> Property = Parser.Chain
        (
            Identifier,
            Term ("="),
            Literal,
            (name, _, value) => new PropertyNode (name, value)
        );

    /// <summary>
    /// Контрол.
    /// </summary>
    private static readonly ParserHolder<ControlNode> Control = new (null!);

    /// <summary>
    /// Программа.
    /// </summary>
    private static readonly Parser<EmlProgramNode> Program = Parser.Chain
        (
            ImportSection,
            Control,
            (imports, root) => new EmlProgramNode(imports, root)
        );

    private static bool _isInitialized;

    private static void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        Control.Value = Parser.Chain
            (
                Identifier,
                Term ("{"),
                Property.Repeated (),
                Control.Repeated (),
                Term ("}"),
                (name, _, properties, children, _)
                    => new ControlNode (name, properties, children)
            );

        _isInitialized = true;
    }

    /// <summary>
    /// Разбор исходного кода программы.
    /// </summary>
    public static EmlProgramNode ParseSourceCode
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        Initialize();

        var tokenizer = new Tokenizer
        {
            Refiner = new StandardTokenRefiner (ReservedWords),
            Recognizers =
            {
                new WhitespaceRecognizer(),
                new IdentifierRecognizer(),
                new IntegerRecognizer(),
                new TermRecognizer (KnownTerms)
            }
        };
        var tokens = tokenizer.ScanForTokens (sourceCode);
        var state = new ParseState (tokens);

        return Program.ParseOrThrow (state);
    }

    /// <summary>
    /// Загрузка разметки из файла с последующим разбором.
    /// </summary>
    public static EmlProgramNode ParseFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var sourceCode = File.ReadAllText (fileName);
        return ParseSourceCode (sourceCode);
    }
}
