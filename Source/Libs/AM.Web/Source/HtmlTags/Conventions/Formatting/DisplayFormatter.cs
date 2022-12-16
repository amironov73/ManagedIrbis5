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

namespace AM.HtmlTags.Conventions.Formatting;

using System;

using Reflection;

public class DisplayFormatter : IDisplayFormatter
{
    private readonly Func<Type, object> _locator;
    private readonly Stringifier _stringifier;

    public DisplayFormatter (Func<Type, object> locator)
    {
        _locator = locator;
        _stringifier = new Stringifier();
    }

    public string GetDisplay (GetStringRequest request)
    {
        request.Locator = _locator;
        return _stringifier.GetString (request);
    }

    public string GetDisplay (Accessor accessor, object target)
    {
        var request = new GetStringRequest (accessor, target, _locator, null, null);
        return _stringifier.GetString (request);
    }

    public string GetDisplayForValue (Accessor accessor, object rawValue)
    {
        var request = new GetStringRequest (accessor, rawValue, _locator, null, null);
        return _stringifier.GetString (request);
    }
}
