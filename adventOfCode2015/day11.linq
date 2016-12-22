<Query Kind="Program" />

void Main()
{
	// Routine to solve day 11 of the AdventOfCode 2015
	// http://adventofcode.com/day/11

	Test();
	string current = "hepxcrrq";
	string result = GetNextPassword(current);
	result.Dump("New password");

	// Part 2
	string result2 = GetNextPassword(result);
	result2.Dump("Next new password");
}

public void Test()
{
	// With the input of "abcdefgh" the next password should be "abcdffaa"
	string current = "abcdefgh";
	string expected = "abcdffaa";

	// Make sure the expected password passes
	if (!TestPassword(expected))
	{
		throw new Exception(string.Format("Expected password failed: \"{0}\"", expected));
	}

	string actual = GetNextPassword(current);

	if (expected == actual)
	{
		Console.WriteLine("Test 1 passed");
	}
	else
	{
		throw new Exception(string.Format("Test failed. Expected \"{0}\" but got \"{1}\"", expected, actual));
	}

	// Second test
	current = "ghijklmn";
	expected = "ghjaabcc";

	// Make sure the expected password passes
	if (!TestPassword(expected))
	{
		throw new Exception(string.Format("Expected password failed: \"{0}\"", expected));
	}
	actual = GetNextPassword(current);

	if (expected == actual)
	{
		Console.WriteLine("Test 2 passed");
	}
	else
	{
		throw new Exception(string.Format("Test failed. Expected \"{0}\" but got \"{1}\"", expected, actual));
	}

}

public string GetNextPassword(string currentPassword)
{
	string newPwd = IncrementPassword(currentPassword);
	while (true)
	{
		if (TestPassword(newPwd))
		{
			return newPwd;
		}
		newPwd = IncrementPassword(newPwd);
	}
}

Regex re1 = new Regex("[iol]");
Regex re2 = new Regex(@"(?<pair>((?<ch>[a-z])\k<ch>)).*(?<pair2>((?<ch2>[a-z])\k<ch2>))");
//Regex re2 = new Regex(@"(?<pair>(?<ch>[a-z])\k<ch>).*(?<pair2>(?<ch2>[a-z])\k<ch2>)");

public bool TestPassword(string pwd)
{
	// Must be 8 characters - it should always pass this
	if (pwd.Length != 8) throw new Exception("Password length is wrong. \"" + pwd + "\"");

	// Cannot contain i, o, l
	if (re1.IsMatch(pwd)) return false;
	// Must have two different pairs of letters
	var match = re2.Match(pwd);
	if (!match.Success)
	{
		return false;
	}
	else 
	{
		// Check the pairs are different
		if (match.Groups["pair"].Value == match.Groups["pair2"].Value) return false;
	}
	// Must have sequence
	if (!TestSequence(pwd)) return false;
	
	return true;
}

public bool TestSequence(string pwd)
{
	var chars = pwd.ToCharArray();
	int index = 0;
	while (index < chars.Length - 2)
	{
		char ch = chars[index];
		// Test next char
		if (chars[index + 1] == (char)(((int)ch) + 1))
		{
			// Test next plus one char
			if (chars[index + 2] == (char)(((int)ch) + 2))
			{	
				return true;
			}
        }
		index++;
	}

	return false;
}

public string IncrementPassword(string current)
{
	Char last = current.ToCharArray().Last();
	if (last == 'z')
	{
		// Recursively increment the other characters
		string temp = IncrementPassword(current.Remove(current.Length - 1, 1));
		return temp + 'a';
	}
	else
	{
		return current.Remove(current.Length - 1, 1) + (char)(((int)last) + 1);
	}
}