// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CopyProgressCode.cs -- значения, возвращаемые функцией CopyProgressRoutine
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Функция CopyProgressRoutine должна возвращать
/// одно из следующих значений.
/// </summary>
public enum CopyProgressCode
{
    /// <summary>
    /// Продолжать копирование файла.
    /// </summary>
    PROGRESS_CONTINUE = 0,

    /// <summary>
    /// Прекратить копирование и удалить целевой файл.
    /// </summary>
    PROGRESS_CANCEL = 1,

    /// <summary>
    /// Остановить копирование. Оно может быть перезапущено позже.
    /// </summary>
    PROGRESS_STOP = 2,

    /// <summary>
    /// Продолжать операцию копирования. но прекратить вызывать
    /// CopyProgressRoutine для отслеживания прогресса.
    /// </summary>
    PROGRESS_QUIET = 3
}
