// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.AppServices;

using Microsoft.Extensions.Logging;

#endregion

namespace ApplicationExample
{
    /// <summary>
    /// Наше приложение.
    /// </summary>
    internal class Program
        : MagnaApplication
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="args"></param>
        public Program(string[] args)
            : base(args)
        {
        }

        private static void CustomRun
            (
                MagnaApplication application
            )
        {
            application.Logger.LogInformation("Привет из приложения");
        }

        static int Main(string[] args)
        {
            return new Program(args).Run(CustomRun);
        }

    } // class Program

} // namespace ApplicationExample
