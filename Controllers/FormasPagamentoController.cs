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
    public class FormasPagamentoController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetFormasPagamento")]
        [ResponseType(typeof(FormasPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFormasPagamento(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FormasPagamentoModel> formalist = new List<FormasPagamentoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode as idFormasPagamento,PayMethCod as codigoFormasPagamento,YM.Descript as nomeFormasPagamento, YM.Active as ativo from OPYM YM Inner join  OCRD RD ON RD.PymCode = YM.PayMethCod");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            FormasPagamentoModel F = new FormasPagamentoModel();

                            F.idFormasPagamento = doc.Recordset.Fields.Item("idFormasPagamento").Value.ToString();
                            F.codigoFormasPagamento = doc.Recordset.Fields.Item("codigoFormasPagamento").Value.ToString();
                            F.nomeFormasPagamento = doc.Recordset.Fields.Item("nomeFormasPagamento").Value.ToString();
                            F.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            formalist.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FormasPagamentoModel>>(formalist);

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
        [System.Web.Http.Route("GetFormasPagamento(ID)")]
        [ResponseType(typeof(FormasPagamentoModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFormasPagamentoID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FormasPagamentoModel> formalist = new List<FormasPagamentoModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode as idFormasPagamento,PayMethCod as codigoFormasPagamento,YM.Descript as nomeFormasPagamento, YM.Active as ativo from OPYM YM Inner join  OCRD RD ON RD.PymCode = YM.PayMethCod WHERE RD.CardCode = '{0}'", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            FormasPagamentoModel F = new FormasPagamentoModel();
                            F.idFormasPagamento = doc.Recordset.Fields.Item("idFormasPagamento").Value.ToString();
                            F.codigoFormasPagamento = doc.Recordset.Fields.Item("codigoFormasPagamento").Value.ToString();
                            F.nomeFormasPagamento = doc.Recordset.Fields.Item("nomeFormasPagamento").Value.ToString();
                            F.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            formalist.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FormasPagamentoModel>>(formalist);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
        }
    }
}