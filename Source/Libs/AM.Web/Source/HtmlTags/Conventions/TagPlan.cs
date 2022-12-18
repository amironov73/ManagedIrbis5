// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagPlan.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

#region Using directives

using Elements;

#endregion

/// <summary>
///
/// </summary>
public interface ITagPlan
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    HtmlTag Build (ElementRequest request);
}

/// <summary>
///
/// </summary>
public class TagPlan
    : ITagPlan
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public ITagBuilder Builder { get; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<ITagModifier> Modifiers => _modifiers;

    /// <summary>
    ///
    /// </summary>
    public IElementNamingConvention? ElementNamingConvention { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="modifiers"></param>
    /// <param name="elementNamingConvention"></param>
    public TagPlan
        (
            ITagBuilder builder,
            IEnumerable<ITagModifier> modifiers,
            IElementNamingConvention? elementNamingConvention
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (modifiers);

        Builder = builder;
        ElementNamingConvention = elementNamingConvention;

        // Important to force the enumerable to be executed no later than this point
        _modifiers.AddRange (modifiers);
    }

    #endregion

    #region Private members

    private readonly List<ITagModifier> _modifiers = new ();

    #endregion

    #region ITagPlan members

    /// <inheritdoc cref="ITagPlan.Build"/>
    public HtmlTag Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        request.ElementId = string.IsNullOrEmpty (request.ElementId)
            ? ElementNamingConvention!.GetName (request.HolderType(), request.Accessor)
            : request.ElementId;

        var tag = Builder.Build (request);
        request.ReplaceTag (tag);

        _modifiers.Each (m => m.Modify (request));

        return request.CurrentTag;
    }

    #endregion
}
