// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ShebangDirective.cs -- псевдодиректива
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Псевдодиректива для совместимости с bash.
/// </summary>
public sealed class ShebangDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ShebangDirective()
        : base ("!")
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
        // пустое тело метода
    }

    #endregion
}
