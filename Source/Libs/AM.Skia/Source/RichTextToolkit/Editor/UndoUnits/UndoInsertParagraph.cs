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

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor.UndoUnits
{
    class UndoInsertParagraph : UndoUnit<TextDocument>
    {
        public UndoInsertParagraph(int index, Paragraph paragraph)
        {
            _index = index;
            _paragraph = paragraph;
        }

        public override void Do(TextDocument context)
        {
            context._paragraphs.Insert(_index, _paragraph);
        }

        public override void Undo(TextDocument context)
        {
            context._paragraphs.RemoveAt(_index);
        }

        int _index;
        Paragraph _paragraph;
    }
}
