﻿
/* ------------- */
/* --- MENUS --- */
/* ------------- */

/*
$('td.Menu_Past_Completed_Has_Invoice').contextMenu('ContextMenu_Past_Completed_Has_Invoice_Clinic', {

    bindings: {
        'pchi_viewinvoice_cl': function (t) {
            view_invoice(t.id, false);
        },
        'pchi_futurebooking_cl': function (t) {

            // popup new booking screen..

            var booking_id = get_booking_id_from_t_id(t.id);
            var patient_id = ajax_get_patient_id_by_booking_id(booking_id);

            var new_url = window.location.href;
            new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
            new_url = set_url_field(new_url, "patient", patient_id);
            new_url = remove_url_field(new_url, "offering");
            new_url = remove_url_field(new_url, "date");  // remove date to make it show today, so that it's not in the past!
            //open_new_window(new_url, -1, -1);
            var win = window.open(new_url, '_blank');
            win.focus();
        },
        'pchi_printinvoice_cl': function (t) {

            var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
            var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

            if (!ajax_booking_has_next_after_today(get_booking_id_from_t_id(t.id)))
                if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
                    return;
            document.getElementById('printOrEmailInvoiceBookingID').value = get_booking_id_from_t_id(t.id);
            document.getElementById('btnPrintInvoice').click();
        },
        'pchi_emailinvoice_cl': function (t) {

            var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
            var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

            if (!ajax_booking_has_next_after_today(get_booking_id_from_t_id(t.id)))
                if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
                    return;
            document.getElementById('printOrEmailInvoiceBookingID').value = get_booking_id_from_t_id(t.id);
            document.getElementById('btnEmailInvoice').click();
        },
        'pchi_reversebooking_cl': function (t) {
            reverse_booking(t.id);
        },
        'pchi_printletters_cl': function (t) {
            print_letters(t.id, false);
        }
    }
});

$('td.Menu_Past_Completed_Has_Invoice').contextMenu('ContextMenu_Past_Completed_Has_Invoice_AC', {

    bindings: {
        'pchi_viewinvoice_ac': function (t) {
            view_invoice(t.id, false);
        },
        'pchi_futurebooking_ac': function (t) {

            // popup new booking screen..

            var booking_id = get_booking_id_from_t_id(t.id);
            var patient_id = ajax_get_patient_id_by_booking_id(booking_id);

            var new_url = window.location.href;
            new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
            new_url = set_url_field(new_url, "patient", patient_id);
            new_url = remove_url_field(new_url, "offering");
            new_url = remove_url_field(new_url, "date");  // remove date to make it show today, so that it's not in the past!
            //open_new_window(new_url, -1, -1);
            var win = window.open(new_url, '_blank');
            win.focus();
        },
        'pchi_printinvoice_ac': function (t) {

            var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
            var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

            if (!ajax_booking_has_next_after_today(get_booking_id_from_t_id(t.id)))
                if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
                    return;
            document.getElementById('printOrEmailInvoiceBookingID').value = get_booking_id_from_t_id(t.id);
            document.getElementById('btnPrintInvoice').click();
        },
        'pchi_emailinvoice_ac': function (t) {

            var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
            var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

            if (!ajax_booking_has_next_after_today(get_booking_id_from_t_id(t.id)))
                if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
                    return;
            document.getElementById('printOrEmailInvoiceBookingID').value = get_booking_id_from_t_id(t.id);
            document.getElementById('btnEmailInvoice').click();
        },
        'pchi_reversebooking_ac': function (t) {
            reverse_booking(t.id);
        },
        'pchi_printletters_ac': function (t) {
            print_letters(t.id, false);
        }
    }
});

$(document).ready(function () {
    $('td.Menu_Past_Completed_No_Invoice').contextMenu('ContextMenu_Past_Completed_No_Invoice', {

        bindings: {
            'pcni_futurebooking': function (t) {

                // popup new booking screen..

                var booking_id = get_booking_id_from_t_id(t.id);
                var patient_id = ajax_get_patient_id_by_booking_id(booking_id);

                var new_url = window.location.href;
                new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
                new_url = set_url_field(new_url, "patient", patient_id);
                new_url = remove_url_field(new_url, "offering");
                new_url = remove_url_field(new_url, "date");  // remove date to make it show today, so that it's not in the past!
                //open_new_window(new_url, -1, -1);
                var win = window.open(new_url, '_blank');
                win.focus();
            },
            'pcni_printletters': function (t) {
                print_letters(t.id, false);
            },
            'pcni_reversebooking': function (t) {
                reverse_booking(t.id);
            }
        }
    });
});

$(document).ready(function () {
    $('td.Menu_Past_Uncompleted_Clinic').contextMenu('ContextMenu_Past_Uncompleted_Clinic', {

        bindings: {
            'pu_complete_cl': function (t) {
                show_modal_complete_booking(t.id, "complete", true);
            },
            'pu_viewphnum_cl': function (t) {
                show_modal_view_ph_num(t.id);
            },
            'pu_printletters_cl': function (t) {
                print_letters(t.id, false);
            },
            'pu_edit_cl': function (t) {
                set_edit_booking(t.id);
            },
            'pu_cancel_no_fee_cl': function (t) {
                delete_confirm_cancel_booking(t.id, "cancel");
            },
            'pu_cancel_w_fee_cl': function (t) {
                show_modal_complete_booking(t.id, "cancel", true);
            },
            'pu_delete_cl': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            },
            'pu_deceased_cl': function (t) {
                delete_confirm_cancel_booking(t.id, "deceased");
            }
        }

    });
});

$(document).ready(function () {
    $('td.Menu_Past_Uncompleted_AC').contextMenu('ContextMenu_Past_Uncompleted_AC', {

        bindings: {
            'pu_complete_ac': function (t) {
                show_modal_complete_booking(t.id, "complete", true);
            },
            'pu_add_patients_ac': function (t) {
                show_modal_complete_booking(t.id, "add_patients", true);
            },
            'pu_viewphnum_ac': function (t) {
                show_modal_view_ph_num(t.id);
            },
            'pu_printletters_ac': function (t) {
                print_letters(t.id, false);
            },
            'pu_edit_ac': function (t) {
                set_edit_booking(t.id);
            },
            'pu_cancel_no_fee_ac': function (t) {
                delete_confirm_cancel_booking(t.id, "cancel");
            },
            'pu_cancel_w_fee_ac': function (t) {
                show_modal_complete_booking(t.id, "cancel", true);
            },
            'pu_delete_ac': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            },
            'pu_deceased_ac': function (t) {
                delete_confirm_cancel_booking(t.id, "deceased");
            }
        }

    });
});

$(document).ready(function () {
    $('td.Menu_Future_Clinic').contextMenu('ContextMenu_Future_Clinic', {

        bindings: {
            'f_viewphnum_cl': function (t) {
                show_modal_view_ph_num(t.id);
            },
            'f_printletters_cl': function (t) {
                print_letters(t.id, false);
            },
            'f_edit_cl': function (t) {
                set_edit_booking(t.id);
            },
            'f_delete_cl': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            }
        }
    });
});

$(document).ready(function () {
    $('td.Menu_Future_AC').contextMenu('ContextMenu_Future_AC', {

        bindings: {
            'f_viewphnum_ac': function (t) {
                show_modal_view_ph_num(t.id);
            },
            'f_printletters_ac': function (t) {
                print_letters(t.id, false);
            },
            'f_edit_ac': function (t) {
                set_edit_booking(t.id);
            },
            'f_delete_ac': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            }
        }
    });
});

$(document).ready(function () {
    $('td.Menu_Future_PatientLoggedIn').contextMenu('ContextMenu_Future_PatientLoggedIn', {

        bindings: {
            'fpt_edit': function (t) {
                set_edit_booking(t.id);
            },
            'fpt_delete': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            }
        }
    });
});




$(document).ready(function () {
    $('td.showAddableContext').contextMenu('addableMenu', {

        bindings: {
            'add': function (t) {
                show_modal_add_box(t.id, false);
            },
            'add_multiple_days': function (t) {
                show_modal_add_box(t.id, true);
            }
        }
    });
});

$(document).ready(function () {
    $('td.showAddableContext_PatientLoggedIn').contextMenu('addableMenu_PatientLoggedIn', {

        bindings: {
            'add_pt_loggedin': function (t) {
                show_modal_add_box(t.id, false);
            }
        }
    });
});

$(document).ready(function () {
    $('td.showTakenButAddableContext').contextMenu('takenButAddableMenu', {

        bindings: {
            'taken_add': function (t) {

                var line1 = "Warning!  This will add a booking to an unavailable time slot.";
                var line2 = "Are you sure you want to continue?";
                var text = line1 + "\r\n" + "\r\n" + Array((line1.length - line2.length) / 2).join(" ") + line2;
                if (confirm(text))
                    show_modal_add_box(t.id, false);
            },
            'taken_add_multiple_days': function (t) {

                var line1 = "Warning!  This will add a booking to an unavailable time slot.";
                var line2 = "Are you sure you want to continue?";
                var text = line1 + "\r\n" + "\r\n" + Array((line1.length - line2.length) / 2).join(" ") + line2;
                if (confirm(text))
                    show_modal_add_box(t.id, true);
            }
        }
    });
});

$(document).ready(function () {
    $('td.showTakenButUpdatableContext').contextMenu('takenButUpdatableMenu', {

        bindings: {
            'taken_update_newtime': function (t) {

                var line1 = "Warning!  This will move the booking to an unavailable time slot.";
                var line2 = "Are you sure you want to continue?";
                var text = line1 + "\r\n" + "\r\n" + Array(Math.round((line1.length - line2.length) / 2)).join(" ") + line2;
                if (confirm(text))
                    show_modal_add_box(t.id);
            },
            'taken_update_cancel_edit': function (t) {
                cancel_edit();
            }
        }
    });
});

$(document).ready(function () {
    $('td.showTakenContext').contextMenu('takenMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});

$(document).ready(function () {
    $('td.showUpdatableContext').contextMenu('updateableMenu', {

        bindings: {
            'update_newtime': function (t) {
                show_modal_add_box(t.id);
            },
            'update_cancel_edit': function (t) {
                cancel_edit();
            }
        }
    });
});

$(document).ready(function () {
    $('th.showFullDayAddableContext').contextMenu('fullDaysAddableMenu', {

        bindings: {
            'full_days_add': function (t) {
                show_modal_add_full_days_box(t.id);
            },
            'full_days_blockout_single_day': function (t) {
                show_modal_add_full_days_box(t.id, "single_day");
            },
            'full_days_blockout_multiple_days': function (t) {
                show_modal_add_full_days_box(t.id, "multiple_days");
            },
            'full_days_blockout_multiple_days_series': function (t) {
                show_modal_add_full_days_box(t.id, "multiple_days_series");
            },
            'full_days_move': function (t) {
                set_edit_day(t.id);
            }
        }
    });
});

$(document).ready(function () {
    $('th.showFullDayUpdatableContext').contextMenu('fullDaysUpdatableMenu', {

        bindings: {
            'update_fullday': function (t) {
                change_full_days_bookings(t.id);
            }
        }
    });
});

$(document).ready(function () {
    $('td.showFullDayTakenContext').contextMenu('fullDayEditable', {

        bindings: {
            'full_days_make_booking': function (t) {

                var line1 = "Warning!  This will add a booking to an unavailable time slot.";
                var line2 = "Are you sure you want to continue?";
                var text = line1 + "\r\n" + "\r\n" + Array((line1.length - line2.length) / 2).join(" ") + line2;
                if (confirm(text))
                    show_modal_add_box(t.id);
            },
            'full_days_delete': function (t) {
                delete_confirm_cancel_booking(t.id, "delete");
            }
        }
    });
});


$(document).ready(function () {
    $('td.emptyContext').contextMenu('emptyContextMenu', {

    });
});
$(document).ready(function () {
    $('th.emptyContext').contextMenu('emptyContextMenu', {

    });
});


$(document).ready(function () {
    $('td.showPatientAndServiceNotSetContext').contextMenu('patientAndServiceNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});
$(document).ready(function () {
    $('th.showPatientAndServiceNotSetContext').contextMenu('patientAndServiceNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});

$(document).ready(function () {
    $('td.showPatientNotSetContext').contextMenu('patientNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});
$(document).ready(function () {
    $('th.showPatientNotSetContext').contextMenu('patientNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});

$(document).ready(function () {
    $('td.showServiceNotSetContext').contextMenu('serviceNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});
$(document).ready(function () {
    $('th.showServiceNotSetContext').contextMenu('serviceNotSetMenu', {

        itemStyle: {
            backgroundColor: 'gray',
            color: 'white',
            border: 'none',
            padding: '1px'
        },
        itemHoverStyle: {
            color: '#fff',
            backgroundColor: 'gray',
            border: 'none'
        }

    });
});
*/

