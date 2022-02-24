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
    public class CategoriaMarcaController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetCategoriaMarca")]
        [ResponseType(typeof(CategoriaMarcaModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCategoriaMarca(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<CategoriaMarcaModel> list = new List<CategoriaMarcaModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select CardCode as categoria, SlpCode as marca from OCRD");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            CategoriaMarcaModel C = new CategoriaMarcaModel();
                            C.categoria = doc.Recordset.Fields.Item("categoria").Value.ToString();
                            C.marca = doc.Recordset.Fields.Item("marca").Value.ToString();
                            list.Add(C);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<CategoriaMarcaModel>>(list);

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
        [System.Web.Http.Route("GetCategoriaMarca(ID)")]
        [ResponseType(typeof(CategoriaMarcaModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetCategoriaMarcaID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<CategoriaMarcaModel> list = new List<CategoriaMarcaModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("select CardCode as categoria, SlpCode as marca from OCRD WHERE CardCode = '{0}' ", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            CategoriaMarcaModel C = new CategoriaMarcaModel();
                            C.categoria = doc.Recordset.Fields.Item("categoria").Value.ToString();
                            C.marca = doc.Recordset.Fields.Item("marca").Value.ToString();
                            list.Add(C);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<CategoriaMarcaModel>>(list);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }
        }
    }
}