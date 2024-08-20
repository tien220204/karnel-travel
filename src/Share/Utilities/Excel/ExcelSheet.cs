﻿using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation.Contracts;
using OfficeOpenXml.Style;

namespace KarnelTravel.Share.Utilities.Excel
{
    public class ExcelSheet
    {
        private readonly ExcelWorksheet _sheet;
        private int _rowIndex;
        private int _colIndex;

        public ExcelSheet(ExcelWorksheet sheet)
        {
            _sheet = sheet;
            _rowIndex = 1;
            _colIndex = 1;
        }

        /// <summary>
        ///     The row index can be overridden with a specific index
        /// </summary>
        /// <param name="index"></param>
        public void OverrideRowIndex(int index)
        {
            _rowIndex = index;
        }

        /// <summary>
        ///     The column index can be overridden with a specific index
        /// </summary>
        /// <param name="index"></param>
        public void OverrideColIndex(int index)
        {
            _colIndex = index;
        }

        /// <summary>
        ///     Blank line(s) are added to the worksheet at the current index location
        /// </summary>
        /// <param name="numberOfLines"></param>
        public void WriteBlankLines(int numberOfLines = 1)
        {
            for (var i = 0; i < numberOfLines; i++)
            {
                _rowIndex++;
            }
        }

        /// <summary>
        ///     Writes a row of values at the current index with optional formatting. If formatting is specified, it is applied to each value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="formatting"></param>
        public void WriteRow<T>(IEnumerable<T> values, ExcelReportFormatting formatting = null)
        {
            int originalColIndex = _colIndex;

            foreach (T value in values)
            {
                SetFormatting(formatting);
                _sheet.Cells[_rowIndex, _colIndex++].Value = value;
            }

            _rowIndex++;
            _colIndex = originalColIndex;
        }

        /// <summary>
        ///     Writes each property of each record at the current index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="includeHeader"></param>
        /// <param name="bordered"></param>
        public void WriteData<T>(IEnumerable<T> records, bool includeHeader = true, bool bordered = true)
        {
            int originalRowIndex = _rowIndex;
            int originalColIndex = _colIndex;
            if (includeHeader)
            {
                foreach (PropertyInfo prop in typeof(T).GetProperties())
                {
                    FormatHeader(_sheet.Cells[_rowIndex, _colIndex++], prop.Name.SplitCamelCase());
                }

                _rowIndex++;
                _colIndex = originalColIndex;
            }

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (T record in records)
            {
                foreach (PropertyInfo prop in props)
                {
                    _sheet.Cells[_rowIndex, _colIndex++].Value = prop.GetGetMethod().Invoke(record, null);
                }

                _rowIndex++;
                _colIndex = originalColIndex;
            }

            if (bordered)
            {
                _sheet.Cells[originalRowIndex, originalColIndex, _rowIndex - 1, props.Length].BorderAll();
            }
        }

        /// <summary>
        /// Writes each property of each record at the current index (Horizontally)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="bordered"></param>
        public void WriteDataHorizontally<T>(IEnumerable<T> records, bool bordered = true)
        {
            int originalRowIndex = _rowIndex;
            int originalColIndex = _colIndex;

            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (T record in records)
            {
                foreach (PropertyInfo prop in props)
                {
                    _sheet.Cells[_rowIndex++, _colIndex].Value = prop.GetGetMethod().Invoke(record, null);
                }

                _rowIndex = originalRowIndex;
                _colIndex++;
            }

            if (bordered)
            {
                _sheet.Cells[originalRowIndex, originalColIndex, props.Length, _colIndex - 1].BorderAll();
            }
        }

