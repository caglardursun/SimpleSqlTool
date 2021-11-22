using System.Collections.Generic;

namespace SqlGen
{
    public class FkModel
    {
        public string TableName { get; set; }
        public Column FKey { get; set; }
        public List<Column> columns { get; set; }
    }
}
