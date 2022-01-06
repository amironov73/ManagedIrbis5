// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DefinitionNode.cs -- псевдо-узел: определение функции в скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Псевдо-узел: определение функции в скрипте.
/// </summary>
internal sealed class DefinitionNode
    : PseudoNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="theName"></param>
    /// <param name="argumentNames"></param>
    /// <param name="body"></param>
    public DefinitionNode
        (
            string theName,
            IEnumerable<string>? argumentNames,
            IEnumerable<StatementNode>? body
        )
    {
        Sure.NotNullNorEmpty (theName);
        Sure.NotNull ((object?) body);

        this.theName = theName;
        theArguments = new ();
        theBody = new ();
        if (argumentNames is not null)
        {
            theArguments.AddRange (argumentNames);
        }

        if (body is not null)
        {
            theBody.AddRange (body);
        }
    }

    #endregion

    #region Private members

    internal readonly string theName;
    internal readonly List<string> theArguments;
    internal readonly List<StatementNode> theBody;

    #endregion
}
