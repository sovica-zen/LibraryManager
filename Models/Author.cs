using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LibraryManagementSystem.Models {
    public class Author {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        [Required]
        [StringLength(255)]
        public required string Name { get; set; }
        [Range(-5000, 2200)]
        public string? DateOfBirth { get; set; }

    }
}
