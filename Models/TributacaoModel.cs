using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class TributacaoModel
    {
        public string idFiliais { get; set; }
        public string idClientes { get; set; }
        public string idProdutos { get; set; }
        public string idPedidoTipo { get; set; }
        public string codigoImposto { get; set; }
        public string ipi { get; set; }
        public string iva { get; set; }
        public string aliquotaInternaIcms { get; set; }
        public string aliquotaExternaIcms { get; set; }
        public string reducaoBaseIcms { get; set; }
        public string ReduçãoBaseIpi { get; set; }
        public string IpiBaseIcms { get; set; }
        public string substituicaoTributaria { get; set; }
        public string stPorPauta { get; set; }
        public string ignorarDescontoIcms { get; set; }
        public string aliquotaFCP { get; set; }
    }
}