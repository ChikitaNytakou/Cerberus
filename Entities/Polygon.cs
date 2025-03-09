using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;

namespace ByeBye.Entities
{
    public class Polygon
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Наименование полигона
        /// </summary>
        [Required(ErrorMessage = "Наименование полигона является обязательным.")]
        [Display(Name = "Наименование полигона")]
        public string Name { get; set; }

        /// <summary>
        /// Тип Полигона. Потом будет завязан на должность
        /// </summary>
        [Display(Name = "Тип полигона")]
        public Data.PolygonType Type { get; set; } = 0;

        /// <summary>
        /// Код ПЭ НСИ полигона
        /// </summary>
        [Display(Name = "Код НСИ")]
        public string? Kod { get; set; }
    }
}
