using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models {
    public static class DataValidation {
        public static bool IsValidISO(string? date) {
            string format = "yyyy-MM-dd";
            var isvalid = DateOnly.TryParseExact(date, format, out _);
            if (isvalid) return true;
            return false;
        }
    }
}  