namespace Dialogs
{
  public class ButtonArgs
  {
    public bool CloseAfterClick { get; set; }

    public IButton Button { get; }

    public ButtonArgs(IButton button)
    {
      Button = button;
      CloseAfterClick = true;
    }
  }
}