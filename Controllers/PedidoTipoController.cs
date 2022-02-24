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
    public class PedidoTipoController : ApiController
    {  /// <summary>
       /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
       /// </summary> 
       /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetPedidoTipo")]
        [ResponseType(typeof(PedidoTipoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPedidoTipo(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<PedidoTipoModel> list = new List<PedidoTipoModel>();
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
                            PedidoTipoModel P = new PedidoTipoModel();
                            P.idPedidoTipo = doc.Recordset.Fields.Item("idPedidoTipo").Value.ToString();
                            P.codigoTipoPedido = doc.Recordset.Fields.Item("codigoTipoPedido").Value.ToString();
                            P.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            P.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            P.valorPedidoMinimo = doc.Recordset.Fields.Item("valorPedidoMinimo").Value.ToString();
                            P.valorPedidoMaximo = doc.Recordset.Fields.Item("valorPedidoMaximo").Value.ToString();
                            P.bloquearPedidoMinimo = doc.Recordset.Fields.Item("bloquearPedidoMinimo").Value.ToString();
                            P.bloquearPedidoMaximo = doc.Recordset.Fields.Item("bloquearPedidoMaximo").Value.ToString();
                            P.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("indiceFinanceiroOpcional").Value.ToString();
                            P.indiceFinanceiroAutomatico = doc.Recordset.Fields.Item("indiceFinanceiroAutomatico").Value.ToString();
                            P.validarVerba = doc.Recordset.Fields.Item("validarVerba").Value.ToString();
                            list.Add(P);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<PedidoTipoModel>>(list);
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
        [System.Web.Http.Route("GetPedidoTipo(ID)")]
        [ResponseType(typeof(PedidoTipoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetPedidoTipoID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<PedidoTipoModel> list = new List<PedidoTipoModel>();
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
                            PedidoTipoModel P = new PedidoTipoModel();
                            P.idPedidoTipo = doc.Recordset.Fields.Item("idPedidoTipo").Value.ToString();
                            P.codigoTipoPedido = doc.Recordset.Fields.Item("codigoTipoPedido").Value.ToString();
                            P.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            P.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            P.valorPedidoMinimo = doc.Recordset.Fields.Item("valorPedidoMinimo").Value.ToString();
                            P.valorPedidoMaximo = doc.Recordset.Fields.Item("valorPedidoMaximo").Value.ToString();
                            P.bloquearPedidoMinimo = doc.Recordset.Fields.Item("bloquearPedidoMinimo").Value.ToString();
                            P.bloquearPedidoMaximo = doc.Recordset.Fields.Item("bloquearPedidoMaximo").Value.ToString();
                            P.indiceFinanceiroOpcional = doc.Recordset.Fields.Item("indiceFinanceiroOpcional").Value.ToString();
                            P.indiceFinanceiroAutomatico = doc.Recordset.Fields.Item("indiceFinanceiroAutomatico").Value.ToString();
                            P.validarVerba = doc.Recordset.Fields.Item("validarVerba").Value.ToString();
                            list.Add(P);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<PedidoTipoModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }

        }
    }
}