// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CsCodeHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Reflection;
using System.CodeDom.Compiler;

using Microsoft.CSharp;

using AM.Reporting.Data;

#endregion

#nullable enable

namespace AM.Reporting.Code;

internal class CsCodeHelper
    : CodeHelperBase
{
    #region Construction

    public CsCodeHelper
        (
            Report report
        )
        : base (report)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private methods

    private string GetEquivalentKeyword
        (
            string keyword
        )
    {
        if (keyword.EndsWith ("[]"))
        {
            return GetEquivalentTypeName (keyword.Substring (0, keyword.Length - 2)) + "[]";
        }

        return GetEquivalentTypeName (keyword);
    }

    private string GetEquivalentTypeName
        (
            string keyword
        )
    {
        Sure.NotNullNorEmpty (keyword);

        return keyword switch
        {
            "Object" => "object",
            "String" => "string",
            "Char" => "char",
            "Byte" => "byte",
            "SByte" => "sbyte",
            "Int16" => "short",
            "UInt16" => "ushort",
            "Int32" => "int",
            "UInt32" => "uint",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Single" => "float",
            "Double" => "double",
            "Decimal" => "decimal",
            "Boolean" => "bool",
            _ => keyword
        };
    }

    #endregion

    #region Protected methods

    protected override string GetTypeDeclaration
        (
            Type type
        )
    {
        Sure.NotNull (type);

        if (type.IsGenericType)
        {
            var result = type.Name;
            result = result.Substring (0, result.IndexOf ('`'));
            result += "<";

            foreach (var elementType in type.GetGenericArguments())
            {
                result += GetTypeDeclaration (elementType) + ",";
            }

            result = result.Substring (0, result.Length - 1) + ">";
            return result;
        }
        else
        {
            return type.Name;
        }
    }

    #endregion

    #region Public methods

    public override string EmptyScript()
    {
        return "using System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\n" +
               "using System.ComponentModel;\r\nusing System.Windows.Forms;\r\nusing System.Drawing;\r\n" +
               "using System.Data;\r\nusing AM.Reporting;\r\nusing AM.Reporting.Data;\r\nusing AM.Reporting.Dialog;\r\n" +
               "using AM.Reporting.Barcode;\r\nusing AM.Reporting.Table;\r\nusing AM.Reporting.Utils;\r\n\r\n" +
               "namespace AM.Reporting\r\n{\r\n  public class ReportScript\r\n  {\r\n  }\r\n}\r\n";
    }

    public override int GetPositionToInsertOwnItems
        (
            string scriptText
        )
    {
        Sure.NotNull (scriptText);

        var pos = scriptText.IndexOf ("public class ReportScript", StringComparison.Ordinal);
        if (pos == -1)
        {
            return -1;
        }

        return scriptText.IndexOf ('{', pos) + 3;
    }

    public override string AddField
        (
            Type type,
            string name
        )
    {
        Sure.NotNull (type);
        Sure.NotNull (name);

        name = name.Replace (" ", "_");
        return "    public " + type.FullName + " " + name + ";\r\n";
    }

    public override string BeginCalcExpression()
    {
        return "    private object CalcExpression(string expression, Variant Value)\r\n    {\r\n      ";
    }

    public override string AddExpression
        (
            string expr,
            string value
        )
    {
        Sure.NotNull (expr);
        Sure.NotNull (value);

        expr = expr.Replace ("\\", "\\\\");
        expr = expr.Replace ("\"", "\\\"");
        return "if (expression == \"" + expr + "\")\r\n        return " + value + ";\r\n      ";
    }

    public override string EndCalcExpression()
    {
        return "return null;\r\n    }\r\n\r\n";
    }

    public override string ReplaceColumnName
        (
            string name,
            Type type
        )
    {
        Sure.NotNull (name);
        Sure.NotNull (type);

        var typeName = GetTypeDeclaration (type);
        var result = "((" + typeName + ")Report.GetColumnValue(\"" + name + "\"";
        result += "))";
        return result;
    }

    public override string ReplaceParameterName
        (
            Parameter parameter
        )
    {
        Sure.NotNull (parameter);

        var typeName = GetTypeDeclaration (parameter.DataType);
        return "((" + typeName + ")Report.GetParameterValue(\"" + parameter.FullName + "\"))";
    }

    public override string ReplaceVariableName
        (
            Parameter parameter
        )
    {
        Sure.NotNull (parameter);

        var typeName = GetTypeDeclaration (parameter.DataType);
        return "((" + typeName + ")Report.GetVariableValue(\"" + parameter.FullName + "\"))";
    }

    public override string ReplaceTotalName
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        return "Report.GetTotalValue(\"" + name + "\")";
    }

    public override string GenerateInitializeMethod()
    {
        var events = new Hashtable();
        var reportString = StripEventHandlers (events);
        var result = "";

        // form the InitializeComponent method
        result += "    private void InitializeComponent()\r\n    {\r\n      ";

        // form the reportString
        result += "string reportString = \r\n        ";

        var totalLength = 0;
        while (reportString.Length > 0)
        {
            string part;
            if (reportString.Length > 80)
            {
                part = reportString.Substring (0, 80);
                reportString = reportString.Substring (80);
            }
            else
            {
                part = reportString;
                reportString = "";
            }

            part = part.Replace ("\\", "\\\\");
            part = part.Replace ("\"", "\\\"");
            part = part.Replace ("\r", "\\r");
            part = part.Replace ("\n", "\\n");
            result += "\"" + part + "\"";
            if (reportString != "")
            {
                if (totalLength > 1024)
                {
                    totalLength = 0;
                    result += ";\r\n      reportString += ";
                }
                else
                {
                    result += " +\r\n        ";
                }

                totalLength += part.Length;
            }
            else
            {
                result += ";\r\n      ";
            }
        }

        result += "LoadFromString(reportString);\r\n      ";
        result += "InternalInit();\r\n      ";

        // form objects' event handlers
        foreach (DictionaryEntry entry in events)
        {
            result += entry.Key + " += " + entry.Value + ";\r\n      ";
        }

        result += "\r\n    }\r\n\r\n";
        result += "    public ReportScript()\r\n    {\r\n      InitializeComponent();\r\n    }\r\n";
        return result;
    }

    public override string ReplaceClassName
        (
            string scriptText,
            string className
        )
    {
        Sure.NotNullNorEmpty (scriptText);
        Sure.NotNullNorEmpty (className);

        return scriptText.Replace ("class ReportScript", "class " + className + " : Report").Replace (
            "public ReportScript()", "public " + className + "()").Replace (
            "private object CalcExpression", "protected override object CalcExpression");
    }

    public override string GetMethodSignature
        (
            MethodInfo info,
            bool fullForm
        )
    {
        Sure.NotNull (info);

        var result = info.Name + "(";
        const string fontBegin = "<font color=\"Blue\">";
        const string fontEnd = "</font>";
        if (fullForm)
        {
            result = fontBegin + GetEquivalentKeyword (info.ReturnType.Name) + fontEnd + " " + result;
        }

        var pars = info.GetParameters();
        foreach (var par in pars)
        {
            // special case - skip "thisReport" parameter
            if (par.Name == "thisReport")
            {
                continue;
            }

            var paramType = "";
            var attr = par.GetCustomAttributes (typeof (ParamArrayAttribute), false);
            if (attr.Length > 0)
            {
                paramType = "params ";
            }

            paramType += GetEquivalentKeyword (par.ParameterType.Name);
            result += (fullForm ? fontBegin : "") + paramType + (fullForm ? fontEnd : "");
            result += (fullForm ? " " + par.Name : "");
            if (par.IsOptional && fullForm)
            {
                result += CodeUtils.GetOptionalParameter(par, CodeUtils.Language.Cs);
            }

            result += ", ";
        }

        if (result.EndsWith (", "))
        {
            result = result.Substring (0, result.Length - 2);
        }

        result += ")";
        return result;
    }

    public override string GetMethodSignatureAndBody
        (
            MethodInfo info
        )
    {
        Sure.NotNull (info);

        var result = info.Name + "(";
        result = "    private " + GetTypeDeclaration (info.ReturnType) + " " + result;

        var pars = info.GetParameters();
        foreach (var par in pars)
        {
            // special case - skip "thisReport" parameter
            if (par.Name == "thisReport")
            {
                continue;
            }

            var paramType = "";
            var attr = par.GetCustomAttributes (typeof (ParamArrayAttribute), false);
            if (attr.Length > 0)
            {
                paramType = "params ";
            }

            paramType += GetTypeDeclaration (par.ParameterType);
            result += paramType;
            result += " " + par.Name;
            if (par.IsOptional)
            {
                result += CodeUtils.GetOptionalParameter(par, CodeUtils.Language.Cs);
            }

            result += ", ";
        }

        if (result.EndsWith (", "))
        {
            result = result.Substring (0, result.Length - 2);
        }

        result += ")";

        result += "\r\n";
        result += "    {\r\n";
        result += "      return " + info.ReflectedType!.Namespace + "." +
                  info.ReflectedType.Name + "." + info.Name + "(";

        foreach (var par in pars)
        {
            var parName = par.Name;

            // special case - handle "thisReport" parameter
            if (parName == "thisReport")
            {
                parName = "Report";
            }

            result += parName + ", ";
        }

        if (result.EndsWith (", "))
        {
            result = result.Substring (0, result.Length - 2);
        }

        result += ");\r\n";
        result += "    }\r\n";
        result += "\r\n";

        return result;
    }


    public override CodeDomProvider GetCodeProvider()
    {
        return new CSharpCodeProvider();
    }

    #endregion
}
