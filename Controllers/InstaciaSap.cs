using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Controllers
{
    public class InstanciaSap : IDisposable
    {
        public InstanciaSap() { }

        public InstanciaSap(SAPbobsCOM.Company company)
        {
            JournalVouchers = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
            BusinessPartners = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            Items = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
            Recordset = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            Orders = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
        }
        public SAPbobsCOM.JournalVouchers JournalVouchers { get; set; }
        public SAPbobsCOM.BusinessPartners BusinessPartners { get; set; }
        public SAPbobsCOM.Items Items { get; set; }
        public SAPbobsCOM.Recordset Recordset { get; set; }
        public SAPbobsCOM.Documents Orders { get; set; }

        public void Dispose()
        {
            if (JournalVouchers != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(JournalVouchers);

            JournalVouchers = null; 

            if (BusinessPartners != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(BusinessPartners);

            BusinessPartners = null;

            if (Items != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Items);

            Items = null;  

            if (Recordset != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Recordset);

            Recordset = null;
            if (Orders != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(Orders);

            Orders = null;
        }
    }
}
 