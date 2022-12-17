// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IElementBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions.Elements;

/// <summary>
///
/// </summary>
public interface IElementBuilder
    : ITagBuilder
{
    // пустое тело интерфейса
}

/// <summary>
///
/// </summary>
public interface IElementBuilderPolicy
    : ITagBuilderPolicy
{
    // пустое тело интерфейса
}

/// <summary>
///
/// </summary>
public abstract class ElementTagBuilder
    : TagBuilder, IElementBuilderPolicy, IElementBuilder
{
    // пустое тело класса
}
