// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlainForm.cs -- простая форма
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.General;

/// <summary>
/// Простая форма, реализованная целиком
/// стандартными средствами WinForms.
/// </summary>
public class PlainForm
    : IGeneralContainer
{
    #region IGeneralContainer members

    /// <inheritdoc cref="IGeneralContainer.Toolbars"/>
    public IGeneralItemList Toolbars => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralContainer.MainMenu"/>
    public IGeneralItem MainMenu => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralContainer.StatusBar"/>
    public IGeneralItem StatusBar => throw new NotImplementedException();

    /// <inheritdoc cref="IGeneralContainer.WorkingArea"/>
    public Control WorkingArea => throw new NotImplementedException();

    #endregion
}
