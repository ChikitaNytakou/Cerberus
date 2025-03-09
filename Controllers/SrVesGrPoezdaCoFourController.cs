using ByeBye.Entities;
using ByeBye.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using System.Xml;
using ByeBye.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using ByeBye.Models;
using System.Diagnostics;

namespace ByeBye.Controllers
{
    public class SrVesGrPoezdaCoFourController : Controller
    {
        private static readonly string dayBlueColor = "#5eb5f7";

        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;

        private string _currentUserId;
        private string _currentUserName;
        private string _ipAddress;
        public SrVesGrPoezdaCoFourController(ApplicationDbContext context,
                                     UserManager<User> userManager)
        {
            _context = context;

            //_currentUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Не определён";
        }

        /// <summary>
        /// Отображение представления для показателя
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public IActionResult SrVesGrPoezdaCoFour(int polygonId = 0, int year = 0)
        {
            // Устанавливаем текущий год по умолчанию
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            // Определяем полигон только если нужно
            if (polygonId == 0)
            {
                // Получаем полигон и его kod в одном запросе
                var defaultPolygon = _context.Polygons
                    .AsNoTracking()
                    .Where(p => p.Name != "Не указано" && p.Type == PolygonType.РУ)
                    .Select(p => new { p.Id, p.Kod })
                    .FirstOrDefault();

                if (defaultPolygon != null)
                {
                    polygonId = defaultPolygon.Id;
                    // Сразу сохраняем код полигона
                    ViewBag.HasMultiplePolygonIds = defaultPolygon.Kod?.Contains(',') ?? false;
                }

            }
            else
            {
                // Получаем код полигона только если polygonId задан
                var polygonKod = _context.Polygons
                    .AsNoTracking()
                    .Where(p => p.Id == polygonId)
                    .Select(p => p.Kod)
                    .FirstOrDefault();

                ViewBag.HasMultiplePolygonIds = polygonKod?.Contains(',') ?? false;
            }

            // 2. Оптимизируем основной запрос, добавив более конкретные условия фильтрации
            // Извлекаем только нужные месяцы, если есть такая возможность
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);

            var rawData = _context.SrVesGrPoezdaCoFour
                .Where(e => e.Start >= startDate && e.Start <= endDate && e.PolygonId == polygonId)
                .Select(e => new
                { // Используем анонимный тип для снижения объема данных
                    e.Id,
                    Month = e.Start.Month,
                    e.Plan,
                    e.FactTkm,
                    e.FactPkm,
                    e.PolygonId
                })
                .ToList(); // Загружаем данные в память один раз

            // 3. Более эффективно группируем и обрабатываем данные в памяти
            var monthlyData = rawData.GroupBy(e => e.Month)
                .ToDictionary(g => g.Key, g => new
                {
                    Ids = g.Select(e => e.Id).ToList(),
                    MaxPlan = g.Max(e => e.Plan),
                    TotalFactTkm = g.Sum(e => e.FactTkm),
                    TotalFactPkm = g.Sum(e => e.FactPkm),
                    FactTkmList = g.Select(e => e.FactTkm).ToList(),
                    FactPkmList = g.Select(e => e.FactPkm).ToList(),
                    TotalPolygonId = g.Max(e => e.PolygonId)
                });

            //var monthlyData = _context.SrVesGrPoezdaCoFour
            //    .Where(e => e.Start >= startDate && e.Start <= endDate && e.PolygonId == polygonId)
            //    .GroupBy(e => e.Start.Month)
            //    .Select(g => new
            //    {
            //        Month = g.Key,
            //        Ids = g.Select(e => e.Id).ToList(),
            //        MaxPlan = g.Max(e => e.Plan),
            //        TotalFactTkm = g.Sum(e => e.FactTkm),
            //        TotalFactPkm = g.Sum(e => e.FactPkm),
            //        FactTkmList = g.Select(e => e.FactTkm).ToList(),
            //        FactPkmList = g.Select(e => e.FactPkm).ToList(),
            //        TotalPolygonId = g.Max(e => e.PolygonId)
            //    })
            //    .ToDictionary(g => g.Month, g => g); // Преобразуем сразу в словарь

            // 4. Формируем итоговую модель из предварительно обработанных данных
            var srVesCoFour = monthlyData.Select(kv => new SrVesGrPoezdaCoFourViewModel
            {
                Id = kv.Value.Ids,
                Month = kv.Key,
                Plan = kv.Value.MaxPlan,
                FactTkm = kv.Value.FactTkmList,
                FactPkm = kv.Value.FactPkmList,
                Fact = kv.Value.TotalFactPkm != 0 ? (kv.Value.TotalFactTkm / kv.Value.TotalFactPkm * 1000) : 0.0,
                Result = kv.Value.TotalFactPkm != 0 && kv.Value.MaxPlan != 0
                    ? ((kv.Value.TotalFactTkm / kv.Value.TotalFactPkm * 1000) / kv.Value.MaxPlan * 100)
                    : 0.0,
                Polygon = kv.Value.TotalPolygonId
            }).ToList();

            // Сохраняем выбранный год и закрытые месяцы в ViewBag
            ViewBag.SelectedYear = year;
            ViewBag.PolygonId = polygonId; // Сохраняем polygonId в ViewBag
            ViewData["Title"] = "Средний вес грузовых поездов без передаточных и вывозных поездов (ДГПС) 2";

            return View(srVesCoFour);
        }

