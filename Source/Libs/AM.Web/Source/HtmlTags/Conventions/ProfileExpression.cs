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

namespace AM.HtmlTags.Conventions;

using Elements;

public class ProfileExpression
{
    protected HtmlConventionLibrary Library { get; set; }
    private readonly string _profileName;

    public ProfileExpression (HtmlConventionLibrary library, string profileName)
    {
        Library = library;
        _profileName = profileName;
    }

    private BuilderSet BuildersFor (string category) =>
        Library.TagLibrary.Category (category).Profile (_profileName);

    public ElementCategoryExpression Labels => new (BuildersFor (ElementConstants.Label));

    public ElementCategoryExpression Displays => new (BuildersFor (ElementConstants.Display));

    public ElementCategoryExpression Editors => new (BuildersFor (ElementConstants.Editor));

    public ElementCategoryExpression ValidationMessages => new (BuildersFor (ElementConstants.ValidationMessage));

    public void Apply (HtmlConventionLibrary library) => library.Import (Library);
}
