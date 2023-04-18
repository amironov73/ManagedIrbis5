// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Results;

#endregion

#nullable enable

namespace UnitTests.AM;

[TestClass]
public sealed class ResultsTest
{
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
}
