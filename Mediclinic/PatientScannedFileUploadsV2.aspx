﻿<%@ Page Title="Scanned Files" Language="C#" MasterPageFile="~/SiteV2.master" AutoEventWireup="true" CodeFile="PatientScannedFileUploadsV2.aspx.cs" Inherits="PatientScannedFileUploadsV2" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <!--// plugin-specific resources //-->
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/jquery.MetaData.js" type="text/javascript"></script>
    <script src="Scripts/jquery.MultiFile.js" type="text/javascript"></script>
    <script src="Scripts/jquery.blockUI.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="clearfix">
        <div id="page_title" runat="server" class="page_title"><asp:Label ID="lblHeading" runat="server">Scanned Documents</asp:Label> &nbsp; <asp:HyperLink ID="lnkToEntity" runat="server"></asp:HyperLink></div>
        <div class="main_content" style="padding:20px 5px;">

             <center>

                <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="ValidationSummary"/>
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" CssClass="failureNotification"></asp:Label>

                <table id="spn_manage_files" runat="server" border="0" cellpadding="0" cellspacing="0">

                    <tr style="vertical-align:top;">
                        <td>
                    
                            <center>
                                <asp:Label ID="lblCurrentPath" runat="server"></asp:Label>
                            </center>

                            <asp:GridView ID="GrdScannedDoc" runat="server" 
                                    AutoGenerateColumns="False"
                                    OnRowCancelingEdit="GrdScannedDoc_RowCancelingEdit" 
                                    OnRowDataBound="GrdScannedDoc_RowDataBound" 
                                    OnRowEditing="GrdScannedDoc_RowEditing" 
                                    OnRowUpdating="GrdScannedDoc_RowUpdating" ShowFooter="FALSE" 
                                    OnRowCommand="GrdScannedDoc_RowCommand" 
                                    OnRowDeleting="GrdScannedDoc_RowDeleting" 
                                    OnRowCreated="GrdScannedDoc_RowCreated"
                                    AllowSorting="False" 
                                    GridLines="Both"
                                    OnSorting="GrdScannedDoc_Sorting"
                                    RowStyle-VerticalAlign="top"
                                    ClientIDMode="Predictable"
                                    CssClass="table table-bordered table-striped table-grid table-grid-top-bottum-padding-normal auto_width block_center vertical_align_top">
                                <Columns>

                                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="text_left">
                                        <ItemTemplate>

                                            <asp:LinkButton runat="server" ID="lbFolderItem" CommandName="OpenFolder" CommandArgument='<%# Eval("FullName") %>'></asp:LinkButton>
                                            <asp:Literal runat="server" ID="ltlFileItem"></asp:Literal>

                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Date Created" SortExpression="CreationTime">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateCreated" runat="server" Text='<%# Eval("CreationTime", "{0:dd MMM, yyyy}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Size" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# DisplaySize((long?) Eval("Size")) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name" SortExpression="Name" ItemStyle-CssClass="text_left">
                                        <ItemTemplate>
                                                <asp:LinkButton ID="lnkFileDownload" runat="server" Visible='<%# (string)Eval("Name") != ".." && (bool)Eval("IsFolder") == false %>' CommandArgument='<%# Eval("FullName") %>' OnClick="btnDownload_Click" Text='download' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" Visible='<%# Eval("Name") != ".." %>' ImageUrl="~/images/Delete-icon-16.png" CommandArgument='<%# Eval("FullName") %>' OnClick="btnDeleteFie_Click" OnClientClick="javascript:if (!confirm('Are you sure you want to permanently delete this file?')) return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>

                        </td>
                    </tr>
                    <tr>
                        <td>

                            <center>
                                <div style="height:8px;"></div>
                                <table class="block_center">
                                    <tr style="vertical-align:top;">
                                        <td>
                                            <center>

                                                <strong>Upload Files</strong>
                                                <div style="line-height:10px;">&nbsp;</div>

                                                <asp:FileUpload ID="FileUpload1" runat="server" class="multi" style="color:transparent;width:85px;" accept="bmp|dcs|doc|docm|docx|dot|dotm|dotx|gz|gif|htm|html|ipg|jpe|jpeg|jpg|mp3|mp4|wav|pdf|png|ppa|ppam|pps|ppsm|ppt|pptm|tar|tgz|tif|tiff|txt|xla|xlam|xlc|xld|xlk|xll|xlm|xls|xlsx|xlt|xltm|xltx|xlw|xml|z|zip|7z"  />
                                                <div style="height:12px;"></div>
                                                <asp:CheckBox ID="chkAllowOverwrite" runat="server" Font-Size="Small" ForeColor="GrayText" Text="&nbsp;Allow File Overwrite" />

                                                <center>
                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload All" OnClick="btnUpload_Click" />
                                                </center>
                                                <div style="height:15px;"></div>
                                                <asp:Label ID="lblUploadMessage" runat="server"></asp:Label>

                                            </center>
                                        </td>
                                        <td style="min-width:30px;"></td>
                                        <td>
                                            <center>

                                                <strong>Create Directory</strong>
                                                <div style="line-height:10px;">&nbsp;</div>

                                                <asp:Panel ID="pnlNewDirectory" runat="server" DefaultButton="btnAddDirectory">
                                                    <asp:Textbox ID="txtNewDirectory" runat="server" style="width:125px;min-width:100%;"></asp:Textbox>
                                                    <div style="line-height:2px;">&nbsp;</div>
                                                    <asp:Button ID="btnAddDirectory" runat="server" style="width:125px;min-width:100%;" Text="Create Directory" OnClick="btnAddDirectory_Click" />&nbsp;
                                                </asp:Panel>

                                            </center>
                                        </td>
                                    </tr>
                                </table>

                                <div style="line-height:15px;">&nbsp;</div>
                                <asp:Button ID="btnClose" runat="server" Text="&nbsp;&nbsp;Close&nbsp;&nbsp;" OnClientClick="javascript:window.close();return false;" />

                            </center>

                        </td>
                    </tr>
                </table>

            </center>

            <div id="autodivheight" class="divautoheight" style="height:100px;">
            </div>

        </div>
    </div>


</asp:Content>



