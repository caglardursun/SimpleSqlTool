using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlGen
{
    public class FkModel
    {
        public string TableName { get; set; }
        public Column FKey { get; set; }
        public List<Column> columns { get; set; }
    }
}
