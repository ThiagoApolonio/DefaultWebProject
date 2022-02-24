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
//    public class ProdutosPorFiliaisController : ApiController
//    {

//        [System.Web.Http.Route("GetProdutosPorFiliais")]
//        [ResponseType(typeof(ProdutosPorFiliaisModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetProdutosPorFiliais()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<ProdutosPorFiliaisModel> list = new List<ProdutosPorFiliaisModel>();
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
//                        ProdutosPorFiliaisModel F = new ProdutosPorFiliaisModel();

//                        F.idFiliais  = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
//                        F.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        F.estoque =  doc.Recordset.Fields.Item("estoque").Value.ToString();

//                        list.Add(F);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<ProdutosPorFiliaisModel>>(list);

//            }


//        }

//        [System.Web.Http.Route("GetProdutosPorFiliais(ID)")]
//        [ResponseType(typeof(ProdutosPorFiliaisModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetProdutosPorFiliaisID()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<ProdutosPorFiliaisModel> list = new List<ProdutosPorFiliaisModel>();
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
//                        ProdutosPorFiliaisModel F = new ProdutosPorFiliaisModel();

//                        F.idFiliais  = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
//                        F.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        F.estoque =  doc.Recordset.Fields.Item("estoque").Value.ToString();

//                        list.Add(F);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<ProdutosPorFiliaisModel>>(list);

//            }


//        }

//    }
//}