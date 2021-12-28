using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recreation_Center
{
    public class Price
    {
        public string child1to2hr { get; set; }
        public string child2to4hr { get; set; }
        public string child4to6hrs { get; set; }
        public string childwholeday { get; set; }
        public string youth1to2hrs { get; set; }
        public string youth2to4hrs { get; set; }
        public string youth4to6hrs { get; set; }
        public string youthwholeday { get; set; }
        public string adult1to2hrs { get; set; }
        public string adult2to4hrs { get; set; }
        public string adult4to6hrs{ get; set; }
        public string adultwholeday { get; set; }
        public string senior1to2hrs { get; set; }
        public string senior2to4hrs { get; set; }
        public string senior4to6hrs { get; set; }
        public string seniorwholeday { get; set; }
        public string group1to5 { get; set; }
        public string group5to10 { get; set; }
        public string groupover10 { get; set; }
        public string weekdays { get; set; }
        public string weekend { get; set; }
        
        private string filePath = "PriceInformation.json";

        public void Add (Price p)
        {
            string data = JsonConvert.SerializeObject(p, Formatting.None);
            Tools.WriteToText(data);
        }
        public List<Price> List()
        {
            string file = Tools.ReadFromTextFile(filePath);
            if (file != null)
            {
                List<Price> priceList = JsonConvert.DeserializeObject<List<Price>>(file);
                return priceList;
            }
            return null;
        }
    }
}