function ajax_get_patient_id_by_booking_id(booking_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("booking_id", booking_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingPatientID.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result;
}


function ajax_booking_update_show_header_section(show) {

    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            var response = String(xmlhttp.responseText);
            if (response == "SessionTimedOutException")
                window.location.href = window.location.href;  // reload page

            // do nothing .. just setting a variable in the database

        }
    }

    xmlhttp.open("GET", "/AJAX/AjaxBookingUpdateShowHeaderSection.aspx?show=" + (show ? "1" : "0"), true);
    xmlhttp.send();
}


/* ------------------------------------- */
/* --- hide and reveal modal element --- */
/* ------------------------------------- */

function reveal_modal(divID) {
    window.onscroll = function () { document.getElementById(divID).style.top = document.body.scrollTop; };
    document.getElementById(divID).style.display = "block";
    document.getElementById(divID).style.top = document.body.scrollTop;
}
function hide_modal(divID) {
    document.getElementById(divID).style.display = "none";
}
function is_hiden_modal(divID) {
    return document.getElementById(divID).style.display == "none";
}


/* --------------------------------------- */
/* --- update booking scroll position ---  */
/* --------------------------------------- */

function elementOnScroll(elementRef) {
    if (typeof elementRef == 'string')
        elementRef = document.getElementById(elementRef);

    document.getElementById('scrollValues').value = String(elementRef.scrollTop) + "_" + String(elementRef.scrollLeft);
}
function resetScrollPosition(val1, val2) {  // called from code behind!
    document.getElementById('main_panel').scrollTop = val1;
    document.getElementById('main_panel').scrollLeft = val2;
}

var leftScrollPos = 0;
function elementOnScrollLeft(elementRef) {
    if (typeof elementRef == 'string')
        elementRef = document.getElementById(elementRef);

    if (leftScrollPos != elementRef.scrollLeft) {
        leftScrollPos = elementRef.scrollLeft;

        if (elementRef.id != 'header_panel')
            document.getElementById('header_panel').scrollLeft = elementRef.scrollLeft;
        if (elementRef.id != 'main_panel')
            document.getElementById('main_panel').scrollLeft = elementRef.scrollLeft;
    }
}



function go_to_anchor(elem_id) {
    document.getElementById("main_panel").scrollTop = document.getElementById(elem_id).offsetTop;
    document.getElementById("main_panel").scrollLeft = document.getElementById(elem_id).offsetLeft;
}

/* ------------------------------------------------------------------------------------------------------------ */
/* --- get_booking_id_from_t_id, get_offering_id_from_t_id, get_provider_id_from_t_id, get_org_id_from_t_id --- */
/* ------------------------------------------------------------------------------------------------------------ */

function get_booking_id_from_t_id(t_id) {
    return get_lbl_data('lblBookingID' + t_id.substring(nth_occurrence(t_id, '_', 0)));
}
function get_offering_id_from_t_id(t_id) {
    var offering_id = get_lbl_data('lblOfferingID' + t_id.substring(nth_occurrence(t_id, '_', 0)));
    return (offering_id == null) ? "-1" : offering_id;
}
function get_provider_id_from_t_id(t_id) {
    return get_lbl_data('lblProviderID' + t_id.substring(nth_occurrence(t_id, '_', 0)));
}
function get_org_id_from_t_id(t_id) {
    return get_lbl_data('lblOrgID' + t_id.substring(nth_occurrence(t_id, '_', 0)));
}

function nth_occurrence(str, c, n) {
    var pos = str.indexOf(c, 0);
    while (n-- > 0 && pos != -1)
        pos = str.indexOf(c, pos + 1);
    return pos;
}


/* ---------------------------------- */
/* --- get prov/org info from id  --- */
/* ---------------------------------- */

function get_prov_name_by_id(provider_id) {
    var provids_array = get_lbl_data('lblProviderIDs').split(",");
    var provnames_array = get_lbl_data('lblProviderNames').split(",");
    for (var i = 0; i < provids_array.length; i++)
        if (provids_array[i] == provider_id)
            return provnames_array[i];
}
function get_org_name_by_id(org_id) {
    var orgids_array = get_lbl_data('lblOrgIDs').split(",");
    var orgnames_array = get_lbl_data('lblOrgNames').split(",");

    for (var i = 0; i < orgids_array.length; i++)
        if (orgids_array[i] == org_id)
            return orgnames_array[i];
}
function get_org_type_id_by_id(org_id) {
    var orgids_array = get_lbl_data('lblOrgIDs').split(",");
    var orgtypeids_array = get_lbl_data('lblOrgTypeIDs').split(",");

    for (var i = 0; i < orgids_array.length; i++)
        if (orgids_array[i] == org_id)
            return orgtypeids_array[i];
}


/* --------------------------------------------------------- */
/* --- updateModalPopupUnavailableRecurring_HourMinRows  --- */
/* --------------------------------------------------------- */

function updateModalPopupUnavailableRecurring_HourMinRows(chkBox) {

    if (chkBox.checked) {

        document.getElementById('td_ModalPopupUnavailableRecurringModalStartHour_Row').className += " hiddencol";
        document.getElementById('td_ModalPopupUnavailableRecurringModalEndHour_Row').className += " hiddencol";
    }
    else {

        document.getElementById("td_ModalPopupUnavailableRecurringModalStartHour_Row").className = document.getElementById("td_ModalPopupUnavailableRecurringModalStartHour_Row").className.replace(/(?:^|\s)hiddencol(?!\S)/, '');
        document.getElementById("td_ModalPopupUnavailableRecurringModalEndHour_Row").className = document.getElementById("td_ModalPopupUnavailableRecurringModalEndHour_Row").className.replace(/(?:^|\s)hiddencol(?!\S)/, '');
    }
}


/* -------------------------- */
/* --- view invoice popup --- */
/* -------------------------- */

function view_invoice(t_id, show_as_popup) {

    var booking_id = get_booking_id_from_t_id(t_id);

    if (show_as_popup)
        window.showModalDialog('Invoice_ViewV2.aspx?booking_id=' + booking_id, '', 'dialogWidth:820px;dialogHeight:860px;center:yes;resizable:no; scroll:no');
    else // show in new tab
    {
        var win = window.open('Invoice_ViewV2.aspx?booking_id=' + booking_id, '_blank');
        win.focus();
    }
}


/* -------------------------- */
/* ------ print invoice ----- */
/* -------------------------- */

function print_invoice(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    if (!ajax_booking_has_next_after_today(booking_id))
        if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
            return;
    document.getElementById('printOrEmailInvoiceBookingID').value = booking_id;
    document.getElementById('btnPrintInvoice').click();
}


/* -------------------------- */
/* ------ email invoice ----- */
/* -------------------------- */

function email_invoice(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    if (!ajax_booking_has_next_after_today(booking_id))
        if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
            return;
    document.getElementById('printOrEmailInvoiceBookingID').value = booking_id;
    document.getElementById('btnEmailInvoice').click();
}


/* -------------------------- */
/* ----- treatment list ----- */
/* -------------------------- */

function treatment_list(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);
    var new_url = '/Invoice_ACTreatmentListV2.aspx?booking=' + booking_id;
    var win = window.open(new_url, '_blank');
    win.focus();
}


/* ---------------------------------------- */
/* --- view future booking popup screen --- */
/* ---------------------------------------- */

function future_booking(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);
    var patient_id = ajax_get_patient_id_by_booking_id(booking_id);

    var new_url = window.location.href;
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "patient", patient_id);
    new_url = remove_url_field(new_url, "offering");
    new_url = remove_url_field(new_url, "date");  // remove date to make it show today, so that it's not in the past!
    //open_new_window(new_url, -1, -1);

    var win = window.open(new_url, '_blank');
    win.focus();

}


/* --------------------------- */
/* --- email invoice popup --- */
/* --------------------------- */

function email_invoice(t_id) {

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    if (!ajax_booking_has_next_after_today(get_booking_id_from_t_id(t_id)))
        if (!confirm("There has been no next booking created to appear on the invoice.\r\n\r\nTo create a next booking first so that it appears on the invoice:\r\nhit \"Cancel\" and right click -> 'Make Future Booking' before printing\r\n\r\nOtherwise - print anyway?"))
            return;
    document.getElementById('printOrEmailInvoiceBookingID').value = get_booking_id_from_t_id(t_id);
    document.getElementById('btnEmailInvoice').click();
}


/* -------------------------- */
/* ---- reverse booking ----- */
/* -------------------------- */

function reverse_booking(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);

    var message = ajax_booking_can_reverse(booking_id);
    if (message.length > 0) {
        alert(message);
        return;
    }

    if (confirm('Are you sure you want to reverse this booking status to un-completed and delete any invoices associated with it?')) {
        ajax_booking_confirm_delete_cancel("reverse", booking_id);
    }
}


/* ------------------------------ */
/* --- complete booking popup --- */
/* ------------------------------ */

function show_modal_complete_booking(t_id, type, show_as_popup) {

    var booking_id = get_booking_id_from_t_id(t_id);

    var t_id_SplitResult     = String(t_id).split("_");
    var selected_org_id      = t_id_SplitResult[1];
    var selected_org_type_id = get_org_type_id_by_id(selected_org_id);
    var is_clinic            = selected_org_type_id == "218";
    var patient_id           = ajax_get_patient_id_by_booking_id(booking_id);


    if (show_as_popup) {

        // show modal popup
        if (!is_clinic)
            var result = window.showModalDialog('BookingCreateInvoiceAgedCareV2.aspx?booking=' + booking_id + '&type=' + type + '&completion_type=standard', '', 'dialogHide:yes;dialogWidth:1275px;dialogHeight:800px;center:yes;resizable:no; scroll:no');
        else if (patient_id != -1)
            var result = window.showModalDialog('BookingCreateInvoiceV2.aspx?booking=' + booking_id + '&type=' + type, '', 'dialogHide:yes;dialogWidth:1325px;dialogHeight:800px;center:yes;resizable:no; scroll:no');
        else
            var result = window.showModalDialog('BookingCreateInvoiceGroupV2.aspx?booking=' + booking_id + '&type=' + type, '', 'dialogHide:yes;dialogWidth:1275px;dialogHeight:800px;center:yes;resizable:no; scroll:no');

        // reload booking page!
        var new_url = window.location.href;
        new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
        if (is_clinic)
            new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));
        window.location.href = new_url;
    }
    else {  // for use on mobiles where it has to be a new tab as no popus are shown

        // open in new tab ... refresh this parent from child when child closes itself
        var url;
        if (!is_clinic)
            url = 'BookingCreateInvoiceAgedCareV2.aspx?booking=' + booking_id + '&type=' + type + '&completion_type=standard&refresh_on_close=1';
        else if (patient_id != -1)
            url = 'BookingCreateInvoiceV2.aspx?booking=' + booking_id + '&type=' + type + "&refresh_on_close=1";
        else
            url = 'BookingCreateInvoiceGroupV2.aspx?booking=' + booking_id + '&type=' + type + "&refresh_on_close=1";

        var w = window.open(url, '_blank');
        w.focus();
    }

}


/* ---------------------------------------------- */
/* --- show phone numbers (&to allow comfirm) --- */
/* ---------------------------------------------- */

