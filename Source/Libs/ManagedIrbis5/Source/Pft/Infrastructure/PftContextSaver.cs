﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftContextSaver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Save state of <see cref="PftContext"/>.
    /// </summary>
    /// <remarks>
    /// Запоминает текущее повторение в группе, группу, поле и запись.
    /// </remarks>

    public sealed class PftContextSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Saved context.
        /// </summary>
        public PftContext Context { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContextSaver
            (
                PftContext context,
                bool clear
            )
        {
            Context = context;
            _index = context.Index;
            _currentGroup = context.CurrentGroup;
            _currentField = context.CurrentField;
            _record = context.Record;

            if (clear)
            {
                context.Index = 0;
                context.CurrentGroup = null;
                context.CurrentField = null;
            }
        }

        #endregion

        #region Private members

        private int _index;
        private PftGroup _currentGroup;
        private PftField _currentField;
        private Record _record;

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Context.Index = _index;
            Context.CurrentGroup = _currentGroup;
            Context.CurrentField = _currentField;
            Context.Record = _record;
        }

        #endregion
    }
}
