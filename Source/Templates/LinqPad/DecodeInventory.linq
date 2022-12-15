<Query Kind="Program">
  <Reference>C:\Projects\ManagedIrbis5\Source\Libs\ManagedIrbis5\bin\Debug\net7.0\AM.Core5.dll</Reference>
  <Reference>C:\Projects\ManagedIrbis5\Source\Libs\ManagedIrbis5\bin\Debug\net7.0\ManagedIrbis5.dll</Reference>
  <Namespace>ManagedIrbis</Namespace>
  <RuntimeVersion>7.0</RuntimeVersion>
</Query>

void Main()
{
    var numbers = new[] { 22714, 2646, 22184, 23143, 4962, 4616, 23235, 28388, 4637,
        21624, 26618, 23249, 28375, 21268, 1699, 26483, 8851, 22391, 17698, 6813 };

    var connection = new SyncConnection
    {
        Host = "127.0.0.1",
        Username = "librarian",
        Password = "secret",
        Database = "ISTU"
    };
	
    connection.Connect();

    var counter = 0;
    var list = new List<RatingInfo>();
    foreach (var number in numbers)
    {
        var description = GetDescription (connection, number);
        //Console.WriteLine ($"ER-{number}\t{description}");
        list.Add(new RatingInfo(++counter, $"ER-{number}", description));
    }
	
    connection.Disconnect();
	
    list.Dump();
}

// You can define other methods, fields, classes and namespaces here
string GetDescription
    (
        SyncConnection connection,
        int integralPart
    )
{
    var parameters = new SearchParameters
    {
        Expression = $"IN=ER-{integralPart}",
        Format = "@brief"
    };

    var found = connection.Search (parameters);
    if (found is null)
    {
        Console.WriteLine (connection.LastError);
        return "FATAL";
    }

    if (found.Length != 1)
    {
        return "ERROR";
    }
	
    return found[0].Text;
}

record RatingInfo (int Place, string Inventory, string Description) {}
