<Query Kind="Program" />

void Main()
{
	// Routine to solve day 10 of the AdventOfCode 2015
	// http://adventofcode.com/day/10

	// Test input - should return length = 6, string = "312211"
	//string input = "1";
	//int count = 5;

	string input = "1321131112";
	// Part 1
	int count = 40;
	// ...or part 2
	//int count = 50;
	
	string result = "";
	for (int index = 0; index < count; index++)
	{
		result = ProcessSequence(input);
		input = result;
	}
	
	result.Length.Dump("Result length");
	//result.Dump();
}

public string ProcessSequence(string input)
{
	StringBuilder sb = new StringBuilder();
	int currentDigit = 0, activeDigit = 0, count = 0;
	foreach (char ch in input)
	{
		currentDigit = (int)Char.GetNumericValue(ch);
		if (activeDigit != currentDigit)
		{
			// New chunk starting so save the previous one
			if (count > 0)
			{
				// Write it out to the result
				sb.Append(count.ToString());
				sb.Append(activeDigit.ToString());
			}
			
			// Start new
			count = 0;
			activeDigit = currentDigit;
		}
		count++;
	}

	// Write out the last chunk
	if (count > 0)
	{
		// Write it out to the result
		sb.Append(count.ToString());
		sb.Append(activeDigit.ToString());
	}

	return sb.ToString();
}
