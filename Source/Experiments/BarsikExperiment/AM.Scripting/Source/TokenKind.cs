// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* TokenKind.cs -- типы токенов в Барсике
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Scripting;

/// <summary>
/// Типы токенов в Барсике.
/// </summary>
public enum TokenKind
{

    /// <summary>
    /// Достигнут конец текста.
    /// </summary>
    EOT,

    /// <summary>
    /// Идентификатор
    /// </summary>
    Identifier,

    /// <summary>
    /// Одиночный символ.
    /// </summary>
    Character,

    /// <summary>
    /// Строка.
    /// </summary>
    String,

    /// <summary>
    /// Сырая строка.
    /// </summary>
    RawString,

    /// <summary>
    /// Регулярное выражение.
    /// </summary>
    Regex,

    /// <summary>
    /// <c>null</c>.
    /// </summary>
    Null,

    /// <summary>
    /// <c>false</c>.
    /// </summary>
    False,

    /// <summary>
    /// <c>true</c>.
    /// </summary>
    True,

    /// <summary>
    /// 32-битное десятеричное со знаком, без префиксов и суффиксов.
    /// </summary>
    Int32,

    /// <summary>
    /// 64-битное десятеричное со знаком, без префикса, но с суффиксом "L".
    /// </summary>
    Int64,

    /// <summary>
    /// 32-битное десятеричное без знака, без префикса, но с суффиксом "U".
    /// </summary>
    UInt32,

    /// <summary>
    /// 64-битное десятеричное без знака, без префикса, но с суффиксом "UL".
    /// </summary>
    UInt64,

    /// <summary>
    /// 32-битное шестнадцатеричное без знака, префикс "0x", без суффикса.
    /// </summary>
    Hex32,

    /// <summary>
    /// 64-битное шестнадцатеричное без знака, префикс "0x", суффикс "L".
    /// </summary>
    Hex64,

    /// <summary>
    /// 32-битное двоичное без знака, префикс "0b", без суффикса.
    /// </summary>
    Bin32,

    /// <summary>
    /// 64-битное двоичное без знака, префикс "0b", суффикс "L".
    /// </summary>
    Bin64,

    /// <summary>
    /// Целое десятеричное произвольной длины со знаком, без префикса, суффикс "B".
    /// </summary>
    BigInteger,

    /// <summary>
    /// Число с плавающей точкой одинарной точности.
    /// </summary>
    Float,

    /// <summary>
    /// Число с плавающей точкой двойной точности.
    /// </summary>
    Double,

    /// <summary>
    /// Число с фиксированной точкой (денежное).
    /// </summary>
    Decimal,

    /// <summary>
    /// Точка.
    /// </summary>
    Dot,

    /// <summary>
    /// Запятая.
    /// </summary>
    Comma,

    /// <summary>
    /// Двоеточие.
    /// </summary>
    Colon,

    /// <summary>
    /// Точка с запятой.
    /// </summary>
    Semicolon,

    /// <summary>
    /// Открывающая скобка (круглая).
    /// </summary>
    OpenBracket,

    /// <summary>
    /// Закрывающая скобка (круглая).
    /// </summary>
    CloseBracket,

    /// <summary>
    /// Знак "меньше" (<c>&lt;</c>).
    /// </summary>
    Less,

    /// <summary>
    /// Знак "больше" (<c>&gt;</c>).
    /// </summary>
    More,

    /// <summary>
    /// Знак равества.
    /// </summary>
    Equal,

    /// <summary>
    /// Открывающая скобка (квадратная).
    /// </summary>
    OpenSquareBracket,

    /// <summary>
    /// Закрывающая скобка (квадратная).
    /// </summary>
    CloseSquareBracket,

    /// <summary>
    /// Открывающая скобка (фигурная).
    /// </summary>
    OpenBrace,

    /// <summary>
    /// Закрывающая скобка (фигурная).
    /// </summary>
    CloseBrace,

    /// <summary>
    /// Знак "плюс".
    /// </summary>
    Plus,

    /// <summary>
    /// Знак "минус".
    /// </summary>
    Minus,

    /// <summary>
    /// Знак умножения (звездочка).
    /// </summary>
    Star,

    /// <summary>
    /// Знак деления (косая черта).
    /// </summary>
    Slash,

    /// <summary>
    /// Знак процента.
    /// </summary>
    Percent,

    /// <summary>
    /// Амперсанд (&amp;).
    /// </summary>
    Ampersand,

    /// <summary>
    /// Коммерческое "at" (@).
    /// </summary>
    At,

    /// <summary>
    /// Обратная косая черта.
    /// </summary>
    Backslash,

    /// <summary>
    /// Крышка (^).
    /// </summary>
    Circumflex,

    /// <summary>
    /// Знак подчеркивания (_).
    /// </summary>
    Underscore,

    /// <summary>
    /// Обратная кавычка.
    /// </summary>
    Grave,

    /// <summary>
    /// Вертикальная черта.
    /// </summary>
    VerticalLine,

    /// <summary>
    /// Знак доллара.
    /// </summary>
    Dollar,

    /// <summary>
    /// Знак решетки (#).
    /// </summary>
    NumberSign,

    /// <summary>
    /// Тильда.
    /// </summary>
    Tilda,

    /// <summary>
    /// Восклицательный знак.
    /// </summary>
    Bang,

    /// <summary>
    /// Вопросительный знак.
    /// </summary>
    Question,

    /// <summary>
    /// +=
    /// </summary>
    PlusEqual,

    /// <summary>
    /// ++
    /// </summary>
    PlusPlus,

    /// <summary>
    /// -=
    /// </summary>
    MinusEqual,

    /// <summary>
    /// --
    /// </summary>
    MinusMinus,

    /// <summary>
    /// *=
    /// </summary>
    StarEqual,

    /// <summary>
    /// /=
    /// </summary>
    SlashEqual,

    /// <summary>
    /// //
    /// </summary>
    EqualEqual,

    /// <summary>
    /// &lt;=
    /// </summary>
    LessEqual,

    /// <summary>
    /// &gt;=
    /// </summary>
    MoreEqual,

    /// <summary>
    /// &lt;&gt;
    /// </summary>
    LessMore,

    /// <summary>
    /// !=
    /// </summary>
    NotEqual,

    /// <summary>
    /// &lt;&lt;
    /// </summary>
    LessLess,

    /// <summary>
    /// &gt;&gt;
    /// </summary>
    MoreMore,

    /// <summary>
    /// %=
    /// </summary>
    PercentEqual,

    /// <summary>
    /// &amp;=
    /// </summary>
    AmpersandEqual,

    /// <summary>
    /// |=
    /// </summary>
    VerticalEqual,

    /// <summary>
    /// ===
    /// </summary>
    EqualEqualEqual,

    /// <summary>
    /// &amp;&amp;
    /// </summary>
    AmpersandAmpersand,

    /// <summary>
    /// ||
    /// </summary>
    VerticalVertical,
}
