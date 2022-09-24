// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IFormsTest.cs -- общий интерфейс теста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

/// <summary>
/// Оьщий интерфейс теста.
/// </summary>
public interface IFormsTest
{
    /// <summary>
    /// Запуск теста.
    /// </summary>
    void RunTest (IWin32Window? ownerWindow);
}
