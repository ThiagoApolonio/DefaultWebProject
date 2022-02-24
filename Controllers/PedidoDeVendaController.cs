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
    public class PedidoDeVendaController : ApiController
    {

        /// <summary>
        /// Adcionar Informações Do E-commerce Sap Selecione o Banco de Dados Desejado Insira o Valor De <BaseId>?</BaseId> Contida No Documento SAPCredetials.xml
        /// </summary> 
        /// <returns></returns> 
        [EnableQuery]
        [System.Web.Http.Route("AdicionarPedidoDeVenda(ID)")]
        [ResponseType(typeof(ClientesUsuariosModel))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AdicionarPedidoDeVenda(List<PedidoDeVendaModel> oPedidoDeVenda, string BaseId)
        {
            foreach (var itemPedido in oPedidoDeVenda)
            {
                if (!string.IsNullOrEmpty(itemPedido.CardCode))
                {
                    var comp = new CompaniaSap().ConectConfig(BaseId);               
                    using (var doc = new InstanciaSap(comp.Company))
                    {
                        {
                            doc.Orders.CardCode = itemPedido.CardCode;
                            try
                            {
                                if (!string.IsNullOrEmpty(itemPedido.Filial))
                                    doc.Orders.BPL_IDAssignedToInvoice = int.Parse(itemPedido.Filial);
                                doc.Orders.DocDate = itemPedido.DataLancamento;
                                doc.Orders.TaxDate = itemPedido.DataDocumento;
                                doc.Orders.DocDueDate = itemPedido.DataCompetencia;
                            }
                            catch
                            {
                                doc.Orders.DocDate = DateTime.Now;
                                doc.Orders.TaxDate = DateTime.Now;
                                doc.Orders.DocDueDate = DateTime.Now.AddDays(2);
                            }
                            doc.Orders.GroupNumber = itemPedido.CondicaoPagamento;
                            doc.Orders.PaymentMethod = itemPedido.FormaPagamento;
                            doc.Orders.JournalMemo = "Pedido gerado pela integração";
                            doc.Orders.Comments = "Pedido gerado pela integração.";
                            for (int i = 0; i < itemPedido.PedidoVendaLinhas.Count; i++)
                            {
                                if (i > 0)
                                {
                                    doc.Orders.Lines.Add();
                                }
                                doc.Orders.Lines.SetCurrentLine(i);
                                doc.Orders.Lines.ItemCode = itemPedido.PedidoVendaLinhas[i].ItemCode;
                                doc.Orders.Lines.Quantity = Convert.ToDouble(itemPedido.PedidoVendaLinhas[i].Quantidade);
                                doc.Orders.Lines.UnitPrice = Convert.ToDouble(itemPedido.PedidoVendaLinhas[i].Preco);
                                if (!string.IsNullOrEmpty(itemPedido.Utilizacao))
                                {
                                    doc.Orders.Lines.Usage = itemPedido.Utilizacao;
                                }
                                doc.Orders.Lines.CostingCode = itemPedido.PedidoVendaLinhas[i].CentroCusto;
                                doc.Orders.Lines.ProjectCode = itemPedido.PedidoVendaLinhas[i].Projeto;
                            }

                            var errCode = doc.Orders.Add();
                            if (errCode ==0)
                            {
                                return Ok();
                            }                         
                        }
                    }
                }
            }
            return BadRequest();
        }
    }
}