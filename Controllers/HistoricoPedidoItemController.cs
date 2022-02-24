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
    public class HistoricoPedidoItemController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetHistoricoPedidoItem")]
        [ResponseType(typeof(HistoricoPedidoItemModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetHistoricoPedidoItem(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<HistoricoPedidoItemModel> list = new List<HistoricoPedidoItemModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT DR.DocNum AS NumeroPedidoERP,RD.CardCode AS CodigoProduto, DR.Volume  AS quantidadePedida, DR.Volume AS quantidadeFaturada, DR.DiscSumFC AS desconto, DR.DocTotalFC  AS valor, DR.Comments AS observacoes FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            HistoricoPedidoItemModel HP = new HistoricoPedidoItemModel();
                            HP.NumeroPedidoERP = doc.Recordset.Fields.Item("NumeroPedidoERP").Value.ToString();
                            HP.CodigoProduto = doc.Recordset.Fields.Item("CodigoProduto").Value.ToString();
                            HP.quantidadePedida = doc.Recordset.Fields.Item("quantidadePedida").Value.ToString();
                            HP.quantidadeFaturada = doc.Recordset.Fields.Item("quantidadeFaturada").Value.ToString();
                            HP.desconto = doc.Recordset.Fields.Item("desconto").Value.ToString();
                            HP.valor = doc.Recordset.Fields.Item("valor").Value.ToString();
                            HP.observacoes = doc.Recordset.Fields.Item("observacoes").Value.ToString();
                            list.Add(HP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<HistoricoPedidoItemModel>>(list);
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
        [System.Web.Http.Route("GetHistoricoPedidoItem(ID)")]
        [ResponseType(typeof(HistoricoPedidoItemModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetHistoricoPedidoItemID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<HistoricoPedidoItemModel> list = new List<HistoricoPedidoItemModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT DR.DocNum AS NumeroPedidoERP,RD.CardCode AS CodigoProduto, DR.Volume  AS quantidadePedida, DR.Volume AS quantidadeFaturada, DR.DiscSumFC AS desconto, DR.DocTotalFC  AS valor, DR.Comments AS observacoes FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode WHERE RD.CardCode = '{0}'", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            HistoricoPedidoItemModel HP = new HistoricoPedidoItemModel();
                            HP.NumeroPedidoERP = doc.Recordset.Fields.Item("NumeroPedidoERP").Value.ToString();
                            HP.CodigoProduto = doc.Recordset.Fields.Item("CodigoProduto").Value.ToString();
                            HP.quantidadePedida = doc.Recordset.Fields.Item("quantidadePedida").Value.ToString();
                            HP.quantidadeFaturada = doc.Recordset.Fields.Item("quantidadeFaturada").Value.ToString();
                            HP.desconto = doc.Recordset.Fields.Item("desconto").Value.ToString();
                            HP.valor = doc.Recordset.Fields.Item("valor").Value.ToString();
                            HP.observacoes = doc.Recordset.Fields.Item("observacoes").Value.ToString();
                            list.Add(HP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<HistoricoPedidoItemModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }     
        }
    }
}