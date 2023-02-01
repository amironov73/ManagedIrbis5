// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Tester.cs -- автоматический тестировщик для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Автоматический тестировщик для Барсика.
/// </summary>
public class Tester
{
    #region Properties

    /// <summary>
    /// Контекст прогона тестов.
    /// </summary>
    public TestContext Context { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Tester
        (
            TestContext context
        )
    {
        Sure.NotNull (context);

        Context = context;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Обнаружить и выполнить тесты из указанной папки
    /// и ее подпапок.
    /// </summary>
    public TestResult[] DiscoverAndRunTests
        (
            string folder
        )
    {
        Sure.NotNullNorEmpty (folder);
        if (!Directory.Exists (folder))
        {
            throw new DirectoryNotFoundException (folder);
        }

        var allResults = new List<TestResult>();
        var directories = Directory.GetDirectories
            (
                folder,
                "*",
                SearchOption.AllDirectories
            );

        Array.Sort (directories);

        foreach (var subDir in directories)
        {
            if (TestUtility.IsDirectoryContainsTest (subDir))
            {
                var test = new Test (subDir);
                var oneResult = test.Run (Context);
                allResults.Add (oneResult);
            }
        }

        return allResults.ToArray();
    }

    #endregion
}
