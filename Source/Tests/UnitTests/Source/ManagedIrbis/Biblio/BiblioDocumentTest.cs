// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio;

[TestClass]
public sealed class BiblioDocumentTest
    : Common.CommonUnitTest
{
    private BiblioContext _GetContext()
    {
        return new BiblioContext
            (
                new BiblioDocument(),
                new NullProvider(),
                new NullOutput()
            );
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void BiblioDocument_Construction_1()
    {
        var document = new BiblioDocument();
        Assert.IsNotNull (document.Chapters);
        Assert.AreEqual (0, document.Chapters.Count);
    }

    [TestMethod]
    [Description ("Построение словарей")]
    [ExpectedException (typeof (NotImplementedException))]
    public void BiblioDocument_BuildDictionaries_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.BuildDictionaries (context);
    }

    [TestMethod]
    [Description ("Построение элементов")]
    [ExpectedException (typeof (NotImplementedException))]
    public void BiblioDocument_BuildItems_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.BuildItems (context);
    }

    [TestMethod]
    [Description ("Сбор записей")]
    public void BiblioDocument_GatherRecords_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.GatherRecords (context);
    }

    [TestMethod]
    [Description ("Инициализация")]
    public void BiblioDocument_Initialize_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.Initialize (context);
    }

    [TestMethod]
    [Description ("Загрузка из файла")]
    [ExpectedException (typeof (NotImplementedException))]
    public void BiblioDocument_LoadFile_1()
    {
        var fileName = Path.Combine (TestDataPath, "Biblio", "rasputin.json");
        var document = BiblioDocument.LoadFile (fileName);
        Assert.IsNotNull (document);
    }

    [TestMethod]
    [Description ("Нумерация элементов")]
    public void BiblioDocument_NumberItems_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.NumberItems (context);
    }

    [TestMethod]
    [Description ("Рендеринг элементов")]
    public void BiblioDocument_RenderItems_1()
    {
        var context = _GetContext();
        var document = context.Document;
        document.RenderItems (context);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void BiblioDocument_Verify_1()
    {
        var context = _GetContext();
        var document = context.Document;
        Assert.IsTrue (document.Verify (false));
    }

}