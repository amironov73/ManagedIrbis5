// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* CatchNode.cs -- блок catch
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Блок catch.
/// </summary>
sealed class CatchNode
{
    #region Properties

    /// <summary>
    /// Начальная позиция в исходном коде.
    /// </summary>
    public SourcePosition StartPosition { get; }

    /// <summary>
    /// Имя переменной, в которую помещается перехваченное исключение.
    /// </summary>
    public string VariableName { get; }

    /// <summary>
    /// Стейтменты, задающие реакцию на исключение.
    /// </summary>
    public IEnumerable<StatementNode> Block { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CatchNode
        (
            SourcePosition position,
            string variableName,
            IEnumerable<StatementNode> block
        )
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull ((object?) block);

        StartPosition = position;
        VariableName = variableName;
        Block = block;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"catch ({StartPosition})";
    }

    #endregion
}