function show_modal_view_ph_num(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);

    var ajax_ph_nums = ajax_get_ph_nums_by_booking_id(booking_id);  // "12345678,98765432";

    var name_nbrs = ajax_ph_nums.split("::");
    var name = "";
    var is_confirmed = false;
    var ph_nums = ""; // var ph_nums = String(ajax_ph_nums).split(",");
    if (name_nbrs.length == 1) {
        ph_nums = String(ajax_ph_nums).split(",");
    }
    else {
        name = name_nbrs[0];
        is_confirmed = (name_nbrs[1] == "1");
        ph_nums = String(name_nbrs[2]).split(",");
    }


    var text = "";
    for (var i = 0; i < ph_nums.length; i++) {
        if (i > 0)
            text = text + "<br />";
        text = text + format_ph_num(ph_nums[i]);
    }

    if (is_confirmed) {
        document.getElementById('conf_buttons').style.visibility = 'hidden';
        document.getElementById('non_conf_buttons').style.visibility = 'visible';
    }
    else {
        document.getElementById('non_conf_buttons').style.visibility = 'hidden';
        document.getElementById('conf_buttons').style.visibility = 'visible';
    }

    document.getElementById('modalPopupViewPhNbrs_Name').innerHTML = "Phone numbers of " + name;
    document.getElementById('modalPopupViewPhNbrs_PhNums').innerHTML = text;
    document.getElementById('modalPopupViewPhNbrs_booking_tid').innerHTML = t_id;
    reveal_modal('modalPopupViewPhNbrs');
}
function format_ph_num(ph_num) {
    if (ph_num.indexOf("-") != -1)
        return ph_num;

    var ret_ph_num = "";
    for (i = 0; i < ph_num.length; i++) {
        if (i == 3 || i == 6)
            ret_ph_num += "-";
        ret_ph_num += ph_num.charAt(i);
    }
    return ret_ph_num;
}
function modal_ph_num__set_confirmed() {
    delete_confirm_cancel_booking(document.getElementById('modalPopupViewPhNbrs_booking_tid').innerHTML, "confirm");
}
function ajax_get_ph_nums_by_booking_id(booking_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("booking_id", booking_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingPhoneNbrs.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result;
}


/* -------------------------- */
/* ----- print letters ------ */
/* -------------------------- */

function print_letters(t_id, go_to_page) {
    var booking_id = get_booking_id_from_t_id(t_id);

    if (go_to_page)
        window.location.href = 'Letters_PrintV2.aspx?booking=' + booking_id;
    else // show in new tab
    {
        var win = window.open('Letters_PrintV2.aspx?booking=' + booking_id, '_blank');
        win.focus();
    }

    
}


/* -------------------------- */
/* --- show modal add box --- */
/* -------------------------- */

function show_modal_add_box(t_id, add_multiple_days) {

    var t_id_SplitResult = String(t_id).split("_");

    if (page_type == "bookings") {
        var selected_org_id = get_ddl_value("ddlClinics");
        var selected_org_type_id = get_ddl_text("ddlClinics");
        var selected_org_name = get_lbl_data("lblOrgName");
        set_lbl_data('lblSelectedOrgID', selected_org_id);
        set_lbl_data('lblSelectedOrgTypeID', selected_org_type_id);
    }
    else if (page_type == "bookings_for_clinic") {
        var selected_org_id = t_id_SplitResult[1];
        var selected_org_type_id = get_org_type_id_by_id(selected_org_id);
        var selected_org_name = get_org_name_by_id(selected_org_id);
        set_lbl_data('lblSelectedOrgID', selected_org_id);
        set_lbl_data('lblSelectedOrgTypeID', selected_org_type_id);
    }

    var is_agedcareorg = document.getElementById('lblEditBooking_IsAgedCare').firstChild.data == "1" || selected_org_type_id != "218";
    

    var offering_info = is_agedcareorg || get_ddl_value("ddlServices") == "-1" ? null : ajax_get_offering_info(get_ddl_value("ddlServices"));
    var staff_info = ajax_get_staff_info(String(t_id_SplitResult[2]));


    // check if is to edit or add new
    var edit_booking_id = getUrlVars()["edit_booking_id"];
    var is_edit_booking = (edit_booking_id != undefined);

    if (!is_edit_booking && !is_agedcareorg) {
        if (get_ddl_value("ddlServices") == "-1") {
            alert("Please select a service");
            return;
        }

        var service_popup_message = offering_info == null ? "" : offering_info[2];
        if (service_popup_message.length > 0) {
            if (!confirm(service_popup_message + "\r\n\r\nWould you like to continue?"))
                return;
        }
    }

    // check offering field is same as provider field
    if (!is_agedcareorg) {
        if (offering_info[3] != staff_info[1]) {
            alert("Can not add a " + offering_info[4] + " service to a provider set as a " + staff_info[2] + ".\r\nEither chante the provider's 'Role' to '" + offering_info[4] + "'  or change the service.");
            return;
        }
    }

    if (!is_edit_booking)
        document.getElementById("editReasonRow").style.display = 'none';

    document.getElementById("modalTitle").innerHTML = (!is_edit_booking) ? "New Booking" : "Edit Booking";

    // disable changing minute in aged care since it is hourly appointments
    document.getElementById("ddlModalStartMinute").disabled = false; //is_agedcareorg;
    document.getElementById("ddlModalEndMinute").disabled = false; //is_agedcareorg;

    // Putting start and end time in modal box
    var las_pos = t_id_SplitResult.length - 1;
    var yr = String(t_id_SplitResult[las_pos - 3]);
    var mo = String(t_id_SplitResult[las_pos - 2]);
    var day = String(t_id_SplitResult[las_pos - 1]);
    var hr = String(t_id_SplitResult[las_pos]).substring(0, 2);
    var min = String(t_id_SplitResult[las_pos]).substring(2, 4);

    var booking_datetime_str = yr + "_" + mo + "_" + day + "_" + hr + min;

    var selectEl = document.getElementById('ddlModalStartHour');
    var opts = selectEl.getElementsByTagName('option');
    for (var i = 0; i < opts.length; i++) {
        opts[i].selected = (String(opts[i].value) == String(hr) ? "selected" : "");
    }

    //if (!is_agedcareorg) {
        selectEl = document.getElementById('ddlModalStartMinute');
        opts = selectEl.getElementsByTagName('option');
        for (i = 0; i < opts.length; i++) {
            opts[i].selected = (String(opts[i].value) == String(min) ? "selected" : "");
        }
    //}

    var minimum_booking_minutes = document.getElementById('lblMinBookingDurationMins').innerHTML; //10;  // (getUrlVars()["type"] == "agedcare") || is_agedcareorg ? 60 : 10;
    service_mins = parseInt(String(service_mins), 10);

    if (is_edit_booking) {

        // set checkbox for current booking
        var is_confirmed = document.getElementById('lblEditBooking_IsConfirmed').firstChild.data == "1";
        var confirmed = document.getElementById("chkConfirmed").checked = is_confirmed;

        // set duration for current booking
        var service_mins = offering_info == null ? "60" : offering_info[1];
        service_mins = parseInt(String(service_mins), 10);
        service_mins = String(Math.ceil(service_mins / minimum_booking_minutes) * minimum_booking_minutes);  // round up to nearest 10 or 60 mins
    }
    else {
        // set default duration for this offering
        var service_mins = offering_info == null ? "60" : offering_info[1];
        service_mins = parseInt(String(service_mins), 10);
        service_mins = String(Math.ceil(service_mins / minimum_booking_minutes) * minimum_booking_minutes);  // round up to nearest 10 or 60 mins
    }


    var endTime = new Date(parseInt(yr, 10), parseInt(mo, 10) - 1, parseInt(day, 10), parseInt(hr, 10), parseInt(String(min), 10), 0)
    endTime.setMinutes(endTime.getMinutes() + parseInt(String(service_mins), 10));
    hr = String(endTime.getHours());
    if (hr.length == 1) hr = "0" + hr;
    if (hr.length == 0) hr = "00";
    min = endTime.getMinutes();
    if (min.length == 1) min_rounded_up = "0" + min_rounded_up;
    if (min.length == 0) min_rounded_up = "00";

    var selectEl = document.getElementById('ddlModalEndHour');
    var opts = selectEl.getElementsByTagName('option');
    for (var i = 0; i < opts.length; i++) {
        opts[i].selected = (String(opts[i].value) == String(hr) ? "selected" : "");
    }
    //if (!is_agedcareorg) {
        var selectEl = document.getElementById('ddlModalEndMinute');
        var opts = selectEl.getElementsByTagName('option');
        for (var i = 0; i < opts.length; i++) {
            opts[i].selected = (String(opts[i].value) == String(min) ? "selected" : "");
        }
    //}

    // set visible date
    document.getElementById('modalServiceDate').innerHTML = "<b><font color=\"#505050\">" + get_weekday(endTime.getDay()) + " " + get_month(endTime.getMonth()) + " " + endTime.getDate() + " " + endTime.getFullYear() + "</font></b>";

    // set hidden date so can get from when click to book
    document.getElementById("modalDate").innerHTML = yr + "_" + mo + "_" + day;

    // set offering name
    if (!is_agedcareorg)
        document.getElementById('modalServiceDescr').innerHTML = "<b><font color=\"#505050\">" + (offering_info == null ? "" : offering_info[0]) + "</font></b>";
    else
        document.getElementById('modalServiceDescr').innerHTML = "<b><font color=\"#505050\">" + "[Aged Care Booking]" + "</font></b>";

    document.getElementById("serviceRow").style.display = is_agedcareorg ? 'none' : '';
    document.getElementById("serviceTrailingSpaceRow").style.display = is_agedcareorg ? 'none' : '';

    document.getElementById('spnModalPage').style.height = is_agedcareorg ? "321px" : "350px";  // change height of box to 29px less....

    if (selected_org_type_id == "218")
        document.getElementById('orgRow').innerHTML = "Clinic:";
    if (selected_org_type_id == "139")
        document.getElementById('orgRow').innerHTML = "Facility:";
    if (selected_org_type_id == "367")
        document.getElementById('orgRow').innerHTML = "Wing:";
    if (selected_org_type_id == "372")
        document.getElementById('orgRow').innerHTML = "Unit:";

    // set provider name
    if (page_type == "bookings") {
        document.getElementById('modalProviderDescr').innerHTML = "<b><font color=\"#505050\">" + get_lbl_data('lblProviderName') + "</font></b>";
    }
    else if (page_type == "bookings_for_clinic") {
        var selected_provider_id = String(t_id_SplitResult[2]);
        document.getElementById('modalProviderDescr').innerHTML = "<b><font color=\"#505050\">" + staff_info[0] + "</font></b>";
        set_lbl_data('lblSelectedProviderID', selected_provider_id);
    }

    // set org name
    document.getElementById('modalOrgDescr').innerHTML = "<b><font color=\"#505050\">" + selected_org_name + "</font></b>";

    // if pt logged in, disalow them to change the times so they can not change the time length of a booking from default
    var bookingTimeEditable = get_lbl_data('lblBookingTimeEditable') == "1";
    if (!bookingTimeEditable) {
        document.getElementById("ddlModalStartHour").disabled = true;
        document.getElementById("ddlModalStartMinute").disabled = true;
        document.getElementById("ddlModalEndHour").disabled = true;
        document.getElementById("ddlModalEndMinute").disabled = true;
    }


    // check if they have used their "service" limit - and if so, popup a message to warn them it will be pt pays item
    if (!is_agedcareorg && document.getElementById('txtPatientID').value != '') {
        var patient_id = document.getElementById('txtPatientID').value;
        var offering_id = get_ddl_value("ddlServices");
        var offering_max_nbr_limits = ajax_get_offering_max_nbr_limit(patient_id, offering_id, booking_datetime_str);
        if (/^\d+:\d+:\d+$/.test(offering_max_nbr_limits)) {
            var result_arr = String(offering_max_nbr_limits).split(":");
            var used = parseInt(result_arr[0]);
            var limit = parseInt(result_arr[1]);
            var limit_months = parseInt(result_arr[2]);

            if (limit > 0 && used >= limit)
                if (!confirm("Patient has been invoiced for this service " + used + " time" + (used > 1 ? "s" : "") + " in the past " + limit_months + " month" + (limit_months > 1 ? "s" : "") + ".\r\nThe medicare limit is " + limit + " so this booking will be paid by the patient.\r\n\r\nWould you still like to make the booking?"))
                    return;
        } else {
            alert("Error: unknown result returned: " + offering_max_nbr_limits);
        }
    }

    document.getElementById('spn_clash_bookings_bk').innerHTML = "";
    document.getElementById('tr_clash_bookings_bk').className = "hiddencol";
    document.getElementById('spnModalPage').className = "modalPopup";

    document.getElementById('ddlEveryNWeeks').selectedIndex = 0;
    document.getElementById('ddlOcurrences').selectedIndex = 0;

    if (add_multiple_days) {

        document.getElementById('modalTitle').innerHTML = "New Booking - Multiple Weeks";

        document.getElementById('tr_ModalPopupRecurring_every_n_weeks').className = "";
        document.getElementById('tr_ModalPopupRecurring_weeks_space').className = "";

        document.getElementById('spnModalPage').style.height = is_edit_booking ? "404px" : "384px";
        document.getElementById('spnModalPage').style.top = is_edit_booking ? "-182px" : "-172px";
    }
    else {

        document.getElementById('modalTitle').innerHTML = "New Booking";

        document.getElementById('tr_ModalPopupRecurring_every_n_weeks').className = "hiddencol";
        document.getElementById('tr_ModalPopupRecurring_weeks_space').className = "hiddencol";

        document.getElementById('spnModalPage').style.height = is_edit_booking ? "370px" : "370px";
        document.getElementById('spnModalPage').style.top = is_edit_booking ? "-160px" : "-160px";
    }

    reveal_modal('modalPage');
}

function ajax_get_offering_info(offering_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("offering", offering_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxGetOfferingInfo.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result.startsWith("Exception: ")) {
        //alert(result);
        return null;
    }
    else {
        return result.split("<<sep>>");
    }
}
function ajax_get_staff_info(staff_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("staff", staff_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxGetStaffInfo.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result.startsWith("Exception: ")) {
        //alert(result);
        return null;
    }
    else {
        return result.split("<<sep>>");
    }
}


/* ----------------------------------- */
/* --- show modal add full day box --- */
/* ----------------------------------- */

function show_modal_add_full_days_box(t_id, type) {

    // types:
    // 
    // single_day
    // multiple_days
    // multiple_days_series

    if (page_type == "bookings") {

        // Putting date in modal box
        var mySplitResult = String(t_id).split("_");  // td_2012_04_19
        var yr = String(mySplitResult[1]);
        var mo = String(mySplitResult[2]);
        var day = String(mySplitResult[3]);

        var str_date = day + "-" + mo + "-" + yr;
        document.getElementById('txtModalPopupBookFullDays_StartDate').value = str_date;
        document.getElementById('txtModalPopupBookFullDays_EndDate').value = str_date;

        document.getElementById('modalPopupBookFullDays_ProviderDescr').innerHTML = "<b>" + get_lbl_data('lblProviderName') + "</font></b>";

        reveal_modal('modalPopupBookFullDays');
    }
    else if (page_type == "bookings_for_clinic") {

        var orgUnavail = false;
        var provUnavail = true;  // change to se this later

        if (provUnavail)
            hide_modal('ddlOrgUnavailabilityReason')
        else // if (orgUnavail)
            hide_modal('ddlProvUnavailabilityReason')

        var mySplitResult = String(t_id).split("_");
        var org_id = String(mySplitResult[1]);
        var org_name = get_org_name_by_id(org_id);
        var provider_id = String(mySplitResult[2]);
        var prov_name = get_prov_name_by_id(provider_id);
        set_lbl_data('lblSelectedOrgID', org_id);
        set_lbl_data('lblSelectedProviderID', provider_id);

        document.getElementById('modalPopupUnavailableRecurring_ProviderDescr').innerHTML = "<b>" + prov_name + "</font></b>";
        document.getElementById('modalPopupUnavailableRecurring_OrgDescr').innerHTML = "<b>" + org_name + "</font></b>";

        document.getElementById('chkModalPopupUnavailableRecurringOnlyThisOrg').checked = true;


        var yr = String(mySplitResult[3]);
        var mo = String(mySplitResult[4]);
        var day = String(mySplitResult[5]);

        var txtboxDate = day + '-' + mo + '-' + yr;
        var date = new Date(parseInt(yr, 10), parseInt(mo, 10) - 1, parseInt(day, 10), 0, 0, 0);

        document.getElementById('chkModalPopupUnavailableRecurring_Sunday').checked = date.getDay() == 0;
        document.getElementById('chkModalPopupUnavailableRecurring_Monday').checked = date.getDay() == 1;
        document.getElementById('chkModalPopupUnavailableRecurring_Tuesday').checked = date.getDay() == 2;
        document.getElementById('chkModalPopupUnavailableRecurring_Wednesday').checked = date.getDay() == 3;
        document.getElementById('chkModalPopupUnavailableRecurring_Thursday').checked = date.getDay() == 4;
        document.getElementById('chkModalPopupUnavailableRecurring_Friday').checked = date.getDay() == 5;
        document.getElementById('chkModalPopupUnavailableRecurring_Saturday').checked = date.getDay() == 6;


        document.getElementById('chkModalPopupUnavailableRecurring_AllDay').checked = false;
        updateModalPopupUnavailableRecurring_HourMinRows(false);

        var list;
        list = document.getElementById('ddlModalPopupUnavailableRecurringModalStartHour');
        list.options[list.selectedIndex].selected = false;
        list = document.getElementById('ddlModalPopupUnavailableRecurringModalStartMinute');
        list.options[list.selectedIndex].selected = false;
        list = document.getElementById('ddlModalPopupUnavailableRecurringModalEndHour');
        list.options[list.selectedIndex].selected = false;
        list = document.getElementById('ddlModalPopupUnavailableRecurringModalEndMinute');
        list.options[list.selectedIndex].selected = false;

        document.getElementById('txtModalPopupUnavailableRecurring_StartDate').value = txtboxDate;
        document.getElementById('txtModalPopupUnavailableRecurring_EndDate').value = txtboxDate;

        document.getElementById('booking_sequence_type_seperate').checked = false;
        document.getElementById('booking_sequence_type_series').checked = false;


        document.getElementById('spn_clash_bookings').innerHTML = "";
        document.getElementById('tr_clash_bookings').className = "hiddencol";
        document.getElementById('spnModalPopupUnavailableRecurring').className = "modalPopupUnavailableRecurring";



        // types:
        // 
        // single_day
        // multiple_days
        // multiple_days_series

        if (type == "single_day") {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Blockout";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "0";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "";
            
            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className               = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className         = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className           = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className      = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "hiddencol";

            document.getElementById('booking_sequence_type_seperate').checked = true;
            document.getElementById('booking_sequence_type_series').checked   = false;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height =  "375px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top = "-178px";
        }
        else if (type == "multiple_days") {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Blockout Multiple Days";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "0";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "";

            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className               = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className         = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className           = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className      = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "hiddencol";

            document.getElementById('booking_sequence_type_seperate').checked = true;
            document.getElementById('booking_sequence_type_series').checked   = false;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height =  "630px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top    = "-315px";
        }
        else if (type == "multiple_days_series") {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Blockout Multiple Days<br /> - As Series - ";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "0";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "";


            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className               = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className         = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className           = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className      = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "hiddencol";

            document.getElementById('booking_sequence_type_seperate').checked = false;
            document.getElementById('booking_sequence_type_series').checked   = true;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height =  "630px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top    = "-315px";
        }
        else if (type == "paid_single_day") {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Paid Blockout";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "1";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "none";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "none";

            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className = "hiddencol";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "hiddencol";

            document.getElementById('booking_sequence_type_seperate').checked = true;
            document.getElementById('booking_sequence_type_series').checked = false;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height = "335px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top = "-158px";
        }
        else if (type == "paid_multiple_days") {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Paid Blockout Multiple Days";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "1";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "none";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "none";

            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "hiddencol";

            document.getElementById('booking_sequence_type_seperate').checked = true;
            document.getElementById('booking_sequence_type_series').checked = false;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height = "580px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top = "-290px";
        }
        else {

            document.getElementById('modalPopupUnavailableRecurring_Title').innerHTML = "Add Recurring Unavailability";
            document.getElementById('modalPopupUnavailableRecurring_IsPaid').value = "0";
            document.getElementById('tr_ModalPopupUnavailableRecurring_onlyThisOrg').style.display = "";
            document.getElementById('tr_ModalPopupUnavailableRecurringModal_fullDay').style.display = "";

            document.getElementById('tr_ModalPopupUnavailableRecurring_days').className               = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_start_date').className         = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_end_date').className           = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_every_n_weeks').className      = "";
            document.getElementById('tr_ModalPopupUnavailableRecurring_series_or_seperate').className = "";

            document.getElementById('booking_sequence_type_seperate').checked = false;
            document.getElementById('booking_sequence_type_series').checked   = false;

            document.getElementById('spnModalPopupUnavailableRecurring').style.height =  "600px";
            document.getElementById('spnModalPopupUnavailableRecurring').style.top    = "-300px";
        }


        reveal_modal('modalPopupUnavailableRecurring');
    }
}


