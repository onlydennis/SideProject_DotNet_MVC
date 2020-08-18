//kendo.directives 給 kendo使用
//pascalprecht.translate 給 angular多國語言使用
var app = angular.module('App', ["kendo.directives", "pascalprecht.translate","ngCookies"]);

//無條件進位到整數
app.filter('ceil', function () {
    return function (input) {
        return Math.ceil(input);
    };
});
//無條件進位到小數第d位
app.filter('decimalto', function () {
    return function (number, d) {
        var zero = "";
        for (var i = 0; i < d; i++) { zero += "0"; }
        var base = parseInt("1" + zero);
        return Math.ceil(number * base) / base;
    };
});


var azureofferid = 'MS-AZR-0146P';
function GetAzureOfferId(azureoffer) {
    azureofferid = azureoffer;
}

var GetLanguageByKeyVale = function () {
    $.ajax({
        type: "POST",
        url: '/Shared/GetLanguageByKeyVale',
        success: function (response) {
            if (response.IsOk) {
                $("#LanguageControl").css("display", "");
                $("#Language").css("display", "");
            }
            else {
                $("#LanguageControl").css("display", "none");
                $("#Language").css("display", "none");
            }
        },
        error: function (jqXHR, textStatus, errorThrowm) {
            alertify.error(errorThrowm);
        },
        dataType: 'json'
    });
}
//GetLanguageByKeyVale();

var SetLanguage = function (e) {
    var lang = e.value;
    $.ajax({
        type: "POST",
        url: '/Shared/SetCultureLanguage',
        data: { language: lang },
        success: function (response) {
            if (response.IsOk) {
                window.location.reload();
            }
            else {
                alertify.alert(response.Message);
            }
        },
        error: function (jqXHR, textStatus, errorThrowm) {
            alertify.error(errorThrowm);
        },
        dataType: 'json'
    });
}


function GridOnChange(e) {
    grid = e.sender;
    var selectedItem = grid.dataItem(this.select());
    angular.element($('#Controller')).scope().EditData(selectedItem);
}
var OnLoading = function () {
    $.blockUI({ message: $('#blockMes') });
}

var OnLoaded = function () {
    $.unblockUI();
}

var OnGridError = function (e) {
    let text = e.xhr.responseText
    match = text.match(/<title>([\s\S]*)<\/title>/)
    if (match != null && match.length > 1) alertify.error(match[1]);
}

function CheckPositiveInteger(val) {
    if (val < 0) return -val;
}

Date.prototype.addHours = function (h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
}

Date.prototype.yyyyMM = function () {
    var mm = this.getMonth() + 1; // getMonth() is zero-based

    return [this.getFullYear(),
    (mm > 9 ? '' : '0') + mm.toString()
    ].join('');
};

Date.prototype.dateToUTC = function () {
    return new Date(this.getUTCFullYear(), this.getUTCMonth(), this.getUTCDate(), this.getUTCHours(), this.getUTCMinutes(), this.getUTCSeconds());
};

Date.prototype.yyyymmdd_1 = function () {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();
    return [this.getFullYear(),
    (mm > 9 ? '' : '0') + mm,
    (dd > 9 ? '' : '0') + dd
    ].join('/');
};
Date.prototype.yyyyMMddhhmm_1 = function () {
    var MM = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();
    var hh = this.getHours();
    var mm = this.getMinutes();
    var yyyyMMdd = [this.getFullYear(),
    (MM > 9 ? '' : '0') + MM,
    (dd > 9 ? '' : '0') + dd
    ].join('/');
    return yyyyMMdd + ' ' + [(hh > 9 ? '' : '0') + hh,
    (mm > 9 ? '' : '0') + mm].join(':');
};
function parseServerJsonDate(parm) {
    if (parm == null)
        return parm;
    var result = null;
    var re = /-?\d+/;
    var m = re.exec(parm);
    result = m != null ? new Date(parseInt(m[0])) : null;
    return result;
}

Number.prototype.DecimalTo = function (d) {
    var zero = "";
    for (var i = 0; i < d; i++) { zero += "0"; }
    var base = parseInt("1" + zero);

    return Math.ceil(this * base) / base;

}

$(".calendar").attr('data-val-date', "Date Error").kendoDatePicker({
    format: "yyyy/MM/dd"
});


function QueryString(name) {
    var AllVars = window.location.search.substring(1);
    var Vars = AllVars.split("&");
    for (i = 0; i < Vars.length; i++) {
        var Var = Vars[i].split("=");
        if (Var[0] == name) return Var[1];
    }
    return "";
}

function ExportBlobData(fileName, fileByte, fileType) {
    var byteCharacters = atob(fileByte);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var data = new Blob([byteArray], { type: fileType });
    var a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    var blob = data,
    url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
};

function GetEndCustomerDataListByReseller(resellerNo, endCustomerDataList) {
    var resultDataList = [];
    if (!!resellerNo && resellerNo !== "" && resellerNo !== 0) {
        //endCustomerDataList.foreach(function (item, index) {
        //    if (value.ResellerId == resellerId) {
        //        resultDataList.push(value);
        //    }

        //});
        angular.forEach(endCustomerDataList, function (value, EndCustomerData) {
            if (value.ResellerNo == resellerNo) {
                resultDataList.push(value);
            }
        });
    } else {
        resultDataList = angular.copy(endCustomerDataList);
    }

    return resultDataList;
}


