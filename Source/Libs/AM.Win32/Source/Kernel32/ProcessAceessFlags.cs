// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ProcessAccessFlags.cs -- флаги доступа к процессу
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги доступа к процессу.
/// </summary>
[Flags]
public enum ProcessAccessFlags
{
    /// <summary>
    /// Требуется для завершения процесса с помощью
    /// <c>TerminateProcess</c>.
    /// </summary>
    PROCESS_TERMINATE = 0x0001,

    /// <summary>
    /// Требуется для создания потока (thread).
    /// </summary>
    PROCESS_CREATE_THREAD = 0x0002,

    /// <summary>
    /// ???
    /// </summary>
    PROCESS_SET_SESSIONID = 0x0004,

    /// <summary>
    /// Требуется для выполнения операции над адресным пространством
    /// процесса (см. <c>VirtualProtectEx</c>
    /// и <c>WriteProcessMemory</c>).
    /// </summary>
    PROCESS_VM_OPERATION = 0x0008,

    /// <summary>
    /// Требуется для чтения памяти в процессе, использующем
    /// <c>ReadProcessMemory</c>.
    /// </summary>
    PROCESS_VM_READ = 0x0010,

    /// <summary>
    /// Требуется для записи в память в процессе, использующем
    /// <c>WriteProcessMemory</c>.
    /// </summary>
    PROCESS_VM_WRITE = 0x0020,

    /// <summary>
    /// Требуется для дублирования дескриптора с помощью
    /// <c>DuplicateHandle</c>.
    /// </summary>
    PROCESS_DUP_HANDLE = 0x0040,

    /// <summary>
    /// Требуется для создания процесса.
    /// </summary>
    PROCESS_CREATE_PROCESS = 0x0080,

    /// <summary>
    /// Требуется для установки пределов памяти с помощью
    /// <c>SetProcessWorkingSetSize</c>.
    /// </summary>
    PROCESS_SET_QUOTA = 0x0100,

    /// <summary>
    /// Требуется для установки определенной информации о процессе,
    /// например его класса приоритета (см. <c>SetPriorityClass</c>).
    /// </summary>
    PROCESS_SET_INFORMATION = 0x0200,

    /// <summary>
    /// Требуется для получения определенной информации о процессе,
    /// такой как его код выхода и класс приоритета
    /// (см. <c>GetExitCodeProcess</c> и <c>GetPriorityClass</c>).
    /// </summary>
    PROCESS_QUERY_INFORMATION = 0x0400,

    /// <summary>
    /// ???
    /// </summary>
    PROCESS_SUSPEND_RESUME = 0x0800,

    /// <summary>
    /// ???
    /// </summary>
    DELETE = 0x00010000,

    /// <summary>
    /// ???
    /// </summary>
    READ_CONTROL = 0x00020000,

    /// <summary>
    /// ???
    /// </summary>
    WRITE_DAC = 0x00040000,

    /// <summary>
    /// ???
    /// </summary>
    WRITE_OWNER = 0x00080000,

    /// <summary>
    /// Требуется дождаться завершения процесса с помощью
    /// функций ожидания.
    /// </summary>
    SYNCHRONIZE = 0x00100000,

    /// <summary>
    /// ???
    /// </summary>
    STANDARD_RIGHTS_REQUIRED = 0x000F0000,

    /// <summary>
    /// Все возможные права доступа для объекта процесса.
    /// </summary>
    PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF
}
