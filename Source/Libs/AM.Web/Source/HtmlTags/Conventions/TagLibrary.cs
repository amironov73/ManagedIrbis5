// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TagLibrary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public interface ITagLibrary
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="profile"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    ITagPlan PlanFor
        (
            ElementRequest subject,
            string? profile = null,
            string? category = null
        );
}

/// <summary>
///
/// </summary>
public class TagLibrary
    : ITagBuildingExpression, ITagLibrary
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public CategoryExpression Always => _categories[TagConstants.Default].Always;

    /// <summary>
    /// Access to the default category
    /// </summary>
    public TagCategory Default => _categories[TagConstants.Default];

    #endregion

    #region ITagLibrary members

    /// <summary>
    ///
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="profile"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    public ITagPlan PlanFor
        (
            ElementRequest subject,
            string? profile = null,
            string? category = null
        )
    {
        profile ??= TagConstants.Default;
        category ??= TagConstants.Default;

        return _categories[category].PlanFor (subject, profile);
    }

    #endregion

    #region Private members

    private readonly Cache<string, TagCategory> _categories =
        new (_ => new TagCategory());

    #endregion

    #region ITagBuilding members

    /// <inheritdoc cref="ITagBuildingExpression.If"/>
    public CategoryExpression If
        (
            Func<ElementRequest, bool> matches
        )
    {
        Sure.NotNull (matches);

        return _categories[TagConstants.Default].If (matches);
    }

    /// <inheritdoc cref="ITagBuildingExpression.Add(System.Func{AM.HtmlTags.Conventions.ElementRequest,bool},AM.HtmlTags.Conventions.ITagBuilder)"/>
    public void Add
        (
            Func<ElementRequest, bool> filter,
            ITagBuilder builder
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (builder);

        Add (new ConditionalTagBuilderPolicy (filter, builder));
    }

    /// <inheritdoc cref="ITagBuildingExpression.Add(AM.HtmlTags.Conventions.ITagBuilderPolicy)"/>
    public void Add
        (
            ITagBuilderPolicy policy
        )
    {
        Sure.NotNull (policy);

        Default.Add (policy);
    }

    /// <inheritdoc cref="ITagBuildingExpression.Add(AM.HtmlTags.Conventions.ITagModifier)"/>
    public void Add
        (
            ITagModifier modifier
        )
    {
        Sure.NotNull (modifier);

        Default.Add (modifier);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// The modifiers and builders for a category of conventions
    /// </summary>
    /// <param name="category">Example:  "Label", "Editor", "Display"</param>
    /// <returns></returns>
    public TagCategory Category
        (
            string category
        )
    {
        Sure.NotNullNorEmpty (category);

        return _categories[category];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    public BuilderSet BuilderSetFor
        (
            string? category = null,
            string? profile = null
        )
    {
        profile ??= TagConstants.Default;
        category ??= TagConstants.Default;

        return _categories[category].Profile (profile);
    }

    /// <summary>
    /// Add builders and modifiers by profile
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    public ITagBuildingExpression ForProfile (string profile)
    {
        return _categories[TagConstants.Default].ForProfile (profile);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    public void Import
        (
            TagLibrary other
        )
    {
        Sure.NotNull (other);

        var keys = _categories.GetKeys()
            .Union (other._categories.GetKeys()).Distinct();

        keys.Each (key => _categories[key].Import (other._categories[key]));
    }

    #endregion
}
