using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recreation_Center
{
    public class Visitors
    {
        public string VisitorID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Age { get; set; }
        public string Date { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public string Groupno { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }

        private string filePath = "VisitorInformation.json";

        public void Add(Visitors v)
        {
            string data = JsonConvert.SerializeObject(v, Formatting.None);
            Tools.WriteToTextFile(filePath,data);
        }

    }
}
