using DefaultWebProject.Conexao;
using DefaultWebProject.Controllers;
using DefaultWebProject.Log;
using DefaultWebProject.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Tokken
{
    public class ValidarLogin
    {
        public ValidarLogin() { }

        public static string LogFile = "";

        public void LoginValidate(Sapcredentials credencial)
        {
            try
            {
                //SAPController.Sapcredentials = new Sapcredentials(true).Empresas;
                string companyTime = "";
                var companie = CompanyList.SAPCompanies.companyLists.Where(x => x.Company.CompanyDB == credencial.Banco_Dados).FirstOrDefault();
                if (companie == null)
                {
                    TentativaReconectar(credencial);
                    companie = CompanyList.SAPCompanies.companyLists.Where(x => x.Company.CompanyDB == credencial.Banco_Dados).FirstOrDefault();
                }
         
            }
            catch (Exception ex)
            {
                LogFile =LogFile = new GravarLog(LogFile).Escrever("::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
               LogFile =LogFile = new GravarLog(LogFile).Escrever("ERRO EM VALIDACAO LOGIN:::::>>>" + ex.Message);
               LogFile =LogFile = new GravarLog(LogFile).Escrever("Solicitando Reconexao");
                var companie = CompanyList.SAPCompanies.companyLists.Where(x => x.Company.CompanyDB == credencial.Banco_Dados).FirstOrDefault();
                if (companie == null)
                {
                    CompanyList.ConexaoInicial(credencial);
                   LogFile =LogFile = new GravarLog(LogFile).Escrever("Reconexão Solicitada");
                }
                else
                {
                   LogFile =LogFile = new GravarLog(LogFile).Escrever("Empresa reconectada: " + companie.Company.CompanyName);
                }
               LogFile =LogFile = new GravarLog(LogFile).Escrever("::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
            }
        }

        private void TentativaReconectar(Sapcredentials credencial)
        {
           LogFile = new GravarLog(LogFile).Escrever("::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::");
           LogFile = new GravarLog(LogFile).Escrever("Empresa não conectada. Realizando tentativa de estabelecer uma nova conexão em 10 segundos.");
            System.Threading.Thread.Sleep(10000);
           LogFile = new GravarLog(LogFile).Escrever("Resetando variaveis do sistema.");
            CompanyList.SAPCompanies.companyLists.Clear();
            CompanyList.SAPCompanies = null;
           LogFile = new GravarLog(LogFile).Escrever("Iniciando reconexão com o sistema.");
            CompanyList.ConexaoInicial(credencial);
           LogFile = new GravarLog(LogFile).Escrever("Finalizada tentativa de reconexão.");
           LogFile = new GravarLog(LogFile).Escrever("Iniciando validação de reconexão.");

            var companie = CompanyList.SAPCompanies.companyLists.Where(x => x.Company.CompanyDB == credencial.Banco_Dados).FirstOrDefault();

            if (companie == null)
               LogFile = new GravarLog(LogFile).Escrever("Empresa não reconectada. Erro interno DIAPI.");
            else
               LogFile = new GravarLog(LogFile).Escrever("Empresa reconectada: " + companie.Company.CompanyName);
        }
        private (string, Company) LoginSAP(Sapcredentials credencial, CompaniaSap companie)
        {
            try
            {
                int errorCode = 0;
                string errorMessage = string.Empty;

                if (companie.Company == null || !companie.Company.Connected)
                {
                    companie.Company = new SAPbobsCOM.Company();
                    companie.Company.Server = credencial.Servidor_banco;//mandatory property 							

                    switch (credencial.Versao_banco)
                    {
                        case "MSSQL2008":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                            break;
                        case "MSSQL2012":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                            break;
                        case "MSSQL2014":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                            break;
                        case "MSSQL2016":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                            break;
                        case "MSSQL2017":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                            break;
                        case "HANADB":
                            companie.Company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                            break;
                    }
                    companie.Company.CompanyDB = credencial.Banco_Dados;//mandatory property 
                    companie.Company.UserName = credencial.Usuario_sap;//mandatory property 
                    companie.Company.Password = credencial.Senha_sap;//mandatory property 

                    companie.Company.DbUserName = credencial.Usuario_banco; //optional in release 8.8 and afterwards 
                    companie.Company.DbPassword = credencial.Senha_banco; //optional in release 8.8 and afterwards 
                    companie.Company.UseTrusted = false; //optional in release 8.8 and afterwards 			 
                    companie.Company.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br; //optional 
                                                                                         //Optional, default value is from DI configuration file in release 8.8 and afterwards 
                    companie.Company.LicenseServer = credencial.Servidor_licenca;
                    errorCode = companie.Company.Connect();
                    if (errorCode != 0)
                    {
                        string msg = companie.Company.GetLastErrorDescription(); //You can also use GetLastError to get the error code and error message at the same time.                      
                        return (msg, new Company());
                    }
                    else
                    {
                        companie.CompanyFirsTime = DateTime.Now;
                    }
                }
                else
                {
                    return ("Connectado", companie.Company);
                }

                return ("Connectado", companie.Company);
            }
            catch (Exception ex)
            {
                return (ex.Message, new Company());
            }
        }
    }
}