// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Context.cs -- контекст исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.Collections;
using AM.Lexey.Barsik.Ast;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Lexey.Barsik;

/// <summary>
/// Контекст исполнения скрипта.
/// </summary>
[PublicAPI]
public sealed class Context
{
    #region Properties

    /// <summary>
    /// Общая часть контекста (чтобы не копировать
    /// по всем дочерним контекстам).
    /// </summary>
    public CommonContext Commmon { get; }

    /// <summary>
    /// Родительский контекст.
    /// </summary>
    public Context? Parent { get; }

    /// <summary>
    /// Переменные.
    /// </summary>
    public Dictionary<string, dynamic?> Variables { get; }

    /// <summary>
    /// Функции.
    /// </summary>
    public Dictionary<string, FunctionDescriptor> Functions { get; }

    /// <summary>
    /// Опциональный префикс, используемый, например, в операторе "new"
    /// при инициализации свойств свежесозданного объекта.
    /// </summary>
    public AtomNode? With { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Context
        (
            Context? parent = null
        )
    {
        Parent = parent;
        Variables = new ();
        Functions = new ();
        Commmon = parent?.Commmon ?? new ();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Конструирование типа, если необходимо.
    /// </summary>
    private Type ConstructType
        (
            Type mainType,
            Type[]? typeArguments
        )
    {
        if (typeArguments is null)
        {
            return mainType;
        }

        if (!mainType.IsGenericType)
        {
            // TODO более осмысленная реакция
            return mainType;
        }

        return mainType.MakeGenericType (typeArguments);
    }

    internal bool EnsureVariableCanBeAssigned
        (
            string variableName
        )
    {
        Sure.NotNullNorEmpty (variableName);

        var tokenizerSettings = Commmon.Settings.TokenizerSettings;
        if (tokenizerSettings is not null
            && (tokenizerSettings.KnownTerms.Contains (variableName)
            || tokenizerSettings.ReservedWords.Contains (variableName)))
        {
            return false;
        }

        if (Commmon.Defines.ContainsKey (variableName))
        {
            return false;
        }

        if (FindFunction (variableName, out _))
        {
            return false;
        }

        return true;
    }

    private bool TryInclude
        (
            string fileName
        )
    {
        if (!Commmon.Inclusions.TryGetValue (fileName, out var program))
        {
            if (!File.Exists (fileName))
            {
                return false;
            }

            // var sourceCode = File.ReadAllText(fileName);
            // program = Commmon.Settings.Grammar.ParseProgram (sourceCode, Commmon.Settings.Tokenizer);
            // Commmon.Inclusions[fileName] = program;
        }

        // TODO отрабатывать ExitException?
        // Execute (program);

        return true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подключение модуля.
    /// </summary>
    public void AttachModule
        (
            IBarsikModule instance
        )
    {
        Sure.NotNull (instance);

        if (!Commmon.Modules.Contains (instance)
            && instance.AttachModule (this))
        {
            Commmon.Modules.Add (instance);
        }
    }

    /// <summary>
    /// Создание контекста-потомка.
    /// </summary>
    public Context CreateChildContext ()
    {
        return new Context (this);
    }

    /// <summary>
    /// Дамп пространств имен.
    /// </summary>
    public void DumpNamespaces()
    {
        var keys = Commmon.Resolver.Namespaces.ToArray();
        if (keys.IsNullOrEmpty())
        {
            Commmon.Output?.WriteLine ("(no namespaces)");
            return;
        }

        Array.Sort (keys);
        foreach (var key in keys)
        {
            Commmon.Output?.WriteLine (key);
        }
    }

    /// <summary>
    /// Дамп переменных.
    /// </summary>
    public void DumpVariables()
    {
        var keys = Variables.Keys.ToArray();

        Array.Sort (keys);
        foreach (var key in keys)
        {
            var value = Variables[key];
            Commmon.Output?.WriteLine
                (
                    value is null
                        ? $"{key}: (null)"
                        : $"{key}: {value.GetType().Name} = {value}"
                );
        }

        if (keys.Length == 0)
        {
            Commmon.Output?.WriteLine ("(no variables in the context)");
        }
    }

    /// <summary>
    /// Вычисление значения переменной.
    /// </summary>
    public AtomNode EvaluateAtom
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        throw new NotImplementedException();

        // var settings = Commmon.Settings;
        // var node = settings.Grammar.ParseExpression
        //     (
        //         sourceCode,
        //         settings.Tokenizer,
        //         Commmon.ParsingDebugOutput
        //     );
        // if (settings.DumpAst && Commmon.Output is { } output)
        // {
        //     output.WriteLine (new string ('=', 60));
        //     node.DumpHierarchyItem (null, 0, output);
        //     output.WriteLine (new string ('=', 60));
        // }

        // return node;
    }

    /// <summary>
    /// Запуск скрипта на исполнение.
    /// </summary>
    public ExecutionResult Execute
        (
            ProgramNode program
        )
    {
        Sure.NotNull (program);

        var haveDefinitions = false;
        foreach (var statement in program.Statements)
        {
            if (statement is FunctionDefinitionNode node)
            {
                haveDefinitions = true;
                var name = node.Name;
                if (Builtins.IsBuiltinFunction (name))
                {
                    Magna.Logger.LogError ("Name {Name} used by builtin function", name);
                    throw new BarsikException ($"Name {name} used by builtin function");
                }

                var definition = new FunctionDefinition
                    (
                        name,
                        node.argumentNames,
                        node.Body
                    );
                var descriptor = new FunctionDescriptor
                    (
                        name,
                        definition.CreateCallPoint()
                    );
                Functions[name] = descriptor;
            }
        }

        if (haveDefinitions)
        {
            program.Statements = program.Statements
                .Where (stmt => stmt is not PseudoNode)
                .ToList();
        }

        var result = new ExecutionResult();
        try
        {
            program.Execute (this);
        }
        catch (ReturnException exception)
        {
            result.ExitRequested = true;
            result.ExitCode = BarsikUtility.ToInt32 (exception.Value);
            result.Message = exception.Message;
        }
        catch (ExitException exception)
        {
            result.ExitRequested = true;
            result.ExitCode = exception.ExitCode;
            result.Message = exception.Message;
        }

        Commmon.Settings.VariableDumper?.DumpContext (this);

        return result;
    }

    /// <summary>
    /// Поиск функции с указанным именем.
    /// Отличается тем, что не швыряется исключениями.
    /// </summary>
    public bool FindFunction
        (
            string name,
            [MaybeNullWhen (false)] out FunctionDescriptor result
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Functions.TryGetValue (name, out result))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Functions.TryGetValue (name, out result))
            {
                return true;
            }
        }

