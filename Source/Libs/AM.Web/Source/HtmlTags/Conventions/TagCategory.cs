// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

public class TagCategory : ITagBuildingExpression
{
    private readonly Cache<string, BuilderSet> _profiles =
        new (name => new BuilderSet());

    public TagCategory()
    {
        _profiles[TagConstants.Default] = Defaults;
    }

    public BuilderSet Defaults { get; } = new ();

    public BuilderSet Profile (string name) => _profiles[name];

    public TagPlan PlanFor (ElementRequest request, string profile = null)
    {
        var subject = new TagSubject (profile, request);
        return BuildPlan (subject);
    }

    private TagPlan BuildPlan (TagSubject subject)
    {
        var sets = SetsFor (subject.Profile).ToList();
        var policy = sets.SelectMany (x => x.Policies).FirstOrDefault (x => x.Matches (subject.Subject));
        if (policy == null)
        {
            throw new ArgumentOutOfRangeException ("Unable to select a TagBuilderPolicy for subject " + subject);
        }

        var builder = policy.BuilderFor (subject.Subject);

        var modifiers = sets.SelectMany (x => x.Modifiers).Where (x => x.Matches (subject.Subject));

        var elementNamingConvention = sets.Select (x => x.ElementNamingConvention).FirstOrDefault();

        return new TagPlan (builder, modifiers, elementNamingConvention);
    }

    private IEnumerable<BuilderSet> SetsFor (string profile)
    {
        if (!string.IsNullOrEmpty (profile) && profile != TagConstants.Default)
        {
            yield return _profiles[profile];
        }

        yield return Defaults;
    }

    public void Add (Func<ElementRequest, bool> filter, ITagBuilder builder) =>
        Add (new ConditionalTagBuilderPolicy (filter, builder));

    public void Add (ITagBuilderPolicy policy) => _profiles[TagConstants.Default].Add (policy);

    public void Add (ITagModifier modifier) => _profiles[TagConstants.Default].Add (modifier);


    public CategoryExpression Always => Defaults.Always;

    public CategoryExpression If (Func<ElementRequest, bool> matches) => Defaults.If (matches);

    public ITagBuildingExpression ForProfile (string profile) => _profiles[profile];

    public void Import (TagCategory other)
    {
        Defaults.Import (other.Defaults);

        var keys = _profiles.GetKeys().Union (other._profiles.GetKeys())
            .Where (x => x != TagConstants.Default)
            .Distinct();

        keys.Each (key => _profiles[key].Import (other._profiles[key]));
    }
}
