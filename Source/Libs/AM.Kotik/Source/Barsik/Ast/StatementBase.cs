// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* StatementBase.cs -- базовый класс для стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Базовый класс для стейтментов.
/// </summary>
public class StatementBase
    : AstNode
{
    #region Private members

    /// <summary>
    /// Номер строки, в которой находится стейтмент в исходном тексте скрипта.
    /// </summary>
    public int Line { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="line"></param>
    public StatementBase
        (
            int line
        )
    {
        Line = line;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Вызывается перед выполнением стейтмента.
    /// </summary>
    protected virtual void PreExecute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        // трассировка
        var topContext = context.GetTopContext();
        var interpreter = topContext.Interpreter;
        var debugger = interpreter?.ScriptDebugger;

        if (debugger is not null)
        {
            debugger.PreTrace (context, this);
            if (debugger.Breakpoints.TryGetValue (this, out var breakpoint))
            {
                if (breakpoint.Trace)
                {
                    debugger.Trace (context, this);
                }

                if (breakpoint.Break)
                {
                    debugger.Raise (context, this);
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение действий, связанных с данным узом,
    /// в определенном контексте.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.
    /// </param>
    public virtual void Execute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        PreExecute (context);

        // больше никаких действий на данном уровне не нужно
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="AstNode.ToString"/>
    public override string ToString() => $"{base.ToString()} at line {Line}";

    #endregion
}
