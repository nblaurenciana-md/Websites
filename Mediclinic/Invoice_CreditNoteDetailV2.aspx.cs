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

public partial class Invoice_CreditNoteDetailV2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            HideErrorMessage();
            Utilities.UpdatePageHeaderV2(Page.Master, true);

            if (!IsPostBack)
            {
                SetupGUI();

                UrlParamType urlParamType = GetUrlParamType();
                if ((urlParamType == UrlParamType.Edit || urlParamType == UrlParamType.View) && IsValidFormID())
                    FillEditViewForm(urlParamType == UrlParamType.Edit);
                else if (GetUrlParamType() == UrlParamType.Add && IsValidFormID())
                    FillEmptyAddForm();
                else
                    HideTableAndSetErrorMessage("", "Invalid URL Parameters");
            }

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


    #region GetUrlParamCard(), GetUrlParamType(), IsValidFormID(), GetFormID()

    private bool IsValidFormID()
    {
        string id = Request.QueryString["id"];
        return id != null && Regex.IsMatch(id, @"^\d+$");
    }
    private int GetFormID()
    {
        if (!IsValidFormID())
            throw new Exception("Invalid url id");

        string id = Request.QueryString["id"];
        return Convert.ToInt32(id);
    }

    private enum UrlParamType { Add, Edit, View, None };
    private UrlParamType GetUrlParamType()
    {
        string type = Request.QueryString["type"];
        if (type != null && type.ToLower() == "add")
            return UrlParamType.Add;
        //else if (type != null && type.ToLower() == "edit")
        //    return UrlParamType.Edit;
        else if (type != null && type.ToLower() == "view")
            return UrlParamType.View;
        else
            return UrlParamType.None;
    }

    #endregion

    #region SetupGUI()

    public void SetupGUI()
    {
        bool editable = GetUrlParamType() == UrlParamType.Add || GetUrlParamType() == UrlParamType.Edit;
        Utilities.SetEditControlBackColour(txtTotal,  editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtReason, editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
    }

    #endregion


    private void FillEditViewForm(bool isEditMode)
    {
        lblHeading.Text = isEditMode ? "Edit Credit Note" : "View Credit Note";
        Page.Title      = isEditMode ? "Edit Credit Note" : "View Credit Note";

        CreditNote creditNote = CreditNoteDB.GetByID(GetFormID());
        if (creditNote == null)
        {
            HideTableAndSetErrorMessage("Invalid Credit Note ID");
            return;
        }


        lblId.Text             = creditNote.CreditNoteID.ToString();
        lblInvoiceId.Text      = creditNote.Invoice.InvoiceID.ToString();
        lblCreditNoteDate.Text = creditNote.CreditNoteDateAdded.ToString("d MMM, yyyy");
        lblAddedBy.Text        = creditNote.Staff.Person.FullnameWithoutMiddlename;
        lblTotal.Font.Bold     = !isEditMode;


        if (isEditMode)
        {
            txtTotal.Text       = creditNote.Total.ToString();
            txtReason.Text      = creditNote.Reason;
            lblAmountOwing.Text = (InvoiceDB.GetByID(creditNote.Invoice.InvoiceID).TotalDue - creditNote.Total).ToString();
            lblTotal.Visible    = false;
            lblReason.Visible   = false;
        }
        else
        {
            lblTotal.Text         = creditNote.Total.ToString();
            lblReason.Text        = creditNote.Reason;
            amountOwedRow.Visible = false;
            txtTotal.Visible      = false;
            txtReason.Visible     = false;
        }



        if (isEditMode)
        {
            btnSubmit.Text = "Update Details";
        }
        else // is view mode
        {
            btnSubmit.Visible = false;
            btnCancel.Text = "Close";
        }
    }

    private void FillEmptyAddForm()
    {
        lblHeading.Text = "Add Adjustment Note";
        Page.Title      = "Add Adjustment Note";

        idRow.Visible = false;

        Invoice invoice = InvoiceDB.GetByID(GetFormID());
        if (invoice == null)
        {
            HideTableAndSetErrorMessage("Invalid invoice ID");
            return;
        }

        lblInvoiceId.Text   = invoice.InvoiceID.ToString();
        lblAmountOwing.Text = invoice.TotalDue.ToString();
        txtTotal.Text       = invoice.TotalDue.ToString();
        addedByRow.Visible        = false;
        creditnoteDateRow.Visible = false;
        lblReason.Visible         = false;

        btnSubmit.Text = "Add Adjustment Note";
        btnCancel.Visible = true;
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (GetUrlParamType() == UrlParamType.Edit)
        {
            Response.Redirect(UrlParamModifier.AddEdit(Request.RawUrl, "type", "view"));
            return;
        }

        // close this window
        Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>window.returnValue=false;self.close();</script>");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (GetUrlParamType() == UrlParamType.View)
        {
            Response.Redirect(UrlParamModifier.AddEdit(Request.RawUrl, "type", "edit"));
        }
        //else if (GetUrlParamType() == UrlParamType.Edit)
        //{
        //    if (!IsValidFormID())
        //    {
        //        HideTableAndSetErrorMessage();
        //        return;
        //    }
        //    Receipt receipt = ReceiptDB.GetByID(GetFormID());
        //    if (receipt == null)
        //    {
        //        HideTableAndSetErrorMessage("Invalid receipt ID");
        //        return;
        //    }

        //    ReceiptDB.Update(receipt.ReceiptID, Convert.ToInt32(ddlPaymentType.SelectedValue), Convert.ToDecimal(txtTotal.Text), Convert.ToDecimal(txtAmountReconciled.Text), chkFailedToClear.Checked, receipt.IsOverpaid, GetBankProcessedDateFromForm());

        //    Response.Redirect(UrlParamModifier.AddEdit(Request.RawUrl, "type", "view_only"));


        //    // close this window
        //    //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>window.returnValue=false;self.close();</script>");
        //}
        else if (GetUrlParamType() == UrlParamType.Add)
        {
            if (!IsValidFormID())
            {
                HideTableAndSetErrorMessage();
                return;
            }
            Invoice invoice = InvoiceDB.GetByID(GetFormID());
            if (invoice == null)
            {
                HideTableAndSetErrorMessage("Invalid invoice ID");
                return;
            }

            decimal thisCreditNoteAmount = Convert.ToDecimal(txtTotal.Text);
            decimal totalOwed            = invoice.TotalDue - thisCreditNoteAmount;
            bool    isOverPaid           = totalOwed <  0;
            bool    isPaid               = totalOwed <= 0;

            if (isOverPaid)
            {
                SetErrorMessage("Can not be more than amount owing.");
                return;
            }

            int credit_note_id = CreditNoteDB.Insert(invoice.InvoiceID, thisCreditNoteAmount, txtReason.Text, Convert.ToInt32(Session["StaffID"]));
            if (isPaid)
                InvoiceDB.UpdateIsPaid(null, invoice.InvoiceID, true);


            // close this window
            Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>window.returnValue=false;self.close();</script>");
        }
        else
        {
            HideTableAndSetErrorMessage("", "Invalid URL Parameters");
        }
    }


    #region SetErrorMessage, HideErrorMessage

    private void HideTableAndSetErrorMessage(string errMsg = "", string details = "")
    {
        maintable.Visible = false;
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