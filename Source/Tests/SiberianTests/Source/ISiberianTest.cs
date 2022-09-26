// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ISiberianTest.cs -- общий интерфейс тестов для SiberianGrid
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace SiberianTests;

/// <summary>
/// Общий интерфейс тестов для SiberianGrid.
/// </summary>
public interface ISiberianTest
{
    /// <summary>
    /// Запуск теста.
    /// </summary>
    void RunTest (IWin32Window? ownerWindow);
}
