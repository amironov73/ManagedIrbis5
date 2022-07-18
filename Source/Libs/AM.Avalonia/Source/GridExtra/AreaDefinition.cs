

namespace GridExtra.Avalonia
{
    public class AreaDefinition
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }

        public AreaDefinition(int row, int column, int rowSpan, int columnSpan)
        {
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
        }
    }
}