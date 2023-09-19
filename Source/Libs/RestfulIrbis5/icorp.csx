#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\Microsoft.Extensions.ObjectPool.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\RestSharp.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\RestSharp.Serializers.NewtonsoftJson.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\HtmlAgilityPack.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\AM.Core5.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\ManagedIrbis5.dll"
#r "D:\Projects\ManagedIrbis5\Source\Libs\RestfulIrbis5\bin\Release\net8.0\publish\RestfulIrbis5.dll"

using System.IO;
using RestfulIrbis.IrbisCorp;

var client = new IrbisCorpClient (IrbisCorpClient.DefaultUrl, "12345678");
var query = new IrbisCorpQuery();
query.Title = "бетоны";

var response = client.SearchBroadcast (query);
var requestId = client.ExtractRequestId (response);

Console.WriteLine ($"Request ID={requestId}");

var underscore = "112345678901";
var result = client.GetResult (underscore, requestId);
File.WriteAllText ("result.txt", result);

var records = client.FindRecords (result);
foreach (var record in records)
{
    Console.WriteLine ($"Id={record.Id} has {record.Fields.Length} fields");
    foreach (var field in record.Fields)
    {
        Console.WriteLine ($"\t{field}");
    }
    Console.WriteLine();

    var download = client.DownloadRecord (record.Id);
    if (!string.IsNullOrEmpty (download))
    {
        File.WriteAllText ($"record{record.Id}.txt", download);
    }
}

Console.WriteLine ("ALL DONE");

Environment.Exit (0);
