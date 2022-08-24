// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* LoadResourceFlags.cs -- флаги для функции LoadImage и аналогичных
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги для функции LoadImage и аналогичных функций.
/// </summary>
[Flags]
public enum LoadResourceFlags
{
    /// <summary>
    /// Значение по умолчанию; не делает ничего.
    /// По факту означает "не LR_MONOCHROME".
    /// </summary>
    LR_DEFAULTCOLOR = 0x0000,

    /// <summary>
    /// Загрузка монохромного изображения.
    /// </summary>
    LR_MONOCHROME = 0x0001,

    /// <summary>
    /// ???
    /// </summary>
    LR_COLOR = 0x0002,

    /// <summary>
    /// ???
    /// </summary>
    LR_COPYRETURNORG = 0x0004,

    /// <summary>
    /// ???
    /// </summary>
    LR_COPYDELETEORG = 0x0008,

    /// <summary>
    ///Загружает изображение из файла, указанного параметром lpszName.
    /// Если этот флаг не указан, lpszName — это имя ресурса.
    /// </summary>
    LR_LOADFROMFILE = 0x0010,

    /// <summary>
    /// <para>Получает значение цвета первого пикселя изображения
    /// и заменяет соответствующую запись в таблице цветов цветом
    /// окна по умолчанию (COLOR_WINDOW). Все пиксели изображения,
    /// использующие эту запись, становятся цветом окна по умолчанию.
    /// Это значение применяется только к изображениям с соответствующими
    /// таблицами цветов.</para>
    /// <para>Не используйте этот параметр, если вы загружаете растровое
    /// изображение с глубиной цвета более 8 бит на пиксель.</para>
    /// </summary>
    LR_LOADTRANSPARENT = 0x0020,

    /// <summary>
    /// Использует ширину или высоту, указанные значениями системных
    /// метрик для курсоров или значков, если значения cxDesired
    /// или cyDesired установлены равными нулю. Если этот флаг
    /// не указан, а cxDesired и cyDesired равны нулю, функция
    /// использует фактический размер ресурса. Если ресурс содержит
    /// несколько изображений, функция использует размер первого
    /// изображения.
    /// </summary>
    LR_DEFAULTSIZE = 0x0040,

    /// <summary>
    /// Uses true VGA colors.
    /// </summary>
    LR_VGACOLOR = 0x0080,

    /// <summary>
    /// Ищет изображение в таблице цветов и заменяет оттенки серого
    /// соответствующим трехмерным цветом.
    /// </summary>
    LR_LOADMAP3DCOLORS = 0x1000,

    /// <summary>
    /// Когда параметр uType указывает IMAGE_BITMAP, функция возвращает
    /// растровое изображение раздела DIB, а не совместимое растровое
    /// изображение. Этот флаг полезен для загрузки растрового изображения
    /// без сопоставления его с цветами устройства отображения.
    /// </summary>
    LR_CREATEDIBSECTION = 0x2000,

    /// <summary>
    /// ???
    /// </summary>
    LR_COPYFROMRESOURCE = 0x4000,

    /// <summary>
    /// Делится дескриптором изображения, если изображение загружается
    /// несколько раз. Если LR_SHARED не установлен, второй вызов
    /// LoadImage для того же ресурса снова загрузит изображение
    /// и вернет другой дескриптор. Когда вы используете этот флаг,
    /// система уничтожит ресурс, когда он больше не нужен.
    /// </summary>
    LR_SHARED = 0x8000
}
