// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GblTester.cs -- движок прогона тестов для глобальной корректировки записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Collections;
using AM.ConsoleIO;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Testing
{
    /// <summary>
    /// Движок прогона тестов для глобальной корректировки записей.
    /// </summary>
    public sealed class GblTester
    {
        #region Properties

        /// <summary>
        /// Синхронный ИРБИС-провайдер.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Имя директории, содержащей тесты.
        /// </summary>
        public string Folder { get; private set; }

        /// <summary>
        /// Коллекция тестов, подлежащих прогону.
        /// </summary>
        public NonNullCollection<GblTest> Tests { get;}

        /// <summary>
        /// Результаты прогона тестов.
        /// </summary>
        public NonNullCollection<GblTestResult> Results { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GblTester
            (
                string folder,
                ISyncProvider provider
            )
        {
            Sure.NotNullNorEmpty (folder, nameof (folder));

            Provider = provider;
            Folder = folder;
            Tests = new NonNullCollection<GblTest>();
            Results = new NonNullCollection<GblTestResult>();

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Обнаружение тестов.
        /// </summary>
        public void DiscoverTests()
        {
            var directories = Directory.GetDirectories
                (
                    Folder,
                    "*", SearchOption.AllDirectories
                );

            foreach (var subDir in directories)
            {
                if (GblTest.IsDirectoryContainsTest (subDir))
                {
                    var test = new GblTest(subDir);
                    Tests.Add (test);
                }
            }

        } // method DiscoverTests

        /// <summary>
        /// Прогон указанного теста.
        /// </summary>
        public GblTestResult? RunTest
            (
                GblTest test
            )
        {
            Sure.NotNull (test, nameof (test));

            GblTestResult? result = null;
            var name = Path.GetFileName (test.Folder);
            if (string.IsNullOrEmpty (name))
            {
                throw new GblException ("Can't determine name of test");
            }

            var foreColor = ConsoleInput.ForegroundColor;
            ConsoleInput.ForegroundColor = ConsoleColor.Cyan;
            ConsoleInput.Write ($"{name}: ");
            ConsoleInput.ForegroundColor = foreColor;

            try
            {
                result = test.Run(name);

                ConsoleInput.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

                ConsoleInput.Write(" ");
                ConsoleInput.WriteLine
                    (
                        result.Failed
                            ? "FAIL"
                            : "OK"
                    );

                ConsoleInput.ForegroundColor = foreColor;

                //ConsoleInput.WriteLine(new string('=', 70));
                //ConsoleInput.WriteLine();
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof (GblTester) + "::" + nameof (RunTest),
                        exception
                    );

                Debug.WriteLine (exception);
                ConsoleInput.WriteLine(exception.ToString());

            } // catch

            //ConsoleInput.WriteLine();

            return result;
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (var test in Tests)
            {
                test.Provider = Provider;
                var result = RunTest (test);
                if (result is not null)
                {
                    Results.Add(result);
                }
            }

        } // method RunTests

        /// <summary>
        /// Запись результатов в указанный файл.
        /// </summary>
        public void WriteResults
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName, nameof (fileName));

            /*
            using (StreamWriter writer = new StreamWriter
                (
                    new FileStream
                    (
                        fileName,
                        FileMode.Create,
                        FileAccess.Write
                    )
                ))
            {
                JArray array = JArray.FromObject(Results);
                string text = array.ToString(Formatting.Indented);
                writer.Write(text);
            }

            */

        } // method WriteResults

        #endregion

    } // class GblTester

} // namespace ManagedIrbis.Gbl.Infrastructure.Testing
