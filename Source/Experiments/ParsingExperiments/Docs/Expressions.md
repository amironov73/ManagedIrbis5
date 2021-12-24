## Построитель выражений

Pidgin предлагает не конструировать разбор математических выражений вручную, а доверить это дело специальному классу `ExpressionParser`

```c#
public static class ExpressionParser
{
    public static Parser<TToken, T> Build<TToken, T>
        (
            Parser<TToken, T> term,
            IEnumerable<OperatorTableRow<TToken, T>> operatorTable
        );

    public static Parser<TToken, T> Build<TToken, T>
        (
            Parser<TToken, T> term,
            IEnumerable<IEnumerable<OperatorTableRow<TToken, T>>> operatorTable
        );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Func<Parser<TToken, T>, Parser<TToken, T>> termFactory,
                IEnumerable<OperatorTableRow<TToken, T>> operatorTable
            );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Func<Parser<TToken, T>, Parser<TToken, T>> termFactory,
                IEnumerable<IEnumerable<OperatorTableRow<TToken, T>>> operatorTable
            );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Parser<TToken, T> term,
                Func<Parser<TToken, T>, IEnumerable<OperatorTableRow<TToken, T>>> operatorTableFactory
            );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Parser<TToken, T> term,
                Func<Parser<TToken, T>, IEnumerable<IEnumerable<OperatorTableRow<TToken, T>>>> operatorTableFactory
            );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Func<Parser<TToken, T>, (Parser<TToken, T> term,
                IEnumerable<OperatorTableRow<TToken, T>> operatorTable)> termAndOperatorTableFactory
            );

        public static Parser<TToken, T> Build<TToken, T>
            (
                Func<Parser<TToken, T>, (Parser<TToken, T> term,
                IEnumerable<IEnumerable<OperatorTableRow<TToken, T>>> operatorTable)> termAndOperatorTableFactory
            );
}
```

Важнейшим свойством `ExpressionParser` является поддержка приоритета операций. Каждый уровень приоритета описывается соответствующим экземпляром класса `OperatorTableRow`, содержащим все возможные операции, применимые на данном уровне.

```c#
public sealed class OperatorTableRow<TToken, T>
{
    // неассоциативные инфиксные операции
    public IEnumerable<Parser<TToken, Func<T, T, T>>> InfixNOps { get; }

    // лево-ассоциативные инфиксные операции (например, сложение и умножение)
    public IEnumerable<Parser<TToken, Func<T, T, T>>> InfixLOps { get; }

    // право-ассоциативные инфиксные операции (например, возведение в степень)
    public IEnumerable<Parser<TToken, Func<T, T, T>>> InfixROps { get; }

    // префиксные операции (например, ++)
    public IEnumerable<Parser<TToken, Func<T, T>>> PrefixOps { get; }

    // суффиксные операции (например, ++)
    public IEnumerable<Parser<TToken, Func<T, T>>> PostfixOps { get; }

    public OperatorTableRow
        (
            IEnumerable<Parser<TToken, Func<T, T, T>>>? infixNOps,
            IEnumerable<Parser<TToken, Func<T, T, T>>>? infixLOps,
            IEnumerable<Parser<TToken, Func<T, T, T>>>? infixROps,
            IEnumerable<Parser<TToken, Func<T, T>>>? prefixOps,
            IEnumerable<Parser<TToken, Func<T, T>>>? postfixOps
        );

    public static OperatorTableRow<TToken, T> Empty { get; }
        = new OperatorTableRow<TToken, T> (null, null, null, null, null);

    public OperatorTableRow<TToken, T> And (OperatorTableRow<TToken, T> otherRow)
        => new OperatorTableRow<TToken, T>
        (
            InfixNOps.Concat(otherRow.InfixNOps),
            InfixLOps.Concat(otherRow.InfixLOps),
            InfixROps.Concat(otherRow.InfixROps),
            PrefixOps.Concat(otherRow.PrefixOps),
            PostfixOps.Concat(otherRow.PostfixOps)
        );
}
```

Операторы в C# в порядке убывания приоритета (внутри одной строки операторы имеют одинаковый приоритет):

| Операторы                                                                                                           | Категория или имя          |
|---------------------------------------------------------------------------------------------------------------------|----------------------------|
| x.y, f(x), a\[i\], x++, x--, new, typeof, <br/>checked, unchecked, default, nameof, sizeof, stackalloc, x->y        | Primary                    |
| +x, -x, !x, ~x, ++x, --x, ^x, (T) x, await, &x, *x, true and false                                                  | Unary                      |
| x..y                                                                                                                | Range                      |
| switch, with                                                                                                        | switch and with            |
| x * y, x / y, x % y                                                                                                 | Multiplicative             |
| x + y, x - y                                                                                                        | Additive                   |
| x \<\< y, x \>\> y                                                                                                  | Shift                      |
| x \< y, x \> y, x \<= y, x \>= y, is, as                                                                            | Relational ad type-testing |
| x == y, x != y                                                                                                      | Equality                   |
| x &amp; y                                                                                                           | Bitwise AND                |
| x ^ y                                                                                                               | Bitwise XOR                |
| x &#166;                                                                                                            | Bitwise OR                 |
| x &amp;&amp; y                                                                                                      | Logical AND                |
| x  &#166; &#166; y                                                                                                  | Logical OR                 |
| x ?? y                                                                                                              | Null-coalescing            |
| c ? t : f                                                                                                           | Conditional                |
| x = y; x += y; x -= y; x *= y; x /= y; x %= y; <br/>x &amp;= y; x &#166;= y; x ^= y; x \<\<= y; x \>\>= y; x ??= y; | Assignment                 |
