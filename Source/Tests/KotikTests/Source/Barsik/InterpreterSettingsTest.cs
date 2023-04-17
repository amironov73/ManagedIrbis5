// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.IO;

using AM.Kotik.Barsik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests.Barsik;

[TestClass]
public sealed class InterpreterSettingsTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void InterpreterSettings_Construciton_1()
    {
        var settings = new InterpreterSettings();
        Assert.IsFalse (settings.StartDebugger);
        Assert.IsFalse (settings.DebugParser);
        Assert.IsFalse (settings.DumpAst);
        Assert.IsNull (settings.VariableDumper);
        Assert.IsNull (settings.EvaluateExpression);
        Assert.IsNotNull (settings.LoadAssemblies);
        Assert.AreEqual (0, settings.LoadAssemblies.Count);
        Assert.IsNotNull (settings.ScriptFiles);
        Assert.AreEqual (0, settings.ScriptFiles.Count);
        Assert.IsFalse (settings.ReplMode);
        Assert.IsNull (settings.TokenizerSettings);
        Assert.IsNotNull (settings.UseNamespaces);
        Assert.AreEqual (0, settings.UseNamespaces.Count);
        Assert.IsNotNull (settings.MainPrompt);
        Assert.IsNotNull (settings.SecondaryPrompt);
        Assert.IsNotNull (settings.Grammar);
        Assert.IsNotNull (settings.Tokenizer);
        Assert.IsNull (settings.Highlight);
        Assert.IsFalse (settings.BarsorMode);
        Assert.IsFalse (settings.DumpTokens);
        Assert.IsNotNull (settings.Paths);
        Assert.AreEqual (0, settings.Paths.Count);
        Assert.IsTrue (settings.AllowNewOperator);
    }

    [TestMethod]
    [Description ("Получение настроек по умолчанию")]
    public void InterpreterSettings_CreateDefault_1()
    {
        var settings = InterpreterSettings.CreateDefault();
        Assert.IsFalse (settings.StartDebugger);
        Assert.IsFalse (settings.DebugParser);
        Assert.IsFalse (settings.DumpAst);
        Assert.IsNull (settings.VariableDumper);
        Assert.IsNull (settings.EvaluateExpression);
        Assert.IsNotNull (settings.LoadAssemblies);
        Assert.AreEqual (0, settings.LoadAssemblies.Count);
        Assert.IsNotNull (settings.ScriptFiles);
        Assert.AreEqual (0, settings.ScriptFiles.Count);
        Assert.IsFalse (settings.ReplMode);
        Assert.IsNull (settings.TokenizerSettings);
        Assert.IsNotNull (settings.UseNamespaces);
        Assert.AreEqual (4, settings.UseNamespaces.Count);
        Assert.IsNotNull (settings.MainPrompt);
        Assert.IsNotNull (settings.SecondaryPrompt);
        Assert.IsNotNull (settings.Grammar);
        Assert.IsNotNull (settings.Tokenizer);
        Assert.IsNull (settings.Highlight);
        Assert.IsFalse (settings.BarsorMode);
        Assert.IsFalse (settings.DumpTokens);
        Assert.IsNotNull (settings.Paths);
        Assert.AreEqual (1, settings.Paths.Count);
        Assert.IsTrue (settings.AllowNewOperator);
    }

    [TestMethod]
    [Description ("Получение настроек из командной строки")]
    public void InterpreterSettings_FromCommandLine_1()
    {
        var args = new[] { "--use-namespace", "System.Security" };
        var settings = InterpreterSettings.CreateDefault();
        settings.FromCommandLine (args);
        Assert.IsFalse (settings.StartDebugger);
        Assert.IsFalse (settings.DebugParser);
        Assert.IsFalse (settings.DumpAst);
        Assert.IsNull (settings.VariableDumper);
        Assert.IsNull (settings.EvaluateExpression);
        Assert.IsNotNull (settings.LoadAssemblies);
        Assert.AreEqual (0, settings.LoadAssemblies.Count);
        Assert.IsNotNull (settings.ScriptFiles);
        Assert.AreEqual (0, settings.ScriptFiles.Count);
        Assert.IsFalse (settings.ReplMode);
        Assert.IsNull (settings.TokenizerSettings);
        Assert.IsNotNull (settings.UseNamespaces);
        Assert.AreEqual (5, settings.UseNamespaces.Count);
        Assert.IsNotNull (settings.MainPrompt);
        Assert.IsNotNull (settings.SecondaryPrompt);
        Assert.IsNotNull (settings.Grammar);
        Assert.IsNotNull (settings.Tokenizer);
        Assert.IsNull (settings.Highlight);
        Assert.IsFalse (settings.BarsorMode);
        Assert.IsFalse (settings.DumpTokens);
        Assert.IsNotNull (settings.Paths);
        Assert.AreEqual (1, settings.Paths.Count);
        Assert.IsTrue (settings.AllowNewOperator);
    }

    [TestMethod]
    [Description ("Загрузка настроек из файла")]
    public void InterpreterSettings_FromFile_1()
    {
        var fileName = Path.Combine (TestDataPath, "barsik.settings");
        var settings = InterpreterSettings.FromFile (fileName);
        Assert.IsFalse (settings.StartDebugger);
        Assert.IsFalse (settings.DebugParser);
        Assert.IsFalse (settings.DumpAst);
        Assert.IsNull (settings.VariableDumper);
        Assert.IsNull (settings.EvaluateExpression);
        Assert.IsNotNull (settings.LoadAssemblies);
        Assert.AreEqual (0, settings.LoadAssemblies.Count);
        Assert.IsNotNull (settings.ScriptFiles);
        Assert.AreEqual (0, settings.ScriptFiles.Count);
        Assert.IsFalse (settings.ReplMode);
        Assert.IsNull (settings.TokenizerSettings);
        Assert.IsNotNull (settings.UseNamespaces);
        Assert.AreEqual (4, settings.UseNamespaces.Count);
        Assert.IsNotNull (settings.MainPrompt);
        Assert.IsNotNull (settings.SecondaryPrompt);
        Assert.IsNotNull (settings.Grammar);
        Assert.IsNotNull (settings.Tokenizer);
        Assert.IsNull (settings.Highlight);
        Assert.IsFalse (settings.BarsorMode);
        Assert.IsFalse (settings.DumpTokens);
        Assert.IsNotNull (settings.Paths);
        Assert.AreEqual (1, settings.Paths.Count);
        Assert.IsTrue (settings.AllowNewOperator);
    }

    [TestMethod]
    [Description ("Сохранение настроек в файл")]
    public void InterpreterSettings_Save_1()
    {
        var fileName = Path.GetTempFileName(); 
        var settings = InterpreterSettings.CreateDefault();
        settings.Save (fileName);
    }
}
