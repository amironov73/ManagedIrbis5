using System.Collections.Specialized;
using System.CodeDom.Compiler;

namespace FastReport.Code
{
    partial class AssemblyDescriptor
    {
        partial void ErrorMsg(CompilerError ce, int number);

        partial void ErrorMsg(string str, CompilerError ce);

        partial void ErrorMsg(string str);

        partial void ReviewReferencedAssemblies(StringCollection referencedAssemblies);
    }
}
