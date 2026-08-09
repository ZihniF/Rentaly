using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.DataAccessLayer.Concrete;

public static class RentalyDataSeeder
{
    private static readonly string[] RetiredDemoVins =
    {
        "RNTLY202600000009", // Bentley Continental GT
        "RNTLY202600000010", // Chevrolet Camaro
        "RNTLY202600000011"  // Ferrari Enzo
    };

    public static async Task SeedExpandedFleetAsync(RentalyContext context)
    {
        var branches = await context.Branches.ToListAsync();
        var branchDefinitions = new[]
        {
            (Name: "İstanbul Havalimanı", City: "İstanbul", Address: "Tayakadın, İstanbul Havalimanı"),
            (Name: "Kadıköy", City: "İstanbul", Address: "Caferağa, Kadıköy"),
            (Name: "Ankara Esenboğa", City: "Ankara", Address: "Esenboğa Havalimanı"),
            (Name: "İzmir Adnan Menderes", City: "İzmir", Address: "Dokuz Eylül, Gaziemir"),
            (Name: "Antalya Havalimanı", City: "Antalya", Address: "Yeşilköy, Muratpaşa"),
            (Name: "Bursa Nilüfer", City: "Bursa", Address: "Odunluk, Nilüfer")
        };

        foreach (var definition in branchDefinitions)
        {
            if (branches.Any(x => x.BranchName == definition.Name))
                continue;

            var branch = new Branch
            {
                BranchName = definition.Name,
                City = definition.City,
                Address = definition.Address
            };
            branches.Add(branch);
            context.Branches.Add(branch);
        }

        var categories = await context.Categories.ToListAsync();
        foreach (var name in new[] { "Sedan", "SUV", "Hatchback", "Minivan", "Pickup", "Lüks" })
        {
            if (categories.Any(x => x.CategoryName == name))
                continue;

            var category = new Category { CategoryName = name };
            categories.Add(category);
            context.Categories.Add(category);
        }

        var brandNames = new[]
        {
            "Audi", "BMW", "Dacia", "Fiat", "Ford", "Honda", "Hyundai", "Jeep", "Kia",
            "Land Rover", "Lexus", "Mercedes-Benz", "MINI", "Nissan", "Opel", "Peugeot",
            "Renault", "SEAT", "Skoda", "Toyota", "Volkswagen"
        };
        var brands = await context.Brands.ToListAsync();
        foreach (var name in brandNames)
        {
            if (brands.Any(x => x.BrandName == name))
                continue;

            var brand = new Brand { BrandName = name, ImageUrl = string.Empty };
            brands.Add(brand);
            context.Brands.Add(brand);
        }
        await context.SaveChangesAsync();

        var modelDefinitions = new[]
        {
            ("Audi", "A4"), ("Audi", "A6"), ("Audi", "Q3"), ("Audi", "Q5"),
            ("BMW", "M5"), ("BMW", "320i"), ("BMW", "520i"), ("BMW", "X1"), ("BMW", "X3"),
            ("Dacia", "Sandero"), ("Dacia", "Duster"), ("Fiat", "Egea"), ("Ford", "Focus"),
            ("Ford", "Raptor"), ("Honda", "Civic"), ("Hyundai", "i20"), ("Hyundai", "Tucson"),
            ("Hyundai", "Staria"), ("Jeep", "Renegade"), ("Kia", "Sportage"),
            ("Land Rover", "Range Rover Sport"), ("Lexus", "RX"),
            ("Mercedes-Benz", "C 200"), ("Mercedes-Benz", "E 200"), ("Mercedes-Benz", "GLC 200"),
            ("MINI", "Cooper"), ("Nissan", "Qashqai"), ("Opel", "Corsa"),
            ("Peugeot", "208"), ("Peugeot", "3008"), ("Renault", "Clio"),
            ("Renault", "Megane Sedan"), ("Renault", "Austral"), ("SEAT", "Ibiza"),
            ("Skoda", "Fabia"), ("Skoda", "Octavia"), ("Skoda", "Superb"),
            ("Skoda", "Karoq"), ("Skoda", "Kodiaq"), ("Toyota", "Corolla"),
            ("Toyota", "C-HR"), ("Toyota", "RAV4"), ("Volkswagen", "Polo"),
            ("Volkswagen", "Passat")
        };

        var models = await context.CarModels.ToListAsync();
        foreach (var (brandName, modelName) in modelDefinitions)
        {
            var brandId = brands.First(x => x.BrandName == brandName).BrandId;
            if (models.Any(x => x.BrandId == brandId && x.ModelName == modelName))
                continue;

            var carModel = new CarModel { BrandId = brandId, ModelName = modelName };
            models.Add(carModel);
            context.CarModels.Add(carModel);
        }
        await context.SaveChangesAsync();

        var retiredCars = await context.Cars
            .Where(x => RetiredDemoVins.Contains(x.VIN))
            .ToListAsync();
        foreach (var car in retiredCars)
        {
            car.IsActive = false;
            car.IsAvailable = false;
        }

        Category Category(string name) => categories.First(x => x.CategoryName == name);
        Branch Branch(string name) => branches.First(x => x.BranchName == name);
        Brand Brand(string name) => brands.First(x => x.BrandName == name);
        CarModel Model(string brandName, string modelName)
        {
            var brandId = Brand(brandName).BrandId;
            return models.First(x => x.BrandId == brandId && x.ModelName == modelName);
        }

        var definitions = new[]
        {
            FleetCar("BMW", "M5", "Sedan", "İstanbul Havalimanı", "34 RNT 001", "RNTLY202600000001", 2026, 9800, 5250, 15000, "/rentaly/images/cars-alt/bmw-m5.png", 5, 2, "Benzin"),
            FleetCar("Volkswagen", "Polo", "Hatchback", "Kadıköy", "34 RNT 002", "RNTLY202600000002", 2025, 12400, 2250, 7000, "/rentaly/images/cars-alt/vw-polo.png", 5, 2, "Benzin"),
            FleetCar("Toyota", "RAV4", "SUV", "Ankara Esenboğa", "06 RNT 003", "RNTLY202600000003", 2026, 6400, 3600, 10000, "/rentaly/images/cars-alt/toyota-rav.png", 5, 3, "Hibrit"),
            FleetCar("Jeep", "Renegade", "SUV", "İstanbul Havalimanı", "34 RNT 004", "RNTLY202600000004", 2025, 18000, 3900, 10000, "/rentaly/images/cars-alt/jeep-renegade.png", 5, 3, "Benzin"),
            FleetCar("MINI", "Cooper", "Hatchback", "Kadıköy", "34 RNT 005", "RNTLY202600000005", 2026, 5100, 3450, 9000, "/rentaly/images/cars-alt/mini-cooper.png", 4, 1, "Benzin"),
            FleetCar("Ford", "Raptor", "Pickup", "Ankara Esenboğa", "06 RNT 006", "RNTLY202600000006", 2025, 22100, 4900, 14000, "/rentaly/images/cars-alt/ford-raptor.png", 5, 4, "Dizel"),
            FleetCar("Hyundai", "Staria", "Minivan", "İstanbul Havalimanı", "34 RNT 007", "RNTLY202600000007", 2026, 8300, 4200, 12000, "/rentaly/images/cars-alt/hyundai-staria.png", 7, 5, "Dizel"),
            FleetCar("Land Rover", "Range Rover Sport", "Lüks", "Kadıköy", "34 RNT 008", "RNTLY202600000008", 2026, 7600, 6900, 20000, "/rentaly/images/cars-alt/range-rover.png", 5, 4, "Hibrit"),
            FleetCar("Lexus", "RX", "SUV", "Kadıköy", "34 RNT 012", "RNTLY202600000012", 2026, 4700, 4700, 13000, "/rentaly/images/cars-alt/lexus.png", 5, 4, "Hibrit"),

            FleetCar("Renault", "Clio", "Hatchback", "İstanbul Havalimanı", "34 RNT 013", "RNTLY202600000013", 2025, 14200, 1850, 6000, "/rentaly/images/cars/vw-polo.jpg", 5, 2, "Benzin"),
            FleetCar("Hyundai", "i20", "Hatchback", "İzmir Adnan Menderes", "35 RNT 014", "RNTLY202600000014", 2025, 11700, 1900, 6000, "/rentaly/images/cars-alt/vw-polo.png", 5, 2, "Benzin"),
            FleetCar("Peugeot", "208", "Hatchback", "Antalya Havalimanı", "07 RNT 015", "RNTLY202600000015", 2026, 6900, 2100, 6500, "/rentaly/images/cars/mini-cooper.jpg", 5, 2, "Benzin"),
            FleetCar("Opel", "Corsa", "Hatchback", "Bursa Nilüfer", "16 RNT 016", "RNTLY202600000016", 2025, 15800, 2050, 6500, "/rentaly/images/cars-alt/mini-cooper.png", 5, 2, "Benzin"),
            FleetCar("Skoda", "Fabia", "Hatchback", "Ankara Esenboğa", "06 RNT 017", "RNTLY202600000017", 2026, 7400, 2150, 6500, "/rentaly/images/cars/vw-polo.jpg", 5, 2, "Benzin"),
            FleetCar("Dacia", "Sandero", "Hatchback", "İzmir Adnan Menderes", "35 RNT 018", "RNTLY202600000018", 2025, 19200, 1750, 5500, "/rentaly/images/cars-alt/vw-polo.png", 5, 2, "Benzin"),
            FleetCar("SEAT", "Ibiza", "Hatchback", "Kadıköy", "34 RNT 019", "RNTLY202600000019", 2025, 13400, 2200, 6500, "/rentaly/images/cars/mini-cooper.jpg", 5, 2, "Benzin"),

            FleetCar("Renault", "Megane Sedan", "Sedan", "Bursa Nilüfer", "16 RNT 020", "RNTLY202600000020", 2025, 18600, 2550, 7500, "/rentaly/images/cars-alt/bmw-m5.png", 5, 3, "Dizel"),
            FleetCar("Toyota", "Corolla", "Sedan", "Antalya Havalimanı", "07 RNT 021", "RNTLY202600000021", 2026, 9100, 2700, 8000, "/rentaly/images/cars/lexus.jpg", 5, 3, "Hibrit"),
            FleetCar("Skoda", "Octavia", "Sedan", "İstanbul Havalimanı", "34 RNT 022", "RNTLY202600000022", 2026, 7800, 2950, 8500, "/rentaly/images/cars-alt/bmw-m5.png", 5, 4, "Benzin"),
            FleetCar("Volkswagen", "Passat", "Sedan", "Ankara Esenboğa", "06 RNT 023", "RNTLY202600000023", 2025, 16300, 3150, 9000, "/rentaly/images/cars/bmw-m5.jpg", 5, 4, "Dizel"),
            FleetCar("Skoda", "Superb", "Sedan", "İzmir Adnan Menderes", "35 RNT 024", "RNTLY202600000024", 2026, 7200, 3400, 9500, "/rentaly/images/cars-alt/bmw-m5.png", 5, 4, "Benzin"),
            FleetCar("Honda", "Civic", "Sedan", "Kadıköy", "34 RNT 025", "RNTLY202600000025", 2025, 14900, 2850, 8000, "/rentaly/images/cars/lexus.jpg", 5, 3, "Benzin"),
            FleetCar("Fiat", "Egea", "Sedan", "Bursa Nilüfer", "16 RNT 026", "RNTLY202600000026", 2025, 22600, 2100, 6500, "/rentaly/images/cars-alt/bmw-m5.png", 5, 3, "Dizel"),
            FleetCar("Ford", "Focus", "Hatchback", "Antalya Havalimanı", "07 RNT 027", "RNTLY202600000027", 2025, 17100, 2600, 7500, "/rentaly/images/cars/bmw-m5.jpg", 5, 3, "Benzin"),

            FleetCar("Dacia", "Duster", "SUV", "İstanbul Havalimanı", "34 RNT 028", "RNTLY202600000028", 2025, 20500, 2750, 8000, "/rentaly/images/cars-alt/jeep-renegade.png", 5, 4, "Dizel"),
            FleetCar("Nissan", "Qashqai", "SUV", "İzmir Adnan Menderes", "35 RNT 029", "RNTLY202600000029", 2026, 8100, 3300, 9500, "/rentaly/images/cars/toyota-rav.jpg", 5, 4, "Hibrit"),
            FleetCar("Hyundai", "Tucson", "SUV", "Antalya Havalimanı", "07 RNT 030", "RNTLY202600000030", 2025, 13700, 3450, 10000, "/rentaly/images/cars-alt/toyota-rav.png", 5, 4, "Hibrit"),
            FleetCar("Kia", "Sportage", "SUV", "Bursa Nilüfer", "16 RNT 031", "RNTLY202600000031", 2026, 6200, 3500, 10000, "/rentaly/images/cars/jeep-renegade.jpg", 5, 4, "Hibrit"),
            FleetCar("Skoda", "Karoq", "SUV", "Ankara Esenboğa", "06 RNT 032", "RNTLY202600000032", 2025, 15600, 3350, 9500, "/rentaly/images/cars-alt/jeep-renegade.png", 5, 4, "Benzin"),
            FleetCar("Skoda", "Kodiaq", "SUV", "İstanbul Havalimanı", "34 RNT 033", "RNTLY202600000033", 2026, 5900, 3950, 11000, "/rentaly/images/cars/toyota-rav.jpg", 7, 5, "Dizel"),
            FleetCar("Toyota", "C-HR", "SUV", "Kadıköy", "34 RNT 034", "RNTLY202600000034", 2026, 6800, 3250, 9000, "/rentaly/images/cars-alt/toyota-rav.png", 5, 3, "Hibrit"),
            FleetCar("Peugeot", "3008", "SUV", "İzmir Adnan Menderes", "35 RNT 035", "RNTLY202600000035", 2025, 12100, 3550, 10000, "/rentaly/images/cars/jeep-renegade.jpg", 5, 4, "Dizel"),
            FleetCar("Renault", "Austral", "SUV", "Antalya Havalimanı", "07 RNT 036", "RNTLY202600000036", 2026, 5300, 3650, 10500, "/rentaly/images/cars-alt/toyota-rav.png", 5, 4, "Hibrit"),

            FleetCar("BMW", "320i", "Lüks", "Kadıköy", "34 RNT 037", "RNTLY202600000037", 2026, 6100, 4650, 14000, "/rentaly/images/cars/bmw-m5.jpg", 5, 3, "Benzin"),
            FleetCar("BMW", "520i", "Lüks", "İstanbul Havalimanı", "34 RNT 038", "RNTLY202600000038", 2026, 4800, 5450, 16000, "/rentaly/images/cars-alt/bmw-m5.png", 5, 4, "Benzin"),
            FleetCar("Mercedes-Benz", "C 200", "Lüks", "Ankara Esenboğa", "06 RNT 039", "RNTLY202600000039", 2026, 5700, 4900, 14500, "/rentaly/images/cars/lexus.jpg", 5, 3, "Hibrit"),
            FleetCar("Mercedes-Benz", "E 200", "Lüks", "İzmir Adnan Menderes", "35 RNT 040", "RNTLY202600000040", 2026, 3900, 5800, 17000, "/rentaly/images/cars-alt/lexus.png", 5, 4, "Hibrit"),
            FleetCar("Audi", "A4", "Lüks", "Antalya Havalimanı", "07 RNT 041", "RNTLY202600000041", 2025, 8900, 4700, 14000, "/rentaly/images/cars/bmw-m5.jpg", 5, 3, "Benzin"),
            FleetCar("Audi", "A6", "Lüks", "Bursa Nilüfer", "16 RNT 042", "RNTLY202600000042", 2026, 4200, 5600, 16500, "/rentaly/images/cars-alt/lexus.png", 5, 4, "Dizel"),
            FleetCar("BMW", "X1", "Lüks", "İstanbul Havalimanı", "34 RNT 043", "RNTLY202600000043", 2026, 5400, 4750, 14000, "/rentaly/images/cars/range-rover.jpg", 5, 4, "Benzin"),
            FleetCar("BMW", "X3", "Lüks", "Kadıköy", "34 RNT 044", "RNTLY202600000044", 2026, 3600, 5500, 16500, "/rentaly/images/cars-alt/range-rover.png", 5, 4, "Hibrit"),
            FleetCar("Mercedes-Benz", "GLC 200", "Lüks", "Ankara Esenboğa", "06 RNT 045", "RNTLY202600000045", 2026, 4600, 5900, 17500, "/rentaly/images/cars/lexus.jpg", 5, 4, "Hibrit"),
            FleetCar("Audi", "Q3", "Lüks", "İzmir Adnan Menderes", "35 RNT 046", "RNTLY202600000046", 2025, 9700, 4900, 14500, "/rentaly/images/cars-alt/lexus.png", 5, 4, "Benzin"),
            FleetCar("Audi", "Q5", "Lüks", "Antalya Havalimanı", "07 RNT 047", "RNTLY202600000047", 2026, 4100, 5850, 17500, "/rentaly/images/cars/range-rover.jpg", 5, 4, "Hibrit")
        };

        var definitionVins = definitions.Select(x => x.Vin).ToList();
        var existingCars = await context.Cars
            .Where(x => definitionVins.Contains(x.VIN))
            .ToDictionaryAsync(x => x.VIN);

        foreach (var definition in definitions)
        {
            if (existingCars.TryGetValue(definition.Vin, out var existingCar))
            {
                existingCar.ImageUrl = definition.ImageUrl;
                continue;
            }

            context.Cars.Add(new Car
            {
                BrandId = Brand(definition.BrandName).BrandId,
                ModelId = Model(definition.BrandName, definition.ModelName).CarModelId,
                CategoryId = Category(definition.CategoryName).CategoryId,
                BranchId = Branch(definition.BranchName).BranchId,
                PlateNumber = definition.Plate,
                VIN = definition.Vin,
                Year = definition.Year,
                Kilometer = definition.Kilometer,
                DailyPrice = definition.DailyPrice,
                DepositAmount = definition.Deposit,
                IsAvailable = true,
                IsActive = true,
                ImageUrl = definition.ImageUrl,
                SeatCount = definition.SeatCount,
                LuggageCount = definition.LuggageCount,
                FuelType = definition.FuelType
            });
        }

        await context.SaveChangesAsync();
    }

