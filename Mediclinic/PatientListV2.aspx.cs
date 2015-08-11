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

public partial class PatientListV2 : System.Web.UI.Page
{

    #region Page_Load

    protected bool debugPageLoadTime = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
                Utilities.SetNoCache(Response);
            HideErrorMessage();


            if (!IsPostBack)
            {
                PagePermissions.EnforcePermissions_RequireAny(Session, Response, true, true, true, true, true, true);

                Session.Remove("patientinfo_sortexpression");
                Session.Remove("patientinfo_data");

                UserView userView = UserView.GetInstance();

                lblHeading.Text = userView.IsAgedCareView ? "Residents" : "Patients";
                Page.Title      = userView.IsAgedCareView ? "Residents" : "Patients";

                bool showExtendedSearch = (Request.QueryString["extended_search_open"] != null && Request.QueryString["extended_search_open"] == "1");
                HideShowExtendedSearch(showExtendedSearch, true);
                FillExtendedSearchLists(true);  //FillExtendedSearchLists(showExtendedSearch); 
                chkShowDeceased.Checked                  = Request.QueryString["show_deceased"]         != null && Request.QueryString["show_deceased"] == "1";
                chkShowDeleted.Checked                   = Request.QueryString["show_deleted"]          != null && Request.QueryString["show_deleted"]  == "1";
                chkShowOnlyMyPatients.Checked            = false; // isProviderView && (Request.QueryString["show_only_my_patients"] == null || Request.QueryString["show_only_my_patients"] == "1");
                chkShowOnlyMyPatients.Visible            = td_ShowOnlyMyPatients.Visible                 = false; // isProviderView;
                chkOnlyDiabetics.Checked                 = Request.QueryString["only_diabetics"]        != null && Request.QueryString["only_diabetics"]    == "1";
                chkOnlyMedicareEPC.Checked               = Request.QueryString["only_medicare_epc"]     != null && Request.QueryString["only_medicare_epc"] == "1";
                chkOnlyDVAEPC.Checked                    = Request.QueryString["only_dva_epc"]          != null && Request.QueryString["only_dva_epc"]      == "1";

                //chkOnlyMedicareEPC.Text = !userView.IsAgedCareView ? "Only With Valid Medicare EPC" : "Only With Valid Medicare Referral";
                lblChkOnlyMedicareEPC.InnerText = !userView.IsAgedCareView ? "Only With Valid Medicare EPC" : "Only With Valid Medicare Referral";

                FillGrid();
                txtSearchFullName.Focus();
            }
            else
            {
                HideShowExtendedSearch(chkShowExtendedSearch.Checked, false);
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

    #endregion

    #region GrdPatient

    protected void FillGrid()
    {
        string searchSurname = "";
        if (Request.QueryString["surname_search"] != null && Request.QueryString["surname_search"].Length > 0)
        {
            searchSurname = Request.QueryString["surname_search"];
            txtSearchSurname.Text = Request.QueryString["surname_search"];
        }
        bool searchSurnameOnlyStartsWith = true;
        if (Request.QueryString["surname_starts_with"] != null && Request.QueryString["surname_starts_with"].Length > 0)
        {
            searchSurnameOnlyStartsWith = Request.QueryString["surname_starts_with"] == "0" ? false : true;
        }
        string searchFirstName = "";
        if (Request.QueryString["firstname_search"] != null && Request.QueryString["firstname_search"].Length > 0)
        {
            searchFirstName = Request.QueryString["firstname_search"];
            txtSearchFirstName.Text = Request.QueryString["firstname_search"];
        }
        bool searchFirstNameOnlyStartsWith = true;
        if (Request.QueryString["firstname_starts_with"] != null && Request.QueryString["firstname_starts_with"].Length > 0)
        {
            searchFirstNameOnlyStartsWith = Request.QueryString["firstname_starts_with"] == "0" ? false : true;
        }
        string searchSuburb = "";
        if (Request.QueryString["suburb_search"] != null && Request.QueryString["suburb_search"].Length > 0)
        {
            searchSuburb = Request.QueryString["suburb_search"].Replace('_', ',');
            Suburb suburb = SuburbDB.GetByID(Convert.ToInt32(searchSuburb));
            if (suburbID != null)
            {
                lblSuburbText.Text = suburb.Name + ", " + suburb.State + " (" + suburb.Postcode + ")";
                suburbID.Value = suburb.SuburbID.ToString();
            }
        }
        string searchStreet = "";
        if (Request.QueryString["street_search"] != null && Request.QueryString["street_search"].Length > 0)
        {
            if (Utilities.GetAddressType().ToString() == "Contact")
            {
                searchStreet = Request.QueryString["street_search"].Replace('_', ',');
                foreach (string s in searchStreet.Split(','))
                    foreach (ListItem li in lstStreets.Items)
                        if (li.Value == s)
                            li.Selected = true;
            }
            else if (Utilities.GetAddressType().ToString() == "ContactAus")
            {
                searchStreet = Request.QueryString["street_search"];
                txtSearchStreet.Text = Request.QueryString["street_search"];
            }
            else
                throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());
        }
        string searchPhoneNbr = "";
        if (Request.QueryString["phone_search"] != null && Request.QueryString["phone_search"].Length > 0)
        {
            searchPhoneNbr = Request.QueryString["phone_search"];
            txtSearchPhNum.Text = searchPhoneNbr;
        }
        string searchEmail = "";
        if (Request.QueryString["email_search"] != null && Request.QueryString["email_search"].Length > 0)
        {
            searchEmail = Request.QueryString["email_search"];
            txtSearchEmail.Text = searchEmail;
        }
        string searchMedicareCardNo = "";
        if (Request.QueryString["medicare_card_no_search"] != null && Request.QueryString["medicare_card_no_search"].Length > 0)
        {
            searchMedicareCardNo = Request.QueryString["medicare_card_no_search"];
            txtSearchMedicareCardNo.Text = Request.QueryString["medicare_card_no_search"];
        }
        bool searchMedicareCardNoOnlyStartsWith = true;
        if (Request.QueryString["medicare_card_no_starts_with"] != null && Request.QueryString["medicare_card_no_starts_with"].Length > 0)
        {
            searchMedicareCardNoOnlyStartsWith = Request.QueryString["medicare_card_no_starts_with"] == "0" ? false : true;
        }
        int dobDay   = -1;
        int dobMonth = -1;
        int dobYear  = -1;
        if (Request.QueryString["dob_search"] != null && Regex.IsMatch(Request.QueryString["dob_search"], @"^\d*_\d*_\d*$"))
        {
            string searchDOB = Request.QueryString["dob_search"];
            string[] parts = searchDOB.Split('_');
            if (parts[0].Length > 0)
            {
                dobDay = Convert.ToInt32(parts[0]);
                ddlDOB_Day.SelectedValue = parts[0];
            }
            if (parts[1].Length > 0)
            {
                dobMonth = Convert.ToInt32(parts[1]);
                ddlDOB_Month.SelectedValue = parts[1];
            }
            if (parts[2].Length > 0)
            {
                dobYear = Convert.ToInt32(parts[2]);
                ddlDOB_Year.SelectedValue = parts[2];
            }
        }
        string searchInternalOrg = "";
        if (Request.QueryString["internal_org_search"] != null && Request.QueryString["internal_org_search"].Length > 0)
        {
            searchInternalOrg = Request.QueryString["internal_org_search"].Replace('_', ',');
            Organisation internalOrg = OrganisationDB.GetByID(Convert.ToInt32(searchInternalOrg));
            if (internalOrg != null)
            {
                lblInternalOrgText.Text = internalOrg.Name;
                internalOrgID.Value = internalOrg.OrganisationID.ToString();
            }
        }
        string searchProvider = "";
        if (Request.QueryString["provider_search"] != null && Request.QueryString["provider_search"].Length > 0)
        {
            searchProvider = Request.QueryString["provider_search"].Replace('_', ',');
            Staff provider = StaffDB.GetByID(Convert.ToInt32(searchProvider));
            if (provider != null)
            {
                lblProviderText.Text = provider.Person.FullnameWithoutMiddlename;
                providerID.Value = provider.StaffID.ToString();
            }
        }
        string searchRefferer = "";
        if (Request.QueryString["referrer_search"] != null && Request.QueryString["referrer_search"].Length > 0)
        {
            searchRefferer = Request.QueryString["referrer_search"].Replace('_', ',');
            RegisterReferrer regRef = RegisterReferrerDB.GetByID(Convert.ToInt32(searchRefferer));
            if (regRef != null)
            {
                lblReferrerText.Text = regRef.Referrer.Person.Surname + ", " + regRef.Referrer.Person.Firstname + " [" + regRef.Organisation.Name + "]";
                referrerID.Value = regRef.RegisterReferrerID.ToString();
            }
        }
        string searchReffererPerson = "";
        if (Request.QueryString["referrer_person_search"] != null && Request.QueryString["referrer_person_search"].Length > 0)
        {
            searchReffererPerson = Request.QueryString["referrer_person_search"].Replace('_', ',');
            Referrer referrer = ReferrerDB.GetByID(Convert.ToInt32(searchReffererPerson));
            if (referrer != null)
            {
                lblReferrerPersonText.Text = referrer.Person.Surname + ", " + referrer.Person.Firstname;
                referrerPersonID.Value = referrer.ReferrerID.ToString();
            }
        }
        string searchReffererOrg = "";
        if (Request.QueryString["referrer_org_search"] != null && Request.QueryString["referrer_org_search"].Length > 0)
        {
            searchReffererOrg = Request.QueryString["referrer_org_search"].Replace('_', ',');
            Organisation refOrg = OrganisationDB.GetByID(Convert.ToInt32(searchReffererOrg));
            if (refOrg != null)
            {
                lblReferrerOrgText.Text = refOrg.Name;
                referrerOrgID.Value = refOrg.OrganisationID.ToString();
            }
        }
        bool onlyDiabetics   = Request.QueryString["only_diabetics"]    != null && Request.QueryString["only_diabetics"]    == "1";
        bool onlyMedicareEPC = Request.QueryString["only_medicare_epc"] != null && Request.QueryString["only_medicare_epc"] == "1";
        bool onlyDVAEPC      = Request.QueryString["only_dva_epc"]      != null && Request.QueryString["only_dva_epc"]      == "1";


        bool showDeceased       = chkShowDeceased.Checked;
        bool showDeleted        = chkShowDeleted.Checked;
        bool showOnlyMyPatients = chkShowOnlyMyPatients.Checked;


        UserView userView                     = UserView.GetInstance();
        bool     ProvsCanSeePatientsOfAllOrgs = ((SystemVariables)System.Web.HttpContext.Current.Session["SystemVariables"])["Bookings_ProvsCanSeePatientsOfAllOrgs"].Value == "1";
        bool     canSeeAllPatients            = userView.IsAdminView || (userView.IsProviderView && ProvsCanSeePatientsOfAllOrgs);

        if (debugPageLoadTime) Logger.LogQuery("A1", false, false, true);

        DataTable dt = null;
        if (canSeeAllPatients && userView.IsClinicView)
            dt = PatientDB.GetDataTable(showDeleted, showDeceased, userView.IsClinicView, userView.IsGPView, searchSurname, searchSurnameOnlyStartsWith, searchFirstName, searchFirstNameOnlyStartsWith, searchSuburb, searchStreet, searchPhoneNbr, searchEmail, searchMedicareCardNo, searchMedicareCardNoOnlyStartsWith, dobDay, dobMonth, dobYear, searchInternalOrg, searchProvider, searchRefferer, searchReffererPerson, searchReffererOrg, onlyDiabetics, onlyMedicareEPC, onlyDVAEPC);
        else if (canSeeAllPatients && userView.IsAgedCareView)
            dt = RegisterPatientDB.GetDataTable_PatientsOfOrgGroupType(false, "6", showDeleted, showDeceased, userView.IsClinicView, false, searchSurname, searchSurnameOnlyStartsWith, searchFirstName, searchFirstNameOnlyStartsWith, searchSuburb, searchStreet, searchPhoneNbr, searchEmail, searchMedicareCardNo, searchMedicareCardNoOnlyStartsWith, dobDay, dobMonth, dobYear, searchInternalOrg, searchProvider, searchRefferer, searchReffererPerson, searchReffererOrg, onlyDiabetics, onlyMedicareEPC, onlyDVAEPC);
        else if (canSeeAllPatients && userView.IsGPView)
            dt = PatientDB.GetDataTable(showDeleted, showDeceased, userView.IsClinicView, userView.IsGPView, searchSurname, searchSurnameOnlyStartsWith, searchFirstName, searchFirstNameOnlyStartsWith, searchSuburb, searchStreet, searchPhoneNbr, searchEmail, searchMedicareCardNo, searchMedicareCardNoOnlyStartsWith, dobDay, dobMonth, dobYear, searchInternalOrg, searchRefferer, searchProvider, searchReffererPerson, searchReffererOrg, onlyDiabetics, onlyMedicareEPC, onlyDVAEPC);
        else // no admin view - so org is set
            dt = RegisterPatientDB.GetDataTable_PatientsOf(false, Convert.ToInt32(Session["OrgID"]), showDeleted, showDeceased, userView.IsClinicView, userView.IsGPView, searchSurname, searchSurnameOnlyStartsWith, searchFirstName, searchFirstNameOnlyStartsWith, searchSuburb, searchStreet, searchPhoneNbr, searchEmail, searchMedicareCardNo, searchMedicareCardNoOnlyStartsWith, dobDay, dobMonth, dobYear, searchInternalOrg, searchProvider, searchRefferer, searchReffererPerson, searchReffererOrg, onlyDiabetics, onlyMedicareEPC, onlyDVAEPC);

        if (debugPageLoadTime) Logger.LogQuery("A2", false, false, true);
        Hashtable offeringsHash = OfferingDB.GetHashtable(true, -1, null, true);
        if (debugPageLoadTime) Logger.LogQuery("A3", false, false, true);
        PatientDB.AddACOfferings(ref offeringsHash, ref dt, "ac_inv_offering_id", "ac_inv_offering_name", "ac_pat_offering_id", "ac_pat_offering_name",
                                                            "ac_inv_aged_care_patient_type_id", "ac_inv_aged_care_patient_type_descr",
                                                            "ac_pat_aged_care_patient_type_id", "ac_pat_aged_care_patient_type_descr"
            );
        if (debugPageLoadTime) Logger.LogQuery("A4", false, false, true);



	    //in display, have pt type 
	    //- if mcd/dva/emergency : Medicare (Low Care)
	    //- if LC/HC/ETC         : Low Care
        dt.Columns.Add("ac_offering", typeof(String));
        for(int i=0; i<dt.Rows.Count; i++)
        {
            int    ac_inv_offering_id   = dt.Rows[i]["ac_inv_offering_id"]   == DBNull.Value ? -1           : Convert.ToInt32(dt.Rows[i]["ac_inv_offering_id"]);
            int    ac_pat_offering_id   = dt.Rows[i]["ac_pat_offering_id"]   == DBNull.Value ? -1           : Convert.ToInt32(dt.Rows[i]["ac_pat_offering_id"]);
            string ac_inv_offering_name = dt.Rows[i]["ac_inv_offering_name"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_inv_offering_name"]);
            string ac_pat_offering_name = dt.Rows[i]["ac_pat_offering_name"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_pat_offering_name"]);

            int    ac_inv_aged_care_patient_type_id    = dt.Rows[i]["ac_inv_aged_care_patient_type_id"]    == DBNull.Value ? -1           : Convert.ToInt32(dt.Rows[i]["ac_inv_aged_care_patient_type_id"]);
            string ac_inv_aged_care_patient_type_descr = dt.Rows[i]["ac_inv_aged_care_patient_type_descr"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[i]["ac_inv_aged_care_patient_type_descr"]);
            int    ac_pat_aged_care_patient_type_id    = dt.Rows[i]["ac_pat_aged_care_patient_type_id"]    == DBNull.Value ? -1           : Convert.ToInt32(dt.Rows[i]["ac_pat_aged_care_patient_type_id"]);
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



        if (debugPageLoadTime) Logger.LogQuery("A5", false, false, true);
        // update AjaxLivePatientSurnameSearch and PatientListV2.aspx and PatientListPopup to disallow providers to see other patients.
        if (userView.IsProviderView && !canSeeAllPatients && showOnlyMyPatients)  // remove any patients who they haven't had bookings with before
        {
            Patient[] patients = BookingDB.GetPatientsOfBookingsWithProviderAtOrg(Convert.ToInt32(Session["StaffID"]), Convert.ToInt32(Session["OrgID"]));
            System.Collections.Hashtable hash = new System.Collections.Hashtable();
            foreach (Patient p in patients)
                hash[p.PatientID] = 1;

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
                if (hash[Convert.ToInt32(dt.Rows[i]["patient_id"])] == null)
                    dt.Rows.RemoveAt(i);
        }

        if (debugPageLoadTime) Logger.LogQuery("A6", false, false, true);
        Session["patientinfo_data"] = dt;

        if (dt.Rows.Count > 0)
        {

            if (IsPostBack && Session["patientinfo_sortexpression"] != null && Session["patientinfo_sortexpression"].ToString().Length > 0)
            {
                DataView dataView = new DataView(dt);
                dataView.Sort = Session["patientinfo_sortexpression"].ToString();
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
                HideTableAndSetErrorMessage("", ex.ToString());
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

        if (debugPageLoadTime) Logger.LogQuery("A7", false, false, true);

        if (!userView.IsAdminView)
            GrdPatient.Columns[GrdPatient.Columns.Count - 1].Visible = false;
    }
    protected void GrdPatient_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (!Utilities.IsDev() && e.Row.RowType != DataControlRowType.Pager)
            e.Row.Cells[0].CssClass = "hiddencol";

        if (e.Row.RowType != DataControlRowType.Pager)
        {
            foreach (DataControlField col in GrdPatient.Columns)
            {
                if (!chkShowDeceased.Checked)
                    if (col.HeaderText.ToLower().Trim() == "deceased")
                        e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";

                if (!chkShowDeleted.Checked)
                    if (col.HeaderText.ToLower().Trim() == "archived")
                        e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";

                // neither clinic view or aged care views need to see this .. but keep it incase it is needed later
                if (col.HeaderText.ToLower().Trim() == "clinic patient")
                    e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";

                // neither clinic view or aged care views need to see this .. but keep it incase it is needed later
                if (col.HeaderText.ToLower().Trim() == "gp patient")
                    e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";

                if (!UserView.GetInstance().IsAgedCareView)
                    if (col.HeaderText.ToLower().Trim() == "pt type")
                        e.Row.Cells[GrdPatient.Columns.IndexOf(col)].CssClass = "hiddencol";

            }
        }
    }
    protected void GrdPatient_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        UserView userView = UserView.GetInstance();

        DataTable dt = Session["patientinfo_data"] as DataTable;
        bool tblEmpty = (dt.Rows.Count == 1 && dt.Rows[0][0] == DBNull.Value);
        if (!tblEmpty && e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblId = (Label)e.Row.FindControl("lblId");
            DataRow[] foundRows = dt.Select("patient_id=" + lblId.Text);
            DataRow thisRow = foundRows[0];

            bool is_deleted = Convert.ToBoolean(thisRow["is_deleted"]);


            DropDownList ddlTitle = (DropDownList)e.Row.FindControl("ddlTitle");
            if (ddlTitle != null)
            {
                DataTable titles = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "Title", Convert.ToInt32(thisRow["title_id"]) == 0 ? "" : " title_id <> 0 ", " descr ", "title_id", "descr");
                ddlTitle.DataSource = titles;
                ddlTitle.DataTextField = "descr";
                ddlTitle.DataValueField = "title_id";
                ddlTitle.DataBind();
                ddlTitle.SelectedValue = thisRow["title_id"].ToString();
            }
            DropDownList ddlGender = (DropDownList)e.Row.FindControl("ddlGender");
            if (ddlGender != null)
            {
                if (thisRow["gender"].ToString() != "")
                    for (int i = ddlGender.Items.Count - 1; i >= 0; i--)
                        if (ddlGender.Items[i].Value == "")
                            ddlGender.Items.RemoveAt(i);
            }

            if (ddlTitle != null && ddlGender != null)
                ddlTitle.Attributes["onchange"] = "title_changed_reset_gender('" + ddlTitle.ClientID + "','" + ddlGender.ClientID + "');";


            DropDownList ddlDOB_Day   = (DropDownList)e.Row.FindControl("ddlDOB_Day");
            DropDownList ddlDOB_Month = (DropDownList)e.Row.FindControl("ddlDOB_Month");
            DropDownList ddlDOB_Year  = (DropDownList)e.Row.FindControl("ddlDOB_Year");
            if (ddlDOB_Day != null && ddlDOB_Month != null && ddlDOB_Year != null)
            {
                ddlDOB_Day.Items.Add(new ListItem("--", "-1"));
                ddlDOB_Month.Items.Add(new ListItem("--", "-1"));
                ddlDOB_Year.Items.Add(new ListItem("----", "-1"));

                for (int i = 1; i <= 31; i++)
                    ddlDOB_Day.Items.Add(new ListItem(i.ToString(), i.ToString()));
                for (int i = 1; i <= 12; i++)
                    ddlDOB_Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
                for (int i = 1915; i <= DateTime.Today.Year; i++)
                    ddlDOB_Year.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (thisRow["dob"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(thisRow["dob"]);

                    ddlDOB_Day.SelectedValue = dob.Day.ToString();
                    ddlDOB_Month.SelectedValue = dob.Month.ToString();

                    int firstYearSelectable = Convert.ToInt32(ddlDOB_Year.Items[1].Value);
                    int lastYearSelectable  = Convert.ToInt32(ddlDOB_Year.Items[ddlDOB_Year.Items.Count - 1].Value);
                    if (dob.Year < firstYearSelectable)
                        ddlDOB_Year.Items.Insert(1, new ListItem(dob.Year.ToString(), dob.Year.ToString()));
                    if (dob.Year > lastYearSelectable)
                        ddlDOB_Year.Items.Add(new ListItem(dob.Year.ToString(), dob.Year.ToString()));

                    ddlDOB_Year.SelectedValue = dob.Year.ToString();
                }
            }


            DropDownList ddlDARev_Day   = (DropDownList)e.Row.FindControl("ddlDARev_Day");
            DropDownList ddlDARev_Month = (DropDownList)e.Row.FindControl("ddlDARev_Month");
            DropDownList ddlDARev_Year  = (DropDownList)e.Row.FindControl("ddlDARev_Year");
            if (ddlDARev_Day != null && ddlDARev_Month != null && ddlDARev_Year != null)
            {
                ddlDARev_Day.Items.Add(new ListItem("--", "-1"));
                ddlDARev_Month.Items.Add(new ListItem("--", "-1"));
                ddlDARev_Year.Items.Add(new ListItem("----", "-1"));

                for (int i = 1; i <= 31; i++)
                    ddlDARev_Day.Items.Add(new ListItem(i.ToString(), i.ToString()));
                for (int i = 1; i <= 12; i++)
                    ddlDARev_Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
                for (int i = 2000; i <= DateTime.Today.Year; i++)
                    ddlDARev_Year.Items.Add(new ListItem(i.ToString(), i.ToString()));

                if (thisRow["diabetic_assessment_review_date"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(thisRow["diabetic_assessment_review_date"]);

                    ddlDARev_Day.SelectedValue = dob.Day.ToString();
                    ddlDARev_Month.SelectedValue = dob.Month.ToString();

                    int firstYearSelectable = Convert.ToInt32(ddlDARev_Year.Items[1].Value);
                    int lastYearSelectable  = Convert.ToInt32(ddlDARev_Year.Items[ddlDARev_Year.Items.Count - 1].Value);
                    if (dob.Year < firstYearSelectable)
                        ddlDARev_Year.Items.Insert(1, new ListItem(dob.Year.ToString(), dob.Year.ToString()));
                    if (dob.Year > lastYearSelectable)
                        ddlDARev_Year.Items.Add(new ListItem(dob.Year.ToString(), dob.Year.ToString()));

                    ddlDARev_Year.SelectedValue = dob.Year.ToString();
                }
            }


            DropDownList ddlACInvOffering = (DropDownList)e.Row.FindControl("ddlACInvOffering");
            if (ddlACInvOffering != null)
            {
                int    ac_inv_offering_id   = thisRow["ac_inv_offering_id"]   == DBNull.Value ? -1            : Convert.ToInt32(thisRow["ac_inv_offering_id"]);
                int    ac_pat_offering_id   = thisRow["ac_pat_offering_id"]   == DBNull.Value ? -1            : Convert.ToInt32(thisRow["ac_pat_offering_id"]);
                string ac_inv_offering_name = thisRow["ac_inv_offering_name"] == DBNull.Value ?  string.Empty : Convert.ToString(thisRow["ac_inv_offering_name"]);
                string ac_pat_offering_name = thisRow["ac_pat_offering_name"] == DBNull.Value ?  string.Empty : Convert.ToString(thisRow["ac_pat_offering_name"]);

                int    ac_inv_aged_care_patient_type_id    = thisRow["ac_inv_aged_care_patient_type_id"]    == DBNull.Value ? -1           : Convert.ToInt32(thisRow["ac_inv_aged_care_patient_type_id"]);
                string ac_inv_aged_care_patient_type_descr = thisRow["ac_inv_aged_care_patient_type_descr"] == DBNull.Value ? string.Empty : Convert.ToString(thisRow["ac_inv_aged_care_patient_type_descr"]);
                int    ac_pat_aged_care_patient_type_id    = thisRow["ac_pat_aged_care_patient_type_id"]    == DBNull.Value ? -1           : Convert.ToInt32(thisRow["ac_pat_aged_care_patient_type_id"]);
                string ac_pat_aged_care_patient_type_descr = thisRow["ac_pat_aged_care_patient_type_descr"] == DBNull.Value ? string.Empty : Convert.ToString(thisRow["ac_pat_aged_care_patient_type_descr"]);


                DataTable dt_offerings = OfferingDB.GetDataTable(true, -1, null, true);
                for (int i = dt_offerings.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToInt32(dt_offerings.Rows[i]["o_offering_id"]) != ac_inv_offering_id &&
                        (Convert.ToInt32(dt_offerings.Rows[i]["o_aged_care_patient_type_id"]) == 1 || Convert.ToBoolean(dt_offerings.Rows[i]["o_is_deleted"])))
                        dt_offerings.Rows.RemoveAt(i);

                    // if clinic patient and no ac pt type set, only allow HC/LC/HCU/LCF
                    else if ((ac_inv_aged_care_patient_type_id == -1 || ac_pat_aged_care_patient_type_id == -1) &&
                        !(new List<int> { 2, 3, 4, 5 }).Contains(Convert.ToInt32(dt_offerings.Rows[i]["acpatientcat_aged_care_patient_type_id"])) )
                        dt_offerings.Rows.RemoveAt(i);

                    else if (!(new List<int> { 2, 4 }).Contains(ac_pat_aged_care_patient_type_id) && Convert.ToInt32(dt_offerings.Rows[i]["acpatientcat_aged_care_patient_type_id"]) == 9) // if not LC/LCF - remove option for DVA
                        dt_offerings.Rows.RemoveAt(i);
                    else if (!(new List<int> { 2, 4 }).Contains(ac_pat_aged_care_patient_type_id) && Convert.ToInt32(dt_offerings.Rows[i]["acpatientcat_aged_care_patient_type_id"]) == 6) // if not LC/LCF - remove option for LCE
                        dt_offerings.Rows.RemoveAt(i);
                    else if (!(new List<int> { 3, 5 }).Contains(ac_pat_aged_care_patient_type_id) && Convert.ToInt32(dt_offerings.Rows[i]["acpatientcat_aged_care_patient_type_id"]) == 7) // if not HC/HCU - remove option for HCE
                        dt_offerings.Rows.RemoveAt(i);

                    else if (ac_inv_offering_id != -1 && (new List<int> { 2, 3, 4, 5 }).Contains(ac_inv_aged_care_patient_type_id) && (new List<int> { 6, 7, 8, 9, 10 }).Contains(Convert.ToInt32(dt_offerings.Rows[i]["o_aged_care_patient_type_id"])))
                        dt_offerings.Rows[i]["o_name"] = dt_offerings.Rows[i]["o_name"].ToString() + " (" + ac_pat_offering_name + ")";
                }

                DataView dv = dt_offerings.DefaultView;
                dv.Sort = "acpatientcat_aged_care_patient_type_id";
                dt_offerings = dv.ToTable();

                ddlACInvOffering.DataSource = dt_offerings;
                ddlACInvOffering.DataBind();

                if (ac_inv_offering_id != -1)
                    ddlACInvOffering.SelectedValue = ac_inv_offering_id.ToString();
            }


            HyperLink lnkBookings = (HyperLink)e.Row.FindControl("lnkBookings");
            if (lnkBookings != null)
            {
                if (userView.IsAdminView)
                {
                    lnkBookings.NavigateUrl = string.Format("~/BookingScreenGetPatientOrgsV2.aspx?patient_id={0}", Convert.ToInt32(thisRow["patient_id"]));
                    lnkBookings.Visible = (Convert.ToInt32(thisRow["num_registered_orgs"]) > 0);
                }
                else
                {
                    lnkBookings.NavigateUrl = userView.IsAgedCareView ?
                        string.Format("~/BookingsV2.aspx?orgs={0}&patient={1}", Convert.ToInt32(Session["OrgID"]), Convert.ToInt32(thisRow["patient_id"])) :
                        string.Format("~/BookingsV2.aspx?orgs={0}", Convert.ToInt32(Session["OrgID"]));

                }
            }

            ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
            if (btnDelete != null)
            {
                if (is_deleted)
                {
                    btnDelete.CommandName = "_UnDelete";
                    btnDelete.ImageUrl = "~/images/tick-24.png";
                    btnDelete.AlternateText = "Un-Archive";
                    btnDelete.ToolTip = "Un-Archive";
                }
            }


            ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
            if (lnkEdit != null && is_deleted && !userView.IsAdminView)
                lnkEdit.Visible = false;

            Label lblIsDeleted = (Label)e.Row.FindControl("lblIsDeleted");
            if (is_deleted)
            {
                e.Row.ForeColor = System.Drawing.Color.Gray;
                if (lblIsDeleted != null)
                    lblIsDeleted.ForeColor = System.Drawing.Color.Red;
            }


            Utilities.AddConfirmationBox(e, true, "Archive");
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                Utilities.SetEditRowBackColour(e, System.Drawing.Color.LightGoldenrodYellow);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable titles = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "Title", " title_id <> 0 ", " descr ", "title_id", "descr");
            DropDownList ddlTitle = (DropDownList)e.Row.FindControl("ddlNewTitle");
            ddlTitle.DataSource = titles;
            ddlTitle.DataBind();
            ddlTitle.SelectedIndex = Utilities.IndexOf(ddlTitle, "mr", "mr.");
        }
    }
    protected void GrdPatient_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GrdPatient.EditIndex = -1;
        FillGrid();
    }
    protected void GrdPatient_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label        lblId                        = (Label)GrdPatient.Rows[e.RowIndex].FindControl("lblId");
        DropDownList ddlTitle                     = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlTitle");
        TextBox      txtFirstname                 = (TextBox)GrdPatient.Rows[e.RowIndex].FindControl("txtFirstname");
        TextBox      txtMiddlename                = (TextBox)GrdPatient.Rows[e.RowIndex].FindControl("txtMiddlename");
        TextBox      txtSurname                   = (TextBox)GrdPatient.Rows[e.RowIndex].FindControl("txtSurname");
        TextBox      txtNickname                  = (TextBox)GrdPatient.Rows[e.RowIndex].FindControl("txtNickname");
        DropDownList ddlGender                    = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlGender");
        DropDownList ddlDOB_Day                   = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDOB_Day");
        DropDownList ddlDOB_Month                 = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDOB_Month");
        DropDownList ddlDOB_Year                  = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDOB_Year");
        //CheckBox chkIsClinicPatient = (CheckBox)GrdPatient.Rows[e.RowIndex].FindControl("chkIsClinicPatient");
        CheckBox     chkIsDeceased                = (CheckBox)GrdPatient.Rows[e.RowIndex].FindControl("chkIsDeceased");
        CheckBox     chkIsDiabetic                = (CheckBox)GrdPatient.Rows[e.RowIndex].FindControl("chkIsDiabetic");
        CheckBox     chkIsMemberDiabetesAustralia = (CheckBox)GrdPatient.Rows[e.RowIndex].FindControl("chkIsMemberDiabetesAustralia");
        DropDownList ddlDARev_Day                 = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDARev_Day");
        DropDownList ddlDARev_Month               = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDARev_Month");
        DropDownList ddlDARev_Year                = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlDARev_Year");
        DropDownList ddlACInvOffering             = (DropDownList)GrdPatient.Rows[e.RowIndex].FindControl("ddlACInvOffering");

        UserView userView = UserView.GetInstance();

        DataTable dt = Session["patientinfo_data"] as DataTable;
        DataRow[] foundRows = dt.Select("patient_id=" + lblId.Text);
        DataRow row = foundRows[0];
        Patient patient = PatientDB.LoadAll(row);

        Hashtable offeringsHash = OfferingDB.GetHashtable(true, -1, null, true);
        PatientDB.AddACOfferings(ref offeringsHash, ref patient);

        int acInvOfferingID_New = patient.ACInvOffering == null ? -1 : patient.ACInvOffering.OfferingID;
        int acPatOfferingID_New = patient.ACPatOffering == null ? -1 : patient.ACPatOffering.OfferingID;

        if (patient.ACInvOffering == null || patient.ACPatOffering == null)
        {
            acInvOfferingID_New = Convert.ToInt32(ddlACInvOffering.SelectedValue);
            acPatOfferingID_New = Convert.ToInt32(ddlACInvOffering.SelectedValue);
        }
        else if (userView.IsAgedCareView && patient.ACInvOffering.OfferingID != Convert.ToInt32(ddlACInvOffering.SelectedValue))
        {
            int acInvOfferingID_Old = patient.ACInvOffering == null ? -1 : patient.ACInvOffering.OfferingID;
            int acPatOfferingID_Old = patient.ACPatOffering == null ? -1 : patient.ACPatOffering.OfferingID;

            acInvOfferingID_New = Convert.ToInt32(ddlACInvOffering.SelectedValue);
            acPatOfferingID_New = acPatOfferingID_Old;

            int acInvAcPtTypeID_New = ((Offering)offeringsHash[Convert.ToInt32(ddlACInvOffering.SelectedValue)]).AgedCarePatientType.ID;
            int acInvAcPtTypeID_Old = patient.ACInvOffering.AgedCarePatientType.ID;

	        //when updating:
	        //- if changing to LC/HC/LCF/HCUF    - change BOTH to that (to make sure second is always clearly the pt type)
	        //- if changing to MC/DVA/TAC/LCE/HCE
            //  - if prev_first is LC/HC/LCF/HCUF     - move prev_first to second, and set first as selected
            //  - if prev_first is MC/DVA/TAC/LCE/HCE - set first as selected (and leave second)

            if ((new List<int> { 2, 3, 4, 5 }).Contains(acInvAcPtTypeID_New))
            {
                acPatOfferingID_New = acInvOfferingID_New;
            }
            else if ((new List<int> { 6, 7, 8, 9, 10 }).Contains(acInvAcPtTypeID_New))
            {
                if ((new List<int> { 2, 3, 4, 5 }).Contains(acInvAcPtTypeID_Old))
                    acPatOfferingID_New = acInvOfferingID_Old;
            }
            else // (?)
                ; //
        }



        PatientHistoryDB.Insert(patient.PatientID, patient.IsClinicPatient, patient.IsGPPatient, patient.IsDeleted, patient.IsDeceased,
                                patient.FlashingText, patient.FlashingTextAddedBy == null ? -1 : patient.FlashingTextAddedBy.StaffID, patient.FlashingTextLastModifiedDate,
                                patient.PrivateHealthFund, patient.ConcessionCardNumber, patient.ConcessionCardExpiryDate, patient.IsDiabetic, patient.IsMemberDiabetesAustralia, patient.DiabeticAAassessmentReviewDate, patient.ACInvOffering == null ? -1 : patient.ACInvOffering.OfferingID, patient.ACPatOffering == null ? -1 : patient.ACPatOffering.OfferingID, patient.Login, patient.Pwd, patient.IsCompany, patient.ABN, patient.NextOfKinName, patient.NextOfKinRelation, patient.NextOfKinContactInfo, 
                                patient.Person.Title.ID, patient.Person.Firstname, patient.Person.Middlename, patient.Person.Surname, patient.Person.Nickname, patient.Person.Gender, patient.Person.Dob, Convert.ToInt32(Session["StaffID"]));

        PersonDB.Update(patient.Person.PersonID, Convert.ToInt32(ddlTitle.SelectedValue), Utilities.FormatName(txtFirstname.Text), Utilities.FormatName(txtMiddlename.Text), Utilities.FormatName(txtSurname.Text), txtNickname.Text, ddlGender.SelectedValue, GetDate(ddlDOB_Day.SelectedValue, ddlDOB_Month.SelectedValue, ddlDOB_Year.SelectedValue), DateTime.Now);
        PatientDB.Update(patient.PatientID, patient.Person.PersonID, patient.IsClinicPatient, patient.IsGPPatient, chkIsDeceased.Checked, patient.FlashingText, patient.FlashingTextAddedBy == null ? -1 : patient.FlashingTextAddedBy.StaffID, patient.FlashingTextLastModifiedDate, patient.PrivateHealthFund, patient.ConcessionCardNumber, patient.ConcessionCardExpiryDate, chkIsDiabetic.Checked, chkIsMemberDiabetesAustralia.Checked, GetDate(ddlDARev_Day.SelectedValue, ddlDARev_Month.SelectedValue, ddlDARev_Year.SelectedValue), acInvOfferingID_New, acPatOfferingID_New, patient.Login, patient.Pwd, patient.IsCompany, patient.ABN, patient.NextOfKinName, patient.NextOfKinRelation, patient.NextOfKinContactInfo);


        GrdPatient.EditIndex = -1;
        FillGrid();

    }
    protected void GrdPatient_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblId = (Label)GrdPatient.Rows[e.RowIndex].FindControl("lblId");
        DataTable dt = Session["patientinfo_data"] as DataTable;
        DataRow[] foundRows = dt.Select("patient_id=" + lblId.Text);
        int patient_id = Convert.ToInt32(lblId.Text);
        int person_id = Convert.ToInt32(foundRows[0]["person_id"]);


