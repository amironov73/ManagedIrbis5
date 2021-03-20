// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlainTextDriver.cs --
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
    public sealed class PlainTextDriver
        : TextDriver
    {
        #region Properties

        /// <summary>
        /// Name of the driver.
        /// </summary>
        public override string Name => "Plain text";

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextDriver
            (
                PftOutput output
            )
            : base(output)
        {
        }

        #endregion

        #region TextDriver members

        /// <inheritdoc/>
        public override TextDriver Bold
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Italic
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver NewParagraph()
        {
            Output.WriteLine();

            return this;
        }

        /// <inheritdoc/>
        public override TextDriver Underline
            (
                string text
            )
        {
            Output.Write(text);

            return this;
        }

        #endregion

    }
}
