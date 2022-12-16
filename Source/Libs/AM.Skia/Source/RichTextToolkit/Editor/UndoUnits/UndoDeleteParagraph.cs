// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UndoDeleteParagraph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor.UndoUnits;

internal class UndoDeleteParagraph
    : UndoUnit<TextDocument>
{
    public UndoDeleteParagraph (int index)
    {
        _index = index;
    }

    public override void Do (TextDocument context)
    {
        _paragraph = context._paragraphs[_index];
        context._paragraphs.RemoveAt (_index);
    }

    public override void Undo (TextDocument context)
    {
        context._paragraphs.Insert (_index, _paragraph!);
    }

    private int _index;
    private Paragraph? _paragraph;
}
