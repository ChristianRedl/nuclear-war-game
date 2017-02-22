using SimpleNuclearWarGameplayDemo.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNuclearWarGameplayDemo.Data
{
    public static class Serializer
    {
        public static FileInfo Filename = new FileInfo("gameset.json");

        public static void SaveAll(GameSet gameSet)
        {
            
            Save(Filename, gameSet);
            //            Save(new FileInfo("world.json"), gameSet.World);
            //            Save(new FileInfo("terror.json"), gameSet.RandomEvents);
            //            foreach (Nation nation in gameSet.Nations)
            //            {
            //                Save(new FileInfo(nation.Name + ".json"), nation);
            //            }
        }

        public static void Save(FileInfo file, Object obj)
        {
            var json = new System.Web.Script.Serialization.JavaScriptSerializer();
            string text = json.Serialize(obj).Replace("\\u003c", "<").Replace("\\u003e", ">");
            File.WriteAllText(file.FullName, text);
        }

        public static GameSet Load(FileInfo file)
        {
            try
            {
                if (file.Exists)
                {
                    var jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return jsonSerializer.Deserialize<GameSet>(File.ReadAllText(file.FullName));
                }
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return null;
        }
    }
}
