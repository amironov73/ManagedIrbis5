// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ReportSettings.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.Json;
using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>

    public sealed class ReportSettings
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Assemblies to load.
        /// </summary>
        [JsonPropertyName("assemblies")]
        public NonNullCollection<string> Assemblies { get; private set; }

        /// <summary>
        /// Name of the <see cref="ReportDriver"/>.
        /// </summary>
        [JsonPropertyName("driver")]
        public string? DriverName { get; set; }

        /// <summary>
        /// Settings for driver.
        /// </summary>
        [JsonPropertyName("driverSettings")]
        public string? DriverSettings { get; set; }

        /// <summary>
        /// Record filter.
        /// </summary>
        [JsonPropertyName("filter")]
        public string? Filter { get; set; }

        /// <summary>
        /// Output file name.
        /// </summary>
        [JsonPropertyName("outputFile")]
        public string? OutputFile { get; set; }

        /// <summary>
        /// Page settings.
        /// </summary>
        [JsonPropertyName("pageSettings")]
        public string? PageSettings { get; set; }

        /// <summary>
        /// Printer to send report to.
        /// </summary>
        [JsonPropertyName("printer")]
        public string? PrinterName { get; set; }

        /// <summary>
        /// Name of <see cref="ISyncIrbisProvider"/>.
        /// </summary>
        [JsonPropertyName("providerName")]
        public string? ProviderName { get; set; }

        /// <summary>
        /// Settings for provider.
        /// </summary>
        [JsonPropertyName("providerSettings")]
        public string? ProviderSettings { get; set; }

        /// <summary>
        /// Register <see cref="ReportDriver"/>
        /// before report building.
        /// </summary>
        [JsonPropertyName("registerDriver")]
        public string? RegisterDriver { get; set; }

        /// <summary>
        /// <see cref="ISyncIrbisProvider"/> to register
        /// before report building.
        /// </summary>
        [JsonPropertyName("registerProvider")]
        public string? RegisterProvider { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportSettings()
        {
            Assemblies = new NonNullCollection<string>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load <see cref="ReportSettings"/>
        /// from specified file.
        /// </summary>
        public static ReportSettings LoadFromFile
            (
                string fileName
            )
        {
            ReportSettings result = JsonUtility
                .ReadObjectFromFile<ReportSettings>(fileName);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReportSettings>(this, throwOnError);

            verifier
                .NotNull(Assemblies, "Assemblies")
                .NotNullNorEmpty(DriverName)
                .NotNullNorEmpty(ProviderName);

            return verifier.Result;
        }

        #endregion
    }
}
