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
//    public class TabelaPrecoItemController :ApiController
//    {
//        [System.Web.Http.Route("GetTabelaPrecoItem")]
//        [ResponseType(typeof(TabelaPrecoItemModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTabelaPrecoItem()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TabelaPrecoItemModel> list = new List<TabelaPrecoItemModel>();
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
//                        TabelaPrecoItemModel TP = new TabelaPrecoItemModel();

//                        TP.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        TP.idTabelaPreco = doc.Recordset.Fields.Item("idTabelaPreco").Value.ToString();
//                        TP.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("indiceFinanceiroOpcional").Value.ToString();
//                        TP.preco = doc.Recordset.Fields.Item("preco").Value.ToString();
//                        TP.quantidade = doc.Recordset.Fields.Item("quantidade").Value.ToString();



//                        list.Add(TP);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TabelaPrecoItemModel>>(list);

//            }


//        }
//        [System.Web.Http.Route("GetTabelaPrecoItem(ID)")]
//        [ResponseType(typeof(TabelaPrecoItemModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTabelaPrecoItemIID(string ID )
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TabelaPrecoItemModel> list = new List<TabelaPrecoItemModel>();
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
//                        TabelaPrecoItemModel TP = new TabelaPrecoItemModel();

//                        TP.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        TP.idTabelaPreco = doc.Recordset.Fields.Item("idTabelaPreco").Value.ToString();
//                        TP.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("indiceFinanceiroOpcional").Value.ToString();
//                        TP.preco = doc.Recordset.Fields.Item("preco").Value.ToString();
//                        TP.quantidade = doc.Recordset.Fields.Item("quantidade").Value.ToString();



//                        list.Add(TP);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TabelaPrecoItemModel>>(list);

//            }


//        }

//    }
//}