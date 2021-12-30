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
        
        public int VisitorID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Age { get; set; }
        public DateTime Date { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public string Groupno { get; set; }
        public string Price { get; set; }

        private string filePath = "VisitorInformation.json";

        public void Add(Visitors v)
        { 
            Random r = new Random();
            v.VisitorID = r.Next(0, 1000000);
            string data = JsonConvert.SerializeObject(v, Formatting.None);
            Tools.WriteToTextFile(filePath,data);
        }
        public List<Visitors> List()
        {
            string file = Tools.ReadFromTextFile(filePath);
            if (file != null)
            {
                List<Visitors> visitorsList = JsonConvert.DeserializeObject<List<Visitors>>(file);
                return visitorsList;
            }
            return null;
        }
        public void Edit(Visitors visitorsDetails)
        {
            List<Visitors> visitorsList = List();
            Visitors visitor = visitorsList.Where(editingVisitor => editingVisitor.VisitorID == visitorsDetails.VisitorID).FirstOrDefault();
            visitorsList.Remove(visitor);
            visitorsList.Add(visitorsDetails);
            string visitorInfo = JsonConvert.SerializeObject(visitorsList, Formatting.None);
            Tools.WriteToTextFile(filePath, visitorInfo, false);
        }

    }
}
