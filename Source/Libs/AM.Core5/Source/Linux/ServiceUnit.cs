// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ServiceUnit.cs -- обертка над файлом .service
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Linux;

/*
    https://www.freedesktop.org/software/systemd/man/systemd.unit.html
    https://linux-notes.org/pishem-systemd-unit-fajl/
    https://habr.com/ru/company/southbridge/blog/255845/
    https://fedoraproject.org/wiki/GSOC_2017/Student_application_amitosh/DotNet_core_wrapper_for_systemd?rd=GSOC_2017/Student_application_amitosh_dotnet_core_wrapper_for_systemd
    https://github.com/tmds/Tmds.Systemd

    Пример файла:

    [Unit]
    Description=MyUnit
    After=syslog.target
    After=network.target
    After=nginx.service
    After=mysql.service
    Requires=mysql.service
    Wants=redis.service

    [Service]
    Type=forking
    PIDFile=/work/www/myunit/shared/tmp/pids/service.pid
    WorkingDirectory=/work/www/myunit/current

    User=myunit
    Group=myunit

    Environment=RACK_ENV=production

    OOMScoreAdjust=-1000

    ExecStart=/usr/local/bin/bundle exec service -C /work/www/myunit/shared/config/service.rb --daemon
    ExecStop=/usr/local/bin/bundle exec service -S /work/www/myunit/shared/tmp/pids/service.state stop
    ExecReload=/usr/local/bin/bundle exec service -S /work/www/myunit/shared/tmp/pids/service.state restart
    TimeoutSec=300

    [Install]
    WantedBy=multi-user.target

 */

