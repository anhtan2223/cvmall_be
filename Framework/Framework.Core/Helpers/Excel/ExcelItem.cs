namespace Framework.Core.Helpers.Excel
{
    public class ExcelItem
    {
        public string key { get; set; }
        public string header { get; set; }
        public double? width { get; set; }
        public DataType? type { get; set; } = DataType.TEXT;
        public CellAlign? header_align { get; set; } = CellAlign.CENTER;
        public CellAlign? content_align { get; set; } = CellAlign.LEFT;
    }
}
