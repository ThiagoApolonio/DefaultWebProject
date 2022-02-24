using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;

namespace DefaultWebProject.TratamentoString
{
    public class Header
    {

        public Header() { }

        public DateTime ReferenceDate { get; set; }
        public string Reference { get; set; }
        public string Reference2 { get; set; }
        public string TransactionCode { get; set; }
        public string ProjectCode { get; set; }
        public string Series { get; set; }
        public string Indicator { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public string UseAutoStorno { get; set; }
        public DateTime StornoDate { get; set; }
        public string Memo { get; set; }

        public Header GetHeader(string header)
        {
            header = string.Format(@"<root>{0}</root>", header);
            XmlDocument Header = new XmlDocument();
            Header.LoadXml(header);
            var h = new Header();
            var datastr = Header.GetElementsByTagName("ReferenceDate").Item(0).InnerXml.ToString().Replace("\\\\", "\\");
            if (!string.IsNullOrEmpty(datastr))
                h.ReferenceDate = DateTime.ParseExact(datastr, "yyyyMMdd", CultureInfo.InvariantCulture);
            h.TaxDate = h.ReferenceDate;
            h.DueDate = h.ReferenceDate;
            h.StornoDate = DateTime.UtcNow.AddMonths(1);

            h.Reference = !string.IsNullOrEmpty(GetElementByID(Header, "Reference")) ? GetElementByID(Header, "Reference") : "";

            h.Reference2 = !string.IsNullOrEmpty(GetElementByID(Header, "Reference2")) ? GetElementByID(Header, "Reference2") : "";

            h.TransactionCode = !string.IsNullOrEmpty(GetElementByID(Header, "TransactionCode")) ? GetElementByID(Header, "TransactionCode") : "";

            h.ProjectCode = !string.IsNullOrEmpty(GetElementByID(Header, "ProjectCode")) ? GetElementByID(Header, "ProjectCode") : "";

            h.Indicator = !string.IsNullOrEmpty(GetElementByID(Header, "Indicator")) ? GetElementByID(Header, "Indicator") : "";

            h.Series = !string.IsNullOrEmpty(GetElementByID(Header, "Series")) ? GetElementByID(Header, "Series") : "";

            h.UseAutoStorno = GetElementByID(Header, "UseAutoStorno");

            h.Memo = GetElementByID(Header, "Memo");

            return h;
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