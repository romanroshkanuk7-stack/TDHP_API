using Microsoft.EntityFrameworkCore;
using TDHP_API.DTOs.Play;
using TDHP_API.TDHPDbContext;
using TDHP_API.TDHPDbContext.Models;

namespace TDHP_API.Services
{
    public class PlayService : IPlayService
    {
        private readonly THDPContext _db;
        public PlayService(THDPContext db) => _db = db;

        public async Task<List<PlayDto>> GetAllAsync()
        {
            var plays = await _db.Plays
                .OrderBy(p => p.SortIndex)
                .ThenBy(p => p.DateOfCreate)
                .ToListAsync();

            return plays.Select(p => ToDto(p)).ToList();
        }

        public async Task<PlayDto?> GetByIdAsync(Guid id)
        {
            var play = await _db.Plays.FindAsync(id);
            return play == null ? null : ToDto(play);
        }

        public async Task<List<PlayDto>> GetByCategoryAsync(Guid categoryId)
        {
            var plays = await _db.Plays
                .Where(p => p.PerformanceCategoryId == categoryId)
                .OrderBy(p => p.SortIndex)
                .ThenBy(p => p.DateOfCreate)
                .ToListAsync();

            return plays.Select(p => ToDto(p)).ToList();
        }

        public async Task<PlayDto> CreateAsync(CreatePlayDto dto)
        {
            int sortIndex = dto.SortIndex ?? 0;
            if (!dto.SortIndex.HasValue)
            {
                var maxIndex = await _db.Plays
                    .Where(p => p.PerformanceCategoryId == dto.PerformanceCategoryId)
                    .Select(p => (int?)p.SortIndex)
                    .MaxAsync() ?? -1;
                sortIndex = maxIndex + 1;
            }

            var entity = new PlayEntity
            {
                PerformanceCategoryId = dto.PerformanceCategoryId,
                Title = dto.Title,
                Image = dto.Image,
                Description = dto.Description,
                CreditsJson = dto.CreditsJson,
                Target = dto.Target,
                Duration = dto.Duration,
                SortIndex = sortIndex
            };

            _db.Plays.Add(entity);
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<PlayDto?> UpdateAsync(Guid id, UpdatePlayDto dto)
        {
            var entity = await _db.Plays.FindAsync(id);
            if (entity == null) return null;

            if (dto.PerformanceCategoryId.HasValue) entity.PerformanceCategoryId = dto.PerformanceCategoryId.Value;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Image != null) entity.Image = dto.Image;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.CreditsJson != null) entity.CreditsJson = dto.CreditsJson;
            if (dto.Target != null) entity.Target = dto.Target;
            if (dto.Duration != null) entity.Duration = dto.Duration;
            if (dto.SortIndex.HasValue) entity.SortIndex = dto.SortIndex.Value;

            entity.LastUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Plays.FindAsync(id);
            if (entity == null) return false;

