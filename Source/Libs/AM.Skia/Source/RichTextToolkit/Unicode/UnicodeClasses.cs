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

#nullable enable

namespace AM.Skia.RichTextKit
{
    /// <summary>
    /// Helper for looking up unicode character class information
    /// </summary>
    internal static class UnicodeClasses
    {
        static UnicodeClasses()
        {
            // Load trie resources
            _bidiTrie = new UnicodeTrie(typeof(LineBreaker).Assembly.GetManifestResourceStream("AM.Skia.RichTextKit.Resources.BidiClasses.trie"));
            _classesTrie = new UnicodeTrie(typeof(LineBreaker).Assembly.GetManifestResourceStream("AM.Skia.RichTextKit.Resources.LineBreakClasses.trie"));
            _boundaryTrie = new UnicodeTrie(typeof(LineBreaker).Assembly.GetManifestResourceStream("AM.Skia.RichTextKit.Resources.WordBoundaryClasses.trie"));
            _graphemeTrie= new UnicodeTrie(typeof(LineBreaker).Assembly.GetManifestResourceStream("AM.Skia.RichTextKit.Resources.GraphemeClusterClasses.trie"));
        }

        static UnicodeTrie _bidiTrie;
        static UnicodeTrie _classesTrie;
        static UnicodeTrie _boundaryTrie;
        static UnicodeTrie _graphemeTrie;

        /// <summary>
        /// Get the directionality of a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's directionality</returns>
        public static Directionality Directionality(int codePoint)
        {
            return (Directionality)(_bidiTrie.Get(codePoint) >> 24);
        }

        /// <summary>
        /// Get the directionality of a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's directionality</returns>
        public static uint BidiData(int codePoint)
        {
            return _bidiTrie.Get(codePoint);
        }

        /// <summary>
        /// Get the bracket type for a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's paired bracked type</returns>
        public static PairedBracketType PairedBracketType(int codePoint)
        {
            return (PairedBracketType)((_bidiTrie.Get(codePoint) >> 16) & 0xFF);
        }

        /// <summary>
        /// Get the associated bracket type for a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's opposite bracket, or 0 if not a bracket</returns>
        public static int AssociatedBracket(int codePoint)
        {
            return (int)(_bidiTrie.Get(codePoint) & 0xFFFF);
        }

        /// <summary>
        /// Get the line break class for a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's line break class</returns>
        public static LineBreakClass LineBreakClass(int codePoint)
        {
            return (LineBreakClass)_classesTrie.Get(codePoint);
        }

        /// <summary>
        /// Get the line break class for a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's line break class</returns>
        public static WordBoundaryClass BoundaryGroup(int codePoint)
        {
            return (WordBoundaryClass)_boundaryTrie.Get(codePoint);
        }

        /// <summary>
        /// Get the grapheme cluster class for a Unicode Code Point
        /// </summary>
        /// <param name="codePoint">The code point in question</param>
        /// <returns>The code point's grapheme cluster class</returns>
        public static GraphemeClusterClass GraphemeClusterClass(int codePoint)
        {
            return (GraphemeClusterClass)_graphemeTrie.Get(codePoint);
        }
    }
}