        /// <summary>
        ///     Writes each specified property of each record at the current index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="headers"></param>
        /// <param name="includeHeader"></param>
        /// <param name="bordered"></param>
        public void WriteData<T>(IEnumerable<T> records, List<ExcelReportHeader> headers, bool includeHeader = true, bool bordered = true)
        {
            int originalRowIndex = _rowIndex;
            int originalColIndex = _colIndex;
            if (includeHeader)
            {
                foreach (ExcelReportHeader header in headers)
                {
                    FormatHeader(_sheet.Cells[_rowIndex, _colIndex++], header.DisplayName ?? header.FieldName.SplitCamelCase());
                }

                _rowIndex++;
                _colIndex = originalColIndex;
            }

            Dictionary<string, PropertyInfo> propDictionary = typeof(T).GetProperties().ToDictionary(n => n.Name);
            foreach (T record in records)
            {
                foreach (ExcelReportHeader header in headers)
                {
                    object value;
                    if (header.FieldName == null)
                    {
                        value = header.DefaultValue;
                    }
                    else
                    {
                        PropertyInfo prop = propDictionary.GetValueOrDefault(header.FieldName);
                        value = prop.GetGetMethod().Invoke(record, null);
                    }

                    var stringValue = string.Empty;
                    switch (header.Format)
                    {
                        case ExcelFormat.Decimal0Places:
                        case ExcelFormat.Decimal2Places:
                        case ExcelFormat.Decimal4Places:
                        case ExcelFormat.Decimal6Places:
                            stringValue = ((decimal?)value)?.ToString($"N{(int)header.Format}");
                            break;

                        case ExcelFormat.Currency:
                            stringValue = ((decimal?)value)?.ToString("C");
                            break;

                        case ExcelFormat.ShortDate:
                            stringValue = ((DateTime?)value)?.ToString("d");
                            break;

                        case ExcelFormat.ShortDateTime:
                            stringValue = ((DateTime?)value)?.ToString("g");
                            break;

                        case ExcelFormat.BooleanYesNo:
                            stringValue = (bool?)value ?? false ? "Yes" : "No";
                            break;

                        case ExcelFormat.EnumDescription:
                            EnumMemberAttribute attr = header.Type.GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
                            stringValue = attr?.Value ?? value.ToString();
                            break;

                        case ExcelFormat.ListValues:
                            var objList = new List<object>((IEnumerable<object>)value);
                            stringValue = "";
                            PropertyInfo[] props = objList.FirstOrDefault()?.GetType().GetProperties();
                            foreach (object o in objList)
                            {
                                if (!string.IsNullOrEmpty(stringValue))
                                {
                                    stringValue += Environment.NewLine;
                                }

                                var internalStringValue = "";
                                foreach (string headerChildListValue in header.ChildListValues)
                                {
                                    object val = props?.First(n => n.Name == headerChildListValue).GetValue(o, null);
                                    if (!string.IsNullOrEmpty(internalStringValue) && val != null)
                                    {
                                        internalStringValue += header.ChildListOperator;
                                    }

                                    internalStringValue += val?.ToString();
                                }

                                stringValue += internalStringValue;
                            }

                            break;
                        case ExcelFormat.DropDownList:
                            IExcelDataValidationList list = _sheet.Cells[2, _colIndex, 100, _colIndex].DataValidation.AddListDataValidation();
                            foreach (string headerChildListValue in header.ChildListValues)
                            {
                                list.Formula.Values.Add(headerChildListValue);
                            }

                            list.AllowBlank = true;
                            break;

                        case ExcelFormat.DropDownListFormula:
                            IExcelDataValidationList dataList = _sheet.Cells[2, _colIndex, 100, _colIndex].DataValidation.AddListDataValidation();
                            dataList.Formula.ExcelFormula = header.Formula;

                            dataList.AllowBlank = true;
                            break;

                        case ExcelFormat.ConvertFromDictionary:
                            header.ValueDisplayMapping ??= new Dictionary<object, string>();
                            stringValue = header.ValueDisplayMapping.ContainsKey(value) ? header.ValueDisplayMapping[value] : value.ToString();
                            break;

                        default:
                            stringValue = value?.ToString();
                            break;
                    }

                    SetFormatting(header);
                    _sheet.Cells[_rowIndex, _colIndex++].Value = stringValue;
                }

                _rowIndex++;
                _colIndex = originalColIndex;
            }

            if (bordered)
            {
                _sheet.Cells[originalRowIndex, originalColIndex, _rowIndex - 1, headers.Count].BorderAll();
            }
        }

        /// <summary>
        ///     Adds a Named Range for a set of cells
        /// </summary>
        /// <param name="rangeName"></param>
        /// <param name="cellRange"></param>
        public void CreateNamedRange(string rangeName, string cellRange)
        {
            ExcelRange range = _sheet.Cells[cellRange];
            _sheet.Names.Add(rangeName, range);
        }

        private void SetFormatting(ExcelReportFormatting formatting)
        {
            ExcelStyle cellStyle = _sheet.Cells[_rowIndex, _colIndex].Style;

            cellStyle.Font.Bold = formatting?.IsBold ?? false;
            cellStyle.Font.Italic = formatting?.IsItalics ?? false;
            cellStyle.Font.UnderLine = formatting?.IsUnderlined ?? false;

            if (formatting?.FontSize != null)
            {
                cellStyle.Font.Size = formatting.FontSize.Value;
            }

            if (formatting?.BackgroundColor != null)
            {
                cellStyle.Fill.PatternType = ExcelFillStyle.Solid;
                cellStyle.Fill.BackgroundColor.SetColor(formatting.BackgroundColor.Value);
            }

            if (formatting?.TextColor != null)
            {
                cellStyle.Font.Color.SetColor(formatting.TextColor.Value);
            }

            cellStyle.VerticalAlignment = ExcelVerticalAlignment.Top;
        }

        public void AutoFitColumns()
        {
            _sheet.Cells[_sheet.Dimension.Address].AutoFitColumns();
        }

        public void WrapText(bool wrapText = true)
        {
            _sheet.Cells[_sheet.Dimension.Address].Style.WrapText = wrapText;
        }

        public void VerticalAlignment(ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center)
        {
            _sheet.Cells[_sheet.Dimension.Address].Style.VerticalAlignment = verticalAlignment;
        }

        public void HideSheet()
        {
            _sheet.Hidden = eWorkSheetHidden.VeryHidden;
        }

        private void FormatHeader(ExcelRangeBase range, string value)
        {
            range.Value = value;
            range.Style.Font.Bold = true;
        }
    }

    public static class BorderExtension
    {
        /// <summary>
        ///     Adds a border to all sides (top, right, bottom & left) to a specified range of cells
        /// </summary>
        /// <param name="range"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public static void BorderAll(this ExcelRange range, ExcelBorderStyle style = ExcelBorderStyle.Thin, Color? color = null)
        {
            for (int row = range.Start.Row; row <= range.End.Row; row++)
            {
                for (int col = range.Start.Column; col <= range.End.Column; col++)
                {
                    range.Worksheet.Cells[row, col].Style.Border.BorderAround(style, color ?? Color.Black);
                }
            }
        }
    }
}
