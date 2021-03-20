// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Text
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TextDriver
    {
        #region Properties

        /// <summary>
        /// Name of the driver.
        /// </summary>
        public virtual string Name => "None";

        /// <summary>
        /// Output.
        /// </summary>
        public PftOutput Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected TextDriver
            (
                PftOutput output
            )
        {
            Output = output;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Bold face.
        /// </summary>
        public virtual TextDriver Bold
            (
                string text
            )
        {
            return this;
        }

        /// <summary>
        /// Italic.
        /// </summary>
        public virtual TextDriver Italic
            (
                string text
            )
        {
            return this;
        }

        /// <summary>
        /// New paragraph.
        /// </summary>
        public virtual TextDriver NewParagraph()
        {
            return this;
        }

        /// <summary>
        /// Underline.
        /// </summary>
        public virtual TextDriver Underline
            (
                string text
            )
        {
            return this;
        }

        #endregion
    }
}
