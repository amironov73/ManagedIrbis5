// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StandardDumper.cs -- стандартный дампер контекста.
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Diagnostics;

/// <summary>
/// Стандартный дампер контекста, распечатывает переменные
/// в стандартный поток вывода.
/// </summary>
public sealed class StandardDumper
    : IBarsikDumper
{
    #region IBarsikDumper members

    /// <inheritdoc cref="IBarsikDumper.DumpContext"/>
    public void DumpContext
        (
            Context context
        )
    {
        Sure.NotNull (context);

        if (context.Commmon.Output is { } output)
        {
            output.WriteLine (new string ('=', 60));
            context.DumpVariables();
            output.WriteLine (new string ('=', 60));
        }
    }

    #endregion
}
