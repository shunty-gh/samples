<Query Kind="Program" />

void Main()
{
	// Routine to solve day 8 of the AdventOfCode 2015
	// http://adventofcode.com/day/8

	var raw = GetInput();
	//raw.Dump();
	int chcount = 0;
	int convcount = 0;
	int p2count = 0;
	Regex re_hex = new Regex(@"(\\x[0-9a-fA-F]{2})");
	foreach (string s in raw)
	{
		chcount += s.Length;
		string stripped = s.Remove(s.Length - 1, 1).Remove(0, 1);

		string rep1;
		if (stripped.Contains("\\x"))
		{
			rep1 = re_hex.Replace(stripped, "_");
		}
		else
		{
			rep1 = stripped;
		}
		string rep2 = rep1.Replace("\\\"", "\"").Replace("\\\\", "\\");
		
		convcount += rep2.Length;
		//rep2.Dump();

		// Part 2
		StringBuilder sb = new StringBuilder();
		sb.Append("\"");
		foreach (char ch in s)
		{
			if ((ch == '"') || (ch == '\\'))
			{
				sb.Append(@"\");
			}
			sb.Append(ch);
		}
		sb.Append("\"");
		string s3 = sb.ToString();
		p2count += s3.Length;
		//s3.Dump();
	}
	
	chcount.Dump("Text count");
	convcount.Dump("Code count");
	(chcount - convcount).Dump("Result");
	"".Dump();
	p2count.Dump("Part 2 count");
	(p2count - chcount).Dump("Result 2");
}

public IEnumerable<string> GetInput()
{
	string fname = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day8-input.txt");
	var lines = File.ReadAllLines(fname);
	return lines.ToList();
}
