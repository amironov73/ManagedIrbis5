// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* GlobalState.cs -- хранение глобального состояния
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Хранение глобального состояния.
/// </summary>
[PublicAPI]
internal sealed class GlobalState
{
    #region Properties

    /// <summary>
    /// Логгер чисто для нас.
    /// </summary>
    public static ILogger Logger { get; set; } = null!;

    /// <summary>
    /// Общий экземпляр.
    /// </summary>
    public static readonly GlobalState Instance = new ();

    /// <summary>
    /// Сообщение.
    /// </summary>
    [JsonPropertyName ("message")]
    public string? Message { get; set; } = "Пока никаких событий не происходило";

    #endregion

    #region Private members

    private static readonly object _syncObject = new ();

    #endregion

    #region Public methods

    public static void Update
        (
            GlobalState newState
        )
    {
        lock (_syncObject)
        {
            // TODO реализовать
        }
    }

    #endregion
}
