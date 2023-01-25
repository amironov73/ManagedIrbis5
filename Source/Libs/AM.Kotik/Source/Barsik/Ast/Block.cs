// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Block.cs -- блок стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Блок стейтментов.
/// </summary>
public sealed class BlockNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BlockNode
        (
            int line,
            StatementBase[] statements
        )
        : base (line)
    {
        _statements = statements;
    }

    #endregion

    #region Private members

    private readonly StatementBase[] _statements;

    private int? FindLabel 
        (
            string label
        )
    {
        for (var i = 0; i < _statements.Length; i++)
        {
            if (_statements[i] is LabelNode labelNode 
                && string.CompareOrdinal (label, labelNode.Name) == 0)
            {
                return i;
            }
        }

        return null;
    }

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var index = 0;
        while (index < _statements.Length)
        {
            var statement = _statements[index];
            try
            {
                statement.Execute (context);
                index++;
            }
            catch (GotoException gotoException)
            {
                var whereLabel = FindLabel (gotoException.Label);
                if (!whereLabel.HasValue)
                {
                    // передаем исключение наверх
                    throw;
                }

                index = whereLabel.Value;
            }
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

        foreach (var statement in _statements)
        {
            statement.DumpHierarchyItem ("Statement", level + 1, writer);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Перечисление элементов.
    /// </summary>
    public IEnumerator<StatementBase> GetEnumerator()
    {
        return ((IEnumerable<StatementBase>) _statements).GetEnumerator();
    }

    #endregion
}
