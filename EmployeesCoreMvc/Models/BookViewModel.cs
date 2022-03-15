using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesCoreMvc.Models
{
    public class BookViewModel
    {
        [Key]
        public int BookID { get; set; }
        [Required]
       
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =("Price should be greater than or equal to 1"))]
        public int Price { get; set; }
    }
}
