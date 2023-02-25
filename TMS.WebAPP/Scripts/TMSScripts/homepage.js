function GetLanguagetSelected(value) {
    var paramValue = JSON.stringify({
        languageId: value
    });
    $.ajax({
        url: '../../Home/SelectedLanguage',
        type: 'POST',
        dataType: 'json',
        data: paramValue,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
        },
        success: function (data) {
            location.reload();
        },
        error: function (mess) {
            location.reload();
            $("#error-system").removeClass("display-none");
        },
    })
}

function GeCurrentCompanySelected(value) {
    var paramValue = JSON.stringify({
        companyId: value
    });
    $.ajax({
        url: '../../Home/SelectedCompany',
        type: 'POST',
        dataType: 'json',
        data: paramValue,
        contentType: 'application/json; charset=utf-8',
        beforeSend: function () {
        },
        success: function (data) {
            location.reload();
        },
        error: function (mess) {
            location.reload();
            $("#error-system").removeClass("display-none");
        },
    })
}