    private static FleetCarDefinition FleetCar(
        string brandName, string modelName, string categoryName, string branchName,
        string plate, string vin, int year, int kilometer, decimal dailyPrice, decimal deposit,
        string imageUrl, int seatCount, int luggageCount, string fuelType) =>
        new(brandName, modelName, categoryName, branchName, plate, vin, year, kilometer,
            dailyPrice, deposit, ModelImageUrl(brandName, modelName), seatCount, luggageCount, fuelType);

    private static string ModelImageUrl(string brandName, string modelName)
    {
        var slug = (brandName, modelName) switch
        {
            ("BMW", "M5") => "bmw-m5",
            ("Volkswagen", "Polo") => "volkswagen-polo",
            ("Toyota", "RAV4") => "toyota-rav4",
            ("Jeep", "Renegade") => "jeep-renegade",
            ("MINI", "Cooper") => "mini-cooper",
            ("Ford", "Raptor") => "ford-raptor",
            ("Hyundai", "Staria") => "hyundai-staria",
            ("Land Rover", "Range Rover Sport") => "range-rover-sport",
            ("Lexus", "RX") => "lexus-rx",
            ("Renault", "Clio") => "renault-clio",
            ("Hyundai", "i20") => "hyundai-i20",
            ("Peugeot", "208") => "peugeot-208",
            ("Opel", "Corsa") => "opel-corsa",
            ("Skoda", "Fabia") => "skoda-fabia",
            ("Dacia", "Sandero") => "dacia-sandero",
            ("SEAT", "Ibiza") => "seat-ibiza",
            ("Renault", "Megane Sedan") => "renault-megane-sedan",
            ("Toyota", "Corolla") => "toyota-corolla",
            ("Skoda", "Octavia") => "skoda-octavia",
            ("Volkswagen", "Passat") => "volkswagen-passat",
            ("Skoda", "Superb") => "skoda-superb",
            ("Honda", "Civic") => "honda-civic",
            ("Fiat", "Egea") => "fiat-egea",
            ("Ford", "Focus") => "ford-focus",
            ("Dacia", "Duster") => "dacia-duster",
            ("Nissan", "Qashqai") => "nissan-qashqai",
            ("Hyundai", "Tucson") => "hyundai-tucson",
            ("Kia", "Sportage") => "kia-sportage",
            ("Skoda", "Karoq") => "skoda-karoq",
            ("Skoda", "Kodiaq") => "skoda-kodiaq",
            ("Toyota", "C-HR") => "toyota-chr",
            ("Peugeot", "3008") => "peugeot-3008",
            ("Renault", "Austral") => "renault-austral",
            ("BMW", "320i") => "bmw-320i",
            ("BMW", "520i") => "bmw-520i",
            ("Mercedes-Benz", "C 200") => "mercedes-c200",
            ("Mercedes-Benz", "E 200") => "mercedes-e200",
            ("Audi", "A4") => "audi-a4",
            ("Audi", "A6") => "audi-a6",
            ("BMW", "X1") => "bmw-x1",
            ("BMW", "X3") => "bmw-x3",
            ("Mercedes-Benz", "GLC 200") => "mercedes-glc200",
            ("Audi", "Q3") => "audi-q3",
            ("Audi", "Q5") => "audi-q5",
            _ => throw new InvalidOperationException($"Araç görsel eşleşmesi bulunamadı: {brandName} {modelName}")
        };

        return $"/rentaly/images/cars-models/{slug}.jpg";
    }

    private sealed record FleetCarDefinition(
        string BrandName,
        string ModelName,
        string CategoryName,
        string BranchName,
        string Plate,
        string Vin,
        int Year,
        int Kilometer,
        decimal DailyPrice,
        decimal Deposit,
        string ImageUrl,
        int SeatCount,
        int LuggageCount,
        string FuelType);
}
