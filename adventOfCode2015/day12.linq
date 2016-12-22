<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

void Main()
{
	// Routine to solve day 12 of the AdventOfCode 2015
	// http://adventofcode.com/day/12

	var input = GetInput();
	int sum = 0, ch0 = (int)'0', ch9 = (int)'9';
	string currentNum = "";
	char previous = ' ';
	bool isNeg = false;
	List<int> numbers = new List<int>();
	foreach (char ch in input)
	{
		int chThis = (int)ch;
		if ((chThis >= ch0) && (chThis <= ch9))
		{
			currentNum += ch;
			if (previous == '-')
			{
				isNeg = true;
			}
        }
		else if (!string.IsNullOrWhiteSpace(currentNum))
		{
			int thisValue = int.Parse(currentNum);
			if (isNeg)
			{
				thisValue *= -1;
			}
			numbers.Add(thisValue);
			sum += thisValue;
			// Reset
			isNeg = false;
			currentNum = "";
			
		}
		previous = ch;
	}
	
	sum.Dump("Sum is");
	numbers.Sum().Dump("Numbers list sum is");
	//numbers.Dump();

	// Part 2
	var root = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(input);
	sum = 0;
	foreach (var prop in root.Properties())
	{
		sum += ProcessProperty(prop);
	}
	sum.Dump("Sum 2");
}

public int ProcessProperty(JProperty property)
{
	int result = 0;
	switch (property.Value.Type)
	{
		case JTokenType.Array:
			//Console.WriteLine("Processing array " + propName);
			result += ProcessArray((JArray)property.Value);
			break;
		case JTokenType.Object:
			//Console.WriteLine("Processing object " + propName);
			result += ProcessObject((JObject)property.Value);
			break;
		case JTokenType.String:
			//Console.WriteLine("Processing string " + propName);
			// Ignore it
			break;
		case JTokenType.Integer:
			//Console.WriteLine("Processing int " + propName);
			result += (int)property.Value;
			break;
		default:
			Console.WriteLine("Unhandled property " + property.Name + "; Type=" + property.Value.Type.ToString());
			break;
	}
	return result;
}

public int ProcessObject(JObject obj)
{
	int result = 0;
	foreach (var pchild in obj.Properties())
	{
		// PART 2 : If this object has any property with the VALUE of "red" then ignore it
		if ((pchild.Value.Type == JTokenType.String) && ((string)pchild.Value == "red"))
		{
			return 0;
		}
		result += ProcessProperty(pchild);
	}
	return result;
}

public int ProcessArray(JArray array)
{
	int result = 0;
	foreach (var el in array)
	{
		switch (el.Type)
		{
			case JTokenType.Integer:
				result += (int)(JValue)el;
				break;
			case JTokenType.Array:
				result += ProcessArray((JArray)el);
				break;
			case JTokenType.Object:
				result += ProcessObject((JObject)el);
				break;
			default:
				break;
		}
	}
	return result;
}
public string GetInput()
{
	string fname = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day12-input.txt");
	var lines = File.ReadAllText(fname);
	return lines;
}