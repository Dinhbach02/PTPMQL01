using System.ComponentModel.DataAnnotations;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
namespace MvcBach.Models
{
    [Table("HeThongPhanPhoi")]
     public class HeThongPhanPhoi
     {
        
        public string MaHTPP {get;set;}

        [Key]
        public string TenHTPP { get; set; }
   
     }
}