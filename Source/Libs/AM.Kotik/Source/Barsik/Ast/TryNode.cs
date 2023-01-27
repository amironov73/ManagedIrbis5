// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TryNode.cs -- блок try-catch-finally
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Блок try-catch-finally
/// </summary>
internal sealed class TryNode
    : StatementBase
{
    #region NestedTypes

    internal sealed class CatchBlock
        : AstNode
    {
        #region Properties

        public string Name { get; }

        public StatementBase Body { get; }

        #endregion

        #region Construction

        public CatchBlock
            (
                string name,
                StatementBase body
            )
        {
            Name = name;
            Body = body;
        }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TryNode
        (
            int line,
            StatementBase tryBlock,
            CatchBlock? catchBlock,
            StatementBase? finallyBlock
        )
        : base(line)
    {
        Sure.NotNull (tryBlock);

        _tryBlock = tryBlock;
        _variableName = catchBlock?.Name;
        _catchBlock = catchBlock?.Body;
        _finallyBlock = finallyBlock;
    }

    #endregion

    #region Private members

    private readonly StatementBase _tryBlock;
    private readonly string? _variableName;
    private readonly StatementBase? _catchBlock;
    private readonly StatementBase? _finallyBlock;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        try
        {
            _tryBlock.Execute (context);
        }
        catch (Exception exception)
        {
            var nestedContext = context.CreateChildContext();
            if (_catchBlock is not null)
            {
                if (!string.IsNullOrEmpty (_variableName))
                {
                    nestedContext.Variables[_variableName] = exception;
                }

                _catchBlock.Execute (nestedContext);
            }
        }
        finally
        {
            _finallyBlock?.Execute (context);
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        _tryBlock.DumpHierarchyItem ("Try", level + 1, writer);
        if (!string.IsNullOrEmpty (_variableName))
        {
            DumpHierarchyItem ("Variable", level + 1, writer, _variableName);
        }

        _catchBlock?.DumpHierarchyItem ("Catch", level + 1, writer);
        _finallyBlock?.DumpHierarchyItem ("Finally", level + 1, writer);
    }

    #endregion
}
