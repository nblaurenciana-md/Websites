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

public partial class InvoiceListV2 : System.Web.UI.Page
{
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["UpdateFromWebPay"] != null)
            {
                PaymentPendingDB.UpdateAllPaymentsPending(null, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(1), Convert.ToInt32(Session["StaffID"]));
                Session.Remove("UpdateFromWebPay");
            }

            if (!IsPostBack)
                Utilities.SetNoCache(Response);
            HideErrorMessage();

            divReinvoice.Visible = false;

            if (!IsPostBack)
            {
                PagePermissions.EnforcePermissions_RequireAny(Session, Response, true, true, true, false, false, false);
                Session.Remove("invoiceinfo_sortexpression");
                Session.Remove("invoiceinfo_data");
                SetupGUI();
                FillGrid();
                txtInvoiceNbrSearch.Focus();
            }

            this.GrdInvoice.EnableViewState = true;
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

    #endregion

    #region SetupGUI

    protected void SetupGUI()
    {
        chkIncMedicare.Checked  = IsValidFormIncMedicare()  ? GetFormIncMedicare(false)  : true;
        chkIncDVA.Checked       = IsValidFormIncDVA()       ? GetFormIncDVA(false)       : true;
        chkIncTAC.Checked       = IsValidFormIncTAC()       ? GetFormIncTAC(false)       : true;
        chkIncPrivate.Checked   = IsValidFormIncPrivate()   ? GetFormIncPrivate(false)   : true;
        chkIncPaid.Checked      = IsValidFormIncPaid()      ? GetFormIncPaid(false)      : true;
        chkIncUnpaid.Checked    = IsValidFormIncUnpaid()    ? GetFormIncUnpaid(false)    : true;
        chkViewReversed.Checked = IsValidFormViewReversed() ? GetFormViewReversed(false) : false;


        txtInvoiceNbrSearch.Text = GetFormInvoiceIDSearch();
        txtBookingNbrSearch.Text = GetFormBookingIDSearch();
        txtClaimNbrSearch.Text   = GetFormClaimNbrSearch();
    
        int patientID = GetID("patient", "Patient",     "patient_id");
        if (patientID != -1)
        {
            txtStartDate.Text = IsValidFormStartDate() ? (GetFormStartDate(false) == DateTime.MinValue ? "" : GetFormStartDate(false).ToString("dd-MM-yyyy")) : string.Empty;  //  DateTime.Now.AddYears(-1).ToString("dd-MM-yyyy"); // 
            txtEndDate.Text   = IsValidFormEndDate()   ? (GetFormEndDate(false)   == DateTime.MinValue ? "" : GetFormEndDate(false).ToString("dd-MM-yyyy"))   : string.Empty;  //  DateTime.Now.AddYears(1).ToString("dd-MM-yyyy");  // 
        }
        else
        {
            DateTime firstDayOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfThisMonth  = firstDayOfThisMonth.AddMonths(1).AddDays(-1);

            txtStartDate.Text = IsValidFormStartDate() ? (GetFormStartDate(false) == DateTime.MinValue ? "" : GetFormStartDate(false).ToString("dd-MM-yyyy")) : DateTime.Today.ToString("dd-MM-yyyy"); // firstDayOfThisMonth.ToString("dd-MM-yyyy");
            txtEndDate.Text   = IsValidFormEndDate()   ? (GetFormEndDate(false)   == DateTime.MinValue ? "" : GetFormEndDate(false).ToString("dd-MM-yyyy"))   : DateTime.Today.ToString("dd-MM-yyyy"); // lastDayOfThisMonth.ToString("dd-MM-yyyy");
        }

        ddlProviders.Style["width"] = "100%";
        ddlProviders.Items.Clear();
        ddlProviders.Items.Add(new ListItem("All Providers", (-1).ToString()));
        foreach (Staff curProv in StaffDB.GetAll())
            if (curProv.IsProvider)
                ddlProviders.Items.Add(new ListItem(curProv.Person.FullnameWithoutMiddlename, curProv.StaffID.ToString()));
        int providerID = GetID("provider", "Staff", "staff_id");
        ddlProviders.SelectedValue = providerID.ToString();


        if (IsValidFormOnlyRejected() && GetFormOnlyRejected(false))
        {
            txtStartDate.Text = "";
            txtEndDate.Text   = "";
            div_search_section.Visible = false;
        }



        txtStartDate_Picker.OnClientClick = "displayDatePicker('txtStartDate', this, 'dmy', '-'); return false;";
        txtEndDate_Picker.OnClientClick   = "displayDatePicker('txtEndDate', this, 'dmy', '-'); return false;";
    }

    #endregion

    #region GetUrlParamType(),  IsValidFormParam(),GetFormParam()....


    protected bool IsValidDate(string strDate)
    {
        try
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(strDate, @"^\d{2}\-\d{2}\-\d{4}$"))
                return false;

            string[] parts = strDate.Split('-');
            DateTime d = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public DateTime GetDate(string inDate)
    {
        inDate = inDate.Trim();

        if (inDate.Length == 0)
        {
            return DateTime.MinValue;
        }
        else
        {
            string[] dobParts = inDate.Split(new char[] { '-' });
            return new DateTime(Convert.ToInt32(dobParts[2]), Convert.ToInt32(dobParts[1]), Convert.ToInt32(dobParts[0]));
        }
    }


    protected static Hashtable _validParams;
    protected bool IsValidParam(string param)
    {
        if (_validParams == null)
        {
            _validParams = new Hashtable();
            _validParams["entity"] = 1;
            _validParams["patient"] = 1;
            _validParams["orgs"] = 1;
            _validParams["provider"] = 1;
        }

        return _validParams.Contains(param);
    }
    protected bool IsValidUrlParam(string paranName)
    {
        if (!IsValidParam(paranName))
            throw new CustomMessageException("Invalid param name: " + paranName);

        string id = Request.QueryString[paranName];
        return id != null && Regex.IsMatch(id, @"^\d+$");
    }
    protected bool IsValidUrlParams(string paranName)
    {
        if (!IsValidParam(paranName))
            throw new CustomMessageException("Invalid param name: " + paranName);

        string ids = Request.QueryString[paranName];
        return ids != null && Regex.IsMatch(ids, @"^([0-9]+_)*[0-9]+$");
    }
    protected int GetUrlParam(string paranName)
    {
        if (!IsValidUrlParam(paranName))
            throw new Exception("Invalid " + paranName);
        return Convert.ToInt32(Request.QueryString[paranName]);
    }
    protected string GetUrlParams(string paranName)
    {
        if (!IsValidUrlParams(paranName))
            throw new Exception("Invalid " + paranName);
        return  Request.QueryString[paranName].Replace("_", ",");
    }

    protected bool IsValidFormIncMedicare()
    {
        string inc_medicare = Request.QueryString["inc_medicare"];
        return inc_medicare != null && (inc_medicare == "0" || inc_medicare == "1");
    }
    protected bool GetFormIncMedicare(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncMedicare())
            throw new Exception("Invalid url 'inc_medicare'");
        return Request.QueryString["inc_medicare"] == "1";
    }
    protected bool IsValidFormIncDVA()
    {
        string inc_dva = Request.QueryString["inc_dva"];
        return inc_dva != null && (inc_dva == "0" || inc_dva == "1");
    }
    protected bool GetFormIncDVA(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncDVA())
            throw new Exception("Invalid url 'inc_dva'");
        return Request.QueryString["inc_dva"] == "1";
    }
    protected bool IsValidFormIncTAC()
    {
        string inc_tac = Request.QueryString["inc_tac"];
        return inc_tac != null && (inc_tac == "0" || inc_tac == "1");
    }
    protected bool GetFormIncTAC(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncTAC())
            throw new Exception("Invalid url 'inc_tac'");
        return Request.QueryString["inc_tac"] == "1";
    }
    protected bool IsValidFormIncPrivate()
    {
        string inc_private = Request.QueryString["inc_private"];
        return inc_private != null && (inc_private == "0" || inc_private == "1");
    }
    protected bool GetFormIncPrivate(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncPrivate())
            throw new Exception("Invalid url 'inc_private'");
        return Request.QueryString["inc_private"] == "1";
    }
    protected bool IsValidFormIncPaid()
    {
        string inc_paid = Request.QueryString["inc_paid"];
        return inc_paid != null && (inc_paid == "0" || inc_paid == "1");
    }
    protected bool GetFormIncPaid(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncPaid())
            throw new Exception("Invalid url 'inc_paid'");
        return Request.QueryString["inc_paid"] == "1";
    }
    protected bool IsValidFormIncUnpaid()
    {
        string inc_unpaid = Request.QueryString["inc_unpaid"];
        return inc_unpaid != null && (inc_unpaid == "0" || inc_unpaid == "1");
    }
    protected bool GetFormIncUnpaid(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncUnpaid())
            throw new Exception("Invalid url 'inc_unpaid'");
        return Request.QueryString["inc_unpaid"] == "1";
    }
    protected bool IsValidFormIncReversed()
    {
        string inc_reversed = Request.QueryString["inc_reversed"];
        return inc_reversed != null && (inc_reversed == "0" || inc_reversed == "1");
    }
    protected bool GetFormIncReversed(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormIncReversed())
            throw new Exception("Invalid url 'inc_reversed'");
        return Request.QueryString["inc_reversed"] == "1";
    }
    protected bool IsValidFormViewReversed()
    {
        string view_reversed = Request.QueryString["view_reversed"];
        return view_reversed != null && (view_reversed == "0" || view_reversed == "1");
    }
    protected bool GetFormViewReversed(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormViewReversed())
            throw new Exception("Invalid url 'view_reversed'");
        return Request.QueryString["view_reversed"] == "1";
    }
    protected bool IsValidFormOnlyRejected()
    {
        string only_rejected = Request.QueryString["only_rejected"];
        return only_rejected != null && (only_rejected == "0" || only_rejected == "1");
    }
    protected bool GetFormOnlyRejected(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormOnlyRejected())
            throw new Exception("Invalid url 'only_rejected'");
        return Request.QueryString["only_rejected"] == "1";
    }

    protected string GetFormInvoiceIDSearch()
    {
        return Request.QueryString["invoice_nbr_search"] == null ? "" : Request.QueryString["invoice_nbr_search"];
    }
    protected string GetFormBookingIDSearch()
    {
        return Request.QueryString["booking_nbr_search"] == null ? "" : Request.QueryString["booking_nbr_search"];
    }
    protected string GetFormClaimNbrSearch()
    {
        return Request.QueryString["claim_nbr_search"] == null ? "" : Request.QueryString["claim_nbr_search"];
    }



    protected bool IsValidFormStartDate()
    {
        string start_date = Request.QueryString["start_date"];
        return start_date != null && (start_date.Length == 0 || Regex.IsMatch(start_date, @"^\d{4}_\d{2}_\d{2}$"));
    }
    protected DateTime GetFormStartDate(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormStartDate())
            throw new Exception("Invalid url 'start date'");
        return Request.QueryString["start_date"].Length == 0 ? DateTime.MinValue : GetDateFromString(Request.QueryString["start_date"], "yyyy_mm_dd");
    }
    protected bool IsValidFormEndDate()
    {
        string end_date = Request.QueryString["end_date"];
        return end_date != null && (end_date.Length == 0 || Regex.IsMatch(end_date, @"^\d{4}_\d{2}_\d{2}$"));
    }
    protected DateTime GetFormEndDate(bool checkIsValid = true)
    {
        if (checkIsValid && !IsValidFormEndDate())
            throw new Exception("Invalid url 'end date'");
        return Request.QueryString["end_date"].Length == 0 ? DateTime.MinValue : GetDateFromString(Request.QueryString["end_date"], "yyyy_mm_dd");
    }
    protected DateTime GetDateFromString(string sDate, string format)
    {
        if (format == "yyyy_mm_dd")
        {
            string[] dateparts = sDate.Split('_');
            return new DateTime(Convert.ToInt32(dateparts[0]), Convert.ToInt32(dateparts[1]), Convert.ToInt32(dateparts[2]));
        }
        else if (format == "dd_mm_yyyy")
        {
            string[] dateparts = sDate.Split('_');
            return new DateTime(Convert.ToInt32(dateparts[2]), Convert.ToInt32(dateparts[1]), Convert.ToInt32(dateparts[0]));
        }
        if (format == "yyyy-mm-dd")
        {
            string[] dateparts = sDate.Split('-');
            return new DateTime(Convert.ToInt32(dateparts[0]), Convert.ToInt32(dateparts[1]), Convert.ToInt32(dateparts[2]));
        }
        else if (format == "dd-mm-yyyy")
        {
            string[] dateparts = sDate.Split('-');
            return new DateTime(Convert.ToInt32(dateparts[2]), Convert.ToInt32(dateparts[1]), Convert.ToInt32(dateparts[0]));
        }
        else
            throw new ArgumentOutOfRangeException("Unknown date format");
    }


    protected void ValidBankProcessedDateDateCheck(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv = (CustomValidator)sender;
            GridViewRow grdRow = ((GridViewRow)cv.Parent.Parent);
            TextBox txtDate = (TextBox)grdRow.FindControl("txtBankProcessedDate");

            if (!IsValidDate(txtDate.Text))
                throw new Exception();

            DateTime d = GetDate(txtDate.Text);

            e.IsValid = (d == DateTime.MinValue) || (Utilities.IsValidDBDateTime(d) && Utilities.IsValidDOB(d));
        }
        catch (Exception)
        {
            e.IsValid = false;
        }
    }


    #endregion

    #region GrdInvoice

    private int GetID(string urlParamName, string table, string dbField, int defaultValue = -1)
    {
        if (!IsValidParam(urlParamName))
            throw new CustomMessageException("Invalid param name: " + urlParamName);
        return (IsValidUrlParam(urlParamName) && DBBase.GetGenericCount(table, dbField + "=" + GetUrlParam(urlParamName)) > 0) ? GetUrlParam(urlParamName) : defaultValue;
    }
    private string GetIDs(string urlParamName, string table, string dbField, string defaultValue = null)
    {
        if (!IsValidParam(urlParamName))
            throw new CustomMessageException("Invalid param name: " + urlParamName);

        if (!IsValidUrlParams(urlParamName))
            return defaultValue;

        string paramList = GetUrlParams(urlParamName);
        int count = paramList.Split(',').Length - 1;

        return (IsValidUrlParams(urlParamName) && DBBase.GetGenericCount(table, dbField + " IN (" + paramList + ")") >= count) ? paramList : defaultValue;
    }

    protected Hashtable _lettersHash = null;
    protected Hashtable GetLettersHash()
    {
        if (_lettersHash == null)
        {
            _lettersHash = new Hashtable();
            foreach (DataRow row in LetterDB.GetDataTable().Rows)
            {
                Letter letter = LetterDB.LoadAll(row);
                _lettersHash[letter.LetterID] = letter;
            }
        }
        return _lettersHash;
    }

    protected void FillGrid()
    {
        bool viewRejected = IsValidFormOnlyRejected() && GetFormOnlyRejected(false);

        DateTime fromDate = IsValidDate(txtStartDate.Text) ? GetDate(txtStartDate.Text)                              : DateTime.MinValue;
        DateTime toDate   = IsValidDate(txtEndDate.Text)   ? GetDate(txtEndDate.Text).Add(new TimeSpan(23, 59, 59))  : DateTime.MinValue;

        int    patientID  = GetID("patient", "Patient", "patient_id");
        string orgIDs     = GetIDs("orgs", "Organisation", "organisation_id");
        int    providerID = GetID("provider", "Staff", "staff_id");

        //int[]  orgIdList = orgIDs == null ? new int[] {} : orgIDs.Split(',').Select(s => int.Parse(s)).ToArray();
        if (patientID != -1)
        {
            Patient patient = PatientDB.GetByID(patientID);
            lblHeading.Text = "Invoices For ";
            lnkToEntity.Text = "<a href='PatientDetailV2.aspx?type=view&id=" + patient.PatientID + "'>" + patient.Person.FullnameWithoutMiddlename + "</a>";
        }

        else if (orgIDs != null)
        {
            string output = string.Empty;

            Hashtable orgIdsSeen = new Hashtable();

            int[] orgIdList = orgIDs == null ? new int[] { } : orgIDs.Split(',').Select(s => int.Parse(s)).ToArray();
            Hashtable orgHash = OrganisationDB.GetAllHashtable(true, true, false, false, false, true);

            int maxTextLength = 0;
            for (int i = 0; i < orgIdList.Length; i++)
            {
                Organisation curOrgOfList = (Organisation)orgHash[orgIdList[i]];

                Organisation[] flattenedTree = OrganisationTree.GetFlattenedTree(null, curOrgOfList.IsDeleted, orgIdList[i], true, "139,367,372,218");
                for (int j = 0; j < flattenedTree.Length; j++)
                {
                    Organisation curOrg = flattenedTree[j];
                    orgIdsSeen[curOrg.OrganisationID] = 1;

                    for (int k = 0; k < curOrg.TreeLevel; k++)
                        output += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    output += "・ <a href='/OrganisationDetailV2.aspx?type=view&id=" + curOrg.OrganisationID + "'>" + curOrg.Name + "</a>" + "<br />";

                    int textLength = 6 * curOrg.TreeLevel + 2 + curOrg.Name.Length;
                    if (textLength > maxTextLength) maxTextLength = textLength;
                }
            }

            if (orgIdsSeen.Keys.Count <= 1) output = output.Replace("・ ", "");
            if (output.EndsWith("<br />"))  output = output.Substring(0, output.Length - "<br />".Length);

            lblHeading.Text = "Invoices For ";

            string headingFormatStartTag = "";
            string headingFormatEndTag   = "";
            if (orgIdsSeen.Keys.Count == 1) { headingFormatStartTag = ""; headingFormatEndTag = ""; }
            else if (maxTextLength < 30)    { headingFormatStartTag = "";  headingFormatEndTag = "";  }

            lnkToEntity.Text = headingFormatStartTag + output + headingFormatEndTag;


            // reset org ids for query below
            orgIDs = string.Empty;
            foreach (int key in orgIdsSeen.Keys)
                orgIDs += (orgIDs.Length == 0 ? "" : ",") + key;
        }
        else
        {
            if (viewRejected)
            {
                lblHeading.Text = "Rejected Invoices Not Yet Paid, Wiped, or Reinvoiced";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "Rejected Invoices";
            }
            else if (chkIncMedicare.Checked && !chkIncDVA.Checked && !chkIncTAC.Checked && !chkIncPrivate.Checked)
            {
                lblHeading.Text = "Medicare Invoices";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "Medicare Invoices";
            }
            else if (!chkIncMedicare.Checked && chkIncDVA.Checked && !chkIncTAC.Checked && !chkIncPrivate.Checked)
            {
                lblHeading.Text = "DVA Invoices";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "DVA Invoices";
            }
            else if (!chkIncMedicare.Checked && !chkIncDVA.Checked && chkIncTAC.Checked && !chkIncPrivate.Checked)
            {
                lblHeading.Text = "Insurance Invoices";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "Insurance Invoices";
            }
            else if (!chkIncMedicare.Checked && !chkIncDVA.Checked && !chkIncTAC.Checked && chkIncPrivate.Checked)
            {
                lblHeading.Text = "PT Payable Invoices";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "PT Payable Invoices";
            }
            else
            {
                lblHeading.Text = "Invoices";
                Page.Title = ((SystemVariables)Session["SystemVariables"])["Site"].Value + " - " + "Invoices";
            }
        }


        Hashtable rejectLetterCodes = new Hashtable();
        DataTable tbl = LetterDB.GetDataTable(Convert.ToInt32(Session["SiteID"]));
        foreach (DataRow row in tbl.Rows)
        {
            Letter letter = LetterDB.LoadAll(row);
            rejectLetterCodes[letter.LetterID] = letter.Code;
        }



        DataTable dt = null;
        if (txtInvoiceNbrSearch.Text.Trim().Length > 0 || txtBookingNbrSearch.Text.Trim().Length > 0 || txtClaimNbrSearch.Text.Trim().Length > 0)
            dt = InvoiceDB.GetDataTable(false, DateTime.MinValue, DateTime.MinValue, true, true, null,  -1, patientID, providerID, orgIDs, true, true, true, true, true, true, false, txtInvoiceNbrSearch.Text.Trim(), txtBookingNbrSearch.Text.Trim(), txtClaimNbrSearch.Text.Trim());
        else if (viewRejected)
            dt = InvoiceDB.GetDataTable(false, DateTime.MinValue, DateTime.MinValue, true, false, null, Convert.ToInt32(Session["SiteID"]), patientID, providerID, orgIDs, true, true, true, true, false, true, true, string.Empty, string.Empty, string.Empty);
        else
        {
            // if getting more than 28 days, its just faster to get all invoices and then remove them in the code instead of the database
            if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue && toDate.Subtract(fromDate).TotalDays <= 28)
            {
                dt = InvoiceDB.GetDataTable(false, fromDate, toDate, !chkViewReversed.Checked, chkViewReversed.Checked, null, Convert.ToInt32(Session["SiteID"]), patientID, providerID, orgIDs, chkIncMedicare.Checked, chkIncDVA.Checked, chkIncTAC.Checked, chkIncPrivate.Checked, chkIncPaid.Checked, chkIncUnpaid.Checked, false, txtInvoiceNbrSearch.Text.Trim(), txtBookingNbrSearch.Text.Trim(), txtClaimNbrSearch.Text.Trim());
            }
            else
            {
                dt = InvoiceDB.GetDataTable(false, DateTime.MinValue, DateTime.MinValue, !chkViewReversed.Checked, chkViewReversed.Checked, null, Convert.ToInt32(Session["SiteID"]), patientID, providerID, orgIDs, chkIncMedicare.Checked, chkIncDVA.Checked, chkIncTAC.Checked, chkIncPrivate.Checked, chkIncPaid.Checked, chkIncUnpaid.Checked, false, txtInvoiceNbrSearch.Text.Trim(), txtBookingNbrSearch.Text.Trim(), txtClaimNbrSearch.Text.Trim());
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DateTime invDate = Convert.ToDateTime(dt.Rows[i]["inv_invoice_date_added"]);
                    if ((fromDate != DateTime.MinValue && invDate < fromDate) || (toDate != DateTime.MinValue && invDate > toDate))
                        dt.Rows.RemoveAt(i);
                }
            }
        }

        InvoiceDB.AddTotalDueColumn(ref dt);

        // get invoicelines for aged care bookings (ie no patient in the booking as there are multiple patients in the bookingpatient table)
        ArrayList invoicesNoBkPt = new ArrayList();
        for (int i = 0; i < dt.Rows.Count; i++)
            if (dt.Rows[i]["inv_payer_patient_id"] == DBNull.Value && dt.Rows[i]["booking_patient_id"] == DBNull.Value)
                invoicesNoBkPt.Add(InvoiceDB.LoadAll(dt.Rows[i], false));
        Hashtable invoiceLineHash = InvoiceLineDB.GetBulkInvoiceLinesByInvoiceID((Invoice[])invoicesNoBkPt.ToArray(typeof(Invoice)));



        Hashtable staffHash = StaffDB.GetAllInHashtable(true, true, false, false);
        bool autoMedicareClaiming = Convert.ToInt32(SystemVariableDB.GetByDescr("AutoMedicareClaiming").Value) == 1;
        
        dt.Columns.Add("inv_reject_letter_code",typeof(String));
        dt.Columns.Add("provider_person_firstname", typeof(String));
        dt.Columns.Add("provider_person_surname", typeof(String));
        dt.Columns.Add("added_by_reversed_by_row", typeof(String));
        dt.Columns.Add("reversed_by_col", typeof(String));
        dt.Columns.Add("add_claim_nbr_link_visible", typeof(Boolean));
        if (!dt.Columns.Contains("booking_patient_person_firstname")) dt.Columns.Add("booking_patient_person_firstname", typeof(String));
        if (!dt.Columns.Contains("booking_patient_person_surname"))   dt.Columns.Add("booking_patient_person_surname",   typeof(String));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["inv_reject_letter_code"] = dt.Rows[i]["inv_reject_letter_id"] == DBNull.Value ? DBNull.Value : rejectLetterCodes[Convert.ToInt32(dt.Rows[i]["inv_reject_letter_id"])];

            Staff provider = dt.Rows[i]["booking_provider"] == DBNull.Value ? null : (Staff)staffHash[Convert.ToInt32(dt.Rows[i]["booking_provider"])];
            dt.Rows[i]["provider_person_firstname"] = provider == null ? "" : provider.Person.Firstname;
            dt.Rows[i]["provider_person_surname"]   = provider == null ? "" : provider.Person.Surname;


            Invoice curInvoice = InvoiceDB.LoadAll(dt.Rows[i], false);

            string addedBy      = curInvoice.Staff      == null || staffHash[curInvoice.Staff.StaffID]      == null ? "" : ((Staff)staffHash[curInvoice.Staff.StaffID]).Person.FullnameWithoutMiddlename;
            string addedDate    = curInvoice.InvoiceDateAdded == DateTime.MinValue                                  ? "" : curInvoice.InvoiceDateAdded.ToString("MMM d, yyyy");
            string reversedBy   = curInvoice.ReversedBy == null || staffHash[curInvoice.ReversedBy.StaffID] == null ? "" : ((Staff)staffHash[curInvoice.ReversedBy.StaffID]).Person.FullnameWithoutMiddlename;
            string reversedDate = curInvoice.ReversedDate == DateTime.MinValue                                      ? "" : curInvoice.ReversedDate.ToString("MMM d, yyyy");
            
            string added_by_deleted_by_row = string.Empty;
            added_by_deleted_by_row += "Added By: " + addedBy + " (" + addedDate + ")";
            if (reversedBy.Length > 0 || reversedDate.Length > 0)
                added_by_deleted_by_row += "\r\nReversed By: " + reversedBy + " (" + reversedDate + ")";
            dt.Rows[i]["added_by_reversed_by_row"] = added_by_deleted_by_row;

            string reversed_by_col = string.Empty;
            if (reversedBy.Length > 0 || reversedDate.Length > 0)
                reversed_by_col += reversedBy + " (" + reversedDate + ")";
            dt.Rows[i]["reversed_by_col"] = reversed_by_col;

            dt.Rows[i]["add_claim_nbr_link_visible"] = curInvoice.InvoiceDateAdded >= new DateTime(2013, 7, 1) && autoMedicareClaiming && curInvoice.HealthcareClaimNumber == string.Empty && (curInvoice.PayerOrganisation != null && (curInvoice.PayerOrganisation.OrganisationID == -1 || curInvoice.PayerOrganisation.OrganisationID == -2));




            // if aged care invoice with no patients, then check if only one invoice line .. if so use that patient
            if (dt.Rows[i]["inv_payer_patient_id"] == DBNull.Value && dt.Rows[i]["booking_patient_id"] == DBNull.Value)
            {
                InvoiceLine[] invoiceLines = (InvoiceLine[])invoiceLineHash[curInvoice.InvoiceID];

                Hashtable ptIDs = new Hashtable();
                foreach (InvoiceLine invoiceLine in invoiceLines)
                    ptIDs[invoiceLine.Patient.PatientID] = 1;

                if (ptIDs.Keys.Count == 1)
                {
                    dt.Rows[i]["booking_patient_id"]               = invoiceLines[0].Patient.PatientID;
                    dt.Rows[i]["booking_patient_person_firstname"] = (invoiceLines[0].Patient.PatientID == -1) ? "" : invoiceLines[0].Patient.Person.Firstname;
                    dt.Rows[i]["booking_patient_person_surname"]   = (invoiceLines[0].Patient.PatientID == -1) ? "" : invoiceLines[0].Patient.Person.Surname;
                }

            }
        }
        
        
        Session["invoiceinfo_data"] = dt;

        if (dt.Rows.Count > 0)
        {
            if (IsPostBack && Session["invoiceinfo_sortexpression"] != null && Session["invoiceinfo_sortexpression"].ToString().Length > 0)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = Session["invoiceinfo_sortexpression"].ToString();
                GrdInvoice.DataSource = dataView;
            }
            else
            {
                //GrdInvoice.DataSource = dt;
                DataView dataView = new DataView(dt);
                dataView.Sort = "inv_invoice_date_added";
                GrdInvoice.DataSource = dataView;
            }


            try
            {
                GrdInvoice.DataBind();
                GrdInvoice.PagerSettings.FirstPageText = "1";
                GrdInvoice.PagerSettings.LastPageText = GrdInvoice.PageCount.ToString();
                GrdInvoice.DataBind();

                bool containsSetPaidButtons       = false;
                bool containsSetWipedButtons      = false;
                bool containsAddReceiptButtons    = false;
                for (int i = 0; i < GrdInvoice.Rows.Count; i++)
                {
                    if (GrdInvoice.Rows[i].RowType != DataControlRowType.Pager)
                    {
                        Button btnSetPaid       = (Button)GrdInvoice.Rows[i].FindControl("btnSetPaid");
                        Button btnSetWiped      = (Button)GrdInvoice.Rows[i].FindControl("SetWiped");
                        Button btnAddReceipt    = (Button)GrdInvoice.Rows[i].FindControl("btnAddReceipt");

                        if (btnSetPaid       != null && btnSetPaid.Visible)
                            containsSetPaidButtons = true;
                        if (btnSetWiped      != null && btnSetWiped.Visible)
                            containsSetWipedButtons = true;
                        if (btnAddReceipt    != null && btnAddReceipt.Visible)
                            containsAddReceiptButtons = true;
                    }
                }
                for(int i=0; i<GrdInvoice.Columns.Count; i++)
                {
                    if (GrdInvoice.Columns[i].HeaderText == "Set Paid")
                        GrdInvoice.Columns[i].Visible = containsSetPaidButtons    || containsSetWipedButtons;
                    if (GrdInvoice.Columns[i].HeaderText == "Add Rcpt/Cr.Note")
                        GrdInvoice.Columns[i].Visible = containsAddReceiptButtons;
                }

                
            }
            catch (Exception ex)
            {
                HideTableAndSetErrorMessage("", ex.ToString());
            }
        }
        else
        {
            dt.Rows.Add(dt.NewRow());
            GrdInvoice.DataSource = dt;
            GrdInvoice.DataBind();

            int TotalColumns = GrdInvoice.Rows[0].Cells.Count;
            GrdInvoice.Rows[0].Cells.Clear();
            GrdInvoice.Rows[0].Cells.Add(new TableCell());
            GrdInvoice.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            GrdInvoice.Rows[0].Cells[0].Text = "No Record Found";
        }
    }
    protected void GrdInvoice_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
            return;

        // for some reason, on postback this is not set
        chkViewReversed.Checked = IsValidFormViewReversed() ? GetFormViewReversed(false) : false;

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            foreach (DataControlField col in GrdInvoice.Columns)
            {
                if (col.HeaderText.ToLower().Trim() == "type")
                    e.Row.Cells[GrdInvoice.Columns.IndexOf(col)].CssClass = "hiddencol";

                if (!chkViewReversed.Checked && col.HeaderText.ToLower().Trim() == "reversed by")
                    e.Row.Cells[GrdInvoice.Columns.IndexOf(col)].CssClass = "hiddencol";

                if (!chkViewReversed.Checked && col.HeaderText.ToLower().Trim() == "un-reverse")
                    e.Row.Cells[GrdInvoice.Columns.IndexOf(col)].CssClass = "hiddencol";
            }
        }

    }
    protected void GrdInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable docTypes = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "InvoiceType", "", "descr", "invoice_type_id", "descr");
        DataTable sites      = SiteDB.GetDataTable();
        DataTable orgs       = OrganisationDB.GetDataTable_GroupOrganisations();
        DataTable rejLetters = LetterDB.GetDataTable(Convert.ToInt32(Session["SiteID"]));

        DataTable dt = Session["invoiceinfo_data"] as DataTable;
        bool tblEmpty = (dt.Rows.Count == 1 && dt.Rows[0][0] == DBNull.Value);
        if (!tblEmpty && e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink lnkId = (HyperLink)e.Row.FindControl("lnkId");
            DataRow[] foundRows = dt.Select("inv_invoice_id=" + lnkId.Text);
            DataRow thisRow = foundRows[0];

            Invoice invoice      = InvoiceDB.LoadAll(thisRow, false);
            bool    canReInvoice = invoice.RejectLetter != null && !invoice.IsPaID;

            if (lnkId != null)
            {
                string url = "Invoice_ViewV2.aspx?invoice_id=" + invoice.InvoiceID;
                lnkId.Attributes["onclick"] = "open_new_tab('" + url + "');return false;";
                lnkId.NavigateUrl = url;
            }

            LinkButton btnReversedMessageHoverToolTip = (LinkButton)e.Row.FindControl("btnReversedMessageHoverToolTip");
            if (btnReversedMessageHoverToolTip != null)
                btnReversedMessageHoverToolTip.Visible = invoice.IsReversed;

            LinkButton lnkUnReverse = (LinkButton)e.Row.FindControl("lnkUnReverse");
            if (lnkUnReverse != null)
                lnkUnReverse.Visible = invoice.IsReversed;

            LinkButton lnkAllFromBooking = (LinkButton)e.Row.FindControl("lnkAllFromBooking");
            if (lnkAllFromBooking != null)
                lnkAllFromBooking.Visible = invoice.Booking != null;

            

            int letter_type_id = -1;
            
            Label lblPayerPagePopup = (Label)e.Row.FindControl("lblPayerPagePopup");
            if (lblPayerPagePopup != null)
            {

                if (thisRow["inv_payer_organisation_id"] != DBNull.Value)
                {
                    lblPayerPagePopup.Text = (invoice.PayerOrganisation.OrganisationID == -1 || invoice.PayerOrganisation.OrganisationID == -2 || invoice.PayerOrganisation.OrganisationType.OrganisationTypeID == 150) ?
                        invoice.PayerOrganisation.Name.ToString() :
                        "<a href=\"#\" onclick=\"open_new_tab('OrganisationDetailV2.aspx?type=view&id=" + invoice.PayerOrganisation.OrganisationID + "');return false;\">" + invoice.PayerOrganisation.Name + "</a>";


                    // 234 = medicare
                    // 235 = dva
                    // 214 = reject invoice for a facility
                    // 3   = person in aged care home
                    if (invoice.PayerOrganisation.OrganisationID == -1)
                        letter_type_id = 234;
                    else if (invoice.PayerOrganisation.OrganisationID == -2)
                        letter_type_id = 235;
                    else 
                        letter_type_id = 214;
                }
                else if (thisRow["inv_payer_patient_id"] != DBNull.Value)
                {
                    lblPayerPagePopup.Text = "<a href=\"#\" onclick=\"open_new_tab('PatientDetailV2.aspx?type=view&id=" + invoice.PayerPatient.PatientID + "');return false;\">" + invoice.PayerPatient.Person.FullnameWithoutMiddlename + "</a>";
                    letter_type_id = 3;
                }
                else
                {
                    if (invoice.Booking != null && invoice.Booking.Patient != null)
                    {
                        // can add this query each row because in the whole system there is only 32 invoices that get to here
                        // since the rest keep the patient as the payer_patient
                        // and doing this for only 32 rows avoids pulling all the extra data for all invoices so its faster doing this

                        Booking booking = BookingDB.GetByID(invoice.Booking.BookingID);
                        lblPayerPagePopup.Text = "<a href=\"#\" onclick=\"open_new_tab('PatientDetailV2.aspx?type=view&id=" + booking.Patient.PatientID + "');return false;\">" + booking.Patient.Person.FullnameWithoutMiddlename + "</a>";
                        letter_type_id = 3;
                    }

                    else // no debtor for some cash invoices
                    {
                        lblPayerPagePopup.Text = string.Empty;
                    }

                }
            }




            //DropDownList ddlPayingOrganisation = (DropDownList)e.Row.FindControl("ddlPayerOrganisation");
            //if (ddlPayingOrganisation != null)
            //{
            //    ddlPayingOrganisation.Items.Add(new ListItem("--", "0"));
            //    foreach (DataRow row in orgs.Rows)
            //        ddlPayingOrganisation.Items.Add(new ListItem(row["name"].ToString(), row["organisation_id"].ToString()));
            //    ddlPayingOrganisation.SelectedValue = thisRow["inv_payer_organisation_id"].ToString();
            //}

            DropDownList ddlInvoiceType = (DropDownList)e.Row.FindControl("ddlInvoiceType");
            if (ddlInvoiceType != null)
            {
                ddlInvoiceType.DataSource = docTypes;
                ddlInvoiceType.DataTextField = "descr";
                ddlInvoiceType.DataValueField = "invoice_type_id";
                ddlInvoiceType.DataBind();
                ddlInvoiceType.SelectedValue = thisRow["inv_invoice_type_id"].ToString();
            }


            DropDownList ddlMedicareRejectionCode = (DropDownList)e.Row.FindControl("ddlMedicareRejectionCode");
            if (ddlMedicareRejectionCode != null)
            {
                // only can set this if unpaid and for medicare/dva
                bool canSetRejected = !invoice.IsPaID && invoice.PayerOrganisation != null && (invoice.PayerOrganisation.OrganisationID == -1 || invoice.PayerOrganisation.OrganisationID == -2);

                ddlMedicareRejectionCode.Visible = canSetRejected;

                if (canSetRejected)
                {
                    ddlMedicareRejectionCode.Items.Clear();
                    ddlMedicareRejectionCode.Items.Add(new ListItem("--", "-1"));
                    for (int i = 0; i < rejLetters.Rows.Count; i++)
                    {
                        Letter letter = LetterDB.LoadAll(rejLetters.Rows[i]);
                        if ((invoice.RejectLetter != null && letter.LetterID == invoice.RejectLetter.LetterID) || (letter.LetterType.ID == letter_type_id && letter.Code.Length > 0 && (letter.Docname.Length == 0 || letter.FileExists(Convert.ToInt32(Session["SiteID"])))))
                            ddlMedicareRejectionCode.Items.Add(new ListItem(letter.Code, letter.LetterID.ToString()));
                    }
                    if (invoice.RejectLetter != null)
                        ddlMedicareRejectionCode.SelectedValue = invoice.RejectLetter.LetterID.ToString();
                }
            }


            Label      lblMedicareRejectionCode = (Label)e.Row.FindControl("lblMedicareRejectionCode");
            LinkButton btnReInvoice             = (LinkButton)e.Row.FindControl("btnReInvoice");
            if (lblMedicareRejectionCode != null)
            {
                //lblMedicareRejectionCode.Visible   = canReInvoice;
                btnReInvoice.Visible               = canReInvoice;

                if (canReInvoice)
                {
                    Hashtable allLetters = GetLettersHash();
                    Letter letter = (Letter)allLetters[invoice.RejectLetter.LetterID];

                    bool isMedicareOrDVA = invoice.PayerOrganisation != null && (invoice.PayerOrganisation.OrganisationID == -1 || invoice.PayerOrganisation.OrganisationID == -2);
                    if (isMedicareOrDVA)
                    {
                        if (letter.IsAllowedReclaim)
                        {
                            //btnReInvoice.OnClientClick = "javascript:if (!confirm('This will re-issue a new " + invoice.PayerOrganisation.Name + " invoice to reclaim. Are you sure you want to continue?')) return false;";
                        }
                        else
                        {

                            //string newDebtor = "";
                            //if (invoice.PayerPatient != null)
                            //{
                            //    newDebtor = invoice.PayerPatient.Person.FullnameWithoutMiddlename;
                            //}
                            //else 
                            //{
                            //    Booking bk = BookingDB.GetByID(invoice.Booking.BookingID);
                            //    newDebtor = bk.Patient == null ? "" : bk.Patient.Person.FullnameWithoutMiddlename;
                            //}

                            //btnReInvoice.OnClientClick = "javascript:if (!confirm('This will re-issue a PRIVATE invoice for " + newDebtor + ". Are you sure you want to continue?')) return false;";
                        }
                    }
                    else
                    {
                        btnReInvoice.OnClientClick = "javascript:alert('Can not reclaim this - the code has not been implemented yet.'); return false;";
                    }

                }

            }


            Label lblPatientPagePopup = (Label)e.Row.FindControl("lblPatientPagePopup");
            if (lblPatientPagePopup != null)
            {
                //lblPatientPagePopup.Visible = canReInvoice;
                //if (canReInvoice)
                //{

                    if (invoice.Booking != null)
                    {
                        Booking booking = BookingDB.GetByID(invoice.Booking.BookingID);
                        string patientLink = booking.Patient == null ?
                            "<a href=\"#\" onclick=\"open_new_tab('PatientDetailV2.aspx?type=view&id=" + thisRow["booking_patient_id"] + "');return false;\">" + thisRow["booking_patient_person_firstname"] + " " + thisRow["booking_patient_person_surname"] + "</a>" :
                            "<a href=\"#\" onclick=\"open_new_tab('PatientDetailV2.aspx?type=view&id=" + booking.Patient.PatientID + "');return false;\">" + booking.Patient.Person.FullnameWithoutMiddlename + "</a>";

                        lblPatientPagePopup.Text = patientLink;
                    }
                    else // for private invoices
                    {
                        string patientLink = invoice.PayerPatient == null ? "" : "<a href=\"#\" onclick=\"open_new_tab('PatientDetailV2.aspx?type=view&id=" + invoice.PayerPatient.PatientID + "');return false;\">" + invoice.PayerPatient.Person.FullnameWithoutMiddlename + "</a>";
                        lblPatientPagePopup.Text = patientLink;
                    }
                //}
            }


            Button btnSetPaid = (Button)e.Row.FindControl("btnSetPaid");
            if (btnSetPaid != null)
                btnSetPaid.Visible = !invoice.IsPaID && invoice.ReceiptsTotal == 0 && invoice.CreditNotesTotal == 0;

            Button btnAddReceipt = (Button)e.Row.FindControl("btnAddReceipt");
            if (btnAddReceipt != null)
                btnAddReceipt.OnClientClick = Receipt.GetAddReceiptPopupLinkV2(invoice.InvoiceID, "Add Payment", "document.getElementById('btnUpdateList').click();", true);




            
            Utilities.AddConfirmationBox(e);
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                Utilities.SetEditRowBackColour(e, System.Drawing.Color.LightGoldenrodYellow);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblSum_Total = (Label)e.Row.FindControl("lblSum_Total");
            lblSum_Total.Text = String.Format("{0:C}", dt.Compute("Sum(inv_total)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));   // dt.Compute("Sum(inv_total)", "").ToString();
            if (lblSum_Total.Text == "") lblSum_Total.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";

            Label lblSum_GST = (Label)e.Row.FindControl("lblSum_GST");
            lblSum_GST.Text = String.Format("{0:C}", dt.Compute("Sum(inv_gst)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));  // dt.Compute("Sum(inv_gst)", "").ToString();
            if (lblSum_GST.Text == "") lblSum_GST.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";

            Label lblSum_Receipts = (Label)e.Row.FindControl("lblSum_Receipts");
            lblSum_Receipts.Text = String.Format("{0:C}", dt.Compute("Sum(inv_receipts_total)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));  // dt.Compute("Sum(inv_receipts_total)", "").ToString();
            if (lblSum_Receipts.Text == "") lblSum_Receipts.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";

            Label lblSum_Vouchers = (Label)e.Row.FindControl("lblSum_Vouchers");
            lblSum_Vouchers.Text = String.Format("{0:C}", dt.Compute("Sum(inv_vouchers_total)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));  // dt.Compute("Sum(inv_vouchers_total)", "").ToString();
            if (lblSum_Vouchers.Text == "") lblSum_Vouchers.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";

            Label lblSum_CreditNotes = (Label)e.Row.FindControl("lblSum_CreditNotes");
            lblSum_CreditNotes.Text = String.Format("{0:C}", dt.Compute("Sum(inv_credit_notes_total)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));  // dt.Compute("Sum(inv_credit_notes_total)", "").ToString();
            if (lblSum_CreditNotes.Text == "") lblSum_CreditNotes.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";

            Label lblSum_Due = (Label)e.Row.FindControl("lblSum_Due");
            lblSum_Due.Text = String.Format("{0:C}", dt.Compute("Sum(total_due)", "inv_reversed_by IS NULL AND inv_reversed_date IS NULL"));  // dt.Compute("Sum(total_due)", "").ToString();
            if (lblSum_Due.Text == "") lblSum_Due.Text = System.Globalization.RegionInfo.CurrentRegion.CurrencySymbol + "0.00";
        }
    }
    protected void GrdInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GrdInvoice.EditIndex = -1;
        FillGrid();
    }
    protected void GrdInvoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        HyperLink lnkId = (HyperLink)GrdInvoice.Rows[e.RowIndex].FindControl("lnkId");

        DropDownList ddlInvoiceType             = (DropDownList)GrdInvoice.Rows[e.RowIndex].FindControl("ddlInvoiceType");
        TextBox      txtTotal                   = (TextBox)GrdInvoice.Rows[e.RowIndex].FindControl("txtTotal");
        TextBox      txtGST                     = (TextBox)GrdInvoice.Rows[e.RowIndex].FindControl("txtGST");
        DropDownList ddlMedicareRejectionCode   = (DropDownList)GrdInvoice.Rows[e.RowIndex].FindControl("ddlMedicareRejectionCode");



        int       invoice_id = Convert.ToInt32(lnkId.Text);
        DataTable dt         = Session["invoiceinfo_data"] as DataTable;
        DataRow[] foundRows  = dt.Select("inv_invoice_id=" + invoice_id.ToString());
        Invoice   inv        = InvoiceDB.LoadAll(foundRows[0], false);

        try
        {

            int i1 = Convert.ToInt32(ddlInvoiceType.SelectedValue);
            decimal d1 = Convert.ToDecimal(txtTotal.Text);
            decimal d2 = Convert.ToDecimal(txtGST.Text);

            InvoiceDB.Update(inv.InvoiceID,
                Convert.ToInt32(ddlInvoiceType.SelectedValue),
                inv.Booking == null ? -1 : inv.Booking.BookingID,
                inv.PayerOrganisation == null ? 0 : inv.PayerOrganisation.OrganisationID,
                inv.PayerPatient == null ? -1 : inv.PayerPatient.PatientID,
                inv.NonBookinginvoiceOrganisation == null ? 0 : inv.NonBookinginvoiceOrganisation.OrganisationID,
                inv.HealthcareClaimNumber,
                ddlMedicareRejectionCode.SelectedValue.Length == 0 ? -1 : Convert.ToInt32(ddlMedicareRejectionCode.SelectedValue),
                inv.Message,
                Convert.ToDecimal(txtTotal.Text),
                Convert.ToDecimal(txtGST.Text),
                inv.IsPaID,
                inv.IsRefund,
                inv.IsBatched,
                inv.ReversedBy == null ? -1 : inv.ReversedBy.StaffID,
                inv.ReversedDate,
                inv.LastDateEmailed);

            if (ddlMedicareRejectionCode.SelectedValue.Length > 0 && ddlMedicareRejectionCode.SelectedValue != "-1")
            {

                // send letter -- and keep it in history !!

                Letter letter = LetterDB.GetByID(Convert.ToInt32(ddlMedicareRejectionCode.SelectedValue));
                if (letter.Docname.Length > 0)
                {
                    Booking booking = BookingDB.GetByID(inv.Booking.BookingID);
                    Letter.FileContents fileContents = Letter.CreateLetter(Letter.FileFormat.Word, SiteDB.GetByID(Convert.ToInt32(Session["SiteID"])), letter.LetterID, booking.Organisation.OrganisationID, booking.Patient.PatientID, Convert.ToInt32(Session["StaffID"]), booking.BookingID, -1, 1, null, true, 1);  // .pdf

                    // set in session and show a popup on reload to get the letter
                    Session["downloadFile_Contents"] = fileContents.Contents;
                    Session["downloadFile_DocName"] = fileContents.DocName;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "close", @"<script language=javascript>window.showModalDialog('DownloadFile.aspx', '', 'dialogWidth:10px;dialogHeight:10px;resizable:no; scroll:no');</script>");
                }

            }
        }
        catch (UniqueConstraintException ucEx)
        {
            InvoiceDB.Update(inv.InvoiceID,
                inv.InvoiceType.ID,
                inv.Booking == null ? -1 : inv.Booking.BookingID,
                inv.PayerOrganisation == null ? 0 : inv.PayerOrganisation.OrganisationID,
                inv.PayerPatient == null ? -1 : inv.PayerPatient.PatientID,
                inv.NonBookinginvoiceOrganisation == null ? 0 : inv.NonBookinginvoiceOrganisation.OrganisationID,
                inv.HealthcareClaimNumber,
                inv.RejectLetter == null ? -1 : inv.RejectLetter.LetterID,
                inv.Message,
                inv.Total,
                inv.Gst,
                inv.IsPaID,
                inv.IsRefund,
                inv.IsBatched,
                inv.ReversedBy == null ? -1 : inv.ReversedBy.StaffID,
                inv.ReversedDate,
                inv.LastDateEmailed);

            SetErrorMessage(ucEx.Message);
            return;
        }
        catch (CustomMessageException cmEx)
        {
            InvoiceDB.Update(inv.InvoiceID,
                inv.InvoiceType.ID,
                inv.Booking == null ? -1 : inv.Booking.BookingID,
                inv.PayerOrganisation == null ? 0 : inv.PayerOrganisation.OrganisationID,
                inv.PayerPatient == null ? -1 : inv.PayerPatient.PatientID,
                inv.NonBookinginvoiceOrganisation == null ? 0 : inv.NonBookinginvoiceOrganisation.OrganisationID,
                inv.HealthcareClaimNumber,
                inv.RejectLetter == null ? -1 : inv.RejectLetter.LetterID,
                inv.Message,
                inv.Total,
                inv.Gst,
                inv.IsPaID,
                inv.IsRefund,
                inv.IsBatched,
                inv.ReversedBy == null ? -1 : inv.ReversedBy.StaffID,
                inv.ReversedDate,
                inv.LastDateEmailed);

            SetErrorMessage(cmEx.Message);
            return;
        }
        catch (Exception ex)
        {
            InvoiceDB.Update(inv.InvoiceID,
                inv.InvoiceType.ID,
                inv.Booking == null ? -1 : inv.Booking.BookingID,
                inv.PayerOrganisation == null ? 0 : inv.PayerOrganisation.OrganisationID,
                inv.PayerPatient == null ? -1 : inv.PayerPatient.PatientID,
                inv.NonBookinginvoiceOrganisation == null ? 0 : inv.NonBookinginvoiceOrganisation.OrganisationID,
                inv.HealthcareClaimNumber,
                inv.RejectLetter == null ? -1 : inv.RejectLetter.LetterID,
                inv.Message,
                inv.Total,
                inv.Gst,
                inv.IsPaID,
                inv.IsRefund,
                inv.IsBatched,
                inv.ReversedBy == null ? -1 : inv.ReversedBy.StaffID,
                inv.ReversedDate,
                inv.LastDateEmailed);

            SetErrorMessage(Utilities.IsDev() ? ex.ToString() : ex.Message);
            return;
        }


        GrdInvoice.EditIndex = -1;
        FillGrid();

    }
    protected void GrdInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        throw new CustomMessageException("Deleting of financial records is not permitted.");

        //HyperLink lnkId = (HyperLink)GrdInvoice.Rows[e.RowIndex].FindControl("lnkId");
        //int invoice_id = Convert.ToInt32(lnkId.Text);

        //try
        //{
        //    //InvoiceDB.UpdateInactive(invoice_id);
        //}
        //catch (ForeignKeyConstraintException fkcEx)
        //{
        //    if (Utilities.IsDev())
        //        SetErrorMessage("Can not delete because other records depend on this : " + fkcEx.Message);
        //    else
        //        SetErrorMessage("Can not delete because other records depend on this");
        //}

        //FillGrid();
    }
    protected void GrdInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("ReInvoice"))
        {

            try
            {
                int invoice_id = Convert.ToInt32(e.CommandArgument);
                Invoice invoice = InvoiceDB.GetByID(invoice_id);
                if (invoice == null)
                    throw new CustomMessageException("Invalid invoice");
                if (invoice.RejectLetter == null)
                    throw new CustomMessageException("Can not re-invoice " + invoice.InvoiceID);

                InvoiceLine[] invoiceLines = InvoiceLineDB.GetByInvoiceID(invoice.InvoiceID);


                int newInvoiceId = -1;

                Letter rejLetter = LetterDB.GetByID(invoice.RejectLetter.LetterID);

                if (invoice.PayerOrganisation != null && (invoice.PayerOrganisation.OrganisationID == -1 || invoice.PayerOrganisation.OrganisationID == -2))
                {
                    // 1. create new invoice
                    if (rejLetter.IsAllowedReclaim) // re-invoice medicare/dva
                    {
                        newInvoiceId = InvoiceDB.Insert(
                            invoice.InvoiceType.ID,
                            invoice.Booking           == null ? -1 : invoice.Booking.BookingID,
                            invoice.PayerOrganisation == null ?  0 : invoice.PayerOrganisation.OrganisationID,
                            invoice.PayerPatient      == null ? -1 : invoice.PayerPatient.PatientID,
                            invoice.NonBookinginvoiceOrganisation == null ? 0 : invoice.NonBookinginvoiceOrganisation.OrganisationID,
                            "",
                            "",
                            Convert.ToInt32(Session["StaffID"]),
                            invoice.Site.SiteID,
                            invoice.Total,
                            invoice.Gst,
                            false, false, false, DateTime.MinValue);

                        MedicareClaimNbrDB.InsertIntoInvoice(newInvoiceId, DateTime.Now.Date);

                        for (int i = 0; i < invoiceLines.Length; i++)
                        {
                            InvoiceLineDB.Insert(
                                newInvoiceId,
                                invoiceLines[i].Patient.PatientID,
                                invoiceLines[i].Offering.OfferingID,
                                invoiceLines[i].Quantity,
                                invoiceLines[i].Price,
                                invoiceLines[i].Tax,
                                invoiceLines[i].AreaTreated,
                                "",
                                invoiceLines[i].OfferingOrder == null ? -1 : invoiceLines[i].OfferingOrder.OfferingOrderID);
                        }
                    }
                    else // re-invoice patient
                    {
                        bool isClinic   = invoice.InvoiceType.ID == 107;
                        bool isAgedCare = invoice.InvoiceType.ID == 363;

                        // aged care:
                        // get each patient in the invoice lines
                        //   get their ac type (HC/LC/etc)
                        //   check how to reinovice from aged care booking completion screen
                        //   get price
                        //   create invoice and keep id in ilst .. to undo later if error

                        if (invoice.Booking.Patient == null)
                        {
                            Hashtable invLinePtIDHash = new Hashtable();
                            for (int i = 0; i < invoiceLines.Length; i++)
                                invLinePtIDHash[invoiceLines[i].Patient.PatientID] = 1;
                            bool allSamePT = invLinePtIDHash.Keys.Count == 1;

                            if (!allSamePT)
                                throw new CustomMessageException("Can not re-invoice as this invoice is for multiple patients.");

                            invoice.Booking.Patient = PatientDB.GetByID((int)(new ArrayList(invLinePtIDHash.Keys))[0]);

                            // set offering as first offering in invoicelines list (ie the aged care offering such as HC/LC/MC/DVA/etc)
                            if (invoice.Booking.Offering == null)
                                invoice.Booking.Offering = invoiceLines[0].Offering;
                        }


                        newInvoiceId = InvoiceDB.Insert(
                            invoice.InvoiceType.ID,
                            invoice.Booking           == null ? -1 : invoice.Booking.BookingID,
                            0,                                     // set as not paid by org
                            invoice.Booking.Patient.PatientID,     // set as paid by patient
                            0,
                            "",
                            rejLetter.RejectMessage,
                            Convert.ToInt32(Session["StaffID"]),
                            invoice.Site.SiteID,
                            invoice.Booking.Offering.DefaultPrice, // upate from offering table to use default price, not medicare charge
                            invoice.Gst,
                            false, false, false, DateTime.MinValue);

                        for (int i = 0; i < invoiceLines.Length; i++)
                        {
                            InvoiceLineDB.Insert(
                                newInvoiceId,
                                invoiceLines[i].Patient.PatientID,
                                invoiceLines[i].Offering.OfferingID,
                                invoiceLines[i].Quantity,
                                invoiceLines.Length == 1 && invoiceLines[i].Offering.OfferingID == invoice.Booking.Offering.OfferingID ? invoice.Booking.Offering.DefaultPrice : invoiceLines[i].Price,
                                invoiceLines[i].Tax, 
                                invoiceLines[i].AreaTreated,
                                "",
                                invoiceLines[i].OfferingOrder == null ? -1 : invoiceLines[i].OfferingOrder.OfferingOrderID);
                        }


                        //
                        // re-add to epc count (if its a medicare inv)
                        //
                        if (invoice.PayerOrganisation.OrganisationID == -1)
                        {
                            HealthCard[] medicareCards = HealthCardDB.GetAllByPatientID(invoice.Booking.Patient.PatientID, false, -1);
                            if (medicareCards.Length > 0 &&                                                             // first one will be latest medicare card
                                invoice.Booking.DateStart.Date >= medicareCards[0].DateReferralSigned.Date &&           // if booking date after signed date
                                invoice.Booking.DateStart.Date <  medicareCards[0].DateReferralSigned.Date.AddYears(1)) // if booking date before 1yr after signed date
                            {
                                HealthCardEPCRemaining[] epcsRemaining = HealthCardEPCRemainingDB.GetByHealthCardID(medicareCards[0].HealthCardID, invoice.Booking.Offering.Field.ID);
                                foreach (HealthCardEPCRemaining epcRemaining in epcsRemaining)
                                    HealthCardEPCRemainingDB.UpdateNumServicesRemaining(epcRemaining.HealthCardEpcRemainingID, epcRemaining.NumServicesRemaining + 1);
                            }
                        }

                    }


                    // 2. close this old invoice
                    if (newInvoiceId != -1)
                    {
                        if (!invoice.IsPaID)
                        {
                            CreditNoteDB.Insert(invoice.InvoiceID, invoice.TotalDue, "Invoice rejected. Code: " + rejLetter.Code, Convert.ToInt32(Session["StaffID"]));
                            InvoiceDB.UpdateIsPaid(null, invoice.InvoiceID, true);
                        }

                        // leave rejection code in there, just remove the link to re-invoice it
                        //InvoiceDB.UpdateRejectLetterID(invoice.InvoiceID, -1);

                        //SetErrorMessage("Invoice "+ invoice.InvoiceID +" closed and new invoice created: " + newInvoiceId + "<br />");
                        lblReinvoiceMsg.Text = "Invoice " + invoice.InvoiceID + " closed and new invoice created: " + newInvoiceId + "<br />";
                        lnkPrint.CommandArgument = newInvoiceId.ToString();
                        lnkEmail.CommandArgument = newInvoiceId.ToString();
                        lnkInvoice.OnClientClick = String.Format("javascript:window.showModalDialog('Invoice_ViewV2.aspx?invoice_id={0}', '', 'dialogWidth:775px;dialogHeight:" + (invoice.Booking != null ? "900" : "650") + "px;center:yes;resizable:no; scroll:no');return false;", newInvoiceId);
                        divReinvoice.Visible = true;

                        FillGrid();
                    }
                }
            }
            catch(CustomMessageException cmEx)
            {
                SetErrorMessage(cmEx.Message);
            }
            catch (Exception ex)
            {
                SetErrorMessage("", ex.ToString());
            }
        }

        if (e.CommandName.Equals("SetPaid") || e.CommandName.Equals("SetWiped"))
        {
            try
            {
                int invoice_id = Convert.ToInt32(e.CommandArgument);
                Invoice invoice = InvoiceDB.GetByID(invoice_id);
                if (invoice == null)
                    throw new CustomMessageException("Invalid invoice");
                if (invoice.IsPaID)
                    throw new CustomMessageException("Invoice already paid");

                if (e.CommandName.Equals("SetPaid"))
                {
                    ReceiptDB.Insert(null, 362, invoice.InvoiceID, invoice.TotalDue, Convert.ToDecimal(0.00), false, false, DateTime.MinValue, Convert.ToInt32(Session["StaffID"]));
                    InvoiceDB.UpdateIsPaid(null, invoice.InvoiceID, true);
                }
                else if (e.CommandName.Equals("SetWiped"))
                {
                    CreditNoteDB.Insert(invoice.InvoiceID, invoice.TotalDue, string.Empty, Convert.ToInt32(Session["StaffID"]));
                    InvoiceDB.UpdateIsPaid(null, invoice.InvoiceID, true);
                }

                FillGrid();
            }
            catch (CustomMessageException cmEx)
            {
                SetErrorMessage(cmEx.Message);
            }
            catch (Exception ex)
            {
                SetErrorMessage("", ex.ToString());
            }
        }


        if (e.CommandName == "AddClaimNumber")
        {
            int invoiceID = Convert.ToInt32(e.CommandArgument);
            Invoice inv = InvoiceDB.GetByID(invoiceID);

            if (inv.InvoiceDateAdded >= new DateTime(2013, 7, 1) && Convert.ToInt32(SystemVariableDB.GetByDescr("AutoMedicareClaiming").Value) == 1 && inv.HealthcareClaimNumber == string.Empty && (inv.PayerOrganisation.OrganisationID == -1 || inv.PayerOrganisation.OrganisationID == -2))
            {
                MedicareClaimNbrDB.InsertIntoInvoice(invoiceID, DateTime.Now.Date);
                FillGrid();
            }
        }

        if (e.CommandName == "Un-Reverse")
        {
            int invoiceID = Convert.ToInt32(e.CommandArgument);
            Invoice invoice = InvoiceDB.GetByID(invoiceID);

            if (invoice == null)
            {
                SetErrorMessage("Can't Un-Reverse - no such invoice");
                return;
            }
            if (!invoice.IsReversed)
            {
                SetErrorMessage("Can't Un-Reverse an invoice that is not reversed");
                return;
            }
            if (invoice.Booking != null && invoice.Booking.BookingStatus.ID != 0)
            {
                string URL = invoice.Booking.GetBookingSheetLinkV2();
                if (URL.StartsWith("~")) URL = URL.Substring(1);
                string bookingSheetHREF = "<a href=\"#\" onclick=\"open_new_window('" + URL + "');return false;\" >this booking</a>";

                SetErrorMessage("Can't Un-Reverse invoice " + invoice.InvoiceID + " because " + bookingSheetHREF + " is set as " + invoice.Booking.BookingStatus.Descr);
                return;
            }


            // if medicare booking - check if has epc left
            // - if not, message and disallow
            // - if has, reduce by one and continue;
            if (invoice.Booking != null && invoice.PayerOrganisation != null && invoice.PayerOrganisation.OrganisationID == -1)
            {
                InvoiceLine[] invLines = InvoiceLineDB.GetByInvoiceID(invoice.InvoiceID);
                if (invLines.Length != 1)
                {
                    SetErrorMessage("Medicare invoice must have one invoice line, but has " + invLines.Length);
                    return;
                }


                bool isClinic   = invoice.InvoiceType.ID == 107;
                bool isAgedCare = invoice.InvoiceType.ID == 363;

                HealthCard hc = HealthCardDB.GetActiveByPatientID(isClinic ? invoice.Booking.Patient.PatientID : invLines[0].Patient.PatientID, invoice.PayerOrganisation.OrganisationID);
                if (hc == null || hc.Organisation.OrganisationID != -1)
                {
                    SetErrorMessage("Medicare invoice but active healthcard is not a Medicare card. Set Medicare card as the active card first.");
                    return;
                }

                invLines[0].Offering = OfferingDB.GetByID(invLines[0].Offering.OfferingID);
                invLines[0].Offering.Field = isClinic ? invoice.Booking.Offering.Field : invoice.Booking.Provider.Field;
                HealthCardEPCRemaining[] epcsRemaining = hc == null ? new HealthCardEPCRemaining[] { } : HealthCardEPCRemainingDB.GetByHealthCardID(hc.HealthCardID, invLines[0].Offering.Field.ID);
                //HealthCardEPCRemaining[] epcsRemainingOriginal = HealthCardEPCRemaining.CloneList(epcsRemaining);  // can be used for un-doing when catching exceptions

                bool reduced = false;
                for (int j = 0; j < epcsRemaining.Length; j++)
                {
                    if (epcsRemaining[j].Field.ID == invLines[0].Offering.Field.ID)
                    {
                        if (epcsRemaining[j].NumServicesRemaining >= 1)
                        {
                            epcsRemaining[j].NumServicesRemaining -= 1;
                            HealthCardEPCRemainingDB.UpdateNumServicesRemaining(epcsRemaining[j].HealthCardEpcRemainingID, epcsRemaining[j].NumServicesRemaining);
                            reduced = true;
                        }
                    }
                }

                if (!reduced)
                {
                    SetErrorMessage("Can't Un-Reverse invoice " + invoice.InvoiceID + " because no " + invLines[0].Offering.Field.Descr + " " + (!UserView.GetInstance().IsAgedCareView ? "EPC's" : "Referrals") + " remaining");
                    return;
                }  // else already reduced

                

            }




            // Set booking as completed
            if (invoice.Booking != null)
                BookingDB.UpdateSetBookingStatusID(invoice.Booking.BookingID, 187);

            // Remove Credit Notes from invoice, which were created to make it reversed
            CreditNote[] creditNotes = CreditNoteDB.GetByInvoice(invoice.InvoiceID);
            foreach(CreditNote cn in CreditNoteDB.GetByInvoice(invoice.InvoiceID))
                CreditNoteDB.Delete(cn.CreditNoteID);

            // Set invoice as not paid and not reversed
            InvoiceDB.Update(
                invoice.InvoiceID,
                invoice.InvoiceType.ID,
                invoice.Booking == null ? -1 : invoice.Booking.BookingID,
                invoice.PayerOrganisation == null ? 0 : invoice.PayerOrganisation.OrganisationID,
                invoice.PayerPatient == null ? -1 : invoice.PayerPatient.PatientID,
                invoice.NonBookinginvoiceOrganisation == null ? 0 : invoice.NonBookinginvoiceOrganisation.OrganisationID,
                invoice.HealthcareClaimNumber,
                invoice.RejectLetter == null ? -1 : invoice.RejectLetter.LetterID,
                invoice.Message,
                invoice.Total,
                invoice.Gst,
                false,             // invoice.IsPaID,
                invoice.IsRefund,
                invoice.IsBatched,
                -1,                // invoice.ReversedBy == null ? -1 : invoice.ReversedBy.StaffID,
                DateTime.MinValue, // invoice.ReversedDate
                invoice.LastDateEmailed);

            FillGrid();


            string bookingHREF = string.Empty;
            if (invoice.Booking != null)
            {
                string URL = invoice.Booking.GetBookingSheetLinkV2();
                if (URL.StartsWith("~")) URL = URL.Substring(1);
                bookingHREF = " [<a href=\"#\" onclick=\"open_new_window('" + URL + "');return false;\" >Booking</a>]";
            }

            SetErrorMessage("Invoice " + invoice.InvoiceID + " has been Un-Reversed." + bookingHREF);
        }

        if (e.CommandName == "AllFromBooking")
        {
            int invoiceID = Convert.ToInt32(e.CommandArgument);
            Invoice inv = InvoiceDB.GetByID(invoiceID);

            if (inv == null)
            {
                SetErrorMessage("No Invoice with ID " + invoiceID + ".");
                return;
            }
            if (inv.Booking == null)
            {
                SetErrorMessage("This is a cash (non-booking) invoice.");
                return;
            }

            string url = Request.RawUrl;
            url = UrlParamModifier.Remove(url, "invoice_nbr_search");
            url = UrlParamModifier.AddEdit(url, "booking_nbr_search", inv.Booking.BookingID.ToString());
            url = UrlParamModifier.Remove(url, "claim_nbr_search");
            Response.Redirect(url);
        }

        if (e.CommandName.Equals("Insert"))
        {

            /*
            DropDownList ddlOrganisation            = (DropDownList)GrdInvoice.FooterRow.FindControl("ddlNewOrganisation");
            TextBox      txtOrganisationClaimNumber = (TextBox)GrdInvoice.FooterRow.FindControl("txtNewOrganisationClaimNumber");
            DropDownList ddlInvoiceType   = (DropDownList)GrdInvoice.FooterRow.FindControl("ddlNewInvoiceType");
            DropDownList ddlReceiptPaymentType      = (DropDownList)GrdInvoice.FooterRow.FindControl("ddlNewReceiptPaymentType");
            DropDownList ddlReceiptReturnReason     = (DropDownList)GrdInvoice.FooterRow.FindControl("ddlNewReceiptReturnReason");
            TextBox      txtTotal                   = (TextBox)GrdInvoice.FooterRow.FindControl("txtNewTotal");
            TextBox      txtGST                     = (TextBox)GrdInvoice.FooterRow.FindControl("txtNewGST");
            TextBox      txtAmtReconciled           = (TextBox)GrdInvoice.FooterRow.FindControl("txtNewAmtReconciled");
            CheckBox     chkIsInvoicePaID           = (CheckBox)GrdInvoice.FooterRow.FindControl("chkNewIsInvoicePaID");
            CheckBox     chkIsReceiptFailedToClear  = (CheckBox)GrdInvoice.FooterRow.FindControl("chkNewIsReceiptFailedToClear");
            CheckBox     chkIsReceiptOverpaid       = (CheckBox)GrdInvoice.FooterRow.FindControl("chkNewIsReceiptOverpaid");
            CheckBox     chkIsInvoiceRefund         = (CheckBox)GrdInvoice.FooterRow.FindControl("chkNewIsInvoiceRefund");
            CheckBox     chkIsInvoiceBatched        = (CheckBox)GrdInvoice.FooterRow.FindControl("chkNewIsInvoiceBatched");


            // -- can only insert if these fields are in the url : 


            // url: 
            // ? type=clinic &site=1 or &patient=55  <======= serach by what to view??


            // clinic inv - need patient, booking, what else?
            // ac inv - need booking, and somehow list of patients... ?

            InvoiceDB.Insert(
                -1,
                Convert.ToInt32(ddlInvoiceType.SelectedValue),
                Convert.ToInt32(ddlReceiptPaymentType.SelectedValue),
                Convert.ToInt32(ddlReceiptReturnReason.SelectedValue),
                Convert.ToInt32(ddlOrganisation.SelectedValue),
                txtOrganisationClaimNumber.Text,
                -1,
                -1,
                -1,
                -1,
                Convert.ToDecimal(txtTotal.Text),
                Convert.ToDecimal(txtGST.Text),
                Convert.ToDecimal(txtAmtReconciled.Text),
                chkIsInvoicePaID.Checked,
                chkIsReceiptFailedToClear.Checked,
                chkIsReceiptOverpaid.Checked,
                chkIsInvoiceRefund.Checked,
                chkIsInvoiceBatched.Checked);

            // then put in doc-entity registration table...

            FillGrid();
            */

        }
    }
    protected void GrdInvoice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrdInvoice.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        // dont allow sorting if in edit mode
        if (GrdInvoice.EditIndex >= 0)
            return;

        DataTable dataTable = Session["invoiceinfo_data"] as DataTable;

        if (dataTable != null)
        {
            if (Session["invoiceinfo_sortexpression"] == null)
                Session["invoiceinfo_sortexpression"] = "";

            DataView dataView = new DataView(dataTable);
            string[] sortData = Session["invoiceinfo_sortexpression"].ToString().Trim().Split(' ');
            string newSortExpr = (e.SortExpression == sortData[0] && sortData[1] == "ASC") ? "DESC" : "ASC";
            dataView.Sort = e.SortExpression + " " + newSortExpr;
            Session["invoiceinfo_sortexpression"] = e.SortExpression + " " + newSortExpr;

            GrdInvoice.DataSource = dataView;
            GrdInvoice.DataBind();
        }
    }
    protected void GrdInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdInvoice.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    #endregion

    #region btnUpdateList_Click

    protected void btnUpdateList_Click(object sender, EventArgs e)
    {
        if (Session["UpdateFromWebPay"] != null)
        {
            PaymentPendingDB.UpdateAllPaymentsPending(null, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(1), Convert.ToInt32(Session["StaffID"]));
            Session.Remove("UpdateFromWebPay");
        }

        FillGrid();
    }

    #endregion

    #region ddlProviders_SelectedIndexChanged

    protected void ddlProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
        int newProvID = Convert.ToInt32(ddlProviders.SelectedValue);

        string url = Request.RawUrl;
        url = UrlParamModifier.Update(newProvID != -1, url, "provider", newProvID == -1 ? "" : newProvID.ToString());
        Response.Redirect(url);
    }

    #endregion

    #region btnSearch_Click

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        HideErrorMessage();

        if (txtStartDate.Text.Length > 0 && (!Regex.IsMatch(txtStartDate.Text, @"^\d{2}\-\d{2}\-\d{4}$") || !IsValidDate(txtStartDate.Text)))
        {
            SetErrorMessage("Start date must be empty or valid and of the format dd-mm-yyyy");
            return;
        }
        if (txtEndDate.Text.Length > 0 && (!Regex.IsMatch(txtEndDate.Text, @"^\d{2}\-\d{2}\-\d{4}$") || !IsValidDate(txtEndDate.Text)))
        {
            SetErrorMessage("End date must be empty or valid and of the format dd-mm-yyyy");
            return;
        }


        DateTime startDate = txtStartDate.Text.Length == 0 ? DateTime.MinValue : GetDateFromString(txtStartDate.Text, "dd-mm-yyyy");
        DateTime endDate   = txtEndDate.Text.Length == 0   ? DateTime.MinValue : GetDateFromString(txtEndDate.Text, "dd-mm-yyyy");

        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "start_date",    startDate == DateTime.MinValue ? "" : startDate.ToString("yyyy_MM_dd"));
        url = UrlParamModifier.AddEdit(url, "end_date",      endDate == DateTime.MinValue   ? "" : endDate.ToString("yyyy_MM_dd"));
        url = UrlParamModifier.AddEdit(url, "inc_medicare",  chkIncMedicare.Checked  ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "inc_dva",       chkIncDVA.Checked       ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "inc_tac",       chkIncTAC.Checked       ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "inc_private",   chkIncPrivate.Checked   ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "view_reversed", chkViewReversed.Checked ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "inc_paid",      chkIncPaid.Checked      ? "1" : "0");
        url = UrlParamModifier.AddEdit(url, "inc_unpaid",    chkIncUnpaid.Checked    ? "1" : "0");

        url = UrlParamModifier.Update(txtInvoiceNbrSearch.Text.Trim().Length > 0, url, "invoice_nbr_search", txtInvoiceNbrSearch.Text.Trim());
        url = UrlParamModifier.Update(txtBookingNbrSearch.Text.Trim().Length > 0, url, "booking_nbr_search", txtBookingNbrSearch.Text.Trim());
        url = UrlParamModifier.Update(txtClaimNbrSearch.Text.Trim().Length   > 0, url, "claim_nbr_search"  , txtClaimNbrSearch.Text.Trim());

        Response.Redirect(url);
    }

    protected string ClearSearchesFromUrl(string url)
    {
        url = UrlParamModifier.Remove(url, "start_date");
        url = UrlParamModifier.Remove(url, "end_date");
        url = UrlParamModifier.Remove(url, "inc_medicare");
        url = UrlParamModifier.Remove(url, "inc_dva");
        url = UrlParamModifier.Remove(url, "inc_tac");
        url = UrlParamModifier.Remove(url, "inc_private");
        url = UrlParamModifier.Remove(url, "inc_reversed");
        url = UrlParamModifier.Remove(url, "inc_paid");
        url = UrlParamModifier.Remove(url, "inc_unpaid");
        url = UrlParamModifier.Remove(url, "only_rejected");

        return url;
    }

    #endregion

    #region btnPrint_Click

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int maxToPrint = 100;

        DataTable dt = (DataTable)Session["invoiceinfo_data"];

        if (dt.Rows.Count > maxToPrint)
        {
            SetErrorMessage("Please narrow your search to less than " + maxToPrint + " invoices to print at one time.");
            return;
        }

        int[] invoiceIDs = new int[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
            if (dt.Rows[i]["inv_invoice_id"] != null)
                invoiceIDs[i] = Convert.ToInt32(dt.Rows[i]["inv_invoice_id"]);

        Letter.GenerateAllInvoicesAndTypes(invoiceIDs, Response);
    }

    #endregion

    #region btnExport_Click

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = Session["invoiceinfo_data"] as DataTable;
        bool tblEmpty = (dt.Rows.Count == 1 && dt.Rows[0][0] == DBNull.Value);
        if (tblEmpty)
            dt.Rows.RemoveAt(0);

        /*
        // get entity ID list so that we can use 1 db call to get all addresses of patients
        int[] entityIDs = new int[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
            entityIDs[i] = Convert.ToInt32(dt.Rows[i]["entity_id"]);

        // get all patients addresses into a hashtable
        System.Collections.Hashtable contactHash;
        if (Utilities.GetAddressType().ToString() == "Contact")
            contactHash = ContactDB.GetHashByEntityIDs(1, entityIDs);
        else if (Utilities.GetAddressType().ToString() == "ContactAus")
            contactHash = ContactAusDB.GetHashByEntityIDs(1, entityIDs);
        else
            throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());
        */


        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("Invoice #").Append(",");
        sb.Append("Inv Date").Append(",");
        sb.Append("Treat Date").Append(",");
        sb.Append("Debtor").Append(",");
        sb.Append("Claim Nbr").Append(",");
        sb.Append("Inv Total").Append(",");
        sb.Append("GST").Append(",");
        sb.Append("Receipts").Append(",");
        sb.Append("Vouchers").Append(",");
        sb.Append("Adj").Append(",");
        sb.Append("Due").Append(",");
        sb.Append("Paid").Append(",");
        sb.Append("Patient").Append(",");
        sb.Append("Provider").Append(",");
        sb.AppendLine();


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Invoice invoice = InvoiceDB.LoadAll(dt.Rows[i], false);
            if (invoice.Booking != null)
                invoice.Booking = BookingDB.GetByID(invoice.Booking.BookingID);


            string payerName = string.Empty;
            if (dt.Rows[i]["inv_payer_organisation_id"] != DBNull.Value)
                payerName = invoice.PayerOrganisation.Name;
            else if (dt.Rows[i]["inv_payer_patient_id"] != DBNull.Value)
                payerName = invoice.PayerPatient.Person.FullnameWithoutMiddlename;
            else if (invoice.Booking != null && invoice.Booking.Patient != null)
                    payerName = invoice.Booking.Patient.Person.FullnameWithoutMiddlename;

            string patientName = string.Empty;
            if (invoice.Booking != null)
                patientName = invoice.Booking.Patient == null ?
                    dt.Rows[i]["booking_patient_person_firstname"] + " " + dt.Rows[i]["booking_patient_person_surname"] :
                    invoice.Booking.Patient.Person.FullnameWithoutMiddlename;
            else
                patientName = invoice.PayerPatient.Person.FullnameWithoutMiddlename;


            sb.Append(dt.Rows[i]["inv_invoice_id"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_invoice_date_added"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["booking_date_start"].ToString()).Append(",");
            sb.Append(payerName).Append(",");
            sb.Append(dt.Rows[i]["inv_healthcare_claim_number"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_total"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_gst"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_receipts_total"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_vouchers_total"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_credit_notes_total"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["total_due"].ToString()).Append(",");
            sb.Append(dt.Rows[i]["inv_is_paid"].ToString()).Append(",");
            sb.Append(patientName).Append(",");
            sb.Append(dt.Rows[i]["provider_person_firstname"].ToString() + " " + dt.Rows[i]["provider_person_surname"].ToString()).Append(",");

            sb.AppendLine();
        }

        ExportCSV(Response, sb.ToString(), "invoices_export.csv");
    }
    protected static void ExportCSV(HttpResponse response, string fileText, string fileName)
    {
        byte[] buffer = GetBytes(fileText);

        try
        {
            response.Clear();
            response.ContentType = "text/plain";
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
            response.End();
        }
        catch (System.Web.HttpException ex) 
        {
            // ignore exception where user closed the download box
            if (!ex.Message.StartsWith("The remote host closed the connection. The error code is"))
                throw;
        }
    }
    protected static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    #endregion

    #region lnkPrint_Command, lnkEmail_Command

    protected void lnkPrint_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ReInvoiced")
            divReinvoice.Visible = true;

        int invoiceID = Convert.ToInt32(e.CommandArgument);
        Invoice invoice = InvoiceDB.GetByID(invoiceID);
        Letter.GenerateInvoicesToPrint(new int[] { invoiceID }, Response, invoice.Site.SiteType.ID == 1, invoice.Booking != null);
    }
    protected void lnkEmail_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ReInvoiced")
            divReinvoice.Visible = true;

        int invoiceID = Convert.ToInt32(e.CommandArgument);
        Invoice invoice = InvoiceDB.GetByID(invoiceID);

        try
        {
            Tuple<string, string[]> sentTo = Letter.GenerateInvoiceToEmail(invoiceID, invoice.Site.SiteType.ID == 1);
            SetErrorMessage("Invoice sent" + (sentTo == null || sentTo.Item1.Length == 0 ? "" : " to " + sentTo.Item1));
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }

    #endregion

    #region SetErrorMessage, HideErrorMessage

    private void HideTableAndSetErrorMessage(string errMsg = "", string details = "")
    {
        GrdInvoice.Visible = false;
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