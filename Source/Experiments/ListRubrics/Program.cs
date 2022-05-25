// ReSharper disable StringLiteralTypo

using System.Xml.Linq;
using System.Xml.XPath;

var doc = XDocument.Load ("rubricator.xml");
var rubrics = doc.Root!.XPathSelectElements ("//rubric");

foreach (var rubric in rubrics)
{
    var title = rubric.Attribute ("title")?.Value;
    var bbk = rubric.Attribute ("bbk")?.Value;
    var udc = rubric.Attribute ("udc")?.Value;
    Console.WriteLine ($"{bbk,-8}\t{udc,-8}\t{title}");
}
