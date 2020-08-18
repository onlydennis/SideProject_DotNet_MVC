app.controller('HomeController', HomeController);

function HomeController($scope, $http, $filter) {
    $scope.init = function () {
        $scope.data = {

        }
    }

    $scope.QueryAll = function () {
        $('#gridRegion').data('kendoGrid').dataSource.transport.options.read.data.query = $scope.Query;
        $('#gridRegion').data("kendoGrid").dataSource.read();
        $('#gridRegion').data("kendoGrid").dataSource.page(1);
        $('#gridRegion').data("kendoGrid").refresh();
        $('#gridRegion').show();
    }

    var dataSrc = new kendo.data.DataSource({
        transport: {
            read: {
                //以下其實就是$.ajax的參數
                type: "POST",
                url: "/Home/ReadData",
                dataType: "json",
                data: {
                    query: $scope.Query
                }
            }
        },
        schema: {
            //取出資料陣列
            data: function (d) {
                return d.data;
            },
            //取出資料總筆數(計算頁數用)
            total: function (d) {
                //return d.TotalCount;
                return d.total;
            }
        },

        pageSize: 10,
    });

    $("#gridRegion").kendoGrid({
        dataSource: dataSrc,
        columns: [{
            "field": "Id",
            "title": "Id"
        },
        {
            "field": "KeyValueName",
            "title": "KeyValueName"
        },
        {
            "field": "Text",
            "title": "Text"
        },
        {
            "field": "Value",
            "title": "Value"
        }
        ],
        selectable: true,
        pageable: true,
        sortable: true,
        resizable: true,
        change: GridOnChange
    });

    $scope.ShowQuery = function () {
        $scope.QueryShow = true;
        $scope.EditShow = false;
    }

    $scope.ShowQuery();

    $scope.ShowEdit = function () {
        $scope.QueryShow = false;
        $scope.EditShow = true;

        $scope.init();
    }

    $scope.EditData = function (data) {
        $http.post('/Home/Edit', { keyValueName: data.KeyValueName, text: data.Text })
            .then(function (result) {
                if (result.data.IsOk) {
                    $scope.IsCreate = "修改";
                    $scope.IsShowDelBtn = true;
                    $scope.ShowEdit();
                    $scope.data = result.data.Data;
                }
                else {
                    $.unblockUI();
                    alertify.alert(result.data.Message);
                    $scope.IsShowDelBtn = false;
                }
            });
    }

    $scope.SaveData = function () {
        if ($scope.data == null)
            return false;

        var url = '/Home/Update';

        $http.post(url,
            {
                data: $scope.data
            }).then(function (result) {
                if (result.data.IsOk) {
                    $scope.QueryAll();
                    $scope.ShowQuery();
                    alertify.success(result.data.Message);
                }
                else if (result.data.IsOk == false)
                    alertify.alert(result.data.Message);
                else {
                    alertify.error(result.data.Message);
                }
            });
    }
}