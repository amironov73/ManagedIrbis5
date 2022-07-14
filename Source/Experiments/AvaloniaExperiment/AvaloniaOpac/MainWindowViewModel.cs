using ReactiveUI;

namespace AvaloniaOpac;

public class MainWindowViewModel
    : ReactiveObject
{
    private string? _keyword;

    public string? Keyword
    {
        get => _keyword;
        set => this.RaiseAndSetIfChanged (ref _keyword, value, nameof (Keyword));
    }

    private bool _truncation;

    public bool Truncation
    {
        get => _truncation;
        set => this.RaiseAndSetIfChanged (ref _truncation, value, nameof (Truncation));
    }

    private string[]? _found;

    public string[]? Found
    {
        get => _found;
        set => this.RaiseAndSetIfChanged (ref _found, value, nameof (Found));
    }
}
