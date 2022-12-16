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

public interface IElementTagOverride
{
    string Category { get; }
    string Profile { get; }
    IElementBuilder Builder();
}

public class ElementTagOverride<T> : IElementTagOverride where T : IElementBuilder, new()
{
    public ElementTagOverride (string category, string profile)
    {
        Category = category ?? TagConstants.Default;
        Profile = profile ?? TagConstants.Default;
    }

    public string Category { get; }
    public string Profile { get; }
    public IElementBuilder Builder() => new T();
}
