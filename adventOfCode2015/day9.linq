<Query Kind="Program" />

void Main()
{
	// Routine to solve day 9 of the AdventOfCode 2015
	// http://adventofcode.com/day/9

	var raw = GetInput();
	var routes = ParseInput(raw);
	//routes.Dump();

	var locations = routes.Select(r => r.From)
		.Concat(routes.Select(r => r.To))
		.Distinct()
		.ToList();

	List<FullRoute> routesTravelled = new List<FullRoute>();
	foreach (var location in locations)
	{
		var fr = new FullRoute(location);
		routesTravelled.AddRange(BuildRoutes(routes, locations, fr));
	}

	routesTravelled.Count().Dump("Total routes");
	var shortest = routesTravelled.OrderBy(r => r.Distance).First();
	shortest.Dump();

	var longest = routesTravelled.OrderByDescending(r => r.Distance).First();
	longest.Dump();
}

public IEnumerable<FullRoute> BuildRoutes(IEnumerable<RouteDistance> routes, IEnumerable<string> locations, FullRoute currentRoute)
{
	List<FullRoute> result = new List<FullRoute>();
	string thisLocation = currentRoute.CurrentLocation;
	string thisRoute = currentRoute.Route;
	// The current route needs to go next to each one of the places it hasn't yet visited
	foreach (string location in locations.Where(l => !currentRoute.Visited(l)))
	{
		int dist = GetDistance(routes, thisLocation, location);
		var newRoute = currentRoute.Clone();
		newRoute.AddLocation(dist, location);
		if (newRoute.NumberOfLocationsVisited == locations.Count())
		{
			// Done, add this to the result
			result.Add(newRoute);
		}
		else
		{
			// More places to visit. Go recursive...
			result.AddRange(BuildRoutes(routes, locations, newRoute));
		}
	}
	
	return result;
}

public int GetDistance(IEnumerable<RouteDistance> routes, string A, string B)
{
	return routes.Where(r => ((r.From == A && r.To == B) || (r.From == B && r.To == A)))
		.Select(r => r.Distance)
		.First();
}

public class FullRoute
{
	List<string> _locations = new List<string>();
	
	public int Distance { get; set; }
	public string Route
	{
		get { return string.Join(" -> ", _locations); }
		set 
		{ 
			_locations.Clear();
			_locations.AddRange(value.Replace(" -> ", "_").Split('_'));
		} 
	}

	protected FullRoute()
	{
	}
	
	public FullRoute(string startingLocation)
		: this()
	{
		_locations.Add(startingLocation);
		Distance = 0;
	}

	public string StartingLocation
	{
		get { return _locations.FirstOrDefault(); }
	}
	
	public string CurrentLocation
	{
		get
		{
			return _locations.Last();
		}
	}

	public int NumberOfLocationsVisited
	{
		get
		{
			return _locations.Count;
		}
	}

	public bool Visited(string location)
	{
		return _locations.Contains(location);
	}
	
	public void AddLocation(int distance, string location)
	{
		_locations.Add(location);
		Distance += distance;
	}

	public FullRoute Clone()
	{
		FullRoute result = new FullRoute
		{
			Distance = this.Distance,
		};
		result._locations.AddRange(this._locations);
		return result;
	}
}

public class RouteDistance
{
	public string From { get; set; }
	public string To { get; set; }
	public int Distance { get; set; }
}

public IEnumerable<RouteDistance> ParseInput(IEnumerable<string> raw)
{
	var result = new List<RouteDistance>();
	Regex re = new Regex(@"(?<from>\w+)\sto\s(?<to>\w+)\s=\s(?<dist>\d+)");
	foreach (string line in raw)
	{
		var match = re.Match(line);
		var item = new RouteDistance
		{
			From = match.Groups["from"].Value,
			To = match.Groups["to"].Value,
			Distance = int.Parse(match.Groups["dist"].Value),
		};
		result.Add(item);
	}
	return result;
}

public IEnumerable<string> GetInput()
{
	string fname = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day9-input.txt");
	var lines = File.ReadAllLines(fname);
	return lines.ToList();
}
