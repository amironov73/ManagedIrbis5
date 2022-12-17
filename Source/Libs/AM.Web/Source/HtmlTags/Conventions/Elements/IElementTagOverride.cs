// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IElementTagOverride.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

/// <summary>
///
/// </summary>
public interface IElementTagOverride
{
    /// <summary>
    ///
    /// </summary>
    string Category { get; }

    /// <summary>
    ///
    /// </summary>
    string Profile { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IElementBuilder Builder();
}

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ElementTagOverride<T>
    : IElementTagOverride
    where T : IElementBuilder, new()
{
    #region Properties

    /// <inheritdoc cref="IElementTagOverride.Category"/>
    public string Category { get; }

    /// <inheritdoc cref="IElementTagOverride.Profile"/>
    public string Profile { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    public ElementTagOverride
        (
            string? category,
            string? profile
        )
    {
        Category = category ?? TagConstants.Default;
        Profile = profile ?? TagConstants.Default;
    }

    #endregion

    #region IElementTagOverride members

    /// <inheritdoc cref="IElementTagOverride.Builder"/>
    public IElementBuilder Builder() => new T();

    #endregion
}
