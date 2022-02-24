using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class FiliaisModel
    {
        [Key]
        public string idFiliais { get; set; }
        public string codigoFiliaisERP { get; set; }
        public string razaoSocial { get; set; }
        public string ativo { get; set; }
    }
}