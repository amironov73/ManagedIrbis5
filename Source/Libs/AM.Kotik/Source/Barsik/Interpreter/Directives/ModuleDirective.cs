// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ModuleDirective.cs -- дамп загруженных модулей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: дамп загруженных модулей.
/// </summary>
public sealed class ModuleDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ModuleDirective()
        : base ("module")
    {
        // пустое тело конструктора
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
        var interpreter = context.GetTopContext().Interpreter;
        if (interpreter is not null)
        {
            if (string.IsNullOrEmpty (argument))
            {
                var modules = interpreter.Modules;
                foreach (var module in modules)
                {
                    context.Output?.WriteLine ($"{module.GetType().Name}: {module.Description} v{module.Version}");
                }

                if (modules.Count == 0)
                {
                    context.Output?.WriteLine ("(no modules loaded)");
                }
            }
            else
            {
                if (argument.StartsWith ("/"))
                {
                    var navigator = new TextNavigator (argument);
                    navigator.ReadChar();
                    var subCommand = navigator.ReadWord();
                    navigator.SkipWhitespace();
                    var subArgument = navigator.GetRemainingText().ToString();
                    if (subCommand.SameString ("unload"))
                    {
                        var success = interpreter.UnloadModule (subArgument);
                        context.Output?.WriteLine ($"Unload module {subArgument}: {success}");
                    }
                }
                else
                {
                    interpreter.LoadModule (argument);
                }
            }
        }
    }

    #endregion
}
