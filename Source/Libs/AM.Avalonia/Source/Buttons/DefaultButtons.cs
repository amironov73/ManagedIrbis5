namespace Dialogs.Buttons
{
  public static class DefaultButtons
  {
    public static Button OkButton => new OkButtonImpl();

    public static Button CancelButton => new CancelButtonImpl();

    private class OkButtonImpl : Button
    {
      public OkButtonImpl()
      {
        Name = "Ok";
        IsDefault = true;
      }
    }

    private class CancelButtonImpl : Button
    {
      public CancelButtonImpl()
      {
        Name = "Cancel";
        IsCancel = true;
      }
    }
  }
}