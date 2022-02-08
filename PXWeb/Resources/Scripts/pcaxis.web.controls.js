/// <reference path="jquery.js" />

jQuery.noConflict();

///CommandBar

function PivotManual_Switch(fromId, toId) {
    jQuery("#" + fromId + " option:selected").appendTo("#" + toId);
    //Begin Hack for IE (Does not reposition listboxes automatically)
    jQuery(".commandbar_container").hide(0.0000001);
    jQuery(".commandbar_container").show(0.0000001);
    //End Hack

    PivotManual_Deselect(fromId, toId);

    return false;
}

function PivotManual_Deselect(fromId, toId) {
    jQuery("#" + fromId + " option:selected").attr("selected", false);
    jQuery("#" + toId + " option:selected").attr("selected", false);

    jQuery("#" + fromId).attr("selectedIndex", "-1");
    jQuery("#" + toId).attr("selectedIndex", "-1");
}

function PivotManual_Reorder(listId, direction) {
    var selectedOption = jQuery("#" + listId + " option:selected");

    if (direction == "top") {
        if (selectedOption[0].previousElementSibling) {
            selectedOption[0].parentNode.insertBefore(selectedOption[0], selectedOption[0].previousElementSibling);
        }
    }
    else {
        if (selectedOption[0].nextElementSibling) {
            selectedOption[0].parentNode.insertBefore(selectedOption[0].nextElementSibling, selectedOption[0]);
        }
    }
    return false;
}

function PivotManual_Submit(headingId, stubId) {
    var headings = "";
    var stubs = "";

    jQuery("#" + headingId + " option").each(function() {
        headings += jQuery(this).text() + "@";
    });

    jQuery("#" + stubId + " option").each(function() {
        stubs += jQuery(this).text() + "@";
    });

    jQuery("#PivotManualStubList").val(stubs)
    jQuery("#PivotManualHeadingList").val(headings)

}


function UpdateNumberSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy) {

    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    var selected = selectbox.selectedOptions.length;

    SetNumberSelected(ListBoxID, StatsSpanID);

    if (window.SelectedValueChanged) {
        SelectedValueChanged(ListBoxID.replace(/\$/gi, "_"), variablePlacement, selected, limitSelectionBy);
    }
}

function SetNumberSelected(ListBoxID, StatsSpanID) {
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    var statsspan = document.getElementById(StatsSpanID.replace(/\$/gi, "_"));

    if (statsspan != null) {
        statsspan.textContent = selectbox.selectedOptions.length;
    }
}

function SetButtonEnablePropertyToHasSelected(ListBoxID, ButtonId) {
    var disableButton = true;
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    disableButton = selectbox.selectedIndex == -1;

    var button = document.getElementById(ButtonId.replace(/\$/gi, "_"));
    button.disabled = disableButton;
}


function SetButtonEnablePropertyToHasDeselected(ListBoxID, ButtonId) {
    var disableButton = true;
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    disableButton = selectbox.length == selectbox.selectedOptions.length;

    var button = document.getElementById(ButtonId.replace(/\$/gi, "_"));
    button.disabled = disableButton;
}

function SelectOperands(selectedValue, textBoxIDValue1, textBoxIDValue2) {
    var count = 0;
    var textbox1 = document.getElementById(textBoxIDValue1);
    var textbox2 = document.getElementById(textBoxIDValue2);
    if (textbox1.value.length < 1) {
        textbox1.value = selectedValue;
    }
    else {
        if (textbox1.value != selectedValue) {
            textbox2.value = selectedValue;
        }
    }
}

function SetUniqueRadioButton(nameregex, current) {
    re = new RegExp(nameregex);
    for (i = 0; i < current.form.elements.length; i++) {
        elm = current.form.elements[i]
        if (elm.type == 'radio') {
            if (re.test(elm.name)) {
                elm.checked = false;
            }
        }
    }
    current.checked = true;
}


// Variable selector


// Select all elements in a listbox and updates the number selected
function VariableSelector_SelectAllAndUpdateNrSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy) {
    VariableSelector_SelectAll(ListBoxID)
    //ValidatePage(ListBoxID)
    ValidateControl(ListBoxID)
    UpdateNumberSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy);
    return false;
}


// Select all elements in a listbox
function VariableSelector_SelectAll(ListBoxID) {
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    for (var i = 0; i < selectbox.length; i++) {
        selectbox.options[i].selected = true;
    }
    return false;
}

