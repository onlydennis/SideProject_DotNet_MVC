app.controller('HomeController', HomeController);

function HomeController($scope, $http, $filter) {
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
    }

    $scope.ShowQuery();
}