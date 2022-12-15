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
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor
{
    /// <summary>
    /// Interface implemented by views of a TextDocument
    /// </summary>
    public interface ITextDocumentView
    {
        /// <summary>
        /// Notifies that the view needs to be reset, typically because
        /// the entire content has been reloaded or updated
        /// </summary>
        void OnReset();

        /// <summary>
        /// Notifies that something other than the content of the document
        /// has changed (eg: margins) and the view needs to be redrawn but
        /// the same selection can be maintained
        /// </summary>
        void OnRedraw();

        /// <summary>
        /// Notifies that the document is about to change
        /// </summary>
        /// <param name="view">The view initiating the change</param>
        void OnDocumentWillChange(ITextDocumentView view);

        /// <summary>
        /// Notifies a view that the document has changed and provides
        /// information about which parts of the document were changed.
        /// </summary>
        /// <param name="view">The view initiating the change</param>
        /// <param name="info">Information about the change</param>
        void OnDocumentChange(ITextDocumentView view, DocumentChangeInfo info);

        /// <summary>
        /// Notifies that the document has finished changing
        /// </summary>
        /// <param name="view">The view initiating the change</param>
        void OnDocumentDidChange(ITextDocumentView view);
    }
}
