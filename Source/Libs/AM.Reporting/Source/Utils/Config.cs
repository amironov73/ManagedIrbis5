// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Contains some configuration properties and settings that will be applied to the AM.Reporting.Net
    /// environment, including Report, Designer and Preview components.
    /// </summary>
    public static partial class Config
    {
#if COMMUNITY
        const string CONFIG_NAME = "AM.Reporting.Community.config";
#elif MONO
        const string CONFIG_NAME = "AM.Reporting.Mono.config";
#else
        const string CONFIG_NAME = "AM.Reporting.config";
#endif

        #region Private Fields

        private static readonly XmlDocument FDoc = new XmlDocument();

        private static string FLogs = "";
        private static string systemTempFolder;
        private static bool enableScriptSecurity;
        private static bool userSetsScriptSecurity;
        internal static bool CleanupOnExit;
        private static string applicationFolder;
        private static readonly string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets a value indicating that the Mono runtime is used.
        /// </summary>
        public static bool IsRunningOnMono { get; private set; }

#if CROSSPLATFORM
        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif


        /// <summary>
        /// Gets or sets a value indicating is it impossible to specify a local data path in Xml and Csv.
        /// </summary>
        public static bool ForbidLocalData { get; set; }


        /// <summary>
        /// Gets or sets the optimization of strings. Is experimental feature.
        /// </summary>
        public static bool IsStringOptimization { get; set; } = true;

        /// <summary>
        /// Enable or disable the compression in files with prepared reports (fpx).
        /// </summary>
        public static bool PreparedCompressed { get; set; } = true;

        /// <summary>
        /// Gets or sets the application folder.
        /// </summary>
        public static string ApplicationFolder
        {
            get
            {
                if (applicationFolder == null)
                {
                    return baseDirectory;
                }

                return applicationFolder;
            }
            set => applicationFolder = value;
        }

        /// <summary>
        /// Gets an english culture information for localization purposes
        /// </summary>
        public static CultureInfo EngCultureInfo { get; } = new CultureInfo ("en-US");

        /// <summary>
        /// Gets or sets the path used to load/save the configuration file.
        /// </summary>
        /// <remarks>
        /// By default, the configuration file is saved to the application local data folder
        /// (C:\Documents and Settings\User_Name\Local Settings\Application Data\AM.Reporting\).
        /// Set this property to "" if you want to store the configuration file in the application folder.
        /// </remarks>
        public static string Folder { get; set; }

        /// <summary>
        /// Gets or sets the path used to font.list file.
        /// </summary>
        /// <remarks>
        /// By default, the font.list file is saved to the AM.Reporting.config folder
        /// If WebMode enabled (or config file path is null), then file is saved in the application folder.
        /// </remarks>
        public static string FontListFolder { get; set; }

        /// <summary>
        /// Gets or sets the settings for the Report component.
        /// </summary>
        public static ReportSettings ReportSettings { get; set; } = new ReportSettings();

        /// <summary>
        /// Gets or sets a value indicating whether RTL layout should be used.
        /// </summary>
        public static bool RightToLeft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether hotkeys should be disabled.
        /// </summary>
        public static bool DisableHotkeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating saving last formatting should be disabled.
        /// </summary>
        public static bool DisableLastFormatting { get; set; }

        /// <summary>
        /// Gets the root item of config xml.
        /// </summary>
        public static XmlItem Root => FDoc.Root;

        /// <summary>
        /// Gets or sets the path to the temporary folder used to store temporary files.
        /// </summary>
        /// <remarks>
        /// The default value is <b>null</b>, so the system temp folder will be used.
        /// </remarks>
        public static string TempFolder { get; set; }

        /// <summary>
        /// Gets the path to the system temporary folder used to store temporary files.
        /// </summary>
        public static string SystemTempFolder => systemTempFolder == null ? GetTempPath() : systemTempFolder;

        /// <summary>
        /// Gets AM.Reporting version.
        /// </summary>
        public static string Version { get; } = typeof (Report).Assembly.GetName().Version.ToString (3);

        /// <summary>
        /// Called on script compile
        /// </summary>
        public static event EventHandler<ScriptSecurityEventArgs> ScriptCompile;

        /// <summary>
        /// Gets a PrivateFontCollection instance.
        /// </summary>
        public static FRPrivateFontCollection PrivateFontCollection { get; } = new FRPrivateFontCollection();

        /// <summary>
        /// Enable report script validation. For WebMode only
        /// </summary>
        public static bool EnableScriptSecurity
        {
            get => enableScriptSecurity;
            set
            {
                if (OnEnableScriptSecurityChanged != null)
                {
                    OnEnableScriptSecurityChanged.Invoke (null, null);
                }

                enableScriptSecurity = value;

                //
                userSetsScriptSecurity = true;
                if (value)
                {
                    if (ScriptSecurityProps == null)
                    {
                        ScriptSecurityProps = new ScriptSecurityProperties();
                    }
                }
            }
        }

        /// <summary>
        /// Throws when property EnableScriptSecurity has been changed
        /// </summary>
        public static event EventHandler OnEnableScriptSecurityChanged;

        /// <summary>
        /// Properties of report script validation
        /// </summary>
        public static ScriptSecurityProperties ScriptSecurityProps { get; private set; }

        /// <summary>
        /// Settings of report compiler.
        /// </summary>
        public static CompilerSettings CompilerSettings { get; set; } = new CompilerSettings();

        #endregion Public Properties

        #region Internal Methods

        internal static string CreateTempFile (string dir)
        {
            if (string.IsNullOrEmpty (dir))
            {
                return GetTempFileName();
            }

            return Path.Combine (dir, Path.GetRandomFileName());
        }

        internal static string GetTempFolder()
        {
            return TempFolder == null ? GetTempPath() : TempFolder;
        }

        internal static void Init()
        {
            IsRunningOnMono = Type.GetType ("Mono.Runtime") != null;
#if SKIA
            Topten.RichTextKit.FontFallback.CharacterMatcher = characterMatcher;
#endif

            CheckWebMode();

#if !CROSSPLATFORM
            if (!WebMode)
            {
                LoadConfig();
            }
#endif

            if (!userSetsScriptSecurity && WebMode)
            {
                enableScriptSecurity = true; // don't throw event
                ScriptSecurityProps = new ScriptSecurityProperties();
            }

#if !COMMUNITY
            RestoreExportOptions();
#endif
            LoadPlugins();
            InitTextRenderingHint();
        }

        private static void InitTextRenderingHint()
        {
            // init TextRenderingHint.SystemDefault
            // bug in .Net: if you use any other hint before SystemDefault, the SystemDefault will
            // look like SingleBitPerPixel
            using (var bmp = new Bitmap (1, 1))
            using (var g = Graphics.FromImage (bmp))
            {
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
                g.DrawString (" ", SystemFonts.DefaultFont, Brushes.Black, 0, 0);
            }
        }

        private static void CheckWebMode()
        {
            // If we/user sets 'WebMode = true' before this check - Config shouln't change it (because check may be incorrect)
            if (!WebMode)
            {
#if NETSTANDARD || NETCOREAPP
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var loadedAsmbly in loadedAssemblies)
                {
                    var isAspNetCore = loadedAsmbly.GetName().Name.StartsWith ("Microsoft.AspNetCore");
                    if (isAspNetCore)
                    {
                        WebMode = true;
                        break;
                    }
                }
#else
                string processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                WebMode = String.Compare(processName, "iisexpress") == 0 ||
                              String.Compare(processName, "w3wp") == 0;
#endif
            }
        }

        internal static void WriteLogString (string s)
        {
            WriteLogString (s, false);
        }

        internal static void WriteLogString (string s, bool distinct)
        {
            if (distinct)
            {
                if (FLogs.IndexOf (s + "\r\n") != -1)
                {
                    return;
                }
            }

            FLogs += s + "\r\n";
        }


        internal static void OnScriptCompile (ScriptSecurityEventArgs e)
        {
            if (ScriptCompile != null)
            {
                ScriptCompile.Invoke (null, e);
            }

            if (!e.IsValid)
            {
                throw new CompilerException (Res.Get ("Messages,CompilerError"));
            }
        }

