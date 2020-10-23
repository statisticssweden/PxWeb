/// <reference path="jquery.js" />

jQuery.noConflict();

function CollapseTreeView() {
    jQuery(".AspNet-TreeView-Collapse").each(function(i) {//Collapse treeview for users with javascript enabled.
        ExpandCollapse__AspNetTreeView(this);
    });

    HideAllText();
}


function ExpandFirstNode() {
    jQuery(".AspNet-TreeView-Root > .AspNet-TreeView-Expand").each(function(i) {//Collapse treeview for users with javascript enabled.
        ExpandCollapse__AspNetTreeView(this);
    });
}

function ShowHideText(id) {
    javascript: jQuery('#' + id).toggleClass('ctrlsearchvariablevaluestext');
}

function HideAllText() {
    javascript: jQuery('ctrlsearchvariablevaluestext').removeClass('ctrlsearchvariablevaluestext');
}

function SetResizable() {
    //jQuery(".variableselector_valuesselect_box").resizable({ handles: 'e', minWidth: 150 });
    alert('Ställer in resizable på ' + jQuery(".variableselector_valuesselect_box").length + ' objekt');
    jQuery(".variableselector_valuesselect_box").resizable();
}

function openCenteredWindow(url, width, height) {
    var left = parseInt((screen.availWidth / 2) - (width / 2));
    var top = parseInt((screen.availHeight / 2) - (height / 2));
    var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
    window.open(url, '_blank', windowFeatures);
}



//Calls CollapseChildrenFromLevel to collapse all treenodes from fromLevel and below
function CollapseChildren(fromLevel) {
    jQuery(".AspNet-TreeView-Parent").each(function(i) {//Ge all AspNet-TreeView-Parent elements and call CollapseChildrenFromLevel with each
        CollapseChildrenFromLevel(this, fromLevel)
    });
}

function CollapseChildrenFromLevel(ele, fromLevel) {
    jQuery(ele).find(".levelIndexField:first").each(function(i) {//Get first levelIndexField in element wich should me the direct child
        if (this.value == fromLevel) {
            jQuery(ele).find(".AspNet-TreeView-Collapse").each(function(i) {//If value matches selected level, collapse the node and its children
                ExpandCollapse__AspNetTreeView(this);
            });
        }
    });
}

//function ChangeFontsize(decrease)
//{
//    var originalSize = jQuery("body").css("font-size");
//    originalSize = originalSize.replace("px", "");
//    var newSize = (originalSize / jQuery("body").width()) * 100;
//    if (decrease == true)
//    {
//        newSize -= 10;
//    }
//    else if (decrease == false)
//    {
//        newSize += 10;
//    }    
//    newSize = newSize + "%";
//    //jQuery("ds").css(
//    jQuery("body").css("font-size", newSize);
//}

jQuery(function () {
    var isOnIOS = navigator.userAgent.match(/iPad/i) || navigator.userAgent.match(/iPhone/i) || (navigator.platform === 'MacIntel' && navigator.maxTouchPoints > 1);

    if (typeof sessionStorage !== 'undefined' && !isOnIOS) {
        // Get rxid session cookie
        var rxid = document.cookie.replace(/(?:(?:^|.*;\s*)rxid\s*\=\s*([^;]*).*$)|^.*$/, "$1");
        // Store rxid in sessionStorage because this storage is unique for each browser tab
        if (rxid && !sessionStorage.getItem("rxid")) {
            sessionStorage.setItem('rxid', rxid);
        }
        // Delete session cookie
        document.cookie = "rxid=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        // On beforunload make sure the next request uses rxid from sessionStorage
        jQuery(window).bind('beforeunload', function(e) {
            if (sessionStorage.getItem("rxid")) {
                document.cookie = "rxid=" + sessionStorage.getItem("rxid") + "; path=/";
            }
        });
    }
});
function isSelectionLayoutCompact() {
    var Selectionlayout = getCookie("layoutCookie");
    return Boolean(Selectionlayout == "compact")
}
function getCookie(key) {
    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
    return keyValue ? keyValue[2] : null;
}  

