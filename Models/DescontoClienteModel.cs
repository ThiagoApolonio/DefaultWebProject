using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class DescontoClienteModel
    {
        public string idClientes { get; set; }
        public string idProdutos { get; set; }
        public string idListasPrecos { get; set; }
        public string desconto { get; set; }
        public string dataInicial { get; set; }
    }
}