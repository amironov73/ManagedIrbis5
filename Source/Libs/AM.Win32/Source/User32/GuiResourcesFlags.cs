// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* GuiResourcesFlags.cs -- тип GUI-объекта
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Тип GUI-объекта.
/// </summary>
public enum GuiResourcesFlags
{
    /// <summary>
    /// Количество GDI-объектов.
    /// </summary>
    GR_GDIOBJECTS = 0,

    /// <summary>
    /// Количество USER-объектов.
    /// </summary>
    GR_USEROBJECTS = 1
}