/* ------------------------ */
/* --- set edit booking --- */
/* ------------------------ */

function set_edit_booking(t_id) {

    var booking_id = get_booking_id_from_t_id(t_id);
    var booked_offering_id = get_offering_id_from_t_id(t_id);
    var booked_provider_id = get_provider_id_from_t_id(t_id);

    var booked_org_id = -1;
    if (page_type == "bookings")
        booked_org_id = get_org_id_from_t_id(t_id);
    else if (page_type == "bookings_for_clinic") {
        var mySplitResult = String(t_id).split("_");
        booked_org_id = String(mySplitResult[1]);
    }

    var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

    var new_url = window.location.href;
    new_url = set_url_field(new_url, "edit_booking_id", booking_id);
    new_url = set_url_field(new_url, "org", booked_org_id);       // new_url = set_url_field(new_url, "org", org);
    new_url = set_url_field(new_url, "staff", booked_provider_id);  // new_url = set_url_field(new_url, "staff", staff);
    new_url = (booked_offering_id != "-1") ?
        set_url_field(new_url, "offering", booked_offering_id) :  // new_url = set_url_field(new_url, "offering", offering);
        new_url = remove_url_field(new_url, "offering");
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "is_collapsed", is_collapsed);
    //new_url = new_url.replace(/#/g, ''); // for some reason there is a hash in the url sometimes which prevents the next line from running
    window.location.href = new_url;
}


/* ------------------------ */
/* ----- cancel edit  ----- */
/* ------------------------ */

function cancel_edit() {

    var is_edit_booking_mode = getUrlVars()["edit_booking_id"] != undefined;
    var is_edit_day_mode = getUrlVars()["edit_date"] != undefined && getUrlVars()["edit_org"] != undefined && getUrlVars()["edit_provider"] != undefined;
    if (is_edit_booking_mode) {
        var new_url = window.location.href;
        new_url = remove_url_field(new_url, "edit_booking_id");
        new_url = remove_url_field(new_url, "offering");
        new_url = remove_url_field(new_url, "patient");
        new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
        new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
        window.location.href = new_url;
    }
    else if (is_edit_day_mode) {
        var new_url = window.location.href;
        new_url = remove_url_field(new_url, "edit_date");
        new_url = remove_url_field(new_url, "edit_org");
        new_url = remove_url_field(new_url, "edit_provider");
        new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
        new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
        window.location.href = new_url;

    }
}


/* ------------------------ */
/* ----- set edit day ----- */
/* ------------------------ */

function set_edit_day(t_id) {

    var mySplitResult = String(t_id).split("_");
    var org_id = String(mySplitResult[1]);
    var provider_id = String(mySplitResult[2]);
    var yr = String(mySplitResult[3]);
    var mo = String(mySplitResult[4]);
    var day = String(mySplitResult[5]);
    var date = yr + '_' + mo + '_' + day;

    var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

    var new_url = window.location.href;
    new_url = set_url_field(new_url, "edit_date", date);
    new_url = set_url_field(new_url, "edit_org", org_id);
    new_url = set_url_field(new_url, "edit_provider", provider_id);
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "is_collapsed", is_collapsed);

    window.location.href = new_url;
}


/* ------------------- */
/* --- add booking --- */
/* ------------------- */

