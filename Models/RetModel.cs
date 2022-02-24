using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class RetModel
    {
        public RetModel() { }
        public string Campo { get; set; }
        public string Valor { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public bool ValorIncorreto { get; set; }
        public string MsgErro { get; set; } = "";

    }
}