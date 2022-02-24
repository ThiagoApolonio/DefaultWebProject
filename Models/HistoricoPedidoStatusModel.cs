using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class HistoricoPedidoStatusModel
    {
        public string idHistoricoPedidoStatus                    { get; set; }
        public string codigoStatus                              { get; set; }
        public string descricao                                 { get; set; }
        public string statusFinalizado                              { get; set; }
        public string statusNormal                                  { get; set; }
        public string statusCancelado                                               { get; set; }
        public string statusRequerAtencao                                       { get; set; }
    }
}