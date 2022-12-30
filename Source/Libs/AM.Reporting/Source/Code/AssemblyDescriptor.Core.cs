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

using System.Collections.Specialized;
using System.CodeDom.Compiler;

#endregion

#nullable enable

namespace AM.Reporting.Code
{
    partial class AssemblyDescriptor
    {
        partial void ErrorMsg (CompilerError ce, int number);

        partial void ErrorMsg (string str, CompilerError ce);

        partial void ErrorMsg (string str);

        partial void ReviewReferencedAssemblies (StringCollection referencedAssemblies);
    }
}
