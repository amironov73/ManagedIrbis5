// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* Parser.cs -- базовый класс для парсеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AM.Collections;
using AM.Lexey.Tokenizing;
using AM.Results;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Parsing;

/// <summary>
/// Базовый класс для парсеров.
/// </summary>
[PublicAPI]
public abstract class Parser<TResult>
    : IParser<TResult>
{
    #region Properties

    /// <inheritdoc cref="IParser{TResult}.Label"/>
    [UsedImplicitly]
    public string? Label { get; set; }

    #endregion

    #region Protected members

    /// <summary>
    /// Отладочная зацепка, вызывается перед началом разбора
    /// текущего токена.
    /// </summary>
    protected void DebugHook
        (
            ParseState state
        )
    {
        // пустое тело метода, переопределите в потомке, если нужно
    }

    /// <summary>
    /// Отладочная печать текущей признака успешности выполнения парсинга.
    /// Вызывается после окончания разбора текущего токена.
    /// </summary>
    protected internal bool DebugSuccess
        (
            ParseState state,
            bool success
        )
    {
        return state.DebugSuccess (this, success);
    }

    #endregion

    #region Public methods

    /// <inheritdoc cref="IParser{TResult}.TryParse"/>
    public abstract bool TryParse
        (
            ParseState state,
            [MaybeNull] out TResult result
        );

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Label ?? GetType().Name;

    #endregion
}

/// <summary>
/// Полезные методы расширения.
/// </summary>
[PublicAPI]
public static class Parser
{
    #region Public methods

    /// <summary>
    /// "Нужное после ненужного".
    /// </summary>
    public static Parser<TResult> After<TAfter, TResult>
        (
            this IParser<TResult> parser,
            IParser<TAfter> other
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (other);

        return new AfterParser<TAfter, TResult> (parser, other);
    }

    /// <summary>
    /// Навешивает дополнительное условие на парсер.
    /// </summary>
    public static Parser<TResult> Assert<TResult>
        (
            this IParser<TResult> parser,
            Func<TResult, bool> predicate,
            [CallerArgumentExpression (nameof (predicate))] string? message = null
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (predicate);

        return new AssertParser<TResult> (parser, predicate, message!);
    }
    /// <summary>
    /// "Нужное перед ненужным".
    /// </summary>
    public static Parser<TResult> Before<TBefore, TResult>
        (
            this IParser<TResult> parser,
            IParser<TBefore> other
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (other);

        return new BeforeParser<TBefore, TResult> (parser, other);
    }

    /// <summary>
    /// "Нужное между ненужным".
    /// </summary>
    public static Parser<TResult> Between<TBefore, TResult, TAfter>
        (
            this IParser<TResult> parser,
            IParser<TBefore> before,
            IParser<TAfter> after
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (before);
        Sure.NotNull (after);

        return new BetweenParser<TBefore, TResult, TAfter> (before, parser, after);
    }

