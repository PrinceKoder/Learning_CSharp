using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Lesson0
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Factory
            Factory factory1 = new Factory() { Id = 1, Name = "НПЗ#1", Description = "Первый нефтеперерабатывающий завод" };
            Factory factory2 = new Factory() { Id = 2, Name = "НПЗ#2", Description = "Второй нефтеперерабатывающий завод" };
            #endregion

            #region Unit
            Unit unit1 = new Unit() { Id = 1, Name = "ГФУ", FactoryId = 1 };
            Unit unit2 = new Unit() { Id = 2, Name = "АВТ - 6", FactoryId = 1 };
            Unit unit3 = new Unit() { Id = 3, Name = "АВТ - 10", FactoryId = 2 };
            #endregion

            #region Tank
            Tank tank1 = new Tank() { Id = 1, Name = "Резервуар 1", Volume = 1500, MaxVolume = 2000, UnitId = 1};
            Tank tank2 = new Tank() { Id = 2, Name = "Резервуар 2", Volume = 2500, MaxVolume = 3000, UnitId = 1 };
            Tank tank3 = new Tank() { Id = 3, Name = "Дополнительный резервуар 24", Volume = 3000, MaxVolume = 3000, UnitId = 2 };
            Tank tank4 = new Tank() { Id = 4, Name = "Резервуар 35", Volume = 3000, MaxVolume = 3000, UnitId = 2 };
            Tank tank5 = new Tank() { Id = 5, Name = "Резервуар 47", Volume = 4000, MaxVolume = 5000, UnitId = 2 };
            Tank tank6 = new Tank() { Id = 6, Name = "Резервуар 256", Volume = 500, MaxVolume = 500, UnitId = 3 };
            #endregion

            #region MakeJsonFiles
            string jsFilesPath = @"D:\C#_Projects\Learning_Csharp\Lessons\Lesson0\JSON_files";
            string jsonString = string.Empty;

            JsonSerializerOptions jsonSO = new JsonSerializerOptions() 
            { 
                WriteIndented = true, 
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) 
            };

            #region factory
            jsonString = JsonSerializer.Serialize(factory1, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "factory1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(factory2, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "factory2.json"), jsonString);
            #endregion

            #region unit
            jsonString = JsonSerializer.Serialize(unit1, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "unit1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(unit2, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "unit2.json"), jsonString);

            jsonString = JsonSerializer.Serialize(unit3, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "unit3.json"), jsonString);
            #endregion

            #region tank
            jsonString = JsonSerializer.Serialize(tank1, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank1.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank2, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank2.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank3, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank3.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank4, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank4.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank5, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank5.json"), jsonString);

            jsonString = JsonSerializer.Serialize(tank6, jsonSO);
            File.WriteAllText(Path.Combine(jsFilesPath, "tank6.json"), jsonString);
            #endregion

            #endregion

            #region ReadJsonFiles

            #region factory
            Factory factory1_dsr = JsonSerializer.Deserialize<Factory>(File.ReadAllText(Path.Combine(jsFilesPath, "factory1.json")));
            Factory factory2_dsr = JsonSerializer.Deserialize<Factory>(File.ReadAllText(Path.Combine(jsFilesPath, "factory2.json")));

            #endregion

            #region unit
            Unit unit1_dsr = JsonSerializer.Deserialize<Unit>(File.ReadAllText(Path.Combine(jsFilesPath, "unit1.json")));
            Unit unit2_dsr = JsonSerializer.Deserialize<Unit>(File.ReadAllText(Path.Combine(jsFilesPath, "unit2.json")));
            Unit unit3_dsr = JsonSerializer.Deserialize<Unit>(File.ReadAllText(Path.Combine(jsFilesPath, "unit3.json")));
            #endregion

            #region tank

            #endregion
            Tank tank1_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank1.json")));
            Tank tank2_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank2.json")));
            Tank tank3_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank3.json")));
            Tank tank4_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank4.json")));
            Tank tank5_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank5.json")));
            Tank tank6_dsr = JsonSerializer.Deserialize<Tank>(File.ReadAllText(Path.Combine(jsFilesPath, "tank6.json")));

            #endregion
        }
    }
}
