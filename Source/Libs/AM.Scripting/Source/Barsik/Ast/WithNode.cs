// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* WithNode.cs -- блок With
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Блок with
/// </summary>
internal sealed class WithNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WithNode
        (
            SourcePosition startPosition,
            AtomNode center,
            IEnumerable<StatementNode> statements
        )
        : base (startPosition)
    {
        Sure.NotNull (center);
        Sure.NotNull (statements);

        _center = center;
        _statements = new List<StatementNode> (statements);
    }

    #endregion

    #region Private members

    private readonly AtomNode _center;

    private readonly List<StatementNode> _statements;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var previousCenter = context.With;
        context.With = _center;

        try
        {
            foreach (var statement in _statements)
            {
                statement.Execute (context);
            }
        }
        finally
        {
            context.With = previousCenter;
        }
    }

    #endregion
}
