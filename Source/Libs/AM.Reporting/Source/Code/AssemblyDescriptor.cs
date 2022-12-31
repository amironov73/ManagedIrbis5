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
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

using AM.Reporting.Data;
using AM.Reporting.Engine;
using AM.Reporting.Utils;

#if SKIA
using HMACSHA1 = AM.Reporting.Utils.DetravHMACSHA1;
#endif

#endregion

namespace AM.Reporting.Code
{
    partial class AssemblyDescriptor
    {
        private static readonly ConcurrentDictionary<string, Assembly> FAssemblyCache;
        private readonly FastString scriptText;
        private readonly List<SourcePosition> sourcePositions;
        private int insertLine;
        private int insertPos;
        private bool needCompile;
        private const string shaKey = "FastReportCode";
        private readonly static object compileLocker;
        private readonly string currentFolder;

        public Assembly Assembly { get; private set; }

        public object Instance { get; private set; }

        public Report Report { get; }

        public Hashtable Expressions { get; }

        private void InsertItem (string text, string objName)
        {
            string[] lines = text.Split ('\r');
            scriptText.Insert (insertPos, text);
            var pos = new SourcePosition (objName, insertLine, insertLine + lines.Length - 2);
            sourcePositions.Add (pos);
            insertLine += lines.Length - 1;
            insertPos += text.Length;
        }

