﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ExtensionMethods;
using System.Text.RegularExpressions;

/// <summary>
/// 文字格式化
/// </summary>
public class fn_stringFormat
{
    /// <summary>
    /// 電話格式轉換
    /// </summary>
    /// <param name="Nation">電話國碼</param>
    /// <param name="Area">電話區碼</param>
    /// <param name="Tel">電話</param>
    /// <param name="Ext">分機號碼</param>
    /// <returns>string</returns>
    /// <example>886-02-22183233#375</example>
    public static string Tel_Format(string Nation, string Area, string Tel, string Ext)
    {
        try
        {
            if (string.IsNullOrEmpty(Tel))
                return "";

            Nation = Nation.Trim();
            Area = Area.Trim();
            Tel = Tel.Trim();
            Ext = Ext.Trim();

            string rStr = Tel;
            if (!string.IsNullOrEmpty(Nation))
            {
                if (!string.IsNullOrEmpty(Area))
                    rStr = Nation + "-" + Area + "-" + Tel;
            }
            else
            {
                if (!string.IsNullOrEmpty(Area))
                    rStr = Area + "-" + Tel;
            }
            if (!string.IsNullOrEmpty(Ext))
                rStr += "#" + Ext;

            return rStr;
        }
        catch (Exception)
        {
            return "";

        }
    }

    /// <summary>
    /// 金額格式轉換 (含三位點)
    /// </summary>
    /// <param name="inputValue">傳入的值</param>
    /// <returns>string</returns>
    /// <example>100,000</example>
    public static string Money_Format(string inputValue)
    {
        try
        {
            //去除三位點
            inputValue = inputValue.Replace(",", "");
            //判斷是否為數值
            if (inputValue.IsNumeric() == false)
                return inputValue;
            //轉型為Double
            double dbl_Value = Convert.ToDouble(inputValue);
            //金額 >= 1000
            if (dbl_Value >= 1000)
            {
                if (dbl_Value > Math.Floor(dbl_Value))
                    return String.Format("{0:#,000.00}", dbl_Value);
                else
                    return String.Format("{0:#,000}", dbl_Value);
            }
            //金額 > 0 And 金額 < 1000
            if (dbl_Value > 0 & dbl_Value < 1000)
            {
                if (dbl_Value > Math.Floor(dbl_Value))
                    return String.Format("{0:0.00}", dbl_Value);
                else
                    return Convert.ToString(dbl_Value);
            }
            //金額 = 0
            if (dbl_Value == 0)
                return Convert.ToString(dbl_Value);

            //金額 < 0 And 金額 > -1000
            if (dbl_Value < 0 & dbl_Value > -1000)
            {
                if (Math.Abs(dbl_Value) > Math.Floor(Math.Abs(dbl_Value)))
                    return String.Format("{0:0.00}", dbl_Value);
                else
                    return Convert.ToString(dbl_Value);
            }

            //金額 < -1000
            if (dbl_Value < -1000)
            {
                if (Math.Abs(dbl_Value) > Math.Floor(Math.Abs(dbl_Value)))
                    return "-" + String.Format("{0:#,000.00}", Math.Abs(dbl_Value));
                else
                    return "-" + String.Format("{0:#,000}", Math.Abs(dbl_Value));
            }

            return "";
        }
        catch (Exception)
        {
            return "";

        }
    }

    /// <summary>
    /// 數字小數點格式轉換
    /// </summary>
    /// <param name="inputValue">傳入的值</param>
    /// <param name="idxNumber">取到第幾位</param>
    /// <returns>string</returns>
    public static string Decimal_Format(string inputValue, int idxNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(inputValue))
                return "";
            if (inputValue.IsNumeric() == false)
                return "";
            if (idxNumber < 0)
                return "";

