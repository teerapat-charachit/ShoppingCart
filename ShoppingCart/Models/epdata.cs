using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class empdata
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? lastname { get; set; }

        public string? nickname { get; set; }

        public string? gen { get; set; }
        public string? religion { get; set; }

        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        public int idcard { get; set; }
        public int phone { get; set; }
        public string Email { get; set; }
        public string? address { get; set; }

    }
}