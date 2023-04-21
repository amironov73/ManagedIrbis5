// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Results;

#endregion

#nullable enable

namespace UnitTests.AM;

[TestClass]
public sealed class ResultsTest
{
    /// <summary>
    /// Класс для проверки представления результата
    /// в виде OneOf
    /// </summary>
    sealed class ParsingResult
        : OneOf<string, End, Skip>
    {
        private ParsingResult (string value)
            : base (value)
        {
            // пустое тело конструктора
        }

        private ParsingResult (End value)
            : base (value)
        {
            // пустое тело конструктора
        }

        private ParsingResult (Skip value)
            : base (value)
        {
            // пустое тело конструктора
        }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Text => As1();

        /// <summary>
        /// Проверка на успешность.
        /// </summary>
        public bool IsSuccess => Is1;

        /// <summary>
        /// Проверка на достижение конца.
        /// </summary>
        public bool IsEnd => Is2;

        /// <summary>
        /// Проверка на пропуск.
        /// </summary>
        public bool IsSkip => Is3;

        /// <summary>
        /// Конструирование успешного результата.
        /// </summary>
        public static ParsingResult Of (string value) => new (value);

        /// <summary>
        /// Результат: достигнут конец.
        /// </summary>
        public static readonly ParsingResult End = new (new End());

        /// <summary>
        /// Результат: пропуск.
        /// </summary>
        public static readonly ParsingResult Skip = new (new Skip());
    }

    [TestMethod]
    [Description ("Конструирование успешного результата")]
    public void ParsingResult_Of_1()
    {
        const string hello = "Hello";
        var result = ParsingResult.Of (hello);
        Assert.AreEqual (hello, result.Text);
        Assert.IsTrue (result.IsSuccess);
        Assert.IsFalse (result.IsEnd);
        Assert.IsFalse (result.IsSkip);
    }

    [TestMethod]
    [Description ("Конструирование результата: достигнут конец")]
    public void ParsingResult_End_1()
    {
        var result = ParsingResult.End;
        Assert.IsFalse (result.IsSuccess);
        Assert.IsTrue (result.IsEnd);
        Assert.IsFalse (result.IsSkip);
    }

    [TestMethod]
    [Description ("Конструирование результата: пропуск")]
    public void ParsingResult_Skip_1()
    {
        var result = ParsingResult.Skip;
        Assert.IsFalse (result.IsSuccess);
        Assert.IsFalse (result.IsEnd);
        Assert.IsTrue (result.IsSkip);
    }

    [TestMethod]
    [Description ("Попытка извлечения значения: успешная")]
    public void ParsingResult_Try_1()
    {
        const string hello = "Hello";
        var result = ParsingResult.Of (hello);
        Assert.IsTrue (result.Try1 (out var value));
        Assert.AreEqual (hello, value);
        Assert.IsFalse (result.Try2 (out _));
        Assert.IsFalse (result.Try3 (out _));
    }

    [TestMethod]
    [Description ("Попытка извлечения значения: неуспешная")]
    public void ParsingResult_Try_2()
    {
        var result = ParsingResult.End;
        Assert.IsFalse (result.Try1 (out _));
        Assert.IsTrue (result.Try2 (out _));
        Assert.IsFalse (result.Try3 (out _));
    }

    [TestMethod]
    [Description ("Попытка извлечения значения: неуспешная")]
    public void ParsingResult_Try_3()
    {
        var result = ParsingResult.Skip;
        Assert.IsFalse (result.Try1 (out _));
        Assert.IsFalse (result.Try2 (out _));
        Assert.IsTrue (result.Try3 (out _));
    }

    [TestMethod]
    [Description ("Выбор результатов: первый вариант")]
    public void ParsingResult_Switch_1()
    {
        var flag = -1;
        var result = ParsingResult.Of ("hello");
        result.Switch (_ => flag = 0, _ => flag = 1, _ => flag = 2);
        Assert.AreEqual (0, flag);
    }

    [TestMethod]
    [Description ("Выбор результатов: второй вариант")]
    public void ParsingResult_Switch_2()
    {
        var flag = -1;
        var result = ParsingResult.End;
        result.Switch (_ => flag = 0, _ => flag = 1, _ => flag = 2);
        Assert.AreEqual (1, flag);
    }

    [TestMethod]
    [Description ("Выбор результатов: третий вариант")]
    public void ParsingResult_Switch_3()
    {
        var flag = -1;
        var result = ParsingResult.Skip;
        result.Switch (_ => flag = 0, _ => flag = 1, _ => flag = 2);
        Assert.AreEqual (2, flag);
    }

