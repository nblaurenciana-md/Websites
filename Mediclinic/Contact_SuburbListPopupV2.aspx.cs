﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

public partial class Contact_SuburbListPopupV2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
                Utilities.SetNoCache(Response);
            HideErrorMessage();
            Utilities.UpdatePageHeaderV2(Page.Master, true);

            if (!IsPostBack)
            {
                Session.Remove("suburb_sortExpression");
                Session.Remove("suburb_data");


                ddlState.Items.Add(new ListItem("All", "All"));
                DataTable states = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "Suburb", "", "state", "distinct state");
                foreach (DataRow row in states.Rows)
                    ddlState.Items.Add(new ListItem(row["state"].ToString(), row["state"].ToString()));

                SystemVariable sysVariable = SystemVariableDB.GetByDescr("DefaultState");
                if (sysVariable != null && sysVariable.Value.Trim() != "")
                    ddlState.SelectedValue = sysVariable.Value.Trim();

                FillSuburbGrid();
            }

            this.GrdSuburb.EnableViewState = true;

        }
        catch (CustomMessageException ex)
        {
            if (IsPostBack) SetErrorMessage(ex.Message);
            else HideTableAndSetErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            if (IsPostBack) SetErrorMessage("", ex.ToString());
            else HideTableAndSetErrorMessage("", ex.ToString());
        }
    }

    #region GetUrlParams

    protected bool IsValidFormOnlyProviders()
    {
        string only_providers = Request.QueryString["only_providers"];
        return only_providers != null && (Request.QueryString["only_providers"] == "1" || Request.QueryString["only_providers"] == "0");
    }
    protected bool GetFormOnlyProviders(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormOnlyProviders())
            throw new Exception("Invalid url field only_providers");
        return Request.QueryString["only_providers"] == "1";
    }


    /*
    protected bool IsValidFormOrg()
    {
        string orgID = Request.QueryString["org"];
        return orgID != null && Regex.IsMatch(orgID, @"^\d+$") && OrganisationDB.Exists(Convert.ToInt32(orgID));
    }
    protected int GetFormOrg(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormOrg())
            throw new Exception("Invalid url org");
        return Convert.ToInt32(Request.QueryString["org"]);
    }

    protected bool IsValidFormOrgs()
    {
        string orgIDs = Request.QueryString["orgs"];
        return orgIDs != null && Regex.IsMatch(orgIDs, @"^[\d,]+$") && OrganisationDB.Exists(orgIDs);
    }
    protected string GetFormOrgs(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormOrgs())
            throw new Exception("Invalid url orgs");
        return Request.QueryString["orgs"];
    }
     */

    #endregion

    #region ddlState_SelectedIndexChanged

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSuburbGrid();
    }

    #endregion

    #region GrdSuburb

    protected void FillSuburbGrid()
    {
        DataTable dt = SuburbDB.GetDataTable(false, txtSearchSuburbName.Text.Trim(), true, null, (ddlState.SelectedValue == "All" ? null : ddlState.SelectedValue) );
        Session["suburb_data"] = dt;

        if (dt.Rows.Count > 0)
        {

            if (IsPostBack && Session["suburb_sortExpression"] != null && Session["suburb_sortExpression"].ToString().Length > 0)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = Session["suburb_sortExpression"].ToString();
                GrdSuburb.DataSource = dataView;
            }
            else
            {
                GrdSuburb.DataSource = dt;
            }

            try
            {
                GrdSuburb.DataBind();
                GrdSuburb.PagerSettings.FirstPageText = "1";
                GrdSuburb.PagerSettings.LastPageText = GrdSuburb.PageCount.ToString();
                GrdSuburb.DataBind();
            }
            catch (Exception ex)
            {
                this.lblErrorMessage.Visible = true;
                this.lblErrorMessage.Text = ex.ToString();
            }
        }
        else
        {
            dt.Rows.Add(dt.NewRow());
            GrdSuburb.DataSource = dt;
            GrdSuburb.DataBind();

            int TotalColumns = GrdSuburb.Rows[0].Cells.Count;
            GrdSuburb.Rows[0].Cells.Clear();
            GrdSuburb.Rows[0].Cells.Add(new TableCell());
            GrdSuburb.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            GrdSuburb.Rows[0].Cells[0].Text = "No Record Found";
        }
    }
    protected void GrdSuburb_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (!Utilities.IsDev() && e.Row.RowType != DataControlRowType.Pager)
        {
            e.Row.Cells[0].CssClass = "hiddencol";
        }
    }
    protected void GrdSuburb_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = Session["suburb_data"] as DataTable;
        bool tblEmpty = (dt.Rows.Count == 1 && dt.Rows[0][0] == DBNull.Value);
        if (!tblEmpty && e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblId = (Label)e.Row.FindControl("lblId");
            DataRow[] foundRows = dt.Select("suburb_id=" + lblId.Text);
            DataRow thisRow = foundRows[0];


            Button btnSelect = (Button)e.Row.FindControl("btnSelect");
            if (btnSelect != null)
                btnSelect.OnClientClick = "javascript:select_suburb('" + thisRow["suburb_id"].ToString() + ":" + thisRow["name"].ToString() + ", " + thisRow["state"].ToString() + " (" + thisRow["postcode"].ToString() + ")" + "');";


            Utilities.AddConfirmationBox(e);
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                Utilities.SetEditRowBackColour(e, System.Drawing.Color.LightGoldenrodYellow);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
        }
    }
    protected void GrdSuburb_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GrdSuburb.EditIndex = -1;
        FillSuburbGrid();
    }
    protected void GrdSuburb_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    protected void GrdSuburb_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void GrdSuburb_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void GrdSuburb_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrdSuburb.EditIndex = e.NewEditIndex;
        FillSuburbGrid();
    }
    protected void GrdSuburb_Sorting(object sender, GridViewSortEventArgs e)
    {
        // dont allow sorting if in edit mode
        if (GrdSuburb.EditIndex >= 0)
            return;

        DataTable dataTable = Session["suburb_data"] as DataTable;

        if (dataTable != null)
        {
            if (Session["suburb_sortExpression"] == null)
                Session["suburb_sortExpression"] = "";

            DataView dataView = new DataView(dataTable);
            string[] sortData = Session["suburb_sortExpression"].ToString().Trim().Split(' ');
            string newSortExpr = (e.SortExpression == sortData[0] && sortData[1] == "ASC") ? "DESC" : "ASC";
            dataView.Sort = e.SortExpression + " " + newSortExpr;
            Session["suburb_sortExpression"] = e.SortExpression + " " + newSortExpr;

            GrdSuburb.DataSource = dataView;
            GrdSuburb.DataBind();
        }
    }
    protected void GrdSuburb_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdSuburb.PageIndex = e.NewPageIndex;
        FillSuburbGrid();
    }

    #endregion

    #region btnSearchSurname_Click, btnClearSurnameSearch_Click

    protected void btnSearchSuburbName_Click(object sender, EventArgs e)
    {
        if (!Regex.IsMatch(txtSearchSuburbName.Text, @"^[a-zA-Z\-\'\s]*$"))
        {
            SetErrorMessage("Search text can only be letters and hyphens");
            return;
        }
        else if (txtSearchSuburbName.Text.Trim().Length == 0)
        {
            SetErrorMessage("No search text entered");
            return;
        }
        else
            HideErrorMessage();


        FillSuburbGrid();
    }
    protected void btnClearSuburbNameSearch_Click(object sender, EventArgs e)
    {
        txtSearchSuburbName.Text = string.Empty;

        FillSuburbGrid();
    }

    #endregion

    #region SetErrorMessage, HideErrorMessage

    private void HideTableAndSetErrorMessage(string errMsg = "", string details = "")
    {
        GrdSuburb.Visible = false;
        SetErrorMessage(errMsg, details);
    }
    private void SetErrorMessage(string errMsg = "", string details = "")
    {
        if (errMsg.Contains(Environment.NewLine))
            errMsg = errMsg.Replace(Environment.NewLine, "<br />");

        // double escape so shows up literally on webpage for 'alert' message
        string detailsToDisplay = (details.Length == 0 ? "" : " <a href=\"#\" onclick=\"alert('" + details.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("'", "\\'").Replace("\"", "\\'") + "'); return false;\">Details</a>");

        lblErrorMessage.Visible = true;
        if (errMsg != null && errMsg.Length > 0)
            lblErrorMessage.Text = errMsg + detailsToDisplay + "<br />";
        else
            lblErrorMessage.Text = "An error has occurred. Plase contact the system administrator. " + detailsToDisplay + "<br />";
    }
    private void HideErrorMessage()
    {
        lblErrorMessage.Visible = false;
        lblErrorMessage.Text = "";
    }

    #endregion

}