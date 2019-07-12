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
                        id: 'Id',
                        fields: {
                            Id: { editable: false },
                            PCategory: { editable: false },
                            ProductName: { editable: false },
                            ProductAmount: { editable: false },
                            ExpectedEnableDate: { editable: false },
                            ProductOrder: { editable: false },
                            AutoEnable: { editable: false },
                            CreateDate: { editable: false },
                            IsSellPdt: { editable: false }
                        }
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        alertList: function (param, _url) {
            var ds = new kendo.data.DataSource({
                type: "json",
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: _url,
                        dataType: "json",
                        data: param
                    }
                },
                schema: {
                    model: {
                        id: "Id"
                    },
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
        toSellProductDs: function (parameter) {
            var url = "/Product/GetToSellProductList";
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
                        id: 'Id',
                        fields: {
                            Id: { editable: false },
                            PCategory: { editable: false },
                            ProductName: { editable: false },
                            ProductAmount: { editable: false },
                            ExpectedEnableDate: { editable: false },
                            ProductOrder: { editable: false },
                            AutoEnable: { editable: false },
                            CreateDate: { editable: false },
                            IsSellPdt: { editable: false }
                        }
                    },
                    data: "DataResult",
                    total: "TotalCount"
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
        }),
        onlineProductDs: new kendo.data.DataSource({
            type: "json",
            pageSize: 15,
            transport: {
                read: {
                    url: "/Product/GetOnlineList",
                    dataType: "json"
                },
            },
            schema: {
                model: {
                    id: 'id',
                    fields: {
                        id: { editable: false },
                        name: { editable: false },
                        category: { editable: false },
                        amount: { editable: false },
                        amount_raised: { editable: false },
                        amount_available: { editable: false },
                        is_alert: { editable: false },
                        c_time: { editable: false },
                        enable_time: { editable: false }
                    }
                }
            }
        }),
        durationDs: new kendo.data.DataSource({
            async: false,
            transport: {
                read: {
                    url: "/Product/GetConfig?configName=Product&element=ProductDuration",
                    dataType: "json"
                }
            }
        })
    }
});