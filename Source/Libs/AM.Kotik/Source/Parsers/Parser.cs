// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Parser.cs -- базовы класс для парсеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AM.Collections;
using AM.Kotik.Parsers;
using AM.Results;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Базовый класс для парсеров.
/// </summary>
public abstract class Parser<TResult>
    where TResult: class
{
    #region Properties

    /// <summary>
    /// Метка для упрощения отладки.
    /// </summary>
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

    /// <summary>
    /// Создание псевдо-экземпляра.
    /// Этот вызов нужен, чтобы защитить статические экземпляры
    /// парсеров, используемые для разных нужд.
    /// В прочих случаях можно без проблем использовать метод.
    /// <see cref="Labeled"/>.
    /// </summary>
    public Parser<TResult> Instance
        (
            string label
        )
    {
        Sure.NotNullNorEmpty (label);

        return new ParserInstance<TResult> (this, label);
    }

    /// <summary>
    /// Пометка парсера для упрощения отладки.
    /// Предупреждение: не используйте больше одного раза для
    /// одного экземпляра парсера, т.к . результат предыдущего
    /// вызова будет потерян. Пометить один экземпляр разными
    /// метками можно с помощью вызова <see cref="Instance"/>.
    /// </summary>
    public Parser<TResult> Labeled
        (
            string label
        )
    {
        Sure.NotNull (label);

        Label = label;

        return this;
    }

    /// <summary>
    /// Подключение альтернативы.
    /// </summary>
    public Parser<TResult> Or
        (
            Parser<TResult> other
        )
    {
        Sure.NotNull (other);

        return new OneOfParser<TResult> (this, other);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public Parser<TResult> Or
        (
            Parser<TResult> other1,
            Parser<TResult> other2
        )
    {
        Sure.NotNull (other1);
        Sure.NotNull (other2);

        return new OneOfParser<TResult> (this, other1, other2);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public Parser<TResult> Or
        (
            Parser<TResult> other1,
            Parser<TResult> other2,
            Parser<TResult> other3
        )
    {
        Sure.NotNull (other1);
        Sure.NotNull (other2);
        Sure.NotNull (other3);

        return new OneOfParser<TResult> (this, other1, other2, other3);
    }

    /// <summary>
    /// Подключение альтернатив.
    /// </summary>
    public Parser<TResult> Or
        (
            params Parser<TResult>[] others
        )
    {
        Sure.AssertState (!others.IsNullOrEmpty());

        var list = new List<Parser<TResult>> (others);
        list.Insert (0, this);

        return new OneOfParser<TResult> (list.ToArray());
    }

    /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public Result<TResult> Parse
        (
            ParseState state
        )
    {
        Sure.NotNull (state);

        if (!TryParse (state, out var temporary))
        {
            return Result<TResult>.Failure();
        }

        return new Result<TResult> (temporary);
    }

    /// <summary>
    /// Разбор потока токенов с текущей позиции.
    /// </summary>
    public TResult ParseOrThrow
        (
            ParseState state
        )
    {
        Sure.NotNull (state);

        if (!TryParse (state, out var temporary))
        {
            throw new SyntaxException (state);
        }

        return new Result<TResult> (temporary).Value;
    }

    /// <summary>
    /// Запоминание успешного выполнения парсера
    /// в <see cref="ParseState"/> под указанным ключом.
    /// </summary>
    public Parser<TResult> Remember 
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        return new RememberParser<TResult> (key, this);
    }
    
    /// <summary>
    /// Парсинг последовательности однообразных токенов.
    /// </summary>
    public Parser<IList<TResult>> Repeated
        (
            int minCount = 0,
            int maxCount = int.MaxValue
        )
    {
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new RepeatParser<TResult> (this, minCount, maxCount);
    }

    /// <summary>
    /// Парсинг разделенных однообразных токенов.
    /// </summary>
    public Parser<IList<TResult>> SeparatedBy<TSeparator>
        (
            Parser<TSeparator> separator,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
        where TSeparator: class
    {
        Sure.NotNull (separator);
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new SeparatedParser<TResult, TSeparator, string>
            (
                itemParser: this,
                separator,
                delimiterParser: null,
                minCount,
                maxCount
            );
    }

    /// <summary>
    /// Парсинг разделенных однообразных токенов.
    /// </summary>
    public Parser<IList<TResult>> SeparatedBy<TSeparator, TDelimiter>
        (
            Parser<TSeparator> separator,
            Parser<TDelimiter> delimiter,
            int minCount = 0,
            int maxCount = int.MaxValue
        )
        where TSeparator: class
        where TDelimiter: class
    {
        Sure.NotNull (separator);
        Sure.NotNull (delimiter);
        Sure.NonNegative (minCount);
        Sure.NonNegative (maxCount);

        return new SeparatedParser<TResult, TSeparator, TDelimiter>
            (
                this,
                separator,
                delimiter,
                minCount,
                maxCount
            );
    }

    /// <summary>
    /// Разбор входного потока (попытка).
    /// Является правилом хорошего тона, чтобы парсер
    /// восстанавливал состояние <paramref name="state"/>,
    /// если возвращает <c>false</c>.
    /// </summary>
    public abstract bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
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
public static class Parser
{
    #region Public methods

    /// <summary>
    /// "Нужное после ненужного".
    /// </summary>
    public static Parser<TResult> After<TAfter, TResult>
        (
            this Parser<TResult> parser,
            Parser<TAfter> other
        )
        where TAfter: class
        where TResult: class
    {
        return new AfterParser<TAfter, TResult> (parser, other);
    }

    /// <summary>
    /// "Нужное перед ненужным".
    /// </summary>
    public static Parser<TResult> Before<TBefore, TResult>
        (
            this Parser<TResult> parser,
            Parser<TBefore> other
        )
        where TBefore: class
        where TResult: class
    {
        return new BeforeParser<TBefore, TResult> (parser, other);
    }

    /// <summary>
    /// "Нужное между ненужным".
    /// </summary>
    public static Parser<TResult> Between<TBefore, TResult, TAfter>
        (
            this Parser<TResult> parser,
            Parser<TBefore> before,
            Parser<TAfter> after
        )
        where TBefore: class
        where TResult: class
        where TAfter: class
    {
        return new BetweenParser<TBefore, TResult, TAfter> (before, parser, after);
    }

    /// <summary>
    /// Цепочка из двух парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Func<T1, T2, TResult> function
        )
        where T1: class
        where T2: class
        where TResult: class
    {
        return new ChainParser<T1, T2, TResult> (first, second, function);
    }

    /// <summary>
    /// Цепочка из трех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Func<T1, T2, T3, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, TResult> (first, second,
            third, function);
    }

    /// <summary>
    /// Цепочка из четырех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Func<T1, T2, T3, T4, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, TResult> (first, second,
            third, fourth, function);
    }

    /// <summary>
    /// Цепочка из пяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, TResult> (first, second,
            third, fourth, fifth, function);
    }

    /// <summary>
    /// Цепочка из шести парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, TResult> (first,
            second, third, fourth, fifth, sixth, function);
    }

    /// <summary>
    /// Цепочка из семи парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, TResult> (first,
            second, third, fourth, fifth, sixth, seventh, function);
    }

    /// <summary>
    /// Цепочка из восьми парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, TResult> (first,
            second, third, fourth, fifth, sixth, seventh, eight, function);
    }

    /// <summary>
    /// Цепочка из девяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Parser<T9> nineth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where T9: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> (first,
            second, third, fourth, fifth, sixth, seventh, eight, nineth, function);
    }

    /// <summary>
    /// Цепочка из десяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where T9: class
        where T10: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
            (first, second, third, fourth, fifth, sixth, seventh, eight, nineth,
                tenth, function);
    }

    /// <summary>
    /// Цепочка из одиннадцати парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Parser<T11> eleventh,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where T9: class
        where T10: class
        where T11: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
            (first, second, third, fourth, fifth, sixth, seventh, eight, nineth,
                tenth, eleventh, function);
    }

    /// <summary>
    /// Цепочка из двенадцати парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Parser<T11> eleventh,
            Parser<T12> twelveth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where T9: class
        where T10: class
        where T11: class
        where T12: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
            (first, second, third, fourth, fifth, sixth, seventh, eight, nineth,
                tenth, eleventh, twelveth, function);
    }

    /// <summary>
    /// Выражение в угловых скобках.
    /// </summary>
    public static Parser<TResult> CornerBrackets<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
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
            this Parser<TResult> parser
        )
        where TResult: class
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
            this Parser<TResult> parser
        )
        where TResult: class
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
    /// Парсер с отложенной инициализацией.
    /// </summary>
    public static Parser<TResult> Lazy<TResult>
        (
            Func<Parser<TResult>> function
        )
        where TResult: class
    {
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
            this Parser<TIntermediate> parser,
            Func<TIntermediate, TResult> function
        )
        where TIntermediate: class
        where TResult: class
    {
        return new MapParser<TIntermediate, TResult> (parser, function);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            Parser<TResult> first,
            Parser<TResult> second
        )
        where TResult: class
    {
        return new OneOfParser<TResult> (first, second);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            Parser<TResult> first,
            Parser<TResult> second,
            Parser<TResult> third
        )
        where TResult: class
    {
        return new OneOfParser<TResult> (first, second, third);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            Parser<TResult> first,
            Parser<TResult> second,
            Parser<TResult> third,
            Parser<TResult> fourth
        )
        where TResult: class
    {
        return new OneOfParser<TResult> (first, second, third, fourth);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            params Parser<TResult>[] parsers
        )
        where TResult : class
    {
        return new OneOfParser<TResult> (parsers);
    }

    /// <summary>
    /// Парсинг альтернатив.
    /// </summary>
    public static Parser<TResult> OneOf<TResult>
        (
            IList<Parser<TResult>> parsers
        )
        where TResult : class
    {
        return new OneOfParser<TResult> (parsers);
    }

    /// <summary>
    /// Опциональный парсер.
    /// </summary>
    public static Parser<TResult> Optional<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new OptionalParser<TResult> (parser);
    }

    /// <summary>
    /// Выдает текущую позицию в исходном тексте скрипта.
    /// </summary>
    public static readonly SourcePositionParser Position = new ();

    /// <summary>
    /// Зарезервированное слово.
    /// </summary>
    public static ReservedWordParser Reserved (string word) => new (word);

    /// <summary>
    /// Немедленный возврат значения.
    /// </summary>
    public static Parser<TResult> Return<TResult> (TResult result)
        where TResult: class
        => new ReturnParser<TResult> (result);

    /// <summary>
    /// Выражение в круглых скобках.
    /// </summary>
    public static Parser<TResult> RoundBrackets<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("("),
                parser,
                Term (")")
            );
    }

    /// <summary>
    /// Выражение в квадратных скобках.
    /// </summary>
    public static Parser<TResult> SquareBrackets<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new BetweenParser<string, TResult, string>
            (
                Term ("["),
                parser,
                Term ("]")
            );
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
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new TraceParser<TResult> (parser);
    }

    #endregion
}
