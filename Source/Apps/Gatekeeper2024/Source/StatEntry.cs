// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StatEntry.cs -- элемент статистики
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Элемент статистики.
/// </summary>
[PublicAPI]
internal sealed class StatEntry
{
    #region Properties

    /// <summary>
    /// Заголовок элемента.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Значение элемента
    /// </summary>
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Данные для тестирования.
    /// </summary>
    public static StatEntry[] FakeEntries() => new StatEntry[]
    {
        new () { Title = "Зашло", Value = "1234" },
        new () { Title = "Вышло", Value = "1230" },
        new () { Title = "В библиотеке", Value = "4" },
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeWithReadableCyrillic (this);

    #endregion
}
