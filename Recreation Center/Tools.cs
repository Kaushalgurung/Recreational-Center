using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace Recreation_Center
{
    public static class Tools
    {
        public static string _filepath = "PriceInformation.json";

        public static string WriteToText(string data) // for saving price information
        {
            if (!File.Exists(_filepath))
            {
                using (File.Create(_filepath))
                {
                    ;
                }
            }
            using (StreamWriter outputFile = new StreamWriter(_filepath))
            {
                outputFile.WriteLine(data);
            }
            return "Price saved successfully!";
        }
        public static void WriteToTextFile(string fileName, string visitorInfo, bool append = true, int count = 1)
        {
            if (!File.Exists(fileName))
            {
                var file = File.Create(fileName);
                file.Close();

            }
            using (StreamWriter streamWriter = new StreamWriter(fileName, append: append))
            {
                if (!append)
                {
                    visitorInfo = visitorInfo.Trim().Substring(1, visitorInfo.Trim().Length - 1);
                    visitorInfo = visitorInfo.Trim().Substring(0, visitorInfo.Trim().Length - 1);

                }
                if (count != 0)
                {
                    visitorInfo = visitorInfo + ",";
                }
                streamWriter.WriteLine(visitorInfo);
            }
        }
        public static string ReadFromTextFileMenu(string fileName)
        {
            if (File.Exists(fileName))
            {
                string info;
                using (StreamReader r = new StreamReader(fileName))
                {
                    info = r.ReadToEnd();

                }
                return info;

            }
            return null;
        }
        public static string ReadFromTextFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                string info;
                using (StreamReader r = new StreamReader(fileName))
                {
                    info = r.ReadToEnd();

                }
                if (info != "")
                {
                    info = "[" + info + "]";

                }
                return info;

            }
            return null;
        }
        public static DataTable ToTable<T>(IList<T> list)
        {
            PropertyDescriptorCollection propertyDescriptor = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor i in propertyDescriptor)
                table.Columns.Add(i.Name, Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType);

            if (list != null)
            {
                foreach (T j in list)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor k in propertyDescriptor)
                        row[k.Name] = k.GetValue(j) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            return table;

        }
        public static DataTable ToTableSingle<T>(T list)
        {
            PropertyDescriptorCollection propertyDescriptor = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor i in propertyDescriptor)
                table.Columns.Add(i.Name, Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType);

            if (list != null)
            {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor k in propertyDescriptor)
                        row[k.Name] = k.GetValue(list) ?? DBNull.Value;
                    table.Rows.Add(row);
                
            }
            return table;

        }

    }
}
