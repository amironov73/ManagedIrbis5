// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* UsingNode.cs -- блок using
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Блок using.
/// </summary>
sealed class UsingNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UsingNode
        (
            SourcePosition position,
            string variableName,
            AtomNode initialization,
            IEnumerable<StatementNode> body
        )
        : base (position)
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (initialization);
        Sure.NotNull ((object?) body);

        _variableName = variableName;
        _initialization = initialization;
        _body = new (body);
    }

    #endregion

    #region Private members

    private readonly string _variableName;
    private readonly AtomNode _initialization;
    private readonly List<StatementNode> _body;

    #endregion

    #region Statement members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var disposable = _initialization.Compute (context);
        if (disposable is null)
        {
            return;
        }

        var childContext = context.CreateChild();
        childContext.Variables[_variableName] = disposable;
        foreach (var statement in _body)
        {
            statement.Execute (childContext);
        }

        ((IDisposable) disposable).Dispose();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"using ({StartPosition}): {_variableName} = {_initialization}";
    }

    #endregion
}
