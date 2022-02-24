using DefaultWebProject.Conexao;
using DefaultWebProject.Context;
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
    public class CondicoesController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetCondicoesPagamento")]
        [ResponseType(typeof(CondicoesPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCondicoesPagamento(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<CondicoesPagamentoModel> condicao = new List<CondicoesPagamentoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select rd.CardCode as idCondicoesPagamento , oc.GroupNum as codigoCondicoesPagamento , oc.PymntGroup  as nomeCondicoesPagamento, oc.InstNum as qtdParcelas from OCRD rd inner join OCTG oc on oc.GroupNum = rd.GroupNum");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            CondicoesPagamentoModel c = new CondicoesPagamentoModel();
                            c.idCondicoesPagamento = doc.Recordset.Fields.Item("idCondicoesPagamento").Value.ToString();
                            c.codigoCondicoesPagamento = doc.Recordset.Fields.Item("codigoCondicoesPagamento").Value.ToString();
                            c.nomeCondicoesPagamento = doc.Recordset.Fields.Item("nomeCondicoesPagamento").Value.ToString();
                            c.qtdParcelas = doc.Recordset.Fields.Item("qtdParcelas").Value.ToString();
                            condicao.Add(c);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<CondicoesPagamentoModel>>(condicao);
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
        [System.Web.Http.Route("GetCondicoesPagamento(ID)")]
        [ResponseType(typeof(CondicoesPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCondicoesPagamentoID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<CondicoesPagamentoModel> condicao = new List<CondicoesPagamentoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select rd.CardCode as idCondicoesPagamento , oc.GroupNum as codigoCondicoesPagamento , oc.PymntGroup  as nomeCondicoesPagamento, oc.InstNum as qtdParcelas from OCRD rd inner join OCTG oc on oc.GroupNum = rd.GroupNum WHERE rd.CardCode = '{0}' ", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            CondicoesPagamentoModel c = new CondicoesPagamentoModel();
                            c.idCondicoesPagamento = doc.Recordset.Fields.Item("idCondicoesPagamento").Value.ToString();
                            c.codigoCondicoesPagamento = doc.Recordset.Fields.Item("codigoCondicoesPagamento").Value.ToString();
                            c.nomeCondicoesPagamento = doc.Recordset.Fields.Item("nomeCondicoesPagamento").Value.ToString();
                            c.qtdParcelas = doc.Recordset.Fields.Item("qtdParcelas").Value.ToString();
                            condicao.Add(c);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<CondicoesPagamentoModel>>(condicao);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }       
        }

    }
}