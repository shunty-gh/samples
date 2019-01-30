<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>ClosedXML</NuGetReference>
  <NuGetReference>DocumentFormat.OpenXml</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>NLog</NuGetReference>
  <Namespace>DocumentFormat.OpenXml.Packaging</Namespace>
  <Namespace>DocumentFormat.OpenXml.Spreadsheet</Namespace>
  <Namespace>ClosedXML.Excel</Namespace>
</Query>

void Main()
{
    // Read first row of a worksheet
    // Answer for SO question https://stackoverflow.com/q/51736561
    
	var fileName = @"c:\temp\openxml-read-row.xlsx";
	
    using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
        using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fs, false))
		{
	
			// Get the necessary bits of the doc
			WorkbookPart workbookPart = doc.WorkbookPart;
			SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
			SharedStringTable sst = sstpart.SharedStringTable;
			WorkbookStylesPart ssp = workbookPart.GetPartsOfType<WorkbookStylesPart>().First();
			Stylesheet ss = ssp.Stylesheet;
			
			// Get the first worksheet
			WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
			Worksheet sheet = worksheetPart.Worksheet;
	
			var rows = sheet.Descendants<Row>();
            var row = rows.First();
	        foreach (var cell in row.Descendants<Cell>())
            {
                var txt = GetCellText(cell, sst);
                $"{cell.CellReference} = {txt}".Dump();
            }
		}
	}	
    
    // Using ClosedXML instead
    using (var workbook = new XLWorkbook(fileName))
    {
        var worksheet = workbook.Worksheets.First();
        var row = worksheet.Row(1);
        foreach (var cell in row.CellsUsed())
        {
            var txt = cell.Value.ToString();
            $"{cell.Address.ToString()} = {txt}".Dump();
        }
    }
}

// Very basic way to get the text of a cell using OpenXML
private string GetCellText(Cell cell, SharedStringTable sst)
{
	if (cell == null)
		return "";
		
	if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
	{
		int ssid = int.Parse(cell.CellValue.Text);
		string str = sst.ChildElements[ssid].InnerText;
		return str;
	}
	else if (cell.CellValue != null)
	{
		return cell.CellValue.Text;
	}
	return "";
}
