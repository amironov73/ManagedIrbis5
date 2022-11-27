// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

#pragma warning disable CA1069 // элемент перечисления имеет то же значение, что и другой элемент

/* DeviceOrientation.cs -- ориентация изображений или бумаге при выводе
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Ориентация изображений или бумаги при выводе изображений.
/// </summary>
public enum DeviceOrientation
{
    /// <summary>
    /// Портретная ориентация.
    /// </summary>
    DMORIENT_PORTRAIT = 1,

    /// <summary>
    /// Альбомная ориентация.
    /// </summary>
    DMORIENT_LANDSCAPE = 2,

    /// <summary>
    /// Ориентация дисплея — это естественная ориентация устройства
    /// отображения; режим следует использовать по умолчанию.
    /// </summary>
    DMDO_DEFAULT = 0,

    /// <summary>
    /// Ориентация дисплея -- повернутый на 90 градусов (по часовой
    /// стрелке) от DMDO_DEFAULT.
    /// </summary>
    DMDO_90 = 1,

    /// <summary>
    /// Ориентация дисплея -- повернутый на 180 градусов (по часовой
    /// стрелке) от DMDO_DEFAULT.
    /// </summary>
    DMDO_180 = 2,

    /// <summary>
    /// Ориентация дисплея -- повернутый на 270 градусов (по часовой
    /// стрелке) от DMDO_DEFAULT.
    /// </summary>
    DMDO_270 = 3
}
