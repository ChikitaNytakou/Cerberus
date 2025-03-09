using ByeBye.Entities;

namespace Miniproj.Coefficients
{
    public class Coefficient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public string Description { get; set; }
        public double? SmenaValue { get; set; }
        public double? MonthValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
