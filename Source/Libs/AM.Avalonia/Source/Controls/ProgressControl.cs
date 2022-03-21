namespace Dialogs.Controls
{
  public class ProgressControl : AbstractControl<double>
  {
    private double minValue;
    private double maxValue;

    public double MaxValue
    {
      get { return maxValue; }
      set
      {
        maxValue = value;
        OnPropertyChanged();
      }
    }

    public double MinValue
    {
      get { return minValue; }
      set
      {
        minValue = value;
        OnPropertyChanged();
      }
    }
  }
}