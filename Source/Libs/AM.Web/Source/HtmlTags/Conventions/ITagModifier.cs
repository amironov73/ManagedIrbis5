// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ITagModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public interface ITagModifier
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    bool Matches
        (
            ElementRequest token
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    void Modify
        (
            ElementRequest request
        );
}
