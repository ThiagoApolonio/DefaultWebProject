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
    public class FabricanteController : ApiController
    {

        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetFabricante")]
        [ResponseType(typeof(FabricantesModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFabricante(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FabricantesModel> list = new List<FabricantesModel>();
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
                            FabricantesModel F = new FabricantesModel();
                            F.idFabricantes = doc.Recordset.Fields.Item("idFabricantes").Value.ToString();
                            F.codigoFabricante = doc.Recordset.Fields.Item("codigoFabricante").Value.ToString();
                            F.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FabricantesModel>>(list);
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
        [System.Web.Http.Route("GetFabricante(ID)")]
        [ResponseType(typeof(FabricantesModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFabricante(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FabricantesModel> list = new List<FabricantesModel>();
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
                            FabricantesModel F = new FabricantesModel();
                            F.idFabricantes = doc.Recordset.Fields.Item("idFabricantes").Value.ToString();
                            F.codigoFabricante = doc.Recordset.Fields.Item("codigoFabricante").Value.ToString();
                            F.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FabricantesModel>>(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }



        }
    }
}