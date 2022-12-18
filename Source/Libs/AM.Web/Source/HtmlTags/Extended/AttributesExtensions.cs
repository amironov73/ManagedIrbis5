// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AttributeExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.HtmlTags.Extended.Attributes;

/// <summary>
///
/// </summary>
public static class AttributesExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static THtmlTag UnEncoded<THtmlTag>
        (
            this THtmlTag tag
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull (tag);

        tag.Encoded (false).NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static THtmlTag Value<THtmlTag>
        (
            this THtmlTag tag,
            object value
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull (tag);

        tag.Attr ("value", value).NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Name<T>
        (
            this T tag,
            string name
        )
        where T: HtmlTag
    {
        Sure.NotNull (tag);
        Sure.NotNullNorEmpty (name);

        tag.Attr ("name", name).NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <typeparam name="THtmlTag"></typeparam>
    /// <returns></returns>
    public static THtmlTag MultilineMode<THtmlTag>
        (
            this THtmlTag tag
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull (tag);

        if (tag.HasAttr ("value"))
        {
            tag.Text (tag.Attr ("value")).NotUsed();
            tag.RemoveAttr ("value").NotUsed();
        }

        tag.TagName ("textarea").NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <typeparam name="THtmlTag"></typeparam>
    /// <returns></returns>
    public static THtmlTag NoAutoComplete<THtmlTag>
        (
            this THtmlTag tag
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull(tag);

        tag.Attr ("autocomplete", "off").NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <typeparam name="THtmlTag"></typeparam>
    /// <returns></returns>
    public static THtmlTag PasswordMode<THtmlTag>
        (
            this THtmlTag tag
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull (tag);

        tag.TagName ("input").Attr ("type", "password").NotUsed();
        tag.NoAutoComplete();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <typeparam name="THtmlTag"></typeparam>
    /// <returns></returns>
    public static THtmlTag FileUploadMode<THtmlTag>
        (
            this THtmlTag tag
        )
        where THtmlTag: HtmlTag
    {
        Sure.NotNull (tag);

        tag.Attr ("type", "file").NotUsed();
        tag.NoClosingTag().NotUsed();
        return tag;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="shouldDisplay"></param>
    /// <typeparam name="THtmlTag"></typeparam>
    /// <returns></returns>
    public static THtmlTag HideUnless<THtmlTag>
        (
            this THtmlTag tag,
            bool shouldDisplay
        )
        where THtmlTag: HtmlTag
    {
        if (!shouldDisplay)
        {
            tag.Style ("display", "none").NotUsed();
        }

        return tag;
    }

    #endregion
}


