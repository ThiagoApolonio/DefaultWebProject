using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class PedidoTipoModel
    {
        public string idPedidoTipo { get; set; }
        public string codigoTipoPedido { get; set; }
        public string descricao { get; set; }
        public string ativo { get; set; }
        public string valorPedidoMinimo { get; set; }
        public string valorPedidoMaximo { get; set; }
        public string bloquearPedidoMinimo { get; set; }
        public string bloquearPedidoMaximo { get; set; }
        public string indiceFinanceiroOpcional { get; set; }
        public string indiceFinanceiroAutomatico { get; set; }
        public string validarVerba { get; set; }

    }
}