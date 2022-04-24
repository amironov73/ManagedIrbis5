// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.InMemory;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory;

[TestClass]
public sealed class InMemoryProviderTest
    : Common.CommonUnitTest
{
    private string _GetDatabasePath()
    {
        return Path.Combine (TestDataPath, "NoDataYet");
    }

    private string _GetTemporaryPath()
    {
        return Path.Combine (Path.GetTempPath(), "WriteHere");
    }

    private InMemoryProvider _GetProvider()
    {
        var resources = new InMemoryResourceProvider();
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return new InMemoryProvider (resources, serviceProvider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void InMemoryProvider_Construction_1()
    {
        var resources = new InMemoryResourceProvider();
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        using var provider = new InMemoryProvider (resources, serviceProvider);
        Assert.AreSame (resources, provider.Resources);
        Assert.IsNotNull (provider.Databases);
        Assert.IsFalse (provider.Connected);
    }

    [TestMethod]
    [Description ("Дамп всех баз данных")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_DumpAll_1()
    {
        using var provider = _GetProvider();
        var output = new StringWriter();
        provider.DumpAll (output);
        var dump = output.ToString();
        Assert.IsNotNull (dump);
    }

    [TestMethod]
    [Description ("Загрузка данных")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_LoadData_1()
    {
        using var provider = _GetProvider();
        var path = _GetDatabasePath();
        provider.LoadData (path);
    }

    [TestMethod]
    [Description ("Сохранение данных")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_SaveData_1()
    {
        using var provider = _GetProvider();
        var path = _GetTemporaryPath();
        provider.SaveData (path);
    }

    [TestMethod]
    [Description ("Конфигурация провайдера")]
    public void InMemoryProvider_Configure_1()
    {
        using var provider = _GetProvider();
        const string configurationString = "some string";
        provider.Configure (configurationString);
    }

    [TestMethod]
    [Description ("Актуализация записи")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ActualizeRecord_1()
    {
        using var provider = _GetProvider();
        var parameters = new ActualizeRecordParameters();
        provider.ActualizeRecord (parameters);
    }

    [TestMethod]
    [Description ("Подключение")]
    public void InMemoryProvider_Connect_1()
    {
        using var provider = _GetProvider();
        provider.Connect();
        Assert.IsTrue (provider.Connected);
        Assert.AreEqual (0, provider.LastError);
    }

    [TestMethod]
    [Description ("Создание базы данных")]
    [ExpectedException (typeof (ArgumentException))]
    public void InMemoryProvider_CreateDatabase_1()
    {
        using var provider = _GetProvider();
        var parameters = new CreateDatabaseParameters();
        provider.CreateDatabase (parameters);
    }

    [TestMethod]
    [Description ("Создание словаря")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_CreateDictionary_1()
    {
        using var provider = _GetProvider();
        provider.CreateDictionary();
    }

    [TestMethod]
    [Description ("Удаление базы данных")]
    [ExpectedException (typeof (IrbisException))]
    public void InMemoryProvider_DeleteDatabase_1()
    {
        using var provider = _GetProvider();
        provider.DeleteDatabase ();
    }

    [TestMethod]
    [Description ("Отключение")]
    public void InMemoryProvider_Disconnect_1()
    {
        using var provider = _GetProvider();
        provider.Connect();
        Assert.IsTrue (provider.Connected);
        Assert.AreEqual (0, provider.LastError);
        provider.Disconnect();
        Assert.IsFalse (provider.Connected);
        Assert.AreEqual (0, provider.LastError);
    }

    [TestMethod]
    [Description ("Проверка существования файла")]
    [ExpectedException (typeof (ArgumentException))]
    public void InMemoryProvider_FileExist_1()
    {
        using var provider = _GetProvider();
        var specification = new FileSpecification();
        var result = provider.FileExist (specification);
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Форматирование записей")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_FormatRecords_1()
    {
        using var provider = _GetProvider();
        var parameters = new FormatRecordParameters();
        var result = provider.FormatRecords (parameters);
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Полнотекстовый поиск")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_FullTextSearch_1()
    {
        using var provider = _GetProvider();
        var searchParameters = new SearchParameters();
        var textParameters = new TextParameters();
        var result = provider.FullTextSearch (searchParameters, textParameters);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Информация о базе данных")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_GetDatabaseInfo_1()
    {
        using var provider = _GetProvider();
        var result = provider.GetDatabaseInfo ("IBIS");
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Получение максимального MFN")]
    public void InMemoryProvider_GetMaxMfn_1()
    {
        using var provider = _GetProvider();
        var result = provider.GetMaxMfn ("IBIS");
        Assert.AreEqual (0, result);
    }

    [TestMethod]
    [Description ("Статистика работы сервера")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_GetServerStat_1()
    {
        using var provider = _GetProvider();
        var result = provider.GetServerStat();
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Версия сервера")]
    public void InMemoryProvider_GetServerVersion_1()
    {
        using var provider = _GetProvider();
        var result = provider.GetServerVersion();
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Получение списка файлов")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ListFiles_1()
    {
        using var provider = _GetProvider();
        var specification = new FileSpecification();
        var result = provider.ListFiles (specification);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Получение списка процессов")]
    public void InMemoryProvider_ListProcesses_1()
    {
        using var provider = _GetProvider();
        var result = provider.ListProcesses ();
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Получение списка пользователей")]
    public void InMemoryProvider_ListUsers_1()
    {
        using var provider = _GetProvider();
        var result = provider.ListUsers ();
        Assert.IsNull (result);
    }

    [TestMethod]
    [Description ("Пустая операция")]
    public void InMemoryProvider_NoOperation_1()
    {
        using var provider = _GetProvider();
        var result = provider.NoOperation ();
        Assert.IsTrue (result);
    }

    [TestMethod]
    [Description ("Печать таблицы")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_PrintTable_1()
    {
        using var provider = _GetProvider();
        var definition = new TableDefinition();
        var result = provider.PrintTable (definition);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение бинарного файла")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReadBinaryFile_1()
    {
        using var provider = _GetProvider();
        var specification = new FileSpecification();
        var result = provider.ReadBinaryFile (specification);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение постингов")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReadPostings_1()
    {
        using var provider = _GetProvider();
        var parameters = new PostingParameters();
        var result = provider.ReadPostings (parameters);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение записи")]
    [ExpectedException (typeof (IrbisException))]
    public void InMemoryProvider_ReadRecord_1()
    {
        using var provider = _GetProvider();
        var parameters = new ReadRecordParameters();
        var result = provider.ReadRecord (parameters);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение постингов записи")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReadRecordPostings_1()
    {
        using var provider = _GetProvider();
        var parameters = new ReadRecordParameters();
        var result = provider.ReadRecordPostings (parameters, "T=");
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение терминов поискового словаря")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReadTerms_1()
    {
        using var provider = _GetProvider();
        var parameters = new TermParameters();
        var result = provider.ReadTerms (parameters);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Чтение текстового файла")]
    [ExpectedException (typeof (ArgumentException))]
    public void InMemoryProvider_ReadTextFile_1()
    {
        using var provider = _GetProvider();
        var specification = new FileSpecification();
        var result = provider.ReadTextFile (specification);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Перезагрузка словаря")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReloadDictionary_1()
    {
        using var provider = _GetProvider();
        var result = provider.ReloadDictionary ("IBIS");
        Assert.IsTrue (result);
    }

    [TestMethod]
    [Description ("Перезагрузка файла документов")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_ReloadMasterFile_1()
    {
        using var provider = _GetProvider();
        var result = provider.ReloadMasterFile ("IBIS");
        Assert.IsTrue (result);
    }

    [TestMethod]
    [Description ("Перезапуск сервера")]
    public void InMemoryProvider_RestartServer_1()
    {
        using var provider = _GetProvider();
        var result = provider.RestartServer();
        Assert.IsTrue (result);
    }

    [TestMethod]
    [Description ("Поиск записей")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_Search_1()
    {
        using var provider = _GetProvider();
        var parameters = new SearchParameters();
        var result = provider.Search (parameters);
        Assert.IsNotNull (result);
    }

    [TestMethod]
    [Description ("Очистка базы данных")]
    public void InMemoryProvider_TruncateDatabase_1()
    {
        using var provider = _GetProvider();
        var result = provider.TruncateDatabase ("IBIS");
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Снятие блокировки с базы данных")]
    public void InMemoryProvider_UnlockDatabase_1()
    {
        using var provider = _GetProvider();
        var result = provider.UnlockDatabase ("IBIS");
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Снятие блокировки с отдельных записей")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_UnlockRecords_1()
    {
        using var provider = _GetProvider();
        var result = provider.UnlockRecords (Array.Empty<int>(), "IBIS");
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Обновление списка пользователей")]
    [ExpectedException (typeof (NotImplementedException))]
    public void InMemoryProvider_UpdateUserList_1()
    {
        using var provider = _GetProvider();
        var result = provider.UpdateUserList (Array.Empty<UserInfo>());
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Сохранение текстового файла на сервере")]
    [ExpectedException (typeof (ArgumentException))]
    public void InMemoryProvider_WriteTextFile_1()
    {
        using var provider = _GetProvider();
        var specification = new FileSpecification ();
        var result = provider.WriteTextFile (specification);
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Сохранение записи")]
    public void InMemoryProvider_WriteRecord_1()
    {
        using var provider = _GetProvider();
        var parameters = new WriteRecordParameters();
        var result = provider.WriteRecord  (parameters);
        Assert.IsFalse (result);
    }

    [TestMethod]
    [Description ("Получение сервиса")]
    public void InMemoryProvider_GetService_1()
    {
        using var provider = _GetProvider();
        var result = provider.GetService (typeof (IAsyncConnection));
        Assert.IsNull (result);
    }

}