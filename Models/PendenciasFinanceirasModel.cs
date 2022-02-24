using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class PendenciasFinanceirasModel
    {
        public string idFiliais { get; set;}
        public string idClientes { get; set;}
        public string idPedidoVenda { get; set;}
        public string idUsuarios { get; set;}
        public string lojaCliente { get; set;}
        public string tipoDocumento { get; set;}
        public string numeroDocumento { get; set;}
        public string parcela { get; set;}
        public string dataEmissao { get; set;}
        public string dataVencimento { get; set;}
        public string valorOriginal { get; set;}
        public string valorAtual { get; set;}
        public string valorJuros { get; set;}
        public string valordesconto  { get; set;}
        public string valorPago { get; set;}
        public string saldo { get; set;}
        public string numeroDuplicata { get; set;}
   
    }
}