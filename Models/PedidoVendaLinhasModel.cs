using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class PedidoVendaLinhasModel
    {
        public string ItemCode { get; set; }
        public string Quantidade { get; set; }
        public string Preco { get; set; }
        public string CentroCusto { get; set; }
        public string Projeto { get; set; }
    }
}