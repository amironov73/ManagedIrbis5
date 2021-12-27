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
    public string VariableName { get; }

    public IEnumerable<StatementNode> Block { get; }

    public CatchNode
        (
            string variableName,
            IEnumerable<StatementNode> block
        )
    {
        VariableName = variableName;
        Block = block;
    }
}
