// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Among.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.AOT.Stemming
{
    internal class Among
    {
        public readonly int s_size; /* search string */
        public readonly char[] s; /* search string */
        public readonly int substring_i; /* index to longest matching substring */
        public readonly int result; /* result of the lookup */
        public delegate bool boolDel();
        public readonly boolDel? method; /* method to use if substring matches */

        public Among(string s, int substring_i, int result, boolDel? linkMethod)
        {
            s_size = s.Length;
            this.s = s.ToCharArray();
            this.substring_i = substring_i;
            this.result = result;
            method = linkMethod;
        }
    }
}
