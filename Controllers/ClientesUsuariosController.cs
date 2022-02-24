using DefaultWebProject.Conexao;
using DefaultWebProject.Context;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;


namespace DefaultWebProject.Controllers
{
    public class ClientesUsuariosController : ApiController
    {

        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetCliente")]
        [ResponseType(typeof(ClientesUsuariosModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCliente(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ClientesUsuariosModel> clientesUsuarios = new List<ClientesUsuariosModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select CardCode as IdCliente,SlpCode as idUsuarios from OCRD");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ClientesUsuariosModel c = new ClientesUsuariosModel();
                            c.idClientes = doc.Recordset.Fields.Item("IdCliente").Value.ToString();
                            c.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
                            clientesUsuarios.Add(c);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ClientesUsuariosModel>>(clientesUsuarios);
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
        [System.Web.Http.Route("GetCliente(ID)")]
        [ResponseType(typeof(ClientesUsuariosModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetClienteID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ClientesUsuariosModel> clientesUsuarios = new List<ClientesUsuariosModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select CardCode as IdCliente,SlpCode as idUsuarios from OCRD WHERE CardCode ='{0}' ", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ClientesUsuariosModel c = new ClientesUsuariosModel();
                            c.idClientes = doc.Recordset.Fields.Item("IdCliente").Value.ToString();
                            c.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
                            clientesUsuarios.Add(c);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ClientesUsuariosModel>>(clientesUsuarios);

                }

            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
        }
    }
}