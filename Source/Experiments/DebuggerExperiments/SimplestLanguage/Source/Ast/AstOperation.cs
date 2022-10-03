// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstOperation.cs -- арифметическая операция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Арифметическая операция.
/// </summary>
public sealed class AstOperation
    : AstValue
{
    #region Properties

    /// <summary>
    /// Левая часть выражения.
    /// </summary>
    public AstValue Left { get; }

    /// <summary>
    /// Код операции: плюс или минус.
    /// </summary>
    public char OperationCode { get; }

    /// <summary>
    /// Правая часть выражения.
    /// </summary>
    public AstValue Right { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstOperation
        (
            AstValue left,
            char operationCode,
            AstValue right
        )
    {
        Sure.NotNull (left);
        Sure.NotNull (right);

        Left = left;
        OperationCode = operationCode;
        Right = right;
    }

    #endregion

    #region AstValue members

    /// <inheritdoc cref="AstValue.ComputeInt32"/>
    public override int ComputeInt32
        (
            LanguageContext context
        )
    {
        Sure.NotNull (context);

        var leftValue = Left.ComputeInt32 (context);
        var rightValue = Right.ComputeInt32 (context);

        return OperationCode switch
        {
            '+' => leftValue + rightValue,
            '-' => leftValue - rightValue,
            _ => throw new LanguageException()
        };
    }

    #endregion
}
