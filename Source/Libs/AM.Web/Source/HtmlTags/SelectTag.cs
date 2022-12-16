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
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags;

public class SelectTag : HtmlTag
{
    private const string SelectedAttributeKey = "selected";

    public SelectTag()
        : base ("select")
    {
    }

    public SelectTag (Action<SelectTag> configure) : this()
    {
        configure (this);
    }

    public SelectTag TopOption (string display, object value, Action<HtmlTag> optionAction)
    {
        var option = TopOption (display, value);
        optionAction?.Invoke (option);
        return this;
    }

    public HtmlTag TopOption (string display, object value)
    {
        var option = MakeOption (display, value);
        InsertFirst (option);
        return option;
    }

    public HtmlTag Option (string display, object value)
    {
        var option = MakeOption (display, value);
        Append (option);
        return option;
    }

    public SelectTag DefaultOption (string display)
    {
        var option = TopOption (display, "");
        MarkOptionAsSelected (option);

        return this;
    }

    private static HtmlTag MakeOption (string display, object value) =>
        new HtmlTag ("option").Text (display).Attr ("value", value);

    public void SelectByValue (object value)
    {
        var valueToMatch = value.ToString();
        var child = Children.FirstOrDefault (x => x.Attr ("value").Equals (valueToMatch));

        if (child != null)
        {
            MarkOptionAsSelected (child);
        }
    }

    private void MarkOptionAsSelected (HtmlTag optionTag)
    {
        var prevSelected = Children.FirstOrDefault (x => x.HasAttr (SelectedAttributeKey));

        prevSelected?.RemoveAttr (SelectedAttributeKey);

        optionTag.Attr (SelectedAttributeKey, SelectedAttributeKey);
    }
}
