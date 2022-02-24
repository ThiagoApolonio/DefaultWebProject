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
    public class ClientesporFilialController : ApiController
    {
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetClientesporFilial")]
        [ResponseType(typeof(ClientesporFilialModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetClientesporFilial(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ClientesporFilialModel> list = new List<ClientesporFilialModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT DflBranch AS idFiliais, CardCode AS idClientes from OCRD ");
                    doc.Recordset.DoQuery(sql);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ClientesporFilialModel F = new ClientesporFilialModel();
                            F.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ClientesporFilialModel>>(list);

                }
            }
            catch (Exception)
            {

                throw;
            }          
        }
        /// <summary>
        /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetClientesporFilial(ID)")]
        [ResponseType(typeof(ClientesporFilialModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetClientesporFilial(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ClientesporFilialModel> list = new List<ClientesporFilialModel>();
                using (var doc = new InstanciaSap(comp.Company))
                {
                    comp.Company.Connect();
                    string sql = String.Format("SELECT DflBranch AS idFiliais, CardCode AS idClientes from OCRD WHERE CardCode ='{0} ", ID);
                    doc.Recordset.DoQuery(sql);
                    if (doc.Recordset.RecordCount > 0)
                    {
                        doc.Recordset.MoveFirst();
                        for (int i = 0; i < doc.Recordset.RecordCount; i++)
                        {
                            ClientesporFilialModel F = new ClientesporFilialModel();
                            F.idClientes = doc.Recordset.Fields.Item("idClientes").Value.ToString();
                            F.idFiliais = doc.Recordset.Fields.Item("idFiliais").Value.ToString();
                            list.Add(F);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ClientesporFilialModel>>(list);

                }
            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }           
        }
    }
}