using DefaultWebProject.Conexao;
using DefaultWebProject.Enum;
using DefaultWebProject.Log;
using DefaultWebProject.Models;
using DefaultWebProject.Tokken;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;

namespace DefaultWebProject.Controllers
{


    public class HistoricoPedidoStatusController : ApiController
    {


        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetHistoricoPedidoStatus")]
        [ResponseType(typeof(HistoricoPedidoStatusModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetHistoricoPedidoStatus(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<HistoricoPedidoStatusModel> list = new List<HistoricoPedidoStatusModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode AS idHistoricoPedidoStatus,DR.DocNum AS codigoStatus, R1.Dscription  AS descricao, DR.DocStatus as Stattus, DR.CANCELED  AS statusCancelado FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode INNER JOIN RDR1 R1 ON R1.BaseCard = DR.CardCode");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            HistoricoPedidoStatusModel HP = new HistoricoPedidoStatusModel();
                            var Status = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                            HP.idHistoricoPedidoStatus = doc.Recordset.Fields.Item("idHistoricoPedidoStatus").Value.ToString();
                            HP.codigoStatus = doc.Recordset.Fields.Item("codigoStatus").Value.ToString();
                            HP.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();

                            switch (Status)
                            {
                                case "O":
                                    HP.statusNormal = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                                    break;
                                case "C":
                                    HP.statusFinalizado = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                                    break;
                                default:
                                    break;
                            }
                            HP.statusCancelado = doc.Recordset.Fields.Item("statusCancelado").Value.ToString();
                            HP.statusRequerAtencao = "Campo Inexistente Sap";
                            list.Add(HP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<HistoricoPedidoStatusModel>>(list);
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
        [System.Web.Http.Route("GetHistoricoPedidoStatus(ID)")]
        [ResponseType(typeof(HistoricoPedidoStatusModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetHistoricoPedidoStatusID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<HistoricoPedidoStatusModel> list = new List<HistoricoPedidoStatusModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode AS idHistoricoPedidoStatus,DR.DocNum AS codigoStatus, R1.Dscription  AS descricao, DR.DocStatus as Stattus, DR.CANCELED  AS statusCancelado FROM ORDR DR INNER JOIN OCRD RD ON RD.CardCode = DR.CardCode INNER JOIN RDR1 R1 ON R1.BaseCard = DR.CardCode Where RD.CardCode = '{0}'", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            HistoricoPedidoStatusModel HP = new HistoricoPedidoStatusModel();
                            var Status = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                            HP.idHistoricoPedidoStatus = doc.Recordset.Fields.Item("idHistoricoPedidoStatus").Value.ToString();
                            HP.codigoStatus = doc.Recordset.Fields.Item("codigoStatus").Value.ToString();
                            HP.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            switch (Status)
                            {
                                case "O":
                                    HP.statusNormal = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                                    break;
                                case "C":
                                    HP.statusFinalizado = doc.Recordset.Fields.Item("Stattus").Value.ToString();
                                    break;
                                default:
                                    break;
                            }
                            HP.statusCancelado = doc.Recordset.Fields.Item("statusCancelado").Value.ToString();
                            HP.statusRequerAtencao = "Campo Inexistente Sap";
                            list.Add(HP);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<HistoricoPedidoStatusModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
        }       
    }
}