// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ForEachNode.cs -- цикл foreach
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Цикл foreach.
/// </summary>
internal sealed class ForEachNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ForEachNode
        (
            int line,
            string variableName,
            AtomNode enumerable,
            StatementBase body,
            StatementBase? elseBody
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (enumerable);

        _variableName = variableName;
        _enumerable = enumerable;
        _body = body;
        _elseBody = elseBody;
    }

    #endregion

    #region Private members

    private readonly string _variableName;
    private readonly AtomNode _enumerable;
    private readonly StatementBase _body;
    private readonly StatementBase? _elseBody;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var enumerable = _enumerable.Compute (context);
        if (enumerable is null or not IEnumerable)
        {
            return;
        }

        try
        {
            var success = false;
            foreach (var value in enumerable)
            {
                success = true;
                context.Variables[_variableName] = value;
                try
                {
                    _body.Execute (context);
                }
                catch (ContinueException)
                {
                    Debug.WriteLine ("foreach-continue");
                }
            }

            if (!success && _elseBody is not null)
            {
                _elseBody.Execute (context);
            }
        }
        catch (BreakException)
        {
            Debug.WriteLine ("foreach-break");
        }
    }

    #endregion
}
