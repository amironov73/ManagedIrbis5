// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Person.cs -- канареечный класс для генерации Direct-свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;

using AM.Avalonia.SourceGeneration;

#endregion

namespace SourceGenerationTests;

/// <summary>
/// Канареечный класс для генерации Direct-свойств.
/// </summary>
internal partial class Person
    : AvaloniaObject
{
    [DirectProperty] private string? _name;
    [DirectProperty] private int _age;

    public override string ToString() => $"{Name} of age {Age}";
}