            _db.Plays.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReorderAsync(List<Guid> playIds)
        {
            if (playIds == null || playIds.Count == 0) return false;

            var items = await _db.Plays.Where(p => playIds.Contains(p.Id)).ToListAsync();
            foreach (var item in items)
            {
                var newIndex = playIds.IndexOf(item.Id);
                if (newIndex >= 0)
                {
                    item.SortIndex = newIndex;
                    item.LastUpdate = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task SeedDefaultPlaysAsync()
        {
            if (await _db.Plays.AnyAsync()) return;

            var categories = await _db.PerformanceCategories.ToListAsync();
            var nejmensi = categories.FirstOrDefault(c => c.Slug == "pro-nejmensi");
            var skoly = categories.FirstOrDefault(c => c.Slug == "pro-skoly");
            var dospele = categories.FirstOrDefault(c => c.Slug == "pro-dospele");
            var rodinne = categories.FirstOrDefault(c => c.Slug == "rodinne");

            if (nejmensi != null)
            {
                var list = new List<CreatePlayDto>
                {
                    new()
                    {
                        PerformanceCategoryId = nejmensi.Id,
                        Title = "HANAKO",
                        Image = "/image 2.png",
                        Description = "Inspirace japonským příběhem o malé vesničce na břehu oceánu a ještě menší dívence na břehu života. Hanako se učí plavat sama v sobě. Vydává se do tajemných hlubin, aby našla nejkrásnějšího draka na světě, který se díky ní vznese až k vysokému nebi. Taneční pohádka",
                        CreditsJson = "{\"SCÉNÁŘ, CHOREOGRAFIE\": \"HONZA POKUSIL A EVA KRATOCHVÍLOVÁ\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"VÝPRAVA\": \"RADKA JANŮ\", \"TANČÍ A HRAJÍ\": \"BARBORA MACHOVÁ KŘOVÁČKOVÁ, VERONIKA ŠPAČKOVÁ\"}",
                        Target = "PRO DĚTI JIŽ OD 4 LET",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 40 MINUT ( + 10 MINUT POHYBOVÁ DÍLNA PO PŘEDCHOZÍ DOMLUVĚ)",
                        SortIndex = 0
                    },
                    new()
                    {
                        PerformanceCategoryId = nejmensi.Id,
                        Title = "HRAČKY",
                        Image = "/image 3.png",
                        Description = "Pohybová inscenace pro nejmenší diváky o světě hraček, které ožívají, když děti usnou. Hravé představení plné barev, hudby a fantazie.",
                        CreditsJson = "{\"SCÉNÁŘ, CHOREOGRAFIE\": \"BARBORA KŘOVÁČKOVÁ MACHOVÁ\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"TANČÍ A HRAJÍ\": \"VERONIKA ŠPAČKOVÁ, ONDŘEJ NOVOTNÝ\"}",
                        Target = "PRO DĚTI JIŽ OD 2 LET",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 35 MINUT",
                        SortIndex = 1
                    },
                    new()
                    {
                        PerformanceCategoryId = nejmensi.Id,
                        Title = "KVÍTKO",
                        Image = "/image 4.png",
                        Description = "Příběh o síle přírody, koloběhu ročních období a malém semínku, které se navzdory všem překážkám probije ke světlu.",
                        CreditsJson = "{\"SCÉNÁŘ, CHOREOGRAFIE\": \"ŠTĚPÁN MACH\", \"HUDBA\": \"JAN ČERNÝ\", \"TANČÍ A HRAJÍ\": \"BARBORA KŘOVÁČKOVÁ MACHOVÁ, ŠTĚPÁN MACH\"}",
                        Target = "PRO DĚTI JIŽ OD 3 LET",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 45 MINUT",
                        SortIndex = 2
                    },
                    new()
                    {
                        PerformanceCategoryId = nejmensi.Id,
                        Title = "BUBLINY",
                        Image = "/image 6.png",
                        Description = "Hravé interaktivní představení plné obřích mýdlových bublin, světelných efektů a radostného pohybu, které vtáhne do hry i ty nejmenší diváky.",
                        CreditsJson = "{\"CHOREOGRAFIE, NÁMĚT\": \"VERONIKA ŠPAČKOVÁ\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"TANČÍ A HRAJÍ\": \"VERONIKA ŠPAČKOVÁ, ONDŘEJ NOVOTNÝ\"}",
                        Target = "PRO DĚTI JIŽ OD 1.5 ROKU",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 30 MINUT",
                        SortIndex = 3
                    },
                    new()
                    {
                        PerformanceCategoryId = nejmensi.Id,
                        Title = "LESNÍ DOBRODRUŽSTVÍ",
                        Image = "/image 7.png",
                        Description = "Taneční vyprávění o zvířátkách v lese, která se učí pomáhat si navzájem a chránit svůj domov. Představení plné zvířecích motivů a akrobacie.",
                        CreditsJson = "{\"CHOREOGRAFIE\": \"ŠTĚPÁN MACH\", \"HUDBA\": \"ZVUKY LESA A DUŠAN HŘEBÍČEK\", \"TANČÍ\": \"SOUBOR TANEČNÍHO DIVADLA HONZY POKUSILA\"}",
                        Target = "PRO DĚTI JIŽ OD 3 LET",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 40 MINUT",
                        SortIndex = 4
                    }
                };

                foreach (var p in list) await CreateAsync(p);
            }

            if (skoly != null)
            {
                var list = new List<CreatePlayDto>
                {
                    new()
                    {
                        PerformanceCategoryId = skoly.Id,
                        Title = "NAŠE HISTORIE",
                        Image = "/image 7.png",
                        Description = "Edukativní taneční projekt přibližující důležité milníky našich dějin hravou a srozumitelnou formou pro školní mládež.",
                        CreditsJson = "{\"CHOREOGRAFIE\": \"ŠTĚPÁN MACH A HONZA POKUSIL\", \"HUDBA\": \"HISTORICKÉ MOTIVY\", \"TANČÍ\": \"SOUBOR TANEČNÍHO DIVADLA HONZY POKUSILA\"}",
                        Target = "PRO ŽÁKY 1. A 2. STUPNĚ ZŠ",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 50 MINUT",
                        SortIndex = 0
                    },
                    new()
                    {
                        PerformanceCategoryId = skoly.Id,
                        Title = "KOMUNIKACE",
                        Image = "/image 8.png",
                        Description = "Taneční reflexe moderních technologií a mezilidských vztahů v digitální éře. Jak se mění náš jazyk, když mluvíme přes obrazovky?",
                        CreditsJson = "{\"CHOREOGRAFIE\": \"EVA KRATOCHVÍLOVÁ\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"TANČÍ A HRAJÍ\": \"BARBORA MACHOVÁ KŘOVÁČKOVÁ, VERONIKA ŠPAČKOVÁ, ONDŘEJ NOVOTNÝ\"}",
                        Target = "PRO STŘEDNÍ ŠKOLY",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 45 MINUT",
                        SortIndex = 1
                    }
                };

                foreach (var p in list) await CreateAsync(p);
            }

            if (dospele != null)
            {
                var list = new List<CreatePlayDto>
                {
                    new()
                    {
                        PerformanceCategoryId = dospele.Id,
                        Title = "ZRCADLENÍ",
                        Image = "/image 9.png",
                        Description = "Celovečerní taneční inscenace zkoumající hlubiny lidského podvědomí, zrcadlení našich strachů, tužeb a skrytých já.",
                        CreditsJson = "{\"CHOREOGRAFIE\": \"HONZA POKUSIL\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"TANČÍ\": \"TANEČNÍ SOUBOR TDHP\"}",
                        Target = "PRO DOSPĚLÉ DIVÁKY",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 60 MINUT",
                        SortIndex = 0
                    },
                    new()
                    {
                        PerformanceCategoryId = dospele.Id,
                        Title = "KONTRASTY",
                        Image = "/image 6.png",
                        Description = "Současný taneční duet dvou odlišných individualit hledajících společnou řeč a harmonii prostřednictvím dialogu těl.",
                        CreditsJson = "{\"CHOREOGRAFIE\": \"BARBORA MACHOVÁ\", \"TANČÍ\": \"BARBORA MACHOVÁ, ŠTĚPÁN MACH\"}",
                        Target = "PRO DOSPĚLÉ DIVÁKY",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 50 MINUT",
                        SortIndex = 1
                    }
                };

                foreach (var p in list) await CreateAsync(p);
            }

            if (rodinne != null)
            {
                var list = new List<CreatePlayDto>
                {
                    new()
                    {
                        PerformanceCategoryId = rodinne.Id,
                        Title = "HANAKO",
                        Image = "/image 2.png",
                        Description = "Inspirace japonským příběhem o malé vesničce na břehu oceánu a ještě menší dívence na břehu života. Hanako se učí plavat sama v sobě. Vydává se do tajemných hlubin, aby našla nejkrásnějšího draka na světě, který se díky ní vznese až k vysokému nebi. Taneční pohádka pro celé rodiny.",
                        CreditsJson = "{\"SCÉNÁŘ, CHOREOGRAFIE\": \"HONZA POKUSIL A EVA KRATOCHVÍLOVÁ\", \"HUDBA\": \"DUŠAN HŘEBÍČEK\", \"TANČÍ\": \"TDHP SOUBOR\"}",
                        Target = "PRO RODINY S DĚTMI OD 4 LET",
                        Duration = "DÉLKA PŘEDSTAVENÍ: 40 MINUT",
                        SortIndex = 0
                    }
                };

                foreach (var p in list) await CreateAsync(p);
            }
        }

        private static PlayDto ToDto(PlayEntity p) => new()
        {
            Id = p.Id,
            PerformanceCategoryId = p.PerformanceCategoryId,
            Title = p.Title,
            Image = p.Image,
            Description = p.Description,
            Credits = string.IsNullOrEmpty(p.CreditsJson)
                ? new Dictionary<string, string>()
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(p.CreditsJson) ?? new(),
            Target = p.Target,
            Duration = p.Duration,
            SortIndex = p.SortIndex
        };
    }
}
