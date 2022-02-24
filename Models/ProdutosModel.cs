using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class ProdutosModel
    {
        public string idProdutos { get; set; }
        public string codigoProduto { get; set; }
        public string codigoEmbalagem { get; set; }
        public string codigoFabricante { get; set; }
        public string grupoGerencial { get; set; }
        public string categoria { get; set; }
        public string marca { get; set; }
        public string sabor { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public string pesoLiquido { get; set; }
        public string idFornecedores { get; set; }
        public string pesoBruto { get; set; }
        public string altura { get; set; }
        public string largura { get; set; }
        public string comprimento { get; set; }
        public string unidadePorCaixa { get; set; }
        public string caixasCamada { get; set; }
        public string caixasPallet { get; set; }
        public string ean { get; set; }
        public string dun { get; set; }
        public string su { get; set; }
        public string multiploVenda { get; set; }
        public string valorClassificacao { get; set; }
        public string percentualComissao { get; set; }
        public string estoqueCritico { get; set; }
        public string ultimaAtualizacao { get; set; }
        public string codigoGrupo { get; set; }
        public string pai { get; set; }
        public string litros { get; set; }
        public string ativo { get; set; }
    }
}