using DefaultWebProject.Conexao;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using SAPbobsCOM;
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
    public class PendenciasFinanceirasController :ApiController
    {

        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetPendenciasFinanceiras")]
        [ResponseType(typeof(PendenciasFinanceirasModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPendenciasFinanceiras(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<PendenciasFinanceirasModel> list = new List<PendenciasFinanceirasModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.DflBranch AS idFiliais, RD.CardCode AS idClientes, DR.DocNum AS idPedidoVenda, RD.SlpCode as idUsuarios, DR.DocNum AS numeroDocumento, DR.DocType AS tipoDocumento, OC.InstNum AS parcela FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode INNER JOIN RDR1 R1 ON R1.BaseCard = DR.CardCode INNER JOIN OCTG OC on oc.GroupNum = rd.GroupNum");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            PendenciasFinanceirasModel PF = new PendenciasFinanceirasModel();
                            PF.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            PF.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            PF.idPedidoVenda = doc.Recordset.Fields.Item("idPedidoVenda").Value.ToString();
                            PF.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
                            PF.lojaCliente = "INEXISTENTE NO SAP";
                            PF.tipoDocumento = doc.Recordset.Fields.Item("tipoDocumento").Value.ToString();
                            PF.numeroDocumento = doc.Recordset.Fields.Item("numeroDocumento").Value.ToString();
                            PF.parcela = doc.Recordset.Fields.Item("parcela").Value.ToString();
                            PF.dataEmissao = "    INEXISTENTE NO SAP";
                            PF.dataVencimento = " INEXISTENTE NO SAP";
                            PF.valorOriginal = "  INEXISTENTE NO SAP";
                            PF.valorAtual = "     INEXISTENTE NO SAP";
                            PF.valorJuros = "     INEXISTENTE NO SAP";
                            PF.valorPago = "      INEXISTENTE NO SAP";
                            PF.saldo = "          INEXISTENTE NO SAP";
                            PF.numeroDuplicata = "INEXISTENTE NO SAP";
                            list.Add(PF);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<PendenciasFinanceirasModel>>(list);

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
        [System.Web.Http.Route("GetPendenciasFinanceiras(ID)")]
        [ResponseType(typeof(PendenciasFinanceirasModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPendenciasFinanceirasID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<PendenciasFinanceirasModel> list = new List<PendenciasFinanceirasModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.DflBranch AS idFiliais, RD.CardCode AS idClientes, DR.DocNum AS idPedidoVenda, RD.SlpCode as idUsuarios, DR.DocNum AS numeroDocumento, DR.DocType AS tipoDocumento, OC.InstNum AS parcela FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode INNER JOIN RDR1 R1 ON R1.BaseCard = DR.CardCode INNER JOIN OCTG OC on oc.GroupNum = rd.GroupNum WHERE  DR.CardCode = {'0'}", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            PendenciasFinanceirasModel PF = new PendenciasFinanceirasModel();
                            PF.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            PF.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            PF.idPedidoVenda = doc.Recordset.Fields.Item("idPedidoVenda").Value.ToString();
                            PF.idUsuarios = doc.Recordset.Fields.Item("idUsuarios").Value.ToString();
                            PF.lojaCliente = doc.Recordset.Fields.Item("lojaCliente").Value.ToString();
                            PF.tipoDocumento = doc.Recordset.Fields.Item("tipoDocumento").Value.ToString();
                            PF.numeroDocumento = doc.Recordset.Fields.Item("numeroDocumento").Value.ToString();
                            PF.parcela = doc.Recordset.Fields.Item("parcela").Value.ToString();
                            PF.dataEmissao = "INEXISTENTE NO SAP";
                            PF.dataVencimento = "INEXISTENTE NO SAP";
                            PF.valorOriginal = "INEXISTENTE NO SAP";
                            PF.valorAtual = "INEXISTENTE NO SAP";
                            PF.valorJuros = "INEXISTENTE NO SAP";
                            PF.valorPago = "INEXISTENTE NO SAP";
                            PF.saldo = "INEXISTENTE NO SAP";
                            PF.numeroDuplicata = "INEXISTENTE NO SAP";
                            list.Add(PF);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<PendenciasFinanceirasModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }

        }        
    }
}