function add_booking() {

    if (page_type == "bookings") {

        var booking_scr_type = getUrlVars()["type"];
        var url_booking_scr_type = "";
        if (booking_scr_type == "patient")
            url_booking_scr_type = "&patient=" + getUrlVars()["patient"];
        else if (booking_scr_type == "provider")
            url_booking_scr_type = "&provider=" + getUrlVars()["provider"];
        else if (booking_scr_type == "clinic")
            url_booking_scr_type = "&clinic=" + getUrlVars()["clinic"];
        else if (booking_scr_type == "agedcare")
            url_booking_scr_type = "&agedcare=" + getUrlVars()["agedcare"];
        else {
            alert("unknown url type");
            return;
        }

        var booking_date = document.getElementById('modalDate').firstChild.data;
        var start_datetime = booking_date + "_" + get_ddl_value("ddlModalStartHour") + get_ddl_value("ddlModalStartMinute");
        var end_datetime = booking_date + "_" + get_ddl_value("ddlModalEndHour") + get_ddl_value("ddlModalEndMinute");
        var org = get_ddl_value("ddlClinics");
        var staff = get_ddl_value("ddlProviders");
        var offering = get_ddl_value("ddlServices");
        var confirmed = document.getElementById("chkConfirmed").checked ? "1" : "0"; // = true

        if (String(start_datetime) >= String(end_datetime)) {
            alert("End time must be after start time");
            return;
        }

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString();

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        // ajax check
        if (ajax_has_clash_onetime(staff, org, 1, start_datetime, end_datetime, "-1", "34"))
            alert("Clash with existing booking. Please pick another time.");
        else
            window.location.href = "DoBooking.aspx?type=add&booking_scr_type=" + booking_scr_type + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + confirmed + "&page_return_date=" + url_date + "&booking_type_id=34&scroll_pos=" + document.getElementById('scrollValues').value;
    }
    else if (page_type == "bookings_for_clinic") {

        var booking_date = document.getElementById('modalDate').firstChild.data;
        var start_datetime = booking_date + "_" + get_ddl_value("ddlModalStartHour") + get_ddl_value("ddlModalStartMinute");
        var end_datetime = booking_date + "_" + get_ddl_value("ddlModalEndHour") + get_ddl_value("ddlModalEndMinute");
        var org = get_lbl_data('lblSelectedOrgID')
        var org_type = get_lbl_data('lblSelectedOrgTypeID');
        var staff = get_lbl_data("lblSelectedProviderID");
        var offering = (org_type == "139" || org_type == "367" || org_type == "372") ? "-1" : get_ddl_value("ddlServices");
        var confirmed = document.getElementById("chkConfirmed").checked;

        var patient_id = (org_type == "139" || org_type == "367" || org_type == "372") ? (document.getElementById('txtPatientID').value == '' ? "-1" : document.getElementById('txtPatientID').value) : document.getElementById('txtPatientID').value;

        if (String(start_datetime) >= String(end_datetime)) {
            alert("End time must be after start time");
            return;
        }

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString();

        var url_ndays = getUrlVars()["ndays"];
        if (url_ndays == undefined)
            url_ndays = "-1";

        var url_orgs = getUrlVars()["orgs"];

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";



        var ddlEveryNWeeks = get_ddl_value('ddlEveryNWeeks');
        var ddlOcurrences = get_ddl_value('ddlOcurrences');




        
        if (ddlEveryNWeeks == "1" && ddlOcurrences == "1") {

            // ajax check
            if (ajax_has_clash_onetime(staff, org, 1, start_datetime, end_datetime, "-1", "34")) {
                alert("Clash with existing booking. Please pick another time.");
                return;
            }

            ajax_booking_add(true, start_datetime, end_datetime, "34", patient_id, org, staff, offering, confirmed);
        }
        else { // if (ddlEveryNWeeks != "1" || ddlOcurrences != "1") {

            var d = convert_to_date_type(booking_date + '_0000');
            var weekday = d.getDay();
            var days = 
                (weekday == 0 ? "1" : "0") +
                (weekday == 1 ? "1" : "0") +
                (weekday == 2 ? "1" : "0") +
                (weekday == 3 ? "1" : "0") +
                (weekday == 4 ? "1" : "0") +
                (weekday == 5 ? "1" : "0") +
                (weekday == 6 ? "1" : "0");


            var start_datetime = booking_date + "_0000";

            var end_d = new Date(d.getTime());;
            var days_to_add = parseInt(ddlEveryNWeeks, 10) * (parseInt(ddlOcurrences, 10) - 1) * 7 + 1;
            end_d.setDate(d.getDate() + days_to_add);
            var end_datetime = end_d.getFullYear() + "_" + (end_d.getMonth() + 1).padLeft(2, '0') + "_" + end_d.getDate().padLeft(2, '0') + "_2359";

            var start_time = get_ddl_value("ddlModalStartHour") + get_ddl_value("ddlModalStartMinute");
            var end_time = get_ddl_value("ddlModalEndHour") + get_ddl_value("ddlModalEndMinute");


            // ajax check 
            var clashes_cust_bkgs = ajax_get_clashes_recurring(staff, 0, start_datetime, end_datetime, days, start_time, end_time, ddlEveryNWeeks, "-1", "34", false, true);
            if (clashes_cust_bkgs.length > 0) {
                var tbl = "";
                for (var i = 0; i < clashes_cust_bkgs.length; i++)
                    tbl += (i == 0 ? "" : "<br />") + clashes_cust_bkgs[i];

                document.getElementById('spn_clash_bookings_bk').innerHTML = tbl;
                document.getElementById('tr_clash_bookings_bk').className = "";

                // if height not set for this div, change class, else add 80 to the div height
                if (document.getElementById('spnModalPage').style.height == "")
                    document.getElementById('spnModalPage').className = "modal_Big";
                else
                {
                    var curHeight = document.getElementById('spnModalPage').style.height;
                    curHeight = curHeight.substr(0, curHeight.length - 2); // remove 'px'
                    var newHeight = String(parseInt(curHeight, 10) + 80) + "px";
                    document.getElementById('spnModalPage').style.height = newHeight;
                }

                return;
            }

            // ajax check
            var clashes_unavail_bkgs = ajax_get_clashes_recurring(staff, org, start_datetime, end_datetime, days, start_time, end_time, ddlEveryNWeeks, "-1", "34", true, false);
            if (clashes_unavail_bkgs.length > 0) {
                var tbl = "";
                for (var i = 0; i < clashes_unavail_bkgs.length; i++)
                    tbl += (i == 0 ? "" : "<br />") + clashes_unavail_bkgs[i];

                document.getElementById('spn_clash_bookings_bk').innerHTML = tbl;
                document.getElementById('tr_clash_bookings_bk').className = "";

                // if height not set for this div, change class, else add 80 to the div height
                if (document.getElementById('spnModalPage').style.height == "")
                    document.getElementById('spnModalPage').className = "modal_Big";
                else {
                    var curHeight = document.getElementById('spnModalPage').style.height;
                    curHeight = curHeight.substr(0, curHeight.length - 2); // remove 'px'
                    var newHeight = String(parseInt(curHeight, 10) + 80) + "px";
                    document.getElementById('spnModalPage').style.height = newHeight;
                }

                return;
            }

            ajax_booking_add_recurring(true, start_datetime, end_datetime, start_time, end_time, "34", patient_id, org, staff, offering, confirmed, -1, days, false, ddlEveryNWeeks);
        }


        //ajax_booking_add(true, start_datetime, end_datetime, "34", patient_id, org, staff, offering, confirmed);
        //window.location.href = "DoBooking.aspx?return_page=BookingsForClinic&type=add&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&patient=" + patient_id + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + confirmed + "&page_return_date=" + url_date + "&ndays=" + url_ndays + "&orgs=" + url_orgs + "&booking_type_id=34&scroll_pos=" + document.getElementById('scrollValues').value;
    }

}


/* --------------------------------- */
/* --- change full days bookings --- */
/* --------------------------------- */

function change_full_days_bookings(t_id) {

    //td_2301_3_2012_06_27
    var t_id_SplitResult = String(t_id).split("_");
    var las_pos = t_id_SplitResult.length - 1;
    var yr = String(t_id_SplitResult[las_pos - 2]);
    var mo = String(t_id_SplitResult[las_pos - 1]);
    var day = String(t_id_SplitResult[las_pos]);


    var old_edit_date = getUrlVars()["edit_date"];
    var old_edit_org = getUrlVars()["edit_org"];
    var old_edit_provider = getUrlVars()["edit_provider"];

    var new_edit_date = yr + "_" + mo + "_" + day;
    var new_edit_org = String(t_id_SplitResult[1]);
    var new_edit_provider = String(t_id_SplitResult[2]);




    var modalPopupPageLoadingMessage_Text = document.getElementById("modalPopupPageLoadingMessage_Text").innerHTML;
    document.getElementById("modalPopupPageLoadingMessage_Text").innerHTML = "Processing. This may take up to a minute. Please wait....";
    reveal_modal('modalPopupPageLoadingMessage');

    // ajax check
    var has_clashes = ajax_has_clash_full_day_move(old_edit_date, old_edit_org, old_edit_provider, new_edit_date, new_edit_org, new_edit_provider);

    if (has_clashes) {
        document.getElementById("modalPopupPageLoadingMessage_Text").innerHTML = modalPopupPageLoadingMessage_Text;
        hide_modal('modalPopupPageLoadingMessage');

        alert("There are clashes that need to be moved first.");
    }
    else {
        ajax_booking_edit_full_day(old_edit_date, old_edit_org, old_edit_provider, new_edit_date, new_edit_org, new_edit_provider);

        document.getElementById("modalPopupPageLoadingMessage_Text").innerHTML = modalPopupPageLoadingMessage_Text;
        hide_modal('modalPopupPageLoadingMessage');

        var new_url = window.location.href;
        new_url = remove_url_field(new_url, "edit_date");
        new_url = remove_url_field(new_url, "edit_org");
        new_url = remove_url_field(new_url, "edit_provider");
        new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
        new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
        window.location.href = new_url;

    }

}


/* ------------------------------------------------------------------ */
/* --- add full day booking, add_recurring_unavailability_booking --- */
/* ------------------------------------------------------------------ */

function add_fulldays_booking(t_id) {  // argument t_id only used for BookingsV2.aspx

    if (page_type == "bookings") {

        var booking_scr_type = getUrlVars()["type"];
        var url_booking_scr_type = "";
        if (booking_scr_type == "patient")
            url_booking_scr_type = "&patient=" + getUrlVars()["patient"];
        else if (booking_scr_type == "provider")
            url_booking_scr_type = "&provider=" + getUrlVars()["provider"];
        else if (booking_scr_type == "clinic")
            url_booking_scr_type = "&clinic=" + getUrlVars()["clinic"];
        else if (booking_scr_type == "agedcare")
            url_booking_scr_type = "&agedcare=" + getUrlVars()["agedcare"];
        else {
            alert("unknown url type");
            return;
        }

        if (booking_scr_type == "patient") {
            alert("An error has occurred. Plase contact the system administrator");
            return;
        }

        var start_datetime = get_fulldays_date('txtModalPopupBookFullDays_StartDate', 0);
        var end_datetime = get_fulldays_date('txtModalPopupBookFullDays_EndDate', 1);  // add day because inc 19th will mean we make end date 20th at 00:00

        var org = (booking_scr_type != "clinic" && booking_scr_type != "agedcare") ? "0" : get_ddl_value("ddlClinics");
        var staff = (booking_scr_type != "provider") ? "-1" : get_ddl_value("ddlProviders");
        var offering = -1;
        var confirmed = "1";
        var url_patient = -1;
        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString();


        var booking_type_id = (booking_scr_type == "clinic" || booking_scr_type == "agedcare") ? "340" : "342";

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        // -- dont check for clashes for full day unavailabilities
        // yes check for them!

        // ajax check
        if (ajax_has_clash_onetime(staff, org, 0, start_datetime, end_datetime, "-1", booking_type_id))
            alert("Please move or delete existing bookings first.");
        else
            window.location.href = "DoBooking.aspx?type=add&booking_scr_type=" + booking_scr_type + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + confirmed + "&page_return_date=" + url_date + "&booking_type_id=" + booking_type_id + "&scroll_pos=" + document.getElementById('scrollValues').value;
    }
    else if (page_type == "bookings_for_clinic") {

        //td_2301_3_2012_06_27
        var t_id_SplitResult = String(t_id).split("_");
        var las_pos = t_id_SplitResult.length - 1;
        var yr = String(t_id_SplitResult[las_pos - 2]);
        var mo = String(t_id_SplitResult[las_pos - 1]);
        var day = String(t_id_SplitResult[las_pos]);


        var time_txt = yr + "_" + mo + "_" + day + "_0000";
        var start_datetime = get_fulldays_date_txtinput(time_txt, 0);
        var end_datetime = get_fulldays_date_txtinput(time_txt, 1);  // add day because inc 19th will mean we make end date 20th at 00:00

        var org = String(t_id_SplitResult[1]);
        var staff = String(t_id_SplitResult[2]);
        var offering = "-1";
        var confirmed = "1";
        var url_patient = "-1";

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString();

        var url_ndays = getUrlVars()["ndays"];
        if (url_ndays == undefined)
            url_ndays = "-1";

        var url_orgs = getUrlVars()["orgs"];

        var booking_type_id = "341";

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        // ajax check
        if (ajax_has_clash_onetime(staff, org, 0, start_datetime, end_datetime, "-1", booking_type_id))
            alert("Please move or delete existing bookings first.");
        else {
            //window.location.href = "DoBooking.aspx?return_page=BookingsForClinic&type=add&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + confirmed + "&page_return_date=" + url_date + "&ndays=" + url_ndays + "&orgs=" + url_orgs + "&booking_type_id=" + booking_type_id + "&scroll_pos=" + document.getElementById('scrollValues').value;
            ajax_booking_add(true, start_datetime, end_datetime, booking_type_id, "-1", org, staff, offering, true);
        }
    }
}


