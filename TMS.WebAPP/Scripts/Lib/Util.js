var openprofile = function () {
    window.location.href = '../../user/profile';
}

var _process = '<div class="progress">'
 + '<div class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100" style="width: 40%">'
   + '<span class="sr-only">Vui lòng chờ trong giây lát</span>'
 + '</div>'
 + '</div>';

var Maximinepopup = function () {
    if ($('#windowpopup_size_hidden_maximine').val() == 'min') {
        $('#windowpopup_size').css(
                {
                    width: function () {
                        return "100%";
                    },
                    height: function () {
                        return "100%";
                    }
                });
        $('#windowpopup_size_hidden_maximine').val('max');
    }
    else {
        $('#windowpopup_size').css(
                {
                    width: function () {
                        return "80%";
                    },
                    height: function () {
                        return "80%";
                    }
                });
        $('#windowpopup_size_hidden_maximine').val('min');
    }
}
//Hàm mở popup
var loadpopup = function (url, title, width, isblank) {
    var _para = '';
    if (checkparameterURL(url) == true) {
        _para = '&_isbl=' + isblank;
    }
    else {
        _para = '?_isbl=' + isblank;
    }
    url = url + _para;
    if (isblank == false) {
        $('#windowpopup_content').html(_process);
        $('#windowpopup_title').html(title);
        $('#windowpopup_content').load(url);
        //$('modal-body-iframe').attr("src", url);
        // $('#windowpopup').modal();
        $('#windowpopup').modal({
            backdrop: 'static',
            keyboard: false
        })
        if (width != "none") {
            $('#windowpopup_size').css(
                {
                    width: function () {
                        if (width == "") {
                            return "80%";
                        }
                        else {
                            return width;
                        }
                    }
                });
        }
    }
    else {
        var w = 630, h = 440; // default sizes
        if (window.screen) {
            w = window.screen.availWidth * 85 / 100;
            h = window.screen.availHeight * 85 / 100;
        }
        //  javascript: OpenWindow(url, title, "channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1,menubar=no,resizable=1,top=20,left=20,fullscreen=yes,location=yes,Width=" + w + ",height=" + h + "");

        //  var randomnumber = Math.floor((Math.random() * 100) + 1);
        //  window.open(url, "_blank", title,  "channelmode = 1, scrollbars = 1, status = 0, titlebar = 0, toolbar = 0, resizable = 1, menubar = no, resizable = 1, top = 20, left = 20, fullscreen = yes, location = yes, Width = " + w + ", height = " + h + "");
        //  window.open(url, title, "channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1,menubar=no,resizable=1,top=20,left=20,fullscreen=yes,location=yes,Width=" + w + ",height=" + h + "");
        //javascript: OpenWindow(url, title, "channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1,menubar=no,resizable=1,top=20,left=20,fullscreen=yes,location=yes,Width=" + w + ",height=" + h + "");
        var d = new Date();
        var n = d.getTime();
        title = title + n;
        var _window = window.open(url, title, "channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1,menubar=no,resizable=1,top=20,left=20,fullscreen=yes,location=yes,Width=" + w + ",height=" + h + "");
        _window.focus();
    }
}
var openprintpage = function (url) {
    var w = 630, h = 440; // default sizes
    if (window.screen) {
        // w = window.screen.availWidth * 85 / 100;
        w = 700;
        h = window.screen.availHeight * 85 / 100;
    }
    $.get(url, function (my_var) {
        var bdhtml = my_var;
        sprnstr = "<!--startprint-->";
        eprnstr = "<!--endprint-->";
        prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
        prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
        var pwin = window.open('', 'print_content', "channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1,menubar=no,resizable=1,top=20,left=20,fullscreen=yes,location=yes,Width=" + w + ",height=" + h + "");
        pwin.document.open();
        pwin.document.write('<html><head><title>SCM - In Ấn - Nhấn Ctrl + P để in</title></head><body onload="window.print()">' + prnhtml + '</body></html>');
        pwin.focus();
        pwin.document.close();
        //setTimeout(function () { pwin.close(); }, 1000);
    })
}
var checkparameterURL = function (url) {
    if (url.indexOf("?") > 0) {
        return true;
    }
    return false;
}
var ClosePopup = function () {
    // alert('t');
    //this.submit();
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}
//check tham số truyền vào null hoặc rỗng
function isNullOrEmpty(input) {
    var _q = $.trim(input);
    if ($.trim(input) == '' || $.trim(input) == 'NULL' || $.trim(input) == 'Null' || $.trim(input) == 'null' || $.trim(input).length == 0) {
        return true;
    }
    return false;
}
//show thông báo
function endshow() {
    $('body').find('.message').remove();
}
function show(issuccess, msg, titleCaption) {
    var cls = '';
    var title = '';
    if (issuccess == true) {
        cls = 'alert-success';
        title = titleCaption;
    }
    else {
        cls = 'alert-danger';
        title = titleCaption;
    }

    $('body').find('.message').remove();
    // }
    $('body').append('<div class="message alert ' + cls + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button><b class="message_title"><span class="fa fa-warning"></span> ' + title + '</b> <div class="row content_message">' + msg + '</div></div>');
    msg = '<div>' + msg + '</div>';
    /*if ($(msg).length > 0) {
       // alert($(msg).text());
    }
    else {
       // alert(msg);
    }
    */
    //if (issuccess == true) {
    //    setTimeout(function () {
    //        $('.message').fadeOut();
    //    }, 10000);
    //}
    //else {
    //    setTimeout(function () {
    //        $('.message').fadeOut();
    //    }, 20000);
    //}
}
//set error cho cac control tren form
function isObject(val) {
    return val instanceof Object;
}
function setError(controlid, iserror, _title, position) {
    var _positiontooltip = 'top';
    if (position) {
        _positiontooltip = position;
    }
    if (isObject(controlid)) {
        if (iserror == true) {
            if (!$(controlid).hasClass("has-error")) {
                $(controlid).toggleClass("has-error");
            }
            if ($(controlid).hasClass("k-grid")) {
                if (!$(controlid).hasClass("has-error-grid")) {
                    $(controlid).toggleClass("has-error-grid");
                }
            }
            $(controlid).tooltip({
                title: _title,
                placement: _positiontooltip
            });
        } else {
            $(controlid).removeClass("has-error");
            if ($(controlid).hasClass("k-grid")) {
                $(controlid).removeClass("has-error-grid");
            }
            $(controlid).tooltip('destroy');
        }
    }
    else {
        if (iserror == true) {
            if (!$('#' + controlid).closest("div").hasClass("has-error")) {
                $('#' + controlid).closest("div").toggleClass("has-error");
            }
            if ($('#' + controlid).closest("div").hasClass("k-grid")) {
                if (!$('#' + controlid).closest("div").hasClass("has-error-grid")) {
                    $('#' + controlid).closest("div").toggleClass("has-error-grid");
                }
            }
            $('#' + controlid).closest("div").tooltip({
                title: _title,
                placement: _positiontooltip
            });
            /*
             */
            //set tooltip to kendo combobox

            /*
            if ($('#' + controlid).closest("span").hasClass("k-combobox")) {
                var span = $('#' + controlid).closest("span");
                $(span).find('input:text, input:password, input:file, select, textarea')
                        .each(function () {
                            $(this).tooltip({
                                title: _title
                            });
                        });
            }
            //end set tooltip to kendo combobox
                //kendo grid
            if ($('#' + controlid).closest("div").hasClass("k-grid")) {
                $('#' + controlid).closest("div").tooltip({
                    title: _title
                });
            }
            else {
                $('#' + controlid).tooltip({
                    title: _title
                });
            }
            */
        } else {
            $('#' + controlid).closest("div").removeClass("has-error");
            if ($('#' + controlid).closest("div").hasClass("k-grid")) {
                $('#' + controlid).closest("div").removeClass("has-error-grid");
            }
            //unset tooltip to kendo combobox
            $('#' + controlid).closest("div").tooltip('destroy');
            /*
            if ($('#' + controlid).closest("span").hasClass("k-combobox")) {
                var span = $('#' + controlid).closest("span");
                $(span).find('input:text, input:password, input:file, select, textarea')
                        .each(function () {
                            $(this).tooltip('destroy');
                        });
            }
            else {
                $('#' + controlid).tooltip('destroy');
            }
            */
        }
    }
}
//reset form
//set default value and remove all css class has-error
function resetform(formid) {
    $('#' + formid).trigger("reset");
    $('.has-error').removeClass('has-error');
    $('body').find('.message').remove();
}

var StringToNumber = function (input) {
    var _t = $.trim(input);
    _t = _t.replace(/ /g, '');
    _t = _t.replace(/,/g, '');
    // alert(input +' - ' + _t);
    var output = Number(_t);
    if ($.trim(output) == '') {
        output = 0;
    }
    return output;
}

var waitingDialog = (function ($) {
    // Creating modal dialog's DOM
    /*var $dialog = $(
		'<div class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
		'<div class="modal-dialog modal-m">' +
		'<div class="modal-content">' +
			'<div class="modal-header"><img src="../../Content/AdminTemplate/images/wait20.gif" /><h3 style="margin:0;"></h3></div>' +
			'<div class="modal-body">' +
				'<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
			'</div>' +
		'</div></div></div>');
    */
    var $dialog = $(
       // '<div class="modal-backdrop fade in" style="z-index: 2060; opacity: 0.1;"></div>' +
		'<div  class="modal fade" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:30%; z-index: 15060; overflow-y:visible;">' +
		'<div style = "margin:0 auto; text-align:center;">' +
		'<div >' +
			'<div ><img src="../../Content/Images/page-loader.gif" style="top: 50%;" width="50px" /><h3 style="margin:0; color:white;"></h3></div>' +
			 +
		'</div></div></div>');

    return {
        /**
		 * Opens our dialog
		 * @param message Custom message
		 * @param options Custom options:
		 * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
		 * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
		 */
        show: function (message, options) {
            // Assigning defaults
            var settings = $.extend({
                dialogSize: 'm',
                progressType: ''
            }, options);
            if (typeof message === 'undefined') {
                message = 'Loading';
            }
            if (typeof options === 'undefined') {
                options = {};
            }
            // Configuring dialog
            $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
            $dialog.find('.modal-dialog').toggleClass('waitingpopup');
            $dialog.find('.progress-bar').attr('class', 'progress-bar');
            if (settings.progressType) {
                $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
            }
            $dialog.find('h3').text(message);
            // Opening dialog
            $dialog.modal();
        },
        /**
		 * Closes dialog
		 */
        hide: function () {
            $dialog.modal('hide');
        }
    }
})(jQuery);

//so sanh gia tri 2 ngay
var dtCh = "/";
var minYear = 1900;
var maxYear = 3000;
function CompareDates(str1, str2) {
    if (!isDate(str1) || !isDate(str2)) {
        return false;
    }
    var date1 = strDateToInteger(str1);
    var date2 = strDateToInteger(str2);
    if (date2 * 1 < date1 * 1) {
        //        alert("Ngày bắt đầu không được lớn hơn ngày kết thúc");
        return false;
    }
    return true;
}

//ham kiem tra ngay hop le
function PadLeft(value, length) {
    var i = value.length;
    for (; i < length; i++) {
        value = "0" + value;
    }
    return value;
}

function stripCharsInBag(s, bag) {
    var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}
function strDateToInteger(strDate) {
    var arr1 = strDate.split('/');
    arr1[1] = PadLeft(arr1[1], 2);
    arr1[0] = PadLeft(arr1[0], 2);
    return arr1[2] + arr1[1] + arr1[0];
}
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31;
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30; }
        if (i == 2) { this[i] = 29; }
    }
    return this;
}
function isDate(dtStr) {
    var flag = true;
    var daysInMonth = DaysArray(12);
    var pos1 = dtStr.indexOf(dtCh);
    var pos2 = dtStr.indexOf(dtCh, pos1 + 1);
    var strDay = dtStr.substring(0, pos1);
    var strMonth = dtStr.substring(pos1 + 1, pos2);
    var strYear = dtStr.substring(pos2 + 1);
    strYr = strYear;
    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1);
    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1);
    for (var i = 1; i <= 3; i++) {
        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1);
    }
    month = parseInt(strMonth);
    day = parseInt(strDay);
    year = parseInt(strYr);
    if (pos1 == -1 || pos2 == -1) {
        flag = false;
    }
    if (strMonth.length < 1 || month < 1 || month > 12) {
        flag = false;
    }
    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
        flag = false;
    }
    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
        flag = false;
    }
    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
        flag = false;
    }
    if (flag == false) {
        //		alert("Giá trị ngày không hợp lệ");
        return false;
    }
    return true;
}
function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}
function ConfirmOnDelete(message) {
    if (confirm(message) == true)
        return true;
    else
        return false;
}
function setdisable(controlid, istrue) {
    if (isObject(controlid)) {
        if (istrue == true) {
            if (!$(controlid).hasClass("disabled")) {
                $(controlid).toggleClass("disabled");
            }
        }
        else {
            $(controlid).removeClass("disabled");
        }
    }
    else {
        if (istrue == true) {
            if (!$('#' + controlid).hasClass("disabled")) {
                $('#' + controlid).toggleClass("disabled");
            }
        }
        else {
            $('#' + controlid).removeClass("disabled");
        }
    }
}