        return Builtins.Registry.TryGetValue (name, out result);
    }

    /// <summary>
    /// Получение типа по его имени.
    /// </summary>
    public Type? ResolveType
        (
            string name,
            IList<string>? arguments = null
        )
    {
        Sure.NotNullNorEmpty (name);

        var descriptor = new Types.TypeDescriptor
        {
            TypeName = name,
            GenericParameters = arguments
        };
        return Commmon.Resolver.ResolveType (descriptor);
    }

    /// <summary>
    /// Поиск функции в текущем и в родительском контекстах.
    /// </summary>
    public FunctionDescriptor ResolveFunction
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        if (!FindFunction (name, out var result))
        {
            Magna.Logger.LogError ("Function {Name} not found", name);
            throw new Exception ($"Function {name} not found");
        }

        return result;
    }

    /// <summary>
    /// Получение корневого контекста, используемого как свалка
    /// для регистрации модулей и сборок.
    /// </summary>
    public Context GetRootContext()
    {
        var result = this;

        while (result.Parent is not null)
        {
            result = result.Parent;
        }

        return result;
    }

    /// <summary>
    /// Вложение скрипта - файла на диске.
    /// </summary>
    public void Include
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var logger = Magna.Logger;
        var extension = Path.GetExtension (fileName);
        if (string.IsNullOrEmpty (extension))
        {
            fileName += ".meow";
        }

        // сначала пытаемся найти в текущей директории
        // или по полному пути (если он указан)
        if (TryInclude (fileName))
        {
            return;
        }

        if (Path.IsPathRooted (fileName))
        {
            throw new BarsikException ($"Can't include '{fileName}'");
        }

        // ищем по путям (INCLUDE)
        foreach (var part in Commmon.Settings.Paths)
        {
            var fullPath = Path.Combine (part, fileName);
            if (TryInclude (fullPath))
            {
                return;
            }
        }

        // пытаемся загрузить файл рядом со скриптом
        if (TryGetVariable ("__DIR__", out var scriptDir))
        {
            var fullPath = Path.Combine (scriptDir, fileName);
            if (TryInclude (fullPath))
            {
                return;
            }
        }

        logger.LogError
            (
                "Can't include '{FileName}'",
                fileName
            );
        throw new BarsikException ($"Can't include '{fileName}'");
    }

    /// <summary>
    /// Загрузка сборки.
    /// </summary>
    public Assembly? LoadAssembly
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        // TODO сделать умное сравнение имен сборок
        name = name.Trim();
        var logger = Magna.Logger;
        if (string.IsNullOrEmpty (name))
        {
            throw new BarsikException();
        }

        foreach (var assembly in Commmon.Resolver.Assemblies)
        {
            if (assembly.GetName().Name.SameString (name))
            {
                // уже загружено, пропускаем
                logger.LogInformation
                    (
                        "Assembly {Name} is already loaded",
                        name
                    );
                return assembly;
            }
        }

        var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in allAssemblies)
        {
            var asmName = assembly.GetName().Name;
            if (asmName.SameString (name))
            {
                // уже загружено, пропускаем, предварительно добавив в список
                logger.LogInformation
                    (
                        "Assembly {Name} is already loaded",
                        name
                    );
                Commmon.Resolver.Assemblies.Add (assembly);
                return assembly;
            }
        }

        Assembly? result = null;
        if (File.Exists (name))
        {
            var fullPath = Path.GetFullPath (name);
            result = Assembly.LoadFile (fullPath);
        }
        else
        {
            try
            {
                result = Assembly.Load (name);
            }
            catch
            {
                // просто съедаем исключение
            }
        }

        if (result is not null)
        {
            Commmon.Resolver.Assemblies.Add (result);
            return result;
        }

        var extension = Path.GetExtension (name);
        if (string.IsNullOrEmpty (extension))
        {
            name += ".dll";
        }
        else
        {
            if (extension.ToLowerInvariant () != ".dll")
            {
                name += ".dll";
            }
        }

        foreach (var part in Commmon.Settings.Paths)
        {
            var fullPath = Path.GetFullPath (Path.Combine (part, name));
            if (File.Exists (fullPath))
            {
                result = Assembly.LoadFile (fullPath);
                Commmon.Resolver.Assemblies.Add (result);
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка указанного модуля.
    /// </summary>
    public void LoadModule
        (
            ModuleDefinition moduleDefinition
        )
    {
        Sure.NotNull (moduleDefinition);
        Sure.NotNull (moduleDefinition.ModuleType);

        if (moduleDefinition.AssembliesToLoad is { } assembliesToLoad)
        {
            foreach (var assemblyName in assembliesToLoad)
            {
                LoadAssembly (assemblyName);
            }
        }

        var moduleTypeName = moduleDefinition.ModuleType.ThrowIfNull();
        LoadModule (moduleTypeName);
    }

    /// <summary>
    /// Загрузка модуля по его имени.
    /// </summary>
    /// <remarks>
    /// Нужно короткое (без namespace и не Assembly-qualified)
    /// имя типа.
    /// </remarks>
    public void LoadModule
        (
            string moduleTypeName
        )
    {
        Sure.NotNullNorEmpty (moduleTypeName);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var types = assembly.GetTypes()
                .Where (type => type.IsAssignableTo (typeof (IBarsikModule)))
                .ToArray();
            foreach (var type in types)
            {
                var alreadyHave = false;
                foreach (var module in Commmon.Modules)
                {
                    if (module.GetType() == type)
                    {
                        alreadyHave = true;
                        break;
                    }
                }

                if (
                        alreadyHave
                        || string.CompareOrdinal (type.Name, moduleTypeName) != 0
                   )
                {
                    continue;
                }

                var instance = (IBarsikModule?) Activator.CreateInstance (type);
                if (instance is not null)
                {
                    AttachModule (instance);
                }

                return;
            }
        }
    }

    /// <summary>
    /// Вывод на печать значения AST-узла.
    /// </summary>
    public void Print
        (
            AtomNode node
        )
    {
        Sure.NotNull (node);

        var value = node.Compute (this);
        BarsikUtility.PrintObject (Commmon.Output, value);
    }

    /// <summary>
    /// Удаление переменной с указанным именем.
    /// Если такой переменной не существует,
    /// то ничего не происходит.
    /// </summary>
    public void RemoveVariable
        (
            string variableName
        )
    {
        Sure.NotNullNorEmpty (variableName);

        for (var context = this; context is not null; context = context.Parent)
        {
            if (context.Variables.Remove (variableName))
            {
                return;
            }
        }
    }

    /// <summary>
    /// Сброс состояния интерпретатора.
    /// </summary>
    public void Reset()
    {
        Variables.Clear();
    }

    /// <summary>
    /// Установка значения дефайна
    /// (с сохранением места в контексте).
    /// </summary>
    public void SetDefine
        (
            string name,
            dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Builtins.IsBuiltinFunction (name))
        {
            Magna.Logger.LogError
                (
                    "Name {Name} is reserved for builtin function",
                    name
                );
            throw new BarsikException
                (
                    $"Name {name} is reserved for builtin function"
                );
        }

        for (var context = this; context is not null; context = context.Parent)
        {
            context.Variables.Remove (name);
        }

        Commmon.Defines[name] = value;
    }

    /// <summary>
    /// Установка значения переменной
    /// (с сохранением ее места в контексте).
    /// </summary>
    public void SetVariable
        (
            string name,
            dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        var logger = Magna.Logger;
        if (Builtins.IsBuiltinFunction (name))
        {
            logger.LogError ("{Name} reserved for builtin function", name);
            throw new BarsikException ($"{name} reserved for builtin function");
        }

        if (Commmon.Defines.ContainsKey (name))
        {
            logger.LogError ("Can't redefine {Name}", name);
            throw new BarsikException ($"Can't redefine {name}");
        }

        if (Variables.ContainsKey (name))
        {
            Variables[name] = value;
            return;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.Variables.ContainsKey (name))
            {
                context.Variables[name] = value;
                return;
            }
        }

        // сюда мы попадаем, если переменная не нашлась ни в одном из контекстов
        Variables[name] = value;
    }

    /// <summary>
    /// Поиск переменной в текущем и в родительских контекстах.
    /// </summary>
    public bool TryGetVariable
        (
            string name,
            out dynamic? value
        )
    {
        Sure.NotNullNorEmpty (name);

        if (Commmon.Defines.TryGetValue (name, out value) ||
            Variables.TryGetValue (name, out value))
        {
            return true;
        }

        for (var context = Parent; context is not null; context = context.Parent)
        {
            if (context.TryGetVariable (name, out value)
                || context.Variables.TryGetValue (name, out value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Выгрузка модуля.
    /// </summary>
    public bool UnloadModule
        (
            string moduleName
        )
    {
        Sure.NotNullNorEmpty (moduleName);

        IBarsikModule? found = null;
        foreach (var module in Commmon.Modules)
        {
            if (module.GetType().Name.SameString (moduleName))
            {
                found = module;
                break;
            }
        }

        if (found is not null)
        {
            found.DetachModule (this);
            Commmon.Modules.Remove (found);

            return true;
        }

        return false;
    }

    #endregion
}