#if NETSTANDARD || NETCOREAPP

        // public static event EventHandler<Code.CodeDom.Compiler.CompilationEventArgs> BeforeEmitCompile;

        // internal static void OnBeforeScriptCompilation(object sender, Code.CodeDom.Compiler.CompilationEventArgs e)
        // {
        //     if (BeforeEmitCompile != null)
        //     {
        //         BeforeEmitCompile.Invoke(sender, e);
        //     }
        // }
#endif

        #endregion Internal Methods

        #region Private Methods

        private static string GetTempFileName()
        {
            return Path.Combine (GetTempFolder(),
                SystemFake.DateTime.Now.ToString ("yyyy-dd-M--HH-mm-ss-") + Path.GetRandomFileName());
        }

        private static string GetTempPath()
        {
            if (!string.IsNullOrEmpty (systemTempFolder))
            {
                return systemTempFolder;
            }

            systemTempFolder = Environment.GetEnvironmentVariable ("TMP");
            if (string.IsNullOrEmpty (systemTempFolder))
            {
                systemTempFolder = Environment.GetEnvironmentVariable ("TEMP");
            }

            if (string.IsNullOrEmpty (systemTempFolder))
            {
                systemTempFolder = Environment.GetEnvironmentVariable ("TMPDIR");
            }

            if (string.IsNullOrEmpty (systemTempFolder))
            {
                systemTempFolder = Path.GetTempPath();
            }

            return systemTempFolder;
        }

        private static void CurrentDomain_ProcessExit (object sender, EventArgs e)
        {
            FDoc.Root.Name = "Config";
            FDoc.AutoIndent = true;
            SaveUIStyle();
            SaveUIOptions();
            SavePreviewSettings();
            SaveCompilerSettings();
            SaveAuthServiceUser();
#if !COMMUNITY
            SaveExportOptions();
#endif

            if (!WebMode)
            {
                try
                {
                    if (!Directory.Exists (Folder))
                    {
                        Directory.CreateDirectory (Folder);
                    }

                    var configFile = Path.Combine (Folder, CONFIG_NAME);
                    if (CleanupOnExit)
                    {
                        File.Delete (configFile);
                    }
                    else
                    {
                        FDoc.Save (configFile);
                    }

                    if (FLogs != "")
                    {
                        File.WriteAllText (Path.Combine (Folder, "AM.Reporting.logs"), FLogs);
                    }
                }
                catch
                {
                }
            }
        }

        private static void LoadConfig()
        {
            var configLoaded = false;
            if (!WebMode)
            {
                try
                {
                    if (Folder == null)
                    {
                        var baseFolder = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData);
                        Folder = Path.Combine (baseFolder, "AM.Reporting");
                    }
                    else if (Folder == "")
                    {
                        Folder = ApplicationFolder;
                    }
                }
                catch
                {
                }

                var fileName = Path.Combine (Folder, CONFIG_NAME);

                if (File.Exists (fileName))
                {
                    try
                    {
                        FDoc.Load (fileName);
                        configLoaded = true;
                    }
                    catch
                    {
                    }
                }

                RestoreUIStyle();
                RestoreDefaultLanguage();
                RestoreUIOptions();
                RestorePreviewSettings();
                RestoreAuthServiceUser();
                RestoreCompilerSettings();
                Res.LoadDefaultLocale();
                AppDomain.CurrentDomain.ProcessExit += new EventHandler (CurrentDomain_ProcessExit);
            }

            if (!configLoaded)
            {
                // load default config
                using (var stream = ResourceLoader.GetStream ("AM.Reporting.config"))
                {
                    FDoc.Load (stream);
                }
            }
        }

        private static void LoadPlugins()
        {
            // main assembly initializer
            ProcessMainAssembly();

            var pluginsItem = Root.FindItem ("Plugins");
            for (var i = 0; i < pluginsItem.Count; i++)
            {
                var item = pluginsItem[i];
                var pluginName = item.GetProp ("Name");

                try
                {
                    var assembly = Assembly.LoadFrom (pluginName);
                    ProcessAssembly (assembly);
                }
                catch
                {
                }
            }

            // For CoreWin
#if COREWIN
            LoadPluginsInCurrentFolder();
#endif
        }


        private static void ProcessAssembly (Assembly a)
        {
            foreach (var t in a.GetTypes())
            {
                if (t.IsSubclassOf (typeof (AssemblyInitializerBase)))
                {
                    Activator.CreateInstance (t);
                }
            }
        }

        private static void RestoreDefaultLanguage()
        {
            var xi = Root.FindItem ("Designer").FindItem ("Code");
            var defaultLanguage = xi.GetProp ("DefaultScriptLanguage");
            ReportSettings.DefaultLanguage = defaultLanguage == Language.Vb.ToString() ? Language.Vb : Language.CSharp;
        }

        private static void RestoreRightToLeft()
        {
            var xi = Root.FindItem ("UIOptions");
            var rtl = xi.GetProp ("RightToLeft");

            if (!string.IsNullOrEmpty (rtl))
            {
                switch (rtl)
                {
                    case "Auto":
                        RightToLeft = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
                        break;

                    case "No":
                        RightToLeft = false;
                        break;

                    case "Yes":
                        RightToLeft = true;
                        break;

                    default:
                        RightToLeft = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Properties of ScriptSecurity
        /// </summary>
        public class ScriptSecurityProperties
        {
            private static readonly string[] defaultStopList = new[]
            {
                "GetType",
                "typeof",
                "TypeOf", // VB
                "DllImport",
                "LoadLibrary",
                "GetProcAddress",
            };

            private string[] stopList;

            /// <summary>
            /// Add stubs for the most dangerous classes (in System.IO, System.Reflection etc)
            /// </summary>
            public bool AddStubClasses { get; set; } = true;

            /// <summary>
            /// List of keywords that shouldn't be declared in the report script
            /// </summary>
            public string[] StopList
            {
                get => (string[])stopList.Clone();
                set
                {
                    if (value != null)
                    {
                        OnStopListChanged?.Invoke (this, null);
                        stopList = value;
                    }
                }
            }

            /// <summary>
            /// Throws when <see cref="StopList"/> has changed
            /// </summary>
            public event EventHandler OnStopListChanged;

            internal ScriptSecurityProperties()
            {
                SetDefaultStopList();
            }

            internal ScriptSecurityProperties (string[] stopList)
            {
                this.stopList = stopList;
            }

            /// <summary>
            /// Sets default value for <see cref="StopList"/>
            /// </summary>
            public void SetDefaultStopList()
            {
                StopList = defaultStopList;
            }
        }

        private static void SaveUIOptions()
        {
            var xi = Root.FindItem ("UIOptions");
            xi.SetProp ("DisableHotkeys", Converter.ToString (DisableHotkeys));
            xi.SetProp ("DisableLastFormatting", Converter.ToString (DisableLastFormatting));
        }

        private static void RestoreUIOptions()
        {
            RestoreRightToLeft();

            var xi = Root.FindItem ("UIOptions");

            var disableHotkeysStringValue = xi.GetProp ("DisableHotkeys");
            if (!string.IsNullOrEmpty (disableHotkeysStringValue))
            {
                DisableHotkeys = disableHotkeysStringValue.ToLower() != "false";
            }

            var disableLastFormattingStringValue = xi.GetProp ("DisableLastFormatting");
            if (!string.IsNullOrEmpty (disableLastFormattingStringValue))
            {
                DisableLastFormatting = disableLastFormattingStringValue.ToLower() != "false";
            }
        }

        private static void SaveCompilerSettings()
        {
            var xi = Root.FindItem ("CompilerSettings");
            xi.SetProp ("Placeholder", CompilerSettings.Placeholder);
            xi.SetProp ("ExceptionBehaviour", Converter.ToString (CompilerSettings.ExceptionBehaviour));
        }

        private static void RestoreCompilerSettings()
        {
            var xi = Root.FindItem ("CompilerSettings");
            CompilerSettings.Placeholder = xi.GetProp ("Placeholder");

            var exceptionBehaviour = xi.GetProp ("ExceptionBehaviour");
            if (!string.IsNullOrEmpty (exceptionBehaviour))
            {
                try
                {
                    CompilerSettings.ExceptionBehaviour =
                        (CompilerExceptionBehaviour)Converter.FromString (typeof (CompilerExceptionBehaviour),
                            exceptionBehaviour);
                }
                catch
                {
                    CompilerSettings.ExceptionBehaviour = CompilerExceptionBehaviour.Default;
                }
            }
        }

        #endregion Private Methods
    }
}
