// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DescriptionBase.cs -- база для описаний
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.Mastermind;

/// <summary>
/// База для описаний.
/// </summary>
[PublicAPI]
public class DescriptionBase
{
    #region Proprerties

    /// <summary>
    /// Заголовок, например, "Anna Netrebko".
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Имя файла, например, "AM110_Gracie.pt".
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Размер файла в байтах.
    /// </summary>
    public int? FileSize { get; set; }

    /// <summary>
    /// Описание в произвольном виде, например,
    /// "Anna Netrebko (aka Angelina Ballerina, Emilia,
    /// Gracie, Gracie A, Marion, Marion Y, Natalia,
    /// Vivian, Vivian B) is Ukrainian nude model since 2015".
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Пути к иллюстрирующим изображениям.
    /// </summary>
    public string[]? Illustrations { get; set; }


    #endregion
}