    [TestMethod]
    [Description ("Выбор результатов: первый вариант")]
    public void ParsingResult_Match_1()
    {
        const string hello = "hello";
        var result = ParsingResult.Of (hello);
        var code = result.Match (_ => 100, _ => 200, _ => 300);
        Assert.AreEqual (100, code);
    }
    [TestMethod]
    [Description ("Выбор результатов: второй вариант")]
    public void ParsingResult_Match_2()
    {
        var result = ParsingResult.End;
        var code = result.Match (_ => 100, _ => 200, _ => 300);
        Assert.AreEqual (200, code);
    }

    [TestMethod]
    [Description ("Выбор результатов: тратий вариант")]
    public void ParsingResult_Match_3()
    {
        var result = ParsingResult.Skip;
        var code = result.Match (_ => 100, _ => 200, _ => 300);
        Assert.AreEqual (300, code);
    }

    [TestMethod]
    [Description ("Неявное преобразование в строку")]
    public void ParsingResult_Operator_1()
    {
        const string hello = "hello";
        var result = ParsingResult.Of (hello);
        string text = result;
        Assert.AreEqual (hello, text);
    }

    [TestMethod]
    [Description ("Результат: да")]
    public void Yes_Value_1()
    {
        Assert.IsNotNull (Yes.Value);
    }

    [TestMethod]
    [Description ("Результат: да плюс значение")]
    public void Yes_Value_2()
    {
        const int value = 123;
        var yes = new Yes<int> (value);
        Assert.AreEqual (value, yes.Value);
    }

    [TestMethod]
    [Description ("Результат: нет")]
    public void No_Value_1()
    {
        Assert.IsNotNull (No.Value);
    }

    [TestMethod]
    [Description ("Результат: неизвестно")]
    public void Unknown_Value_1()
    {
        Assert.IsNotNull (Unknown.Value);
    }

    [TestMethod]
    [Description ("Результат: истина")]
    public void True_Value_1()
    {
        Assert.IsNotNull (True.Value);
    }

    [TestMethod] [Description ("Результат: ложь")]
    public void False_Value_1()
    {
        Assert.IsNotNull (False.Value);
    }

    [TestMethod] [Description ("Результат: все")]
    public void All_Value_1()
    {
        Assert.IsNotNull (All.Value);
    }

    [TestMethod] [Description ("Результат: некоторые")]
    public void Some_Value_1()
    {
        Assert.IsNotNull (Some.Value);
    }

    [TestMethod] [Description ("Результат: достигнут конец")]
    public void End_Value_1()
    {
        Assert.IsNotNull (End.Value);
    }

    [TestMethod] [Description ("Результат: пропуск")]
    public void Skip_Value_1()
    {
        Assert.IsNotNull (Skip.Value);
    }

    [TestMethod] [Description ("Результат: ни одного")]
    public void None_Value_1()
    {
        Assert.IsNotNull (None.Value);
    }

    [TestMethod] [Description ("Результат: не найдено")]
    public void NotFound_Value_1()
    {
        Assert.IsNotNull (NotFound.Value);
    }

    [TestMethod] [Description ("Результат: успех")]
    public void Success_Value_1()
    {
        Assert.IsNotNull (Success.Value);
    }

    [TestMethod]
    [Description ("Результат: успех плюс значение")]
    public void Success_Value_2()
    {
        const int value = 123;
        var success = new Success<int> (value);
        Assert.AreEqual (value, success.Value);
    }

    [TestMethod] [Description ("Результат: ошибка")]
    public void Error_Value_1()
    {
        Assert.IsNotNull (Error.Value);
    }

    [TestMethod]
    [Description ("Результат: ошибка плюс значение")]
    public void Error_Value_2()
    {
        const int value = 123;
        var error = new Error<int> (value);
        Assert.AreEqual (value, error.Value);
    }

    [TestMethod]
    [Description ("Результат плюс значение")]
    public void Result_Value_1()
    {
        const int value = 123;
        var result = new Result<int> (value);
        Assert.IsTrue (result.IsSuccess);
        Assert.AreEqual (value, result.Value);
    }

    [TestMethod]
    [Description ("Результат без значения")]
    [ExpectedException (typeof (InvalidOperationException))]
    public void Result_Value_2()
    {
        var result = Result<int>.Failure;
        _ = result.Value;
    }

    [TestMethod]
    [Description ("Результат: сообщение об ошибке")]
    public void Result_Fail_1()
    {
        const string message = "Error occurred";
        var result = Result<int>.Fail (message);
        Assert.IsFalse (result.IsSuccess);
        Assert.AreEqual (message, result.Message);
    }

    [TestMethod]
    [Description ("Результат: общий сбой")]
    public void Result_Failure_1()
    {
        Assert.IsNotNull (Result<int>.Failure);
    }
}
