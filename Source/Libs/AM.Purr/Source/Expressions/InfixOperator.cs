// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InfixOperator.cs -- инфиксный оператор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Purr.Parsers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Expressions;

/// <summary>
/// Инфиксный оператор.
/// </summary>
[PublicAPI]
public class InfixOperator<TResult>
{
    #region Properties

    /// <summary>
    /// Операция.
    /// </summary>
    public Parser<string> Operation { get; }

    /// <summary>
    /// Функция, производящая вычисления.
    /// </summary>
    public Func<TResult, string, TResult, TResult> Function { get; }

    /// <summary>
    /// Вид оператора.
    /// </summary>
    public InfixOperatorKind Kind { get; }

    /// <summary>
    /// Метка (для отладки).
    /// </summary>
    public string Label { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InfixOperator
        (
            Parser<string> operation,
            Func<TResult, string, TResult, TResult> function,
            string label,
            InfixOperatorKind kind
        )
    {
        Sure.NotNull (operation);
        Sure.NotNull (function);
        Sure.NotNullNorEmpty (label);

        Operation = operation;
        Function = function;
        Kind = kind;
        Label = label;
    }

    #endregion
}
