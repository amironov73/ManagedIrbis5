// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ITagGenerator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public interface ITagGenerator
{
    /// <summary>
    ///
    /// </summary>
    string? ActiveProfile { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    HtmlTag Build
        (
            ElementRequest request,
            string? category = null,
            string? profile = null
        );
}
