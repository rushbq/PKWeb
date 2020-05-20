using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using ExtensionUI;

public partial class PriceList : SecurityCheckDealer
{
    public string ErrMsg;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //** 次標題 **
                this.Page.Title = Resources.resPublic.home_經銷商專區;

            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    #region -- 資料顯示 --
    /// <summary>
    /// 匯出PriceList Excel
    /// </summary>
    protected void btn_PriceList_Click(object sender, EventArgs e)
    {
        //查詢StoreProcedure (myPrc_GetCustFullPrice_OverSales)
        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "myPrc_GetCustFullPrice_OverSales";
            cmd.Parameters.AddWithValue("CustID", Get_CustID);
            cmd.Parameters.AddWithValue("ProdClass", string.IsNullOrEmpty(Get_ProdCls) ? DBNull.Value : (Object)Get_ProdCls);
            cmd.CommandTimeout = 90;

            using (DataTable DT = dbConn.LookupDT(cmd, dbConn.DBS.PKSYS, out ErrMsg))
            {
                if (DT == null)
                {
                    fn_Extensions.JsAlert("No Data", "");
                    return;
                }

                if (DT.Rows.Count == 0)
                {
                    fn_Extensions.JsAlert("Fail", "");
                    return;
                }

                //取得Datatable, 篩選欄位
                var query =
                from el in DT.AsEnumerable()
                orderby el.Field<string>("Model_No")
                select new
                {
                    Stop_Offer = el.Field<string>("Stop_Offer"),
                    Item_NO = el.Field<string>("Model_No"),
                    Class = el.Field<string>("ClassName_{0}".FormatThis(fn_Language.Param_Lang)),
                    Description = el.Field<string>("Model_Name_{0}".FormatThis(fn_Language.Param_Lang)),
                    Currency = el.Field<string>("Currency"),
                    Unit_Price = el.Field<double?>("myPrice"),
                    Unit = el.Field<string>("Unit"),
                    Quote_Date = el.Field<string>("QuoteDate"),
                    MOQ = el.Field<int?>("MOQ"),
                    VOL = el.Field<string>("Vol"),
                    Page = el.Field<string>("Page"),
                    Qty_Inner = el.Field<int?>("InnerBox_Qty"),
                    NW = el.Field<double?>("InnerBox_NW"),
                    GW = el.Field<double?>("InnerBox_GW"),
                    CUFT = el.Field<double?>("InnerBox_Cuft"),
                    BarCode = el.Field<string>("BarCode"),
                    Packing = el.Field<string>("Packing_{0}".FormatThis(fn_Language.Param_Lang)),
                    Ship_From = el.Field<string>("Ship_From"),
                    Term = el.Field<string>("TransTermValue")
                };

                //Linq轉DataTable
                DataTable myDT = fn_CustomUI.LINQToDataTable(query);

                //匯出Excel
                fn_CustomUI.ExportExcel(myDT
                    , "{0}-PriceList.xlsx".FormatThis(DateTime.Now.ToShortDateString().ToDateString("yyyyMMdd"))
                    );
            }
        }

    }

    #endregion

    #region -- 參數設定 --

    /// <summary>
    /// 設定參數 - 客戶編號
    /// </summary>
    public string Get_CustID
    {
        get
        {
            String DataID = fn_Member.GetDealerID(fn_Param.MemberID);

            return string.IsNullOrEmpty(DataID) ? "" : DataID;
        }
        set
        {
            this._Get_CustID = value;
        }
    }
    private string _Get_CustID;


    /// <summary>
    /// 取得參數 - 產品類別
    /// </summary>
    public string Get_ProdCls
    {
        get
        {
            String data = Request.QueryString["Cls"] == "-1" ? "" : Request.QueryString["Cls"].ToString();

            return data;
        }
        set
        {
            this._Get_ProdCls = value;
        }
    }
    private string _Get_ProdCls;
    #endregion
}