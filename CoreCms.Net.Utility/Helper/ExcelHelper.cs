using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace CoreCms.Net.Utility.Helper
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 获取头部样式
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static ICellStyle GetHeaderStyle(HSSFWorkbook hs)
        {
            //样式
            ICellStyle headerStyle = hs.CreateCellStyle();//创建样式
            headerStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中 方法1 
            //style.Alignment = HorizontalAlignment.CenterSelection;//设置居中 方法2
            headerStyle.Alignment = HorizontalAlignment.Center;//设置居中 方法3 
            HSSFFont headerfont = (HSSFFont)hs.CreateFont();//创建字体
            headerfont.Color = HSSFColor.Black.Index;//给字体设置颜色
            headerfont.FontName = "宋体";
            headerfont.IsBold = true;
            headerfont.FontHeight = 11;
            headerfont.FontHeightInPoints = 11;
            headerStyle.SetFont(headerfont);

            headerStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;    //下边框线
            headerStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;      //左边框线
            headerStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;     //右边框线
            headerStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;       //上边框线
            return headerStyle;
        }
        /// <summary>
        /// 获取通用列样式
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static ICellStyle GetCommonStyle(HSSFWorkbook hs)
        {
            //样式
            ICellStyle commonCellStyle = hs.CreateCellStyle();//创建样式
            commonCellStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            //commonCellStyle.Alignment = HorizontalAlignment.CenterSelection;//设置居中
            commonCellStyle.Alignment = HorizontalAlignment.Center;//设置居中

            HSSFFont commonFont = (HSSFFont)hs.CreateFont();//创建字体
            commonFont.FontName = "宋体";
            commonFont.FontHeight = 10;
            commonFont.FontHeightInPoints = 10;
            commonCellStyle.SetFont(commonFont);

            commonCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;    //下边框线
            commonCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;      //左边框线
            commonCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;     //右边框线
            commonCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;       //上边框线
            return commonCellStyle;
        }

    }
}