/// <summary>
/// Обертка над файлом .service.
/// </summary>
public sealed class ServiceUnit
    : IVerifiable
{
    #region Properties

    #region Секция Unit

    /// <summary>
    /// Описание юнита в произвольной форме.
    /// </summary>
    public string? UnitDescription { get; set; }

    /// <summary>
    /// Юниты, перечисленные в этой директиве, не будут запущены до тех пор,
    /// пока текущий блок не будет отмечен как запущенный, если они будут
    /// активированы одновременно.
    /// </summary>
    /// <example>
    /// <code>
    /// Before=some.target
    /// </code>
    /// </example>
    public string[]? Before { get; set; }

    /// <summary>
    /// Запускать юнит после какого-либо сервиса или группы сервисов.
    /// </summary>
    /// <example>
    /// <code>
    /// After=syslog.target
    /// After=network.target
    /// After=nginx.service
    /// After=mysql.service
    /// </code>
    /// </example>
    public string[]? After { get; set; }

    /// <summary>
    /// Данный юнит можно использовать для отображения юнитов, которые
    /// нельзя запускать одновременно с текущим устройством. Запуск устройства
    /// с этой связью приведет к остановке других устройств.
    /// </summary>
    /// <example>
    /// <code>
    /// Conflicts=other.target
    /// </code>
    /// </example>
    public string[]? Conflicts { get; set; }

    /// <summary>
    /// Существует ряд директив, начинающихся с условия, которые позволяют
    /// администратору протестировать определенные условия до запуска устройства.
    /// Это можно использовать для предоставления файла универсального элемента,
    /// который будет запускаться только в соответствующих системах.
    /// Если условие не выполнено, юнит — пропускается.
    /// </summary>
    public string[]? Conditions { get; set; }

    /// <summary>
    /// Подобно директиве <see cref="Conditions"/>, но установленные директивы
    /// проверяют различные аспекты рабочей среды, чтобы решить, следует ли активировать
    /// устройство. Однако, в отличие от директив Condition…=, отрицательный результат
    /// вызывает сбой в этой директиве.
    /// </summary>
    public string[]? Assertions { get; set; }

    /// <summary>
    /// Для запуска сервиса необходимы запущенные другие сервисы.
    /// </summary>
    /// <example>
    /// <code>
    /// Requires=mysql.service
    /// </code>
    /// </example>
    public string[]? Requires { get; set; }

    /// <summary>
    /// Эта директива аналогична <see cref="Requires"/>, но также приводит
    /// к остановке текущего устройства, когда соответствующий узел завершается.
    /// </summary>
    public string[]? BindsTo { get; set; }

    /// <summary>
    /// Ждя запуска сервиса желательные запущенные другие сервисы.
    /// </summary>
    /// <example>
    /// <code>
    /// Wants=redis.service
    /// </code>
    /// </example>
    /// <remarks>
    /// Чисто пожелание.
    /// </remarks>
    public string[]? Wants { get; set; }

    #endregion

    #region Секция Service

    /// <summary>
    /// Тип сервиса.
    /// </summary>
    /// <example>
    /// Type=simple
    /// </example>
    /// <remarks>
    /// <para><c>Type=simple</c> (по умолчанию): systemd предполагает, что служба
    /// будет запущена незамедлительно. Процесс при этом не должен разветвляться.
    /// Не используйте этот тип, если другие службы зависят от очередности
    /// при запуске данной службы.
    /// </para>
    /// <para><c>Type=forking</c>: systemd предполагает, что служба запускается
    /// однократно и процесс разветвляется с завершением родительского процесса.
    /// Данный тип используется для запуска классических демонов.
    /// </para>
    /// </remarks>
    public string? ServiceType { get; set; }

    /// <summary>
    /// Файл с идентификатором процесса, позволяющий systemd. отслеживать основной
    /// процесс при <c>Type=forking</c>.
    /// </summary>
    /// <example>
    /// PIDFile=/work/www/myunit/shared/tmp/pids/service.pid
    /// </example>
    public string? PidFile { get; set; }

    /// <summary>
    /// Рабочий каталог, его устанавливает systemd перед запуском сервиса.
    /// </summary>
    /// <example>
    /// WorkingDirectory=/usr/opt/irbis_server
    /// </example>
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// Переменные окружения.
    /// </summary>
    /// <example>
    /// Environment=LD_LIBRARY_PATH=/usr/opt/irbis_server:RACK_ENV=production
    /// </example>
    public string[]? EnvironmentVariables { get; set; }

    /// <summary>
    /// Пользователь, под которым надо стартовать сервис.
    /// </summary>
    /// <example>
    /// User=librarian
    /// </example>
    public string? User { get; set; }

    /// <summary>
    /// Группа, под которой надо стартовать сервис.
    /// </summary>
    /// <example>
    /// Group=librarians
    /// </example>
    public string? Group { get; set; }

    /// <summary>
    /// Запрет на убийство сервиса при нехватке памяти
    /// и срабатывании механизма OOM: -1000 означает полный запрет
    /// (такой установлен у sshd), -100 означает пониженную вероятность.
    /// </summary>
    public int? OutOfMemory { get; set; }

    /// <summary>
    /// Команда на запуск сервиса.
    /// </summary>
    /// <example>
    /// ExecStart=/usr/local/bin/bundle exec service -C /work/www/myunit/shared/config/service.rb --daemon
    /// </example>
    /// <remarks>
    /// Тут есть тонкость — systemd настаивает, чтобы команда указывала на конкретный исполняемый файл.
    /// Надо указывать полный путь.
    /// </remarks>
    public string? ExecStart { get; set; }

    /// <summary>
    /// Команда на остановку сервиса.
    /// </summary>
    /// <example>
    /// ExecStop=/usr/local/bin/bundle exec service -S /work/www/myunit/shared/tmp/pids/service.state stop
    /// </example>
    /// <remarks>
    /// Тут есть тонкость — systemd настаивает, чтобы команда указывала на конкретный исполняемый файл.
    /// Надо указывать полный путь.
    /// </remarks>
    public string? ExecStop { get; set; }

    /// <summary>
    /// Команда на перезапуск сервиса.
    /// </summary>
    /// <example>
    /// ExecReload=/usr/local/bin/bundle exec service -S /work/www/myunit/shared/tmp/pids/service.state restart
    /// </example>
    /// <remarks>
    /// Тут есть тонкость — systemd настаивает, чтобы команда указывала на конкретный исполняемый файл.
    /// Надо указывать полный путь.
    /// </remarks>
    public string? ExecReload { get; set; }

    /// <summary>
    /// Таймаут в секундах, сколько ждать отработки команд старт/стоп.
    /// </summary>
    /// <example>
    /// TimeoutSec=300
    /// </example>
    public int? Timeout { get; set; }

    /// <summary>
    /// Автоматический перезапуск сервиса, если он вдруг перестает работать.
    /// Контроль ведется по наличию процесса из PID-файла.
    /// </summary>
    /// <example>
    /// Restart=always
    /// </example>
    public string? Restart { get; set; }

    #endregion

    #region Секция Install

    /// <summary>
    /// Уровань запуска сервиса.
    /// </summary>
    /// <example>
    /// WantedBy=multi-user.target
    /// </example>
    /// <remarks>
    /// <c>multi-user.target</c> или <c>runlevel3.target</c> соответствует нашему
    /// привычному <c>runlevel=3</c> «Многопользовательский режим без графики.
    /// Пользователи, как правило, входят в систему при помощи множества консолей
    /// или через сеть».
    /// </remarks>
    public string? WantedBy { get; set; }

    #endregion

    #region Private members

    private static string[] Append
        (
            string[]? array,
            string value
        )
    {
        if (array is null)
        {
            array = new string[1];
        }
        else
        {
            Array.Resize (ref array, array.Length + 1);
        }

        array[^1] = value;

        return array;
    }

    private static void Write
        (
            TextWriter writer,
            string name,
            string? value
        )
    {
        if (!string.IsNullOrEmpty (value))
        {
            writer.WriteLine ($"{name}={value}");
        }
    }

    private static void Write
        (
            TextWriter writer,
            string name,
            IEnumerable<string>? values
        )
    {
        if (values is not null)
        {
            foreach (var value in values)
            {
                Write (writer, name, value);
            }
        }
    }

    #endregion

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстового представления.
    /// </summary>
    public void Parse
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        while (reader.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace (line))
            {
                continue;
            }

            if (!line.Contains ('='))
            {
                continue;
            }

            var parts = line.Split ('=', 2, StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                continue;
            }

            var key = parts[0];
            var value = parts[1];
            switch (key)
            {
                case "Description":
                    UnitDescription = value;
                    break;

                case "After":
                    After = Append (After, value);
                    break;

                case "Before":
                    Before = Append (Before, value);
                    break;

                case "Requires":
                    Requires = Append (Requires, value);
                    break;

                case "Wants":
                    Wants = Append (Wants, value);
                    break;

                case "User":
                    User = value;
                    break;

                case "Group":
                    Group = value;
                    break;

                case "Environment":
                    EnvironmentVariables = Append (EnvironmentVariables, value);
                    break;

                case "WorkingDirectory":
                    WorkingDirectory = value;
                    break;

                case "PIDFile":
                    PidFile = value;
                    break;

                case "ExecStart":
                    ExecStart = value;
                    break;

                case "ExecStop":
                    ExecStop = value;
                    break;

                case "ExecReload":
                    ExecReload = value;
                    break;

                case "TimeoutSec":
                    Timeout = value.SafeToInt32();
                    break;

                case "WantedBy":
                    WantedBy = value;
                    break;

                default:
                    Magna.Debug ($"Unknown key: {key}");
                    break;
            }
        }
    }

    /// <summary>
    /// Разбор текстового представления.
    /// </summary>
    public void Parse
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var reader = File.OpenText (fileName);

        Parse (reader);
    }

    /// <summary>
    /// Запись в текстовое представление.
    /// </summary>
    public void WriteTo
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteLine ("[Unit]");
        Write (writer, "Description", UnitDescription);
        Write (writer, "After", After);
        Write (writer, "Requires", Requires);
        Write (writer, "Wants", Wants);
        writer.WriteLine();

        writer.WriteLine("[Service]");
        Write (writer, "Type", ServiceType);
        Write (writer, "PIDFile", PidFile);
        Write (writer, "WorkingDirectory", WorkingDirectory);
        Write (writer, "Environment", EnvironmentVariables);
        writer.WriteLine();

        writer.WriteLine("[Install]");
        Write (writer, "WantedBy", WantedBy);
    }

    /// <summary>
    /// Запись в файл в текстовом представлении.
    /// </summary>
    public void WriteTo
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        using var writer = File.CreateText (fileName);
        WriteTo (writer);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ServiceUnit> (this, throwOnError);

        verifier
            .NotNullNorEmpty (UnitDescription)
            .NotNullNorEmpty (ExecStart);

        return verifier.Result;
    }

    #endregion
}
