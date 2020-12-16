using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Table
{
    public class DB_Table
    {
        public Person person { get; set; }
        public Production production { get; set; }
        public Sales sales { get; set; }
        public DBTable() { }

        public DBTable(Person person, Production production, Sales sales)
        {
            this.person = person;
            this.production = production;
            this.sales = sales;
        }
    }
}