            return Math.Round(Convert.ToDouble(inputValue), idxNumber).ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    /// <summary>
    /// 限制字串輸出n個字元
    /// </summary>
    /// <param name="inputValue">輸入文字</param>
    /// <param name="setLength">輸出字數</param>
    /// <param name="WordType">限制型態 (WordType)</param>        
    /// <param name="ShowMore">是否顯示...</param>
    /// <returns>string</returns>
    public static string Set_StringOutput(string inputValue, int setLength, WordType WordType, bool showMore)
    {
        //檢查 - 是否為空白字串
        if (string.IsNullOrEmpty(inputValue))
            return "";

        //重新組合字元
        StringBuilder Return_Word = new StringBuilder();
        int Return_Word_Num = 0;
        for (int i = 0; i < inputValue.Length; i++)
        {
            //取得新字元的長度
            int NewCharLength = 0;
            if (WordType == WordType.字數)
            {
                NewCharLength = inputValue.Substring(i, 1).Length;
            }
            else
            {
                //轉成byte, 適用於中文字
                NewCharLength = Encoding.Default.GetBytes(inputValue.Substring(i, 1)).Length;
            }

            //若加上 NewCharLength 新字元的長度後Return_Word_Num 累計長度已超出 LimitLength 限制長度，則回傳結果
            if ((NewCharLength + Return_Word_Num) > setLength)
            {
                if (showMore)
                    Return_Word.Append("...");
                break;
            }
            else
            {
                Return_Word.Append(inputValue.Substring(i, 1));
                Return_Word_Num += NewCharLength;
            }
        }

        return Return_Word.ToString();
    }

    public enum WordType
    {
        字數, Bytes
    }

