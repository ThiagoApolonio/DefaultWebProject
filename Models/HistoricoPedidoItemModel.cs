using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class HistoricoPedidoItemModel
    {
        public string NumeroPedidoERP { get; set; }
        public string CodigoProduto { get; set; }
        public string quantidadePedida { get; set; }
        public string quantidadeFaturada { get; set; }
        public string desconto { get; set; }
        public string valor { get; set; }
        public string observacoes { get; set; }
    }
}