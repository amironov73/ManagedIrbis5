// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TypeDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// These classes are required for correct data binding to Text property of FastColoredTextbox
/// </summary>
internal class FctbDescriptionProvider
    : TypeDescriptionProvider
{
    public FctbDescriptionProvider (Type type)
        : base (GetDefaultTypeProvider (type))
    {
        // пустое тело конструктора
    }

    private static TypeDescriptionProvider GetDefaultTypeProvider (Type type)
    {
        return TypeDescriptor.GetProvider (type);
    }


    public override ICustomTypeDescriptor GetTypeDescriptor (Type objectType, object? instance)
    {
        var defaultDescriptor = base.GetTypeDescriptor (objectType, instance);
        return new FctbTypeDescriptor (defaultDescriptor!, instance!);
    }
}

class FctbTypeDescriptor : CustomTypeDescriptor
{
    ICustomTypeDescriptor parent;
    object instance;

    public FctbTypeDescriptor (ICustomTypeDescriptor parent, object instance)
        : base (parent)
    {
        this.parent = parent;
        this.instance = instance;
    }

    public override string? GetComponentName()
    {
        var ctrl = (instance as Control);
        return ctrl?.Name;
    }

    /// <inheritdoc cref="CustomTypeDescriptor.GetEvents()"/>
    public override EventDescriptorCollection GetEvents()
    {
        var coll = base.GetEvents();
        var list = new EventDescriptor[coll.Count];

        for (var i = 0; i < coll.Count; i++)
            if (coll[i]!.Name == "TextChanged") //instead of TextChanged slip BindingTextChanged for binding
            {
                list[i] = new FooTextChangedDescriptor (coll[i]!);
            }
            else
            {
                list[i] = coll[i]!;
            }

        return new EventDescriptorCollection (list);
    }
}

class FooTextChangedDescriptor : EventDescriptor
{
    public FooTextChangedDescriptor (MemberDescriptor desc)
        : base (desc)
    {
    }

    public override void AddEventHandler (object component, Delegate value)
    {
        ((SyntaxTextBox) component).BindingTextChanged += value as EventHandler;
    }

    public override Type ComponentType => typeof (SyntaxTextBox);

    public override Type EventType => typeof (EventHandler);

    public override bool IsMulticast => true;

    public override void RemoveEventHandler (object component, Delegate value)
    {
        (component as SyntaxTextBox)!.BindingTextChanged -= value as EventHandler;
    }
}