// Deselect all elements in a listbox and updates the number selected
function VariableSelector_DeselectAllAndUpdateNrSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy) {
    VariableSelector_DeselectAll(ListBoxID)
    //ValidatePage(ListBoxID)
    ValidateControl(ListBoxID)
    UpdateNumberSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy);
    return false;
}

// Deselect all elements in a listbox
function VariableSelector_DeselectAll(ListBoxID) {
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    var currentOrder = new Array();

    for (var i = 0; i < selectbox.length; i++) {
        selectbox.options[i].selected = false;
        currentOrder.push(selectbox.options[i].value)
    }

    if ((ListBoxID in sessionStorage)) {
        VariableSelector_OrignalOrder(ListBoxID, currentOrder)
    }

    return false;
}
// Validate Page
function ValidatePage(validationGroup) {
    if (typeof (Page_ClientValidate) == 'function') {
        var scrollToOrig = window.scrollTo;
        window.scrollTo = function () { };
        Page_ClientValidate(validationGroup);
        window.scrollTo = scrollToOrig;
    }

}
// Validate Control
function ValidateControl(targetedControlId) {
    var targetedControl = document.getElementById(targetedControlId);
    if (typeof (ValidatorValidate) == 'function') {
        if (typeof (targetedControl.Validators) != "undefined") {
            vals = targetedControl.Validators

            var scrollToOrig = window.scrollTo;
            window.scrollTo = function () { };
            for (var i = 0; i < vals.length; i++) {
                ValidatorValidate(vals[i]);
                ValidatorUpdateIsValid();
            }
            window.scrollTo = scrollToOrig;
        }
    }
}


// Restore original order
function VariableSelector_OrignalOrder(ListBoxID, currentOrder) {
    var selectbox = document.getElementById(ListBoxID.replace(/\$/gi, "_"));
    var originalOrder = JSON.parse(sessionStorage.getItem(ListBoxID));
    var sortedOption;
    var originalOption = new Array();

    for (var i = 0; i < originalOrder.length; i++) {
        sortedOption = selectbox.options[currentOrder.indexOf(originalOrder[i])];
        originalOption.push(sortedOption);
    }

    selectbox.options.length = 0;

    for (var i = 0; i < originalOrder.length; i++) {
        selectbox.add(originalOption[i], i);
    }
}

//Search values in listbox with the given text
function VariableSelector_SearchValues(ListBoxID, TextBoxID, CheckBoxID, StatsSpanID, variablePlacement, limitSelectionBy, localizedLanguage) {
    var sc;
    var r, re;
    var lst;
    var contains;
    var localizedTextArray = localizedLanguage.split("|");

    var storeOriginalOrder = false;

    if (!(ListBoxID in sessionStorage)) {
        storeOriginalOrder = true;
        var order = new Array();
    }

    //Get text
    sc = jQuery("#" + TextBoxID).prop("value");

    //Search from beginning or not?
    contains = !jQuery("#" + CheckBoxID).prop("checked");

    //Get listbox
    lst = document.getElementById(ListBoxID.replace(/\$/gi, "_"));

    //Remove illegal characters
    re = /\(/i;
    r = sc.replace(re, "\\(");
    sc = r;
    re = /\)/i;
    r = sc.replace(re, "\\)");
    sc = r;
    re = /\./i;
    r = sc.replace(re, "\\.");
    sc = r;
    re = /\+/i;
    r = sc.replace(re, "\\+");
    sc = r;
    re = /\?/i;
    r = sc.replace(re, "\\?");
    sc = r;

    var cmplength = sc.length;
    var rex = RegExp(sc, "gi");

    // Old browsers might not support startsWith
    if (!String.prototype.startsWith) {
        String.prototype.startsWith = function (searchString, position) {
            return this.substr(position || 0, searchString.length) === searchString;
        };
    }

    var searchStr = sc.toLowerCase();
    var selected = new Array();
    var originPos = new Array();
    var searchResult = "";
    var searchHits = 0;

    for (i = lst.options.length - 1; i > -1; i--) {
        var currentOption = lst.options[i];

        if (storeOriginalOrder) {
            order.unshift(currentOption.value);
        }

        if (contains) {
            if (currentOption.text.toLowerCase().indexOf(searchStr) !== -1 || currentOption.text.toLowerCase().search(rex) > -1) {
                currentOption.selected = true;
                selected.unshift(currentOption);
                originPos.unshift(currentOption.index);
                searchResult = searchResult + localizedTextArray[4] + currentOption.text + " ";
                searchHits++;
            }
        } else {
            var optionText = currentOption.text.toLowerCase();

            if ((optionText.startsWith(searchStr) || optionText.indexOf(' ' + searchStr) !== -1) || (optionText.substr(0, cmplength).search(rex) > -1) )
            {
                currentOption.selected = true;
                selected.unshift(currentOption);
                originPos.unshift(currentOption.index);
                searchResult = searchResult + localizedTextArray[4] + currentOption.text + " ";
                searchHits++;
            }
        }
    }

    if (storeOriginalOrder) {
        sessionStorage.setItem(ListBoxID, JSON.stringify(order));
    }

    UpdateNumberSelected(ListBoxID, StatsSpanID, variablePlacement, limitSelectionBy);
    MoveSelectedToTop(lst, selected, originPos);
    UpdateSearchResultScreenReader(searchHits, searchResult, localizedTextArray, searchStr);
    lst.scrollTop = 0;
    ValidateControl(ListBoxID)  //piv
    return false;
}

