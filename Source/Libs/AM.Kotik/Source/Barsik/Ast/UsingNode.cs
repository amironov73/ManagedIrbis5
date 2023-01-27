// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* UsingNode.cs -- блок using
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Блок using.
/// </summary>
internal sealed class UsingNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UsingNode
        (
            int line,
            string variableName,
            AtomNode initialization,
            StatementBase body
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (initialization);
        Sure.NotNull (body);

        _variableName = variableName;
        _initialization = initialization;
        _body = body;
    }

    #endregion

    #region Private members

    private readonly string _variableName;
    private readonly AtomNode _initialization;
    private readonly StatementBase _body;

    #endregion

    #region Statement members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var variable = _initialization.Compute (context);

        var contextToUse = context;
        if (!contextToUse.TryGetVariable (_variableName, out _))
        {
            var childContext = context.CreateChildContext();
            childContext.Variables[_variableName] = variable;
            contextToUse = childContext;
        }

        _body.Execute (contextToUse);
        if (variable is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

        DumpHierarchyItem ("Variable", level + 1, writer, _variableName);
        _body.DumpHierarchyItem ("Block", level + 1, writer);
    }

    #endregion
}
