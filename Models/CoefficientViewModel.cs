using ByeBye.Entities;
using Miniproj.Coefficients;
using System.Globalization;

namespace Bankbook.Models.ViewModels
{
    public class CoefficientViewModel
    {
        public CoefficientViewModel()
        {
        }

        public CoefficientViewModel(Coefficient сoefficient)
        {
            Description = сoefficient.Description;
            Value1 = сoefficient.SmenaValue.ToString();
            Value2 = сoefficient.MonthValue.ToString();
        }

        /// <summary>
        /// Наименование должности
        /// </summary>
        public string PositionType { get; set; }

        /// <summary>
        /// Описание показателя премирования
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Размер премии за СМЕНУ (в процентах)
        /// </summary>
        private string? _value1;
        public string? Value1
        {
            get => _value1;
            set => _value1 = value?.Replace('.', ',');
        }

        /// <summary>
        /// Размер премии за МЕСЯЦ (в процентах)
        /// </summary>
        private string? _value2;
        public string? Value2
        {
            get => _value2;
            set => _value2 = value?.Replace('.', ',');
        }

        /// <summary>
        /// Значение за смену для расчётов
        /// </summary>
        public double SmenaValue => string.IsNullOrWhiteSpace(Value1) ? 1 : double.TryParse(Value1.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        /// <summary>
        /// Значение за месяц для расчётов
        /// </summary>
        public double MonthValue => string.IsNullOrWhiteSpace(Value2) ? 1 : double.TryParse(Value2.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : 0;

        public override bool Equals(object? obj)
        {
            if (obj is CoefficientViewModel other)
            {
                var result = (PositionType == other.PositionType &&
                       Description.Trim() == other.Description.Trim() &&
                       SmenaValue == other.SmenaValue &&
                       MonthValue == other.MonthValue);
                return result;
            }
            return false;
        }
    }
}
