// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstAssignment.cs -- оператор присваивания значения переменной
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Оператор присваивания значения переменной.
/// </summary>
public sealed class AstAssignment
    : AstNode
{
    #region Properties

    /// <summary>
    /// Имя переменной назначения.
    /// </summary>
    public string TargetName { get; }

    /// <summary>
    /// Вычисляемое выражение.
    /// </summary>
    public AstValue Expression { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstAssignment
        (
            string targetName,
            AstValue expression
        )
    {
        TargetName = targetName;
        Expression = expression;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.Execute"/>
    public override void Execute
        (
            LanguageContext context
        )
    {
        var target = context.FindOrCreateVariable (TargetName);
        var value = Expression.ComputeInt32 (context);

        target.Value = value;
    }

    #endregion
}
