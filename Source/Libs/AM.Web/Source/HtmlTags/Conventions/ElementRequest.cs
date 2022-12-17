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

using System;

using Elements;

using Formatting;

using Reflection;

public class ElementRequest
{
    private bool _hasFetched;
    private object _rawValue;
    private Func<Type, object> _services;

    public ElementRequest (Accessor accessor)
    {
        Accessor = accessor;
    }

    public object RawValue
    {
        get
        {
            if (!_hasFetched)
            {
                _rawValue = Model == null ? null : Accessor.GetValue (Model);
                _hasFetched = true;
            }

            return _rawValue;
        }
    }

    public string ElementId { get; set; }
    public object Model { get; set; }
    public Accessor Accessor { get; }
    public HtmlTag OriginalTag { get; private set; }
    public HtmlTag CurrentTag { get; private set; }

    public void WrapWith (HtmlTag tag)
    {
        CurrentTag.WrapWith (tag);
        ReplaceTag (tag);
    }

    public void ReplaceTag (HtmlTag tag)
    {
        if (OriginalTag == null)
        {
            OriginalTag = tag;
        }

        CurrentTag = tag;
    }

    public AccessorDef ToAccessorDef() => new (Accessor, HolderType());


    public Type HolderType() => Model == null ? Accessor.DeclaringType : Model?.GetType();

    public T Get<T>() => (T)_services (typeof (T));

    public bool TryGet<T> (out T service) => (service = (T)_services (typeof (T))) != null;

    // virtual for mocking
    public virtual HtmlTag BuildForCategory (string category, string? profile = null) =>
        Get<ITagGenerator>().Build (this, category, profile);

    public T Value<T>() => (T)RawValue;

    public string StringValue() =>
        new DisplayFormatter (_services).GetDisplay (new GetStringRequest (Accessor, RawValue, _services, null,
            null));

    public bool ValueIsEmpty() => RawValue == null || string.Empty.Equals (RawValue);

    public void ForValue<T> (Action<T> action)
    {
        if (ValueIsEmpty())
        {
            return;
        }

        action ((T)RawValue);
    }

    public void Attach (Func<Type, object> locator) => _services = locator;

    public ElementRequest ToToken() => new (Accessor);
}
