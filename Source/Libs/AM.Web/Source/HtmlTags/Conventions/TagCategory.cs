// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagCategory.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class TagCategory
    : ITagBuildingExpression
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public BuilderSet Defaults { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CategoryExpression Always => Defaults.Always;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public TagCategory()
    {
        _profiles[TagConstants.Default] = Defaults;
    }

    #endregion

    #region Private members

    private readonly Cache<string, BuilderSet> _profiles =
        new (_ => new BuilderSet());

    private TagPlan BuildPlan
        (
            TagSubject subject
        )
    {
        var sets = SetsFor (subject.Profile).ToList();
        var policy = sets.SelectMany (x => x.Policies)
            .FirstOrDefault (x => x.Matches (subject.Subject));
        if (policy == null)
        {
            throw new ArgumentOutOfRangeException ("Unable to select a TagBuilderPolicy for subject " + subject);
        }

        var builder = policy.BuilderFor (subject.Subject);

        var modifiers = sets.SelectMany (x => x.Modifiers).Where (x => x.Matches (subject.Subject));

        var elementNamingConvention = sets.Select (x => x.ElementNamingConvention).FirstOrDefault();

        return new TagPlan (builder, modifiers, elementNamingConvention);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public BuilderSet Profile
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return _profiles[name];
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <returns></returns>
    public TagPlan PlanFor
        (
            ElementRequest request,
            string? profile = null
        )
    {
        var subject = new TagSubject (profile, request);
        return BuildPlan (subject);
    }

    private IEnumerable<BuilderSet> SetsFor (string profile)
    {
        if (!string.IsNullOrEmpty (profile) && profile != TagConstants.Default)
        {
            yield return _profiles[profile];
        }

        yield return Defaults;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="builder"></param>
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

    /// <summary>
    ///
    /// </summary>
    /// <param name="policy"></param>
    public void Add
        (
            ITagBuilderPolicy policy
        )
    {
        Sure.NotNull (policy);

        _profiles[TagConstants.Default].Add (policy);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modifier"></param>
    public void Add
        (
            ITagModifier modifier
        )
    {
        Sure.NotNull (modifier);

        _profiles[TagConstants.Default].Add (modifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <returns></returns>
    public CategoryExpression If
        (
            Func<ElementRequest, bool> matches
        )
    {
        Sure.NotNull (matches);

        return Defaults.If (matches);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <returns></returns>
    public ITagBuildingExpression ForProfile
        (
            string profile
        )
    {
        Sure.NotNullNorEmpty (profile);

        return _profiles[profile];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    public void Import
        (
            TagCategory other
        )
    {
        Sure.NotNull (other);

        Defaults.Import (other.Defaults);

        var keys = _profiles.GetKeys().Union (other._profiles.GetKeys())
            .Where (x => x != TagConstants.Default)
            .Distinct();

        keys.Each (key => _profiles[key].Import (other._profiles[key]));
    }

    #endregion
}