function add_recurring_unavailability_booking() {

    if (page_type == "bookings") {



    }
    else if (page_type == "bookings_for_clinic") {

        var isPaid = document.getElementById('modalPopupUnavailableRecurring_IsPaid').value == "1";

        var only_this_org = document.getElementById('chkModalPopupUnavailableRecurringOnlyThisOrg').checked;
        var org = get_lbl_data('lblSelectedOrgID');
        var staff = get_lbl_data('lblSelectedProviderID');
        var offering = "-1";
        var confirmed = "1";
        var patient = "-1";

        var booking_type_id = only_this_org ? "341" : "342";

        var days = (chkModalPopupUnavailableRecurring_Sunday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Monday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Tuesday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Wednesday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Thursday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Friday.checked ? "1" : "0") +
                    (chkModalPopupUnavailableRecurring_Saturday.checked ? "1" : "0");

        // need to make sure at least one day is selected
        if (!chkModalPopupUnavailableRecurring_Sunday.checked &&
            !chkModalPopupUnavailableRecurring_Monday.checked &&
            !chkModalPopupUnavailableRecurring_Tuesday.checked &&
            !chkModalPopupUnavailableRecurring_Wednesday.checked &&
            !chkModalPopupUnavailableRecurring_Thursday.checked &&
            !chkModalPopupUnavailableRecurring_Friday.checked &&
            !chkModalPopupUnavailableRecurring_Saturday.checked) {
            alert("At least one day must be selected");
            return;
        }

        var allDay = document.getElementById('chkModalPopupUnavailableRecurring_AllDay').checked;
        var start_time = allDay ? "0000" : get_ddl_value("ddlModalPopupUnavailableRecurringModalStartHour") + get_ddl_value("ddlModalPopupUnavailableRecurringModalStartMinute");
        var end_time = allDay ? "2359" : get_ddl_value("ddlModalPopupUnavailableRecurringModalEndHour") + get_ddl_value("ddlModalPopupUnavailableRecurringModalEndMinute");

        // need to check that 2nd time is after first time
        if (!allDay && (String(start_time) >= String(end_time))) {
            alert("End time must be after start time");
            return;
        }

        // need to check start date and end date are valid dates  (make another method to check this)    is_valid_date(txt_date)
        var start_date_text = document.getElementById('txtModalPopupUnavailableRecurring_StartDate').value;
        var end_date_text = document.getElementById('txtModalPopupUnavailableRecurring_EndDate').value;
        var valid_date_regex = /^\d{2}\-\d{2}\-\d{4}$/;
        var valid_start_date = valid_date_regex.test(start_date_text);
        var valid_end_date = end_date_text.length == 0 || valid_date_regex.test(end_date_text);
        if (!valid_start_date) {
            alert("Invalid start date - Must be in the format dd-mm-yyyy");
            return;
        }
        if (!valid_end_date) {
            alert("Invalid end date - Must be in the format dd-mm-yyyy");
            return;
        }

        var start_datetime = get_fulldays_date('txtModalPopupUnavailableRecurring_StartDate', 0);
        var end_datetime = end_date_text.length == 0 ? "NULL" : get_fulldays_date('txtModalPopupUnavailableRecurring_EndDate', 0);
        var same_start_and_end_date = (start_datetime == end_datetime);

        var ddl_every_n_weeks = document.getElementById('ddlUnavailableEveryNWeeks');
        var every_n_weeks = ddl_every_n_weeks.options[ddl_every_n_weeks.selectedIndex].value


        // need to check that IF end date not null ... check 3nd date is after first date
        if (end_date_text.length > 0) {
            if (String(start_datetime) > String(end_datetime)) {
                alert("End date must be after start date");
                return;
            }

            // add one day to the end date because 7th-8th will want 8th included, so make it 7th 00:00 to 9th 00:00
            //end_datetime = get_fulldays_date('txtModalPopupUnavailableRecurring_EndDate', 1);
        }




        if (!same_start_and_end_date &&
            parseInt(every_n_weeks) > 1 &&
            document.getElementById('booking_sequence_type_series').checked == true) {

            alert("For bookings less frequently than every 1 week, you must select \"Create seperate unavailabilities\"." +
                  ((end_date_text.length > 0) ? "" : "\r\n" +
                  "\r\n" +
                  "You also must set an end date when creating seperate unavailabilities.")
                 );
            return;
        }
        if (!same_start_and_end_date &&
            parseInt(every_n_weeks) == 1 &&
            document.getElementById('booking_sequence_type_seperate').checked == false &&
            document.getElementById('booking_sequence_type_series').checked == false) {

            alert("Please select either \"Create seperate unavailabilities\" or \"Create single series\"" + "\r\n" +
            "\r\n" +
            "Creating seperate unavailabilities - once created, deleting one of those day's unavailability will not remove other unavailabilities" + "\r\n" +
            "Creating as a series - once created, deleting any instance of the series will remove all instances of this series");
            return;
        }
        if (document.getElementById('booking_sequence_type_seperate').checked == true && end_date_text.length == 0) {

            alert("Can not select \"Create seperate unavailabilities\" without an end date" + "\r\n" +
            "\r\n" +
            "Either add an end date, or change to \"Create single series\"");
            return;
        }

        var create_as_series = !same_start_and_end_date && document.getElementById('booking_sequence_type_series').checked ? true : false;
        if (parseInt(every_n_weeks) > 1) create_as_series = false;

        var unavailability_reason_id = "-1";
        if (!is_hiden_modal('ddlProvUnavailabilityReason'))
            unavailability_reason_id = get_ddl_value("ddlProvUnavailabilityReason");
        if (!is_hiden_modal('ddlOrgUnavailabilityReason'))
            unavailability_reason_id = get_ddl_value("ddlOrgUnavailabilityReason");


        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString();

        var url_ndays = getUrlVars()["ndays"];
        if (url_ndays == undefined)
            url_ndays = "-1";

        var url_orgs = getUrlVars()["orgs"];

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";


        // ajax check clashes

        var clashes_cust_bkgs    = ajax_get_clashes_recurring(staff, only_this_org ? org : "0", start_datetime, end_datetime, days, start_time, end_time, every_n_weeks, "-1", booking_type_id, false, true);
        if (clashes_cust_bkgs.length > 0) {
            var tbl = "";
            for (var i = 0; i < clashes_cust_bkgs.length; i++)
                tbl += (i == 0 ? "" : "<br />") + clashes_cust_bkgs[i];
            document.getElementById('spn_clash_bookings').innerHTML = tbl;
            document.getElementById('tr_clash_bookings').className = "";
            document.getElementById('spnModalPopupUnavailableRecurring').className = "modalPopupUnavailableRecurring_Big";
            if (String(document.getElementById('spnModalPopupUnavailableRecurring').style.height).length > 0) {
                var curHeightPx = document.getElementById('spnModalPopupUnavailableRecurring').style.height;
                var curHeight = curHeightPx.substr(0, curHeightPx.length - 2);
                var newHeight = parseInt(parseInt(curHeight) + 50);
                var newTop = parseInt(newHeight / 2);
                document.getElementById('spnModalPopupUnavailableRecurring').style.height = newHeight + "px";
                document.getElementById('spnModalPopupUnavailableRecurring').style.top = "-" + newTop + "px";
            }
            return;
        }

        var clashes_unavail_bkgs = ajax_get_clashes_recurring(staff, only_this_org ? org : "0", start_datetime, end_datetime, days, start_time, end_time, every_n_weeks, "-1", booking_type_id, true, false);
        if (clashes_unavail_bkgs.length > 0) {
            var tbl = "";
            for (var i = 0; i < clashes_unavail_bkgs.length; i++)
                tbl += (i == 0 ? "" : "<br />") + clashes_unavail_bkgs[i];
            document.getElementById('spn_clash_bookings').innerHTML = tbl;
            document.getElementById('tr_clash_bookings').className = "";
            if (String(document.getElementById('spnModalPopupUnavailableRecurring').style.height).length > 0) {
                var curHeightPx = document.getElementById('spnModalPopupUnavailableRecurring').style.height;
                var curHeight = curHeightPx.substr(0, curHeightPx.length - 2);
                var newHeight = parseInt(parseInt(curHeight) + 50);
                var newTop = parseInt(newHeight / 2);
                document.getElementById('spnModalPopupUnavailableRecurring').style.height = newHeight + "px";
                document.getElementById('spnModalPopupUnavailableRecurring').style.top = "-" + newTop + "px";
            }
            document.getElementById('spnModalPopupUnavailableRecurring').className = "modalPopupUnavailableRecurring_Big";
            return;
        }

        ajax_booking_add_recurring(true, start_datetime, end_datetime, start_time, end_time, isPaid ? "36" : booking_type_id, "-1", org, staff, offering, confirmed, unavailability_reason_id, days, create_as_series, every_n_weeks);

    }
}


function get_fulldays_date(txtBoxID, days_to_add) {

    var time_split = document.getElementById(txtBoxID).value.split('-');
    var time_txt = String(time_split[2]) + "_" + String(time_split[1]) + "_" + String(time_split[0]) + "_0000";

    return get_fulldays_date_txtinput(time_txt, days_to_add);
}
function get_fulldays_date_txtinput(time_txt, days_to_add) {

    var time = convert_to_date_type(time_txt);

    if (days_to_add > 0)
        time.setDate(time.getDate() + days_to_add);

    var month = String(time.getMonth() + 1);
    if (month.length < 2)
        month = "0" + month;

    var day = String(time.getDate());
    if (day.length < 2)
        day = "0" + day;

    return time.getFullYear() + "_" + month + "_" + day + "_0000";
}


/* ---------------------- */
/* --- update booking --- */
/* ---------------------- */

