// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StandardFunctions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    static partial class StandardFunctions
    {
        #region Private members

        //================================================================
        // STANDARD BUILTIN FUNCTIONS
        //================================================================

        private static void AddField(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(context.Record, null))
            {
                var parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );
                if (parts.Length == 2)
                {
                    var tag = parts[0];
                    var text = parts[1];
                    var lines = text.SplitLines()
                        .NonEmptyLines().ToArray();

                    foreach (var body in lines)
                    {
                        var field = FieldUtility.Parse(tag.AsMemory(), body.AsMemory());
                        context.Record.Fields.Add(field);
                    }
                }
            }
        }

        //=================================================

        private static void Bold(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringValue(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<b>" + expression + "</b>");
            }
        }

        //=================================================

        private static void Cat
            (
                PftContext context,
                PftNode node,
                PftNode[] arguments
            )
        {
            //
            // TODO: add some caching
            //

            var expression = context.GetStringValue(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = context.Provider.Database,
                        FileName = expression
                    };
                var source = context.Provider.ReadTextFile
                    (
                        specification
                    );
                context.Write(node, source);
            }
        }

        //=================================================

        private static void Chr(PftContext context, PftNode node, PftNode[] arguments)
        {
            int code;
            char c;

            var call = (PftFunctionCall)node;
            if (call.Arguments.Count == 0)
            {
                return;
            }

            var numeric = call.Arguments[0] as PftNumeric;
            if (!ReferenceEquals(numeric, null))
            {
                code = (int)numeric.Value;
                c = (char)code;
                context.Write(numeric, c.ToString());
            }
            else
            {
                var expression = context.GetStringArgument(arguments, 0);
                if (!string.IsNullOrEmpty(expression))
                {
                    if (int.TryParse(expression, out code))
                    {
                        c = (char)code;
                        context.Write(node, c.ToString());
                    }
                }
            }
        }

        //=================================================

        private static void CommandLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            context.Write(node, Environment.CommandLine);
        }

        private static void COut(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                Console.Write(expression);
            }
        }

        //=================================================

        private static void Debug(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Debug.WriteLine(expression);
            }
        }

        //=================================================

        private static void DelField(PftContext context, PftNode node, PftNode[] arguments)
        {
            var record = context.Record;

            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(record, null))
            {
                var repeat = -1;
                var parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );
                var tag = parts[0];
                var fields = record.Fields.GetField(tag.SafeToInt32());

                if (parts.Length == 2)
                {
                    var repeatText = parts[1];
                    if (repeatText == "*")
                    {
                        repeat = fields.Length - 1;
                    }
                    else
                    {
                        if (!int.TryParse(repeatText, out repeat))
                        {
                            return;
                        }
                        repeat--;
                    }
                }

                if (repeat < 0)
                {
                    foreach (var field in fields)
                    {
                        record.Fields.Remove(field);
                    }
                }
                else
                {
                    var field = fields.GetOccurrence(repeat);
                    if (!ReferenceEquals(field, null))
                    {
                        record.Fields.Remove(field);
                    }
                }
            }
        }

        //=================================================

        private static void Error(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Error.WriteLine(expression);
            }
        }

        //=================================================

        private static void Exit(PftContext context, PftNode node, PftNode[] arguments)
        {
            throw new PftExitException();
        }

        //=================================================

        private static void Fatal(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            var message = expression ?? string.Empty;

            Magna.Critical
                (
                    "StandardFunctions::Fatal: "
                    + "message="
                    + message.ToVisibleString()
                );

            context.Provider.PlatformAbstraction.FailFast(message);
        }

        //=================================================

        private static void GetEnv(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var result = context.Provider.PlatformAbstraction
                    .GetEnvironmentVariable(expression);
                context.Write(node, result);
            }
        }

        //=================================================

        private static void HtmlEscape(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var result = HtmlText.Encode(expression);
                context.Write(node, result);
            }
        }

        //=================================================

        private static void Include(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                Unifors.Unifor6.ExecuteNestedFormat
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        //=================================================

        private static void Insert(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringValue(arguments, 0);
            var index = context.GetNumericArgument(arguments, 1);
            var value = context.GetStringValue(arguments, 2);

            if (!ReferenceEquals(text, null)
                && index.HasValue
                && !ReferenceEquals(value, null)
               )
            {
                string result;
                var offset = (int) index.Value;

                if (offset <= 0)
                {
                    result = value + text;
                }
                else if (offset >= text.Length)
                {
                    result = text + value;
                }
                else
                {
                    result = text.Insert(offset, value);
                }

                context.Write(node, result);
            }
        }

        //=================================================

        private static void IOcc
            (
                PftContext context,
                PftNode node,
                PftNode[] arguments
            )
        {
            var index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }
            var text = index.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void Italic(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringValue(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, "<i>" + expression + "</i>");
            }
        }

        //=================================================

        private static void Len(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringValue(arguments, 0);
            var size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.Length;
            var text = size.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void LoadRecord(PftContext context, PftNode node, PftNode[] arguments)
        {
            var number = context.GetNumericArgument(arguments, 0);
            var level = context.GetNumericArgument(arguments, 1);
            if (number != null)
            {
                var mfn = (int)number;
                var record = context.Provider
                    .ReadRecord(mfn);

                if (ReferenceEquals(record, null))
                {
                    context.Write(node, "0");
                }
                else
                {
                    if (level == null)
                    {
                        var ctx = context;
                        while (!ReferenceEquals(ctx, null))
                        {
                            ctx.Record = record;
                            ctx = ctx.Parent;
                        }
                    }
                    else
                    {
                        var limit = (int)level.Value;
                        var count = 0;
                        var ctx = context;
                        while (!ReferenceEquals(ctx, null)
                            && count < limit)
                        {
                            ctx.Record = record;
                            ctx = ctx.Parent;
                            count++;
                        }

                    }
                    context.Write(node, "1");
                }
            }
        }

        //=================================================

        private static void MachineName(PftContext context, PftNode node, PftNode[] arguments)
        {
            var machineName = context.Provider.PlatformAbstraction.GetMachineName();
            context.Write(node, machineName);
        }

        //=================================================

        private static void NOcc(PftContext context, PftNode node, PftNode[] arguments)
        {
            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                context.Write(node, "0");
                return;
            }

            var result = 0;

            if (arguments.Length != 0)
            {
                var value = context.GetNumericArgument(arguments, 0);
                if (value.HasValue)
                {
                    var tag = (int) value;
                    result = record.Fields.GetFieldCount(tag);
                }
            }
            else
            {
                if (!ReferenceEquals(context.CurrentGroup, null))
                {
                    var fields = context.CurrentGroup.GetDescendants<PftV>()
                        .Where(field => field.Command == 'v' || field.Command == 'V')
                        .ToArray();

                    foreach (var field in fields)
                    {
                        var count = record.Fields.GetField(field.Tag.SafeToInt32()).Length;
                        if (count > result)
                        {
                            result = count;
                        }
                    }
                }
            }

            var text = result.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void Now(PftContext context, PftNode node, PftNode[] arguments)
        {
            var now = context.Provider.PlatformAbstraction.Now();

            var expression = context.GetStringArgument(arguments, 0);
            var output = string.IsNullOrEmpty(expression)
                ? now.ToString(CultureInfo.CurrentCulture)
                : now.ToString(expression);

            context.Write(node, output);
        }

        //=================================================

        private static void NPost(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var specification = new FieldSpecification();
                specification.Parse(expression);
                var record = context.Record;
                if (!ReferenceEquals(record, null))
                {
                    var count = record.Fields.GetField(specification.Tag).Length;
                    var text = count.ToInvariantString();
                    context.Write(node, text);
                }
            }
        }

        //=================================================

        private static void OsVersion(PftContext context, PftNode node, PftNode[] arguments)
        {
            var result = context.Provider.PlatformAbstraction.OsVersion().ToString();

            context.Write(node, result);
        }

        //=================================================

        private static void PadLeft(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringValue(arguments, 0);
            var width = context.GetNumericArgument(arguments, 1);
            var padding = context.GetStringValue(arguments, 2);

            if (ReferenceEquals(text, null)
                || !width.HasValue)
            {
                return;
            }

            var pad = ' ';
            if (!string.IsNullOrEmpty(padding))
            {
                pad = padding[0];
            }

            var output = text.PadLeft
                (
                    (int)width.Value,
                    pad
                );
            context.Write(node, output);
        }

        //=================================================

        private static void PadRight(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringValue(arguments, 0);
            var width = context.GetNumericArgument(arguments, 1);
            var padding = context.GetStringValue(arguments, 2);

            if (ReferenceEquals(text, null)
                || !width.HasValue)
            {
                return;
            }

            var pad = ' ';
            if (!string.IsNullOrEmpty(padding))
            {
                pad = padding[0];
            }

            var output = text.PadRight
                (
                    (int)width.Value,
                    pad
                );
            context.Write(node, output);
        }

        //=================================================

        private static void Remove(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringValue(arguments, 0);
            var index = context.GetNumericArgument(arguments, 1);
            var count = context.GetNumericArgument(arguments, 2);

            if (!ReferenceEquals(text, null)
                && index.HasValue
                && count.HasValue
               )
            {
                var length = text.Length;
                var offset = (int) index.Value;
                var c = (int) count.Value;

                if (offset >= 0
                    && c >= 0
                    && offset + c < length
                   )
                {
                    var result = text.Remove(offset, c);
                    context.Write(node, result);
                }
            }
        }

        //=================================================

        private static void Replace(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringValue(arguments, 0);
            var oldValue = context.GetStringValue(arguments, 1);
            var newValue = context.GetStringValue(arguments, 2);

            if (ReferenceEquals(text, null)
                || ReferenceEquals(oldValue, null)
                || ReferenceEquals(newValue, null))
            {
                return;
            }

            var output = text.Replace(oldValue, newValue);
            context.Write(node, output);
        }

        //=================================================

        private static void RtfEscape(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var result = RichText.Encode(expression, UnicodeRange.Russian);
                context.Write(node, result);
            }
        }

        //=================================================

        private static void Search(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var foundMfns = context.Provider.Search(expression);
                if (foundMfns.Length != 0)
                {
                    var foundLines = foundMfns.Select
                        (
                            item => item.ToInvariantString()
                        )
                        .ToArray();
                    var output = string.Join
                        (
                            Environment.NewLine,
                            foundLines
                        );
                    context.Write(node, output);
                }
            }
        }

        //=================================================

        private static void Size(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            var size = string.IsNullOrEmpty(expression)
                ? 0
                : expression.SplitLines().Length;
            var text = size.ToInvariantString();
            context.Write(node, text);
        }

        //=================================================

        private static void Sort(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var lines = expression.SplitLines()
                .NonEmptyLines().ToArray();
            lines = NumberText.Sort(lines).ToArray();
            context.Write
                (
                    node,
                    string.Join
                    (
                        Environment.NewLine,
                        lines
                    )
                );
        }

        //=================================================

        private static void Split(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringArgument(arguments, 0);
            var separator = context.GetStringArgument(arguments, 1);

            if (ReferenceEquals(text, null)
                || ReferenceEquals(separator, null))
            {
                return;
            }

            var lines = text.Split(separator);
            var output = string.Join(Environment.NewLine, lines);
            context.Write(node, output);
        }

        //=================================================

        private static void Substring(PftContext context, PftNode node, PftNode[] arguments)
        {
            var text = context.GetStringArgument(arguments, 0);
            var offset = context.GetNumericArgument(arguments, 1);
            var length = context.GetNumericArgument(arguments, 2);

            if (ReferenceEquals(text, null)
                || !offset.HasValue
                || !length.HasValue)
            {
                return;
            }

            var output = text.SafeSubstring
                (
                    (int)offset.Value,
                    (int)length.Value
                );
            context.Write(node, output);
        }

        //=================================================

        private static void System(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                // TODO use PlatformAbstractionLayer

                string comspec = Environment.GetEnvironmentVariable("comspec")
                    ?? "cmd.exe";

                expression = "/c " + expression;
                ProcessStartInfo startInfo = new ProcessStartInfo
                    (
                        comspec,
                        expression
                    )
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = IrbisEncoding.Oem
                };

                Process process = Process.Start(startInfo)
                    .ThrowIfNull("process");
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                {
                    context.Write(node, output);
                }
            }
        }

        //=================================================

        private static void Tags(PftContext context, PftNode node, PftNode[] arguments)
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                var tags = record.Fields.Select
                    (
                        field => field.Tag
                    )
                    .Distinct()
                    .OrderBy(tag => tag)
                    .Select(tag => tag.ToInvariantString())
                    .ToArray();

                var expression = context.GetStringArgument(arguments, 0);
                if (!string.IsNullOrEmpty(expression))
                {
                    var regex = new Regex(expression);
                    tags = tags.Where
                        (
                            tag => regex.IsMatch(tag)
                        )
                        .ToArray();
                }

                var output = string.Join
                    (
                        Environment.NewLine,
                        tags
                    );
                context.Write(node, output);
            }
        }

        //=================================================

        private static void Today(PftContext context, PftNode node, PftNode[] arguments)
        {
            var today = context.Provider.PlatformAbstraction.Today();

            var expression = context.GetStringArgument(arguments, 0);
            var output = string.IsNullOrEmpty(expression)
                ? today.ToShortDateString()
                : today.ToString(expression);

            context.Write(node, output);
        }

        //=================================================

        private static void ToLower(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var output = IrbisText.ToLower(expression);
                context.Write(node, output);
            }
        }

        //=================================================

        private static void ToUpper(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                var output = IrbisText.ToUpper(expression);
                context.Write(node, output);
            }
        }

        //=================================================

        private static void Trace(PftContext context, PftNode node, PftNode[] arguments)
        {
#if CLASSIC || NETCORE || ANDROID

            string expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                global::System.Diagnostics.Trace.WriteLine(expression);
            }