function setActiveProcess(controlid, istrue) {
    if (isObject(controlid)) {
        if (istrue == true) {
            if (!$(controlid).hasClass("btnActive")) {
                $(controlid).toggleClass("btnActive");
            }
        }
        else {
            $(controlid).removeClass("btnActive");
            if (!$(controlid).hasClass("btnNon")) {
                $(controlid).toggleClass("btnNon");
            }
        }
    }
    else {
        if (istrue == true) {
            if (!$('#' + controlid).hasClass("btnActive")) {
                $('#' + controlid).toggleClass("btnActive");
            }
        }
        else {
            $('#' + controlid).removeClass("btnActive");
            if (!$('#' + controlid).hasClass("btnNon")) {
                $('#' + controlid).toggleClass("btnNon");
            }
        }
    }
}

function loginpopup_click() {
    var txtusername = $('#loginformpopup_txtusername').val();
    var txtpass = $('#loginformpopup_txtpassword').val();
    var _check = true;
    if (isNullOrEmpty(txtusername)) {
        setError('loginformpopup_txtusername', true, 'Vui lòng nhập Tên đăng nhập');
        _check = false;
    }
    else if (txtusername.length > 40) {
        setError('loginformpopup_txtusername', true, 'Tên đăng nhập quá dài');
        _check = false;
    }
    else {
        setError('loginformpopup_txtusername', false);
    }
    if (isNullOrEmpty(txtpass)) {
        setError('loginformpopup_txtpassword', true, 'Vui lòng nhập Mật khẩu');
        _check = false;
    }
    else if (txtpass.length > 100) {
        setError('loginformpopup_txtpassword', true, 'Mật khẩu quá dài');
        _check = false;
    }
    else {
        setError('loginformpopup_txtpassword', false);
    }
    if (_check == false) {
        return;
    }
    var iChars = "!`#$%^&*()+=-[]\\\';,/{}|\":<>?~_";
    setError('loginformpopup_txtusername', false);
    for (var i = 0; i < txtusername.length; i++) {
        if (iChars.indexOf(txtusername.charAt(i)) != -1) {
            setError('loginformpopup_txtusername', true, 'Tên đăng nhập không hợp lệ vì sử dụng ký tự đặc biệt.');
            _check = false;
            break;
        }
    }
    if (_check == false) {
        return;
    }

    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
    var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtpass), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    }).toString();
    // alert(encryptedpassword);
    var paramValue = JSON.stringify({
        username: txtusername,
        pass: encryptedpassword
    });

    $.ajax({
        url: '../../Account/LoginJSon',
        type: 'POST',
        dataType: 'json',
        data: paramValue,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $("#loginformpopup_btnlogin").button('loading');
        },
        success: function (result) {
            $("#loginformpopup_btnlogin").button('reset');
            if (result.mess = 1) {
                $('#loginformpopup_txtusername').val('');
                $('#loginformpopup_txtpassword').val('');
                reloadheaderAfterLogin();
                $('#windowpopup_login').modal("hide");
            }
            else {
                setError('loginformpopup_txtusername', true, result.responemess);
            }
        },
        error: function (msg) {
            $("#loginformpopup_btnlogin").button('reset');
            setError('loginformpopup_txtusername', true, "Đăng nhập thất bại, có lỗi trong quá trình xử lý.");
        },
    })
}

