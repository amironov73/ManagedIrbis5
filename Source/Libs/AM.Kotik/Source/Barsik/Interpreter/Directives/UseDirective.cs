// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* UseDirective.cs -- использование пространства имен
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Использование указанного пространства имен.
/// </summary>
public sealed class UseDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UseDirective()
        : base ("use")
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
        argument = argument.SafeTrim();
        context = context.GetTopContext();
        if (string.IsNullOrEmpty (argument))
        {
            context.DumpNamespaces();
        }
        else
        {
            context.Namespaces[argument] = null;
        }
    }

    #endregion
}
