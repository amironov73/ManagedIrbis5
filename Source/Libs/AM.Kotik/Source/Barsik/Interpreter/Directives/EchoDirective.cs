// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* EchoDirective.cs -- переключение флага "Echo"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: переключение флага "Echo".
/// </summary>
public sealed class EchoDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EchoDirective()
        : base ("echo")
    {
        // пустое тело метода
    }

    #endregion

    #region DirectiveBase members

    /// <inheritdoc cref="DirectiveBase.Execute"/>
    public override void Execute
        (
            Context context,
            string? argument
        )
    {
        var rootContext = context.GetRootContext();
        var repl = (Repl?) rootContext.UserData["repl"];
        if (repl is not null)
        {
            var echo = repl.Echo;
            if (argument.SameString ("on"))
            {
                echo = true;
            }
            else if (argument.SameString ("off"))
            {
                echo = false;
            }

            var onoff = echo ? "on" : "off";
            repl.Echo = echo;
            context.Commmon.Output?.WriteLine ($"Echo is {onoff} now");
        }
    }

    #endregion
}
