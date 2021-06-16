// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ServerUtility.cs -- полезные методы, применяемые в серверном движке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Полезные методы, применяемые в серверном движке
    /// </summary>
    public static class ServerUtility
    {
        #region Constants

        /// <summary>
        /// Inclusion begin sign.
        /// </summary>
        public const char InclusionStart = '\x1C';

        /// <summary>
        /// Inclusion end sign.
        /// </summary>
        public const char InclusionEnd = '\x1D';

        /// <summary>
        /// Признак того, что client_m.mnu зашифрован.
        /// </summary>
        /// <remarks>Irbis64_Crypted</remarks>
        public static byte[] EncryptionMark =
        {
            0x49, 0x72, 0x62, 0x69, 0x73, 0x36, 0x34, 0x5F,
            0x43, 0x72, 0x79, 0x70, 0x74, 0x65, 0x64
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Create the engine.
        /// </summary>
        public static ServerEngine CreateEngine
            (
                string[] arguments
            )
        {
            // CommandLineParser parser = new CommandLineParser();
            // ParsedCommandLine parsed = parser.Parse(arguments);
            ServerSetup setup;

            // string logPath = parsed.GetValue("log", null);
            // if (!string.IsNullOrEmpty(logPath))
            // {
            //     TeeLogger tee = Log.Logger as TeeLogger;
            //     if (!ReferenceEquals(tee, null))
            //     {
            //         tee.Loggers.Add(new FileLogger(logPath));
            //     }
            // }

            // if (!ReferenceEquals(Log.Logger, null))
            // {
            //     Log.SetLogger(new TimeStampLogger(Log.Logger.ThrowIfNull()));
            // }

            // if (parsed.HaveSwitch("nolog"))
            // {
            //     Log.SetLogger(null);
            // }

            var iniPath = Path.Combine(AppContext.BaseDirectory, "irbis_server.ini");
            // iniPath = parsed.GetArgument(0, iniPath).ThrowIfNull("iniPath");
            iniPath = Path.GetFullPath(iniPath);

            var iniFile = new IniFile(iniPath, IrbisEncoding.Ansi, false);
            var serverIniFile = new ServerIniFile(iniFile);
            setup = new ServerSetup(serverIniFile)
            {
                //RootPathOverride = PathUtility.ExpandHomePath(parsed.GetValue("root", null)),
                //WorkdirOverride = PathUtility.ExpandHomePath(parsed.GetValue("workdir", null)),
                //PortNumberOverride = parsed.GetValue("port", 0)
            };

            // if (parsed.HaveSwitch("noipv4"))
            // {
            //     setup.UseTcpIpV4 = false;
            // }

            // if (parsed.HaveSwitch("ipv6"))
            // {
            //     setup.UseTcpIpV6 = true;
            // }

            // int httpPort = parsed.GetValue("http", 0);
            // if (httpPort > 0)
            // {
            //     setup.HttpPort = httpPort;
            // }

            // if (parsed.HaveSwitch("break"))
            // {
            //     setup.Break = true;
            // }

            var result = new ServerEngine(setup);

            return result;

        } // method CreateEngine

        /// <summary>
        /// Dump engine settings.
        /// </summary>
        public static void DumpEngineSettings
            (
                ServerEngine engine
            )
        {
            Magna.Info(GetServerVersion().ToString());
            Magna.Trace("BUILD: " + ClientVersion.Version);

            foreach (var listener in engine.Listeners)
            {
                Magna.Info("Listening " + listener.GetLocalAddress());
            }

        } // method DumpEngineSettings

        /// <summary>
        /// Expand inclusion.
        /// </summary>
        public static string ExpandInclusion
            (
                string text,
                string extension,
                string[] pathArray
            )
        {
            Sure.NotNull(text, nameof(text));
            Sure.NotNull(extension, nameof(extension));
            Sure.NotNull(pathArray, nameof(pathArray));

            if (!text.Contains(InclusionStart))
            {
                return text;
            }

            if (pathArray.Length == 0)
            {
                throw new IrbisException();
            }

            var result = new StringBuilder(text.Length * 2);
            var navigator = new TextNavigator(text);

            var fileName = navigator.ReadUntil(InclusionEnd).ToString();
            while (!navigator.IsEOF)
            {
                var prefix = navigator.ReadUntil(InclusionStart).ToString();
                result.Append(prefix);
                if (navigator.ReadChar() != InclusionStart)
                {
                    break;
                }

                if (ReferenceEquals(fileName, null)
                    || fileName.Length == 0
                    || navigator.ReadChar() != InclusionEnd)
                {
                    break;
                }

                var fullPath = FindFileOnPath
                    (
                        fileName,
                        extension,
                        pathArray
                    );
                var fileContent = File.ReadAllText
                    (
                        fullPath,
                        IrbisEncoding.Ansi
                    );
                fileContent = ExpandInclusion
                    (
                        fileContent,
                        extension,
                        pathArray
                    );
                result.Append(fileContent);
            }

            var remaining = navigator.GetRemainingText().ToString();
            result.Append(remaining);

            return result.ToString();

        } // method ExpandInclusion

        /// <summary>
        /// Find file on path.
        /// </summary>
        public static string FindFileOnPath
            (
                string fileName,
                string extension,
                string[] pathArray
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));
            Sure.NotNull(extension, nameof(extension));
            Sure.NotNull(pathArray, nameof(pathArray));

            if (!fileName.Contains('.'))
            {
                if (!extension.StartsWith("."))
                {
                    fileName += '.';
                }
                fileName += extension;
            }

            foreach (var path in pathArray)
            {
                var fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            throw new IrbisException();

        } // method FindFileOnPath

        /// <summary>
        /// Get server version.
        /// </summary>
        public static ServerVersion GetServerVersion()
        {
            var result = new ServerVersion
            {
                Version = "64.2012.1",
                Organization = "Open source version",
                MaxClients = int.MaxValue
            };

            return result;

        } // method GetServerVersion

        /// <summary>
        /// Загрузка client_m.mnu с учетом того,
        /// что тот может быть зашифрован.
        /// </summary>
        public static UserInfo[] LoadClientList
            (
                string fileName,
                MenuFile clientIni
            )
        {
            var rawContent = File.ReadAllBytes(fileName);
            if (!ArrayUtility.Coincide(rawContent, 0, EncryptionMark,
                0, EncryptionMark.Length))
            {
                return UserInfo.ParseFile(fileName, clientIni);
            }

            var shift = EncryptionMark.Length;
            var length = rawContent.Length - shift;
            for (var i = 0; i < length; i++)
            {
                var mask = i % 2 == 0 ? (byte)0xE6 : (byte)0x14;
                rawContent[i + shift] = (byte) (rawContent[i + shift] ^ mask);
            }

            var decryptedText = IrbisEncoding.Ansi.GetString(rawContent, shift, length);
            using var reader = new StringReader(decryptedText);

            return UserInfo.ParseStream(reader, clientIni);

        } // method LoadClientList

        #endregion

    } // class ServerUtility

} // namespace ManagedIrbis.Server
