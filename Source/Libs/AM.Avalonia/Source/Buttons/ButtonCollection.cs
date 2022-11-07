// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ButtonCollection.cs -- коллекция кнопок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

using ReactiveUI;

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia.Buttons;

/// <summary>
/// Коллекция кнопок.
/// </summary>
public class ButtonCollection
    : ReactiveObject, IButtonCollection
{
    private readonly ICollection<IButton> buttons = new List<IButton>();

    /// <summary>
    /// Кнопка по умолчанию.
    /// </summary>
    public IButton? DefaultButton
    {
        get { return buttons.SingleOrDefault(b => b.IsDefault); }
        set
        {
            ChangeDefault(value);
            this.RaisePropertyChanged();
        }
    }

    public IButton CancelButton
    {
        get { return buttons.SingleOrDefault(b => b.IsCancel); }
        set
        {
            ChangeCancel(value);
            this.RaisePropertyChanged();
        }
    }

    public IObservable<ButtonArgs> Clicked => clickedSubject;
    private readonly Subject<ButtonArgs> clickedSubject = new Subject<ButtonArgs>();

    public void AddButton(IButton button)
    {
        if (buttons.Contains(button))
            return;

        button.Clicked.Subscribe(args => clickedSubject.OnNext(args));

        if (button.IsDefault)
            ChangeDefault(button);

        if (button.IsCancel)
            ChangeCancel(button);

        buttons.Add(button);
    }

    public IButton AddButton(string buttonName)
    {
        var newButton = new Button();
        newButton.Name = buttonName;
        this.AddButton(newButton);
        return newButton;
    }

    public void AddOkCancel()
    {
        AddButton(DefaultButtons.OkButton);

        AddCancel();
    }

    public void AddCancel()
    {
        AddButton(DefaultButtons.CancelButton);
    }

    public IEnumerator<IButton> GetEnumerator()
    {
        return buttons.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) buttons).GetEnumerator();
    }

    public int Count
    {
        get { return buttons.Count; }
    }

    private void ChangeDefault(IButton newButton)
    {
        var old = buttons.SingleOrDefault(b => b.IsDefault);
        if (old != null)
            old.IsDefault = false;

        newButton.IsDefault = true;
    }

    private void ChangeCancel(IButton newButton)
    {
        var old = buttons.SingleOrDefault(b => b.IsCancel);
        if (old != null)
            old.IsCancel = false;

        newButton.IsCancel = true;
    }
}
