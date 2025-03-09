using ByeBye.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Miniproj.Coefficients;

namespace Bankbook.Controllers
{
    /// <summary>
    /// Контроллер загрузки данных из ИАСПУРГП
    /// </summary>
    [Authorize]
    public class CoefficientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public CoefficientController(ApplicationDbContext context,
                                   UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> SeedCoefficients_rew()
        {
            ViewData["Title"] = "Коэффициенты";

            // Проверяем, есть ли данные в таблице RelationForUzel
            if (!_context.Coefficients.Any())
            {
                // Если таблица пуста, заполняем её данными
                SeedCoefficients();
                // Логика инициализации данных
                return Ok(new { success = true, message = "Коэффициенты успешно инициализированы." });
            }
            return Ok(new { success = true, message = "Коэффициенты уже успешно инициализированы." });
        }

        public void SeedCoefficients()
        {
            // Проверьте, есть ли данные в таблице Coefficients
            if (!_context.Coefficients.Any())
            {
                // Пример данных для заполнения
                var coefficientsData = new List<Coefficient>
        {
            new Coefficient
            {
                Name = "coefSecSpeed",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "1. Выполнение плана участковой скорости движения поездов без передаточных и вывозных поездов за смену",
                SmenaValue = 1.25,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOtsutZader",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "2. Отсутствие задержек проследования поездов международных, межрегиональных, региональных и городских линий и грузовых поездов за смену, отнесенных на службу перевозок",
                SmenaValue = 1.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdV1",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.1. на однопутных участках",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdV2",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.2. на двухпутных участках",
                SmenaValue = 0.02,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdTD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.3.1. тяжеловесный и длинносоставный",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdPvPd",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.3.2. повышенного веса или повышенной длины",
                SmenaValue = 0.1,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdSbor",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.3.3. сборный",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPoezdOpN",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "3.3.4. с вагонами, загруженными опасными грузами класса 1 и негабаритными грузами",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExecutPlan",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "4. Выполнение плана погрузки грузов в тоннах",
                SmenaValue = null,
                MonthValue = 10.0,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOkna",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "5. Обеспечение заданных размеров движения и технологии перевозочного процесса в период предоставления \"окон\" для ремонтных и строительно-монтажных работ, за каждое предоставленное \"окон\" в смену",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "6. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRazvoz",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "7. Выполнение плана развоза местного груза в вагонах",
                SmenaValue = 0.6,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPeredacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 5),
                Description = "8. Выполнение плана передачи местного груза в вагонах",
                SmenaValue = 0.6,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrVes",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "1. Выполнение плана среднего веса отправленных грузовых поездов своего формирования",
                SmenaValue = 1.25,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOtsutZader",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "2. Отсутствие задержек проследования поездов международных, межрегиональных, региональных и городских линий и грузовых поездов за смену, отнесенных на службу перевозок",
                SmenaValue = 1.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefVsego",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.1. грузовой без вывозных и передаточных поездов",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefTD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.2. тяжеловесный или длинносоставный",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSborn",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.3. сборный",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPVPD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.4. повышенного веса или повышенной длины",
                SmenaValue = 0.1,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefU",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.5. дальнего назначения, не предусмотренный планом формирования поездов",
                SmenaValue = 0.15,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.6. на удлиненное гарантийное плечо обслуживания локомотивными бригадами",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOpg",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "3.7. с вагонами, загруженными опасными грузами класса 1 и негабаритными грузами",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOkna",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "5. Обеспечение заданных размеров движения и технологии перевозочного процесса в период предоставления \"окон\" для ремонтных и строительно-монтажных работ, за каждое предоставленное \"окон\" в смену",
                SmenaValue = 0.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "6. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExecutPlan",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "4. Выполнение плана погрузки грузов в тоннах",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRazvoz",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "7. Выполнение плана развоза местного груза в вагонах",
                SmenaValue = 0.6,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPeredacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 6),
                Description = "8. Выполнение плана передачи местного груза в вагонах",
                SmenaValue = 0.6,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOtsutZader",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "1. Отсутствие задержек проследования, отправления и прибытия поездов международных, межрегиональных, региональных и городских линий и грузовых поездов за смену, отнесенных на службу перевозок",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProizv",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "2. Выполнение плана производительности поездного локомотива за сутки",
                SmenaValue = 0.25,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrUchScor",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "3. Выполнение плана участковой скорости движения поездов без передаточных и вывозных поездов за смену",
                SmenaValue = 0.25,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrVrOborotVag",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "4. Выполнение плана по обороту вагона с местным грузом за сутки",
                SmenaValue = 0.25,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "5. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExport",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "12. Экспорт услуг (к аналогичному периоду прошлого года, в процентах)",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExecutPlan",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "8. Выполнение плана погрузки грузов в тоннах",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPV",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "9.1. повышенного веса",
                SmenaValue = 0.02,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "9.2. повышенной длины",
                SmenaValue = 0.02,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "9.3. дальнего назначения, не предусмотренный планом формирования поездов",
                SmenaValue = 0.08,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRazvoz",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "10. Выполнение плана развоза местного груза в вагонах",
                SmenaValue = 0.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPeredacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 7),
                Description = "11. Выполнение плана передачи местного груза в вагонах",
                SmenaValue = 0.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProizv",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "1. Выполнение плана производительности поездного локомотива в границах отделения Белорусской железной дороги за сутки",
                SmenaValue = 1.1,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProb",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "2. Выполнение плана среднесуточного пробега локомотива в границах отделения за сутки",
                SmenaValue = 1.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefU",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "3.1. на удлиненное гарантийное плечо обслуживания локомотивными бригадами",
                SmenaValue = 0.05,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPVPD",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "3.2. повышенного веса или повышенной длинны",
                SmenaValue = 0.1,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefBKG",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "4. Выполнение плана среднего веса отправленных грузовых поездов с локомотивами серии БКГ1",
                SmenaValue = null,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOtsutNarush",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "5. Отсутствие нарушений режима непрерывной работы локомотивных бригад, отнесенных на службу перевозок, за смену",
                SmenaValue = null,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRemont",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "6. Выполнение своевременной подгонки локомотивов на плановые виды ремонтов и технического обслуживания (ТР-1, ТР-2, ТР-3, ТО-3)",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 8),
                Description = "7. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProizv",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "1. Выполнение плана производительности поездного локомотива в границах отделения Белорусской железной дороги за сутки",
                SmenaValue = 1.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProb",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "2. Выполнение плана среднесуточного пробега локомотива в границах отделения за сутки",
                SmenaValue = 1.1,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrVesGrPoezda",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "3. Выполнение плана среднего веса грузового поезда без передаточных и вывозных поездов за сутки",
                SmenaValue = null,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefBKG",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "4. Выполнение плана среднего веса отправленных грузовых поездов с локомотивами серии БКГ1 в целом по Белорусской железной дороге",
                SmenaValue = null,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRemont",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "5. Выполнение своевременной подгонки локомотивов на плановые виды ремонтов и технического обслуживания (ТР-1, ТР-2, ТР-3, ТО-3)",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "6. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 1.2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSaldo",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "7. Обеспечение роста положительного сальдо тягового обслуживания с соседними железнодорожными администрациями (к аналогичному периоду прошлого года, в процентах)",
                SmenaValue = null,
                MonthValue = 15,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefU",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 9),
                Description = "8. За каждый отправленный поезд на удлиненное гарантийное плечо обслуживания локомотивными бригадами",
                SmenaValue = 0.01,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefOtsutZader",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "1. Отсутствие задержек проследования, отправления и прибытия поездов международных, межрегиональных, региональных и городских линий и грузовых поездов за смену, отнесенных на службу перевозок",
                SmenaValue = 1.4,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrProizv",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "2. Выполнение плана производительности поездного локомотива за сутки",
                SmenaValue = 0.8,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSdacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "3. Выполнение плана сдачи вагонов по Белорусской железной дороге за сутки",
                SmenaValue = 2,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefSrVesGrPoezda",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "4. Выполнение плана среднего веса грузового поезда без передаточных и вывозных поездов за сутки",
                SmenaValue = 0.5,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExecutPlan",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "5. Выполнение плана погрузки грузов в тоннах",
                SmenaValue = null,
                MonthValue = 10,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefRazvoz",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "6. Выполнение плана развоза местного груза в вагонах",
                SmenaValue = 0.7,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefPeredacha",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "7. Выполнение плана передачи местного груза в вагонах",
                SmenaValue = 0.7,
                MonthValue = null,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            },
            new Coefficient
            {
                Name = "coefExport",
                Position = _context.Positions.FirstOrDefault(p => p.Id == 10),
                Description = "8. Экспорт услуг (к аналогичному периоду прошлого года, в процентах)",
                SmenaValue = null,
                MonthValue = 15,
                StartDate = new DateTime(2020, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null
            }
        };

                // Добавляем данные в таблицу Coefficients
                _context.Coefficients.AddRange(coefficientsData);
                _context.SaveChanges();
            }
        }
    }
}
