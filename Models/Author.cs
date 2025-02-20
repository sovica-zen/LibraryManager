using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Models {
    public class Author {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID { get; set; }
        public required string Name { get; set; }
        public string? DateOfBirth { get; set; }

    }
}
