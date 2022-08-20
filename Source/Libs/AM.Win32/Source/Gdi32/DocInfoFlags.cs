// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DocInfoFlags.cs -- содержит дополнительную информацию о задании на печать
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Содержит дополнительную информацию о задании на печать.
/// </summary>
[Flags]
public enum DocInfoFlags
{
    /// <summary>
    /// Приложения, использующие печать по полосам, должны установить
    /// этот флаг для оптимальной производительности во время печати.
    /// </summary>
    DI_APPBANDING = 0x00000001,

    /// <summary>
    /// Приложение будет использовать растровые операции, включающие
    /// чтение с целевой поверхности.
    /// </summary>
    DI_ROPS_READ_DESTINATION = 0x00000002
}
