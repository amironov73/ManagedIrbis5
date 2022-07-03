﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GraphicsStateSaver.cs -- simple holder of graphics context state
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Drawing;

/// <summary>
/// Holds state of <see cref="T:System.Drawing.Graphics"/>
/// class.
/// </summary>
public sealed class GraphicsStateSaver
    : IDisposable
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:GraphicsStateSaver"/> class.
    /// </summary>
    public GraphicsStateSaver
        (
            Graphics graphics
        )
    {
        _graphics = graphics;
        _state = _graphics.Save();
    }

    /// <summary>
    /// Releases unmanaged resources and performs
    /// other cleanup operations before the
    /// <see cref="T:AM.Drawing.GraphicsStateSaver"/>
    /// is reclaimed by garbage collection.
    /// </summary>
    ~GraphicsStateSaver()
    {
        Dispose (false);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Object of <see cref="T:System.Drawing.Graphics"/> type
    /// which state have been saved.
    /// </summary>
    private readonly Graphics _graphics;

    /// <summary>
    /// Saved state itself.
    /// </summary>
    private GraphicsState? _state;

    /// <summary>
    /// Disposes the object.
    /// </summary>
    /// <param name="disposing">if set to <c>true</c>
    /// [disposing].</param>
    private void Dispose
        (
            bool disposing
        )
    {
        if (_state != null)
        {
            _graphics.Restore (_state);
            _state = null;
        }
    }

    #endregion

    #region IDisposable members

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    #endregion
}
