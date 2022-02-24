using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class TabelaPrecoModel
    {
        public string idTabelaPreco { get; set; }
        public string codigoTabela { get; set; }
        public string codigoTabelaGrupo { get; set; }
        public string descricao { get; set; }
        public string validadeInicial { get; set; }
        public string validadeFinal { get; set; }
        public string indiceFinanceiroOpcional { get; set; }
        public string indiceFinanceiroAutomatico { get; set; }
        public string indiceFinanceiro { get; set; }
        public string ativo { get; set; }
      
    }
}