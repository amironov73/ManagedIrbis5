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

namespace AM.Reporting.Utils
{
    internal class MyEncodingInfo
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Properties

        public int CodePage { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public MyEncodingInfo (Encoding encoding)
        {
            DisplayName = encoding.EncodingName;
            Name = encoding.WebName;
            CodePage = encoding.CodePage;
        }

        public MyEncodingInfo (EncodingInfo info)
        {
            DisplayName = info.DisplayName;
            Name = info.Name;
            CodePage = info.CodePage;
        }

        #endregion Public Constructors

        #region Public Methods

        public static IEnumerable<MyEncodingInfo> GetEncodings()
        {
            List<MyEncodingInfo> encodings = new List<MyEncodingInfo>();

            foreach (var info in Encoding.GetEncodings())
            {
                encodings.Add (new MyEncodingInfo (info));
            }

            encodings.Sort (new Comparison<MyEncodingInfo> (compareEncoding));
            ;

            return encodings;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        #endregion Public Methods

        #region Private Methods

        private static int compareEncoding (MyEncodingInfo x, MyEncodingInfo y)
        {
            if (x != null && y != null)
            {
                return string.Compare (x.DisplayName, y.DisplayName);
            }

            return 0;
        }

        #endregion Private Methods
    }
}