var reloadheaderAfterLogin = function () {
    try {
        var url = $('#hiddenFieldCurrentUrl').val();
        var urldivContentMainHeader = "../../MasterData/MainHeader";
        //  divContentMainHeader

        var urldivContentFunctionPageHeader = "../../MasterData/FunctionPageHeader?url=" + url;
        //divContentFunctionPageHeader
        $("#divContentMainHeader").load(urldivContentMainHeader);
        $("#divContentFunctionPageHeader").load(urldivContentFunctionPageHeader);

        var urldivContentFunctionNavigation = "../../MasterData/FunctionNavigation?url=" + url;
        $("#divContentFunctionNavigation").load(urldivContentFunctionNavigation);
        //divContentFunctionNavigation
    } catch (error) { }
}

var replaceCurrentUrl = function (suburl) {
    //try{
    var stateObj = { foo: "bar" };
    var url = $('#hiddenFieldCurrentUrl').val() + "?" + suburl + "&_isbl=true";
    //window.location.href = url;
    $('#hiddenFieldCurrentUrl_InEdit').val(url);
    //window.history.pushState(stateObj, "Title", url);
    //window.history.pushState({ path: url }, '', url);
    //window.history.replaceState({ path: url }, '', url);
    //alert(url);

    //window.history.pushState(url);

    //} catch (error)
    //{ }
}

