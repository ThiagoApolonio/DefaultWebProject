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
    public class ProdutosController : ApiController
    { /// <summary>
      /// Busca Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
      /// </summary> 
      /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("GetProdutos")]
        [ResponseType(typeof(ProdutosModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetProdutos(string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ProdutosModel> list = new List<ProdutosModel>();
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
                            ProdutosModel P = new ProdutosModel();
                            P.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
                            P.codigoProduto = doc.Recordset.Fields.Item("codigoProduto").Value.ToString();
                            P.codigoEmbalagem = doc.Recordset.Fields.Item("codigoEmbalagem").Value.ToString();
                            P.codigoFabricante = doc.Recordset.Fields.Item("codigoFabricante").Value.ToString();
                            P.grupoGerencial = doc.Recordset.Fields.Item("grupoGerencial").Value.ToString();
                            P.categoria = doc.Recordset.Fields.Item("categoria").Value.ToString();
                            P.marca = doc.Recordset.Fields.Item("marca").Value.ToString();
                            P.sabor = doc.Recordset.Fields.Item("sabor").Value.ToString();
                            P.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            P.unidade = doc.Recordset.Fields.Item("unidade").Value.ToString();
                            P.pesoLiquido = doc.Recordset.Fields.Item("pesoLiquido").Value.ToString();
                            P.idFornecedores = doc.Recordset.Fields.Item("idFornecedores").Value.ToString();
                            P.pesoBruto = doc.Recordset.Fields.Item("pesoBruto").Value.ToString();
                            P.altura = doc.Recordset.Fields.Item("altura").Value.ToString();
                            P.largura = doc.Recordset.Fields.Item("largura").Value.ToString();
                            P.comprimento = doc.Recordset.Fields.Item("comprimento").Value.ToString();
                            P.unidadePorCaixa = doc.Recordset.Fields.Item("unidadePorCaixa").Value.ToString();
                            P.caixasCamada = doc.Recordset.Fields.Item("caixasCamada").Value.ToString();
                            P.caixasPallet = doc.Recordset.Fields.Item("caixasPallet").Value.ToString();
                            P.ean = doc.Recordset.Fields.Item("ean").Value.ToString();
                            P.dun = doc.Recordset.Fields.Item("dun").Value.ToString();
                            P.su = doc.Recordset.Fields.Item("su").Value.ToString();
                            P.multiploVenda = doc.Recordset.Fields.Item("multiploVenda").Value.ToString();
                            P.valorClassificacao = doc.Recordset.Fields.Item("valorClassificacao").Value.ToString();
                            P.percentualComissao = doc.Recordset.Fields.Item("percentualComissao").Value.ToString();
                            P.estoqueCritico = doc.Recordset.Fields.Item("estoqueCritico").Value.ToString();
                            P.ultimaAtualizacao = doc.Recordset.Fields.Item("ultimaAtualizacao").Value.ToString();
                            P.codigoGrupo = doc.Recordset.Fields.Item("codigoGrupo").Value.ToString();
                            P.pai = doc.Recordset.Fields.Item("pai").Value.ToString();
                            P.litros = doc.Recordset.Fields.Item("litros").Value.ToString();
                            list.Add(P);
                            doc.Recordset.MoveNext();
                        }
                    }
                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ProdutosModel>>(list);
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
        [System.Web.Http.Route("GetProdutos(ID)")]
        [ResponseType(typeof(ProdutosModel))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetProdutosID(string ID, string BaseId)
        {
            try
            {
                var comp = new CompaniaSap().ConectConfig(BaseId);
                List<ProdutosModel> list = new List<ProdutosModel>();
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
                            ProdutosModel P = new ProdutosModel();
                            P.idProdutos = doc.Recordset.Fields.Item("idProdutos").Value.ToString();
                            P.codigoProduto = doc.Recordset.Fields.Item("codigoProduto").Value.ToString();
                            P.codigoEmbalagem = doc.Recordset.Fields.Item("codigoEmbalagem").Value.ToString();
                            P.codigoFabricante = doc.Recordset.Fields.Item("codigoFabricante").Value.ToString();
                            P.grupoGerencial = doc.Recordset.Fields.Item("grupoGerencial").Value.ToString();
                            P.categoria = doc.Recordset.Fields.Item("categoria").Value.ToString();
                            P.marca = doc.Recordset.Fields.Item("marca").Value.ToString();
                            P.sabor = doc.Recordset.Fields.Item("sabor").Value.ToString();
                            P.descricao = doc.Recordset.Fields.Item("descricao").Value.ToString();
                            P.unidade = doc.Recordset.Fields.Item("unidade").Value.ToString();
                            P.pesoLiquido = doc.Recordset.Fields.Item("pesoLiquido").Value.ToString();
                            P.idFornecedores = doc.Recordset.Fields.Item("idFornecedores").Value.ToString();
                            P.pesoBruto = doc.Recordset.Fields.Item("pesoBruto").Value.ToString();
                            P.altura = doc.Recordset.Fields.Item("altura").Value.ToString();
                            P.largura = doc.Recordset.Fields.Item("largura").Value.ToString();
                            P.comprimento = doc.Recordset.Fields.Item("comprimento").Value.ToString();
                            P.unidadePorCaixa = doc.Recordset.Fields.Item("unidadePorCaixa").Value.ToString();
                            P.caixasCamada = doc.Recordset.Fields.Item("caixasCamada").Value.ToString();
                            P.caixasPallet = doc.Recordset.Fields.Item("caixasPallet").Value.ToString();
                            P.ean = doc.Recordset.Fields.Item("ean").Value.ToString();
                            P.dun = doc.Recordset.Fields.Item("dun").Value.ToString();
                            P.su = doc.Recordset.Fields.Item("su").Value.ToString();
                            P.multiploVenda = doc.Recordset.Fields.Item("multiploVenda").Value.ToString();
                            P.valorClassificacao = doc.Recordset.Fields.Item("valorClassificacao").Value.ToString();
                            P.percentualComissao = doc.Recordset.Fields.Item("percentualComissao").Value.ToString();
                            P.estoqueCritico = doc.Recordset.Fields.Item("estoqueCritico").Value.ToString();
                            P.ultimaAtualizacao = doc.Recordset.Fields.Item("ultimaAtualizacao").Value.ToString();
                            P.codigoGrupo = doc.Recordset.Fields.Item("codigoGrupo").Value.ToString();
                            P.pai = doc.Recordset.Fields.Item("pai").Value.ToString();
                            P.litros = doc.Recordset.Fields.Item("litros").Value.ToString();
                            list.Add(P);
                            doc.Recordset.MoveNext();
                        }
                    }

                    Marshal.ReleaseComObject(doc.Recordset);
                    doc.Recordset = null;
                    return Ok<List<ProdutosModel>>(list);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Erro :" + ex);
            }


        }
    }
}