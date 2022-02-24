using DefaultWebProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Conexao.QueryRecordSet
{

    public class SboRecordSet : IDisposable
    {
        public SboRecordSet(string banco)
        {
            SAPConpany = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == banco).FirstOrDefault();
        }
        private CompaniaSap SAPConpany;
        private SAPbobsCOM.Recordset Rs { get; set; }
        public SAPbobsCOM.Recordset ExecutarQuery(string query)
        {
            Rs = SAPConpany.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            Rs.DoQuery(query);
            return Rs;
        }

        public string QueryContaContabil(string conta)
        {
            return string.Format(@"select AcctCode from OACT where AcctCode = '{0}'", conta);
        }

        public string QueryCodigoParceiro(string conta)
        {
            return string.Format(@"select CardCode from OCRD where CardCode = '{0}'", conta);
        }

        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(Rs);
            Rs = null;
            SAPConpany = null;
            GC.Collect();
        }
    }

}