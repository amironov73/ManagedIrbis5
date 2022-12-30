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
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace FastReport.Functions
{
    internal abstract class NumToLettersBase
    {
        #region Protected Methods
        protected string Str(int value, char[] letters)
        {
            if (value < 0) return "";

            int n = value;
            StringBuilder r = new StringBuilder();

            //if (minus)
            //    r.Insert(0, GetMinus() + " ");
            int letter;
            while (n >= letters.Length)
            {
                letter = n % letters.Length;
                r.Insert(0, letters[letter]);
                n /= letters.Length;
                if (n < letters.Length) --n;
            }
            r.Insert(0, letters[n]);

            return r.ToString();
        }
        #endregion

        #region Public Methods
        public abstract string ConvertNumber(int value, bool isUpper);
        #endregion
    }
}
