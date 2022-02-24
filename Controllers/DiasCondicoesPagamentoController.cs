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
    public class DiasCondicoesPagamentoController :ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]       
        [System.Web.Http.Route("GetDiasCondicoesPagamento")]
        [ResponseType(typeof(DiasCondicoesPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDiasCondicoesPagamento(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<DiasCondicoesPagamentoModel> list = new List<DiasCondicoesPagamentoModel>();
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
                            DiasCondicoesPagamentoModel DCP = new DiasCondicoesPagamentoModel();
                            DCP.idCondicoesPagamento = doc.Recordset.Fields.Item("idCondicoesPagamento").Value.ToString();
                            DCP.parcela = doc.Recordset.Fields.Item("parcela").Value.ToString();
                            DCP.dias = doc.Recordset.Fields.Item("dias").Value.ToString();
                            list.Add(DCP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<DiasCondicoesPagamentoModel>>(list);

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
        [System.Web.Http.Route("GetDiasCondicoesPagamento(ID)")]
        [ResponseType(typeof(DiasCondicoesPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDiasCondicoesPagamentoID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<DiasCondicoesPagamentoModel> list = new List<DiasCondicoesPagamentoModel>();
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
                            DiasCondicoesPagamentoModel DCP = new DiasCondicoesPagamentoModel();
                            DCP.idCondicoesPagamento = doc.Recordset.Fields.Item("idCondicoesPagamento").Value.ToString();
                            DCP.parcela = doc.Recordset.Fields.Item("parcela").Value.ToString();
                            DCP.dias = doc.Recordset.Fields.Item("dias").Value.ToString();
                            list.Add(DCP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<DiasCondicoesPagamentoModel>>(list);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
        }
    }
}