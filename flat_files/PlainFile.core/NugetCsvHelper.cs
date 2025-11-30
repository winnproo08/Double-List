using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainFile.core
{
    public class NugetCsvHelper
    {
        public IEnumerable<Person> Read(string path)
        {
            using var sr = new StreamReader(path);
            using var cr = new CsvReader(sr, CultureInfo.InvariantCulture);
            return cr.GetRecords<Person>().ToList();
        }

        public void Write(string path, IEnumerable<Person> people)
        {
            using var sw = new StreamWriter(path);
            using var cw = new CsvWriter(sw, CultureInfo.InvariantCulture);
            cw.WriteRecords(people);
        }
    }
}
