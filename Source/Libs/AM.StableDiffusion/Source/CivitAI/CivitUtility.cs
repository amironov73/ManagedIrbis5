// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CivitUtility.cs -- полезные методы для клиента CivitAI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Полезные методы для клиента CivitAI.
/// </summary>
[PublicAPI]
public static class CivitUtility
{
    #region Public methods

    /// <summary>
    /// Расщифровывает поле <c>nsfw</c>.
    /// </summary>
    public static bool IsNotSafe (string? value) =>
        !string.IsNullOrEmpty (value)
        && !value.IsOneOf ("false", "None", "0");

    #endregion
}
