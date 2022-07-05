// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TextSegment.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.Layout
{
	public class TextSegment
	{
		public XFont Font { get; set; }
		public XBrush Brush { get; set; }
		public string Text { get; set; }
		public double LineIndent { get; set; }
		public bool SkipParagraphAlignment { get; set; }

		public double LineSpace { get; set; }
		public double CyAscent { get; set; }
		public double CyDescent { get; set; }
		public double SpaceWidth { get; set; }
	}
}