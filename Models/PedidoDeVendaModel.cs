using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class PedidoDeVendaModel
    {
        public int IdLinha { get; set; }
        public string Filial { get; set; }
        public string CardCode { get; set; }
        public int CondicaoPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string Utilizacao { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataCompetencia { get; set; }
        public bool Processado { get; set; }
        public List<PedidoVendaLinhasModel> PedidoVendaLinhas { get; set; }
    }
}