function UpdateSearchResultScreenReader(searchHits, searchResults, localizedText, searchWord) {
    var element = document.getElementById("SearchResults");
    element.textContent = localizedText[0] + searchWord + localizedText[1] + searchHits + localizedText[2];
    if (searchHits > 0) {
        element.textContent = element.textContent + localizedText[3] + searchResults;
    }
}

function MoveSelectedToTop(lst, selected, originPos) {
    var options = lst.getElementsByTagName("OPTION");
    
    for (var i = 0; i < selected.length; i++) {
        lst.removeChild(options[originPos[i]]);
        lst.insertBefore(selected[i], options[i]);
    }
};

// Compare helper used to sort text in ascending order
function compareOptionTextAscending(a, b) {
    return a.text.localeCompare(b.text);
    // textual comparison
    //return a.text != b.text ? a.text < b.text ? -1 : 1 : 0;
    // numerical comparison
    //  return a.text - b.text;
}

// Compare helper used to sort text in descending order
function compareOptionTextDescending(a, b) {
    return -(a.text.localeCompare(b.text));
    //return a.text != b.text ? a.text < b.text ? 1 : -1 : 0;
}



// Sorts a lisbox.
// If isAscending == true   => ascending order
// If isAscending == false  => descending order
function sortOptions(list, isAscending) {
    var items = list.options.length;
    // create array and make copies of options in list
    var tmpArray = new Array(items);
    for (i = 0; i < items; i++)
        tmpArray[i] = new 
    Option(list.options[i].text, list.options[i].value);
    // sort options using given function
    if (isAscending)
        tmpArray.sort(compareOptionTextAscending);
    else
        tmpArray.sort(compareOptionTextDescending);
    // make copies of sorted options back to list
    for (i = 0; i < items; i++)
        list.options[i] = new Option(tmpArray[i].text, tmpArray[i].value);
}

//Automatical download of file from Commandbar (don´t have to click the download-link...)
function automaticFileDownload(id) {
    //Open file in a new window
    window.open(jQuery("#" + id).attr("href"));
    //Hide the download link
    jQuery("#" + id).hide();
}

// Downloads the file selected in a combobox by open a new window with the download url.
function downloadSelectedFile(combobox) {
    if (combobox.selectedIndex > 0) {
        //window.open(combobox.options[combobox.selectedIndex].value);
        //window.location = combobox.options[combobox.selectedIndex].value;
        commandbarDownloadFile(combobox.options[combobox.selectedIndex].value);
    }
    combobox.selectedIndex = 0;
}

// Download file. The parameter url must have the following format: webpage?parameters...&downloadfile=filetype
function commandbarDownloadFile(url) {
    var closeScript = "jQuery('[id$=commandbarDownloadFileDialog]').dialog('destroy');"
    jQuery("[id$=commandbarDownloadFileLink]").attr("href", url);
    jQuery("[id$=commandbarDownloadFileLink]").attr("onclick", closeScript);
    //jQuery("[id$=commandbarDownloadFileDialog]").dialog({ modal: true, hide: "fade", resizable: false, close: function(ev, ui) { jQuery(this).dialog('destroy'); } });
    jQuery("[id$=commandbarDownloadFileDialog]").dialogFunction();
}

