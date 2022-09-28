// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ClipboardChangedEventArgs.cs -- данные для события отслеживания изменений в буфере обмена
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Данные для события отслеживания изменений в буфере обмена.
/// </summary>
public sealed class ClipboardChangedEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Объект данных.
    /// </summary>
    public IDataObject? DataObject { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ClipboardChangedEventArgs
        (
            IDataObject? dataObject
        )
    {
        DataObject = dataObject;
    }

    #endregion
}