        private void InitField (string name, object c)
        {
            var info = Instance.GetType().GetField (name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            info.SetValue (Instance, c);
        }

        private void InitFields()
        {
            InitField ("Report", Report);
            InitField ("Engine", Report.Engine);
            var allObjects = Report.AllObjects;
            foreach (Base c in allObjects)
            {
                if (!string.IsNullOrEmpty (c.Name))
                {
                    InitField (c.Name, c);
                }
            }
        }

        private string GetErrorObjectName (int errorLine)
        {
            foreach (var pos in sourcePositions)
            {
                if (errorLine >= pos.start && errorLine <= pos.end)
                {
                    return pos.sourceObject;
                }
            }

            return "";
        }

        private int GetScriptLine (int errorLine)
        {
            var start = sourcePositions[0].start;
            var end = sourcePositions[sourcePositions.Count - 1].end;
            if (errorLine >= start && errorLine <= end)
            {
                return -1;
            }

            if (errorLine > end)
            {
                return errorLine - (end - start + 1);
            }

            return errorLine;
        }

        private string ReplaceDataItems (string expression)
        {
            var args = new FindTextArgs
            {
                Text = new FastString (expression),
                OpenBracket = "[",
                CloseBracket = "]"
            };

            while (args.StartIndex < args.Text.Length)
            {
                expression = CodeUtils.GetExpression (args, true);
                if (expression == null)
                {
                    break;
                }

                if (DataHelper.IsValidColumn (Report.Dictionary, expression))
                {
                    var type = DataHelper.GetColumnType (Report.Dictionary, expression);
                    expression = Report.CodeHelper.ReplaceColumnName (expression, type);
                }
                else if (DataHelper.IsValidParameter (Report.Dictionary, expression))
                {
                    expression =
                        Report.CodeHelper.ReplaceParameterName (DataHelper.GetParameter (Report.Dictionary,
                            expression));
                }
                else if (DataHelper.IsValidTotal (Report.Dictionary, expression))
                {
                    expression = Report.CodeHelper.ReplaceTotalName (expression);
                }
                else
                {
                    expression = "[" + ReplaceDataItems (expression) + "]";
                }

                args.Text = args.Text.Remove (args.StartIndex, args.EndIndex - args.StartIndex);
                args.Text = args.Text.Insert (args.StartIndex, expression);
                args.StartIndex += expression.Length;
            }

            return args.Text.ToString();
        }

        private bool ContainsAssembly (StringCollection assemblies, string assembly)
        {
            var asmName = Path.GetFileName (assembly);
            foreach (var a in assemblies)
            {
                var asmName1 = Path.GetFileName (a);
                if (string.Compare (asmName, asmName1, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddFastReportAssemblies (StringCollection assemblies)
        {
            foreach (var assembly in RegisteredObjects.Assemblies)
            {
                var aLocation = assembly.Location;
#if CROSSPLATFORM || COREWIN
                if (aLocation == "")
                {
                    // try fix SFA in AM.Reporting.Compat
                    string fixedReference = CodeDomProvider.TryFixReferenceInSingeFileApp(assembly);
                    if (!string.IsNullOrEmpty(fixedReference))
                        aLocation = fixedReference;
                }
#endif
                if (!ContainsAssembly (assemblies, aLocation))
                {
                    assemblies.Add (aLocation);
                }
            }
        }

        private void AddReferencedAssemblies (StringCollection assemblies, string defaultPath)
        {
            for (var i = 0; i < Report.ReferencedAssemblies.Length; i++)
            {
                var s = Report.ReferencedAssemblies[i];

#if CROSSPLATFORM
                if (s == "System.Windows.Forms.dll")
                    s = "AM.Reporting.Compat";
#endif

                // fix for old reports with "System.Windows.Forms.DataVisualization" in referenced assemblies
                if (s.IndexOf ("System.Windows.Forms.DataVisualization") != -1)
                {
                    s = "AM.Reporting.DataVisualization";
                }
#if SKIA
                if (s.IndexOf("AM.Reporting.Compat") != -1)
                    s = "AM.Reporting.Compat.Skia";
                if (s.IndexOf("AM.Reporting.DataVisualization") != -1)
                    s = "AM.Reporting.DataVisualization.Skia";
#endif

                AddReferencedAssembly (assemblies, defaultPath, s);
            }

#if SKIA
            AddReferencedAssembly(assemblies, defaultPath, "AM.Reporting.SkiaDrawing");
#endif

            // these two required for "dynamic" type support
            AddReferencedAssembly (assemblies, defaultPath, "System.Core");
            AddReferencedAssembly (assemblies, defaultPath, "Microsoft.CSharp");
        }

        private void AddReferencedAssembly (StringCollection assemblies, string defaultPath, string assemblyName)
        {
            var location = GetFullAssemblyReference (assemblyName, defaultPath);
            if (location != "" && !ContainsAssembly (assemblies, location))
            {
                assemblies.Add (location);
            }
        }

        private string GetFullAssemblyReference (string relativeReference, string defaultPath)
        {
            // in .NET Core we get the AssemblyReference in FR.Compat
#if !(CROSSPLATFORM || COREWIN)
            if (relativeReference == null || relativeReference.Trim() == "")
            {
                return "";
            }

            // Strip off any trailing ".dll" ".exe" if present.
            var dllName = relativeReference;
            if (string.Compare (relativeReference.Substring (relativeReference.Length - 4), ".dll", true) == 0 ||
                string.Compare (relativeReference.Substring (relativeReference.Length - 4), ".exe", true) == 0)
            {
                dllName = relativeReference.Substring (0, relativeReference.Length - 4);
            }

            // See if the required assembly is already present in our current AppDomain
            foreach (var currAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (string.Compare (currAssembly.GetName().Name, dllName, true) == 0)
                {
                    // Found it, return the location as the full reference.
                    return currAssembly.Location;
                }
            }

            // See if the required assembly is present in the ReferencedAssemblies but not yet loaded
            foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                if (string.Compare (assemblyName.Name, dllName, true) == 0)
                {
                    // Found it, try to load assembly and return the location as the full reference.
                    try
                    {
                        return Assembly.ReflectionOnlyLoad (assemblyName.FullName).Location;
                    }
                    catch
                    {
                    }
                }
            }

            // See if the required assembly is present locally
            var path = Path.Combine (defaultPath, relativeReference);
            if (File.Exists (path))
            {
                return path;
            }
#endif
            return relativeReference;
        }

        private void AddExpression (string expression, Base source, bool forceSimpleItems)
        {
            if (expression.Trim() == "" || Expressions.ContainsKey (expression))
            {
                return;
            }

            var expr = expression;
            if (expr.StartsWith ("[") && expr.EndsWith ("]"))
            {
                expr = expr.Substring (1, expr.Length - 2);
            }

            // skip simple items. Report.Calc does this.
            if (!forceSimpleItems)
            {
                if (DataHelper.IsSimpleColumn (Report.Dictionary, expr) ||
                    DataHelper.IsValidParameter (Report.Dictionary, expr) ||
                    DataHelper.IsValidTotal (Report.Dictionary, expr))
                {
                    return;
                }
            }

            // handle complex expressions, relations
            var descriptor = new ExpressionDescriptor (this);
            Expressions.Add (expression, descriptor);
            descriptor.MethodName = "CalcExpression";

            if (DataHelper.IsValidColumn (Report.Dictionary, expr))
            {
                expr = "[" + expr + "]";
            }
            else
            {
                expr = expression;
            }

            var expressionCode = ReplaceDataItems (expr);
            InsertItem (Report.CodeHelper.AddExpression (expression, expressionCode),
                source == null ? "" : source.Name);
            needCompile = true;
        }

        public void AddObjects()
        {
            var allObjects = Report.AllObjects;
            SortedList<string, Base> objects = new SortedList<string, Base>();

            // add all report objects
            InsertItem (Report.CodeHelper.AddField (typeof (Report), "Report") +
                        Report.CodeHelper.AddField (typeof (ReportEngine), "Engine"), "Report");
            foreach (Base c in allObjects)
            {
                if (!string.IsNullOrEmpty (c.Name) && !objects.ContainsKey (c.Name))
                {
                    objects.Add (c.Name, c);
                }
            }

            foreach (var c in objects.Values)
            {
                InsertItem (Report.CodeHelper.AddField (c.GetType(), c.Name), c.Name);
            }

            // add custom script
            var processedCode = "";
            foreach (var c in objects.Values)
            {
                var customCode = c.GetCustomScript();

                // avoid custom script duplicates
                if (!string.IsNullOrEmpty (customCode) && processedCode.IndexOf (customCode) == -1)
                {
                    InsertItem (customCode, c.Name);
                    processedCode += customCode;
                    needCompile = true;
                }
            }
        }

        public void AddSingleExpression (string expression)
        {
            InsertItem (Report.CodeHelper.BeginCalcExpression(), "");
            AddExpression (expression, null, true);
            InsertItem (Report.CodeHelper.EndCalcExpression(), "");
            needCompile = true;
        }

        public void AddExpressions()
        {
            // speed up the case: lot of report objects (> 1000) and lot of data columns in the dictionary (> 10000).
            Report.Dictionary.CacheAllObjects = true;

            InsertItem (Report.CodeHelper.BeginCalcExpression(), "");

            var allObjects = Report.AllObjects;
            var l = Report.Dictionary.AllObjects;
            foreach (Base c in l)
            {
                allObjects.Add (c);
            }

            foreach (Base c in allObjects)
            {
                string[] expressions = c.GetExpressions();
                if (expressions != null)
                {
                    foreach (var expr in expressions)
                    {
                        AddExpression (expr, c, false);
                    }
                }
            }

            InsertItem (Report.CodeHelper.EndCalcExpression(), "");
            Report.Dictionary.CacheAllObjects = false;
        }

        public void AddFunctions()
        {
            List<FunctionInfo> list = new List<FunctionInfo>();
            RegisteredObjects.Functions.EnumItems (list);

            foreach (var info in list)
            {
                if (info.Function != null)
                {
                    InsertItem (Report.CodeHelper.GetMethodSignatureAndBody (info.Function), "Function");
                }
            }
        }

        public string GenerateReportClass (string className)
        {
            InsertItem (Report.CodeHelper.GenerateInitializeMethod(), "");
            return Report.CodeHelper.ReplaceClassName (scriptText.ToString(), className);
        }

        public void Compile()
        {
            if (needCompile)
            {
                lock (compileLocker)
                {
                    if (needCompile)
                    {
                        InternalCompile();
                    }
                }
            }
        }

        private void InternalCompile()
        {
            // configure compiler options
            var cp = new CompilerParameters();
            AddFastReportAssemblies (cp.ReferencedAssemblies); // 2
            AddReferencedAssemblies (cp.ReferencedAssemblies, currentFolder); // 9
            ReviewReferencedAssemblies (cp.ReferencedAssemblies);
            cp.GenerateInMemory = true;

            // sometimes the system temp folder is not accessible...
            if (Config.TempFolder != null)
            {
                cp.TempFiles = new TempFileCollection (Config.TempFolder, false);
            }

            if (Config.WebMode &&
                Config.EnableScriptSecurity &&
                Config.ScriptSecurityProps.AddStubClasses)
            {
                AddStubClasses();
            }

            var errors = string.Empty;
            var exception = !InternalCompile (cp, out var cr);
            for (var i = 0; exception && i < Config.CompilerSettings.RecompileCount; i++)
            {
                exception = !TryRecompile (cp, ref cr);
            }

            if (cr != null)
            {
                HandleCompileErrors (cr, out errors);
            }

            if (exception && errors != string.Empty)
            {
                throw new CompilerException (errors);
            }
        }

        private string GetAssemblyHash (CompilerParameters cp)
        {
            var assemblyHashSB = new StringBuilder();
            foreach (var a in cp.ReferencedAssemblies)
            {
                assemblyHashSB.Append (a);
            }

            var script = scriptText.ToString();
            assemblyHashSB.Append (script);
            byte[] hash;

            using (var hMACSHA1 = new HMACSHA1 (Encoding.ASCII.GetBytes (shaKey)))
            {
                hash = hMACSHA1.ComputeHash (Encoding.Unicode.GetBytes (assemblyHashSB.ToString()));
            }

            return Convert.ToBase64String (hash);
        }

        /// <summary>
        /// Returns true, if compilation is successful
        /// </summary>
        private bool InternalCompile (CompilerParameters cp, out CompilerResults cr)
        {
            // find assembly in cache
            var assemblyHash = GetAssemblyHash (cp);
            if (FAssemblyCache.TryGetValue (assemblyHash, out var cachedAssembly))
            {
                Assembly = cachedAssembly;
                var reportScript = Assembly.CreateInstance ("AM.Reporting.ReportScript");
                InitInstance (reportScript);
                cr = null;
                return true;
            }


            // compile report scripts
            using (var provider = Report.CodeHelper.GetCodeProvider())
            {
                var ssea =
                    new ScriptSecurityEventArgs (Report, scriptText.ToString(), Report.ReferencedAssemblies);
                Config.OnScriptCompile (ssea);

#if CROSSPLATFORM || COREWIN
                provider.BeforeEmitCompilation += Config.OnBeforeScriptCompilation;
#endif

                cr = provider.CompileAssemblyFromSource (cp, scriptText.ToString());
                Assembly = null;
                Instance = null;

                if (cr.Errors.Count != 0) // Compile errors
                {
                    return false;
                }

                FAssemblyCache.TryAdd (assemblyHash, cr.CompiledAssembly);

                Assembly = cr.CompiledAssembly;
                var reportScript = Assembly.CreateInstance ("AM.Reporting.ReportScript");
                InitInstance (reportScript);
                return true;
            }
        }

        private string ReplaceExpression (string error, TextObjectBase text)
        {
            var result = text.Text;
            string[] parts = error.Split ('\"');
            if (parts.Length == 3)
            {
                string[] expressions = text.GetExpressions();
                foreach (var expr in expressions)
                {
                    if (expr.Contains (parts[1]))
                    {
                        if (!DataHelper.IsValidColumn (Report.Dictionary, expr))
                        {
                            var replaceString = text.Brackets[0] + expr + text.Brackets[2];
                            if (Config.CompilerSettings.ExceptionBehaviour is CompilerExceptionBehaviour.ShowExceptionMessage or CompilerExceptionBehaviour.ReplaceExpressionWithPlaceholder)
                            {
                                result = result.Replace (replaceString, Config.CompilerSettings.Placeholder);
                            }
                            else if (Config.CompilerSettings.ExceptionBehaviour ==
                                     CompilerExceptionBehaviour.ReplaceExpressionWithExceptionMessage)
                            {
                                result = result.Replace (replaceString, error);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Handle compile errors
        /// </summary>
        private void HandleCompileErrors (CompilerResults cr, out string errors)
        {
            errors = string.Empty;
            Regex regex;

            if (Config.WebMode && Config.EnableScriptSecurity)
            {
                for (var i = 0; i < cr.Errors.Count;)
                {
                    var ce = cr.Errors[i];
                    if (ce.ErrorNumber == "CS1685") // duplicate class
                    {
                        cr.Errors.Remove (ce);
                        continue;
                    }
                    else if (ce.ErrorNumber == "CS0436") // user using a forbidden type
                    {
                        const string pattern = "[\"'](\\S+)[\"']";
                        regex = new Regex (pattern, RegexOptions.Compiled);
                        var typeName = regex.Match (ce.ErrorText).Value;

                        const string res = "Web,ScriptSecurity,ForbiddenType";
                        var message = Res.TryGet (res);
                        if (string.Equals (res, message))
                        {
                            message = "Please don't use the type " + typeName;
                        }
                        else
                        {
                            message = message.Replace ("{typeName}",
                                typeName); //$"Please don't use the type {typeName}";
                        }

                        ce.ErrorText = message;
                    }
                    else if (ce.ErrorNumber == "CS0117") // user using a forbidden method
                    {
                        const string pattern = "[\"'](\\S+)[\"']";
                        regex = new Regex (pattern, RegexOptions.Compiled);
                        var mathes = regex.Matches (ce.ErrorText);
                        if (mathes.Count > 1)
                        {
                            var methodName = mathes[1].Value;

                            const string res = "Web,ScriptSecurity,ForbiddenMethod";
                            var message = Res.TryGet (res);
                            if (string.Equals (res, message))
                            {
                                message = "Please don't use the method " + methodName;
                            }
                            else
                            {
                                message = message.Replace ("{methodName}",
                                    methodName); //$"Please don't use the method {methodName}";
                            }

                            ce.ErrorText = message;
                        }
                    }

                    i++;
                }
            }

            foreach (CompilerError ce in cr.Errors)
            {
                var line = GetScriptLine (ce.Line);

                // error is inside own items
                if (line == -1)
                {
                    var errObjName = GetErrorObjectName (ce.Line);

                    if (Config.CompilerSettings.ExceptionBehaviour != CompilerExceptionBehaviour.Default)
                    {
                        // handle errors when name does not exist in the current context
                        if (ce.ErrorNumber == "CS0103")
                        {
                            var text = Report.FindObject (errObjName) as TextObjectBase;
                            text.Text = ReplaceExpression (ce.ErrorText, text);
                            if (Config.CompilerSettings.ExceptionBehaviour ==
                                CompilerExceptionBehaviour.ShowExceptionMessage)
                            {
                                System.Windows.Forms.MessageBox.Show (ce.ErrorText);
                            }

                            continue;
                        }
                    }

                    // handle division by zero errors
                    if (ce.ErrorNumber == "CS0020")
                    {
                        var text = Report.FindObject (errObjName) as TextObjectBase;
                        text.CanGrow = true;
                        text.FillColor = Color.Red;
                        text.Text = "DIVISION BY ZERO!";
                        continue;
                    }
                    else
                    {
                        errors += $"({errObjName}): {Res.Get ("Messages,Error")} {ce.ErrorNumber}: {ce.ErrorText}\r\n";
                        ErrorMsg (errObjName, ce);
                    }
                }
                else
                {
                    errors +=
                        $"({line},{ce.Column}): {Res.Get ("Messages,Error")} {ce.ErrorNumber}: {ce.ErrorText}\r\n";
                    ErrorMsg (ce, line);
                }
            }
        }

        /// <summary>
        /// Returns true if recompilation is successful
        /// </summary>
        private bool TryRecompile (CompilerParameters cp, ref CompilerResults cr)
        {
            List<string> additionalAssemblies = new List<string> (4);

            foreach (CompilerError ce in cr.Errors)
            {
                if (ce.ErrorNumber == "CS0012") // missing reference on assembly
                {
                    // try to add reference
                    try
                    {
                        // in .Net Core compiler will return other quotes
#if CROSSPLATFORM || COREWIN
                        const string quotes = "\'";
#else
                        const string quotes = "\"";
#endif
                        const string pattern = quotes + @"(\S{1,}),";
                        var regex = new Regex (pattern, RegexOptions.Compiled);
                        var assemblyName =
                            regex.Match (ce.ErrorText).Groups[1]
                                .Value; // Groups[1] include string without quotes and , symbols
                        if (!additionalAssemblies.Contains (assemblyName))
                        {
                            additionalAssemblies.Add (assemblyName);
                        }

                        continue;
                    }
                    catch
                    {
                    }
                }
            }

            if (additionalAssemblies.Count > 0) // need recompile
            {
                // try to load missing assemblies
                foreach (var assemblyName in additionalAssemblies)
                {
                    AddReferencedAssembly (cp.ReferencedAssemblies, currentFolder, assemblyName);
                }

                return InternalCompile (cp, out cr);
            }

            return false;
        }


        public void InitInstance (object instance)
        {
            Instance = instance;
            InitFields();
        }

        public bool ContainsExpression (string expr)
        {
            return Expressions.ContainsKey (expr);
        }

        public object CalcExpression (string expr, Variant value)
        {
            if (Expressions[expr] is ExpressionDescriptor expressionDescriptor)
            {
                return expressionDescriptor.Invoke (new object[] { expr, value });
            }
            else
            {
                return null;
            }
        }

        public object InvokeMethod (string name, object[] parms)
        {
            if (string.IsNullOrEmpty (name))
            {
                return null;
            }

            var exprName = "method_" + name;
            if (!ContainsExpression (exprName))
            {
                var descriptor = new ExpressionDescriptor (this);
                Expressions.Add (exprName, descriptor);
                descriptor.MethodName = name;
            }

            try
            {
                return (Expressions[exprName] as ExpressionDescriptor).Invoke (parms);
            }
            catch (TargetInvocationException ex)
            {
                throw (ex.InnerException); // ex now stores the original exception
            }
        }

        public AssemblyDescriptor (Report report, string scriptText)
        {
            Report = report;
            this.scriptText = new FastString (scriptText);
            Expressions = new Hashtable();
            sourcePositions = new List<SourcePosition>();
            insertPos = Report.CodeHelper.GetPositionToInsertOwnItems (scriptText);
            if (insertPos == -1)
            {
                var msg = Res.Get ("Messages,ClassError");
                ErrorMsg (msg);
                throw new CompilerException (msg);
            }
            else
            {
                string[] lines = scriptText.Substring (0, insertPos).Split ('\r');
                insertLine = lines.Length;
                if (scriptText != Report.CodeHelper.EmptyScript())
                {
                    needCompile = true;
                }
            }

            // set the current folder
            currentFolder = Config.ApplicationFolder;
            if (Config.WebMode)
            {
                try
                {
                    var bin_directory = Path.Combine (currentFolder, "Bin");
                    if (Directory.Exists (bin_directory))
                    {
                        currentFolder = bin_directory;
                    }
                }
                catch
                {
                }
            }

            // Commented by Samuray
            //Directory.SetCurrentDirectory(currentFolder);
        }

        static AssemblyDescriptor()
        {
            FAssemblyCache = new ConcurrentDictionary<string, Assembly>();

            compileLocker = new object();
        }

        private class SourcePosition
        {
            public readonly string sourceObject;
            public readonly int start;
            public readonly int end;

            public SourcePosition (string obj, int start, int end)
            {
                sourceObject = obj;
                this.start = start;
                this.end = end;
            }
        }
    }
}
