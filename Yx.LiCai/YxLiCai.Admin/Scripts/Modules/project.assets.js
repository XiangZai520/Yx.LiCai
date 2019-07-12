define(function () {
    return {
        projectDs: function (parameter) {     
            var ds = new kendo.data.DataSource({
                batch: true,
                transport: {
                    read: {
                        type:"POST",
                        url: "/Product/GetProjectKeyValuePairList",
                        dataType: "json",
                        data: parameter
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
                Id: productID || -1,
                ProductName: $("#ProductName").val(),
                ProductCategory: $("#ProductCategory").val(),
                ProductStatus: status || 0,
                CreatorID: $("#CreatorID").val() || -1,
                YieldBase: $("#YieldBase").val() > 0 ? $("#YieldBase").val() : $("#YieldTop").val(),
                YieldIncrease: $("#YieldIncrease").val(),
                YieldTop: $("#YieldTop").val(),
                ProductOrder: $("#ProductOrder").val(),
                IsAutoEnable: $('input[name="productRadio"]:checked').val(),
                ExpectedEnableDate: $("#ExpectedEnableDate").val(),
                ProductAmount: $("#ProductAmount").val(),
                Remark: $("#Remark").val() || "",
                ProductStr: JSON.stringify($("#ProductList").val()),
                AuditorID: $("#AuditorID").val() || -1,
                ProductDuration: $("#ProductDuration").val(),
            });
        }
    };
});