//Open up the given div as a modal popup dialog
jQuery.fn.dialogFunction = function (options) {
    var divId = jQuery(this)[0].id;
    //Check if dialog already exists, and if so just open it  
    //This solves the can't open same dialog twice issue.
    if (jQuery("#" + divId).is(':data(dialog)')) {
        jQuery("#" + divId).dialog("open");
    }
    else {
        jQuery("#" + divId).dialog({
            width: jQuery("#" + divId).css("width"),
            modal: true,
            hide: "fade",
            resizable: false,
            close: function (ev, ui) { jQuery(this).dialog('destroy'); }
        });
        jQuery("#" + divId).dialog("open").find(":input").eq(0).focus();
    }
};

//Replaces an element with .id-property in an array if it exists, else adds element to array
//arr: array to remove item from
//ele: element to be changed or added to the array
//Return: array
function AddVariableListBox(arr, ele) {
    var foundAt = -1;
    jQuery.each(arr, function(index, obj) {
        if (obj.id == ele.id) {
            arr[index] = ele;
            foundAt = index;
            return false;
        }
    });
    if (foundAt < 0) {
        arr[arr.length] = ele;
    }
    return arr;
}

//Removes an element with .id-property from array
//arr: array to remove item from
//ele: element to be removed from the array
//Return: array
function RemoveVariableListBox(arr, ele) {
    var removeItem = ele.id;
    arr = jQuery.grep(arr, function(arrayElement) {
        return arrayElement.id != removeItem;
    });
    return arr;
}

//Sets text in textbox(es)
//text: text to textbox
//nameregex: part of textboxneme, all textboxes that matches gets the text
function SetTextBoxText(text, nameregex) {
    var re = new RegExp(nameregex);
    jQuery.each(jQuery("input:text"), function(index, obj) {
        if (re.test(obj.name)) {
            obj.value = text;
        }
    });
}


//Replaces or appends text in label(s)
//text: text to label
//idregex: part of labelid, all labels that matches gets the text
//append: if true text is appended, if false text is replaced
function SetLabelText(text, idregex, append) {
    var re = new RegExp(idregex);
    jQuery.each(jQuery("span"), function(index, obj) {
        if (re.test(obj.id)) {
            if (append) {
                obj.innerHTML += text;
            } else {
                obj.innerHTML = text;
            }
        }
    });
}

//Replaces or appends text in label(s)
//text: text to label
//idregex: part of labelid, all labels that matches gets the text
//cssclass: the labels css-class
//append: if true text is appended, if false text is replaced
function SetLabelText_IdAndCSS(text, idregex, cssclass, append) {
    var re = new RegExp(idregex);
    jQuery.each(jQuery("span"), function(index, obj) {
        if (re.test(obj.id)) {
            if (jQuery(this).hasClass(cssclass)) {
                if (append) {
                    obj.innerHTML += text;
                } else {
                    obj.innerHTML = text;
                }
            }
        }
    });
}

//Return text from label
//idregex: part of labelid, first label that matches both in id part and css class is used.
//cssclass: the labels css-class
function GetLabelText(idregex, cssclass) {
    var re = new RegExp(idregex);
    var retval = "";
    jQuery.each(jQuery("span"), function(index, obj) {
        if (re.test(obj.id)) {
            if (jQuery(this).hasClass(cssclass)) {
                retval = (this).innerHTML;
                return false;
            }
        }
    });
    return retval;
}


//Hides an element depending on jQuery selector
///Parameter example: ".commandbar_action"
function PCAxis_HideElement(selector) {
    jQuery(selector).hide(0);
}

