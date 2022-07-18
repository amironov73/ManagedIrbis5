

namespace GridExtra.Avalonia
{
    public class NamedAreaDefinition : AreaDefinition
    {
        public string Name { get; set; }

        public NamedAreaDefinition(string name, int row, int column, int rowSpan, int columnSpan)
            : base(row, column, rowSpan, columnSpan)
        {
            this.Name = name;
        }
    }



}