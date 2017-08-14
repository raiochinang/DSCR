using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HOORESTService
{
    public class ExcelDocument : IExcelDocument
    {
        private readonly string _filePath;

        public ExcelDocument(string filePath)
        {
            _filePath = filePath;
        }

        /// <see cref="IExcelDocument.ReadCell" />
        public CellValue ReadCell(string sheetName, string cellCoordinates)
        {
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, false))
            {
                Cell cell = GetCell(excelDoc, sheetName, cellCoordinates);
                return cell.CellValue;
            }
        }

        /// <see cref="IExcelDocument.UpdateCell" />
        public void UpdateCell(string sheetName, string cellCoordinates, object cellValue)
        {
            using (SpreadsheetDocument excelDoc = SpreadsheetDocument.Open(_filePath, true))
            {
                // tell Excel to recalculate formulas next time it opens the doc
                excelDoc.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                excelDoc.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;

                WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
                Cell cell = GetCell(worksheetPart, cellCoordinates);
                cell.CellValue = new CellValue(cellValue.ToString());
                worksheetPart.Worksheet.Save();
            }
        }

        /// <summary>Refreshes an Excel document by opening it and closing in background by the Excep Application</summary>
        /// <see cref="IExcelDocument.Refresh" />
        //public void Refresh()
        //{
        //    var excelApp = new Application();
        //    Workbook workbook = excelApp.Workbooks.Open(Path.GetFullPath(_filePath));
        //    workbook.Close(true);
        //    excelApp.Quit();
        //}

        private WorksheetPart GetWorksheetPart(SpreadsheetDocument excelDoc, string sheetName)
        {
            Sheet sheet = excelDoc.WorkbookPart.Workbook.Descendants<Sheet>().SingleOrDefault(s => s.Name == sheetName);
            if (sheet == null)
            {
                throw new ArgumentException(
                    String.Format("No sheet named {0} found in spreadsheet {1}", sheetName, _filePath), "sheetName");
            }
            return (WorksheetPart)excelDoc.WorkbookPart.GetPartById(sheet.Id);
        }

        private Cell GetCell(SpreadsheetDocument excelDoc, string sheetName, string cellCoordinates)
        {
            WorksheetPart worksheetPart = GetWorksheetPart(excelDoc, sheetName);
            return GetCell(worksheetPart, cellCoordinates);
        }

        private Cell GetCell(WorksheetPart worksheetPart, string cellCoordinates)
        {
            int rowIndex = int.Parse(cellCoordinates.Substring(1));
            Row row = GetRow(worksheetPart, rowIndex);

            Cell cell = row.Elements<Cell>().FirstOrDefault(c => cellCoordinates.Equals(c.CellReference.Value));
            if (cell == null)
            {
                throw new ArgumentException(String.Format("Cell {0} not found in spreadsheet", cellCoordinates));
            }
            return cell;
        }

        private Row GetRow(WorksheetPart worksheetPart, int rowIndex)
        {
            Row row = worksheetPart.Worksheet.GetFirstChild<SheetData>().
                                    Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                throw new ArgumentException(String.Format("No row with index {0} found in spreadsheet", rowIndex));
            }
            return row;
        }
    }
}