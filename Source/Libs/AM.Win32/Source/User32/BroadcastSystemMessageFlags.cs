// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BroadcastSystemMessageFlags.cs -- опции для рассылки широковещательных системных сообщений
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Опции для рассылки широковещательных системных сообщений.
/// </summary>
[Flags]
public enum BroadcastSystemMessageFlags
{
    /// <summary>
    /// Отправляет сообщение одному получателю за раз, отправляя
    /// следующему получателю, только если текущий получатель
    /// возвращает TRUE.
    /// </summary>
    BSF_QUERY = 0x00000001,

    /// <summary>
    /// Не отправляет сообщение окнам, принадлежащим текущей задаче.
    /// Это предотвращает получение приложением собственного сообщения.
    /// </summary>
    BSF_IGNORECURRENTTASK = 0x00000002,

    /// <summary>
    /// Сбрасывает дисковый кеш после обработки сообщения каждым получателем.
    /// </summary>
    BSF_FLUSHDISK = 0x00000004,

    /// <summary>
    /// Форсирует истечение времени ожидания на зависшем приложении.
    /// Если время ожидания одного из получателей истекло,
    /// не продолжает трансляцию сообщения.
    /// </summary>
    BSF_NOHANG = 0x00000008,

    /// <summary>
    /// Размещает сообщение (не дожидается начала актуальной отправки
    /// сообщения). Не используйте в сочетании с BSF_QUERY.
    /// </summary>
    BSF_POSTMESSAGE = 0x00000010,

    /// <summary>
    /// Продолжает транслировать сообщение, даже если время ожидания
    /// истекло или один из получателей завис.
    /// </summary>
    BSF_FORCEIFHUNG = 0x00000020,

    /// <summary>
    /// Ожидает ответа на сообщение, пока получатель не завис.
    /// Время ожидания никогда не истекает.
    /// </summary>
    BSF_NOTIMEOUTIFNOTHUNG = 0x00000040,

    /// <summary>
    /// Windows 2000/XP: позволяет получателю установить окно переднего
    /// плана при обработке сообщения.
    /// </summary>
    BSF_ALLOWSFW = 0x00000080,

    /// <summary>
    /// Windows 2000/XP: Отправляет сообщение с помощью функции
    /// SendNotifyMessage. Не используйте в сочетании с BSF_QUERY.
    /// </summary>
    BSF_SENDNOTIFYMESSAGE = 0x00000100,

    /// <summary>
    /// Назначение опции неизвестно.
    /// </summary>
    BSF_RETURNHDESK = 0x00000200,

    /// <summary>
    /// Назначение опции неизвестно.
    /// </summary>
    BSF_LUID = 0x00000400
}
