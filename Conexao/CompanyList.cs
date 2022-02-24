using DefaultWebProject.Controllers;
using DefaultWebProject.Log;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using DefaultWebProject.Conexao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using System.Configuration;

namespace DefaultWebProject.Conexao
{
    public class CompanyList
    {
        public static CompanyList SAPCompanies { get; set; }
        public CompaniaSap SapCompany = new CompaniaSap();
        public CompanyList()
        {
            companyLists = new CompaniaSap().ConnectList(new Sapcredentials(true, XmlOrConfigEnum.Xml).Empresas);
        }

        public List<CompaniaSap> companyLists { get; set; }

        public static void ConexaoInicial(Sapcredentials empresa)
        {

            if (SAPCompanies == null)
                SAPCompanies = new CompanyList();
            else
            {
                new ValidarLogin().LoginValidate(empresa);
            }
        }

        public static void ConexaoInicial()
        {
            if (SAPCompanies == null)
                SAPCompanies = new CompanyList();
        }
       
    }
}