///thonglt
function findIndexByKeyValue(obj, key, value) {
    for (var i = 0; i < obj.length; i++) {
        if (obj[i] != null && obj[i][key] == value) {
            return i;
        }
    }
    return null;
}

function ComboboxOnchange() {
    if (this.value() && this.selectedIndex == -1) {
        //var dt = this.dataSource._data[1];
        //this.text(dt[this.options.dataTextField]);
        this.text('');
        this._selectItem();
        // var id = this.("id");
        //show(false, id, true);
        // setError(this.id, true, 'lỗi ');
    }
}

function loadRecentActivity() {
    //lstRecentActivity
    //var paramValue = JSON.stringify({
    //    username: txtusername,
    //    pass: txtpass
    //});
    $.ajax({
        url: '../../Activity/loadRecentActivity',
        type: 'POST',
        dataType: 'json',
        data: '',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
            $('#lstRecentActivity').html('đang tải dữ liệu...');
        },
        success: function (result) {
            //alert(result);
            var html = '';
            for (var i = 0; i < result.data.length ; i++) {
                html = html + '<li>'
                             + '<a href="javascript::;">'
                             + '<i class="menu-icon fa fa-circle"></i>'
                             + '<div>'
                             + '<h5 class="control-sidebar-subheading">'
                             + result.data[i].CreatedBy + ' đã ' + result.data[i].ActivityName + ': <br/> ' + result.data[i].ActivityDescription
                             + '<p> <label class="control-sidebar-subheading-timeago pull-left">' + ' #' + result.data[i].RefCode + '</label>'
                             + '<label class="control-sidebar-subheading-timeago pull-right">' + result.data[i].TimeAgo + '</label> </p>'
                             + '</h5>'
                             + '</div>'
                            + '</a>'
                        + '</li>';
            }

            html = html + '<li>'
                          + '<a href="../../user/activity" class="pull-right">'
                          + 'Xem tất cả'
                          + '</a>'
                          + '</li>';

            $('#lstRecentActivity').html(html);
        },
        error: function (msg) {
            $('#lstRecentActivity').html('Chưa cóc hoạt động nào');
        },
    })
}