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
    public class DescontoClienteController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetDescontoCliente")]
        [ResponseType(typeof(DescontoClienteModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDescontoCliente(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<DescontoClienteModel> list = new List<DescontoClienteModel>();
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
                            DescontoClienteModel D = new DescontoClienteModel();
                            D.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            D.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
                            D.idListasPrecos = doc.Recordset.Fields.Item("idListasPrecos").Value.ToString();
                            D.desconto = doc.Recordset.Fields.Item("desconto").Value.ToString();
                            D.dataInicial = doc.Recordset.Fields.Item("dataInicial").Value.ToString();
                            list.Add(D);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<DescontoClienteModel>>(list);

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
        [System.Web.Http.Route("GetDescontoCliente(ID)")]
        [ResponseType(typeof(DescontoClienteModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDescontoClienteID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<DescontoClienteModel> list = new List<DescontoClienteModel>();
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
                            DescontoClienteModel D = new DescontoClienteModel();
                            D.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            D.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
                            D.idListasPrecos = doc.Recordset.Fields.Item("idListasPrecos").Value.ToString();
                            D.desconto = doc.Recordset.Fields.Item("desconto").Value.ToString();
                            D.dataInicial = doc.Recordset.Fields.Item("dataInicial").Value.ToString();
                            list.Add(D);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<DescontoClienteModel>>(list);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }

        }
    }
}