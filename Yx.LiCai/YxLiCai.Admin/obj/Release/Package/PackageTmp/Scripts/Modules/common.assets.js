define(function () {

    var popupNotification = $("#popupNotification").kendoNotification({
        position: {
            pinned: true,
            top: 30,
            right: 80
        },
        autoHideAfter: 5000,
        stacking: "down"
    }).data("kendoNotification");

    return {
        popupNotification: popupNotification,
        findTextInDs: function (sourceDataSource, value) {
            if (value === null || value === "") {
                return "";
            }

            var data = sourceDataSource.data();
            for (var i = 0; i < data.length; i++) {
                if (data[i].Id == value) return data[i].Name;
            }
            return "";
        },
        findText: function (dataSource, value) {
            var dataItem = dataSource.get(value);
            return dataItem.text;
        },
        deleteRecord: function (grid, e, ds, action, status) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));

            $.ajax({
                url: action,
                data: { IdList: dataItem.id, status: status },
                success: function (result) {
                    if (result) {
                        ds.remove(dataItem);
                    }
                }
            });
        },
        showDetails: function (grid, e, action) {
            e.preventDefault();

            var dataItem = grid.dataItem($(e.currentTarget).closest("tr"));
            window.location.href = action + dataItem.Id;
        },    
        openFile: function (filename) {
            try {
                var obj = new ActiveXObject("wscript.shell");
                if (obj) {
                    obj.Run("\"" + filename + "\"", 1, false);
                    obj = null;
                }
            }
            catch (e) {
                alert(e);
                popupNotification.show("文件不存在");
            }
        }
    };
});