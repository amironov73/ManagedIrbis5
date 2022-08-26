// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CopyCallbackReason.cs -- причина вызова CopyProgressRoutine
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Причина вызова CopyProgressRoutine.
/// </summary>
public enum CopyCallbackReason
{
    /// <summary>
    /// Была скопирована другая часть файла данных.
    /// </summary>
    CALLBACK_CHUNK_FINISHED = 0x00000000,

    /// <summary>
    /// Был создан другой поток, который будет скопирован.
    /// Это причина обратного вызова, указанная при первом
    /// вызове процедуры обратного вызова.
    /// </summary>
    CALLBACK_STREAM_SWITCH = 0x00000001
}
