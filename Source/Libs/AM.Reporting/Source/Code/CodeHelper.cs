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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.CodeDom.Compiler;

using AM.Reporting.Data;
using AM.Reporting.Engine;
using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Code
{
  internal abstract partial class CodeHelperBase
  {
    #region Fields
    private Report report;
    #endregion

    #region Properties
    public Report Report
    {
      get { return report; }
    }
    #endregion

    #region Protected Methods
    protected string StripEventHandlers(Hashtable events)
    {
      using (Report report = new Report())
      {
        report.LoadFromString(Report.SaveToString());
        report.ScriptText = EmptyScript();

        List<Base> list = new List<Base>();
        foreach (Base c in report.AllObjects)
        {
          list.Add(c);
        }
        list.Add(report);

        foreach (Base c in list)
        {
          PropertyInfo[] props = c.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
          foreach (PropertyInfo info in props)
          {
            if (info.PropertyType == typeof(string) && info.Name.EndsWith("Event"))
            {
              string value = (string)info.GetValue(c, null);
              if (!String.IsNullOrEmpty(value))
              {
                string cName = c.Name + ".";
                if (c is Report)
                  cName = "";
                events.Add(cName + info.Name.Replace("Event", ""), value);
                info.SetValue(c, "", null);
              }
            }
          }
        }

        return report.SaveToString();
      }
    }

    protected abstract string GetTypeDeclaration(Type type);
    #endregion


    #region Public Methods
    public abstract string EmptyScript();
    public abstract CodeDomProvider GetCodeProvider();
    public abstract int GetPositionToInsertOwnItems(string scriptText);
    public abstract string AddField(Type type, string name);
    public abstract string BeginCalcExpression();
    public abstract string AddExpression(string expr, string value);
    public abstract string EndCalcExpression();
    public abstract string ReplaceColumnName(string name, Type type);
    public abstract string ReplaceParameterName(Parameter parameter);
    public abstract string ReplaceVariableName(Parameter parameter);
    public abstract string ReplaceTotalName(string name);
    public abstract string GenerateInitializeMethod();
    public abstract string ReplaceClassName(string scriptText, string className);
    public abstract string GetMethodSignature(MethodInfo info, bool fullForm);
    public abstract string GetMethodSignatureAndBody(MethodInfo info);

#endregion

    public CodeHelperBase(Report report)
    {
            this.report = report;
    }

  }

}
