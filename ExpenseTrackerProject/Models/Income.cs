﻿using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerProject.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal IncomeAmount { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
