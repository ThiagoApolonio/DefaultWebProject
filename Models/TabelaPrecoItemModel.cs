using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class TabelaPrecoItemModel
    {
        public string idTabelaPreco { get; set; }
        public string idProdutos { get; set; }
        public string quantidade { get; set; }
        public string preco { get; set; }
        public string indiceFinanceiroOpcional { get; set; }
    }
}