function update_booking() {

    if (page_type == "bookings") {

        var booking_scr_type = getUrlVars()["type"];
        var url_booking_scr_type = "";
        if (booking_scr_type == "patient")
            url_booking_scr_type = "&patient=" + getUrlVars()["patient"];
        else if (booking_scr_type == "provider")
            url_booking_scr_type = "&provider=" + getUrlVars()["provider"];
        else if (booking_scr_type == "clinic")
            url_booking_scr_type = "&clinic=" + getUrlVars()["clinic"];
        else if (booking_scr_type == "agedcare")
            url_booking_scr_type = "&agedcare=" + getUrlVars()["agedcare"];
        else {
            alert("unknown url type");
            return;
        }

        var booking_date = document.getElementById('modalDate').firstChild.data;
        var start_datetime = booking_date + "_" + get_ddl_value("ddlModalStartHour") + get_ddl_value("ddlModalStartMinute");
        var end_datetime = booking_date + "_" + get_ddl_value("ddlModalEndHour") + get_ddl_value("ddlModalEndMinute");
        var org = get_ddl_value("ddlClinics");
        var staff = get_ddl_value("ddlProviders");
        var offering = get_ddl_value("ddlServices");
        var confirmed = document.getElementById("chkConfirmed").checked ? "1" : "0"; // = true

        if (start_datetime == end_datetime) {
            alert('start time can not be the same as end time');
            return;
        }

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString()

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        var edit_reason = document.getElementById("txtComment").value;


        var edit_booking_id = getUrlVars()["edit_booking_id"];

        // ajax check
        if (ajax_has_clash_onetime(staff, org, 1, start_datetime, end_datetime, edit_booking_id, "-1"))
            alert("Clash with existing booking. Please pick another time.");
        else
            window.location.href = "DoBooking.aspx?type=edit&booking_scr_type=" + booking_scr_type + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&edit_booking_id=" + edit_booking_id + "&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + confirmed + "&page_return_date=" + url_date + "&edit_reason=" + edit_reason + "&scroll_pos=" + document.getElementById('scrollValues').value;
    }
    else if (page_type == "bookings_for_clinic") {

        var booking_date = document.getElementById('modalDate').firstChild.data;
        var start_datetime = booking_date + "_" + get_ddl_value("ddlModalStartHour") + get_ddl_value("ddlModalStartMinute");
        var end_datetime = booking_date + "_" + get_ddl_value("ddlModalEndHour") + get_ddl_value("ddlModalEndMinute");

        var org = get_lbl_data('lblSelectedOrgID')
        var org_type = get_lbl_data('lblSelectedOrgTypeID');
        var staff = get_lbl_data("lblSelectedProviderID");
        var offering = (org_type == "139" || org_type == "367" || org_type == "372") ? "-1" : get_ddl_value("ddlServices");
        var confirmed = document.getElementById("chkConfirmed").checked;

        var patient_id = (org_type == "139" || org_type == "367" || org_type == "372") ? (document.getElementById('txtPatientID').value == '' ? "-1" : document.getElementById('txtPatientID').value) : document.getElementById('txtPatientID').value;

        if (String(start_datetime) >= String(end_datetime)) {
            alert("End time must be after start time");
            return;
        }

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString()

        var url_ndays = getUrlVars()["ndays"];
        if (url_ndays == undefined)
            url_ndays = "-1";

        var url_orgs = getUrlVars()["orgs"];

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        //var edit_reason = document.getElementById("txtComment").value;
        var edit_reason_id = get_ddl_value("ddlBookingMovementReason");

        var edit_booking_id = getUrlVars()["edit_booking_id"];

        // ajax check
        if (ajax_has_clash_onetime(staff, 0, 1, start_datetime, end_datetime, edit_booking_id, "-1"))
            alert("Clash with existing booking. Please pick another time.");
        else {
            //window.location.href = "DoBooking.aspx?return_page=BookingsForClinic&type=edit&edit_booking_id=" + edit_booking_id + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + "&start_datetime=" + start_datetime + "&end_datetime=" + end_datetime + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&confirmed=" + (confirmed ? "1" : "0") + "&page_return_date=" + url_date + "&edit_reason_id=" + edit_reason_id + "&ndays=" + url_ndays + "&orgs=" + url_orgs + "&scroll_pos=" + document.getElementById('scrollValues').value;
            ajax_booking_edit(true, edit_booking_id, start_datetime, end_datetime, patient_id, org, staff, offering, confirmed, edit_reason_id);

            var new_url = window.location.href;
            new_url = remove_url_field(new_url, "patient");
            new_url = remove_url_field(new_url, "offering");
            new_url = remove_url_field(new_url, "edit_booking_id");
            new_url = remove_url_field(new_url, "org");
            new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
            new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
            window.location.href = new_url;
        }
    }
}


/* ------------------------------------- */
/* --- delete/confirm/cancel booking --- */
/* ------------------------------------- */

function delete_confirm_cancel_booking(t_id, type) {

    if (page_type == "bookings") {

        var booking_scr_type = getUrlVars()["type"];
        var url_booking_scr_type = "";
        if (booking_scr_type == "patient")
            url_booking_scr_type = "&patient=" + getUrlVars()["patient"];
        else if (booking_scr_type == "provider")
            url_booking_scr_type = "&provider=" + getUrlVars()["provider"];
        else if (booking_scr_type == "clinic")
            url_booking_scr_type = "&clinic=" + getUrlVars()["clinic"];
        else if (booking_scr_type == "agedcare")
            url_booking_scr_type = "&agedcare=" + getUrlVars()["agedcare"];
        else {
            alert("unknown url type");
            return;
        }


        var booking_id = get_booking_id_from_t_id(t_id);
        var org = get_ddl_value("ddlClinics");
        var staff = get_ddl_value("ddlProviders");
        var offering = get_ddl_value("ddlServices");

        var url_date = getUrlVars()["date"];
        if (url_date == undefined)
            url_date = getTodaysDateString()

        var show_unavailable_staff = getUrlVars()["show_unavailable_staff"];
        if (show_unavailable_staff == undefined)
            show_unavailable_staff = "-1";

        var is_collapsed = document.getElementById("chkShowDetails").checked ? "0" : "1";

        if (String(type) == "delete") {
            //if (confirm("Are you sure you want to delete this booking?"))
                window.location.href = "DoBooking.aspx?type=delete&booking_scr_type=" + booking_scr_type + "&booking_id=" + booking_id + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&page_return_date=" + url_date + "&scroll_pos=" + document.getElementById('scrollValues').value;
        }
        if (String(type) == "cancel") {
            //if (confirm("Are you sure you want to cancel this booking?"))
                window.location.href = "DoBooking.aspx?type=cancel&booking_scr_type=" + booking_scr_type + "&booking_id=" + booking_id + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&page_return_date=" + url_date + "&scroll_pos=" + document.getElementById('scrollValues').value;
        }
        if (String(type) == "deceased") {
            //if (confirm("Are you sure you want to set this patient as deceased?"))
                window.location.href = "DoBooking.aspx?type=deceased&booking_scr_type=" + booking_scr_type + "&booking_id=" + booking_id + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&page_return_date=" + url_date + "&scroll_pos=" + document.getElementById('scrollValues').value;
        }
        if (String(type) == "confirm") {
            window.location.href = "DoBooking.aspx?type=confirm&booking_scr_type=" + booking_scr_type + "&booking_id=" + booking_id + "&show_unavailable_staff=" + show_unavailable_staff + "&is_collapsed=" + is_collapsed + url_booking_scr_type + "&org=" + org + "&staff=" + staff + "&offering=" + offering + "&page_return_date=" + url_date + "&scroll_pos=" + document.getElementById('scrollValues').value;
        }
    }
    else if (page_type == "bookings_for_clinic") {

        var split_td = String(t_id).split("_");
        var booking_id = get_booking_id_from_t_id(t_id);

        if (String(type) == "delete") {
            //if (confirm("Are you sure you want to delete this booking?"))
                ajax_booking_confirm_delete_cancel(type, booking_id);
        }
        else if (String(type) == "cancel") {
            //if (confirm("Are you sure you want to cancel this booking?"))
                ajax_booking_confirm_delete_cancel(type, booking_id);
        }
        else if (String(type) == "deceased") {
            //if (confirm("Are you sure you want to set this patient as deceased?"))
                ajax_booking_confirm_delete_cancel(type, booking_id);
        }
        else if (String(type) == "                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ") {
            ajax_booking_confirm_delete_cancel(type, booking_id);
        }
    }

}


/* --------------------------------------- */
/* --- ajax booking (add/edit) methods --- */
/* --------------------------------------- */

function ajax_has_clash_onetime(staff_id, org_id, all_orgs, start_datetime, end_datetime, edit_booking_id, booking_type_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("ocurrence", "onetime");
    url_params[1] = create_url_param("staff", staff_id);
    url_params[2] = create_url_param("org", org_id);
    url_params[3] = create_url_param("all_orgs", all_orgs);
    url_params[4] = create_url_param("start_datetime", start_datetime);
    url_params[5] = create_url_param("end_datetime", end_datetime);
    url_params[6] = create_url_param("edit_booking_id", edit_booking_id);
    url_params[7] = create_url_param("booking_type_id", booking_type_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingCheckClash.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result == "1")
        return true;
    else if (result == "0")
        return false;
    else {
        alert(result);
        return true;
    }
}
function ajax_has_clash_recurring(staff_id, org_id, start_datetime, end_datetime, days, start_time, end_time, edit_booking_id, booking_type_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("ocurrence", "recurring");
    url_params[1] = create_url_param("staff", staff_id);
    url_params[2] = create_url_param("org", org_id);
    url_params[3] = create_url_param("start_datetime", start_datetime);
    url_params[4] = create_url_param("end_datetime", end_datetime);
    url_params[5] = create_url_param("days", days);
    url_params[6] = create_url_param("start_time", start_time);
    url_params[7] = create_url_param("end_time", end_time);
    url_params[8] = create_url_param("edit_booking_id", edit_booking_id);
    url_params[9] = create_url_param("booking_type_id", booking_type_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingCheckClash.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result == "1")
        return true;
    else if (result == "0")
        return false;
    else {
        alert(result);
        return true;
    }
}
function ajax_has_clash_full_day_move(old_edit_date, old_edit_org, old_edit_provider, new_edit_date, new_edit_org, new_edit_provider) {

    var url_params = new Array();
    url_params[0] = create_url_param("ocurrence", "fullday");
    url_params[1] = create_url_param("editfullday_old_date", old_edit_date);
    url_params[2] = create_url_param("editfullday_old_org", old_edit_org);
    url_params[3] = create_url_param("editfullday_old_provider", old_edit_provider);
    url_params[4] = create_url_param("editfullday_new_date", new_edit_date);
    url_params[5] = create_url_param("editfullday_new_org", new_edit_org);
    url_params[6] = create_url_param("editfullday_new_provider", new_edit_provider);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingCheckClash.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result == "1")
        return true;
    else if (result == "0")
        return false;
    else {
        alert(result);
        return true;
    }

}

function ajax_get_clashes_onetime(staff_id, org_id, start_datetime, end_datetime, edit_booking_id, booking_type_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("ocurrence", "onetime");
    url_params[1] = create_url_param("staff", staff_id);
    url_params[2] = create_url_param("org", org_id);
    url_params[3] = create_url_param("start_datetime", start_datetime);
    url_params[4] = create_url_param("end_datetime", end_datetime);
    url_params[5] = create_url_param("edit_booking_id", edit_booking_id);
    url_params[6] = create_url_param("booking_type_id", booking_type_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingGetClashes.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result.length == 0 ? new Array() : result.split("<>");
}
function ajax_get_clashes_recurring(staff_id, org_id, start_datetime, end_datetime, days, start_time, end_time, every_n_weeks, edit_booking_id, booking_type_id, inc_unavailable_bkgs, inc_customer_bkgs) {

    var url_params = new Array();
    url_params[0]  = create_url_param("ocurrence", "recurring");
    url_params[1]  = create_url_param("staff", staff_id);
    url_params[2]  = create_url_param("org", org_id);
    url_params[3]  = create_url_param("start_datetime", start_datetime);
    url_params[4]  = create_url_param("end_datetime", end_datetime);
    url_params[5]  = create_url_param("days", days);
    url_params[6]  = create_url_param("start_time", start_time);
    url_params[7]  = create_url_param("end_time", end_time);
    url_params[8]  = create_url_param("every_n_weeks", every_n_weeks);
    url_params[9]  = create_url_param("edit_booking_id", edit_booking_id);
    url_params[10] = create_url_param("booking_type_id", booking_type_id);
    url_params[11] = create_url_param("inc_unavailable_bkgs", inc_unavailable_bkgs ? "1" : "0");
    url_params[12] = create_url_param("inc_customer_bkgs", inc_customer_bkgs ? "1" : "0");

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingGetClashes.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result.length == 0 ? new Array() : result.split("<>");
}

function ajax_booking_has_next_after_today(booking_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "booking");
    url_params[1] = create_url_param("booking_id", booking_id);
    url_params[2] = create_url_param("inc_completed", "0");

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxBookingHasNextAfterToday.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    if (result == "1")
        return true;
    else if (result == "0")
        return false;
    else {
        alert(result);
        return false;
    }
}

function ajax_get_staff_name(staff_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("staff", staff_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxGetStaffName.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result;
}
function ajax_get_offering_max_nbr_limit(patient_id, offering_id, booking_datetime) {

    var url_params = new Array();
    url_params[0] = create_url_param("patient_id", patient_id);
    url_params[1] = create_url_param("offering_id", offering_id);
    url_params[2] = create_url_param("booking_datetime", booking_datetime);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxCheckOfferingMaxNbrLimit.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "SessionTimedOutException")
        reload_booking_page();

    return result;
}