    /// <summary>
    /// 過濾並轉換HTML特殊字元
    /// </summary>
    /// <param name="inputString">Html字串</param>
    /// <returns>string</returns>
    public static string Set_FilterHtml(string inputString)
    {
        if (string.IsNullOrEmpty(inputString))
            return "";

        //刪除Script
        inputString = Regex.Replace(inputString, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //刪除HTML Tag
        inputString = Regex.Replace(inputString, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);

        inputString = Regex.Replace(inputString, "([\\r\\n])[\\s]+", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "-->", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "<!--.*", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(iexcl|#161);", "!", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(cent|#162);", "￠", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(pound|#163);", "￡", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(copy|#169);", "c", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&#(\\d+);", "", RegexOptions.IgnoreCase);
        inputString =
            inputString
            .Replace("<", "")
            .Replace(">", "")
            .Replace("--", "")
            .Replace(":", "")
            .Replace("+", "")
            .Replace("'", "")
            .Replace(@"""", "") //&quot;
            .Replace("'", "")   //&#39;
            .Replace("" + Convert.ToChar(13) + "" + Convert.ToChar(10) + "", "");

        inputString = HttpContext.Current.Server.HtmlEncode(inputString).Trim();
        return inputString;
    }

    /// <summary>
    /// 過濾並轉換 TextArea的文字
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string Set_FilterString(string inputString)
    {
        if (string.IsNullOrEmpty(inputString.Trim()))
            return "";

        //刪除Script
        inputString = Regex.Replace(inputString, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
        //刪除HTML tag
        inputString = Regex.Replace(inputString, "<(.[^>]*)>", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "-->", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "<!--.*", "", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(amp|#38);", "&", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(lt|#60);", "<", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(gt|#62);", ">", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(iexcl|#161);", "!", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(cent|#162);", "￠", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(pound|#163);", "￡", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&(copy|#169);", "c", RegexOptions.IgnoreCase);
        inputString = Regex.Replace(inputString, "&#(\\d+);", "", RegexOptions.IgnoreCase);

        inputString = HttpContext.Current.Server.HtmlEncode(inputString).Trim();
        return inputString;
    }

    /// <summary>
    /// 清除或轉換含有特殊字的檔名
    /// </summary>
    /// <param name="inputValue">輸入字串</param>
    /// <returns></returns>
    public static string Set_ClearFileName(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue.Trim()))
            return "";

        inputValue.Replace("<", "");
        inputValue.Replace(">", "");
        inputValue.Replace("--", "");
        inputValue.Replace("+", "_");
        inputValue.Replace("&", "_");
        inputValue.Replace("#", "_");

        return inputValue;
    }

    /// <summary>
    /// 去除 HTML 標籤，可自訂合法標籤加以保留
    /// </summary>
    /// <param name="src">來源字串</param>
    /// <param name="reservedTagPool">合法標籤集</param>
    /// <returns></returns>
    /// <example>
    ///  //要保留的Html Tag
    ///  string[] reservedTagPool = { "ul", "/ul", "li", "/li", "br" };
    ///  //去除其他的Html Tag
    ///   StripTags(this.tb_OrgCont.Text, reservedTagPool);
    /// </example>
    public static string StripTags(string src, string[] reservedTagPool)
    {
        return Regex.Replace(
            src,
            String.Format("<(?!{0}).*?>", string.Join("|", reservedTagPool)),
            String.Empty);
    }

    /// <summary>
    /// 加密檔案路徑
    /// </summary>
    /// <param name="filefolder">資料夾路徑</param>
    /// <returns></returns>
    public static string ashx_Pic(string filefolder)
    {
        if (string.IsNullOrEmpty(filefolder))
        {
            return "";
        }

        //下載路徑 
        string downloadPath = "{0}myHandler/Ashx_ImageDownload.ashx?FilePath={1}".FormatThis(
                System.Web.Configuration.WebConfigurationManager.AppSettings["WebUrl"]
                , HttpUtility.UrlEncode(Cryptograph.Encrypt(filefolder))
            );

        return downloadPath;
    }

    /// <summary>
    /// 顯示圖片 (不經過Ashx)
    /// </summary>
    /// <param name="filefolder">資料夾路徑</param>
    /// <returns></returns>
    public static string show_Pic(string filefolder)
    {
        if (string.IsNullOrEmpty(filefolder))
        {
            return "";
        }

        return "{0}{1}".FormatThis(
            System.Web.Configuration.WebConfigurationManager.AppSettings["File_WebUrl"]
            , filefolder
            );

    }

    #region -- 文字遮罩 --
    public enum stringType { Email, Text };

    /// <summary>
    /// 文字遮罩 (可選擇輸入類型)
    /// </summary>
    /// <param name="inputValue">輸入字串</param>
    /// <param name="inputType">輸入類型</param>
    /// <returns></returns>
    /// <example>
    /// fn_stringFormat.wordMask("我是字串", fn_stringFormat.stringType.Text)
    /// </example>
    public static string Set_wordMask(string inputValue, stringType inputType)
    {
        try
        {
            //判斷空值
            if (string.IsNullOrEmpty(inputValue)) return "";
            //判斷輸入類型
            switch (inputType)
            {
                case stringType.Email:
                    string[] myStr = Regex.Split(inputValue, @"\@{1}");

                    return string.Format("{0}{1}"
                        , Set_wordMask(myStr[0])
                        , (inputValue.IndexOf("@")) != -1 ? "@" + myStr[1] : "");

                default:
                    return Set_wordMask(inputValue);
            }
        }

        catch (Exception)
        {
            throw new Exception("文字遮罩處理發生錯誤");
        }


    }

    /// <summary>
    /// 文字遮罩 (直接處理字串)
    /// </summary>
    /// <param name="inputValue">輸入字串</param>
    /// <returns></returns>
    private static string Set_wordMask(string inputValue)
    {
        //取得文字長度
        int countWord = inputValue.Length;
        //取得第一個字
        string firstWord = inputValue.Left(1);
        //取得最後一個字
        string lastWord = inputValue.Right(1);
        //遮罩中間的字
        //若文字長度小於3，則回傳第一個字加*
        string middleWord = (countWord < 3) ? "*" : lastWord.PadLeft(inputValue.Length - 1, '*');

        return firstWord + middleWord;
    }
    #endregion

    #region -- ASCII字碼轉換 --
    /// <summary>
    /// ASCII to String
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    public static char Chr(int Num)
    {
        char C = Convert.ToChar(Num);
        return C;
    }

    /// <summary>
    /// String to ASCII
    /// </summary>
    /// <param name="S"></param>
    /// <returns></returns>
    public static int ASC(string S)
    {
        int N = Convert.ToInt32(S[0]);
        return N;
    }

    /// <summary>
    /// Char to ASCII
    /// </summary>
    /// <param name="C"></param>
    /// <returns></returns>
    public static int ASC(char C)
    {
        int N = Convert.ToInt32(C);
        return N;
    }
    #endregion

    /// <summary>
    /// 回傳Icon圖片名
    /// </summary>
    /// <param name="fileExt">副檔名</param>
    /// <returns></returns>
    public static string Get_Icon(string fileExt)
    {
        switch (fileExt.ToUpper().Replace(".", ""))
        {
            case "PDF":
                return "img_pdf.png";

            case "DOCX":
                return "img_docx.png";

            case "PPTX":
                return "img_pptx.png";

            case "RAR":
                return "img_rar.png";

            case "ZIP":
                return "img_zip.png";

            case "XLSX":
                return "img_xlsx.png";

            default:
                return "img_other.png";

        }
    }

}