using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DefaultWebProject.Models
{
    public class CondicoesPagamentoModel 
    {
        public string idCondicoesPagamento { get; set; }
        public string codigoCondicoesPagamento { get; set; }
        public string nomeCondicoesPagamento { get; set; }
        public string qtdParcelas { get; set; }
        public string indiceFinanceiro { get; set; }
        public string valorMinimoParcela { get; set; }
        public string ativo { get; set; }
     
    }
}