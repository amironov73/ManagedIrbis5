// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* MultipleBinding.cs -- несколько байндингов сгруппированных как один
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Data
{
    //
    // Заимствовано из проекта Praeclarum.Bind
    //
    // https://raw.githubusercontent.com/praeclarum/Bind/master/src/Bind.cs
    //
    // Copyright 2013-2014 Frank A. Krueger
    //

    /// <summary>
    /// Несколько байндингов сгруппированных как один,
    /// для упрощения добавления и удаления.
    /// </summary>
    internal sealed class MultipleBindings
        : EasyBinding
    {
        readonly List<EasyBinding> bindings;

        public MultipleBindings
            (
                IEnumerable<EasyBinding> bindings
            )
        {
            this.bindings = bindings.ToList ();
        }

        public override void Unbind ()
        {
            base.Unbind ();
            foreach (var b in bindings)
            {
                b.Unbind ();
            }
            bindings.Clear ();
        }

    } // class MultipleBindings

} // namespace AM.Data
