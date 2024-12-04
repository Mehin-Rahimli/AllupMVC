using AllupMVC.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace AllupMVC.Models
{
    public class Category:BaseEntity
    {

        public int Id { get; set; }
        [MaxLength(50,ErrorMessage ="Max Category name length is 50")]
        public string Name {  get; set; }


        //relational
        public List<Product>? Products { get; set; }  
    }
}
