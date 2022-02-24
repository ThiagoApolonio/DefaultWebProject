using DefaultWebProject.Models;
using DefaultWebProject.TratamentoString;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Conexao
{
    public class ConnectionWithXml
    {
        string service;
        string banco;
        string user;
        string password;
        SqlConnection con;

        public ConnectionWithXml()
        {
            var xml = new ConfigXmlDocument();

            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");

            this.service = xml.GetElementsByTagName("servidor_banco").Item(0).InnerXml.ToString();
            this.banco = xml.GetElementsByTagName("banco_dados").Item(0).InnerXml.ToString();
            this.user = xml.GetElementsByTagName("usuario_banco").Item(0).InnerXml.ToString();
            this.password = xml.GetElementsByTagName("senha_banco").Item(0).InnerXml.ToString();
            this.con = new SqlConnection(string.Format($@"Server={service};Database='{banco}';User Id={user};Password={password};")); ;

        }


        public SqlConnection conexaoBanco()
        {
            SqlConnection con = new SqlConnection(string.Format($@"Server={service};Database='{banco}';User Id={user};Password={password};")); ;
            return con;
        }

        public List<DadosErroModel> ConsultarPN(List<Lines> lines)
        {
            List<DadosErroModel> dadosErro = new List<DadosErroModel>();

            for (int i = 0; i < lines.Count; i++)
            {
                SqlCommand cmd = new SqlCommand(string.Format($@"select count(cardcode) from ocrd where CardCode = '{lines[i].ShortName}'"), con);
                DadosErroModel dados = new DadosErroModel();
                con.Open();
                int result = (int)cmd.ExecuteScalar();
                con.Close();
                if (result == 0)
                {
                    dados.Campo = "ShortName";
                    dados.conteudo = lines[i].ShortName;
                    dados.mgsErro = "PN não cadastrado";
                    dadosErro.Add(dados);
                }
            }

            return dadosErro;
        }

        public bool ConsultarPRJ(string codePRJ)
        {

            SqlCommand cmd = new SqlCommand(string.Format($@"select count(PrjCode) from OPRJ where PrjCode = '{codePRJ}'"), con);
            con.Open();
            int result = (int)cmd.ExecuteScalar();
            con.Close();
            if (result == 0)
            {
                return false;
            }

            return true;
        }
    }
}