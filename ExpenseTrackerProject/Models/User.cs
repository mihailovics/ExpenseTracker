using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerProject.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public decimal AllowedMinus { get; set; }
        public ICollection<Income>? Incomes { get; set; }
        public ICollection<Outcome>? Outcomes { get; set; }
    }
}
