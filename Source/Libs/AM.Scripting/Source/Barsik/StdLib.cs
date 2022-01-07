// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* StdLib.cs -- стандартная библиотека
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static AM.Scripting.Barsik.Builtins;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Стандартная библиотека (включая небезопасные функции).
/// </summary>
public sealed class StdLib
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "array", new FunctionDescriptor ("array", Array_) },
        { "bon_decode", new FunctionDescriptor ("bon_decode", BonDecode) },
        { "call", new FunctionDescriptor ("call", CallAnyMethod) },
        { "chdir", new FunctionDescriptor ("chdir", ChangeDirectory) },
        { "combine_path", new FunctionDescriptor ("combine_path", CombinePath) },
        { "dir_exists", new FunctionDescriptor ("dir_exists", DirectoryExists) },
        { "dirname", new FunctionDescriptor ("dirname", DirectoryName) },
        { "eval", new FunctionDescriptor ("eval", Evaluate) },
        { "exec", new FunctionDescriptor ("exec", Execute) },
        { "file_exists", new FunctionDescriptor ("file_exists", FileExists) },
        { "file_get_contents", new FunctionDescriptor ("file_get_contents", FileGetContents) },
        { "file_put_contents", new FunctionDescriptor ("file_put_contents", FilePutContents) },
        { "fullpath", new FunctionDescriptor ("fullpath", FullPath) },
        { "getcwd", new FunctionDescriptor ("getcwd", GetCurrentDirectory) },
        { "host", new FunctionDescriptor ("host", Host) },
        { "include", new FunctionDescriptor ("include", Include) },
        { "json_decode", new FunctionDescriptor ("json_decode", JsonDecode) },
        { "json_encode", new FunctionDescriptor ("json_encode", JsonEncode) },
        { "load", new FunctionDescriptor ("load", LoadAssembly) },
        { "module", new FunctionDescriptor ("module", LoadModule) },
        { "readdir", new FunctionDescriptor ("readdir", ReadDirectory) },
        { "remove", new FunctionDescriptor ("remove", RemoveFile) },
        { "rename", new FunctionDescriptor ("rename", RenameFile) },
        { "subscribe", new FunctionDescriptor ("subscribe", Subscribe) },
        { "system", new FunctionDescriptor ("system", System_) },
        { "tmpfile", new FunctionDescriptor ("tmpfile", TemporaryFile) },
        { "type", new FunctionDescriptor ("type", Type_) },
        { "unsubscribe", new FunctionDescriptor ("unsubscribe", Unsubscribe) },
        { "use", new FunctionDescriptor ("use", Use) },
    };

    #endregion

    #region Private members

    private static bool _Include
        (
            Context context,
            string fileName
        )
    {
        if (!File.Exists (fileName))
        {
            return false;
        }

        var sourceCode = File.ReadAllText (fileName);
        var program = Grammar.ParseProgram (sourceCode);
        program.Execute (context);

        return true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание массива.
    /// </summary>
    public static dynamic? Array_
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return Array.Empty<object>();
        }

        var length = Convert.ToInt32 (Compute (context, args, 0));
        if (length <= 0)
        {
            return Array.Empty<object>();
        }

        var type = typeof (object);
        var typeName = Compute (context, args, 1) as string;
        if (!string.IsNullOrEmpty (typeName))
        {
            type = context.FindType (typeName);
            if (type is null)
            {
                context.Error.WriteLine ($"Can't find type {typeName}");
                return null;
            }
        }

        return Array.CreateInstance (type, length);
    }

    /// <summary>
    /// Декодирование BON-объекта.
    /// </summary>
    public static dynamic? BonDecode
        (
            Context context,
            dynamic?[] args
        )
    {
        var bon = ComputeAll (context, args);
        if (string.IsNullOrWhiteSpace (bon))
        {
            return null;
        }

        var interpreter = context.Interpreter;
        if (interpreter is null)
        {
            return null;
        }

        var atom = interpreter.Evaluate (bon);
        var value = atom.Compute (context);

        return value;
    }

    /// <summary>
    /// Динамический вызов произвольного метода.
    /// </summary>
    public static dynamic? CallAnyMethod
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            return null;
        }

        // первый аргумент - экземпляр или класс
        var target = Compute (context, args, 0);
        if (target is null)
        {
            return null;
        }

        if (target is string typeName)
        {
            target = context.FindType (typeName);
            if (target is null)
            {
                context.Error.WriteLine ($"Can't find type {typeName}");
                return null;
            }
        }

        var methodName = Compute (context, args, 1) as string;
        if (string.IsNullOrEmpty (methodName))
        {
            return null;
        }

        var preparedArgs = new List<object?>();
        for (var i = 2; i < args.Length; i++)
        {
            var one = Compute (context, args, i);
            preparedArgs.Add (one);
        }

        return BarsikUtility.CallAnyMethod (context, target, methodName, preparedArgs.ToArray());
    }

    /// <summary>
    /// Изменение текущей директории.
    /// </summary>
    public static dynamic? ChangeDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        var path = ComputeAll (context, args);
        if (string.IsNullOrWhiteSpace (path))
        {
            return null;
        }

        path = path.Trim();
        try
        {
            Environment.CurrentDirectory = path;
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error changing directory: {exception.Message}");
        }

        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Создание пути по его компонентам.
    /// </summary>
    public static dynamic? CombinePath
        (
            Context context,
            dynamic?[] args
        )
    {
        var components = new List<string>();
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                if (!string.IsNullOrEmpty (name))
                {
                    components.Add (name);
                }
            }
        }

        if (components.Count == 0)
        {
            return null;
        }

        return Path.Combine (components.ToArray());
    }

    /// <summary>
    /// Проверка существования папки.
    /// </summary>
    public static dynamic DirectoryExists
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        return Directory.Exists (fileName);
    }

    /// <summary>
    /// Проверка существования папки.
    /// </summary>
    public static dynamic? DirectoryName
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        return Path.GetDirectoryName (fileName);
    }

    /// <summary>
    /// Вычисление значения выражения.
    /// </summary>
    public static dynamic? Evaluate
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var expression = Grammar.ParseExpression (sourceCode);
            var result = expression.Compute (context);

            return result;
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Динамическое исполнение скрипта.
    /// </summary>
    public static dynamic? Execute //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var program = Interpreter.ParseProgram (sourceCode);
            foreach (var statement in program.Statements)
            {
                statement.Execute (context);
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Проверка существования файла.
    /// </summary>
    public static dynamic FileExists
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return false;
        }

        return File.Exists (fileName);
    }

    /// <summary>
    /// Чтение всего файла как строки.
    /// </summary>
    public static dynamic? FileGetContents
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        try
        {
            var encodingName = Compute (context, args, 1) as string;
            if (!string.IsNullOrEmpty (encodingName))
            {
                var encoding = Encoding.GetEncoding (encodingName);

                return File.ReadAllText (fileName, encoding);
            }

            return File.ReadAllText (fileName);
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error reading file {fileName}: {exception.Message}");
        }

        return null;
    }

    /// <summary>
    /// Запись всего файла как одной большой строки.
    /// </summary>
    public static dynamic? FilePutContents //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        var contents = Compute (context, args, 1) as string;

        try
        {
            var encodingName = Compute (context, args, 2) as string;
            if (!string.IsNullOrEmpty (encodingName))
            {
                var encoding = Encoding.GetEncoding (encodingName);

                File.WriteAllText (fileName, contents, encoding);
            }
            else
            {
                File.WriteAllText (fileName, contents);
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error writing file {fileName}: {exception.Message}");
        }

        return null;
    }

    /// <summary>
    /// Доступ к хосту.
    /// </summary>
    public static dynamic? FullPath
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = ComputeAll (context, args);
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        return Path.GetFullPath (fileName);
    }

    /// <summary>
    /// Получение текущей директории.
    /// </summary>
    public static dynamic GetCurrentDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Доступ к хосту.
    /// </summary>
    public static dynamic Host
        (
            Context context,
            dynamic?[] args
        )
    {
        return Magna.Host;
    }

    /// <summary>
    /// Загрузка указанного скрипта.
    /// </summary>
    public static dynamic? Include //-V3009
        (
            Context context,
            dynamic?[] args
        )
    {
        var includePath = Environment.GetEnvironmentVariable ("BARSIK_PATH");

        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                var extension = Path.GetExtension (name);
                if (string.IsNullOrEmpty (extension))
                {
                    name += ".barsik";
                }

                if (!_Include (context, name)
                    && !Path.IsPathRooted (name)
                    && !string.IsNullOrEmpty (includePath))
                {
                    var parts = includePath.Split
                        (
                            Path.PathSeparator,
                            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                        );
                    foreach (var part in parts)
                    {
                        var fullPath = Path.Combine (part, name);
                        if (_Include (context, fullPath))
                        {
                            return null;
                        }
                    }
                }

            }
        }

        return null;
    }

    /// <summary>
    /// Декодирование JSON.
    /// </summary>
    public static dynamic? JsonDecode
        (
            Context context,
            dynamic?[] args
        )
    {
        var json = ComputeAll (context, args);
        if (string.IsNullOrWhiteSpace (json))
        {
            return null;
        }

        return JObject.Parse (json);
    }

    /// <summary>
    /// Коодирование JSON.
    /// </summary>
    public static dynamic JsonEncode
        (
            Context context,
            dynamic?[] args
        )
    {
        var obj = Compute (context, args, 0);

        return JsonConvert.SerializeObject (obj);
    }

    /// <summary>
    /// Загрузка указанной сборки.
    /// </summary>
    public static dynamic? LoadAssembly
        (
            Context context,
            dynamic?[] args
        )
    {
        var topContext = context.GetTopContext();
        Assembly? result = null;
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                result = topContext.LoadAssembly (name);
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка указанного модуля.
    /// </summary>
    public static dynamic? LoadModule
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                context.LoadModule (name);
            }
        }

        return null;
    }

    /// <summary>
    /// Чтение списка файлов в указанной директории.
    /// </summary>
    public static dynamic ReadDirectory
        (
            Context context,
            dynamic?[] args
        )
    {
        var path = Compute (context, args, 0) as string ?? ".";
        var pattern = (Compute (context, args, 1) as string).EmptyToNull();

        return pattern is null
            ? Directory.GetFileSystemEntries (path)
            : Directory.GetFileSystemEntries (path, pattern);
    }

    /// <summary>
    /// Удаление файлов.
    /// </summary>
    public static dynamic? RemoveFile
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var path = Compute (context, args, i) as string;
            if (!string.IsNullOrEmpty (path))
            {
                try
                {
                    File.Delete (path);
                }
                catch (Exception exception)
                {
                    context.Error.WriteLine ($"Error removing file {path}: {exception.Message}");
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Переименование файла.
    /// </summary>
    public static dynamic? RenameFile
        (
            Context context,
            dynamic?[] args
        )
    {
        var oldName = Compute (context, args, 0) as string;
        var newName = Compute (context, args, 1) as string;
        var overwrite = BarsikUtility.ToBoolean (Compute (context, args, 2));
        if (string.IsNullOrEmpty (oldName) || string.IsNullOrEmpty (newName))
        {
            return null;
        }

        try
        {
            File.Move (oldName, newName, overwrite);
        }
        catch (Exception exception)
        {
            context.Error.WriteLine ($"Error renaming {oldName} to {newName}: {exception.Message}");
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Подписка на событие объекта.
    /// </summary>
    public static dynamic Subscribe
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 3)
        {
            throw new BarsikException ("Can't subscribe");
        }

        var target = Compute (context, args, 0);
        if (target is null)
        {
            throw new BarsikException ("Can't subscribe");
        }

        var eventName = Compute (context, args, 1) as string;
        if (string.IsNullOrEmpty (eventName))
        {
            throw new BarsikException ("Can't subscribe");
        }

        var handler = Compute (context, args, 2);
        if (handler is LambdaNode lambda)
        {
            // лямбда-функция
            return new EventPad (context, target, eventName, lambda.Adapter);
        }

        if (handler is FunctionDescriptor descriptor)
        {
            // свободная функция
            return new EventPad (context, target, eventName, descriptor.CallPoint);
        }

        throw new BarsikException ("Can't subscribe");
    }

    /// <summary>
    /// Выполнение внешней программы и получение ее выходного потока.
    /// </summary>
    public static dynamic? System_
        (
            Context context,
            dynamic?[] args
        )
    {
        var command = (string?) Compute (context, args, 0);
        if (string.IsNullOrWhiteSpace (command))
        {
            return null;
        }

        command = command.Trim();
        var startInfo = new ProcessStartInfo (command)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        for (var i = 1; i < args.Length; i++)
        {
            var value = (string?) Compute (context, args, i);
            if (!string.IsNullOrWhiteSpace (value))
            {
                startInfo.ArgumentList.Add (value.Trim());
            }
        }

        var process = Process.Start (startInfo);
        if (process is not null)
        {
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        return null;
    }

    /// <summary>
    /// Выдача типа по его имени.
    /// </summary>
    public static dynamic TemporaryFile
        (
            Context context,
            dynamic?[] args
        )
    {
        return Path.GetTempFileName();
    }

    /// <summary>
    /// Выдача типа по его имени.
    /// </summary>
    public static dynamic? Type_
        (
            Context context,
            dynamic?[] args
        )
    {
        var typeName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (typeName))
        {
            return null;
        }

        return context.FindType (typeName);
    }

    /// <summary>
    /// Отписка от нативного события.
    /// </summary>
    public static dynamic? Unsubscribe
        (
            Context context,
            dynamic?[] args
        )
    {
        foreach (var one in args)
        {
            if (one is EventPad pad)
            {
                pad.Unsubscribe();
            }
        }

        return null;
    }

    /// <summary>
    /// Подключение/отключение пространств имен.
    /// </summary>
    public static dynamic Use
        (
            Context context,
            dynamic?[] args
        )
    {
        var topContext = context.GetTopContext();
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                if (name.StartsWith ('-'))
                {
                    name = name.Substring (1);
                    if (!string.IsNullOrEmpty (name))
                    {
                        topContext.Namespaces.Remove (name);
                    }
                }
                else
                {
                    topContext.Namespaces[name] = null;
                }
            }
        }

        return context.Namespaces.Keys;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "StdLib";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
