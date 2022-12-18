// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LambdaTagModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class LambdaTagModifier
    : ITagModifier
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="matcher"></param>
    /// <param name="modify"></param>
    public LambdaTagModifier
        (
            Func<ElementRequest, bool> matcher,
            Action<ElementRequest> modify
        )
    {
        Sure.NotNull (matcher);
        Sure.NotNull (modify);

        _matcher = matcher;
        _modify = modify;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modify"></param>
    public LambdaTagModifier
        (
            Action<ElementRequest> modify
        )
        : this (_ => true, modify)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private readonly Func<ElementRequest, bool> _matcher;
    private readonly Action<ElementRequest> _modify;

    #endregion

    #region ITagBuilder members

    /// <inheritdoc cref="ITagModifier.Matches"/>
    public bool Matches (ElementRequest token) => _matcher (token);

    /// <inheritdoc cref="ITagModifier.Modify"/>
    public void Modify (ElementRequest request) => _modify (request);

    #endregion
}
