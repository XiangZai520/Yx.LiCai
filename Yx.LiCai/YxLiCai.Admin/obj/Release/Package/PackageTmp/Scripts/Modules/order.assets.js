define(function () {
    return {
        orderDs: function (status) {
            var url = "/OrderManager/GetOrderList/" + status;
            var ds = new kendo.data.DataSource({
                batch: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: url,
                        dataType: "json"
                    }
                },
                schema: {
                    model: {
                        id: "Id"
                    }
                }
            });
            return ds;
        }
    }
});