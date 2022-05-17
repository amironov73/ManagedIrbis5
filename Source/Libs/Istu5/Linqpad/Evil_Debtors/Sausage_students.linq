<Query Kind="Statements">
  <Connection>
    <ID>aa2659b6-5455-4512-928c-5e378735afe5</ID>
    <Persist>true</Persist>
    <Server>libertine</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>kladovka</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

// Выявление студентов-злостных задолжников

var deadline = DateTime.Now.Date.AddMonths (-1);

var onPodsob = 
(
	from p in Podsobs
	join r in Readers on p.CHB equals r.Ticket
	where ( r.Category == "студент" ) 
		&& ( r.Group != null )
		&& ( p.Srok < deadline )
	select new { r.Ticket, r.Name, r.Group, r.Department, Type="n", Count = 1 }
).ToList();

// onPodsob.Dump ( "На научном абонементе" );

var onUch =
(
	from u in Uchtrans
	join r in Readers on u.Chb equals r.Ticket
	where ( r.Category == "студент" )
		&& ( r.Group != null )
		&& ( u.Srok < deadline )
	select new { r.Ticket, r.Name, r.Group, r.Department, Type="u", Count = 1 }
).ToList ();

// onUch.Dump ( "На учебном абонементе" );

var onPerio =
(
	from m in Magtrans
	join r in Readers on m.Chb equals r.Ticket
	where ( r.Category == "студент" )
		&& ( r.Group != null )
		&& ( m.Srok < deadline )
	select new { r.Ticket, r.Name, r.Group, r.Department, Type="p", Count = 1 }
).ToList ();

// onPerio.Dump ( "По периодике" );

var onHudo =
(
	from h in Hudtrans
	join r in Readers on h.Chb equals r.Ticket
	where ( r.Category == "студент" )
		&& ( r.Group != null )
		&& ( h.Srok < deadline )
	select new { r.Ticket, r.Name, r.Group, r.Department, Type="h", Count = 1 }
).ToList ();

// onHudo.Dump ( "По художественной литературе" );

var allRaw = onPodsob
	.Concat ( onUch )
	.Concat ( onPerio )
	.Concat ( onHudo );

// allRaw.Dump ( "Все всплошняк" );

var allCounted = 
(
	from a in allRaw
	group a by a.Ticket into g
	let cn = g.Where ( _=>_.Type == "n" ).Sum ( _=>_.Count )
	let cu = g.Where ( _=>_.Type == "u" ).Sum ( _=>_.Count )
	let cp = g.Where ( _=>_.Type == "p" ).Sum ( _=>_.Count )
	let ch = g.Where ( _=>_.Type == "h" ).Sum ( _=>_.Count )
	let f = g.First ()
	orderby f.Name
	select new { f.Name, f.Group, f.Department, CountN=cn, 
		CountU=cu, CountP=cp, CountH=ch, Count = g.Count () }
).ToList ();

// allCounted.Dump ();

var even = true;

foreach ( var r in allCounted )
{
	Console.WriteLine ( "<tr bgcolor='{8}'><td>{0}</td>"
		+ "<td class='centered'>{1}</td>"
		+ "<td>{2}</td>"
		+ "<td class='centered'>{3}</td>"
		+ "<td class='centered'>{4}</td>"
		+ "<td class='centered'>{5}</td>"
		+ "<td class='centered'>{6}</td>"
		+ "<td class='centered'>{7}</td>"
		+ "</tr>",
		r.Name, r.Group, r.Department, r.CountN, r.CountU, r.CountP, 
		r.CountH, r.Count,
		( even ? "#FFFFFF" : "#CCCCCC" ) );
	even = !even;
}
	
int totalN = allRaw.Where ( _ => _.Type == "n" ).Sum ( _ => _.Count );
int totalU = allRaw.Where ( _ => _.Type == "u" ).Sum ( _ => _.Count );
int totalP = allRaw.Where ( _ => _.Type == "p" ).Sum ( _ => _.Count );
int totalH = allRaw.Where ( _ => _.Type == "h" ).Sum ( _ => _.Count );
int total = allRaw.Sum ( _ => _.Count );

Console.WriteLine ( "<tr><td colspan='3' class='rightAligned'><b>Итого</b></td>"
	+"<td class='centered'><b>{0}</b></td>"
	+"<td class='centered'><b>{1}</b></td>"
	+"<td class='centered'><b>{2}</b></td>"
	+"<td class='centered'><b>{3}</b></td>"
	+"<td class='centered'><b>{4}</b></td>"
	+ "</tr>", totalN, totalU, totalP, totalH, total );