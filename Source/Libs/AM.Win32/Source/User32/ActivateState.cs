// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ActivateState.cs -- флаги для оконного сообщения WM_ACTIVATE
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Флаги для оконного сообщения WM_ACTIVATE (младшее слово wparam).
/// </summary>
public enum ActivateState
    : ushort
{
    /// <summary>
    /// Окно было деактивировано.
    /// </summary>
    WA_INACTIVE = 0,

    /// <summary>
    /// Окно было активировано не щелчком мыши, а другим способом,
    /// например, вызовом функции <c>SetActiveWindow</c>.
    /// </summary>
    WA_ACTIVE = 1,

    /// <summary>
    /// Окно было активировано щелчком мыши.
    /// </summary>
    WA_CLICKACTIVE = 2
}
