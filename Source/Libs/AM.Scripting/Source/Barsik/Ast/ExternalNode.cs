// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExternalNode.cs -- выполнение внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Выполнение некоторого внешнего по отношению к Барсику кода.
/// Полагается на глобальный обработчик, заданный в контексте
/// интерпретатора.
/// </summary>
public sealed class ExternalNode
    : StatementNode
{
    #region Properties

    /// <summary>
    /// Произвольный код, понятный обработчику внешнего кода.
    /// </summary>
    public string? Code { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExternalNode
        (
            SourcePosition position,
            string? code
        )
        : base (position)
    {
        Code = code;
    }

    #endregion

    #region StatementNode example

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var interpreter = context.GetTopContext().Interpreter.ThrowIfNull();
        var handler = interpreter.ExternalCodeHandler;
        if (handler is null)
        {
            context.Error.WriteLine ("Missing external code handler");
            return;
        }

        try
        {
            handler (context, this);
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"external ({StartPosition}): {Code}";
    }

    #endregion
}
