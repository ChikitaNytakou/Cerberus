namespace ByeBye.Models.ViewModels
{
    public class SrVesGrPoezdaCoFourViewModel
    {
        public List<int> Id { get; set; } = new List<int>(); // Список Id записей в таблице
        public int Year { get; set; }
        public int Month { get; set; }
        public int Polygon { get; set; }
        public double Plan { get; set; }
        public List<double> FactTkm { get; set; } = new List<double>(); // Список значений FactTkm
        public List<double> FactPkm { get; set; } = new List<double>(); // Список значений FactPkm
        public double Fact { get; set; }
        public double Result { get; set; }
    }
}

