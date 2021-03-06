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

public partial class PatientEditHistoryV2 : System.Web.UI.Page
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
                Session.Remove("patientedithistory_sortexpression");
                Session.Remove("patientedithistory_data");
                FillGrid();
            }

            this.GrdPatient.EnableViewState = true;

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

    private Patient GetFormPatient()
    {
        try
        {
            string id = Request.QueryString["id"];
            return (id == null || !Regex.IsMatch(id, @"^\d+$")) ? null : PatientDB.GetByID(Convert.ToInt32(id));
        }
        catch (Exception)
        {
            return null;
        }
    }


    #region GrdPatient

    protected void FillGrid()
    {
        Patient patient = GetFormPatient();
        if (patient == null)
        {
            SetErrorMessage("Invalid patient id");
            return;
        }


        DataTable dt = PatientHistoryDB.GetDataTable_ByPatientID(patient.PatientID);


        Hashtable offeringsHash = OfferingDB.GetHashtable(true, -1, null, true);
        PatientDB.AddACOfferings(ref offeringsHash, ref dt, "ac_inv_offering_id", "ac_inv_offering_name", "ac_pat_offering_id", "ac_pat_offering_name",
                                                            "ac_inv_aged_care_patient_type_id", "ac_inv_aged_care_patient_type_descr",
                                                            "ac_pat_aged_care_patient_type_id", "ac_pat_aged_care_patient_type_descr"
                                                            );

        //in display, have pt type 
        //- if mcd/dva/emergency : Medicare (Low Care)
        //- if LC/HC/ETC         : Low Care
        dt.Columns.Add("ac_offering", typeof(String));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int ac_inv_offering_id = dt.Rows[i]["ac_inv_offering_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["ac_inv_offering_id"]);
            int ac_pat_offering_id = dt.Rows[i]["ac_pat_offering_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["ac_pat_offering_id"]);
            string ac_inv_offering_name = dt.Rows[i]["ac_inv_offering_name"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_inv_offering_name"]);
            string ac_pat_offering_name = dt.Rows[i]["ac_pat_offering_name"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_pat_offering_name"]);

            int ac_inv_aged_care_patient_type_id = dt.Rows[i]["ac_inv_aged_care_patient_type_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["ac_inv_aged_care_patient_type_id"]);
            string ac_inv_aged_care_patient_type_descr = dt.Rows[i]["ac_inv_aged_care_patient_type_descr"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_inv_aged_care_patient_type_descr"]);
            int ac_pat_aged_care_patient_type_id = dt.Rows[i]["ac_pat_aged_care_patient_type_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[i]["ac_pat_aged_care_patient_type_id"]);
            string ac_pat_aged_care_patient_type_descr = dt.Rows[i]["ac_pat_aged_care_patient_type_descr"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_pat_aged_care_patient_type_descr"]);


            if (ac_inv_offering_id == -1)
                dt.Rows[i]["ac_offering"] = string.Empty;
            else if ((new List<int> { 2, 3, 4, 5 }).Contains(ac_inv_aged_care_patient_type_id))
                dt.Rows[i]["ac_offering"] = ac_inv_offering_name;
            else if ((new List<int> { 6, 7, 8, 9, 10 }).Contains(ac_inv_aged_care_patient_type_id))
                dt.Rows[i]["ac_offering"] = ac_inv_offering_name + " (" + ac_pat_offering_name + ")";
            else // (?)
                dt.Rows[i]["ac_offering"] = ac_inv_offering_name;
        }


        Session["patientedithistory_data"] = dt;

        if (dt.Rows.Count > 0)
        {

            if (IsPostBack && Session["patientedithistory_sortexpression"] != null && Session["patientedithistory_sortexpression"].ToString().Length > 0)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = Session["patientedithistory_sortexpression"].ToString();
                GrdPatient.DataSource = dataView;
            }
            else
            {
                GrdPatient.DataSource = dt;
            }


            try
            {
                GrdPatient.DataBind();
                GrdPatient.PagerSettings.FirstPageText = "1";
                GrdPatient.PagerSettings.LastPageText = GrdPatient.PageCount.ToString();
                GrdPatient.DataBind();
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex.ToString());
            }
        }
        else
        {
            dt.Rows.Add(dt.NewRow());
            GrdPatient.DataSource = dt;
            GrdPatient.DataBind();

            int TotalColumns = GrdPatient.Rows[0].Cells.Count;
            GrdPatient.Rows[0].Cells.Clear();
            GrdPatient.Rows[0].Cells.Add(new TableCell());
            GrdPatient.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            GrdPatient.Rows[0].Cells[0].Text = "No Record Found";
        }
    }
    protected void GrdPatient_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (!Utilities.IsDev() && e.Row.RowType != DataControlRowType.Pager)
            e.Row.Cells[0].CssClass = "hiddencol";

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            foreach (DataControlField col in GrdPatient.Columns)
            {
                if (!UserView.GetInstance().IsClinicView)
                    if (col.HeaderText.ToLower().Trim() == "ac type")
                        e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";
            }
        }
    }
    protected void GrdPatient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void GrdPatient_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
    }
    protected void GrdPatient_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
    }
    protected void GrdPatient_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void GrdPatient_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void GrdPatient_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        // dont allow sorting if in edit mode
        if (GrdPatient.EditIndex >= 0)
            return;

        DataTable dataTable = Session["patientedithistory_data"] as DataTable;

        if (dataTable != null)
        {
            if (Session["patientedithistory_sortexpression"] == null)
                Session["patientedithistory_sortexpression"] = "";

            DataView dataView = new DataView(dataTable);
            string[] sortData = Session["patientedithistory_sortexpression"].ToString().Trim().Split(' ');
            string newSortExpr = (e.SortExpression == sortData[0] && sortData[1] == "ASC") ? "DESC" : "ASC";
            dataView.Sort = e.SortExpression + " " + newSortExpr;
            Session["patientedithistory_sortexpression"] = e.SortExpression + " " + newSortExpr;

            GrdPatient.DataSource = dataView;
            GrdPatient.DataBind();
        }
    }

    #endregion


    #region SetErrorMessage, HideErrorMessage

    private void HideTableAndSetErrorMessage(string errMsg = "", string details = "")
    {
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