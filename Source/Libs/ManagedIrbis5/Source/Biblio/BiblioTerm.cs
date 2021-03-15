﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BiblioTerm.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BiblioTerm
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public TermCollection Dictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Extended title.
        /// </summary>
        [CanBeNull]
        public string Extended { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public string Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [CanBeNull]
        public BiblioItem Item { get; set; }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify(bool throwOnError)
        {
            Verifier<BiblioTerm> verifier
                = new Verifier<BiblioTerm>(this, throwOnError);

            // TODO do something

            return verifier.Result;
        }

        #endregion
    }
}
