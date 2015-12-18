<Query Kind="Statements">
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

// Routine to solve day 4 of the AdventOfCode 2015
// http://adventofcode.com/day/4

string key = "ckczppom";

MD5 md = MD5.Create();
int suffix = 0;
bool found5 = false, found6 = false;
while (!(found5 && found6))
{
	string str = string.Format("{0}{1}", key, suffix);
	var hash = md.ComputeHash(Encoding.UTF8.GetBytes(str));
	string hashstr = string.Join("", hash.Select(b => b.ToString("X2")));

	if (!found5 && hashstr.StartsWith("00000"))
	{
		found5 = true;
		suffix.Dump("The suffix required for 5 zeros is");
	}
	if (!found6 && hashstr.StartsWith("000000"))
	{
		found6 = true;
		suffix.Dump("The suffix required for 6 zeros is");
	}
	suffix++;
}

