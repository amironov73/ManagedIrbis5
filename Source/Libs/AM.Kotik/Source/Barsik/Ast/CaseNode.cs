// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CaseNode.cs -- клауза case в switch-стейтменте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Клауза case в switch-стейтменте.
/// </summary>
internal sealed class CaseNode
    : AstNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaseNode
        (
            AtomNode condition,
            StatementBase body
        )
    {
        Sure.NotNull (body);

        _condition = condition;
        _body = body;
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly StatementBase _body;

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка условия и выполнение тела в случае успеха.
    /// </summary>
    public bool CheckAndExecute
        (
            Context context,
            dynamic? value
        )
    {
        Sure.NotNull (context);

        var condition = _condition.Compute (context);
        var result = (bool) (OmnipotentComparer.Default.Compare (value, condition) == 0);

        if (result)
        {
            _body.Execute (context);
        }

        return result;
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

        if (_condition is null)
        {
            DumpHierarchyItem ("Condition", level + 1, writer, "(null)");
        }
        else
        {
            _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        }

        _body.DumpHierarchyItem ("Body", level + 1, writer);
    }

    #endregion
}
