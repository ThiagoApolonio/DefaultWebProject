using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;

namespace DefaultWebProject.Tokken
{
    public class Sapcredentials
    {
        private Sapcredentials()
        {

        }

        public Sapcredentials(bool GetEmpresas, XmlOrConfigEnum XmlOrConfg)
        {
            GetCnfg(XmlOrConfg);
        }
        public string Servidor_licenca { get; set; }
        public string Usuario_sap { get; set; }
        public string Senha_sap { get; set; }
        public string Banco_Dados { get; set; }
        public string Servidor_banco { get; set; }
        public string Usuario_banco { get; set; }
        public string Senha_banco { get; set; }
        public string Versao_banco { get; set; }
        public string ChavePrivada { get; set; }
        public DateTime CompanyFirsTime { get; set; }

        public List<Sapcredentials> Empresas { get; set; }
        private void GetCnfg(XmlOrConfigEnum XmlOrConfg)
        {
            switch (XmlOrConfg)
            {
                case XmlOrConfigEnum.Xml:
                    Empresas = new List<Sapcredentials>();
                    XDocument xmlDoc = XDocument.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
                    foreach (var empresa in xmlDoc.Descendants("empresa"))
                    {
                        Empresas.Add(new Sapcredentials
                        {
                            Servidor_licenca = empresa.Element("servidor_licenca").Value,
                            Usuario_sap = empresa.Element("usuario_sap").Value,
                            Senha_sap = empresa.Element("senha_sap").Value,
                            Servidor_banco = empresa.Element("servidor_banco").Value,
                            Banco_Dados = empresa.Element("banco_dados").Value,
                            Usuario_banco = empresa.Element("usuario_banco").Value,
                            Senha_banco = empresa.Element("senha_banco").Value,
                            Versao_banco = empresa.Element("versao_banco").Value,
                            ChavePrivada = empresa.Element("BaseId").Value
                          
                        });
                    }
                    break;
                case XmlOrConfigEnum.Config:
                    Empresas = new List<Sapcredentials>();
                    Empresas.Add(new Sapcredentials
                    {
                        Servidor_licenca =
                        Usuario_sap = ConfigurationManager.AppSettings["user"].ToString(),
                        Senha_sap = ConfigurationManager.AppSettings["password"].ToString(),
                        Servidor_banco = ConfigurationManager.AppSettings["server"].ToString(),
                        Banco_Dados = ConfigurationManager.AppSettings["companydb"].ToString(),
                        Usuario_banco = ConfigurationManager.AppSettings["dbuser"].ToString(),
                        Senha_banco = ConfigurationManager.AppSettings["dbpass"].ToString(),
                        Versao_banco = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017.ToString(),
                        ChavePrivada = ConfigurationManager.AppSettings["companydb"].ToString()
                    }) ;
                    break;

            }

        }
        private class obj //Clase privada de conexão
        {
            public string Server { get; set; }
            public string companydb { get; set; }
            public string dbuser { get; set; }
            public string dbpass { get; set; }
            public string user { get; set; }
            public string password { get; set; }
            public string licenseserver { get; set; }
            public BoDataServerTypes BoDataServer { get; set; }
        }
    }

}