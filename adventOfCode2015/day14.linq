<Query Kind="Program" />

void Main()
{
	var lines = GetInput();
	// eg: Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.
	Regex re = new Regex(@"(?<reindeer>\w+).*\s(?<speed>\d+).*\s(?<time>\d+).*\s(?<rest>\d+) seconds\.");

	var Reindeers = new List<ReindeerInfo>();
	foreach (var line in lines)
	{
		var match = re.Match(line);
		var info = new ReindeerInfo
		{
			Name = match.Groups["reindeer"].Value,
			Speed = int.Parse(match.Groups["speed"].Value),
			FlightTime = int.Parse(match.Groups["time"].Value),
			RestTime = int.Parse(match.Groups["rest"].Value),
		};
		Reindeers.Add(info);
	}
	Reindeers.Dump();

	int raceTime = 2503;
	var results = new List<Tuple<string, int>>();
	foreach (var info in Reindeers)
	{
		int fandr = info.FlightTime + info.RestTime;
		int multiple = raceTime / fandr;
		int remain = raceTime % fandr;
		int dist = multiple * info.FlightTime * info.Speed;
		dist += (Math.Min(remain, info.FlightTime) * info.Speed);
		results.Add(new Tuple<string, int>(info.Name, dist));
	}
	results.Dump();
	results.OrderByDescending(r => r.Item2).First().Dump("Furthest");
	
}

public class ReindeerInfo
{
	public string Name { get; set; }
	public int Speed { get; set; }
	public int FlightTime { get; set; }
	public int RestTime { get; set; }
}

public IEnumerable<string> GetInput()
{
	string fname = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day14-input.txt");
	var lines = File.ReadAllLines(fname);
	return lines.ToList();
}