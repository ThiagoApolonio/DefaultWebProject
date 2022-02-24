using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    public class AddLcmModel
    {
        public List<AddLcmModel> AddLcm = new List<AddLcmModel>();
        public string BANCO { get; set; }
        public string ID_FILIAL { get; set; }
        public string HEADER { get; set; }
        public string LINES { get; set; }
      

    }
}