// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Geolocation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Геолокация.
/// </summary>
public sealed class Geolocation
    : Payload
{
    /// <summary>
    /// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding)
    /// </summary>
    /// <param name="latitude">Latitude with . as splitter</param>
    /// <param name="longitude">Longitude with . as splitter</param>
    /// <param name="encoding">Encoding type - GEO or GoogleMaps</param>
    public Geolocation
        (
            string latitude,
            string longitude,
            GeolocationEncoding encoding = GeolocationEncoding.GEO
        )
    {
        _latitude = latitude.Replace (",", ".");
        _longitude = longitude.Replace (",", ".");
        _encoding = encoding;
    }

    #region Private members

    private readonly string _latitude, _longitude;
    private readonly GeolocationEncoding _encoding;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        switch (_encoding)
        {
            case GeolocationEncoding.GEO:
                return $"geo:{_latitude},{_longitude}";

            case GeolocationEncoding.GoogleMaps:
                return $"https://maps.google.com/maps?q={_latitude},{_longitude}";

            default:
                return "geo:";
        }
    }

    #endregion

    /// <summary>
    /// Схема кодирования геолокации.
    /// </summary>
    public enum GeolocationEncoding
    {
        /// <summary>
        /// GEO.
        /// </summary>
        GEO,

        /// <summary>
        /// Google maps.
        /// </summary>
        GoogleMaps
    }
}
