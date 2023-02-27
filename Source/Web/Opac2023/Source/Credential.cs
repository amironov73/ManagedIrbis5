// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.ComponentModel.DataAnnotations;

#endregion

namespace Opac2023;

/// <summary>
/// Учетные данные для каталога.
/// </summary>
public sealed class OpacCredential
{
    [Required]
    [Display (Name="Логин")]
    public string? UserName { get; set; }

    [Required]
    [Display (Name="Пароль")]
    public string? Password { get; set; }

    [Display (Name="Запомнить меня на этом компьютере")]
    public bool RememberMe { get; set; }
}
