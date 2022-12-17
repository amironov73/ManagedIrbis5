// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AccessorOverrideElementBuilderPolicy.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
[PublicAPI]
public class AccessorOverrideElementBuilderPolicy
    : IElementBuilderPolicy
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="rules"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    public AccessorOverrideElementBuilderPolicy
        (
            AccessorRules rules,
            string category,
            string profile
        )
    {
        Sure.NotNull (rules);
        Sure.NotNullNorEmpty (category);
        Sure.NotNullNorEmpty (profile);

        _rules = rules;
        _category = category;
        _profile = profile;
    }

    #endregion

    #region Private members

    private readonly AccessorRules _rules;

    private readonly string _category;

    private readonly string _profile;

    #endregion

    #region IElementBuilderPolicy members

    /// <inheritdoc cref="ITagBuilderPolicy.Matches"/>
    public bool Matches
        (
            ElementRequest subject
        )
    {
        Sure.NotNull (subject);

        return _rules.AllRulesFor<IElementTagOverride> (subject.Accessor)
            .Any (x => x.Category == _category && x.Profile == _profile);
    }

    /// <inheritdoc cref="ITagBuilderPolicy.BuilderFor"/>
    public ITagBuilder BuilderFor
        (
            ElementRequest subject
        )
    {
        Sure.NotNull (subject);

        return _rules.AllRulesFor<IElementTagOverride> (subject.Accessor)
                .First (x => x.Category == _category && x.Profile == _profile)
                .Builder();
    }

    #endregion
}
