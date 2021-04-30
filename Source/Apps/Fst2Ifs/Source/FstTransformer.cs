// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FstTransformer.cs -- преобразователь FST в IFS
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace Fst2Ifs
{
    /// <summary>
    /// Преобразователь FST в IFS.
    /// </summary>
    public sealed class FstTransformer
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Input text stream.
        /// </summary>
        public TextReader In { get; set; }

        /// <summary>
        /// Output text stream.
        /// </summary>
        public TextWriter Out { get; set; }

        #endregion

        #region Construction

        public FstTransformer ( TextReader reader, TextWriter writer )
        {
            In = reader;
            Out = writer;
        }

        #endregion

        #region Private members

        private readonly Regex _lineMatcher = new ( @"^(\d+)\s+(\d+)" );
        private readonly Regex _itemFinder = new (@"[dvn](\d+)");

        #endregion

        #region Public methods

        /// <summary>
        /// Finds 'd', 'n' and 'v' references in the line.
        /// </summary>
        public void TransformLine ( string line )
        {
            line = line.Trim ();
            if ( string.IsNullOrEmpty ( line ) )
            {
                return;
            }

            Match lineMatch = _lineMatcher.Match ( line );
            if ( !lineMatch.Success )
            {
                Out.WriteLine(line);
                return;
            }

            Out.Write ( lineMatch.Groups[1].Value );

            List <string> found = new List < string > ();
            MatchCollection itemMatches = _itemFinder.Matches ( line );
            foreach ( Match itemMatch in itemMatches )
            {
                string item = itemMatch.Groups [ 1 ].Value;
                if ( !found.Contains ( item ) )
                {
                    found.Add ( item );
                }
            }

            foreach ( string item in found )
            {
                Out.Write ( ",{0}", item );
            }

            Out.Write ( " {0} ", lineMatch.Groups[2].Value );

            Out.WriteLine ( line.Substring ( lineMatch.Length ).TrimStart () );
        }

        /// <summary>
        /// Do the job: reads input line by line and transforms it
        /// into the IFS.
        /// </summary>
        public void TransformFile ()
        {
            string? line;

            while ( (line = In.ReadLine ()) != null )
            {
                TransformLine ( line );
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose ()
        {
            In.Dispose ();
            Out.Dispose ();
        }

        #endregion

    }
}
