using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ByeBye.Entities
{
    public class SrVesGrPoezdaCoFour
    {
        /// <summary>
        /// Сдача вагонов
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Полигон
        /// </summary>
        [ForeignKey("Polygon")]
        public int PolygonId { get; set; } // Внешний ключ на Polygon
        public Polygon Polygon { get; set; }

        /// <summary>
        /// Дата начала смены
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime Start { get; set; }

        /// <summary>
        /// План
        /// </summary>
        public double Plan { get; set; }

        /// <summary>
        /// Факт (т-км)
        /// </summary>
        public double FactTkm { get; set; }

        /// <summary>
        /// Факт (п-км)
        /// </summary>
        public double FactPkm { get; set; }

        /// <summary>
        /// Факт (рассчитанный)
        /// </summary>
        public double Fact { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public double Result { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime? LastUpdated { get; set; }
    }
}
