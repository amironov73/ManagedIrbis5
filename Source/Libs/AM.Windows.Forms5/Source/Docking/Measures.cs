// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Docking
{
    public class Measures
    {
        public int SplitterSize = 4;
        public int AutoHideSplitterSize = 4;
        public int AutoHideTabLineWidth = 6;
        public int DockPadding { get; set; }
    }

    internal static class MeasurePane
    {
        public const int MinSize = 24;
    }
}
