using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shinrai_Client_Launcher
{
    class Translateable
    {
        public static string SelectedJson;
        private string json;
        private Dictionary<string, string> data;

        public void LoadJson(string JsonPath)
        {
            if (!File.Exists(JsonPath))
            {
                Console.WriteLine("JSON dosyası bulunamadı!");
                return;
            }

            SelectedJson = JsonPath;
            json = File.ReadAllText(SelectedJson);

            try
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("JSON parse hatası: " + ex.Message);
            }
        }

        public string TranslatableText(string ID)
        {
            if (data == null)
            {
                return "[Dil dosyası yüklenmedi]";
            }

            if (data.TryGetValue(ID, out var value))
            {
                return value;
            }

            return $"[{ID}]"; // Anahtar bulunamazsa fallback
        }
    }
}
