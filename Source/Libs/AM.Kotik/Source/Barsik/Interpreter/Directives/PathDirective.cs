// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PathDirective.cs -- задание пути для поиска скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Задание пути для поиска скриптов.
/// </summary>
public sealed class PathDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PathDirective()
        : base ("path")
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
        var paths = context.Commmon.Settings.Paths;
        if (string.IsNullOrEmpty (argument))
        {
            if (paths.IsNullOrEmpty())
            {
                context.Commmon.Output?.WriteLine ("(no include path)");
                return;
            }
        
            foreach (var path in paths)
            {
                context.Commmon.Output?.WriteLine (path);
            }

            return;
        }

        var comparison = StringComparison.Ordinal;
        if (OperatingSystem.IsWindows())
        {
            comparison = StringComparison.OrdinalIgnoreCase;
        }
        
        foreach (var path in paths)
        {
            if (string.Compare (path, argument, comparison) == 0)
            {
                return;
            }
        }
        
        paths.Add (argument);
    }

    #endregion
}