        /// <summary>
        /// Список РУ
        /// </summary>
        /// <returns>Список РУ</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolygonViewModelGet>>> GetPolygons()
        {
            var polygons = await _context.Polygons
                .AsNoTracking()
                .Where(p => p.Name != "Не указано" && p.Type == PolygonType.РУ)
                .Select(p => new PolygonViewModelGet
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

            return Ok(polygons);
        }

        /// <summary>
        /// Обновление данных в базе
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        [HttpPost]
        public async Task<IActionResult> SaveSrVesGrPoezdaCoFour([FromBody] SrVesGrPoezdaCoFourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Получаем все записи с тем же годом, месяцем и полигоном
            var existingEntries = await _context.SrVesGrPoezdaCoFour
                .Where(e => e.Start.Year == model.Year && e.Start.Month == model.Month && e.PolygonId == model.Polygon)
                .ToListAsync();

            // Инициализируем суммы
            double totalFactTkm = model.FactTkm.Sum();
            double totalFactPkm = model.FactPkm.Sum();

            // Рассчитываем факт
            var fact = totalFactPkm > 0 ? (totalFactTkm / totalFactPkm) * 1000 : 0;

            // Рассчитываем результат
            var result = (model.Plan > 0 && totalFactPkm > 0) ? (totalFactTkm / totalFactPkm * 1000) / model.Plan * 100 : 0;


            if (existingEntries.Count == 0 && model.Id.Count == 1)
            {
                // Если записи не существует и это одинарный полигон, создаем новую
                var newSrVesCoFour = new SrVesGrPoezdaCoFour
                {
                    Start = new DateTime(model.Year, model.Month, 1),
                    Plan = model.Plan,
                    FactTkm = model.FactTkm[0],
                    FactPkm = model.FactPkm[0],
                    Fact = fact,
                    Result = result,
                    LastUpdated = DateTime.UtcNow,
                    PolygonId = model.Polygon
                };
                await _context.SrVesGrPoezdaCoFour.AddAsync(newSrVesCoFour);
            }
            else if (existingEntries.Count == 0 && model.Id.Count == 2)
            {
                // Если записи не существует и это соединённый полигон, создаем две новых записи
                var newSrVesCoFour1 = new SrVesGrPoezdaCoFour
                {
                    Start = new DateTime(model.Year, model.Month, 1),
                    Plan = model.Plan,
                    FactTkm = model.FactTkm[0],
                    FactPkm = model.FactPkm[0],
                    Fact = fact,
                    Result = result,
                    LastUpdated = DateTime.UtcNow,
                    PolygonId = model.Polygon
                };
                await _context.SrVesGrPoezdaCoFour.AddAsync(newSrVesCoFour1);

                var newSrVesCoFour2 = new SrVesGrPoezdaCoFour
                {
                    Start = new DateTime(model.Year, model.Month, 1),
                    Plan = model.Plan,
                    FactTkm = model.FactTkm[1],
                    FactPkm = model.FactPkm[1],
                    Fact = fact,
                    Result = result,
                    LastUpdated = DateTime.UtcNow,
                    PolygonId = model.Polygon
                };
                await _context.SrVesGrPoezdaCoFour.AddAsync(newSrVesCoFour2);
            }
            else
            {
                // Обновляем существующую(ие) запись(и)
                foreach (var entry in existingEntries)
                {
                    entry.Plan = model.Plan;
                    entry.Fact = fact;
                    entry.Result = result;
                    entry.LastUpdated = DateTime.UtcNow;

                    // Обновляем FactTkm и FactPkm в зависимости от ID
                    entry.FactTkm = entry.Id == model.Id[0] ? model.FactTkm[0] : model.FactTkm[1];
                    entry.FactPkm = entry.Id == model.Id[0] ? model.FactPkm[0] : model.FactPkm[1];

                    if (model.Id.Count == 1 && entry.Plan == 0 && entry.FactTkm == 0 && entry.FactPkm == 0)
                    {
                        _context.SrVesGrPoezdaCoFour.Remove(entry);
                    }
                    if (model.Id.Count == 2 && model.Plan == 0 && model.FactTkm[0] == 0 && model.FactPkm[0] == 0 && model.FactTkm[1] == 0 && model.FactPkm[1] == 0)
                    {
                        _context.SrVesGrPoezdaCoFour.Remove(entry);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ошибка при сохранении данных.");
            }

            return Ok();
        }

        /// <summary>
        /// Получение данных об изменениях показателя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetUpdateInfo([FromBody] SrVesGrPoezdaCoFourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Получаем запись по месяцу из базы данных
            var srVesCoFourData = await _context.SrVesGrPoezdaCoFour
               .FirstOrDefaultAsync(e => e.Start.Year == model.Year && e.Start.Month == model.Month && e.PolygonId == model.Polygon);

            if (srVesCoFourData != null)
            {
                return Json(new
                {
                    LastUpdated = srVesCoFourData.LastUpdated
                });
            }

            return NotFound();
        }

        /// <summary>
        /// Выборка всех показателей экспорта услуг по графику дежурств
        /// </summary>
        /// <param name="timetable"><see cref="Timetable">График дежурств</see> выбранного пользователя</param>
        /// <returns>Список всех показателей по данному графику дежурств пользователя</returns>
        //public List<Export> GetUserExportUslugs(List<Timetable> timetable)
        //{
        //    List<Export> exports = new();

        //    foreach (Timetable t in timetable)
        //    {
        //        var export = _context.Exports
        //        .FirstOrDefault(s =>
        //            s.Start.Date.Month == t.Start.Month && s.Start.Date.Year == t.Start.Year);
        //        if (export != null)
        //            exports.Add(export);
        //        else
        //            exports.Add(new Export()
        //            {
        //                Result = 0,
        //                Start = t.Start,
        //                Fact = 0,
        //                Plan = 0,
        //            });
        //    }
        //    return exports;
        //}
    }
}
