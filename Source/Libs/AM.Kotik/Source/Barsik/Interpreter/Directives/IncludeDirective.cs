// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IncludeDirective.cs -- включение суб-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;


#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Включение суб-скрипта.
/// </summary>
public sealed class IncludeDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncludeDirective()
        : base ("include")
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
        var interpreter = context.Interpreter;
        if (interpreter is null)
        {
            return;
        }

        if (string.IsNullOrEmpty (argument))
        {
            var pathes = interpreter.Pathes;
            if (pathes.IsNullOrEmpty())
            {
                context.Output?.WriteLine ("(no include pathes)");
                return;
            }
            
            foreach (var path in pathes)
            {
                context.Output?.WriteLine (path);
            }

            return;
        }

        var fileName = argument.Trim();
        interpreter.Include (fileName);
    }

    #endregion
}
