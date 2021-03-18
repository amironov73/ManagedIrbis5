// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IFormatExit.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// General format exit
    /// </summary>

    public interface IFormatExit
    {
        /// <summary>
        /// Name of the format exit.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Execute the expression on given context.
        /// </summary>
        void Execute
            (
                PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            );
    }
}