function ajax_booking_add(check_clash_all_orgs, start_datetime, end_datetime, booking_type_id, patient_id, org_id, staff_id, offering_id, confirmed) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "add");
    url_params[1] = create_url_param("check_clash_all_orgs", check_clash_all_orgs ? "1" : "0"); // BookingsForClinic = 1, Bookings = 0
    url_params[2] = create_url_param("start_datetime", start_datetime);
    url_params[3] = create_url_param("end_datetime", end_datetime);
    url_params[4] = create_url_param("booking_type_id", booking_type_id);
    url_params[5] = create_url_param("patient_id", patient_id);
    url_params[6] = create_url_param("org_id", org_id);
    url_params[7] = create_url_param("staff_id", staff_id);
    url_params[8] = create_url_param("offering_id", offering_id);
    url_params[9] = create_url_param("confirmed", confirmed ? "1" : "0");

    ajax_booking_run_parameters(url_params, true);
}
function ajax_booking_add_recurring(check_clash_all_orgs, start_datetime, end_datetime, start_time, end_time, booking_type_id, patient_id, org_id, staff_id, offering_id, confirmed, unavailability_reason_id, days, create_as_series, every_n_weeks) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "addrecurring");
    url_params[1] = create_url_param("check_clash_all_orgs", check_clash_all_orgs ? "1" : "0"); // BookingsForClinic = 1, Bookings = 0
    url_params[2] = create_url_param("start_datetime", start_datetime);
    url_params[3] = create_url_param("end_datetime", end_datetime);
    url_params[4] = create_url_param("start_time", start_time);
    url_params[5] = create_url_param("end_time", end_time);
    url_params[6] = create_url_param("booking_type_id", booking_type_id);
    url_params[7] = create_url_param("patient_id", patient_id);
    url_params[8] = create_url_param("org_id", org_id);
    url_params[9] = create_url_param("staff_id", staff_id);
    url_params[10] = create_url_param("offering_id", offering_id);
    url_params[11] = create_url_param("confirmed", confirmed ? "1" : "0");
    url_params[12] = create_url_param("unavailability_reason_id", unavailability_reason_id);
    url_params[13] = create_url_param("days", days);
    url_params[14] = create_url_param("create_as_series", create_as_series ? "1" : "0");
    url_params[15] = create_url_param("every_n_weeks", every_n_weeks);

    ajax_booking_run_parameters(url_params, true);
}
function ajax_booking_edit(check_clash_all_orgs, booking_id, start_datetime, end_datetime, patient_id, org_id, staff_id, offering_id, confirmed, edit_reason_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "edit");
    url_params[1] = create_url_param("check_clash_all_orgs", check_clash_all_orgs ? "1" : "0"); // BookingsForClinic = 1, Bookings = 0
    url_params[2] = create_url_param("booking_id", booking_id);
    url_params[3] = create_url_param("start_datetime", start_datetime);
    url_params[4] = create_url_param("end_datetime", end_datetime);
    url_params[5] = create_url_param("patient_id", patient_id);
    url_params[6] = create_url_param("org_id", org_id);
    url_params[7] = create_url_param("staff_id", staff_id);
    url_params[8] = create_url_param("offering_id", offering_id);
    url_params[9] = create_url_param("confirmed", confirmed ? "1" : "0");
    url_params[10] = create_url_param("edit_reason_id", edit_reason_id);

    ajax_booking_run_parameters(url_params, false);
}
function ajax_booking_edit_full_day(old_edit_date, old_edit_org, old_edit_provider, new_edit_date, new_edit_org, new_edit_provider) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "editfullday");
    url_params[1] = create_url_param("editfullday_old_date", old_edit_date);
    url_params[2] = create_url_param("editfullday_old_org", old_edit_org);
    url_params[3] = create_url_param("editfullday_old_provider", old_edit_provider);
    url_params[4] = create_url_param("editfullday_new_date", new_edit_date);
    url_params[5] = create_url_param("editfullday_new_org", new_edit_org);
    url_params[6] = create_url_param("editfullday_new_provider", new_edit_provider);

    ajax_booking_run_parameters(url_params, false);
}
function ajax_booking_confirm_delete_cancel(type, booking_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", type);  // types: "delete", "confirm", "unconfirm", "arrived", "unarrived", "cancel", "deceased"
    url_params[1] = create_url_param("booking_id", booking_id);

    ajax_booking_run_parameters(url_params, true);
}
function ajax_booking_can_reverse(booking_id) {

    var url_params = new Array();
    url_params[0] = create_url_param("type", "canreverse");
    url_params[1] = create_url_param("booking_id", booking_id);

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxDoBooking.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);
    if (result == "0") {                // 0 = successful
        return "";
    }
    else if (result == "1") {           // 1 = session timed out
        reload_booking_page();
    }
    else if (result.startsWith("2:")) { // 2 = custom message for user
        return result.substring(2);
    }
    else if (result.startsWith("3:")) { // 3 = code error
        return result.substring(2);
    }
    else {                              // unknown exception
        return result;
    }
}

function ajax_booking_run_parameters(url_params, redirect_back_on_success) {

    var xmlhttp = (window.XMLHttpRequest) ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", "/AJAX/AjaxDoBooking.aspx?" + create_url_params(url_params), false);
    xmlhttp.send();

    var result = String(xmlhttp.responseText);


    if (result == "0") {                // 0 = successful
        if (redirect_back_on_success) {

            var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
            var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

            var new_url = window.location.href;
            new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
            new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");

            if (is_clinic)
                new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));
            window.location.href = new_url;
        }
    }
    else if (result == "1") {           // 1 = session timed out
        reload_booking_page();
    }
    else if (result.startsWith("2:")) { // 2 = custom message for user
        alert(result.substring(2));
    }
    else if (result.startsWith("3:")) { // 3 = code error
        alert(result.substring(2));
    }
    else {                              // unknown exception
        alert(result);
    }

}

if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.indexOf(str) == 0;
    };
}


/* ------------------------- */
/* --- generic functions --- */
/* ------------------------- */

function reload_booking_page() {

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    var new_url = window.location.href;
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    if (is_clinic) {
        new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));
        new_url = update_url_field(get_ddl_value("ddlFields") != "-1", new_url, "field", get_ddl_value("ddlFields"));
    }
    window.location.href = new_url;
}

function reload_booking_page_start_date() {

    var start_date_text = document.getElementById('txtStartDate').value;
    var valid_date_regex = /^\d{2}\-\d{2}\-\d{4}$/;
    var valid_start_date = valid_date_regex.test(start_date_text);

    var new_start_date = "";
    if (valid_start_date) {
        var mySplitResult = String(start_date_text).split("-");
        var yr = String(mySplitResult[2]);
        var mo = String(mySplitResult[1]);
        var day = String(mySplitResult[0]);
        new_start_date = yr + '_' + mo + '_' + day;
    }

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    var new_url = window.location.href;
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
    if (is_clinic)
        new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));

    if (new_start_date.length == 0)
        new_url = remove_url_field(new_url, 'date ');
    else
        new_url = set_url_field(new_url, "date", new_start_date);

    window.location.href = new_url;
}
function reload_booking_page_ndays() {

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    var new_url = window.location.href;
    var ddlDaysToDisplay = document.getElementById("ddlDaysToDisplay");
    new_url = set_url_field(new_url, "ndays", ddlDaysToDisplay.options[ddlDaysToDisplay.selectedIndex].value);
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
    if (is_clinic)
        new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));
    window.location.href = new_url;
}
function reload_booking_page_show_unavailable_staff() {

    var is_aged_care = document.getElementById("lblSiteIsAgedCare").value == "1";
    var is_clinic = document.getElementById("lblSiteIsClinic").value == "1";

    var new_url = window.location.href;
    var chkShowUnavailableStaff = document.getElementById("chkShowUnavailableStaff");
    new_url = set_url_field(new_url, "show_unavailable_staff", chkShowUnavailableStaff.checked ? "1" : "0" );
    new_url = set_url_field(new_url, "scroll_pos", document.getElementById('scrollValues').value);
    new_url = set_url_field(new_url, "is_collapsed", document.getElementById("chkShowDetails").checked ? "0" : "1");
    if (is_clinic)
        new_url = update_url_field(get_ddl_value("ddlServices") != "-1", new_url, "offering", get_ddl_value("ddlServices"));
    window.location.href = new_url;
}

function getUrlVars(new_url) {

    if ('undefined' === typeof new_url)
        new_url = window.location.href;

    var vars = [], hash;
    var hashes = new_url.slice(new_url.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
function create_url_params(list) {
    var url = "";
    for (var i = 0; i < list.length; i++) {
        url = url + "&" + list[i].n + "=" + list[i].v;
    }
    return (url.length > 0) ? url.substr(1, url.length - 1) : url;
}
function create_url_param(n, v) {
    var item = new Array();
    item.n = n;
    item.v = v;
    return item;
}

function set_url_field(new_url, field_name, value) {

    var url_field_id = getUrlVars(new_url)[field_name];

    if (url_field_id == undefined)
        return new_url + (new_url.indexOf("?") !== -1 ? "&" : "?") + field_name + "=" + value;
    else
        return new_url.replace(field_name + "=" + url_field_id, field_name + "=" + value);
}

function remove_url_field(new_url, field_name) {

    if (new_url.indexOf("?") == -1)
        return new_url;

    var toRemove = field_name + "=" + getUrlVars()[field_name];
    if (new_url.indexOf("&" + toRemove) != -1)
        return new_url.replace("&" + toRemove, "");
    else if (new_url.indexOf(toRemove + "&") != -1)
        return new_url.Replace(toRemove + "&", "");
    else if (getUrlVars().length == 1 && new_url.indexOf("?" + toRemove) != -1) {
        var posStart = new_url.indexOf("?" + toRemove);
        var posEnd = posStart + ("?" + toRemove).length;
        return new_url.substring(0, posStart) + (posEnd >= new_url.length ? "" : new_url.substring(posEnd, new_url.length));
    }
    else
        return new_url;
}

function update_url_field(add, new_url, field_name, value) {
    return add ? set_url_field(new_url, field_name, value) : remove_url_field(new_url, field_name);
}

function get_lbl_data(lbl_name) {

    var elem = document.getElementById(lbl_name);

    if (elem == null)
        return null;
    else
        return elem.hasChildNodes() ? String(elem.firstChild.data) : "";
}
function set_lbl_data(lbl_name, v) {
    document.getElementById(lbl_name).innerHTML = v;
}
function get_ddl_value(ddlName) {
    var e = document.getElementById(ddlName);
    return e.options[e.selectedIndex].value
}
function get_ddl_text(ddlName) {
    var e = document.getElementById(ddlName);
    return e.options[e.selectedIndex].text
}

function convert_to_date_type(in_str) {
    var mySplitResult = String(in_str).split("_");
    var yr = String(mySplitResult[0]);
    var mo = String(mySplitResult[1]);
    var day = String(mySplitResult[2]);
    var hr = String(mySplitResult[3]).substring(0, 2);
    var min = String(mySplitResult[3]).substring(2, 4);
    return new Date(parseInt(yr, 10), parseInt(mo, 10) - 1, parseInt(day, 10), parseInt(hr, 10), parseInt(String(min), 10), 0)
}
function getTodaysDateString() {
    var today = new Date();
    var year = today.getFullYear();
    var month = today.getMonth() + 1;
    if (String(month).length == 1) month = "0" + month;
    var day = today.getDate();
    if (String(day).length == 1) day = "0" + day;
    return year + "_" + month + "_" + day;
}

function get_month(n) {
    var months = new Array();
    months[0] = "January";
    months[1] = "February";
    months[2] = "March";
    months[3] = "April";
    months[4] = "May";
    months[5] = "June";
    months[6] = "July";
    months[7] = "August";
    months[8] = "September";
    months[9] = "October";
    months[10] = "November";
    months[11] = "December";
    return months[n];
}
function get_weekday(n) {
    var weekdays = new Array(7);
    weekdays[0] = "Sunday";
    weekdays[1] = "Monday";
    weekdays[2] = "Tuesday";
    weekdays[3] = "Wednesday";
    weekdays[4] = "Thursday";
    weekdays[5] = "Friday";
    weekdays[6] = "Saturday";
    return weekdays[n];
}


function open_new_window(URL, width, height) {

    width  = typeof width !== 'undefined' ? width : 1300;
    height = typeof height !== 'undefined' ? height : 960;

    var strWidth  = width == -1 ? "" : ",Width=" + width;
    var strHeight = height == -1 ? "" : ",Height=" + height;

    //width  = typeof width  !== 'undefined' ? ",Width="  + width  : "";
    //height = typeof height !== 'undefined' ? ",Height=" + height : "";

    NewWindow = window.open(URL, "_blank", "toolbar=no,menubar=0,status=0,copyhistory=0,scrollbars=yes,resizable=1,location=0" + strWidth + strHeight);
    //NewWindow = window.open(URL, "_blank", "toolbar=no,menubar=0,status=0,copyhistory=0,scrollbars=yes,resizable=1,location=0,Width=1300,Height=960");
    NewWindow.location = URL;
}

function element_exists(elementId) {
    var element = document.getElementById(elementId);
    return (typeof (element) != 'undefined' && element != null);
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}

Number.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
}
