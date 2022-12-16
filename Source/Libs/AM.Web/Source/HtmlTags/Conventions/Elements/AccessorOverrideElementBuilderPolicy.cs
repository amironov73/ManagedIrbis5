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

namespace AM.HtmlTags.Conventions.Elements;

using System.Linq;

using Reflection;

public class AccessorOverrideElementBuilderPolicy : IElementBuilderPolicy
{
    private readonly AccessorRules _rules;
    private readonly string _category;
    private readonly string _profile;

    public AccessorOverrideElementBuilderPolicy (AccessorRules rules, string category, string profile)
    {
        _rules = rules;
        _category = category;
        _profile = profile;
    }

    public bool Matches (ElementRequest subject)
    {
        return _rules.AllRulesFor<IElementTagOverride> (subject.Accessor)
            .Any (x => x.Category == _category && x.Profile == _profile);
    }

    public ITagBuilder BuilderFor (ElementRequest subject)
    {
        return
            _rules.AllRulesFor<IElementTagOverride> (subject.Accessor)
                .First (x => x.Category == _category && x.Profile == _profile)
                .Builder();
    }
}
