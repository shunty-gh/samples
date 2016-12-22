<Query Kind="Statements" />

// SO answer https://stackoverflow.com/a/41281283
// Sample data is downloaded from Companies House daily download http://download.companieshouse.gov.uk/en_accountsdata.html
string fname = @"C:\temp\chdata\Prod223_1770_00101234_20160331.html";
var doc = XDocument.Load(fname);
var elements = doc.Root.Descendants().Where(x => x.Name.LocalName == "nonFraction").Where(x => x.Attributes().Any(a => a.Value.Contains("Intangible")));

var lines = new List<string>();
foreach (var element in elements)
{
    var attribs = element.Attributes();
    var ctx = attribs.FirstOrDefault(a => a.Name == "contextRef")?.Value ?? "";
    var dec = attribs.FirstOrDefault(a => a.Name == "decimals")?.Value ?? "";
    var scale = attribs.FirstOrDefault(a => a.Name == "scale")?.Value ?? "";
    var units = attribs.FirstOrDefault(a => a.Name == "unitRef")?.Value ?? "";
    var fmt = attribs.FirstOrDefault(a => a.Name == "format")?.Value ?? "";
    var name = attribs.FirstOrDefault(a => a.Name == "name")?.Value ?? "";
    var value = element.Value;

    string line = $"\"{ctx}\",\"{dec}\",\"{scale}\",\"{units}\",\"{name}\",\"{fmt}\",\"{value}\"";
    lines.Add(line);
    Console.WriteLine(line);
}
File.WriteAllLines(Path.ChangeExtension(fname, "csv"), lines);
