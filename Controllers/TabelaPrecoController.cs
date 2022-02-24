//using DefaultWebProject.Conexao;
//using DefaultWebProject.Models;
//using DefaultWebProject.Tokken;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Web;
//using System.Web.Http;
//using System.Web.Http.Description;

//namespace DefaultWebProject.Controllers
//{
//    public class TabelaPrecoController :ApiController
//    {
//        [System.Web.Http.Route("GetTabelaPreco")]
//        [ResponseType(typeof(TabelaPrecoModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTabelaPreco()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TabelaPrecoModel> list = new List<TabelaPrecoModel>();
//            using (var doc = new InstanciaSap(comp.Company))
//            {
//                comp.Company.Connect();

//                string sql = String.Format("");
//                doc.Recordset.DoQuery(sql);
//                if (doc.Recordset.RecordCount > 0)
//                {
//                    doc.Recordset.MoveFirst();
//                    for (int i = 0; i < doc.Recordset.RecordCount; i++)
//                    {
//                        TabelaPrecoModel T = new TabelaPrecoModel();

                        
//                        T.idTabelaPreco = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.codigoTabela = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.codigoTabelaGrupo = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.validadeInicial = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.validadeFinal = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiroAutomatico = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiro = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.ativo = doc.Recordset.Fields.Item("descricao").Value.ToString();
                



//                        list.Add(T);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TabelaPrecoModel>>(list);

//            }


//        }
//        [System.Web.Http.Route("GetTabelaPreco(ID)")]
//        [ResponseType(typeof(TabelaPrecoModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTabelaPrecoID(string ID)
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TabelaPrecoModel> list = new List<TabelaPrecoModel>();
//            using (var doc = new InstanciaSap(comp.Company))
//            {
//                comp.Company.Connect();

//                string sql = String.Format("",ID);
//                doc.Recordset.DoQuery(sql);
//                if (doc.Recordset.RecordCount > 0)
//                {
//                    doc.Recordset.MoveFirst();
//                    for (int i = 0; i < doc.Recordset.RecordCount; i++)
//                    {
//                        TabelaPrecoModel T = new TabelaPrecoModel();

                        
//                        T.idTabelaPreco = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.codigoTabela = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.codigoTabelaGrupo = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.validadeInicial = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.validadeFinal = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiroAutomatico = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.indiceFinanceiro = doc.Recordset.Fields.Item("descricao").Value.ToString();
//                        T.ativo = doc.Recordset.Fields.Item("descricao").Value.ToString();
                



//                        list.Add(T);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TabelaPrecoModel>>(list);

//            }


//        }
//    }
//}