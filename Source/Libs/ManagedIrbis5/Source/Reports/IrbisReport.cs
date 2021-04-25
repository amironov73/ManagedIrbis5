// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisReport.cs -- отчет, строимый на основе запросов к ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;


#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Отчет, строимый на основе запросов к базам данных ИРБИС64.
    /// </summary>
    public class IrbisReport
        : IAttributable,
        IVerifiable,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Attributes.
        /// </summary>
        [XmlArray("attr")]
        [JsonPropertyName("attr")]
        public ReportAttributes Attributes { get; private set; }

        /// <summary>
        /// Report body band.
        /// </summary>
        [XmlElement("details")]
        [JsonPropertyName("details")]
        public BandCollection<ReportBand> Body { get; set; }

        /// <summary>
        /// Полоса подвала отчета.
        /// </summary>
        [XmlElement("footer")]
        [JsonPropertyName("footer")]
        public ReportBand? Footer
        {
            get => _footer;
            set
            {
                if (_footer is not null)
                {
                    _footer.Report = null;
                    _footer.Parent = null;
                }

                _footer = value;
                if (_footer is not null)
                {
                    _footer.Report = this;
                    _footer.Parent = null;
                }
            }
        }

        /// <summary>
        /// Полоса заголовка отчета.
        /// </summary>
        [XmlElement("header")]
        [JsonPropertyName("header")]
        public ReportBand? Header
        {
            get => _header;
            set
            {
                if (_header is not null)
                {
                    _header.Report = null;
                    _header.Parent = null;
                }

                _header = value;
                if (_header is not null)
                {
                    _header.Report = this;
                    _header.Parent = null;
                }
            }
        }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisReport()
        {
            Magna.Trace("IrbisReport::Constructor");

            Attributes = new ReportAttributes();
            Body = new BandCollection<ReportBand>(this, null);
        }

        #endregion

        #region Private members

        private ReportBand? _footer;
        private ReportBand? _header;

        #endregion

        #region Public methods

        /// <summary>
        /// Render the report.
        /// </summary>
        public virtual void Render
            (
                ReportContext context
            )
        {
            Magna.Trace("IrbisReport::Render");

            context.Output.Clear();

            ReportDriver driver = context.Driver;

            context.CurrentRecord = null;
            context.Index = -1;

            driver.BeginDocument(context, this);

            if (!ReferenceEquals(Header, null))
            {
                Header.Render(context);
            }

            Body.Render(context);

            if (!ReferenceEquals(Footer, null))
            {
                Footer.Render(context);
            }

            driver.EndDocument(context, this);
        }

        /// <summary>
        /// Load report from the JSON file.
        /// </summary>
        public static IrbisReport LoadJsonFile
            (
                 string fileName
            )
        {
            /*

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,

                // TODO fix it
                // TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple

                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };

            IrbisReport result
                = JsonConvert.DeserializeObject<IrbisReport>
                (
                    contents,
                    settings
                );

            return result;

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Load report from the JSON file.
        /// </summary>
        public static IrbisReport LoadShortJson
            (
                string fileName
            )
        {
            /*

            string contents = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            JObject obj = JObject.Parse(contents);

            var tokens = obj.SelectTokens("$..$type");
            foreach (JToken token in tokens)
            {
                JValue val = (JValue)token;

                string typeName = val.Value.ToString();
                if (!typeName.Contains('.'))
                {
                    typeName = "ManagedIrbis.Reports."
                               + typeName
                               + ", ManagedIrbis";
                    val.Value = typeName;
                }
            }

            JsonSerializer serializer = new JsonSerializer
            {
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };

            IrbisReport result = obj.ToObject<IrbisReport>
                (
                    serializer
                );

            return result;

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the report to specified file.
        /// </summary>
        public void SaveJson
            (
                string fileName
            )
        {
            /*

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
            string contents = JsonConvert.SerializeObject
                (
                    this,
                    Formatting.Indented,
                    settings
                );
            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Save the report to specified file.
        /// </summary>
        public void SaveShortJson
            (
                string fileName
            )
        {
            /*

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
            JObject obj = JObject.FromObject
                (
                    this,
                    serializer
                );

            var tokens = obj.SelectTokens("$..$type");
            foreach (JToken token in tokens)
            {
                JValue val = (JValue)token;

                Type type = Type.GetType
                    (
                        val.Value.ToString(),
                        false
                    );
                if (!ReferenceEquals(type, null))
                {
                    val.Value = type.Name;
                }
            }

            while (true)
            {
                tokens = obj.SelectTokens("$..attr");
                var attr = tokens.FirstOrDefault
                    (
                        a => a.Count() == 1
                    );
                if (ReferenceEquals(attr, null))
                {
                    break;
                }
                attr.Parent.Remove();
            }

            while (true)
            {
                tokens = obj.SelectTokens("$..cells");
                var attr = tokens.FirstOrDefault
                    (
                        a => a.Count() == 0
                    );
                if (ReferenceEquals(attr, null))
                {
                    break;
                }
                attr.Parent.Remove();
            }

            string contents = obj.ToString(Formatting.Indented);
            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );

            */

            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<IrbisReport> verifier
                = new Verifier<IrbisReport>(this, throwOnError);

            verifier.VerifySubObject(Attributes, "attributes");

            if (!ReferenceEquals(Header, null))
            {
                verifier
                    .VerifySubObject(Header, "header")
                    .ReferenceEquals
                        (
                            Header.Parent,
                            null,
                            "Header.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            Header.Report,
                            this,
                            "Header.Report != this"
                        );
            }

            if (!ReferenceEquals(Footer, null))
            {
                verifier
                    .VerifySubObject(Footer, "footer")
                    .ReferenceEquals
                        (
                            Footer.Parent,
                            null,
                            "Footer.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            Footer.Report,
                            this,
                            "Footer.Report != this"
                        );
            }

            verifier.VerifySubObject(Body, "body");

            foreach (ReportBand band in Body)
            {
                verifier
                    .ReferenceEquals
                        (
                            band.Parent,
                            null,
                            "band.Parent != null"
                        )
                    .ReferenceEquals
                        (
                            band.Report,
                            this,
                            "band.Report != this"
                        );
            }

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Magna.Trace("IrbisReport::Dispose");

            if (!ReferenceEquals(Header, null))
            {
                Header.Dispose();
            }
            if (!ReferenceEquals(Footer, null))
            {
                Footer.Dispose();
            }
            Body.Dispose();
        }

        #endregion

    } // class IrbisReport

} // namespace ManagedIrbis.Reports
