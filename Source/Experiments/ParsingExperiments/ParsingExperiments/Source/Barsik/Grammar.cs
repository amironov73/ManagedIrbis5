// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantSuppressNullableWarningExpression

/* Grammar.cs -- грамматика Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System.Collections.Generic;

using Pidgin;
using Pidgin.Comment;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Грамматика Barsik.
/// Точнее,
/// <list type="number">
/// <item>общие определения (например, что есть токен),</item>
/// <item>стейтменты, разделители стейтментов и блоки стейтментов
/// (в т. ч. программа - тоже блок стейтментов).</item>
/// </list>
/// <para>Выражения см. <see cref="AssignmentNode"/>.</para>
/// </summary>
internal static class Grammar
{
    #region Public methods

    // /// <summary>
    // /// Разбор текста скрипта.
    // /// </summary>
    // public static ProgramNode ParseProgram
    //     (
    //         string sourceCode
    //     )
    // {
    //     Sure.NotNull (sourceCode);
    //
    //     return WholeProgram.ParseOrThrow (sourceCode);
    // }
    //
    // /// <summary>
    // /// Разбор текста программы.
    // /// </summary>
    // public static AtomNode ParseExpression
    //     (
    //         string sourceCode
    //     )
    // {
    //     Sure.NotNull (sourceCode);
    //
    //     return Expr.Before (End).ParseOrThrow (sourceCode);
    // }

    #endregion

    #region Private members

    // многострочный комментарий в стиле C/C++
    private static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
        (
            Try (String ("/*")),
            Try (String ("*/"))
        );

    // однострочный комментарий в стиле C/C++
    private static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            Try (String ("//"))
        );

    // пространство между токенами
    private static readonly Parser<char, Unit> Filler = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    // токен, возможно, отделенный от других пробелами и комментариями
    internal static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (Filler);

    // токен-символ
    internal static Parser<char, char> Tok (char token) => Tok (Char (token));

    // токен-строка
    internal static Parser<char, string> Tok (string token) => Tok (String (token));

    // // точный токен
    // internal static Parser<char, string> Xok (string token) => new StrictParser (token);
    //
    // internal static Parser<char, string> Sok (string token) => Tok (new StrictParser (token));
    //
    // // точный токен
    // internal static Parser<char, string> Sok (string token, string unexpected) =>
    //     Tok (new StrictParser (token, unexpected));
    //
    // // нечто, заключенное в фигурные скобки
    // internal static Parser<char, TResult> CurlyBraces<TResult> (Parser<char, TResult> parser) =>
    //     Tok (parser).Between (Tok ('{'), Tok ('}'));
    //
    // // нечто, заключенное в круглые скобки
    // internal static Parser<char, TResult> RoundBrackets<TResult> (Parser<char, TResult> parser) =>
    //     Tok (parser).Between (Tok ('('), Tok (')'));
    //
    // // нечто, заключенное в угловые скобки
    // internal static Parser<char, TResult> CornerBrackets<TResult> (Parser<char, TResult> parser) =>
    //     Tok (parser).Between (Tok ('<'), Tok ('>'));
    //
    // // идентификатор
    // internal static readonly Parser<char, string> Identifier = new IdentifierParser();
    //
    // // копия чисто для удобства
    // private static readonly Parser<char, AtomNode> Expr = Rec (() => AssignmentNode.Expr);
    //
    // // разделитель стейтментов
    // internal static readonly Parser<char, Unit> StatementDelimiter = new SwallowParser (';');
    //
    // //
    // // Дальше начинаются разнообразные стейтменты
    // //
    //
    // // блок стейтментов
    // public static readonly Parser<char, IEnumerable<StatementNode>> Block = Map
    //     (
    //         (_, second) => second,
    //         Filler.Optional(),
    //         Rec (() => Statement!).SeparatedAndOptionallyTerminated (StatementDelimiter)
    //     );
    //
    // // блок в фигурных скобках либо один стейтмент
    // public static readonly Parser<char, IEnumerable<StatementNode>> BlockOrSingle =
    //     Try (CurlyBraces(Rec (() => Block)))
    //         .Or (Rec (() => Statement!).Repeat (1));
    //
    // // стейтмент, вычисляющий значение выражения (костыль)
    // private static readonly Parser<char, StatementNode> ExpressionStatement = Map
    //     (
    //         (pos, expr) => (StatementNode) new ExpressionNode (new SourcePosition (pos), expr),
    //         CurrentPos,
    //         Expr
    //     );
    //
    // // цикл while
    // private static readonly Parser<char, StatementNode> While = Map
    //     (
    //         (position, condition, body) =>
    //             (StatementNode) new WhileNode (new SourcePosition (position), condition, body),
    //         Sok ("while").Then (CurrentPos),
    //         RoundBrackets (Expr),
    //         BlockOrSingle
    //     );
    //
    // // цикл for
    // private static readonly Parser<char, StatementNode> For =
    //     from position in Sok ("for").Then (CurrentPos)
    //     from open1 in Tok ('(')
    //     from init in Tok (Expr)
    //     from _2 in Tok (';')
    //     from condition in Tok (Expr)
    //     from _3 in Token (';')
    //     from step in Tok (Expr)
    //     from close1 in Tok (')')
    //     from statements in BlockOrSingle
    //     from elseBody in Rec (() => Tok (Else!)).Optional()
    //     select (StatementNode) new ForNode (new SourcePosition(position), init, condition,
    //         step, statements, elseBody.GetValueOrDefault());
    //
    // // цикл foreach
    // private static readonly Parser<char, StatementNode> ForEach =
    //     from position in Sok ("foreach").Then (CurrentPos)
    //     from open1 in Tok ('(')
    //     from variableName in Tok (Identifier)
    //     from _2 in Sok ("in")
    //     from enumerable in Tok (Expr)
    //     from close1 in Tok (')')
    //     from statements in BlockOrSingle
    //     select (StatementNode) new ForEachNode (new SourcePosition (position),
    //         variableName, enumerable, statements);
    //
    // // блок else
    // private static readonly Parser<char, IEnumerable<StatementNode>> Else =
    //     from _ in Sok ("else")
    //     from statements in BlockOrSingle
    //     select statements;
    //
    // // блок else if
    // private static readonly Parser<char, IfNode> ElseIf =
    //     from position in Sok ("else").Then (CurrentPos)
    //     from _2 in Sok ("if")
    //     from condition in RoundBrackets (Expr)
    //     from statements in BlockOrSingle
    //     select new IfNode (new SourcePosition (position),
    //         condition, statements, null, null);
    //
    // // конструкция if-then-else
    // private static readonly Parser<char, StatementNode> If =
    //     from position in Sok ("if").Then (CurrentPos)
    //     from condition in RoundBrackets (Expr)
    //     from thenBlock in BlockOrSingle
    //     from elseIf in Try (ElseIf).Many().Optional()
    //     from elseBlock in Else.Optional()
    //     select (StatementNode)new IfNode (new SourcePosition (position),
    //         condition, thenBlock, elseIf.GetValueOrDefault(),
    //         elseBlock.GetValueOrDefault());
    //
    // // блок catch
    // private static readonly Parser<char, CatchNode> Catch =
    //     from position in Sok ("catch").Then (CurrentPos)
    //     from variable in RoundBrackets(Identifier)
    //     from block in BlockOrSingle
    //     select new CatchNode (new SourcePosition(position), variable, block);
    //
    // // блок finally
    // private static readonly Parser<char, IEnumerable<StatementNode>> Finally =
    //     from _ in Sok ("finally")
    //     from block in BlockOrSingle
    //     select block;
    //
    // // конструкция try-catch-finally
    // private static readonly Parser<char, StatementNode> TryCatchFinally =
    //     from position in Sok ("try").Then (CurrentPos)
    //     from tryBlock in BlockOrSingle
    //     from catchNode in Catch.Optional()
    //     from finallyBlock in Finally.Optional()
    //     select (StatementNode) new TryNode (new SourcePosition (position),
    //         tryBlock, catchNode.GetValueOrDefault(),
    //         finallyBlock.GetValueOrDefault());
    //
    // // возврат из функции
    // private static readonly Parser<char, StatementNode> Return = Map
    //     (
    //         (position, value) =>
    //             (StatementNode) new ReturnNode (new SourcePosition (position),
    //                 value.GetValueOrDefault()),
    //         Sok ("return").Then (CurrentPos),
    //         Tok (Expr).Optional()
    //     );
    //
    // // внешний код
    // private static readonly Parser<char, StatementNode> External = Map
    //     (
    //         (position, content, _) =>
    //             (StatementNode) new ExternalNode (new SourcePosition(position), content),
    //         Char ('`').Then (CurrentPos),
    //         AnyCharExcept ('`').ManyString(),
    //         Char ('`')
    //     );
    //
    // // определение функции
    // private static readonly Parser<char, StatementNode> FunctionDefinition =
    //     from position in CurrentPos
    //     from _1 in Sok ("func")
    //     from name in Tok (Identifier)
    //     from args in RoundBrackets (Identifier.Separated (Tok (',')).Optional())
    //     from body in CurlyBraces (Block)
    //     select (StatementNode) new DefinitionNode (new SourcePosition (position),
    //         name, args.GetValueOrDefault(), body);
    //
    // // блок using
    // private static readonly Parser<char, StatementNode> Using =
    //     from position in CurrentPos
    //     from _1 in Sok ("using")
    //     from open in Tok ('(')
    //     from variable in Tok (Identifier)
    //     from equal in Tok ('=')
    //     from initialization in Expr
    //     from close in Tok (')')
    //     from body in BlockOrSingle
    //     select (StatementNode) new UsingNode (new SourcePosition (position),
    //         variable, initialization, body);
    //
    // // with-присваивание
    // private static readonly Parser<char, StatementNode> WithAssignment =
    //     from position in CurrentPos
    //     from _1 in Tok ('.')
    //     from property in Tok (Identifier)
    //     from _2 in Tok ('=')
    //     from expression in Tok (Expr)
    //     select (StatementNode)new WithAssignmentNode (new SourcePosition (position),
    //         new VariableNode (property), expression);

    // // блок with
    // private static readonly Parser<char, StatementNode> With =
    //     from position in CurrentPos
    //     from _1 in Sok ("with")
    //     from center in RoundBrackets (Tok (Expr))
    //     from body in BlockOrSingle
    //     select (StatementNode)new WithNode (new SourcePosition (position), center, body);

    // // обобщенный стейтмент
    // internal static readonly Parser<char, StatementNode> Statement = OneOf
    //     (
    //         Try (Tok (If)),
    //         Try (Tok (Return)),
    //         Try (Tok (TryCatchFinally)),
    //         Try (Tok (ForEach)),
    //         Try (Tok (For)),
    //         Try (Tok (FunctionDefinition)),
    //         Try (Tok (While)),
    //         Try (Tok (Using)),
    //         Try (Tok (With)),
    //         Try (Tok (WithAssignment)),
    //         Try (Tok (External)),
    //         Try (Tok (ExpressionStatement))
    //     );

    // //
    // // Собственно программа
    // //
    //
    // private static readonly Parser<char, ProgramNode> WholeProgram =
    //     Block.Before (End)
    //         .Select (s => new ProgramNode (s));

    #endregion
}
