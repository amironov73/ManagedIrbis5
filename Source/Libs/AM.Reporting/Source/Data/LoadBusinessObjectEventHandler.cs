// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LoadBusinessObjectEventHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents the method that will handle the LoadBusinessObject event.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="eventArgs">The event data.</param>
public delegate void LoadBusinessObjectEventHandler
    (
        object sender,
        LoadBusinessObjectEventArgs eventArgs
    );