    /// <summary>
    /// Цепочка из двух парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            Func<T1, T2, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, TResult> (first, second, function);
    }

    /// <summary>
    /// Цепочка из трех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            Func<T1, T2, T3, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, TResult>
            (
                first,
                second,
                third,
                function
            );
    }

    /// <summary>
    /// Цепочка из четырех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            Func<T1, T2, T3, T4, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, TResult>
            (
                first,
                second,
                third,
                fourth,
                function
            );
    }

    /// <summary>
    /// Цепочка из пяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                function
            );
    }

    /// <summary>
    /// Цепочка из шести парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                function
            );
    }

    /// <summary>
    /// Цепочка из семи парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                function
            );
    }

    /// <summary>
    /// Цепочка из восьми парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eight,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eight);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eight,
                function
            );
    }

    /// <summary>
    /// Цепочка из девяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eight,
            IParser<T9> nineth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eight);
        Sure.NotNull (nineth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eight,
                nineth,
                function
            );
    }

    /// <summary>
    /// Цепочка из десяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eight,
            IParser<T9> nineth,
            IParser<T10> tenth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eight);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eight,
                nineth,
                tenth,
                function
            );
    }

    /// <summary>
    /// Цепочка из одиннадцати парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eight,
            IParser<T9> nineth,
            IParser<T10> tenth,
            IParser<T11> eleventh,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eight);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (eleventh);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eight,
                nineth,
                tenth,
                eleventh,
                function
            );
    }

    /// <summary>
    /// Цепочка из двенадцати парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eight,
            IParser<T9> nineth,
            IParser<T10> tenth,
            IParser<T11> eleventh,
            IParser<T12> twelveth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eight);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (eleventh);
        Sure.NotNull (twelveth);
        Sure.NotNull (function);

        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
            (
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eight,
                nineth,
                tenth,
                eleventh,
                twelveth,
                function
            );
    }

    /// <summary>
    /// Выражение в угловых скобках.
    /// </summary>
    public static Parser<TResult> CornerBrackets<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("<"),
                parser,
                Term (">")
            );
    }

    /// <summary>
    /// Выражение в фигурных скобках.
    /// </summary>
    public static Parser<TResult> CurlyBrackets<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("{"),
                parser,
                Term ("}")
            );
    }

    /// <summary>
    /// Проверка успешного окончания разбора.
    /// </summary>
    public static Parser<TResult> End<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new BeforeParser<Unit, TResult> (parser, new EndParser());
    }

    /// <summary>
    /// Индикация сбоя.
    /// </summary>
    public static Parser<Unit> Fail (string message) => new FailParser<Unit> (message);

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Создание псевдо-экземпляра.
    /// Этот вызов нужен, чтобы защитить статические экземпляры
    /// парсеров, используемые для разных нужд.
    /// В прочих случаях можно без проблем использовать метод.
    /// <see cref="Labeled{TResult}"/>.
    /// </summary>
    public static Parser<TResult> Instance<TResult>
        (
            this IParser<TResult> parser,
            string label
        )
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);

        return new LabeledParserInstance<TResult> (parser, label);
    }

    /// <summary>
    /// Пометка парсера для упрощения отладки.
    /// Предупреждение: не используйте больше одного раза для
    /// одного экземпляра парсера, т.к . результат предыдущего
    /// вызова будет потерян. Пометить один экземпляр разными
    /// метками можно с помощью вызова <see cref="Instance{TResult}"/>.
    /// </summary>
    public static IParser<TResult> Labeled<TResult>
        (
            this IParser<TResult> parser,
            string label
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (label);

        parser.Label = label;

        return parser;
    }

    /// <summary>
    /// Парсер с отложенной инициализацией.
    /// </summary>
    public static Parser<TResult> Lazy<TResult>
        (
            Func<IParser<TResult>> function
        )
    {
        Sure.NotNull (function);

        return new LazyParser<TResult> (function);
    }

    /// <summary>
    /// Литерал.
    /// </summary>
    public static readonly LiteralParser Literal = new ();

    /// <summary>
    /// Преобразование разобранного значения.
    /// </summary>
    public static Parser<TResult> Map<TIntermediate, TResult>
        (
            this IParser<TIntermediate> parser,
            Func<TIntermediate, TResult> function
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (function);

        return new MapParser<TIntermediate, TResult> (parser, function);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            IParser<TResult> first,
            IParser<TResult> second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        return new OneOfParser<TResult> (first, second);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            IParser<TResult> first,
            IParser<TResult> second,
            IParser<TResult> third
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);

        return new OneOfParser<TResult>
            (
                first,
                second,
                third
            );
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            IParser<TResult> first,
            IParser<TResult> second,
            IParser<TResult> third,
            IParser<TResult> fourth
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);

        return new OneOfParser<TResult>
            (
                first,
                second,
                third,
                fourth
            );
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            params IParser<TResult>[] parsers
        )
    {
        Sure.NotNull (parsers);

        return new OneOfParser<TResult> (parsers);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            IList<IParser<TResult>> parsers
        )
    {
        Sure.NotNull (parsers);

        return new OneOfParser<TResult> (parsers);
    }

    /// <summary>
    /// Опциональный парсер.
    /// </summary>
    public static Parser<TResult> Optional<TResult>
        (
            this IParser<TResult> parser
        )
    {
        Sure.NotNull (parser);

        return new OptionalParser<TResult> (parser);
    }

    /// <summary>
    /// Выражение в опциональных круглых скобках.
    /// </summary>
    public static Parser<TResult> OptionalRoundBrackets<TResult>
        (
            this IParser<TResult> parser
        )
    {
        Sure.NotNull (parser);

        return new OptionalBetweenParser<string, TResult, string>
            (
                Term ("("),
                parser,
                Term (")")
            );
    }

    /// <summary>
    /// Подключение альтернативы.
    /// </summary>
    public static Parser<TResult> Or<TResult>
        (
            this IParser<TResult> parser,
            IParser<TResult> other
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (other);

        return new OneOfParser<TResult> (parser, other);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public static Parser<TResult> Or<TResult>
        (
            this IParser<TResult> parser,
            IParser<TResult> other1,
            IParser<TResult> other2
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (other1);
        Sure.NotNull (other2);

        return new OneOfParser<TResult> (parser, other1, other2);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public static Parser<TResult> Or<TResult>
        (
            this IParser<TResult> parser,
            IParser<TResult> other1,
            IParser<TResult> other2,
            IParser<TResult> other3
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (other1);
        Sure.NotNull (other2);
        Sure.NotNull (other3);

        return new OneOfParser<TResult> (parser, other1, other2, other3);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public static Parser<TResult> Or<TResult>
        (
            this IParser<TResult> parser,
            params IParser<TResult>[] others
        )
    {
        Sure.NotNull (parser);
        Sure.AssertState (!others.IsNullOrEmpty());

        var list = new List<IParser<TResult>> (others);
        list.Insert (0, parser);

        return new OneOfParser<TResult> (list.ToArray());
    }

        /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public static Result<TResult> Parse<TResult>
        (
            this IParser<TResult> parser,
            ParseState state
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (state);

        if (!parser.TryParse (state, out var temporary))
        {
            return Result<TResult>.Failure;
        }

        return new Result<TResult> (temporary!);
    }

    /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public static Result<TResult> Parse<TResult>
        (
            this IParser<TResult> parser,
            string source,
            Tokenizer tokenizer
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (source);
        Sure.NotNull (tokenizer);

        var tokens = tokenizer.Parse (source);
        var state = new ParseState (tokens);

        return parser.Parse (state);
    }

    /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public static TResult ParseOrThrow<TResult>
        (
            this IParser<TResult> parser,
            ParseState state
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (state);

        if (!parser.TryParse (state, out var temporary))
        {
            throw new SyntaxException (state);
        }

        return temporary!;
    }

    /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public static TResult ParseOrThrow<TResult>
        (
            this IParser<TResult> parser,
            string source,
            Tokenizer tokenizer
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (source);
        Sure.NotNull (tokenizer);

        var tokens = tokenizer.Parse (source);
        var state = new ParseState (tokens);

        return parser.ParseOrThrow (state);
    }

    /// <summary>
    /// Выдает текущую позицию в исходном тексте скрипта.
    /// </summary>
    public static readonly SourcePositionParser Position = new ();

    /// <summary>
    /// Запоминание успешного выполнения парсера
    /// в <see cref="ParseState"/> под указанным ключом.
    /// </summary>
    public static Parser<TResult> Remember<TResult>
        (
            this IParser<TResult> parser,
            string key
        )
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (key);

        return new RememberParser<TResult> (key, parser);
    }

    /// <summary>
    /// Парсинг последовательности однообразных токенов.
    /// </summary>
    public static Parser<IList<TResult>> Repeated<TResult>
        (
            this IParser<TResult> parser,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
    {
        Sure.NotNull (parser);
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new RepeatParser<TResult> (parser, minCount, maxCount);
    }

    /// <summary>
    /// Зарезервированное слово.
    /// </summary>
    public static ReservedWordParser Reserved (string word) => new (word);

    /// <summary>
    /// Немедленный возврат значения.
    /// </summary>
    public static Parser<TResult> Return<TResult> (TResult result)
        => new ReturnParser<TResult> (result);

    /// <summary>
    /// Выражение в круглых скобках.
    /// </summary>
    public static Parser<TResult> RoundBrackets<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("("),
                parser,
                Term (")")
            );
    }

    /// <summary>
    /// Парсинг разделенных однообразных токенов.
    /// </summary>
    public static Parser<IList<TResult>> SeparatedBy<TResult, TSeparator>
        (
            this IParser<TResult> parser,
            IParser<TSeparator> separator,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (separator);
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new SeparatedParser<TResult, TSeparator, string>
            (
                parser,
                separator,
                delimiterParser: null,
                minCount,
                maxCount
            );
    }

    /// <summary>
    /// Парсинг разделенных однообразных токенов.
    /// </summary>
    public static Parser<IList<TResult>> SeparatedBy<TResult, TSeparator, TDelimiter>
        (
            this IParser<TResult> parser,
            IParser<TSeparator> separator,
            IParser<TDelimiter> delimiter,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
    {
        Sure.NotNull (parser);
        Sure.NotNull (separator);
        Sure.NotNull (delimiter);
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new SeparatedParser<TResult, TSeparator, TDelimiter>
            (
                parser,
                separator,
                delimiter,
                minCount,
                maxCount
            );
    }

    /// <summary>
    /// Выражение в квадратных скобках.
    /// </summary>
    public static Parser<TResult> SquareBrackets<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("["),
                parser,
                Term ("]")
            );
    }

    /// <summary>
    /// Последовательность из двух парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2>
        (
            IParser<T1> first,
            IParser<T2> second
        )
    {
        return new ChainParser<T1, T2, Unit> (first, second, (_, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из трех парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third
        )
    {
        return new ChainParser<T1, T2, T3, Unit> (first, second,
            third, (_, _, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из четырех парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3, T4>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth
        )
    {
        return new ChainParser<T1, T2, T3, T4, Unit> (first, second,
            third, fourth, (_, _, _, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из пяти парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3, T4, T5>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth
        )
    {
        return new ChainParser<T1, T2, T3, T4, T5, Unit> (first, second,
            third, fourth, fifth, (_, _, _, _, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из шести парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3, T4, T5, T6>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth
        )
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, Unit> (first, second,
            third, fourth, fifth, sixth, (_, _, _, _, _, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из семи парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3, T4, T5, T6, T7>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh
        )
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, Unit> (first, second,
            third, fourth, fifth, sixth, seventh,
            (_, _, _, _, _, _, _) => Unit.Value);
    }

    /// <summary>
    /// Последовательность из восьми парсеров.
    /// </summary>
    public static Parser<Unit> Sequence<T1, T2, T3, T4, T5, T6, T7, T8>
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth
        )
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, Unit> (first, second,
            third, fourth, fifth, sixth, seventh, eighth,
            (_, _, _, _, _, _, _, _) => Unit.Value);
    }

    /// <summary>
    /// Заставляет парсер запоминать результат.
    /// </summary>
    public static ValueParser<TResult> StoreValue<TResult>
        (
            this IParser<TResult> parser
        )
    {
        Sure.NotNull (parser);

        return new (parser);
    }

    /// <summary>
    /// Терм.
    /// </summary>
    public static TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Трассировка парсера.
    /// </summary>
    public static TraceParser<TResult> Trace<TResult>
        (
            this IParser<TResult> parser
        )
    {
        return new TraceParser<TResult> (parser);
    }

    #endregion
}
