using DefaultWebProject.Controllers;
using DefaultWebProject.Log;
using DefaultWebProject.Tokken;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DefaultWebProject.Conexao
{
    public class CompaniaSap
    {

        public CompaniaSap()
        {
        }

        public SAPbobsCOM.Company Company { get; set; }

        public DateTime CompanyFirsTime { get; set; }
        public string DataBase { get; set; }
        public string ID { get; set; }    

        public string Mensagem { get; set; } = "";


        public List<CompaniaSap> ConnectList(List<Sapcredentials> credentials)
        {
            JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("INICIANDO CONEXÃO COM BANCO");
            var lst = new List<CompaniaSap>();
            var comp = new CompaniaSap();
            JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("RECUPERADA LISTA DE EMPRESAS");
            foreach (var credential in credentials)
            {
                (comp.Company, comp.Mensagem) = ConnectToCompany(credential);
                comp.DataBase = credential.Banco_Dados;
                comp.ID = credential.ChavePrivada;
                comp.CompanyFirsTime = DateTime.Now;
                lst.Add(comp);
                comp = new CompaniaSap();
            }
            return lst;
        }

        private (SAPbobsCOM.Company, string) ConnectToCompany(Sapcredentials credentials)
        {
            JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("-INSTANCIANDO EMPRESA");

            int errorCode;
            string errorMessage;

            Company = new SAPbobsCOM.Company();

            #region Empresa

            Company.Server = credentials.Servidor_banco;//mandatory property 
            Company.LicenseServer = credentials.Servidor_licenca;
            JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("SELECIONA VERSÃO DO BANCO DE DADOS");
            switch (credentials.Versao_banco)
            {
                case "MSSQL2008":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    break;
                case "MSSQL2012":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
                case "MSSQL2014":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
                case "MSSQL2016":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                    break;
                case "MSSQL2017":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                    break;
                case "HANADB":
                    Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    break;
            }
            Company.CompanyDB = credentials.Banco_Dados;//mandatory property 
            Company.UserName = credentials.Usuario_sap;//mandatory property 
            Company.Password = credentials.Senha_sap;//mandatory property 

            Company.DbUserName = credentials.Usuario_banco; //optional in release 8.8 and afterwards 
            Company.DbPassword = credentials.Senha_banco; //optional in release 8.8 and afterwards             
            Company.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br; //optional 

            #endregion

            JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("TENTANDO CONECTAR...");
            errorCode = Company.Connect();
            if (errorCode != 0)
            {
                errorMessage = Company.GetLastErrorDescription();
                errorMessage = ("Fail to conect to SAP Business One. " + "Error Code: " + errorCode.ToString() + " Error Message: " + errorMessage);
                JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("ERRO AO CONECTAR COM SAP: " + errorMessage);
                return (new SAPbobsCOM.Company(), errorMessage);
            }
            else
            {
                JournalVouchersController.LogFile = new GravarLog(JournalVouchersController.LogFile).Escrever("CONECTADO EM EMPRESA: " + Company.CompanyName);
                return (Company, "Sucesso");
            }
        }
        public CompaniaSap ConectConfig(string BaseId)
        {
            List<PersonalAuthentication> credential = new List<PersonalAuthentication>();
            XDocument xml = XDocument.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
            foreach (var itemXml in xml.Descendants("empresa"))
            {
                credential.Add(new PersonalAuthentication
                {
                    Username = itemXml.Element("usuario_sap").Value,
                    Password = itemXml.Element("senha_sap").Value,
                    BaseId = itemXml.Element("BaseId").Value

                });
            }
            var cred = credential.Where(x => x.BaseId == BaseId).FirstOrDefault();
            var (validacao, empresa) = new ValidarToken().CredencialValida(cred);
            CompanyList.ConexaoInicial(empresa);
            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.ID == BaseId).FirstOrDefault();
            return comp;
        }
    }
}