        try
        {
            PatientDB.UpdateInactive(patient_id, Convert.ToInt32(Session["StaffID"]));
            //PersonDB.Delete(person_id);
        }
        catch (ForeignKeyConstraintException fkcEx)
        {
            if (Utilities.IsDev())
                SetErrorMessage("Can not delete because other records depend on this : " + fkcEx.Message);
            else
                SetErrorMessage("Can not delete because other records depend on this");
        }

        FillGrid();
    }
    protected void GrdPatient_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Insert"))
        {
            DropDownList ddlTitle       = (DropDownList)GrdPatient.FooterRow.FindControl("ddlNewTitle");
            TextBox      txtFirstname   = (TextBox)GrdPatient.FooterRow.FindControl("txtNewFirstname");
            TextBox      txtMiddlename  = (TextBox)GrdPatient.FooterRow.FindControl("txtNewMiddlename");
            TextBox      txtSurname     = (TextBox)GrdPatient.FooterRow.FindControl("txtNewSurname");
            TextBox      txtNickname    = (TextBox)GrdPatient.FooterRow.FindControl("txtNewNickname");
            DropDownList ddlGender      = (DropDownList)GrdPatient.FooterRow.FindControl("ddlNewGender");
            TextBox      txtDOB         = (TextBox)GrdPatient.FooterRow.FindControl("txtNewDOB");
            TextBox      txtDARev       = (TextBox)GrdPatient.FooterRow.FindControl("txtDARev");

            //CheckBox chkIsClinicPatient = (CheckBox)GrdPatient.FooterRow.FindControl("chkNewIsClinicPatient");
            CheckBox     chkIsDeceased  = (CheckBox)GrdPatient.FooterRow.FindControl("chkNewIsDeceased");
            CheckBox     chkIsDiabetic  = (CheckBox)GrdPatient.FooterRow.FindControl("chkNewIsDiabetic");
            CheckBox     chkIsMemberDiabetesAustralia = (CheckBox)GrdPatient.FooterRow.FindControl("chkNewIsMemberDiabetesAustralia");


            UserView userView = UserView.GetInstance();
            DateTime dob   = GetDate(txtDOB.Text.Trim());
            DateTime darev = GetDate(txtDARev.Text.Trim());

            int person_id  = -1;
            int patient_id = -1;
            int register_patient_id = -1;

            try
            {
                Staff loggedInStaff = StaffDB.GetByID(Convert.ToInt32(Session["StaffID"]));
                person_id  = PersonDB.Insert(loggedInStaff.Person.PersonID, Convert.ToInt32(ddlTitle.SelectedValue), Utilities.FormatName(txtFirstname.Text), Utilities.FormatName(txtMiddlename.Text), Utilities.FormatName(txtSurname.Text), txtNickname.Text, ddlGender.SelectedValue, dob);
                patient_id = PatientDB.Insert(person_id, userView.IsClinicView, userView.IsGPView, chkIsDeceased.Checked, "", -1, DateTime.MinValue, "", "", DateTime.MinValue, chkIsDiabetic.Checked, chkIsMemberDiabetesAustralia.Checked, darev, -1, -1, "", "", false, "", "", "", "");
                if (!UserView.GetInstance().IsAdminView)
                    register_patient_id = RegisterPatientDB.Insert(Convert.ToInt32(Session["OrgID"]), patient_id);

                FillGrid();
            }
            catch (Exception)
            {
                // roll back - backwards of creation order
                RegisterPatientDB.Delete(register_patient_id);
                PatientDB.Delete(patient_id);
                PersonDB.Delete(person_id);
            }
        }

        if (e.CommandName.Equals("_Delete") || e.CommandName.Equals("_UnDelete"))
        {
            int patient_id = Convert.ToInt32(e.CommandArgument);

            try
            {
                if (e.CommandName.Equals("_Delete"))
                    PatientDB.UpdateInactive(patient_id, Convert.ToInt32(Session["StaffID"]));
                else
                    PatientDB.UpdateActive(patient_id, Convert.ToInt32(Session["StaffID"]));
            }
            catch (ForeignKeyConstraintException fkcEx)
            {
                if (Utilities.IsDev())
                    SetErrorMessage("Can not delete because other records depend on this : " + fkcEx.Message);
                else
                    SetErrorMessage("Can not delete because other records depend on this");
            }

            FillGrid();
        }
    }
    protected void GrdPatient_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GrdPatient.EditIndex = e.NewEditIndex;
        FillGrid();
    }
    protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        // dont allow sorting if in edit mode
        if (GrdPatient.EditIndex >= 0)
            return;

        DataTable dataTable = Session["patientinfo_data"] as DataTable;

        if (dataTable != null)
        {
            if (Session["patientinfo_sortexpression"] == null)
                Session["patientinfo_sortexpression"] = "";

            DataView dataView = new DataView(dataTable);
            string[] sortData = Session["patientinfo_sortexpression"].ToString().Trim().Split(' ');
            string newSortExpr = (e.SortExpression == sortData[0] && sortData[1] == "ASC") ? "DESC" : "ASC";
            dataView.Sort = e.SortExpression + " " + newSortExpr;
            Session["patientinfo_sortexpression"] = e.SortExpression + " " + newSortExpr;

            GrdPatient.DataSource = dataView;
            GrdPatient.DataBind();
        }
    }
    protected void GrdPatient_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdPatient.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    #endregion

    #region FillStreetsList, FillDOBLists

    protected void FillStreetsList()
    {
        lstStreets.Items.Clear();

        DataTable streets = AddressChannelDB.GetDataTable();
        foreach (DataRow row in streets.Rows)
            lstStreets.Items.Add(new ListItem(row["ac_descr"].ToString() + " " + row["act_descr"].ToString(), row["ac_address_channel_id"].ToString()));
    }
    protected void FillDOBLists()
    {
        ddlDOB_Day.Items.Clear();
        ddlDOB_Month.Items.Clear();
        ddlDOB_Year.Items.Clear();

        ddlDOB_Day.Items.Add(new ListItem("--", "-1"));
        ddlDOB_Month.Items.Add(new ListItem("--", "-1"));
        ddlDOB_Year.Items.Add(new ListItem("----", "-1"));

        for (int i = 1; i <= 31; i++)
            ddlDOB_Day.Items.Add(new ListItem(i.ToString(), i.ToString()));
        for (int i = 1; i <= 12; i++)
            ddlDOB_Month.Items.Add(new ListItem(i.ToString(), i.ToString()));
        for (int i = 1900; i <= DateTime.Today.Year; i++)
            ddlDOB_Year.Items.Add(new ListItem(i.ToString(), i.ToString()));
    }

    #endregion

    #region HideShowExtendedSearch, FillExtendedSearchLists

    protected void HideShowExtendedSearch(bool show, bool incSetCheckbox)
    {
        if (incSetCheckbox)
            chkShowExtendedSearch.Checked = show;

        if (show)
            td_extended_search.Attributes.Remove("class");
        else
            td_extended_search.Attributes["class"] = "hiddencol";
    }

    protected void FillExtendedSearchLists(bool show)
    {
        td_extended_search.Visible = show;

        if (show)
        {

            tr_Contact.Visible = false;
            tr_ContactAus.Visible = false;

            if (Utilities.GetAddressType().ToString() == "Contact")
            {
                tr_Contact.Visible = true;
                FillStreetsList();
            }
            else if (Utilities.GetAddressType().ToString() == "ContactAus")
                tr_ContactAus.Visible = true;
            else
                throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());

            FillDOBLists();
        }
        else
        {
            lstStreets.Items.Clear();
            ddlDOB_Day.Items.Clear();
            ddlDOB_Month.Items.Clear();
            ddlDOB_Year.Items.Clear();
        }
    }

    #endregion

    #region DOBAllOrNoneCheck, DARevAllOrNoneCheck, IsValidDate, GetDate

    protected void DOBAllOrNoneCheck(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv          = (CustomValidator)sender;
            GridViewRow     grdRow      = ((GridViewRow)cv.Parent.Parent);
            //TextBox txtDate = grdRow.RowType == DataControlRowType.Footer ? (TextBox)grdRow.FindControl("txtNewDOB") : (TextBox)grdRow.FindControl("txtDOB");
            DropDownList  _ddlDOB_Day   = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDOB_Day"   : "ddlDOB_Day");
            DropDownList  _ddlDOB_Month = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDOB_Month" : "ddlDOB_Month");
            DropDownList  _ddlDOB_Year  = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDOB_Year"  : "ddlDOB_Year");

            e.IsValid = IsValidDate(_ddlDOB_Day.SelectedValue, _ddlDOB_Month.SelectedValue, _ddlDOB_Year.SelectedValue);
        }
        catch (Exception)
        {
            e.IsValid = false;
        }
    }
    protected void DARevAllOrNoneCheck(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv     = (CustomValidator)sender;
            GridViewRow     grdRow = ((GridViewRow)cv.Parent.Parent);
            //TextBox txtDate = grdRow.RowType == DataControlRowType.Footer ? (TextBox)grdRow.FindControl("txtNewDARev") : (TextBox)grdRow.FindControl("txtDARev");
            DropDownList _ddlDARev_Day   = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDARev_Day" : "ddlDARev_Day");
            DropDownList _ddlDARev_Month = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDARev_Month" : "ddlDARev_Month");
            DropDownList _ddlDARev_Year  = (DropDownList)grdRow.FindControl(grdRow.RowType == DataControlRowType.Footer ? "ddlNewDARev_Year" : "ddlDARev_Year");

            e.IsValid = IsValidDate(_ddlDARev_Day.SelectedValue, _ddlDARev_Month.SelectedValue, _ddlDARev_Year.SelectedValue);
        }
        catch (Exception)
        {
            e.IsValid = false;
        }
    }
    public bool IsValidDate(string day, string month, string year)
    {
        bool invalid = ((day == "-1" || month == "-1" || year == "-1") && (day != "-1" || month != "-1" || year != "-1"));

        if ((day == "-1" && month == "-1" && year == "-1"))
            return true;
        else if ((day == "-1" || month == "-1" || year == "-1"))
            return false;

        try
        {
            DateTime d = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    protected DateTime GetDate(string day, string month, string year)
    {
        if ((day == "-1" && month == "-1" && year == "-1"))
            return DateTime.MinValue;

        return new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
    }

    protected void ValidDOBCheck(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv = (CustomValidator)sender;
            GridViewRow grdRow = ((GridViewRow)cv.Parent.Parent);
            TextBox txtDate = grdRow.RowType == DataControlRowType.Footer ? (TextBox)grdRow.FindControl("txtNewDOB") : (TextBox)grdRow.FindControl("txtDOB");

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
    protected void ValidDARevCheck(object sender, ServerValidateEventArgs e)
    {
        try
        {
            CustomValidator cv = (CustomValidator)sender;
            GridViewRow grdRow = ((GridViewRow)cv.Parent.Parent);
            TextBox txtDate = grdRow.RowType == DataControlRowType.Footer ? (TextBox)grdRow.FindControl("txtNewDARev") : (TextBox)grdRow.FindControl("txtDARev");

            if (!IsValidDate(txtDate.Text))
                throw new Exception();

            DateTime d = GetDate(txtDate.Text);

            e.IsValid = (d == DateTime.MinValue) || (Utilities.IsValidDBDateTime(d));
        }
        catch (Exception)
        {
            e.IsValid = false;
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
    public bool IsValidDate(string inDate)
    {
        inDate = inDate.Trim();
        try
        {
            if (inDate.Length == 0)
                return true;

            if (!System.Text.RegularExpressions.Regex.IsMatch(inDate, @"^\d{2}\-\d{2}\-\d{4}$"))
                return false;

            string[] parts = inDate.Split('-');
            DateTime d = new DateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]));
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    #endregion

    #region chkShowExtendedSearch_Submit, chkShowDeceased_Submit, chkShowDeleted_Submit, chkShowOnlyMyPatients_Submit. chkUsePaging_CheckedChanged

    protected void chkShowExtendedSearch_Submit(object sender, EventArgs e)
    {
        FillExtendedSearchLists(chkShowExtendedSearch.Checked);
    }

    protected void chkShowDeceased_Submit(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Update(chkShowDeceased.Checked, url, "show_deceased", "1");
        Response.Redirect(url);
    }

    protected void chkShowDeleted_Submit(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Update(chkShowDeleted.Checked, url, "show_deleted", "1");
        Response.Redirect(url);
    }

    protected void chkShowOnlyMyPatients_Submit(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Update(!chkShowOnlyMyPatients.Checked, url, "show_only_my_patients", "0");
        Response.Redirect(url);
    }

    protected void chkUsePaging_CheckedChanged(object sender, EventArgs e)
    {
        this.GrdPatient.AllowPaging = chkUsePaging.Checked;
        FillGrid();
    }

    #endregion

    #region btnSearchXXX_Click

    protected void btnSearchSurname_Click(object sender, EventArgs e)
    {
        if (!Regex.IsMatch(txtSearchSurname.Text, @"^[a-zA-Z\-\']*$"))
        {
            SetErrorMessage("Search text can only be letters and hyphens");
            return;
        }
        else if (txtSearchSurname.Text.Trim().Length == 0)
        {
            SetErrorMessage("No search text entered");
            return;
        }
        else
            HideErrorMessage();


        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "surname_search", txtSearchSurname.Text);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");


        Response.Redirect(url);
    }
    protected void btnClearSurnameSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchPhNum_Click(object sender, EventArgs e)
    {
        if (txtSearchPhNum.Text.Trim().Length == 0)
            return;

        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "phone_search", Regex.Replace(txtSearchPhNum.Text, "[^0-9]", ""));
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearPhNumSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchEmail_Click(object sender, EventArgs e)
    {
        if (txtSearchEmail.Text.Trim().Length == 0)
            return;

        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "email_search", txtSearchEmail.Text.Trim());
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearEmailSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchMedicareCardNo_Click(object sender, EventArgs e)
    {
        if (!Regex.IsMatch(txtSearchMedicareCardNo.Text, @"^[0-9a-zA-Z]*$"))
        {
            SetErrorMessage("Search text can only be numbers and letters");
            return;
        }
        else if (txtSearchMedicareCardNo.Text.Trim().Length == 0)
        {
            SetErrorMessage("No search text entered");
            return;
        }
        else
            HideErrorMessage();


        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "medicare_card_no_search", txtSearchMedicareCardNo.Text);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");


        Response.Redirect(url);
    }
    protected void btnClearMedicareCardNoSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchDOB_Click(object sender, EventArgs e)
    {
        string day   = ddlDOB_Day.SelectedValue   == "-1"   ? string.Empty : ddlDOB_Day.SelectedValue;
        string month = ddlDOB_Month.SelectedValue == "-1"   ? string.Empty : ddlDOB_Month.SelectedValue;
        string year  = ddlDOB_Year.SelectedValue  == "-1" ? string.Empty : ddlDOB_Year.SelectedValue;

        if (day == string.Empty && month == string.Empty && year == string.Empty)
        {
            SetErrorMessage("No DOB entered");
            return;
        }
        else
            HideErrorMessage();


        string dob = day + "_" + month + "_" + year;


        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "dob_search", dob);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearDOBSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchStreet_Click(object sender, EventArgs e)
    {
        if (Utilities.GetAddressType().ToString() == "Contact")
        {
            string selectedIDs = GetSelectedStreetIDList();

            string url = ClearSearchesFromUrl(Request.RawUrl);
            url = UrlParamModifier.AddEdit(url, "street_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            Response.Redirect(url);
        }
        else if (Utilities.GetAddressType().ToString() == "ContactAus")
        {
            if (txtSearchStreet.Text.Trim().Length == 0)
                return;

            string url = ClearSearchesFromUrl(Request.RawUrl);
            url = UrlParamModifier.AddEdit(url, "street_search", txtSearchStreet.Text.Trim());
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            Response.Redirect(url);
        }
        else
            throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());
    }
    protected void btnClearStreetSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchSuburb_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = suburbID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "suburb_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearSuburbSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "suburb_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnSuburbSelectionUpdate_Click

    protected void btnSuburbSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateSuburbInfo(true);
    }

    protected void UpdateSuburbInfo(bool redirect)
    {
        int newSuburbID = Convert.ToInt32(suburbID.Value);

        if (newSuburbID == -1)
        {
            lblSuburbText.Text = "--";
        }
        else
        {
            Suburb suburb = SuburbDB.GetByID(newSuburbID);
            lblSuburbText.Text = suburb.Name + ", " + suburb.State + "(" + suburb.Postcode + ")";
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newSuburbID != -1, url, "suburb_search", newSuburbID == -1 ? "" : newSuburbID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion

    protected void btnSearchReferrer_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = referrerID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "referrer_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearReferrerSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "referrer_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnReferrerSelectionUpdate_Click

    protected void btnReferrerSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateReferrerInfo(true);
    }

    protected void UpdateReferrerInfo(bool redirect)
    {
        return;

        int newReferrerID = Convert.ToInt32(referrerID.Value);

        if (newReferrerID == -1)
        {
            lblReferrerText.Text = "--";
        }
        else
        {
            RegisterReferrer regRef = RegisterReferrerDB.GetByID(newReferrerID);
            lblReferrerText.Text = regRef.Referrer.Person.Surname + ", " + regRef.Referrer.Person.Firstname + " [" + regRef.Organisation.Name + "]";
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newReferrerID != -1, url, "referrer_search", newReferrerID == -1 ? "" : newReferrerID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion

    protected void btnSearchReferrerPerson_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = referrerPersonID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "referrer_person_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearReferrerPersonSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "referrer_person_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnReferrerPersonSelectionUpdate_Click

    protected void btnReferrerPersonSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateReferrerPersonInfo(true);
    }

    protected void UpdateReferrerPersonInfo(bool redirect)
    {
        return;

        int newReferrerPersonID = Convert.ToInt32(referrerPersonID.Value);

        if (newReferrerPersonID == -1)
        {
            lblReferrerPersonText.Text = "--";
        }
        else
        {
            Referrer referrer = ReferrerDB.GetByID(newReferrerPersonID);
            lblReferrerPersonText.Text = referrer.Person.Surname + ", " + referrer.Person.Firstname;
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newReferrerPersonID != -1, url, "referrer_person_search", newReferrerPersonID == -1 ? "" : newReferrerPersonID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion

    protected void btnSearchReferrerOrg_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = referrerOrgID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "referrer_org_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearReferrerOrgSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "referrer_org_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnReferrerOrgSelectionUpdate_Click

    protected void btnReferrerOrgSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateReferrerOrgInfo(true);
    }

    protected void UpdateReferrerOrgInfo(bool redirect)
    {
        return;

        int newReferrerOrgID = Convert.ToInt32(referrerOrgID.Value);

        if (newReferrerOrgID == -1)
        {
            lblReferrerOrgText.Text = "--";
        }
        else
        {
            Organisation org = OrganisationDB.GetByID(newReferrerOrgID);
            lblReferrerOrgText.Text = org.Name;
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newReferrerOrgID != -1, url, "referrer_org_search", newReferrerOrgID == -1 ? "" : newReferrerOrgID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion

    protected void btnSearchInternalOrg_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = internalOrgID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "internal_org_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearInternalOrgSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "internal_org_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnInternalOrgSelectionUpdate_Click

    protected void btnInternalOrgSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateInternalOrgInfo(true);
    }

    protected void UpdateInternalOrgInfo(bool redirect)
    {
        return;

        int newInternalOrgID = Convert.ToInt32(internalOrgID.Value);

        if (newInternalOrgID == 0)
        {
            lblInternalOrgText.Text = "--";
        }
        else
        {
            Organisation org = OrganisationDB.GetByID(newInternalOrgID);
            lblInternalOrgText.Text = org.Name;
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newInternalOrgID != 0, url, "internal_org", newInternalOrgID == 0 ? "" : newInternalOrgID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion

    protected void btnSearchProvider_Click(object sender, EventArgs e)
    {
        try
        {
            string selectedIDs = providerID.Value;

            string url = Request.RawUrl;
            url = UrlParamModifier.AddEdit(url, "provider_search", selectedIDs);
            url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearProviderSearch_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = UrlParamModifier.Remove(url, "provider_search");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    #region btnProviderSelectionUpdate_Click

    protected void btnProviderSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateProviderInfo(true);
    }

    protected void UpdateProviderInfo(bool redirect)
    {
        return;

        int newProviderID = Convert.ToInt32(providerID.Value);

        if (newProviderID == 0)
        {
            lblProviderText.Text = "--";
        }
        else
        {
            Organisation org = OrganisationDB.GetByID(newProviderID);
            lblProviderText.Text = org.Name;
        }

        if (redirect)
        {
            string url = Request.RawUrl;
            url = UrlParamModifier.Update(newProviderID != 0, url, "internal_org", newProviderID == 0 ? "" : newProviderID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion


    protected void btnSearchOnlyDiabetics_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "only_diabetics", chkOnlyDiabetics.Checked ? "1" : "0");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearOnlyDiabeticsSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchOnlyMedicareEPC_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "only_medicare_epc", chkOnlyMedicareEPC.Checked ? "1" : "0");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearOnlyMedicareEPCSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }

    protected void btnSearchOnlyDVAEPC_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.AddEdit(url, "only_dva_epc", chkOnlyDVAEPC.Checked ? "1" : "0");
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }
    protected void btnClearOnlyDVAEPCSearch_Click(object sender, EventArgs e)
    {
        string url = ClearSearchesFromUrl(Request.RawUrl);
        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        Response.Redirect(url);
    }


    protected string AddSearchesToURL(string url)
    {
        HideErrorMessage();

        txtSearchSurname.Text        = txtSearchSurname.Text.Trim();
        txtSearchFirstName.Text      = txtSearchFirstName.Text.Trim();
        txtSearchEmail.Text          = txtSearchEmail.Text.Trim();
        txtSearchPhNum.Text          = Regex.Replace(txtSearchPhNum.Text.Trim(), "[^0-9]", "");
        txtSearchStreet.Text         = txtSearchStreet.Text.Trim();
        txtSearchMedicareCardNo.Text = txtSearchMedicareCardNo.Text.Trim();

        if (txtSearchSurname.Text.Length > 0 && !Regex.IsMatch(txtSearchSurname.Text, @"^[a-zA-Z\-\']*$"))
            throw new CustomMessageException("Surname search text can only be letters and hyphens");
        if (txtSearchFirstName.Text.Length > 0 && !Regex.IsMatch(txtSearchFirstName.Text, @"^[a-zA-Z\-\']*$"))
            throw new CustomMessageException("First name search text can only be letters and hyphens");
        if (txtSearchMedicareCardNo.Text.Length > 0 && !Regex.IsMatch(txtSearchMedicareCardNo.Text, @"^[0-9a-zA-Z]*$"))
            throw new CustomMessageException("Medicare card search text can only be numbers and letters");

        url = UrlParamModifier.Update(txtSearchSurname.Text.Length   > 0, url, "surname_search",   txtSearchSurname.Text);
        url = UrlParamModifier.Update(txtSearchFirstName.Text.Length > 0, url, "firstname_search", txtSearchFirstName.Text);
        url = UrlParamModifier.Update(txtSearchEmail.Text.Length     > 0, url, "email_search",     txtSearchEmail.Text);
        url = UrlParamModifier.Update(txtSearchPhNum.Text.Length     > 0, url, "phone_search",     txtSearchPhNum.Text);

        if (Utilities.GetAddressType().ToString() == "Contact") {
            string selectedIDs = GetSelectedStreetIDList();
            url = UrlParamModifier.Update(selectedIDs.Length > 0, url, "street_search", selectedIDs);
        } else if (Utilities.GetAddressType().ToString() == "ContactAus") {
            url = UrlParamModifier.Update(txtSearchStreet.Text.Length > 0, url, "street_search", txtSearchStreet.Text);
        } else
            throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());

        string day   = ddlDOB_Day.SelectedValue   == "-1" ? string.Empty : ddlDOB_Day.SelectedValue;
        string month = ddlDOB_Month.SelectedValue == "-1" ? string.Empty : ddlDOB_Month.SelectedValue;
        string year  = ddlDOB_Year.SelectedValue  == "-1" ? string.Empty : ddlDOB_Year.SelectedValue;
        bool   isDOBEntered = day != string.Empty || month != string.Empty || year != string.Empty;
        string dob          = day + "_" + month + "_" + year;
        url = UrlParamModifier.Update(isDOBEntered, url, "dob_search", dob);

        url = UrlParamModifier.Update(txtSearchMedicareCardNo.Text.Length   > 0, url, "medicare_card_no_search",   txtSearchMedicareCardNo.Text);

        url = UrlParamModifier.Update(chkOnlyDiabetics.Checked,   url, "only_diabetics",    chkOnlyDiabetics.Checked   ? "1" : "0");
        url = UrlParamModifier.Update(chkOnlyMedicareEPC.Checked, url, "only_medicare_epc", chkOnlyMedicareEPC.Checked ? "1" : "0");
        url = UrlParamModifier.Update(chkOnlyDVAEPC.Checked,      url, "only_dva_epc",      chkOnlyDVAEPC.Checked      ? "1" : "0");
       

        url = UrlParamModifier.Update(chkShowExtendedSearch.Checked, url, "extended_search_open", "1");
        return url;
    }
    protected string ClearSearchesFromUrl(string url)
    {
        url = UrlParamModifier.Remove(url, "surname_search");
        url = UrlParamModifier.Remove(url, "surname_starts_with");
        url = UrlParamModifier.Remove(url, "firstname_search");
        url = UrlParamModifier.Remove(url, "firstname_starts_with");
        url = UrlParamModifier.Remove(url, "suburb_search");
        url = UrlParamModifier.Remove(url, "street_search");
        url = UrlParamModifier.Remove(url, "phone_search");
        url = UrlParamModifier.Remove(url, "email_search");
        url = UrlParamModifier.Remove(url, "medicare_card_no_search");
        url = UrlParamModifier.Remove(url, "medicare_card_no_starts_with");
        url = UrlParamModifier.Remove(url, "dob_search");
        url = UrlParamModifier.Remove(url, "referrer_search");
        url = UrlParamModifier.Remove(url, "referrer_person_search");
        url = UrlParamModifier.Remove(url, "referrer_org_search");
        url = UrlParamModifier.Remove(url, "internal_org_search");
        url = UrlParamModifier.Remove(url, "only_diabetics");
        url = UrlParamModifier.Remove(url, "only_medicare_epc");
        url = UrlParamModifier.Remove(url, "only_dva_epc");

        return url;
    }


    protected string GetSelectedStreetIDList()
    {
        string s = string.Empty;
        foreach (ListItem li in lstStreets.Items)
        {
            if (li.Selected)
            {
                if (s.Length > 0)
                    s += "_";
                s += li.Value;
            }
        }
        return s;
    }


    protected void btnSearchAll_Click(object sender, EventArgs e)
    {
        try
        {
            string url = Request.RawUrl;
            url = AddSearchesToURL(url);
            Response.Redirect(url);
        }
        catch (CustomMessageException ex)
        {
            SetErrorMessage(ex.Message);
        }
    }
    protected void btnClearAll_Click(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        url = ClearSearchesFromUrl(url);
        Response.Redirect(url);
    }


    #endregion

    #region btnExport_Click

    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = Session["patientinfo_data"] as DataTable;
        bool tblEmpty = (dt.Rows.Count == 1 && dt.Rows[0][0] == DBNull.Value);
        if (tblEmpty)
            dt.Rows.RemoveAt(0);

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


        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("Patient ID").Append(",");
        sb.Append("Title").Append(",");
        sb.Append("Firstname").Append(",");
        sb.Append("Middlename").Append(",");
        sb.Append("Surname").Append(",");
        sb.Append("Gender").Append(",");
        sb.Append("DOB").Append(",");
        sb.Append("Is Diabetic").Append(",");
        sb.Append("Is Member Diabetes Australia").Append(",");
        sb.Append("Is Deceased").Append(",");
        sb.Append("Date Added").Append(",");
        sb.Append("Addresses");
        sb.AppendLine();


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Patient patient   = PatientDB.LoadAll(dt.Rows[i]);


            sb.Append(patient.PatientID.ToString()).Append(",");
            sb.Append(patient.Person.Title == null || patient.Person.Title.ID == 0 || patient.Person.Title.Descr == null ? "" : patient.Person.Title.Descr.ToString()).Append(",");
            sb.Append(patient.Person.Firstname.ToString()).Append(",");
            sb.Append(patient.Person.Middlename.ToString()).Append(",");
            sb.Append(patient.Person.Surname.ToString()).Append(",");
            sb.Append(patient.Person.Gender.ToString()).Append(",");
            sb.Append(patient.Person.Dob == DateTime.MinValue ? "" : patient.Person.Dob.ToString("dd'/'MM'/' yyyy")).Append(",");
            sb.Append(patient.IsDiabetic ? "Yes" : "No").Append(",");
            sb.Append(patient.IsMemberDiabetesAustralia ? "Yes" : "No").Append(",");
            sb.Append(patient.IsDeceased ? "Yes" : "No").Append(",");
            sb.Append(patient.PatientDateAdded.ToString("dd MM yyyy")).Append(",");

            System.Text.StringBuilder sbAddresses = new System.Text.StringBuilder();

            if (Utilities.GetAddressType().ToString() == "Contact")
            {
                Contact[] addresses = contactHash[patient.Person.EntityID] as Contact[];

                for (int j = 0; addresses != null && j < addresses.Length; j++)
                {
                    Contact address = addresses[j];
                    if (j > 0)
                        sbAddresses.AppendLine();
                    sbAddresses.Append(address.ContactType.Descr).Append(",");
                    sbAddresses.Append(address.AddrLine1).Append(",");
                    sbAddresses.Append(address.AddrLine2).Append(",");
                    sbAddresses.Append(address.AddressChannel == null ? "" : address.AddressChannel.DisplayName).Append(",");
                    sbAddresses.Append(address.Suburb         == null ? "" : address.Suburb.Name).Append(",");
                    sbAddresses.Append(address.Suburb         == null ? "" : address.Suburb.Postcode).Append(",");
                    sbAddresses.Append(address.Country        == null ? "" : address.Country.Descr).Append(",");
                    sbAddresses.Append(address.Site           == null ? "" : address.Site.Name).Append(",");
                    sbAddresses.Append(address.IsBilling).Append(",");
                    sbAddresses.Append(address.IsNonBilling);
                }
            }
            else if (Utilities.GetAddressType().ToString() == "ContactAus")
            {
                ContactAus[] addresses = contactHash[patient.Person.EntityID] as ContactAus[];

                for (int j = 0; addresses != null && j < addresses.Length; j++)
                {
                    ContactAus address = addresses[j];
                    if (j > 0)
                        sbAddresses.AppendLine();
                    sbAddresses.Append(address.ContactType.Descr).Append(",");
                    sbAddresses.Append(address.AddrLine1).Append(",");
                    sbAddresses.Append(address.AddrLine2).Append(",");
                    sbAddresses.Append(address.StreetName + (address.AddressChannelType == null ? "" : (address.StreetName.Length == 0 ? "" : " ") + address.AddressChannelType.Descr)).Append(",");
                    sbAddresses.Append(address.Suburb         == null ? "" : address.Suburb.Name).Append(",");
                    sbAddresses.Append(address.Suburb         == null ? "" : address.Suburb.Postcode).Append(",");
                    sbAddresses.Append(address.Country        == null ? "" : address.Country.Descr).Append(",");
                    sbAddresses.Append(address.Site           == null ? "" : address.Site.Name).Append(",");
                    sbAddresses.Append(address.IsBilling).Append(",");
                    sbAddresses.Append(address.IsNonBilling);
                }            
            }
            else
                throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());

            sb.Append("\"" + sbAddresses.ToString() + "\"");
            sb.AppendLine();
        }

        ExportCSV(Response, sb.ToString(), "patient_export.csv");
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