//Display information dialog about the clicked cell in the table
function displayCellInformation(id, closetext) {
    var url = location.href;
    var bookmarkIndex = url.indexOf("#");

    var paramChar;

    if (url.indexOf('?') !== -1) {
        paramChar = '&';
    }
    else {
        paramChar = '?';
    }

    //Handle bookmark in URL
    if (bookmarkIndex >= 0) {
        url = url.slice(0, bookmarkIndex) + paramChar + 'cellid=' + id + url.slice(bookmarkIndex);
    }
    else {
        url = url + paramChar + 'cellid=' + id;
    }

    if (typeof sessionStorage !== 'undefined' && sessionStorage.getItem("rxid")) {
        // create rxid cookie before making Ajax request
        document.cookie = "rxid=" + sessionStorage.getItem("rxid") + "; path=/";
        jQuery("[id$=pxtableCellInformationDialog]").load(url, function () {
            document.cookie = "rxid=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        });
    } else {
        jQuery("[id$=pxtableCellInformationDialog]").load(url);
    }

    jQuery("[id$=pxtableCellInformationDialog]").dialog({ width: 480, height: 400, modal: true, buttons: [{ text: closetext, click: function () { jQuery(this).dialog('close'); } }], hide: "fade", close: function (ev, ui) { jQuery(this).dialog('destroy'); } });
}

//Enable/disable elements of the given class
function disableFromClass(className, disable) {
    jQuery("." + className).prop('disabled', disable);
}

/*------------------------------------------*/
/* Functions for setting panels (accordions) */
/*------------------------------------------*/

// Collapse all settingpanels
function settingpanelCollapseAll() {
    //Hide any currently displayed setting panel
    jQuery('.settingpanel').hide(0);

    //Change expand image on all links
    var col = jQuery('.px-settings-collapseimage');
    col.removeClass('px-settings-collapseimage');
    col.addClass('px-settings-expandimage');
}

// Check if the given setting panel is expanded or not
function settingpanelIsExpanded(obj) {
    return jQuery(obj).hasClass('settingpanelexpanded');
}

// Collapse the specified setting panel
function settingpanelCollapse(obj) {
    jQuery(obj).removeClass('settingpanelexpanded');
}

// Expand the setting panel with the given panel class
function settingpanelExpand(panelclass) {
    //Find link for specified panel
    var lnk = jQuery('.' + panelclass + '.panelshowlink');
    //Remove expanded class from all panellinks
    jQuery('.panelshowlink').removeClass('settingpanelexpanded');
    //Add expanded class to this panellink
    jQuery(lnk).addClass('settingpanelexpanded');
    //Show my setting panel
    jQuery('.' + panelclass + '.settingpanel').show(0);
    //Change expand-image
    var img = jQuery(lnk).find('.px-settings-expandimage');
    img.removeClass('px-settings-expandimage');
    img.addClass('px-settings-collapseimage');

}

function accordionToggle(panel, button) {
    var accordionBody = panel.querySelector('.accordion-body');
    accordionBody.classList.toggle("closed");
    button.classList.toggle("closed");

    if (accordionBody.classList.contains('closed')) {
        button.setAttribute('aria-expanded', 'false');
    } else {
        button.setAttribute('aria-expanded', 'true');
    }
}

function nestedAccordionToggle(panel, button) {
    var accordionBody = panel.querySelector('.nested-accordion-body');
    accordionBody.classList.toggle("closed");
    accordionBody.classList.toggle("open");
    button.classList.toggle("closed");
    button.classList.toggle("open");

    if (accordionBody.classList.contains('closed')) {
        button.setAttribute('aria-expanded', 'false');
    } else {
        button.setAttribute('aria-expanded', 'true');
    }
}

function openAccordion(buttonId, bodyId) {
    var accordionButton = document.getElementById(buttonId);
    var accordionBody = document.getElementById(bodyId);

    accordionButton.classList.remove('closed');
    accordionButton.setAttribute('aria-expanded', 'true');
    accordionBody.classList.remove('closed');
}

function closeAccordion(buttonId, bodyId) {
    var accordionButton = document.getElementById(buttonId);
    var accordionBody = document.getElementById(bodyId);

    accordionButton.classList.add('closed');
    accordionButton.setAttribute('aria-expanded', 'false');
    accordionBody.classList.add('closed');
}

//Set text for button with text from selected radio on load
function setOnLoadRadioLabelForButton(radioButtonList, button, ariaLabelBase) {
    var selectedLabel = jQuery("label[for='" + jQuery('#' + radioButtonList.id + " input[type='radio']:checked").attr('id') + "']");

    if (selectedLabel.length > 0) {
        jQuery('#' + button.id).attr("aria-label", ariaLabelBase + selectedLabel[0].innerText);
    };
}

//Update text for button with selected radio buttonlabel when changed
function setUpdatedRadioLabelForButton(selectedRadioOption, button, ariaLabelBase) {
    if (jQuery(selectedRadioOption).is(':checked')) {
        var labelSelectedRadio = selectedRadioOption.nextElementSibling.innerText;
        jQuery('#' + button.id).attr("aria-label", ariaLabelBase + labelSelectedRadio);
    }
}

//Set focus for element
function setFocusOnElement(elementId) {
    var element = jQuery('#' + elementId);
    element.focus();
}