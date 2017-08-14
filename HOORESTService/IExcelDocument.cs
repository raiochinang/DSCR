using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOORESTService
{
    public interface IExcelDocument
    {
        /// <summary>
        /// Reads a value of a spreadsheet cell
        /// </summary>
        /// <param name="sheetName">Name of the spreadsheet</param>
        /// <param name="cellCoordinates">Cell coordinates e.g. A1</param>
        /// <returns>Value of the specified cell</returns>
        CellValue ReadCell(string sheetName, string cellCoordinates);

        /// <summary>
        /// Updates a value of a spreadsheet cell
        /// </summary>
        /// <param name="sheetName">Name of the spreadsheet</param>
        /// <param name="cellCoordinates">Cell coordinates e.g. A1</param>
        /// <param name="cellValue">New cell value</param>
        void UpdateCell(string sheetName, string cellCoordinates, object cellValue);

        /// <summary>
        /// Refreshes the workbook to recalculate all formula cell values
        /// </summary>
        //void Refresh();
    }
}
