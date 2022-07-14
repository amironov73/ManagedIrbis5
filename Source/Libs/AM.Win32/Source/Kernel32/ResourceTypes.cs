// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ResourceTypes.cs -- типы ресурсов приложений WIN32
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Типы ресурсов приложений WIN32..
/// </summary>
public enum ResourceTypes
{
    /// <summary>
    /// Зависящий от устройства курсор.
    /// </summary>
    RT_CURSOR = 1,

    /// <summary>
    /// Растровый ресурс.
    /// </summary>
    RT_BITMAP = 2,

    /// <summary>
    /// Зависящая от устройства иконка.
    /// </summary>
    RT_ICON = 3,

    /// <summary>
    /// Меню.
    /// </summary>
    RT_MENU = 4,

    /// <summary>
    /// Диалоговое окно.
    /// </summary>
    RT_DIALOG = 5,

    /// <summary>
    /// Запись таблицы строк.
    /// </summary>
    RT_STRING = 6,

    /// <summary>
    /// Каталог шрифтов.
    /// </summary>
    RT_FONTDIR = 7,

    /// <summary>
    /// Шрифт.
    /// </summary>
    RT_FONT = 8,

    /// <summary>
    /// Таблица акселератора.
    /// </summary>
    RT_ACCELERATOR = 9,

    /// <summary>
    /// Специфичный для приложения ресурс (сырые данные,
    /// не интерпретируемые Windows).
    /// </summary>
    RT_RCDATA = 10,

    /// <summary>
    /// Запись таблицы сообщений.
    /// </summary>
    RT_MESSAGETABLE = 11,

    /// <summary>
    /// Не зависящий от устройства курсор.
    /// </summary>
    RT_GROUP_CURSOR = 12,

    /// <summary>
    /// Не зависящая от устройства иконка.
    /// </summary>
    RT_GROUP_ICON = 14,

    /// <summary>
    /// Информация о версии.
    /// </summary>
    RT_VERSION = 16,

    /// <summary>
    /// Включение заголовочного файла.
    /// <para>Allows a resource editing tool to associate a string
    /// with an .rc file. Typically, the string is the name of the
    /// header file that provides symbolic names. The resource
    /// compiler parses the string but otherwise ignores the value.
    /// For example,</para>
    /// <code>1 DLGINCLUDE "foo.h"</code>
    /// </summary>
    RT_DLGINCLUDE = 17,

    /// <summary>
    /// Ресурс Plug and Play.
    /// </summary>
    RT_PLUGPLAY = 19,

    /// <summary>
    /// VXD (драйвер устройства).
    /// </summary>
    RT_VXD = 20,

    /// <summary>
    /// Анимированный курсор.
    /// </summary>
    RT_ANICURSOR = 21,

    /// <summary>
    /// Анимированная иконка.
    /// </summary>
    RT_ANIICON = 22,

    /// <summary>
    /// HTML.
    /// </summary>
    RT_HTML = 23,

    /// <summary>
    /// Начиная с Microsoft® Windows® XP: параллельный (side-by-side)
    /// XML-манифест сборки.
    /// </summary>
    RT_MANIFEST = 24
}
