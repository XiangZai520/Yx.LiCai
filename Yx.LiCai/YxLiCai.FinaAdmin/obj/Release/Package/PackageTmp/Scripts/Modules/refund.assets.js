define(function () {
    return {
        //待还利息列表
        getPageList: function (param) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/RefundManager/GetRefundInterestList",
                        dataType: "json",
                        data: param
                    }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        getPrincipalList: function(e){
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/RefundManager/GetRefundPrincipalList",
                        dataType: "json",
                        data: param
                    }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
                schema: {
                    model: {
                        id: "id"
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        lending: function (grid, e, ds, action) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr")),
               recordID = dataItem.id,
               merchantID = dataItem.project.account_id,
               amount = dataItem.project.Amount,
               projectID = dataItem.pjid,
               bankCardNum = dataItem.BankCardInfo.BankCard;

            var param = {
                recordID: recordID, merchantID: merchantID, amount: amount,
                projectID: projectID, bankCardNum: bankCardNum
            };

            $.ajax({
                url: action,
                data: param,
                success: function (result) {
                    ds.remove(dataItem);
                    return result;
                }
            });
        }
    }
});