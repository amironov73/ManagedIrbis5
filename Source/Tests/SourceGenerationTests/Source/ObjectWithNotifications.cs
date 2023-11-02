// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ObjectWithNotifications.cs -- канареечный класс для генерации INotifyPropertyChanged
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.SourceGeneration;

#endregion

namespace SourceGenerationTests;

/// <summary>
/// Канареечный класс для генерации INotifyPropertyChanged.
/// </summary>
internal partial class ObjectWithNotifications
{
    [Notify] private string? _name;
    [Notify] private int _age;

    public override string ToString() => $"{Name} of age {Age}";
}
