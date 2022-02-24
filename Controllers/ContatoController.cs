using DefaultWebProject.Conexao;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace DefaultWebProject.Controllers
{
    public class ContatoController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetContato")]
        [ResponseType(typeof(ContatoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetContato(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ContatoModel> list = new List<ContatoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ContatoModel C = new ContatoModel();
                            C.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            C.codigoContato = doc.Recordset.Fields.Item("codigoContato").Value.ToString();
                            C.nome = doc.Recordset.Fields.Item("nome").Value.ToString();
                            C.aniversario = doc.Recordset.Fields.Item("aniversario").Value.ToString();
                            C.hoby = doc.Recordset.Fields.Item("hoby").Value.ToString();
                            C.clube = doc.Recordset.Fields.Item("clube").Value.ToString();
                            C.email = doc.Recordset.Fields.Item("email").Value.ToString();
                            C.celular = doc.Recordset.Fields.Item("celular").Value.ToString();
                            C.telefoneComercial = doc.Recordset.Fields.Item("telefoneComercial").Value.ToString();
                            C.telefoneResidencial = doc.Recordset.Fields.Item("telefoneResidencial").Value.ToString();
                            C.departamento = doc.Recordset.Fields.Item("departamento").Value.ToString();
                            list.Add(C);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ContatoModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
           
        }
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetContato(ID)")]
        [ResponseType(typeof(ContatoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetContatoID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ContatoModel> list = new List<ContatoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ContatoModel C = new ContatoModel();
                            C.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            C.codigoContato = doc.Recordset.Fields.Item("codigoContato").Value.ToString();
                            C.nome = doc.Recordset.Fields.Item("nome").Value.ToString();
                            C.aniversario = doc.Recordset.Fields.Item("aniversario").Value.ToString();
                            C.hoby = doc.Recordset.Fields.Item("hoby").Value.ToString();
                            C.clube = doc.Recordset.Fields.Item("clube").Value.ToString();
                            C.email = doc.Recordset.Fields.Item("email").Value.ToString();
                            C.celular = doc.Recordset.Fields.Item("celular").Value.ToString();
                            C.telefoneComercial = doc.Recordset.Fields.Item("telefoneComercial").Value.ToString();
                            C.telefoneResidencial = doc.Recordset.Fields.Item("telefoneResidencial").Value.ToString();
                            C.departamento = doc.Recordset.Fields.Item("departamento").Value.ToString();
                            list.Add(C);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ContatoModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }    
        }
    }
}