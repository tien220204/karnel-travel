using System.Data;
using ExcelDataReader;

namespace KarnelTravel.Share.Utilities.Excel
{
    public static class ExcelReader
    {
        /// <summary>
        /// Reads the first sheet of an Excel file from a stream, and writes it into a List of rows with a List of columns
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="includesHeader"></param>
        /// <returns></returns>
        public static List<List<string>> ReadExcelWorksheet(Stream stream, int rowStart = 0, bool includesHeader = true)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            DataTableCollection dataTables = reader.AsDataSet().Tables;

            var rows = new List<List<string>>();
            if (dataTables.Count == 0) return rows;

            DataTable table = dataTables[0];
            for (int rowIndex = rowStart; rowIndex < table.Rows.Count; rowIndex++)
            {
                var row = new List<string>();
                for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    row.Add(table.Rows[rowIndex][columnIndex].ToString());
                }

                rows.Add(row);
            }

            return rows;
        }

        /// <summary>
        /// Reads an entire Excel file from a stream, and writes it into a List of Worksheets with SheetName and List rows with a List of columns
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="includesHeader"></param>
        /// <returns></returns>
        public static List<(string, List<List<string>>)> ReadExcelWorkbook(Stream stream, bool includesHeader = true)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            DataTableCollection dataTables = reader.AsDataSet().Tables;

            var sheets = new List<(string, List<List<string>>)>();
            if (dataTables.Count == 0) return sheets;

            foreach (DataTable table in dataTables)
            {
                var rows = new List<List<string>>();

                for (int rowIndex = includesHeader ? 1 : 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    var row = new List<string>();
                    for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                    {
                        row.Add(table.Rows[rowIndex][columnIndex].ToString());
                    }

                    rows.Add(row);
                }

                sheets.Add((table.TableName, rows));
            }

            return sheets;
        }

        /// <summary>
        /// Reads the first sheet of an Excel file from a stream, checks the header, and writes it into a List of rows with a List of columns
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expectedHeader">List of expected column names</param>
        /// <param name="rowStart"></param>
        /// <param name="includesHeader"></param>
        /// <returns></returns>
        public static (List<List<string>> rows, bool isHeaderValid, string errorMessage) ReadExcelWorksheet(Stream stream, List<string> expectedHeader, int rowStart = 0, bool includesHeader = true)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
            DataTableCollection dataTables = reader.AsDataSet().Tables;

            var rows = new List<List<string>>();
            if (dataTables.Count == 0) return (rows, false, "No sheets found in the Excel file.");

            DataTable table = dataTables[0];

            if (includesHeader && expectedHeader != null)
            {
                // Check header
                var headerRow = table.Rows[0];
                if (headerRow.ItemArray.Length != expectedHeader.Count)
                {
                    return (rows, false, $"Header column count does not match expected count. Found {headerRow.ItemArray.Length}, expected {expectedHeader.Count}.");
                }

                for (int columnIndex = 0; columnIndex < headerRow.ItemArray.Length; columnIndex++)
                {
                    if (headerRow[columnIndex].ToString() != expectedHeader[columnIndex])
                    {
                        return (rows, false, $"Header column name does not match at index {columnIndex}. Found '{headerRow[columnIndex]}', expected '{expectedHeader[columnIndex]}'.");
                    }
                }

                // Skip the header row if includesHeader is true
                rowStart = 1;
            }

            for (int rowIndex = rowStart; rowIndex < table.Rows.Count; rowIndex++)
            {
                var row = new List<string>();
                for (var columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    row.Add(table.Rows[rowIndex][columnIndex].ToString());
                }

                rows.Add(row);
            }

            return (rows, true, string.Empty);
        }
    }
}
