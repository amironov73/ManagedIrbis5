<Query Kind="Program">
  <Connection>
    <ID>60bca5ab-7575-4d06-977e-6a76772d10c2</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>libertine</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>kladovka</Database>
    <SqlSecurity>true</SqlSecurity>
  </Connection>
</Query>

void Main()
{
	//
	// Злостные задолжники НТБ ИРНИТУ 
	// в раскладке по фондам и категориям
	//

	// дата начала учебного года
	var deadline = new DateTime(2021, 9, 1);

	// научный абонемент
	var onPodsob =
	(
		from p in Podsobs
		where p.Srok < deadline && p.CHB != null
		select p.CHB
	)
	.ToList();

	// учебный абонемент
	var onUch =
	(
		from u in Uchtrans
		where u.Srok < deadline && u.Chb != null
		select u.Chb
	)
	.ToList();

	// периодика (как правило, никто не должен)
	var onPerio =
	(
		from m in Magtrans
		where m.Srok < deadline && m.Chb != null
		select m.Chb
	)
	.ToList();

	// художественная литература
	var onHudo =
	(
		from h in Hudtrans
		where h.Srok < deadline && h.Chb != null
		select h.Chb
	)
	.ToList();

	// все читатели
	var readers =
	(
		from r in Readers
		where r.Podpisal.Value == 0 
			&& r.Category != "служебная запись"
		select new Debtor
		{
			Name = r.Name,
			Ticket = r.Ticket,
			Category = r.Category,
			Department = r.Department,
			Laboratory = r.Laboratory,
			Group = r.Group,
			Workplace = r.Workplace,
			IsDebtor = r.Debtor.Value,
			IsBlocked = r.Blocked.Value != 0
		}
	)
	.ToList();

	var debtors = new List<Debtor>();
	var total = readers.Count;
	var counter = 0;
	foreach (var reader in readers)
	{
		if (++counter % 1000 == 0)
		{
			Util.Progress = counter * 100 / total;
		}
		
		reader.Podsob = onPodsob.Count (ticket => ticket == reader.Ticket);
		reader.Uch = onUch.Count (ticket => ticket == reader.Ticket);
		reader.Perio = onPerio.Count (ticket => ticket == reader.Ticket);
		reader.Hudo = onHudo.Count (ticket => ticket == reader.Ticket);
		if (reader.Podsob != 0
			|| reader.Uch != 0
			|| reader.Perio != 0
			|| reader.Hudo != 0)
		{
			debtors.Add (reader);
		}
	}

	 debtors
		.OrderBy (debtor => debtor.Name)
		.Dump ("Задолжники");

	//foreach (var debtor in debtors.OrderBy (d => d.Name))
	//{
	//	Console.WriteLine (debtor.ToString());
	//}
	
}

class Debtor
{
	public string Name { get; set; }
	public string Ticket { get; set; }
	public string Category { get; set; }
	public string Department { get; set; }
	public string Laboratory { get; set; }
	public string Group { get; set; }
	public string Workplace { get; set; }
	public bool IsDebtor { get; set; }
	public bool IsBlocked { get; set; }
	public int Podsob { get; set; }
	public int Uch { get; set; }
	public int Perio { get; set; }
	public int Hudo { get; set; }

	public override string ToString()
	{
		return $"{Name}\t{Ticket}\t{Category}\t{Department}\t{Laboratory}\t{Group}\t{Workplace}\t{IsDebtor}\t{IsBlocked}\t{Podsob}\t{Uch}\t{Perio}\t{Hudo}";
	}
}
