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
//    public class TributacaoController : ApiController 
//    {
//        [System.Web.Http.Route("GetTributacao")]
//        [ResponseType(typeof(TributacaoModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTributacao()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TributacaoModel> list = new List<TributacaoModel>();
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
//                        TributacaoModel T = new TributacaoModel();

//                        T.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
//                        T.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
//                        T.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        T.idPedidoTipo = doc.Recordset.Fields.Item("idPedidoTipo").Value.ToString();
//                        T.codigoImposto = doc.Recordset.Fields.Item("codigoImposto").Value.ToString();
//                        T.ipi = doc.Recordset.Fields.Item("ipi").Value.ToString();
//                        T.iva = doc.Recordset.Fields.Item("iva").Value.ToString();
//                        T.aliquotaInternaIcms = doc.Recordset.Fields.Item("aliquotaInternaIcms").Value.ToString();
//                        T.aliquotaExternaIcms = doc.Recordset.Fields.Item("aliquotaExternaIcms").Value.ToString();
//                        T.reducaoBaseIcms = doc.Recordset.Fields.Item("reducaoBaseIcms").Value.ToString();
//                        T.ReduçãoBaseIpi = doc.Recordset.Fields.Item("ReduçãoBaseIpi").Value.ToString();
//                        T.IpiBaseIcms = doc.Recordset.Fields.Item("IpiBaseIcms").Value.ToString();
//                        T.substituicaoTributaria = doc.Recordset.Fields.Item("substituicaoTributaria").Value.ToString();
//                        T.stPorPauta = doc.Recordset.Fields.Item("stPorPauta").Value.ToString();
//                        T.ignorarDescontoIcms = doc.Recordset.Fields.Item("ignorarDescontoIcms").Value.ToString();
//                        T.aliquotaFCP = doc.Recordset.Fields.Item("aliquotaFCP").Value.ToString();



//                        list.Add(T);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TributacaoModel>>(list);

//            }


//        }

//        [System.Web.Http.Route("GetTributacao(ID)")]
//        [ResponseType(typeof(TributacaoModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetTributacaoID(string ID)
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<TributacaoModel> list = new List<TributacaoModel>();
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
//                        TributacaoModel T = new TributacaoModel();

//                        T.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
//                        T.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
//                        T.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
//                        T.idPedidoTipo = doc.Recordset.Fields.Item("idPedidoTipo").Value.ToString();
//                        T.codigoImposto = doc.Recordset.Fields.Item("codigoImposto").Value.ToString();
//                        T.ipi = doc.Recordset.Fields.Item("ipi").Value.ToString();
//                        T.iva = doc.Recordset.Fields.Item("iva").Value.ToString();
//                        T.aliquotaInternaIcms = doc.Recordset.Fields.Item("aliquotaInternaIcms").Value.ToString();
//                        T.aliquotaExternaIcms = doc.Recordset.Fields.Item("aliquotaExternaIcms").Value.ToString();
//                        T.reducaoBaseIcms = doc.Recordset.Fields.Item("reducaoBaseIcms").Value.ToString();
//                        T.ReduçãoBaseIpi = doc.Recordset.Fields.Item("ReduçãoBaseIpi").Value.ToString();
//                        T.IpiBaseIcms = doc.Recordset.Fields.Item("IpiBaseIcms").Value.ToString();
//                        T.substituicaoTributaria = doc.Recordset.Fields.Item("substituicaoTributaria").Value.ToString();
//                        T.stPorPauta = doc.Recordset.Fields.Item("stPorPauta").Value.ToString();
//                        T.ignorarDescontoIcms = doc.Recordset.Fields.Item("ignorarDescontoIcms").Value.ToString();
//                        T.aliquotaFCP = doc.Recordset.Fields.Item("aliquotaFCP").Value.ToString();



//                        list.Add(T);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<TributacaoModel>>(list);

//            }


//        }


//    }
//}