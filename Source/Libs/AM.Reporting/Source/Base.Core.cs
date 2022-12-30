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

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class Base
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="macroValues"></param>
        /// <param name="text"></param>
        private string ExtractDefaultMacrosInternal(Dictionary<string, object> macroValues, string text)
        {
            return text;
        }
    }
}
