using System;
using System.Collections.Generic;
using System.Text;
using AM.Reporting.Utils;

namespace AM.Reporting
{
  /// <summary>
  /// Provides the serialize/deserialize functionality.
  /// </summary>
  public interface IFRSerializable
  {
    /// <summary>
    /// Serializes the object.
    /// </summary>
    /// <param name="writer">Writer object.</param>
    void Serialize(FRWriter writer);

    /// <summary>
    /// Deserializes the object.
    /// </summary>
    /// <param name="reader">Reader object.</param>
    void Deserialize(FRReader reader);
  }
}