#endif
        }

        //=================================================

        private static void Trim(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Write(node, expression.Trim());
            }
        }

        //=================================================

        private static void Warn(PftContext context, PftNode node, PftNode[] arguments)
        {
            var expression = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(expression))
            {
                context.Output.Warning.WriteLine(expression);
            }
        }

        #endregion

        #region Public methods

        //=================================================

        internal static void Register()
        {
            var reg = PftFunctionManager.BuiltinFunctions;

            reg.Add("addField", AddField);
            reg.Add("bold", Bold);
            reg.Add("cat", Cat);
            reg.Add("chr", Chr);
            reg.Add("commandline", CommandLine);
            reg.Add("cOut", COut);
            reg.Add("debug", Debug);
            reg.Add("delField", DelField);
            reg.Add("error", Error);
            reg.Add("exit", Exit);
            reg.Add("fatal", Fatal);
            reg.Add("html", HtmlEscape);
            reg.Add("getenv", GetEnv);
            reg.Add("iocc", IOcc);
            reg.Add("include", Include);
            reg.Add("insert", Insert);
            reg.Add("italic", Italic);
            reg.Add("len", Len);
            reg.Add("loadRecord", LoadRecord);
            reg.Add("machineName", MachineName);
            reg.Add("nocc", NOcc);
            reg.Add("now", Now);
            reg.Add("nPost", NPost);
            reg.Add("osVersion", OsVersion);
            reg.Add("padLeft", PadLeft);
            reg.Add("padRight", PadRight);
            reg.Add("remove", Remove);
            reg.Add("replace", Replace);
            reg.Add("rtf", RtfEscape);
            reg.Add("size", Size);
            reg.Add("search", Search);
            reg.Add("sort", Sort);
            reg.Add("split", Split);
            reg.Add("substring", Substring);
            reg.Add("system", System);
            reg.Add("tags", Tags);
            reg.Add("today", Today);
            reg.Add("tolower", ToLower);
            reg.Add("toupper", ToUpper);
            reg.Add("trace", Trace);
            reg.Add("trim", Trim);
            reg.Add("warn", Warn);

            // ===================

            reg.Add("close", Close);
            reg.Add("isOpen", IsOpen);
            reg.Add("openAppend", OpenAppend);
            reg.Add("openRead", OpenRead);
            reg.Add("openWrite", OpenWrite);
            reg.Add("readAll", ReadAll);
            reg.Add("readLine", ReadLine);
            reg.Add("write", Write);
            reg.Add("writeLine", WriteLine);

            // ===================

            reg.Add("call", CallObject);
            reg.Add("createObject", CreateObject);
            reg.Add("closeObject", CloseObject);
            reg.Add("openObject", OpenObject);
        }

        #endregion
    }
}
