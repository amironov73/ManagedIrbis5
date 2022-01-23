// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IFakecontrol.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
///
/// </summary>
public interface IFakeControl
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    event EventHandler<TransfromNeededEventArg>? TransformNeeded;

    /// <summary>
    ///
    /// </summary>
    event EventHandler<PaintEventArgs>? FramePainting;

    /// <summary>
    ///
    /// </summary>
    event EventHandler<PaintEventArgs>? FramePainted;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    Bitmap? BgBmp { get; set; }

    /// <summary>
    ///
    /// </summary>
    Bitmap? Frame { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="animatedControl"></param>
    /// <param name="padding"></param>
    void InitParent (Control animatedControl, Padding padding);

    #endregion
}
