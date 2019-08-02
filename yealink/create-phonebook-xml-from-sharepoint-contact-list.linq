<Query Kind="Program">
  <Reference>&lt;ProgramFilesX64&gt;\Common Files\Microsoft Shared\Web Server Extensions\16\ISAPI\Microsoft.SharePoint.Client.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\Common Files\Microsoft Shared\Web Server Extensions\16\ISAPI\Microsoft.SharePoint.Client.Runtime.dll</Reference>
  <NuGetReference>SharePoint</NuGetReference>
  <Namespace>Microsoft.SharePoint</Namespace>
  <Namespace>Microsoft.SharePoint.Client</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Security</Namespace>
</Query>

void Main()
{
    // Get contact items from a SharePoint list
    // With help from 
    // https://docs.microsoft.com/en-us/previous-versions/office/developer/sharepoint-2010/ee534956(v=office.14)#retrieving-specific-fields-from-a-specified-number-of-items    
    //
    // and then put them into an XML phonebook document suitable for the Yealink phones.
    // Copy the phonebook.xml file into an IIS directory and point the "remote phonebook" entry
    // in the Yealink phone to the appropriate url. eg http://192.168.0.207/phonebook.xml
    
    var siteurl = "https://commandersoftware.sharepoint.com/";
    var listTitle = "Office Address Book";
    
    var context = new ClientContext(siteurl);   
    var usr = Util.GetPassword("SharePoint user");
    var pwd = new NetworkCredential("", Util.GetPassword("SharePoint password", true)).SecurePassword;
    context.Credentials = new SharePointOnlineCredentials(usr, pwd);
    
    var list = context.Web.Lists.GetByTitle(listTitle);
    //context.Load(list);
    //context.ExecuteQuery();

    var qry = new CamlQuery();
    //qry.ViewXml = "<View><Query><Where><Leq><FieldRef Name='ID'/><Value Type='Number'>100</Value></Leq></Where></Query><RowLimit>50</RowLimit></View>";
    var listitems = list.GetItems(qry);
    context.Load(
        listitems,
        //items => items.IncludeWithDefaultProperties(item => item.DisplayName));
        items => items.Include(
            item => item["FirstName"],
            item => item["FullName"],
            item => item["Email"],
            item => item["Company"],
            item => item["WorkPhone"],
            item => item["HomePhone"],
            item => item["CellPhone"]
            ));
        
    context.ExecuteQuery();

    var xml = new XElement("HBCSLIPPhoneDirectory");
    foreach (var item in listitems.ToList())
    {
        var el = new XElement("DirectoryEntry");
        
        var fn = (string)item["FullName"];
        var comp = (string)item["Company"];
        if (!string.IsNullOrWhiteSpace(fn))
            el.Add(new XElement("Name", fn));
        else if (!string.IsNullOrWhiteSpace(comp))
            el.Add(new XElement("Name", comp));
        else
            continue; // No name available for the entry

        var wp = CleanPhoneNumber((string)item["WorkPhone"]);
        var cp = CleanPhoneNumber((string)item["CellPhone"]);
        var hp = CleanPhoneNumber((string)item["HomePhone"]);
        
        if (string.IsNullOrWhiteSpace(wp) && string.IsNullOrWhiteSpace(cp) && string.IsNullOrWhiteSpace(hp))
            continue;
            
        if (!string.IsNullOrWhiteSpace(wp))
            el.Add(new XElement("Telephone", wp));
        if (!string.IsNullOrWhiteSpace(cp))
            el.Add(new XElement("Telephone", cp));
        if (!string.IsNullOrWhiteSpace(hp))
            el.Add(new XElement("Telephone", hp));

        xml.Add(el);
    }
    
    var fname = @"c:\temp\phonebook.xml";
    xml.Save(fname, SaveOptions.None);
}

public string CleanPhoneNumber(string src)
{
    // Strip spaces and alpha characters from the number
    if (string.IsNullOrWhiteSpace(src))
        return "";
    return string.Join("", src.Where(c => (c >= '0' && c <= '9') || (c == '+')));
}
