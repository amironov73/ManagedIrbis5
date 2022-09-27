// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IIrbisFormsTest.cs -- общий интерфейс тестов для визуальных компонентов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace IrbisFormsTests;

/// <summary>
/// Общий интерфейс тестов для визуальных компонентов ИРБИС (WinForms).
/// </summary>
public interface IIrbisFormsTest
{
    /// <summary>
    /// Запуск теста.
    /// </summary>
    public void RunTest (IWin32Window? ownerWindow);
}
