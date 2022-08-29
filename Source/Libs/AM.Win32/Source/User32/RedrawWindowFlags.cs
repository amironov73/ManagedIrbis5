// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* RedrawWindowFlags.cs -- флаги для функции RedrawWindow
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги для функции <c>RedrawWindow</c>.
/// </summary>
[Flags]
public enum RedrawWindowFlags
{
    /// <summary>
    /// Делает недействительными <c>lprcUpdate</c>
    /// или <c>hrgnUpdate</c> (только одно из них может быть
    /// не <c>NULL</c>). Если оба имеют значение <c>NULL</c>,
    /// все окно становится недействительным.
    /// </summary>
    RDW_INVALIDATE = 0x0001,

    /// <summary>
    /// Вызывает отправку сообщения <c>WM_PAINT</c> в окно
    /// независимо от того, недействительна ли какая-либо часть окна.
    /// </summary>
    RDW_INTERNALPAINT = 0x0002,

    /// <summary>
    /// Заставляет окно получать сообщение <c>WM_ERASEBKGND</c>
    /// при перерисовке окна. Также должен быть указан флаг
    /// <c>RDW_INVALIDATE</c>; в противном случае
    /// <c>RDW_ERASE</c> не действует.
    /// </summary>
    RDW_ERASE = 0x0004,

    /// <summary>
    /// Валидирует <c>lprcUpdate</c> или <c>hrgnUpdate</c>
    /// (только один из них может быть не <c>NULL</c>).
    /// Если оба равны <c>NULL</c>, валидируется все окно.
    /// Этот флаг не влияет на внутренние сообщения <c>WM_PAINT</c>.
    /// </summary>
    RDW_VALIDATE = 0x0008,

    /// <summary>
    /// Подавляет любые ожидающие внутренние сообщения <c>WM_PAINT</c>.
    /// Этот флаг не влияет на сообщения WM_PAINT, полученные
    /// из области обновления, отличной от <c>NULL</c>.
    /// </summary>
    RDW_NOINTERNALPAINT = 0x0010,

    /// <summary>
    /// Подавляет любые ожидающие сообщения WM_ERASEBKGND.
    /// </summary>
    RDW_NOERASE = 0x0020,

    /// <summary>
    /// Исключает дочерние окна, если таковые имеются, из операции
    /// перерисовки.
    /// </summary>
    RDW_NOCHILDREN = 0x0040,

    /// <summary>
    /// Включает дочерние окна, если они есть, в операцию перерисовки.
    /// </summary>
    RDW_ALLCHILDREN = 0x0080,

    /// <summary>
    /// Заставляет затронутые окна (как указано флагами
    /// <c>RDW_ALLCHILDREN</c> и <c>RDW_NOCHILDREN</c>) получать
    /// сообщения <c>WM_NCPAINT</c>, <c>WM_ERASEBKGND</c>
    /// и <c>WM_PAINT</c>, если это необходимо, до возврата функции.
    /// </summary>
    RDW_UPDATENOW = 0x0100,

    /// <summary>
    /// Заставляет затронутые окна (как указано флагами
    /// <c>RDW_ALLCHILDREN</c> и <c>RDW_NOCHILDREN</c>) получать
    /// сообщения <c>WM_NCPAINT</c> и <c>WM_ERASEBKGND</c>,
    /// если это необходимо, до возврата функции.
    /// Сообщения <c>WM_PAINT</c> принимаются в обычное время.
    /// </summary>
    RDW_ERASENOW = 0x0200,

    /// <summary>
    /// Заставляет любую часть неклиентской области окна,
    /// пересекающую область обновления, получать сообщение
    /// <c>WM_NCPAINT</c>. Также должен быть указан флаг
    /// <c>RDW_INVALIDATE</c>; в противном случае <c>RDW_FRAME</c>
    /// не действует. Сообщение WM_NCPAINT обычно не отправляется
    /// во время выполнения <c>RedrawWindow</c>,
    /// если не указано либо <c>RDW_UPDATENOW</c>,
    /// либо <c>RDW_ERASENOW</c>.
    /// </summary>
    RDW_FRAME = 0x0400,

    /// <summary>
    /// Подавляет любые ожидающие сообщения <c>WM_NCPAINT</c>.
    /// Этот флаг должен использоваться с <c>RDW_VALIDATE</c>
    /// и обычно используется с <c>RDW_NOCHILDREN</c>.
    /// <c>RDW_NOFRAME</c> следует использовать с осторожностью,
    /// так как это может привести к тому, что части окна будут
    /// прорисованы неправильно.
    /// </summary>
    RDW_NOFRAME = 0x0800
}
