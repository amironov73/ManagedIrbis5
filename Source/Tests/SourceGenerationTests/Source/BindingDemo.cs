// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BindingDemo.cs -- канареечный класс для генерации привязок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.SourceGeneration;

#endregion

namespace SourceGenerationTests;

/// <summary>
/// Канареечный класс для генерации приязок
/// </summary>
internal partial class BindingDemo
{
    /// <summary>
    /// Некое свойство, нуждающееся в привязке.
    /// </summary>
    [Binding]
    public string? Title { get; set; }

    public void DoBinding()
    {
        var binding = TitleBinding();
        binding.Source = this;
        Console.WriteLine ($"Binding for {binding.Path} of {binding.Source}");
    }

    public override string ToString() => "Demo";
}
