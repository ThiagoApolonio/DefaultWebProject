using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DefaultWebProject.Models
{
    [Table("OITM")]
    public class ItemModel
    {
        [Key]
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Itemtype { get; set; }
      
    }
}