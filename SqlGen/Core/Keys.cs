using SqlGen.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SqlGen
{
    public abstract class TableKey : IEnumerable<Column>
    {
        public string ConstraintName { get; set; }
        public abstract IEnumerator<Column> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PrimaryKey : TableKey
    {
        public List<Column> KeyColumns { get; set; }
        public override IEnumerator<Column> GetEnumerator() => KeyColumns.GetEnumerator();
    }

    public class ForeignKey : TableKey
    {
        public string ReferancedTableName { get; set; }
        public List<Column> TableColumns { get; set; }
        public override IEnumerator<Column> GetEnumerator() => TableColumns.GetEnumerator();
    }

    class KeyColumn : Column
    {
        public string ConstraintName { get; set; }

    }
}
