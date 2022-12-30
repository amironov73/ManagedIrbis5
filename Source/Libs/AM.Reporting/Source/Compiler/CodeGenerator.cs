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

#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP
using System;
using System.Collections.Generic;
using System.Text;

namespace AM.Reporting.Code.CodeDom.Compiler
{
    public class CodeGenerator
    {
        public static bool IsValidLanguageIndependentIdentifier (string value)
        {
            //TODO o_0 what????
            return true;
        }
    }
}
#endif
