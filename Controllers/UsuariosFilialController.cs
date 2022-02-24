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
//    public class UsuariosFilialController :ApiController
//    {
//        [System.Web.Http.Route("GetUsuariosFilial")]
//        [ResponseType(typeof(UsuariosFilialModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetUsuariosFilial()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<UsuariosFilialModel> list = new List<UsuariosFilialModel>();
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
//                        UsuariosFilialModel usuariosFilial = new UsuariosFilialModel();

//                        usuariosFilial.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
//                        usuariosFilial.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
       



//                        list.Add(usuariosFilial);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<UsuariosFilialModel>>(list);

//            }


//        }
//        [System.Web.Http.Route("GetUsuariosFilial(ID)")]
//        [ResponseType(typeof(UsuariosFilialModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetUsuariosFilialID(string ID)
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<UsuariosFilialModel> list = new List<UsuariosFilialModel>();
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
//                        UsuariosFilialModel usuariosFilial = new UsuariosFilialModel();

//                        usuariosFilial.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
//                        usuariosFilial.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
       



//                        list.Add(usuariosFilial);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<UsuariosFilialModel>>(list);

//            }


//        }


//    }
//}