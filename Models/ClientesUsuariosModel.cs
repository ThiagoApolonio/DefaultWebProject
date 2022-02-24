using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
   
    public class ClientesUsuariosModel
    {
     
        public string idClientes { get; set; }
        public string idUsuarios { get; set; }
    }
}