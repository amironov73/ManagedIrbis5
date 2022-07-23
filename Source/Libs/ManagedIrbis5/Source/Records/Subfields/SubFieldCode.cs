// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* SubFieldCode.cs -- методы, работающие с кодами подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using AM;
using AM.Collections;

using Microsoft.Extensions.Logging;

#endregion

namespace ManagedIrbis;

/*

    > **ИДЕНТИФИКАТОР ПОДПОЛЯ** (Subfield Identifier) или **КОД ПОДПОЛЯ**
    > (Subfield code) – код, идентифицирующий отдельные подполя внутри переменного
    > поля. Состоит из двух символов. Первый символ – разделитель (Delimiter),
    > всегда один и тот же уникальный символ, установленный по ISO 2709,
    > второй символ – код подполя (Subfield code), который может быть цифровым
    > или буквенным.
    >
    > *Стандарт RUSMARC*

    Коды подполей нечувствительны к регистру символов.

    Технически возможны любые коды подполей, однако стандарт допускает
    лишь коды в дипапазоне от `\u0021` (восклицательный знак)
    до `\u007E` (тильда).

    Кириллические символы лучше не использовать в качестве кодов подполей - возникнут
    проблемы при экспорте записей в коммуникативные форматы. ManagedIrbis обрабатывает
    любые коды подполей без проблем.

    В стандарте RUSMARC принято ссылаться на подполя `$a`, `$b` и т. д.
    В документации ИРБИС64 принято обозначение `^a`, `^b` и т. д.
    Мы будем придерживаться последнего обозначения.

    Стандарт допускает лишь как правило алфавитно-цифровые код
    подполей `A-Z, 0-9`, но в ИРБИС64 бывают подполя с экзотическими
    кодами вроде `!`, `(` и др.

    ИРБИС64 трактует код подполя `*` как "данные до первого разделителя
    либо значение первого по порядку подполя" (смотря по тому,
    что присутствует в записи). ManagedIrbis поддерживает эту особенность.

    Подполе с кодом `\0` используется для хранения значения поля до первого разделителя.
    Это расширение ManagedIrbis.

    Код подполя может быть равен разделителю подполей.

 */

/// <summary>
/// Методы, работающие с кодами подполей.
/// </summary>
public static class SubFieldCode
{
    #region Constants

    /// <summary>
    /// Начало диапазона допустимых кодов подполей.
    /// </summary>
    public const char DefaultFirstCode = '!';

    /// <summary>
    /// Конец диапазона допустимых кодов подполей (включительно!).
    /// </summary>
    public const char DefaultLastCode = '~';

    #endregion

    #region Properties

    /// <summary>
    /// Нужно ли выбрасывать исключение при ошибке верификации кода подполя.
    /// По умолчанию - не нужно.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static bool ThrowOnVerification { get; set; }

    /// <summary>
    /// Набор символов <see cref="CharSet"/> для допустимых кодов подполей.
    /// </summary>
    public static CharSet ValidCodes => _validCodes;

    #endregion

    #region Construction

    /// <summary>
    /// Static constructor.
    /// </summary>
    static SubFieldCode()
    {
        _validCodes = new CharSet();
        _validCodes.AddRange (DefaultFirstCode, DefaultLastCode);

        // _validCodes.Remove ('^'); // символ крышки мы трактуем как допустимый
    }

    #endregion

    #region Private members

    private static readonly CharSet _validCodes;

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка кода подполя на валидность.
    /// </summary>
    public static bool IsValidCode (char code) => ValidCodes.Contains (code);

    /// <summary>
    /// Нормализация кода подполя.
    /// Заключается в приведении символа к нижнему регистру
    /// (если возможно).
    /// </summary>
    public static char Normalize (char code) => char.ToLowerInvariant (code);

    /// <summary>
    /// Верификация кода подполя.
    /// </summary>
    public static bool Verify
        (
            char code,
            bool throwOnError
        )
    {
        var result = IsValidCode (code);

        if (!result)
        {
            Magna.Logger.LogDebug
                (
                    nameof (SubFieldCode) + "::" + nameof (Verify)
                    + ": bad code='{Code}'",
                    code
                );

            if (throwOnError)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        return result;
    }

    /// <summary>
    /// Верификация кода подполя.
    /// </summary>
    public static bool Verify (char code)
    {
        return Verify (code, ThrowOnVerification);
    }

    #endregion
}
