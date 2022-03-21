using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Dialogs.Buttons
{
  public class ButtonCollection : ReactiveObject, IButtonCollection
  {
    private readonly ICollection<IButton> buttons = new List<IButton>();

    public IButton DefaultButton
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
}