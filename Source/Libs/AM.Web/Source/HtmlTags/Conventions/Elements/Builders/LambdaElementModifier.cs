// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LambdaElementModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
// Tested through HtmlConventionRegistry
public class LambdaElementModifier
    : LambdaTagModifier, IElementModifier
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? ConditionDescription { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ModifierDescription { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="matcher"></param>
    /// <param name="modify"></param>
    public LambdaElementModifier
        (
            Func<ElementRequest, bool> matcher,
            Action<ElementRequest> modify
        )
        : base (matcher, modify)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modify"></param>
    public LambdaElementModifier
        (
            Action<ElementRequest> modify
        )
        : base (modify)
    {
        // пустое тело конструктора
    }

    #endregion
}
