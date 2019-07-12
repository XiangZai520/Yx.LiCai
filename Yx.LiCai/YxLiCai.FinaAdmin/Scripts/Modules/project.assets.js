define(function () {
    return {
        projectDs: function (parameter) {     
            var ds = new kendo.data.DataSource({
                batch: true,
                transport: {
                    read: {
                        url: "/Product/GetProjectKeyValuePairList",
                        dataType: "json",
                        data: parameter
                    },
                    destory: {
                        url: "/Project/Delete",
                        dataType: "json"
                    }
                },
                schema: {
                    model: {
                        id: "ID"
                    }
                }
            });
            return ds;
        },
        jsonModel: function (productID, status) {             
             return ({
                Id: productID|| -1,
                ProductName: $("#ProductName").val(),
                ProductCategory: $("#ProductCategory").val(),
                ProductStatus: status || 0,
                CreatorID: typeof ($("#CreatorID")) == "undefined" ? -1 : $("#CreatorID").val(),
                YieldBase: $("#YieldBase").val(),
                YieldIncrease: $("#YieldIncrease").val(),
                YieldTop: $("#YieldTop").val(),
                ProductOrder: $("#ProductOrder").val(),
                IsAutoEnable: $('input[name="productRadio"]:checked').val(),
                ExpectedEnableDate: $("#ExpectedEnableDate").val(),
                ProductAmount: $("#ProductAmount").val(),
                Remark: typeof ($("#Remark")) == "undefined" ? "" : $("#Remark").val(),
                ProductStr: JSON.stringify($("#ProductList").val()),
                AuditorID: typeof ($("#AuditorID")) == "undefined" ? -1 : $("#AuditorID").val(),
                ProductDuration: $("#ProductDuration").val(),
            });
        }
    };
});