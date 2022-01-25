using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lesson0
{
    class Program
    {
        //public static string jsonFilesPath = @"D:\C#_Projects\Learning_Csharp\Lessons\Lesson0\JSON_files";    // правим под свои реалии, это папка для складирования и считывания JSON файлов
        public static string jsonFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON_files"); // относительный путь в папку с билдом
        public static JsonSerializerOptions jsonSO = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        static void Main(string[] args)
        {
            //PrepareJsonFiles();

            var factories = GetFactories();
            var units = GetUnits();
            var tanks = GetTanks();

            if (ExportAllDataToJSON(units, factories, tanks))
            {
                Console.WriteLine($"Экспорт в единый JSON-файл прошёл успешно");
            }

            Console.WriteLine($"Количество заводов: {factories.Length}.");
            Console.WriteLine($"Количество установок: {units.Length}.");
            Console.WriteLine($"Количество резервуаров: {tanks.Length}.");

            string tankName = "Резервуар 2";
            var foundUnit = FindUnit(tanks, units, tankName);
            if (foundUnit != null)
            {
                var factory = FindFactory(factories, foundUnit);
                Console.WriteLine($"{tankName} принадлежит установке: {foundUnit.Name} и заводу: {factory?.Name}");
            }
            else
            {
                Console.WriteLine($"Не удалось найти установку с таким именем резервуара.");
            }



            var totalVolume = GetTotalVolume(tanks);
            var usedVolume = GetUsedVolume(tanks);
            Console.WriteLine($"Общий объем резервуаров: {totalVolume}. Занятый объем: {usedVolume}. Свободный объём: {totalVolume - usedVolume}.");

            GetInfoForAllTanks(tanks, units, factories);

            FindByInputOffer(units, factories, tanks);
        }

        /// <summary>
        /// Получить все резервуары
        /// </summary>        
        public static Tank[] GetTanks()
        {
            // ваш код здесь
            List<Tank> tankList = new List<Tank>();
            foreach (string jsonFilePath in Directory.GetFiles(jsonFilesPath, "tank*"))
            {
                tankList.Add(
                    JsonSerializer.Deserialize<Tank>(File.ReadAllText(jsonFilePath))
                    );
            }
            return tankList.ToArray();
        }
        /// <summary>
        /// Получить все установки
        /// </summary>  
        public static Unit[] GetUnits()
        {
            // ваш код здесь
            List<Unit> unitList = new List<Unit>();
            foreach (string jsonFilePath in Directory.GetFiles(jsonFilesPath, "unit*"))
            {
                unitList.Add(
                    JsonSerializer.Deserialize<Unit>(File.ReadAllText(jsonFilePath))
                    );
            }
            return unitList.ToArray();
        }
        /// <summary>
        /// Получить все заводы
        /// </summary>  
        public static Factory[] GetFactories()
        {
            // ваш код здесь
            List<Factory> factoryList = new List<Factory>();
            foreach (string jsonFilePath in Directory.GetFiles(jsonFilesPath, "factory*"))
            {
                factoryList.Add(
                    JsonSerializer.Deserialize<Factory>(File.ReadAllText(jsonFilePath))
                    );
            }
            return factoryList.ToArray();
        }

        /// <summary>
        /// Получить установку, по имени принадлежащего ей резервуара
        /// </summary>  
        public static Unit FindUnit(Tank[] tanks, Unit[] units, string tankName)
        {
            // ваш код здесь
            Tank tank = tanks.FirstOrDefault(x => string.Equals(x.Name, tankName, StringComparison.OrdinalIgnoreCase));
            return units.FirstOrDefault(x => x.Id == tank?.UnitId);
        }

        /// <summary>
        /// Получить завод, по принадлежащей ему установке
        /// </summary> 
        public static Factory FindFactory(Factory[] factories, Unit unit)
        {
            // ваш код здесь
            return factories.FirstOrDefault(x => x.Id == unit.FactoryId);
        }

        /// <summary>
        /// Получить суммарный объем резервуаров
        /// </summary> 
        public static int GetTotalVolume(Tank[] tanks)
        {
            // ваш код здесь
            return tanks.Sum(x => x.MaxVolume);
        }

        /// <summary>
        /// Получить общий занятый объем в резервуарах
        /// </summary>
        public static int GetUsedVolume(Tank[] tanks)
        {
            // ваш код здесь
            return tanks.Sum(x => x.Volume);
        }

        /// <summary>
        /// Получить все резервуары, с именами установок и заводов, к которым они принадлежат
        /// </summary>        
        public static void GetInfoForAllTanks(Tank[] tanks, Unit[] units, Factory[] factories)
        {
            // ваш код здесь
            var infoForAllTanks = from tank in tanks
                                  join unit in units on tank.UnitId equals unit.Id
                                  join factory in factories on unit.FactoryId equals factory.Id
                                  select new { tankName = tank.Name, unitName = unit.Name, factoryName = factory.Name };

            foreach (var tankInfo in infoForAllTanks)
            {
                Console.WriteLine($"Название: {tankInfo.tankName}. Для установки: {tankInfo.unitName}. На заводе: {tankInfo.factoryName}");
            }
        }

        #region Задача №6
        /// <summary>
        /// * Осуществить возможность поиска по наименованию в коллекции, например через ввод в консоли
        /// </summary>
        public static void FindByInputOffer(Unit[] units, Factory[] factories, Tank[] tanks)
        {
            Console.WriteLine("-------------");
            Console.WriteLine("-------------");
            while (true)
            {
                Console.WriteLine($"Желаете поискать элементы вручную?(Y) или завершить работу программы?(N)");
                var pressedKey = Console.ReadKey(true).Key;
                if (pressedKey == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    SelectEntityForSearching(units, factories, tanks);
                }
                else if (pressedKey == ConsoleKey.N)
                {
                    Console.WriteLine();
                    Console.WriteLine("Завершаем работу программы.");
                    Thread.Sleep(3000);
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Команда не распознана. Попробуйте ещё раз.");
                }
            }

        }
        public static bool FindTankByInput(Tank[] tanks)
        {
            bool res = false;
            try
            {
                Console.WriteLine("Введите имя резервуара:");
                string typedName = Console.ReadLine();

                var tank = tanks.FirstOrDefault(x => string.Equals(x.Name, typedName,StringComparison.OrdinalIgnoreCase));
                if (tank is null)
                {

                    Console.WriteLine($"Точных совпадений не найдено.");
                }
                else
                {

                    Console.WriteLine($"Есть точное совпадение!");
                    Console.WriteLine($"Название: {tank.Name}, Общий объём: {tank.MaxVolume}, Свободный объём: {tank.MaxVolume - tank.Volume}.");
                }

                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"Ошибка при поиске совпадения: {ex.Message}");
            }

            return res;
        }
        public static bool FindUnitByInput(Unit[] units, Factory[] factories)
        {
            bool res = false;
            try
            {
                Console.WriteLine("Введите имя установки:");
                string typedName = Console.ReadLine();
                var unit = units.FirstOrDefault(x => string.Equals(x.Name, typedName, StringComparison.OrdinalIgnoreCase));

                if (unit is null)
                {
                    Console.WriteLine($"Точных совпадений не найдено.");
                }
                else
                {
                    Console.WriteLine($"Есть точное совпадение!");
                    Console.WriteLine($"Название: {unit.Name}, Завод-владелец: {factories.FirstOrDefault(x => x.Id == unit.FactoryId).Name}.");
                }

                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"Ошибка при поиске совпадения: {ex.Message}");
            }

            return res;
        }
        public static bool FindFactoryByInput(Unit[] units, Factory[] factories)
        {
            bool res = false;
            try
            {
                Console.WriteLine("Введите имя завода:");
                string typedName = Console.ReadLine();
                var factory = factories.FirstOrDefault(x => string.Equals(x.Name, typedName, StringComparison.OrdinalIgnoreCase));

                if (factory is null)
                {
                    Console.WriteLine($"Точных совпадений не найдено.");
                }
                else
                {
                    Console.WriteLine($"Есть точное совпадение!");
                    Console.WriteLine($"Название: {factory.Name}, Полное наименование: {factory.Description}, Всего установок: {units.Count(x => x.FactoryId == factory.Id)}.");
                }

                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                Console.WriteLine($"Ошибка при поиске совпадения: {ex.Message}");
            }

            return res;
        }
        public enum EntitiesForSearching
        {
            Factory = 1,
            Unit = 2,
            Tank = 3
        }
        public static void SelectEntityForSearching(Unit[] units, Factory[] factories, Tank[] tanks)
        {            
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Выберите тип элемента для поиска:");
                Console.WriteLine($"Завод: {(int)EntitiesForSearching.Factory}.");
                Console.WriteLine($"Установка: {(int)EntitiesForSearching.Unit}.");
                Console.WriteLine($"Резервуар: {(int)EntitiesForSearching.Tank}.");
                Console.WriteLine("Выйти в меню: 0");

                int selectedTypeNumber;
                bool parseResult = Int32.TryParse(Console.ReadKey(true).KeyChar.ToString(), out selectedTypeNumber);
                Console.WriteLine();

                if (!parseResult)
                {
                    Console.WriteLine("Команда не распознана. Попробуйте ещё раз.");
                    continue;
                }
                else
                {
                    switch (selectedTypeNumber)
                    {
                        case (int)EntitiesForSearching.Factory:
                            FindFactoryByInput(units, factories);
                            break;

                        case (int)EntitiesForSearching.Unit:
                            FindUnitByInput(units, factories);
                            break;

                        case (int)EntitiesForSearching.Tank:
                            FindTankByInput(tanks);
                            break;

                        case 0:                            
                            Console.WriteLine("Выход в меню.");
                            Console.WriteLine();
                            return;
                            

                        default:                           
                            Console.WriteLine("Такого варианта нет. Попробуйте ещё раз.");
                            continue;
                            
                    }                    
                }
            }
           
        }

        #endregion

        #region Задача №7
        /// <summary>
        /// ** Придумать структуру и выгрузить все объекты в json файл
        /// </summary>
        public static bool ExportAllDataToJSON(Unit[] units, Factory[] factories, Tank[] tanks)
        {
            bool res = false;
            try
            {
                List<ExportToJSON> export = new List<ExportToJSON>();

                export.Add(new ExportToJSON
                {
                    factoryJSON = factories.Select(f => new FactoryJSON
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Description = f.Description,
                        unitJSON =
                                               units.Where(x => x.FactoryId == f.Id)
                                               .Select(y => new UnitJSON
                                               {
                                                   Id = y.Id,
                                                   Name = y.Name,
                                                   FactoryId = y.FactoryId,
                                                   tank =
                                                           tanks.Where(z => z.UnitId == y.Id).ToArray()
                                               }).ToArray()
                    }).ToArray()
                });

                File.WriteAllText(Path.Combine(jsonFilesPath, "ExportToJSON.json"), JsonSerializer.Serialize(export, jsonSO));
                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при экспорте в единый JSON-файл: {ex.Message}");
                res = false;
            }

            return res;
        }

        #endregion

        /// <summary>
        /// Подготовить и сохранть в JSON файлы по таблицам
        /// </summary>
        public static void PrepareJsonFiles()
        {
            #region Factory
            Factory factory1 = new Factory() { Id = 1, Name = "НПЗ#1", Description = "Первый нефтеперерабатывающий завод" };
            Factory factory2 = new Factory() { Id = 2, Name = "НПЗ#2", Description = "Второй нефтеперерабатывающий завод" };
            #endregion

            #region Unit
            Unit unit1 = new Unit() { Id = 1, Name = "ГФУ", FactoryId = 1 };
            Unit unit2 = new Unit() { Id = 2, Name = "АВТ-6", FactoryId = 1 };
            Unit unit3 = new Unit() { Id = 3, Name = "АВТ-10", FactoryId = 2 };
            #endregion

            #region Tank
            Tank tank1 = new Tank() { Id = 1, Name = "Резервуар 1", Volume = 1500, MaxVolume = 2000, UnitId = 1 };
            Tank tank2 = new Tank() { Id = 2, Name = "Резервуар 2", Volume = 2500, MaxVolume = 3000, UnitId = 1 };
            Tank tank3 = new Tank() { Id = 3, Name = "Дополнительный резервуар 24", Volume = 3000, MaxVolume = 3000, UnitId = 2 };
            Tank tank4 = new Tank() { Id = 4, Name = "Резервуар 35", Volume = 3000, MaxVolume = 3000, UnitId = 2 };
            Tank tank5 = new Tank() { Id = 5, Name = "Резервуар 47", Volume = 4000, MaxVolume = 5000, UnitId = 2 };
            Tank tank6 = new Tank() { Id = 6, Name = "Резервуар 256", Volume = 500, MaxVolume = 500, UnitId = 3 };
            #endregion

            #region MakeJsonFiles
            //string jsFilesPath = @"D:\C#_Projects\Learning_Csharp\Lessons\Lesson0\JSON_files";
            string jsonString = string.Empty;

            //JsonSerializerOptions jsonSO = new JsonSerializerOptions()
            //{
            //    WriteIndented = true,
            //    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //};

            #region factory
            jsonString = JsonSerializer.Serialize(factory1, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "factory1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(factory2, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "factory2.json"), jsonString);
            #endregion

            #region unit
            jsonString = JsonSerializer.Serialize(unit1, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "unit1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(unit2, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "unit2.json"), jsonString);

            jsonString = JsonSerializer.Serialize(unit3, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "unit3.json"), jsonString);
            #endregion

            #region tank
            jsonString = JsonSerializer.Serialize(tank1, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank2, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank2.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank3, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank3.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank4, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank4.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank5, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank5.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank6, jsonSO);
            File.WriteAllText(Path.Combine(jsonFilesPath, "tank6.json"), jsonString);
            #endregion

            #endregion


        }
    }
}
