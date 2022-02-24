using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class ContatoModel
    {
        public string idClientes { get; set; }
        public string codigoContato { get; set; }
        public string nome { get; set; }
        public string aniversario { get; set; }
        public string hoby { get; set; }
        public string clube { get; set; }
        public string email { get; set; }
        public string celular { get; set; }
        public string telefoneComercial { get; set; }
        public string telefoneResidencial { get; set; }
        public string departamento { get; set; }
    }
}