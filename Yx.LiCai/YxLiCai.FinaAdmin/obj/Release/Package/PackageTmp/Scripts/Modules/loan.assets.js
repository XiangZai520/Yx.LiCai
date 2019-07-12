define(function () {
    return {
        getPageList: function(param){
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/LoanManager/GetAll",
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
        },
    }
});