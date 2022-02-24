using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace DefaultWebProject.TratamentoString
{
    public class Lines
    {
        public Lines() { }
        public string AccountCode { get; set; }
        public string ShortName { get; set; }
        public string Filial { get; set; }
        public string Memo { get; set; } = "";
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string CostingCode { get; set; }
        public string ProjectCode { get; set; }
        public string Reference1 { get; set; }

        public List<Lines> GetLines(string lines)
        {
            lines = string.Format(@"<root>{0}</root>", lines);
            var lst = new List<Lines>();
            var ln = new Lines();
            System.Xml.XmlDocument Lines = new XmlDocument();
            Lines.LoadXml(lines);

            var xmlDoc = XDocument.Parse(Lines.OuterXml);

            foreach (var he in xmlDoc.Descendants("row"))
            {
                try
                {
                    ln.AccountCode = he.Element("AccountCode").Value;

                }
                catch
                {
                    ln.ShortName = he.Element("ShortName").Value;
                }

                try
                {
                    ln.Filial = he.Element("BplId").Value;
                }
                catch
                {
                    ln.Filial = "1";
                }
                ln.Debit = he.Element("Debit").Value;
                ln.Credit = he.Element("Credit").Value;
                try
                {
                    ln.CostingCode = he.Element("CostingCode").Value;
                }
                catch
                {
                }
                try
                {
                    ln.ProjectCode = he.Element("ProjectCode").Value;
                }
                catch
                {
                }

                try
                {
                    ln.Reference1 = he.Element("Reference1").Value;
                }
                catch
                {
                    ln.Reference1 = "";
                }

                try
                {
                    ln.Memo = he.Element("Memo").Value;
                }
                catch
                {
                    ln.Memo = "";
                }

                lst.Add(ln);
                ln = new Lines();
            }
            return lst;
        }

        public string GetElementByID(XmlDocument xmldoc, string id)
        {
            try
            {
                return xmldoc.GetElementsByTagName(id).Item(0).InnerXml.ToString().Replace("\\\\", "\\");
            }
            catch
            {
                return "";
            }
        }
    }
}