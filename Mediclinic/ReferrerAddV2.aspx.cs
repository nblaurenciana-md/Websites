﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class ReferrerAddV2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            HideErrorMessage();

            if (!IsPostBack)
            {
                PagePermissions.EnforcePermissions_RequireAny(Session, Response, true, true, true, true, true, false);
                SetupGUI();
                FillEmptyForm();
                txtFirstname.Focus();
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

    private void SetupGUI()
    {
        if (GetUrlIsPopup()) // remove menu and header stuff
            Utilities.UpdatePageHeaderV2(Page.Master, true);

        hiddenIsMobileDevice.Value = Utilities.IsMobileDevice(Request) ? "1" : "0";

        btnCancel.Visible = GetUrlIsPopup();

        bool editable = true; // GetUrlParamType() == UrlParamType.Add || GetUrlParamType() == UrlParamType.Edit;
        Utilities.SetEditControlBackColour(ddlTitle ,           editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtFirstname ,       editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtMiddlename ,      editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtSurname ,         editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlGender ,          editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtProviderNumber,   editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlOrgsList,         editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtOrgName,          editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtOrgABN,           editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtOrgACN,           editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtOrgComments,      editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
    }

    private bool GetUrlIsPopup()
    {
        return Request.QueryString["popup"] != null && Request.QueryString["popup"] == "1";
    }

    #region SetupAddressGUI()

    protected void SetupAddressGUI()
    {
        string allFeaturesType = "dialogWidth:500px;dialogHeight:750px;center:yes;resizable:no; scroll:no";
        string jsType = "javascript:window.showModalDialog('" + "ContactTypeListV2.aspx', '', '" + allFeaturesType + "');document.getElementById('btnUpdateAddressType').click();return false;";

        string allFeatures = "dialogWidth:1100px;dialogHeight:600px;center:yes;resizable:no; scroll:no";
        string js = "javascript:window.showModalDialog('" + "StreetAndSuburbInfo.aspx', '', '" + allFeatures + "');document.getElementById('btnUpdateAddressStreetAndSuburb').click();return false;";

        lnkAddressUpdateType.NavigateUrl = "  ";
        lnkAddressUpdateType.Text = "Add/Edit";
        lnkAddressUpdateType.Attributes.Add("onclick", jsType);

        lnkAddressUpdateChannel.NavigateUrl = "  ";
        lnkAddressUpdateChannel.Text = "Add/Edit";
        lnkAddressUpdateChannel.Attributes.Add("onclick", js);

        string allFeaturesType2 = "dialogWidth:500px;dialogHeight:750px;center:yes;resizable:no; scroll:no";
        string jsType2 = "javascript:window.showModalDialog('" + "ContactTypeListV2.aspx', '', '" + allFeaturesType2 + "');document.getElementById('btnUpdatePhoneType').click();return false;";

        string allFeaturesType3 = "dialogWidth:500px;dialogHeight:750px;center:yes;resizable:no; scroll:no";
        string jsType3 = "javascript:window.showModalDialog('" + "ContactTypeListV2.aspx', '', '" + allFeaturesType3 + "');document.getElementById('btnUpdateEmailType').click();return false;";

        lnkEmailUpdateType.NavigateUrl = "  ";
        lnkEmailUpdateType.Text = "Add/Edit";
        lnkEmailUpdateType.Attributes.Add("onclick", jsType3);


        ddlAddressContactType.DataSource = ContactTypeDB.GetDataTable(1);
        ddlAddressContactType.DataBind();
        ddlAddressContactType.SelectedValue = "36"; // set default as business address

        DataTable dt_PhoneNumbers = ContactTypeDB.GetDataTable(2);
        ddlPhoneNumber1.DataSource = dt_PhoneNumbers.Copy();
        ddlPhoneNumber2.DataSource = dt_PhoneNumbers.Copy();
        ddlPhoneNumber1.DataBind();
        ddlPhoneNumber2.DataBind();
        ddlPhoneNumber1.SelectedValue = "30"; // mobile
        ddlPhoneNumber2.SelectedValue = "34"; // office
        lnkPhone1UpdateType.NavigateUrl = "  ";
        lnkPhone1UpdateType.Text = "Add/Edit";
        lnkPhone1UpdateType.Attributes.Add("onclick", jsType2);
        lnkPhone2UpdateType.NavigateUrl = "  ";
        lnkPhone2UpdateType.Text = "Add/Edit";
        lnkPhone2UpdateType.Attributes.Add("onclick", jsType2);


        DataTable emailWeb = ContactTypeDB.GetDataTable(4);
        DataView dv = emailWeb.DefaultView;
        dv.RowFilter = string.Format("at_contact_type_id={0}", 27);
        DataTable emailOnly = dv.ToTable();
        ddlEmailContactType.DataSource = emailOnly;

        //ddlEmailContactType.DataSource = ContactTypeDB.GetDataTable(4);
        ddlEmailContactType.DataBind();

        if (Utilities.GetAddressType().ToString() == "Contact")
        {
           // newOrgAddressRow5.Visible = true;
            DataTable addrChannels = AddressChannelDB.GetDataTable();
            ddlAddressAddressChannel.Items.Add(new ListItem("--", "-1"));
            foreach (DataRow row in addrChannels.Rows)
                ddlAddressAddressChannel.Items.Add(new ListItem(row["ac_descr"] + " " + row["act_descr"], row["ac_address_channel_id"].ToString()));
        }
        else if (Utilities.GetAddressType().ToString() == "ContactAus")
        {
            //newOrgAddressRow6.Visible = true;
            DataTable addrChannelTypes = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "AddressChannelType", "", "descr", "address_channel_type_id", "descr");
            ddlAddressAddressChannelType.Items.Add(new ListItem("--", "-1"));
            foreach (DataRow row in addrChannelTypes.Rows)
                ddlAddressAddressChannelType.Items.Add(new ListItem(row["descr"].ToString(), row["address_channel_type_id"].ToString()));
        }
        else
            throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());

        DataTable countries = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "Country", "", "descr", "country_id", "descr");
        ddlAddressCountry.Items.Add(new ListItem("--", "-1"));
        foreach (DataRow row in countries.Rows)
            ddlAddressCountry.Items.Add(new ListItem(row["descr"].ToString(), row["country_id"].ToString()));
        ddlAddressCountry.SelectedIndex = Utilities.IndexOf(ddlAddressCountry, "australia");


        bool editable = true;
        Utilities.SetEditControlBackColour(ddlAddressContactType,        editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtAddressAddrLine1,          editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtAddressAddrLine2,          editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlAddressAddressChannel,     editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtStreet,                    editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlAddressAddressChannelType, editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlAddressCountry,            editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtAddressFreeText,           editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);

        Utilities.SetEditControlBackColour(ddlPhoneNumber1,              editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(ddlPhoneNumber2,              editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtPhoneNumber1,              editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtPhoneNumber2,              editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtPhoneNumber1FreeText,      editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtPhoneNumber2FreeText,      editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);

        Utilities.SetEditControlBackColour(ddlEmailContactType,          editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
        Utilities.SetEditControlBackColour(txtEmailAddrLine1,            editable, System.Drawing.Color.LightGoldenrodYellow, System.Drawing.Color.Empty);
    }

    protected void btnUpdateAddressStreetAndSuburb_Click(object sender, EventArgs e)
    {
        DataTable addrChannels = AddressChannelDB.GetDataTable();
        ddlAddressAddressChannel.Items.Clear();
        ddlAddressAddressChannel.Items.Add(new ListItem("--", "-1"));
        foreach (DataRow row in addrChannels.Rows)
            ddlAddressAddressChannel.Items.Add(new ListItem(row["ac_descr"] + " " + row["act_descr"], row["ac_address_channel_id"].ToString()));
        //ddlAddressChannel.SelectedValue = thisRow["ad_address_channel_id"] == DBNull.Value ? "-1" : thisRow["ad_address_channel_id"].ToString();
    }

    protected void btnUpdateAddressType_Click(object sender, EventArgs e)
    {
        ddlAddressContactType.DataSource = ContactTypeDB.GetDataTable(1);
        ddlAddressContactType.DataBind();
        // ddlAddressContactType.SelectedValue = thisRow["ad_contact_type_id"].ToString();
    }

    protected void btnUpdatePhoneType_Click(object sender, EventArgs e)
    {
        DataTable dt_PhoneNumbers = ContactTypeDB.GetDataTable(2);
        ddlPhoneNumber1.DataSource = dt_PhoneNumbers.Copy();
        ddlPhoneNumber2.DataSource = dt_PhoneNumbers.Copy();
        ddlPhoneNumber1.DataBind();
        ddlPhoneNumber2.DataBind();
        ddlPhoneNumber1.SelectedValue = "30"; // mobile
        ddlPhoneNumber2.SelectedValue = "34"; // office
    }

    protected void btnUpdateEmailType_Click(object sender, EventArgs e)
    {
        ddlEmailContactType.DataSource = ContactTypeDB.GetDataTable(4);
        ddlEmailContactType.DataBind();
        // ddlEmailContactType.SelectedValue = thisRow["ad_contact_type_id"].ToString();
    }

    #endregion



    private void FillEmptyForm()
    {
        if (!Utilities.IsDev())
            idRow.Attributes["class"] = "hiddencol";

        DataTable dt_titles = DBBase.GetGenericDataTable_WithWhereOrderClause(null, "Title", "", " descr ", "title_id", "descr");
        for (int i = dt_titles.Rows.Count - 1; i >= 0; i--)
        {
            if (Convert.ToInt32(dt_titles.Rows[i]["title_id"]) == 0)
            {
                DataRow newRow = dt_titles.NewRow();
                newRow.ItemArray = dt_titles.Rows[i].ItemArray;
                dt_titles.Rows.RemoveAt(i);
                dt_titles.Rows.InsertAt(newRow, 0);
                break;
            }
        }
        ddlTitle.DataSource = dt_titles;
        ddlTitle.DataBind();
        ddlTitle.SelectedIndex = Utilities.IndexOf(ddlTitle, "dr", "dr.");

        DataTable orgs = OrganisationDB.GetDataTable_ByType(new int[] { 191 });
        orgs.DefaultView.Sort = "name ASC";
        foreach (DataRowView row in orgs.DefaultView)
            ddlOrgsList.Items.Add(new ListItem(row["name"].ToString(), row["organisation_id"].ToString()));

        //if (orgs.Rows.Count > 0)
        //    UpdateReferrersAtOrgList();


        if (orgs.Rows.Count > 0)
        {
            orgsListRow.Visible        = true;

            newOrgTitleRow.Visible     = false;
            newOrgNameRow.Visible      = false;
            newOrgABNRow.Visible       = false;
            newOrgACNRow.Visible       = false;
            newOrgCommentsRow.Visible  = false;
            newOrgSepeatorTrailingRow.Visible = false;
            newOrgAddressRow1.Visible  = false;
            newOrgAddressRow2.Visible  = false;
            newOrgAddressRow3.Visible  = false;
            newOrgAddressRow4.Visible  = false;
            newOrgAddressRow5.Visible  = false;
            newOrgAddressRow6.Visible  = false;
            newOrgAddressRow7.Visible  = false;
            newOrgAddressRow8.Visible  = false;
            newOrgAddressRow9.Visible  = false;
            newOrgAddressRow10.Visible = false;
            newOrgContactRow1.Visible  = false;
            newOrgContactRow2.Visible  = false;
            newOrgContactRow3.Visible  = false;
            newOrgContactTitleRow.Visible = false;
        }
        else
        {
            SetAddNewOrgSection();
        }


        btnSubmit.Text = "Add Referrer";

        SetupAddressGUI();
    }

    protected void ddlOrgsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateReferrersAtOrgList();
    }

    protected void UpdateReferrersAtOrgList()
    {
        if (ddlOrgsList.SelectedIndex == -1)
            return;

        lblProvidersOf.Visible = true;

        lblSelectedOrg.Text = ddlOrgsList.Items[ddlOrgsList.SelectedIndex].Text;

        DataTable tbl = RegisterReferrerDB.GetDataTable_ReferrersOf(Convert.ToInt32(ddlOrgsList.Items[ddlOrgsList.SelectedIndex].Value));
        RegisterReferrer[] regRefs = new RegisterReferrer[tbl.Rows.Count];
        for (int i = 0; i < tbl.Rows.Count; i++)
        {
            regRefs[i]                       = RegisterReferrerDB.Load(tbl.Rows[i]);
            regRefs[i].Referrer              = ReferrerDB.Load(tbl.Rows[i]);
            regRefs[i].Referrer.Person       = PersonDB.Load(tbl.Rows[i], "", "person_entity_id");
            regRefs[i].Referrer.Person.Title = IDandDescrDB.Load(tbl.Rows[i], "title_id", "descr");
        }
        string list = tbl.Rows.Count == 0 ? string.Empty : @"<table cellpadding=""0"" cellspacing=""0"">";
        foreach (RegisterReferrer regRef in regRefs)
        {
            string url = "ReferrerList_DoctorClinicV2.aspx?surname_search=" + regRef.Referrer.Person.Surname + @"&surname_starts_with=1" + (regRef.ProviderNumber.Length > 0 ? "&provider_nbr_search=" + regRef.ProviderNumber + "&provider_nbr_starts_with=1" : "");
            string href = @"<a href=""" + url + @""">" + regRef.Referrer.Person.FullnameWithoutMiddlename + "</a>";
            list += "<tr><td>" + href + @"</td><td style=""width:8px;"">&nbsp;</td><td> Prov Nbr: " + (regRef.ProviderNumber.Trim().Length == 0 ? "[None]" : regRef.ProviderNumber) + "</td></tr>";
        }
        list += tbl.Rows.Count == 0 ? string.Empty : "</table>";

        lblProvidersOfSelectedOrg.Text = list.Length > 0 ? list : "None";
    }

    protected void btnSetExistingReferrer_Click(object sender, EventArgs e)
    {
        Referrer referrer = ReferrerDB.GetByID(Convert.ToInt32(jsSetId.Value));

        if (referrer == null)
        {
            lblId.Text = "-1";
            SetErrorMessage("Error getting selected referrer. Plase contact the system administrator.<br />");
        }
        else
        {
            lblId.Text = referrer.ReferrerID.ToString();
            ddlTitle.SelectedValue = referrer.Person.Title.ID.ToString();
            txtFirstname.Text = referrer.Person.Firstname;
            txtMiddlename.Text = referrer.Person.Middlename;
            txtSurname.Text = referrer.Person.Surname;
            if (ddlGender.Items.FindByValue(referrer.Person.Gender) == null)
                ddlGender.Items.Add(new ListItem(referrer.Person.Gender == "" ? "--" : referrer.Person.Gender, referrer.Person.Gender));
            ddlGender.SelectedValue = referrer.Person.Gender;

            ddlTitle.Enabled = false;
            txtFirstname.Enabled = false;
            txtMiddlename.Enabled = false;
            txtSurname.Enabled = false;
            ddlGender.Enabled = false;
        }
    }

    protected void lnkAddNewOrg_Click(object sender, EventArgs e)
    {
        SetAddNewOrgSection();
    }
    protected void SetAddNewOrgSection()
    {
        orgsListRow.Visible = false;
        pnlReferrersOfSelectedOrg.Visible = false;
        td_ReferrersOfSelectedOrg.RowSpan -= 2;

        newOrgSepeatorRow.Visible  = false;
        newOrgTitleRow.Visible     = true;
        newOrgNameRow.Visible      = true;
        newOrgABNRow.Visible       = true;
        newOrgACNRow.Visible       = true;
        newOrgCommentsRow.Visible  = true;
        newOrgSepeatorTrailingRow.Visible = true;
        newOrgAddressRow1.Visible  = true;
        newOrgAddressRow2.Visible  = true;
        newOrgAddressRow3.Visible  = true;
        newOrgAddressRow4.Visible  = true;

        if (Utilities.GetAddressType().ToString() == "Contact")
            newOrgAddressRow5.Visible = true;
        else if (Utilities.GetAddressType().ToString() == "ContactAus")
            newOrgAddressRow6.Visible = true;

        newOrgAddressRow7.Visible  = true;
        newOrgAddressRow8.Visible  = true;
        newOrgAddressRow9.Visible  = true;
        newOrgAddressRow10.Visible = true;
        newOrgContactRow1.Visible  = true;
        newOrgContactRow2.Visible  = true;
        newOrgContactRow3.Visible  = true;
        newOrgContactTitleRow.Visible = true;
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // need to be able to roll back .. so keep id's same as invoice

        int person_id   = -1;
        int referrer_id = -1;
        int new_org_id  = 0;
        bool referrer_added = false;

        int address_id = -1;
        int phone_id1 = -1;
        int phone_id2 = -1;
        int email_id = -1;
        bool contacts_added = false;

        try
        {
            // add referrer
            if (lblId.Text == "-1") // add new referrer
            {
                Staff loggedInStaff = StaffDB.GetByID(Convert.ToInt32(Session["StaffID"]));
                person_id = PersonDB.Insert(loggedInStaff.Person.PersonID, Convert.ToInt32(ddlTitle.SelectedValue), Utilities.FormatName(txtFirstname.Text), Utilities.FormatName(txtMiddlename.Text), Utilities.FormatName(txtSurname.Text), "", ddlGender.SelectedValue, new DateTime(1900, 1, 1));
                referrer_id = ReferrerDB.Insert(person_id);
            }
            else  // set existing referrer
            {
                referrer_id = Convert.ToInt32(lblId.Text);
            }


            // get org (or add new org)
            int org_id = 0;
            if (orgsListRow.Visible)
                org_id = Convert.ToInt32(ddlOrgsList.SelectedValue);
            else
            {
                org_id = new_org_id = OrganisationDB.InsertExtOrg(191, txtOrgName.Text, txtOrgACN.Text, txtOrgABN.Text, false, false, "", txtOrgComments.Text);

                // add contact info

                Organisation org = OrganisationDB.GetByID(org_id);

                if (Utilities.GetAddressType().ToString() == "Contact")
                {
                    if (txtAddressAddrLine1.Text.Trim().Length > 0 || txtAddressAddrLine2.Text.Trim().Length > 0)
                        address_id = ContactDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlAddressContactType.SelectedValue),
                            txtAddressFreeText.Text,
                            txtAddressAddrLine1.Text,
                            txtAddressAddrLine2.Text,
                            Convert.ToInt32(ddlAddressAddressChannel.SelectedValue),
                            //Convert.ToInt32(ddlAddressSuburb.SelectedValue),
                            Convert.ToInt32(suburbID.Value),
                            Convert.ToInt32(ddlAddressCountry.SelectedValue),
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtPhoneNumber1.Text.Trim().Length > 0)
                        phone_id1 = ContactDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlPhoneNumber1.SelectedValue),
                            txtPhoneNumber1FreeText.Text,
                            System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber1.Text, "[^0-9]", ""),
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtPhoneNumber2.Text.Trim().Length > 0)
                        phone_id2 = ContactDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlPhoneNumber2.SelectedValue),
                            txtPhoneNumber2FreeText.Text,
                            System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber2.Text, "[^0-9]", ""),
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtEmailAddrLine1.Text.Trim().Length > 0)
                        email_id = ContactDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlEmailContactType.SelectedValue),
                            "",
                            txtEmailAddrLine1.Text,
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);
                }
                else if (Utilities.GetAddressType().ToString() == "ContactAus")
                {
                    if (txtAddressAddrLine1.Text.Trim().Length > 0 || txtAddressAddrLine2.Text.Trim().Length > 0)
                        address_id = ContactAusDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlAddressContactType.SelectedValue),
                            txtAddressFreeText.Text,
                            txtAddressAddrLine1.Text,
                            txtAddressAddrLine2.Text,
                            txtStreet.Text,
                            Convert.ToInt32(ddlAddressAddressChannelType.SelectedValue),
                            //Convert.ToInt32(ddlAddressSuburb.SelctedValue),
                            Convert.ToInt32(suburbID.Value),
                            Convert.ToInt32(ddlAddressCountry.SelectedValue),
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtPhoneNumber1.Text.Trim().Length > 0)
                        phone_id1 = ContactAusDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlPhoneNumber1.SelectedValue),
                            txtPhoneNumber1FreeText.Text,
                            System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber1.Text, "[^0-9]", ""),
                            string.Empty,
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtPhoneNumber2.Text.Trim().Length > 0)
                        phone_id2 = ContactAusDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlPhoneNumber2.SelectedValue),
                            txtPhoneNumber2FreeText.Text,
                            System.Text.RegularExpressions.Regex.Replace(txtPhoneNumber2.Text, "[^0-9]", ""),
                            string.Empty,
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);

                    if (txtEmailAddrLine1.Text.Trim().Length > 0)
                        email_id = ContactAusDB.Insert(org.EntityID,
                            Convert.ToInt32(ddlEmailContactType.SelectedValue),
                            "",
                            txtEmailAddrLine1.Text,
                            string.Empty,
                            string.Empty,
                            -1,
                            -1,
                            -1,
                            Convert.ToInt32(Session["SiteID"]),
                            true,
                            true);
                }
                else
                {
                    throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());
                }

            }

            contacts_added = true;

            // join association
            RegisterReferrerDB.Insert(org_id, referrer_id, txtProviderNumber.Text, chkIsReportEveryVisit.Checked, chkIsBatchSendAllPatientsTreatmentNotes.Checked);
            referrer_added = true;


            if (GetUrlIsPopup())
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language=javascript>window.returnValue=false;self.close();</script>");
            }
            else
            {
                Response.Redirect("~/ReferrerList_DoctorClinicV2.aspx?surname_search=" + Utilities.FormatName(txtSurname.Text) + "&surname_starts_with=1", false);
                return;
            }
        }
        catch (Exception)
        {
            // roll back - backwards of creation order

            if (Utilities.GetAddressType().ToString() == "Contact")
            {
                ContactDB.Delete(address_id);
                ContactDB.Delete(phone_id1);
                ContactDB.Delete(phone_id2);
                ContactDB.Delete(email_id);
            }
            else if (Utilities.GetAddressType().ToString() == "ContactAus")
            {
                ContactAusDB.Delete(address_id);
                ContactAusDB.Delete(phone_id1);
                ContactAusDB.Delete(phone_id2);
                ContactAusDB.Delete(email_id);
            }
            else
                throw new Exception("Unknown AddressType in config: " + Utilities.GetAddressType().ToString().ToString());

            OrganisationDB.Delete(new_org_id);
            ReferrerDB.Delete(referrer_id);
            PersonDB.Delete(person_id);

            throw;
        }
    }


    #region btnSuburbSelectionUpdate_Click

    protected void btnSuburbSelectionUpdate_Click(object sender, EventArgs e)
    {
        UpdateSuburbInfo(true);
    }

    protected void UpdateSuburbInfo(bool redirect)
    {
        return;

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
            url = UrlParamModifier.Update(newSuburbID != -1, url, "suburb", newSuburbID == -1 ? "" : newSuburbID.ToString());
            Response.Redirect(url);
        }
    }

    #endregion


    #region HideTableAndSetErrorMessage, SetErrorMessage, HideErrorMessag

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