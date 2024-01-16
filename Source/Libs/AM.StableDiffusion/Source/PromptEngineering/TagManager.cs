// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagManager.cs -- централизует работу с тегами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Централизует работу с тегами.
/// </summary>
[PublicAPI]
public sealed class TagManager
{
    #region Properties

    /// <summary>
    /// Требуемые теги, например, "background".
    /// </summary>
    public TagList RequiredTags { get; } = new ();

    /// <summary>
    /// Нежелательные теги, например, "solo" или "realistic".
    /// </summary>
    public TagList UnwantedTags { get; } = new ();

    #endregion

    #region Public methods


    #endregion
}
