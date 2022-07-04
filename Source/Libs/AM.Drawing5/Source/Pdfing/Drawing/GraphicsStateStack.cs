// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* GraphicsStateStack.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Represents a stack of XGraphicsState and XGraphicsContainer objects.
    /// </summary>
    internal class GraphicsStateStack
    {
        public GraphicsStateStack(XGraphics gfx)
        {
            _current = new InternalGraphicsState(gfx);
        }

        public int Count
        {
            get { return _stack.Count; }
        }

        public void Push(InternalGraphicsState state)
        {
            _stack.Push(state);
            state.Pushed();
        }

        public int Restore(InternalGraphicsState state)
        {
            if (!_stack.Contains(state))
                throw new ArgumentException("State not on stack.", "state");
            if (state.Invalid)
                throw new ArgumentException("State already restored.", "state");

            int count = 1;
            InternalGraphicsState top = _stack.Pop();
            top.Popped();
            while (top != state)
            {
                count++;
                state.Invalid = true;
                top = _stack.Pop();
                top.Popped();
            }
            state.Invalid = true;
            return count;
        }

        public InternalGraphicsState Current
        {
            get
            {
                if (_stack.Count == 0)
                    return _current;
                return _stack.Peek();
            }
        }

        readonly InternalGraphicsState _current;

        readonly Stack<InternalGraphicsState> _stack = new Stack<InternalGraphicsState>();
    }
}