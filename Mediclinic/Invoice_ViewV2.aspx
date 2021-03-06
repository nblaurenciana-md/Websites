﻿<%@ Page Title="View Invoice" Language="C#" MasterPageFile="~/SiteV2.master" AutoEventWireup="true" CodeFile="Invoice_ViewV2.aspx.cs" Inherits="Invoice_ViewV2" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">

        function show_hide(id) {

            obj = document.getElementById(id);
            obj.style.display = (obj.style.display == "none") ? "" : "none";
        }
        function show_hide_byname(name) {

            obj = document.getElementsByName(name)[0];
            obj.style.display = (obj.style.display == "none") ? "" : "none";
        }

        function show_hide_booking_info() {
            var show = document.getElementById('booking_offering').style.display == "none" &&
                document.getElementById('booking_patient').style.display == "none" &&
                document.getElementById('booking_provider').style.display == "none" &&
                document.getElementById('booking_org').style.display == "none" &&
                document.getElementById('booking_status').style.display == "none" &&
                document.getElementById('booking_apptmt_time').style.display == "none" &&
                document.getElementById('booking_patiemt_missed_apptmt').style.display == "none" &&
                document.getElementById('booking_provider_missed_apptmt').style.display == "none" &&
                document.getElementById('booking_isemergency').style.display == "none" &&
                document.getElementById('booking_notes').style.display == "none";

            document.getElementById('booking_offering').style.display = show ? "" : "none";
            document.getElementById('booking_patient').style.display = show ? "" : "none";
            document.getElementById('booking_provider').style.display = show ? "" : "none";
            document.getElementById('booking_org').style.display = show ? "" : "none";
            document.getElementById('booking_status').style.display = show ? "" : "none";
            document.getElementById('booking_apptmt_time').style.display = show ? "" : "none";
            document.getElementById('booking_patiemt_missed_apptmt').style.display = show ? "" : "none";
            document.getElementById('booking_provider_missed_apptmt').style.display = show ? "" : "none";
            document.getElementById('booking_isemergency').style.display = show ? "" : "none";
            document.getElementById('booking_notes').style.display = show ? "" : "none";
        }


        function toggleVis(elemName, view) {
            cells = document.getElementsByName(elemName);
            for (j = 0; j < cells.length; j++)
                cells[j].style.display = view ? '' : 'none';
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="clearfix">
        <div class="page_title"><asp:Label ID="lblHeading" runat="server">View Booking Invoice</asp:Label></div>
        <div class="main_content" style="padding:10px 5px;">
            <div class="user_login_form" style="width: 400px;">

                <div class="border_top_bottom" id="divToggleShowReversedRejected" runat="server">

                    <center>
                        <table>
                            <tr>
                                <td style="text-align:left;">
                                    <input type='checkbox' id="chkShowReversed" runat="server" onclick="toggleVis('td_reversed', this.checked)"/>&nbsp;<label for="chkShowReversed">Show/Hide Reversed</label><br />
                                    <input type='checkbox' id="chkShowRejected" runat="server" onclick="toggleVis('td_rejected', this.checked)"/>&nbsp;<label for="chkShowRejected">Show/Hide Medicare/DVA Rejected</label><br />

                                </td>
                            </tr>
                        </table>
                    </center>
                </div>

            </div>


            <div class="text-center">
                <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="ValidationSummary"/>
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" CssClass="failureNotification"></asp:Label>
            </div>


            <center>

                <div id="autodivheight" class="divautoheight" style="height:500px; width: auto; padding-right: 17px;">

                    <table ID="maintable" runat="server">
                        <tr style="vertical-align:top;">
                            <td style="vertical-align:top;">
                                <table>

                                    <!------------->
                                    <!--- START --->
                                    <!------------->

                                    <tr>
                                        <td class="nowrap"><u>Invoice Information</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater1" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" class="nowrap">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDate" runat="server" Text='<%#  Eval("message_reversed_wiped") %>' Font-Bold="True" />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="min-width:25px;display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>

                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td class="nowrap">Invoice #</td>
                                        <td style="min-width:30px;"></td>

                                        <asp:Repeater id="Repeater2" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblId" runat="server" Text='<%#  Eval("inv_invoice_id") %>' />&nbsp;&nbsp;<asp:LinkButton ID="lnkPrint" runat="server" Text="Print" OnCommand="lnkPrint_Command" CommandArgument='<%#  Eval("inv_invoice_id") %>' />&nbsp;&nbsp;<asp:LinkButton ID="lnkEmail" runat="server" Text="Email" OnCommand="lnkEmail_Command" CommandArgument='<%#  Eval("inv_invoice_id") %>' /><asp:Label id="lblLastEmailed" runat="server" Text='<%#  Eval("inv_last_date_emailed") == DBNull.Value ? "" : ( "&nbsp;&nbsp;(Inv Last Emailed: " + Eval("inv_invoice_date_added", "{0:dd MMM yyyy}") + ")" ) %>' ></asp:Label>
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" style="min-width:30px;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Date</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater3" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" class="nowrap">
                                                    <asp:Label ID="lblDate" runat="server" Text='<%#  Eval("inv_invoice_date_added", "{0:dd MMM yyyy}") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Debtor</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater4" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblDebtor" runat="server" Text='<%#  Eval("inv_debtor") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                        <td><asp:Label ID="lblDebtor" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Claim No.</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater5" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblClaimNumber" runat="server" Text='<%#  Eval("inv_healthcare_claim_number") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr style="height:10px">
                                        <td colspan="100%"></td>
                                    </tr>
                                    <tr>
                                        <td>Total</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater6" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblTotal" runat="server" Text='<%#  Eval("inv_total") + (Eval("inv_gst") == DBNull.Value || Convert.ToDecimal(Eval("inv_gst")) == 0 ? "" : "&nbsp;&nbsp; (<i>Inc GST: " + Convert.ToDecimal(Eval("inv_gst")) + "</i>)") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td class="nowrap">Total Payments</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater7" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblReceipts" runat="server" Text='<%#  Eval("inv_receipts_total") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <tr>
                                        <td class="nowrap">Total Vouchers</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater18" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblVouchers" runat="server" Text='<%#  Eval("inv_vouchers_total") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                    <tr>
                                        <td>Total Credit Noted</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater8" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblCreditNotes" runat="server" Text='<%#  Eval("inv_credit_notes_total") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Total Due</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater9" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblTotalDue" runat="server" Font-Bold="true" Text='<%#  Eval("inv_total_due") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr style="height:10px">
                                        <td colspan="100%"></td>
                                    </tr>
                                    <tr>
                                        <td>Paid</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater10" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblPaid" runat="server" Text='<%#  (bool)Eval("inv_is_paid") ? "Yes" : "No" %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Refund</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater11" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblRefund" runat="server" Text='<%#  (bool)Eval("inv_is_refund") ? "Yes" : "No" %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Batched Invoice</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater12" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;">
                                                    <asp:Label ID="lblBatchedInvoice" runat="server" Text='<%#  (bool)Eval("inv_is_batched") ? "Yes" : "No" %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>
                                    <tr>
                                        <td>Added By</td>
                                        <td></td>

                                        <asp:Repeater id="Repeater13" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" class="nowrap">
                                                    <asp:Label ID="lblStaff" runat="server" Text='<%#  Eval("staff_person_firstname") + " " + Eval("staff_person_surname") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>

                                    <tr style="height:12px">
                                        <td colspan="100%"></td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align:top;"  class="nowrap"><u>Invoice Items</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater14" runat="server">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="vertical-align:top;display:<%# Eval("style_display") %>;" class="nowrap">
                                                    <asp:Label ID="lstInvLines" runat="server" Text='<%#  Eval("inv_lines_text") %>' />
                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>

                                    <tr style="height:16px">
                                        <td colspan="100%"></td>
                                    </tr>


                                    <tr>
                                        <td style="vertical-align:top;" class="nowrap"><u>Payments</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater15" runat="server" OnItemCreated="Repeater15_ItemCreated">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" style="vertical-align:top;" class="nowrap">

                                                    <div id="span_add_receipts_row" runat="server">
                                                        <asp:Label  ID="lnkAddReceipt" Text="Add Payment" runat="server" Width="140px" />
                                                        <asp:LinkButton ID="showHideReceiptsList" runat="server">Show/Hide</asp:LinkButton>
                                                    </div>

                                                    <div id="div_receipts_list" runat="server" style="display:none;">
                                                    <div id="span_receipts_trailing_space_row" runat="server" style="line-height:4px;">&nbsp;</div>
                                                    <asp:Panel ID="pnlReceipts" runat="server" ScrollBars="Auto" style="max-height:85px;overflow-y:auto;display:inline-block;padding-right:17px;">
                                                        <asp:Repeater id="lstReceipts" runat="server" OnItemCreated="lstReceipts_ItemCreated">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-grid table-grid-top-bottum-padding-normal auto_width">
                                                                    <tr>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;" class="nowrap">Receipt #</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Date</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;" class="nowrap">Type</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Total</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Reconciled</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;"></th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lnkViewReceipt" runat="server" /> <asp:Label ID="lblPaidBy" runat="server" /></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblReceiptDate" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblPaymentType" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblReceiptTotal" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblReceiptAmountReconciled" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;">
                                                                        <asp:Label ID="lblStatus" runat="server" />
                                                                        <asp:Label ID="lnkReconcile" runat="server"  />
                                                                        <asp:LinkButton ID="lnkReverse" runat="server" Text="Reverse" OnCommand="ReverseReceipt_Command"  />
                                                                        <asp:HiddenField ID="lblHiddenReceiptID" runat="server"></asp:HiddenField>

                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>           
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </asp:Panel>
                                                    </div>

                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>

                                    <tr style="height:2px">
                                        <td colspan="100%"></td>
                                    </tr>


                                    <tr>
                                        <td style="vertical-align:top;" class="nowrap"><u>Vouchers</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater19" runat="server" OnItemCreated="Repeater19_ItemCreated">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" style="vertical-align:top;" class="nowrap">

                                                    <div id="span_add_vouchers_row" runat="server">
                                                        <asp:Label  ID="lnkAddVoucher" Text="Add Voucher Use" runat="server" Width="140px" />
                                                        <asp:LinkButton ID="showHideVouchersList" runat="server">Show/Hide</asp:LinkButton>
                                                    </div>

                                                    <div id="div_vouchers_list" runat="server" style="display:none;">
                                                    <div id="span_vouchers_trailing_space_row" runat="server" style="line-height:4px;">&nbsp;</div>
                                                    <asp:Panel ID="pnlVouchers" runat="server" ScrollBars="Auto" style="max-height:85px;overflow-y:auto;display:inline-block;padding-right:17px;">
                                                        <asp:Repeater id="lstVouchers" runat="server" OnItemCreated="lstVouchers_ItemCreated">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-grid table-grid-top-bottum-padding-normal auto_width">
                                                                    <tr>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;" class="nowrap">Voucher Used</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Date</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Total</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;"></th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lnkViewVoucher" runat="server" /> <asp:Label ID="lblPaidBy" runat="server" /></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblVoucherDate" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblVoucherTotal" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;">
                                                                        <asp:Label ID="lblStatus" runat="server" />
                                                                        <asp:Label ID="lnkReconcile" runat="server"  />
                                                                        <asp:LinkButton ID="lnkReverse" runat="server" Text="Reverse" OnCommand="ReverseVoucher_Command"  />
                                                                        <asp:HiddenField ID="lblHiddenVoucherID" runat="server"></asp:HiddenField>

                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>           
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </asp:Panel>
                                                    </div>

                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>


                                    <tr style="height:2px">
                                        <td colspan="100%"></td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align:top;"  class="nowrap"><u>Adjustment Notes</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater16" runat="server" OnItemCreated="Repeater16_ItemCreated">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" style="vertical-align:top;" class="nowrap">

                                                    <div id="span_add_credit_notes_row" runat="server">
                                                        <asp:Label  ID="lnkAddCreditNote" Text="Add Payment" runat="server" Width="140px" />
                                                        <asp:LinkButton ID="showHideCreditNoteList" runat="server">Show/Hide</asp:LinkButton>
                                                    </div>

                                                    <div id="div_credit_notes_list" runat="server" style="display:none;">
                                                    <div id="span_credit_notes_trailing_space_row" runat="server" style="line-height:4px;">&nbsp;</div>


                                                    <asp:Panel ID="pnlCreditNotes" runat="server" ScrollBars="Auto" style="max-height:85px;overflow-y:auto;display:inline-block;padding-right:17px;">
                                                        <asp:Repeater id="lstCreditNotes" runat="server" OnItemCreated="lstCreditNotes_ItemCommand">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-grid table-grid-top-bottum-padding-normal auto_width">
                                                                    <tr>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;" class="nowrap">Adj Note #</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Date</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Total</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;"></th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lnkViewCreditNote" runat="server" /></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblCreditNoteDate" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblCreditNoteTotal" runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;" id="tdStatusColumn" runat="server">
                                                                        <asp:Label ID="lblStatus" Text="Reversed" runat="server" />
                                                                        <asp:LinkButton ID="lnkReverse" OnCommand="ReverseCreditNote_Command" runat="server" Text="Reverse" OnClientClick="javascript:if (!confirm('Are you sure you want to reverse this record?')) return false;" />
                                                                        <asp:HiddenField ID="lblHiddenCreditNoteID" runat="server"></asp:HiddenField>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>           
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </asp:Panel>
                                                    </div>

                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tr>

                                    <tr style="height:2px">
                                        <td colspan="100%"></td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align:top;" class="nowrap"><u>Refunds</u></td>
                                        <td></td>

                                        <asp:Repeater id="Repeater17" runat="server" OnItemCreated="Repeater17_ItemCreated">
                                            <ItemTemplate>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;" style="vertical-align:top;" class="nowrap">



                                                    <div id="span_add_refunds_row" runat="server">
                                                        <asp:Label ID="lnkAddRefund" Text="Add Refund" runat="server" Width="140px" />
                                                        <asp:LinkButton ID="showHideRefundsList" runat="server">Show/Hide</asp:LinkButton>
                                                    </div>

                                                    <div id="div_refunds_list" runat="server" style="display:none;">
                                                    <div id="span_refunds_trailing_space_row" runat="server" style="line-height:4px;">&nbsp;</div>
                                                    <asp:Panel ID="pnlRefunds" runat="server" ScrollBars="Auto" style="max-height:85px;overflow-y:auto;display:inline-block;padding-right:17px;">
                                                        <asp:Repeater id="lstRefunds" runat="server" OnItemCreated="lstRefunds_ItemCreated">
                                                            <HeaderTemplate>
                                                                <table class="table table-bordered table-striped table-grid table-grid-top-bottum-padding-normal auto_width">
                                                                    <tr>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;" class="nowrap">Refund #</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Date</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Total</th>
                                                                        <th style="padding-left:3px !important;padding-right:3px !important;">Reason</th>
                                                                    </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lnkViewRefund"   runat="server" /></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblRefundDate"   runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblRefundTotal"  runat="server"></asp:Label></td>
                                                                    <td style="padding-left:3px !important;padding-right:3px !important;"><asp:Label ID="lblRefundReason" runat="server"></asp:Label></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </table>           
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </asp:Panel>
                                                    </div>

                                                </td>
                                                <td name='<%# Eval("td_name") %>' style="display:<%# Eval("style_display") %>;"></td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>

                                    <!------------->
                                    <!---  END  --->
                                    <!------------->





                                    <tr id="booking_space" runat="server" height="20">
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr id="booking_title" runat="server">
                                        <td class="nowrap"><u>Booking Information</u></td>
                                        <td></td>
                                        <td><asp:LinkButton ID="showHideBookingInfo" runat="server" OnClientClick="javascript:show_hide_booking_info(); return false;">Show/Hide</asp:LinkButton></td>
                                    </tr>
                                    <tr id="booking_offering" runat="server" style="display:none;">
                                        <td>Service</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Offering" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_patient" runat="server" style="display:none;">
                                        <td>Patient</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Patient" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_provider" runat="server" style="display:none;">
                                        <td>Provider</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Provider" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_org" runat="server" style="display:none;">
                                        <td>Organisation</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Org" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_status" runat="server" style="display:none;">
                                        <td>Status</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_BookingStatus" runat="server"></asp:Label></td>
                                    </tr>

                                    <tr id="booking_apptmt_time" runat="server" style="display:none;">
                                        <td>Appointment Time</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Time" runat="server"></asp:Label></td>
                                    </tr>

                                    <tr id="booking_patiemt_missed_apptmt" runat="server" style="display:none;">
                                        <td>Patient Missed Appt</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_PatientMissedAppt" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_provider_missed_apptmt" runat="server" style="display:none;">
                                        <td>Prov. Missed Appt</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_ProviderMissedAppt" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_isemergency" runat="server" style="display:none;">
                                        <td>Emergency</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Emergency" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr id="booking_notes" runat="server" style="display:none;">
                                        <td>Booking notes</td>
                                        <td></td>
                                        <td><asp:Label ID="lblBooking_Notes" runat="server"></asp:Label></td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>

                </div>

                <table>


                </table>


                <br />
                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.returnValue=false;self.close();" />

            </center>

        </div>
    </div>


</asp:Content>



