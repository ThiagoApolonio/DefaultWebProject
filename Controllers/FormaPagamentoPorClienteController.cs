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
    public class FormaPagamentoPorClienteController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetFormaPagamentoPorCliente")]
        [ResponseType(typeof(FormaPagamentoPorClienteModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFormaPagamentoPorCliente(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FormaPagamentoPorClienteModel> list = new List<FormaPagamentoPorClienteModel>();
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
                            FormaPagamentoPorClienteModel F = new FormaPagamentoPorClienteModel();
                            F.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            F.idFormaPagamentoCliente = doc.Recordset.Fields.Item("idFormaPagamentoCliente").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }

                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FormaPagamentoPorClienteModel>>(list);

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
        [System.Web.Http.Route("GetFormaPagamentoPorCliente(ID)")]
        [ResponseType(typeof(FormaPagamentoPorClienteModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFormaPagamentoPorClienteID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FormaPagamentoPorClienteModel> list = new List<FormaPagamentoPorClienteModel>();
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
                            FormaPagamentoPorClienteModel F = new FormaPagamentoPorClienteModel();
                            F.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            F.idFormaPagamentoCliente = doc.Recordset.Fields.Item("idFormaPagamentoCliente").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FormaPagamentoPorClienteModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }



        }
    }
}