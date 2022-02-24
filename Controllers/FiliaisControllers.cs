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
    public class FiliaisControllers : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetFilial")]
        [ResponseType(typeof(FiliaisModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFilial(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FiliaisModel> listFiliais = new List<FiliaisModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode AS idFiliais, DflBranch AS codigoFiliaisERP , validFor AS ativo, RD.AliasName AS razaoSocial FROM OCRD RD INNER JOIN OCRB RB ON RB.CardCode = RD.CardCode ");
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            FiliaisModel F = new FiliaisModel();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            F.codigoFiliaisERP = doc.Recordset.Fields.Item("codigoFiliaisERP").Value.ToString();
                            F.razaoSocial = doc.Recordset.Fields.Item("razaoSocial").Value.ToString();
                            F.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            listFiliais.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FiliaisModel>>(listFiliais);

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
        [System.Web.Http.Route("GetFilial(ID)")]
        [ResponseType(typeof(FiliaisModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetFilialID(string ID,string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<FiliaisModel> listFiliais = new List<FiliaisModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT RD.CardCode AS idFiliais, DflBranch AS codigoFiliaisERP , validFor AS ativo, RD.AliasName AS razaoSocial FROM OCRD RD INNER JOIN OCRB RB ON RB.CardCode = RD.CardCode WHERE RD.CardCode = '{0}' ", ID);
                    string queryHANA = ServerConnections.TranslateToHana(sql);
                    doc.Recordset.DoQuery(queryHANA);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            FiliaisModel F = new FiliaisModel();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            F.codigoFiliaisERP = doc.Recordset.Fields.Item("codigoFiliaisERP").Value.ToString();
                            F.razaoSocial = doc.Recordset.Fields.Item("razaoSocial").Value.ToString();
                            F.ativo = doc.Recordset.Fields.Item("ativo").Value.ToString();
                            listFiliais.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<FiliaisModel>>(listFiliais);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }


        }

    }
}