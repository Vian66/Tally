using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Tally.Models
{
    public class NpoiExcelImportHelper
    {
        private static NpoiExcelImportHelper _excelImportHelper;

        public static NpoiExcelImportHelper _
        {
            get => _excelImportHelper ?? (_excelImportHelper = new NpoiExcelImportHelper());
            set => _excelImportHelper = value;
        }

        /// <summary>
        /// 讀取excel表格中的資料,將Excel檔案流轉化為dataTable資料來源  
        /// 預設第一行為標題 
        /// </summary>
        /// <param name="stream">excel文件檔案流</param>
        /// <param name="fileType">文件格式</param>
        /// <param name="isSuccess">是否轉化成功</param>
        /// <param name="resultMsg">轉換結果訊息</param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(Stream stream, string fileType, out bool isSuccess, out string resultMsg)
        {
            isSuccess = false;
            resultMsg = "Excel檔案流成功轉化為DataTable資料來源";
            var excelToDataTable = new DataTable();

            try
            {
                //Workbook物件代表一個工作簿,首先定義一個Excel工作薄
                IWorkbook workbook;

                //XSSFWorkbook 適用XLSX格式，HSSFWorkbook 適用XLS格式
                #region 判斷Excel版本
                switch (fileType)
                {
                    //.XLSX是07版(或者07以上的)的Office Excel
                    case ".xlsx":
                        workbook = new XSSFWorkbook(stream);
                        break;
                    //.XLS是03版的Office Excel
                    case ".xls":
                        workbook = new HSSFWorkbook(stream);
                        break;
                    default:
                        throw new Exception("Excel文件格式有誤");
                }
                #endregion

                var sheet = workbook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();

                var headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;//最後一行列數（即為總列數）

                //獲取第一行標題列資料來源,轉換為dataTable資料來源的表格標題名稱
                for (var j = 0; j < cellCount; j++)
                {
                    var cell = headerRow.GetCell(j);
                    excelToDataTable.Columns.Add(cell.ToString());
                }

                //獲取Excel表格中除標題以為的所有資料來源，轉化為dataTable中的表格資料來源
                for (var i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    var dataRow = excelToDataTable.NewRow();

                    var row = sheet.GetRow(i);

                    if (row == null) continue; //沒有資料的行預設是null　

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)//單元格內容非空驗證
                        {
                            #region NPOI獲取Excel單元格中不同型別的資料
                            //獲取指定的單元格資訊
                            var cell = row.GetCell(j);
                            switch (cell.CellType)
                            {
                                //首先在NPOI中數字和日期都屬於Numeric型別
                                //通過NPOI中自帶的DateUtil.IsCellDateFormatted判斷是否為時間日期型別
                                case CellType.Numeric when DateUtil.IsCellDateFormatted(cell):
                                    dataRow[j] = cell.DateCellValue;
                                    break;
                                case CellType.Numeric:
                                    //其他數字型別
                                    dataRow[j] = cell.NumericCellValue;
                                    break;
                                //空資料型別
                                case CellType.Blank:
                                    dataRow[j] = "";
                                    break;
                                //公式型別
                                case CellType.Formula:
                                {
                                    HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(workbook);
                                    dataRow[j] = eva.Evaluate(cell).StringValue;
                                    break;
                                }
                                //布林型別
                                case CellType.Boolean:
                                    dataRow[j] = row.GetCell(j).BooleanCellValue;
                                    break;
                                //錯誤
                                //case CellType.Error:
                                //   dataRow[j] = HSSFErrorConstants.GetText(row.GetCell(j).ErrorCellValue);
                                //    break;
                                //其他型別都按字串型別來處理（未知型別CellType.Unknown，字串型別CellType.String）
                                default:
                                    dataRow[j] = cell.StringCellValue;
                                    break;
                            }
                            #endregion
                        }
                    }
                    excelToDataTable.Rows.Add(dataRow);
                }

                isSuccess = true;
            }
            catch (Exception e)
            {
                resultMsg = e.Message;
            }

            return excelToDataTable;
        }
    }
}