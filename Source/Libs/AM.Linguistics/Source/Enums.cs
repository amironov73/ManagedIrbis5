// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Enums.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics;

/// <summary>
/// Род, число и одушевленность
/// </summary>
public enum Gender : byte
{
    /// <summary>
    /// Мужской род
    /// </summary>
    M = 0,

    /// <summary>
    /// Женский род
    /// </summary>
    F = 1,

    /// <summary>
    /// Средний род
    /// </summary>
    N = 2,

    /// <summary>
    /// Множественное число
    /// </summary>
    P = 3,

    /// <summary>
    /// Мужской, одушевленное
    /// </summary>
    MA = 4,

    /// <summary>
    /// Женский, одушевленное
    /// </summary>
    FA = 5,

    /// <summary>
    /// Средний, одушевленное
    /// </summary>
    NA = 6,

    /// <summary>
    /// Общий род, одушевленное
    /// </summary>
    MAFA = 7,

    /// <summary>
    /// Множественное число, одушевленное
    /// </summary>
    PA = 8,

    /// <summary>
    /// Неопределенный
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Одушевленность
/// </summary>
public enum Animacy : byte
{
    /// <summary>
    /// Неодушевленное
    /// </summary>
    Inanimate = 0,

    /// <summary>
    /// Одушевленное
    /// </summary>
    Animate = 1,

    /// <summary>
    /// Неопределенное
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Чмсло
/// </summary>
public enum Number : byte
{
    /// <summary>
    /// Единственное
    /// </summary>
    Singular = 0,

    /// <summary>
    /// Множественное
    /// </summary>
    Plural = 1,

    /// <summary>
    /// Неопределенное
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Падеж
/// </summary>
public enum Case : byte
{
    /// <summary>
    /// Именительный
    /// </summary>
    Nominative = 0,

    /// <summary>
    /// Родительный
    /// </summary>
    Genitive = 1,

    /// <summary>
    /// Дательный
    /// </summary>
    Dative = 2,

    /// <summary>
    /// Винительный
    /// </summary>
    Accusative = 3,

    /// <summary>
    /// Творительный
    /// </summary>
    Instrumental = 4,

    /// <summary>
    /// Предложный
    /// </summary>
    Locative = 5,

    /// <summary>
    /// Краткая форма
    /// </summary>
    Short = 6,

    /// <summary>
    /// Неопределенный
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Разряд прилагательного
/// </summary>
public enum Comparability : byte
{
    /// <summary>
    /// Качественное (сравнимое)
    /// </summary>
    Comparable = 0,

    /// <summary>
    /// Относительное (несравнимое)
    /// </summary>
    Incomparable = 1,

    /// <summary>
    /// Неопределенный
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Степени сравнения
/// </summary>
public enum Comparison : byte
{
    Comparative1 = 0,
    Comparative2 = 1,
    Comparative3 = 2,
    Comparative4 = 3,
    Comparative5 = 4,
    Undefined = 255
}

/// <summary>
/// Лицо
/// </summary>
public enum Person : byte
{
    /// <summary>
    /// Первое лицо
    /// </summary>
    First = 0,

    /// <summary>
    /// Второе лицо
    /// </summary>
    Second = 1,

    /// <summary>
    /// Третье лицо
    /// </summary>
    Third = 2,

    /// <summary>
    /// Неопределенное
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Вид глагола
/// </summary>
public enum VerbAspect : byte
{
    /// <summary>
    /// Несовершенный
    /// </summary>
    Imperfect = 0,

    /// <summary>
    /// Совершенный
    /// </summary>
    Perfect = 1,

    /// <summary>
    /// Совершенно-несовершенный
    /// </summary>
    PerfectImperfect = 2,

    /// <summary>
    /// Неопределенный
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Залог
/// </summary>
public enum Voice : byte
{
    /// <summary>
    /// Активный
    /// </summary>
    Active = 0,

    /// <summary>
    /// Пассивный
    /// </summary>
    Passive = 1,

    /// <summary>
    /// Неопределенный
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Время
/// </summary>
public enum Tense : byte
{
    /// <summary>
    /// Настоящее
    /// </summary>
    Present = 0,

    /// <summary>
    /// Прошлое
    /// </summary>
    Past = 1,

    /// <summary>
    /// Будущее
    /// </summary>
    Future = 2,

    /// <summary>
    /// Неопределенная форма (инфинитив)
    /// </summary>
    Infinitive = 3,

    /// <summary>
    /// Сослагательное наклонение
    /// </summary>
    Subjunctive = 4,

    /// <summary>
    /// Неопределенное
    /// </summary>
    Undefined = 255
}

/// <summary>
/// Валентность глагола
/// </summary>
public enum VerbTransition : byte
{
    /// <summary>
    /// Переходный
    /// </summary>
    Transitive = 0,

    /// <summary>
    /// Непереходный
    /// </summary>
    Intransitive = 1,

    /// <summary>
    /// Неопределенная
    /// </summary>
    Undefined = 255
}
