define(function () {
    return {
        productDs: function (parameter, _url) {
            var url = _url || "/Product/GetProductList";
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: url,
                        dataType: "json",
                        data: parameter
                    }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
                schema: {
                    model: {
                        id: "Id"
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        interestRateDs: function (type) {
            var url = "/Product/GetConfig?configName=InterestRate&element=" + type;
            var ds = new kendo.data.DataSource({
                async: false,
                transport: {
                    read: {
                        url: url,
                        dataType: "json"
                    }
                }
            });
            return ds;
        },
        statusDs: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/Product/GetConfig?configName=Product&element=ProductStatus",
                    dataType: "json"
                }
            }
        }),
        categoryDs: new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/Product/GetConfig?configName=Product&element=ProductCategory",
                    dataType: "json"
                }
            }
        })
    }
});