using System.ComponentModel.DataAnnotations;

namespace DNote.Models
{
    public class Category
    {
        public int id { get; set; }
        [Display(Name = "Name")]
        public string CategoryName { get; set; }

        public Category()
        {

        }
    }
}
