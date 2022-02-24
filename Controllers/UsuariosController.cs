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
//    public class UsuariosController : ApiController
//    {
//        [System.Web.Http.Route("GetUsuarios")]
//        [ResponseType(typeof(UsuariosModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetUsuarios()
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<UsuariosModel> list = new List<UsuariosModel>();
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
//                        UsuariosModel U = new UsuariosModel();

//                        U.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
//                        U.CodigoUsuario = doc.Recordset.Fields.Item("CodigoUsuario").Value.ToString();
//                        U.Nome = doc.Recordset.Fields.Item("Nome").Value.ToString();
//                        U.Login = doc.Recordset.Fields.Item("Login").Value.ToString();
//                        U.Senha = doc.Recordset.Fields.Item("Senha").Value.ToString();
//                        U.Telefone = doc.Recordset.Fields.Item("Telefone").Value.ToString();
//                        U.Email = doc.Recordset.Fields.Item("Email").Value.ToString();
//                        U.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();



//                        list.Add(U);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<UsuariosModel>>(list);

//            }


//        }
//        [System.Web.Http.Route("GetUsuarios(ID)")]
//        [ResponseType(typeof(UsuariosModel))]
//        [System.Web.Http.HttpGet]
//        public IHttpActionResult GetUsuariosID(string ID)
//        {
//            PersonalAuthentication credential = new PersonalAuthentication();
//            var xml = new ConfigXmlDocument();
//            xml.Load($@"{AppDomain.CurrentDomain.BaseDirectory}SAPcredentials.xml");
//            credential.Username = xml.GetElementsByTagName("usuario_sap").Item(0).InnerXml.ToString();
//            credential.Password = xml.GetElementsByTagName("senha_sap").Item(0).InnerXml.ToString();
//            var (validacao, empresa) = new ValidarToken().CredencialValida(credential);
//            CompanyList.ConexaoInicial(empresa);
//            var comp = CompanyList.SAPCompanies.companyLists.Where(x => x.DataBase == "SBO_Teste").FirstOrDefault();
//            List<UsuariosModel> list = new List<UsuariosModel>();
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
//                        UsuariosModel U = new UsuariosModel();

//                        U.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
//                        U.CodigoUsuario = doc.Recordset.Fields.Item("CodigoUsuario").Value.ToString();
//                        U.Nome = doc.Recordset.Fields.Item("Nome").Value.ToString();
//                        U.Login = doc.Recordset.Fields.Item("Login").Value.ToString();
//                        U.Senha = doc.Recordset.Fields.Item("Senha").Value.ToString();
//                        U.Telefone = doc.Recordset.Fields.Item("Telefone").Value.ToString();
//                        U.Email = doc.Recordset.Fields.Item("Email").Value.ToString();
//                        U.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();



//                        list.Add(U);


//                        doc.Recordset.MoveNext();
//                    }
//                }

//                Marshal.ReleaseComObject(doc.Recordset);
//                doc.Recordset = null;

//                return Ok<List<UsuariosModel>>(list);

//            }


//        }

//    }
//}