// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Base class for report pages and dialog forms.
    /// </summary>
    public abstract partial class PageBase : ComponentBase
    {
        #region Fields

        private string pageName;

        #endregion

        #region Properties

        internal string PageName
        {
            get
            {
                if (!string.IsNullOrEmpty (pageName))
                {
                    return pageName;
                }

                return Name;

                //string pageName = Name;
                //if (pageName.StartsWith(BaseName))
                //  pageName = pageName.Replace(BaseName, Res.Get("Objects," + ClassName));

                //return pageName;
            }
            set => pageName = value;
        }

        internal bool NeedRefresh { get; set; }

        internal bool NeedModify { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Causes the page to refresh in the preview window.
        /// </summary>
        /// <remarks>
        /// Call this method when you handle object's MouseMove, MouseDown, MouseUp, MouseEnter, MouseLeave events
        /// and want to refresh the preview window.
        /// <note type="caution">
        /// If you have changed some objects on a page, the <b>Refresh</b> method will not save the changes.
        /// This means when you print or export the page, you will see original (unmodified) page content.
        /// If you want to save the changes, you have to use the <see cref="Modify"/> method instead.
        /// </note>
        /// </remarks>
        public void Refresh()
        {
            NeedRefresh = true;
        }

        /// <summary>
        /// Modifies the page content and refresh it in the preview window.
        /// </summary>
        /// <remarks>
        /// Call this method when you handle object's Click, MouseDown or MouseUp events
        /// and want to modify an object and refresh the preview window.
        /// </remarks>
        public void Modify()
        {
            NeedModify = true;
            NeedRefresh = true;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PageBase"/> class with default settings.
        /// </summary>
        public PageBase()
        {
            SetFlags (Flags.CanMove | Flags.CanResize | Flags.CanDelete | Flags.CanChangeOrder |
                      Flags.CanChangeParent | Flags.CanCopy, false);
        }
    }
}
