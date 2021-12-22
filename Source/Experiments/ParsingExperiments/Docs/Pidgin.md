# Pidgin

Pidgin - это опенсорсная библиотека и одноименный комбинаторный парсер.

GitHub: https://github.com/benjamin-hodgson/Pidgin (570 звезд)

Документация: https://www.benjamin.pizza/Pidgin

Комбинаторный парсер (синтаксический анализатор) -- метод синтаксического разбора "сверху вниз", основанный на комбинировании простейших парсеров для получения все более высокоуровневых, и в конечном итоге -- разбора некоторой грамматики (применение функционального подхода).

Для платформы .NET разработано множество комбинаторных парсеров: Sprache, Superpower, Parlot и др. Pidgin -- яркий представитель этого семейства, ориентированный на достижение высокой производительности за счет применения современных средств платформы .NET (например, Span<T>).

Pidgin -- легкий декларативный инструмент высокого уровня. Синтаксические анализаторы, написанные с помощью комбинаторов синтаксического анализа, выглядят как высокоуровневая спецификация грамматики языка, но они выражены средствами C# и не требуют специальных инструментов для создания исполняемого кода. Комбинаторы синтаксического анализатора более мощные, чем регулярные выражения - они могут анализировать более широкий класс языков, но более просты и удобны в использовании, чем генераторы синтаксического анализатора, такие как ANTLR.

