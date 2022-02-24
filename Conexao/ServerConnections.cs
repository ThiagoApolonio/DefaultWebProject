using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Configuration;

using System.Xml.Linq;
using DocumentFormat.OpenXml.Bibliography;
using System.Data.SqlClient;
using Translator;

namespace DefaultWebProject.Conexao
{
    public class ServerConnections
    {
        //Declarações privadas locais 
        public static SAPbobsCOM.Company company = new Company();
        //public static SAPbobsCOM.Items oItems = new Items();
        private int connectionResult; // Sucesso == 0 
        private int errorCode = 0; //Codigo do Erro 
        private string errorMessage = "";//String de Msg de Erro
        public static TranslatorTool Translator { get;  set; }
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
        public int Connect()
        {
            //Passando Appsettings Para um Objeto Local

            obj objConexao = new obj();
     
            objConexao.Server = ConfigurationManager.AppSettings["server"].ToString();
            objConexao.companydb = ConfigurationManager.AppSettings["companydb"].ToString();
            objConexao.dbuser = ConfigurationManager.AppSettings["dbuser"].ToString();
            objConexao.dbpass = ConfigurationManager.AppSettings["dbpass"].ToString();
            objConexao.BoDataServer = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
            objConexao.user = ConfigurationManager.AppSettings["user"].ToString();
            objConexao.password = ConfigurationManager.AppSettings["password"].ToString();
            company.Server = objConexao.Server;
            company.CompanyDB = objConexao.companydb;
            company.DbUserName = objConexao.dbuser;
            company.DbPassword = objConexao.dbpass;
            company.LicenseServer = objConexao.licenseserver;
            company.UserName = objConexao.user;
            company.Password = objConexao.password;
            company.DbServerType = objConexao.BoDataServer;

            connectionResult = company.Connect();


            if (connectionResult != 0)
            {
                //Conexão Falhou
                company.GetLastError(out errorCode, out errorMessage);//Utilizando o metodo GetLastError Da CompanyClass Conseguimos Capturar o erro exato gerado ao tentar se conectar com A DI API sap
            }

            return connectionResult;
        }

        public static string TranslateToHana(string sql)
        {
            int count;
            int errCount;

            if (company.DbServerType == (BoDataServerTypes)9) // 9 = Hana
            {
                if (Translator == null)
                {
                    Translator = new TranslatorTool();
                }
                sql = Translator.TranslateQuery(sql, out count, out errCount);
                sql = sql.Substring(0, sql.Length - 3);
                Console.WriteLine(sql);
            }
            return sql;
        }
        public SAPbobsCOM.Company GetCompany()
        {

            return company;
        }
        public int GetErrorCode()
        {
            return this.errorCode;
        }
        public string GetErrorMessage()
        {
            return this.errorMessage;
        }

    }
}