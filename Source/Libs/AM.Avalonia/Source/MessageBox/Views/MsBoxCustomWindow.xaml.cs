using Avalonia.Markup.Xaml;
using AM.Avalonia.BaseWindows.Base;

namespace AM.Avalonia.Views;

public class MsBoxCustomWindow
    : BaseWindow, IWindowGetResult<string>
{
    public MsBoxCustomWindow() : base()
    {
        InitializeComponent();
    }

    public string ButtonResult { get; set; } = null;

    public string GetResult() => ButtonResult;


    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