Pidgin создан [Бенджамином Ходгсоном](https://www.benjamin.pizza/) (Benjamin Hodgson), сотрудником StackOverflow. Текущая версия 3.0, именно она описана в настоящем руководстве.

## Важное замечание

Pidgin не поддерживает многопоточность, т. е. общий экземпляр парсера нельзя использовать одновременно из разных потоков.

## Базовый класс Parser

Базовый класс, вокруг которого построен весь Pidgin -- Parser<TToken, TResult>:

```c#
public abstract partial class Parser<TToken, TResult>
{
    public abstract bool TryParse
        (
            ref ParseState<TToken> state,
            ref PooledList<Expected<TToken>> expecteds,
            [MaybeNullWhen(false)] out TResult result
        );
}
```

Все парсеры в Pidgin устроены по одному принципу: они принимают входной поток каких-то элементов (чаще всего текстовых символов) и либо "проглатывают" их, превращая в некий объект, либо отказываются "глотать" входные символы (в этом случае они возвращают только признак ошибки).  Тип возвращаемого значения может быть любым. Например, если парсер настроен на распознание во входном потоке целых чисел, он может возвращать результат типа `System.Int32`.

Типы `Result` и `ParseError`:

```c#
public class Result<TToken, T>
{
    public bool Success { get; }
    public T Value { get; }
    public ParseError<TToken>? Error { get; }
    public T GetValueOrDefault();
    public U Match<U> (Func<T, U> success, Func<ParseError<TToken>, U> failure);
    public Result<TToken, U> Select<U> (Func<T, U> selector);
}

public class ParseError<TToken> : IEquatable<ParseError<TToken>>
{
    public bool EOF { get; }
    public Maybe<TToken> Unexpected { get; }
    public IEnumerable<Expected<TToken>> Expected { get; }
    public SourcePosDelta ErrorPosDelta { get; }
    public SourcePos ErrorPos { get; }
    public string? Message { get; }
}
```

## Начало. Простейшие парсеры

Чтобы начать, достаточно подключить к своему проекту NuGet-пакет Pidgin и написать

```c#
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
```

Последняя строчка нужна, если мы планируем создавать парсер, разбирающий текст (в большинстве случаев так и есть). Pidgin позволяет строить парсеры для любого типа данных, например, для потока байтов.

Простейший парсер, предоставляемый библиотекой - `Any`: он принимает один любой Unicode-символ.

```c#
var result = Any.Parse ("a");
Console.WriteLine (result.Success); // True

result = Any.Parse ("");
Console.WriteLine (result.Success); // False
```

Обратите внимание, что изо всей поданной на его вход строки парсер "проглатывает" только то, что ему подходит. В вышеприведенном случае -- ровно один символ. Если бы входная строка содержала два символа, то второй остался бы нетронутым, и его можно было подать на вход следующему парсеру. Если входная строка не нравится парсеру, он не "проглатывает" ни одного символа и выставляет результат `False`.

Парсер `Any` "всеядный". Вот пример более разборчивого парсера: `Char`: он принимает только один указанный символ, все остальные отвергает.

```c#
var result = Char ('a').Parse ("a");
Console.WriteLine (result.Success); // True

result = Char ('c').Parse ("a");
Console.WriteLine (result.Success); // False
```

Чуть менее разборчивые парсеры `Digit` (принимает только символы Unicode, входящие в категорию "цифры", в частности, `0`, `1`, `2`, ... `9`) и `Letter` (категория "буквы", в том числе латинские и кириллические).

```c#
var result = Letter.Parse ("Ф");
Console.WriteLine (result.Success); // True

result = Letter.Parse ("?");
Console.WriteLine (result.Success); // False
```

К примитивным мы также отнесем парсер `String`, принимающий строго заданную строку, и ничего кроме нее.

Подытожим:

* **Any** -- "всеядный", принимает ровно один любой Unicode-символ (включая служебные, например, `\t`);
* **AnyCharExcept (params char[] chars)** -- принимает ровно один любой Unicode-символ, кроме перечисленных;
* **AnyCharExcept (IEnumerable<char> chars)** -- принимает ровно один любой Unicode-символ, кроме перечисленных;
* **Char (char c)** -- принимает ровно один заданный Unicode-символ;
* **CIChar (char c)** -- принимает ровно один заданный Unicode-символ без учета регистра;
* **CIString (string str)** -- принимает последовательность Unicode-символов, совпадающую с заданной строкой с точностью до регистра;
* **Digit** -- ровно один символ, соответствующий Unicode-категории `DecimalDigitNumber` (проще говоря, от `0` до `9` включительно);
* **DecimalNum** -- целое десятеричное число, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется;
* **End** -- конец входного потока символов;
* **EndOfLine** -- конец строки;
* **HexNum** -- целое шестнадцатеричное число, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется, без префикса или суффикса;
* **Int (int @base)** -- целое число в указанной системе счисления, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется, без префикса или суффикса;
* **Letter** -- ровно один символ, соответствующий Unicode-категории `UppercaseLetter`, `LowercaseLetter`, `TitlecaseLetter`, `ModifierLetter` и `OtherLetter`;
* **LetterOrDigit** -- сочетание двух предыдущих парсеров;
* **Long (int @base)** -- целое число в указанной системе счисления, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется, без префикса или суффикса;
* **LongNum** -- целое десятеричное число, возможно, со знаком (возвращает `System.Int64`), переполнение не проверяется;
* **Lowercase** -- ровно один символ Unicode-категории `LowercaseLetter`;
* **Num** -- целое десятеричное число, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется, синоним для `DecimalNum`;
* **OctalNum** -- целое восьмеричное число, возможно, со знаком (возвращает `System.Int32`), переполнение не проверяется, без префикса или суффикса;
* **Punctuation** -- ровно один символ Unicode-категории `ConnectorPunctuation`, `DashPunctuation`, `OpenPunctuation`, `ClosePunctuation`, `InitialQuotePunctuation`, `FinalQuotePunctuation` или `OtherPunctuation`;
* **Real** -- число с плавающей точкой, возможно, со знаком (возвращает `System.Double`), переполнение не проверяется, без префикса или суффикса, `InvariantCulture`;
* **Separator** -- ровно один символ Unicode-категории `SpaceSeparator`, `LineSeparator` или `ParagraphSeparator` (обратите внимание, что символы `\u000A` `(LF)`, `\u000C` `(FF)` и `\u000D` `(CR)` согласно стандарту Unicode являются не разделителями, а управляющими символами (категория `Control`));
* **SkipWhitespaces** -- последовательность пропускаемых пробельных символов (см. `Whitespace`);
* **String (string str)** -- принимает последовательность Unicode-символов, совпадающую с заданной строкой с учетом строки;
* **Symbol** -- ровно один символ Unicode-категории `MathSymbol`, `CurrencySymbol`, `ModifierSymbol` или `OtherSymbol`;
* **Token<TToken> (TToken token)** -- ровно один токен (универсальный метод, подходящий для произвольного типа, а не только для `char`);
* **Token<TToken> (Func<TToken, bool> predicate)** -- ровно один токен, удовлетворяющий заданному предикату;
* **UnsignedInt (int @base)** -- целое число в указанной системе счисления, без знака (возвращает `System.UInt32`), переполнение не проверяется, без префикса или суффикса;
* **UnsignedLong (int @base)** -- целое число в указанной системе счисления, без знака (возвращает `System.UInt64`), переполнение не проверяется, без префикса или суффикса;
* **Uppercase** -- ровно один символ Unicode-категории `UppercaseLetter`;
* **Whitespace** -- ровно один пробельный символ, т. е. символ из Unicode-категорий `SpaceSeparator`, `LineSeparator`, `ParagraphSeparator`, либо один из символов: `\u0009`, `\u000A`, `\u000B`, `\u000C`, `\u000D`, `\u0085`;
* **Whitespaces** -- последовательность пробельных символов (см. выше);
* **WhitespaceString** -- последовательность пробельных символов в виде строки;

## Перебор альтернатив

Простейший способ задать альтернативные парсеры -- операция `Or`:

```c#
var parser = Char ('a').Or (Char ('b'));
var result = parser.Parse ("a");
Console.WriteLine (result.Success); // True
```

В результате создается составной (комбинированный) парсер, который сначала пытается применить первый из составляющих его парсеров, и, если это удалось, возвращает его результат, либо пытается применить второй, третий и т. д. -- смотря сколько парсеров было объединено операцией `Or`.

Вместо выписывания длинной цепочки `Char ('a').Or. (Char ('b').Or (Char ('c')))` можно написать просто `OneOf ('a', 'b', 'c')` -- так гораздо нагляднее.

Необходимо учитывать одну особенность: по умолчанию Pidgin не выполняет "отката назад", если парсер начал "глотать" символы, но в процессе "подавился" (так сделано из соображений производительности). Лучше всего это продемонстрировать на примере

```c#
var first = String ("привет");
var second = String ("пример");
var combined = first.Or (second);
var result = combined.Parse ("пример");
Console.WriteLine (result.Success); // False
```

Что здесь произошло? Первый парсер успешно "проглотил" три символа (`при`) и "подавился", т. к. дальше началось разночтение, и вернул ошибку. В Pidgin операции `Or` и `OneOf` переходят к следующей альтернативе, только если парсер вернул ошибку, **не "проглотив" ни одного символа**. Иначе они сразу возвращают отрицательный результат.

Чтобы избежать подобного, нужно включить "откат":

```c#
var first = String ("привет");
var second = String ("пример");
var combined = Try (first).Or (second);
var result = combined.Parse ("пример");
Console.WriteLine (result.Success); // True
```

Операция `Try` возвращает входной поток в исходное состояние, если переданный ей парсер вернул ошибку.

Подытожим:

* **CIOneOf (params char[] chars)** -- принимается ровно один из перечисленных Unicode-символов без учета регистра;
* **CIOneOf (IEnumerable<char> chars)** -- принимается ровно один из заданных Unicode-символов без учета регистра;
* **Or (Parser<TToken, TResult> parser)** -- задание альтернативного парсера;
* **OneOf (params char[] chars)** -- принимается ровно один из перечисленных Unicode-символов с учетом регистра;
* **OneOf (params IEnumerable<char> chars)** -- принимается ровно один из заданных Unicode-символов с учетом регистра;
* **OneOf (params Parser<TToken, TResult>[] parsers)** -- задание нескольких альтернативных парсеров;
* **OneOf (params IEnumerable<Parser<TToken, TResult>> parsers)** -- задание нескольких альтернативных парсеров;
* **Sequence (params TToken[] tokens)** -- парсер, который ожидает заданную последовательность токенов;
* **Sequence (IEnumerable<TToken> tokens)** -- парсер, который ожидает заданную последовательность токенов;
* **Sequence (params Parser<TToken, T>[] parsers)** -- парсер, составленный из заданной последовательности парсеров;
* **Sequence (IEnumerable<Parser<TToken, T>> parsers)** -- парсер, составленный из заданной последовательности парсеров.

## Повторяемость

Другой способ комбинирования -- задание повторяемости парсера. Самый популярная операция `Many`, означающая "произвольное число повторений -- он нуля до бесконечности".

```c#
var parser = Char ('a').Many(); // произвольное количество букв 'a' подряд
var result = parser.Parse ("aaaabbb");
Console.WriteLine (result.Success); // True
Console.WriteLine (result.Value); // aaaa
```

Обратите внимание, что в вышеприведенном примере парсер "съел" только четыре первых символа от строки, остальные ему "не понравились" и он оставил их для другого парсера, который, возможно, Вы захотите применить.

Существует вариант операции `Many`, названный `ManyString`, который отличается тем, что все найденные символы "склеиваются" в одну строку, что довольно удобно.

Конечно же, операции комбинирования можно "нанизывать" друг на друга.

```c#
var parser = OneOf ('0', '1').ManyString();
var r = parser.Parse ("1001+2");
Console.WriteLine (r.Success); // True
Console.WriteLine (r.Value); // 1001
```

Вторая по популярности операция повторения `AtLeastOnce`, означающая "не меньше одного раза".

Подытожим:

* **AtLeastOnce()** -- не менее одного повторения;
* **AtLeastOnceString()** -- не менее одного повторения, возвращает склеенную строку;
* **Many()** -- произвольное число повторений;
* **ManyString()** -- произвольное число повторений, возвращает склеенную строку;
* **Optional()** -- не более одного повторения, возвращает `Maybe<T>` (см. ниже);
* **Repeat (int count)** -- точное число повторений;
* **RepeatString (int count)** -- точное число повторений, возвращает склеенную строку;
* **SkipAtLeastOnce()** -- не менее одного повторения, результат отбрасывается;ы
* **SkipMany()** -- произвольное число повторений, результат отбрасывается.

Структура `Maybe<T>` служит для возврата положительного или отрицательного результата:

```c#
public readonly struct Maybe<T> : IEquatable<Maybe<T>>
{
    public bool HasValue { get; }
    public T Value { get; }

    public T GetValueOrDefault();
    public T GetValueOrDefault (T defaultValue);
    public T GetValueOrDefault (Funct<T> defaultValue);
    public U Match<U> (Func<T, U> just, Func<U> nothing);
    public Maybe<U> Select<U> (Func<T, U> selector);
}
```

## Последовательность парсеров

Чаще всего разбор текстового файла выглядит примерно так: "если встретился символ `A`, ищи символ `B`". Вот как это реализуется с помощью Pidgin:

```c#
var first = String ("aaa");
var second = String ("bbb");
var sequenced = first.Then (second);
var result = sequenced.Parse ("aaabbb");
Console.WriteLine (result.Success); // True
Console.WriteLine (result.Value); // bbb
```

Обратите внимание -- по умолчанию берется только последняя часть распарсенной последовательности! Чтобы "получить всё", нужно написать

```c#
var sequenced = first.Then (second, (a, b) => a + b);
```

Существует парная операция -- `Before`, она выдает результат первого парсера:

```c#
var first = String ("aaa");
var second = String ("bbb");
var sequenced = first.Before (second);
var result = sequenced.Parse ("aaabbb");
Console.WriteLine (result.Success); // True
Console.WriteLine (result.Value); // aaa
```

Наконец, Pidgin предоставляет мощный инструмент объединения нескольких парсеров -- операцию `Map`

```c#
var first = String ("aaa");
var second = String ("bbb");
var third = String ("ccc");
var sequenced = Map ((a, b, c) => a + b + c, first, second, third);
var result = sequenced.Parse ("aaabbbccc");
Console.WriteLine (result.Success);
Console.WriteLine (result.Value);
```

Подытожим:

* **AtLeastOnceUntil<TU> (Parser<TToken, TU> terminator)** -- повторно выполняет первый парсер до тех пор, пока не сработает второй, результирующая последовательность должна содержать не менее одного элемента;
* **Before** - при успехе обоих парсеров выдает результат первого;
* **Between** -- при успехе трех парсеров выдает результат среднего. Отлично подходит для выражений в скобках;
* **Lookahead** -- при успехе второго парсера откатывает его результат (как будто бы он не "проглатывал"), но возвращает успех, при неудаче отката не происходит;
* **Map** -- обработка результата, выданного несколькими успешно сработавшими парсерами. Все остальные операции (`Before`, `Between` и `Then` можно выразить через `Map`);
* **Separated<TU> (Parser<TToken, TU> separator)** -- возвращает список `IEnumerable<T>`, значения типа `T` перемежаются значениями типа `U` (проще говоря список через запятую), возвращает от нуля до бесконечности значений;
* **SeparatedAtLeastOnce<TU> (Parser<TToken, TU> separator)** -- аналог предыдущего случая, однако, возвращает не менее одного элемента в списке;
* **SeparatedAndTerminated<TU> (Parser<TToken, TU> separator)** -- аналог предыдущего случая, однако, список может быть пустым, но должен также завершаться значением типа `U`;
* **SeparatedAndTerminatedAtLestOnce<TU> (Parser<TToken, TU> separator)** -- аналог предыдущего случая, однако, возвращает не менее одного элемента;
* **SeparatedAndOptionallyTerminated<TU> (Parser<TToken, TU> separator)** -- аналог предыдущего случая, однако, список может быть пустым и опционально завершается указанным значением;
* **SeparatedAndOptionallyTerminatedAtLeastOnce<TU> (Parser<TToken, TU> separator)** -- очередная вариация;
* **SkipAtLeastOnceUntil<TU> (Parser<TToken, TU> terminator)** -- повторно выполняет первый парсер (не менее одного раза!) (отбрасывая результаты), пока не сработает второй;
* **SkipUntil<TU> (Parser<TToken, TU> terminator)** -- повторно выполняет первый парсер (отбрасывая его результаты), пока не сработает второй;
* **Then** -- при успехе обоих парсеров выдает результат второго;
* **Until<TU> (Parser<TToken, TU> terminator)** -- повторно выполняет первый парсер до тех пор, пока не сработает второй;
* **Where (Func<T, bool> predicate)** -- навешивает дополнительное условие на парсер.

## Рекурсивные парсеры

Разбор грамматики с рекурсивными правилами довольно нетривиален. Pidgin предлагает выкручиваться за счет операции `Rec` (от слова "рекурсия")

```c#
Parser<char, char> expr = null!;
Parser<char, char> parenthesised = Char ('(')
    .Then (Rec(() => expr))
    .Before (Char(')'));
expr = Digit.Or (parenthesised);
var result = expr.Parse ("((1))");
Console.WriteLine (result.Success); // True
Console.WriteLine (result.Value); // 1
```

### Устранение левой рекурсии

Хотя Pidgin поддерживает рекурсию в грамматике, он не умеет работать с лево-рекурсивными грамматиками. В этом легко убедиться на примере

```c#
Parser<char, int> arithmetic = null!;
Parser<char, int> addExpr = Map
    (
        (x, _, y) => x + y,
        Rec (() => arithmetic),
        Char ('+'),
        Rec (() => arithmetic)
    );
arithmetic = addExpr.Or (Digit.Select (d => (int) char.GetNumericValue (d)));
arithmetic.Parse("2+2");  // stack overflow!
```

Нужно переписать грамматику, например, так

```
СУММА -> цифра
СУММА -> цифра СУММА'
СУММА' -> '+' цифра
СУММА' -> '+' цифра СУММА'
```

Получается право-рекурсивная грамматика, с которой Pidgin справляется без проблем. В терминах Pidgin это будет

```c#
var digit = Digit.Select (d => (int) char.GetNumericValue (d));
var summa = Map
    (
        (first, second) => first + second.Sum(),
        digit,
        Char ('+').Then (digit).Many()
    );
var result = summa.Parse ("1+2+3+4");
Console.WriteLine (result.Success); // True
Console.WriteLine (result.Value); // 10
```

Впрочем, для разбора арифметических выражений в Pidgin есть специальные очень удобные инструменты, о которых мы поговорим в свое время.

Подытожим:

* **Rec (Func<Parser<TToken, TResult>> parser)** -- организация рекурсии.

## Преобразования

Часто бывает необходимо, чтобы парсер выдавал не строку, а некоторую структуру, например, узел AST. Для этого в Pidgin предусмотрено несколько методов, наиболее востребованными из которых являются `ThenReturn` и `Select`

```c#
var value1 = Char ('c').ThenReturn (new AstNode());
var value2 = String ("name").Select (a => "Hello " + a);
```

Подытожим:

* **Bind<TU> (Func<T, Parser<TToken, TU>> selector)** -- применяет трансформирующую функцию к результату парсера;
* **Cast<TU>()** -- кастует результат к указанному типу;
* **IgnoreResult()** -- заставляет парсер возвращать `Unit.Value` (см. ниже);
* **Fail<T> (string message = "Failed")** -- парсер, который всегда падает с заданным сообщением;
* **FromResult<T> (T result)** -- парсер, который всегда выдает заданный результат;
* **Map<TToken, T1, R> (Func<T1, R> func, Parser<TToken, T1> parser1)** -- трансформация результата одного парсера;
* **Map<TToken, T1, T2, R> (Func<T1, T2, R> func, Parser<TToken, T1> parser1, Parser<TToken, T2> parser2)** -- трансформация результата двух парсеров -- и т. д. вплоть до 8 парсеров;
* **Not (Parser<TToken, TResult> parser)** -- инвертирует результат парсера;
* **OfType<TU>()** -- кастует результат к указанному типу. См `Cast`;
* **RecoverWith (Func<ParseError<TToken>, Parser<TToken, TResult>> errorHandler)** -- запускает обработку ошибки, если текущий парсер выдал ошибку. Обработка заключается в выборе какого-нибудь другого парсера;
* **Select<TU> (Func<T, TU> selector)** -- синоним `Map` для случая одного парсера;
* **ThenReturn<TU> (TU result)** -- заменяет результат для текущего парсера;
* **Try (Parser<TToken, T> parser)** -- если заданный парсер потерпел неудачу, выполняет "откат";
* **WithResult<TU> (TU result)** -- заменяет результат для текущего парсера, синоним для `ThenReturn`;

## Служебные операции

В Pidgin предусмотрен целый арсенал служебных операций

* **Assert (Func<T, bool> predicate)** -- парсер, проверяющий заданный предикат;
* **Assert (Func<T, bool> predicate, string message)** -- парсер, проверяющий заданный предикат;
* **Assert (Func<T, bool> predicate, Func<T, string> message)** -- парсер, проверяющий заданный предикат;
* **Configuration** -- доступ к конфигурации (о ней будет рассказано особо);
* **CurrentOffset** -- текущее смещение от начала входного потока;
* **CurrentPos** -- доступ к текущей позиции в разбираемом тексте;
* **CurrentSourcePosDelta** - доступ к текущей позиции в разбираемом тексте;
* **Labelled (string label)** -- задание имени для парсера;
* **Trace (Func<T, string> message)** -- трассировочная печать;
* **Trace (string message)** -- трассировочная печать;
* **TraceResult()** -- трассировочная печать.
