// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Department.cs -- канареечный класс для генерации Styled-свойств
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
internal partial class Department
    : AvaloniaObject
{
    [StyledProperty] private string? _title;
    [StyledProperty] private int _room;

    public override string ToString() => $"{Title} in room {Room}";
}
