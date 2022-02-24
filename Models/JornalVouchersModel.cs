using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Models
{
    [Table("OJDT")]
    public class JornalVouchersModel
    {
        [Key]
        public int TransId { get; set; }
        public string TransType { get; set; }
        public string Memo { get; set; }
        public DateTime RefDate { get; set; }
    }
}