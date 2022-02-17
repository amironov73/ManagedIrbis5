// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* WslDistribution.cs -- информация о дистрибутиве WSL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.Win32;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Информация о дистрибутиве WSL.
/// </summary>
public sealed class WslDistribution
{
    #region Properties

    /// <summary>
    /// GUID.
    /// </summary>
    public string? Guid { get; set; }

    /// <summary>
    /// Базовый путь.
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// Переменные окружения по умолчанию.
    /// </summary>
    public string? DefaultEnvironment { get; set; }

    /// <summary>
    /// Идентификатор пользователя по умолчанию.
    /// </summary>
    public int DefaultUid { get; set; }

    /// <summary>
    /// Имя дистрибутива.
    /// </summary>
    public string? DistributionName { get; set; }

    /// <summary>
    /// Управляющие флаги.
    /// </summary>
    public int Flags { get; set; }

    /// <summary>
    /// Командная строка для ядра.
    /// </summary>
    public string? KernelCommandLine { get; set; }

    /// <summary>
    /// Семейство пакетов.
    /// </summary>
    public string? PackageFamilyName { get; set; }

    /// <summary>
    /// Состояние.
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public int Version { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор информации из ключа реестра.
    /// </summary>
    public void Parse
        (
            RegistryKey key
        )
    {
        Guid = key.Name;
        BasePath = (string?)key.GetValue ("BasePath");
        DefaultEnvironment = (string?)key.GetValue ("DefaultEnvironment");
        DefaultUid = (int)key.GetValue ("DefaultUid", 0)!;
        DistributionName = (string?)key.GetValue ("DistributionName");
        Flags = (int)key.GetValue ("Flags", 0)!;
        KernelCommandLine = (string?)key.GetValue ("KernelCommandLine");
        PackageFamilyName = (string?)key.GetValue ("PackageFamilyName");
        State = (int)key.GetValue ("State", 0)!;
        Version = (int)key.GetValue ("version", 0)!;
    }

    #endregion
}

