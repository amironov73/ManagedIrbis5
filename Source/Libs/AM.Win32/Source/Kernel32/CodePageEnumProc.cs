// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CodePageEnumProc.cs -- делегат для перечисления кодовых страниц
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Делегат для перечисления кодовых страниц, установленных в системе.
/// </summary>
public delegate bool CodePageEnumProc
    (
        string codePage
    );
