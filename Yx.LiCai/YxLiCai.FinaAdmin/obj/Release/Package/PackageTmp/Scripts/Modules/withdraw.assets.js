define(function () {
    return {
        withdrawDs: function (parameter) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/WithdrawManager/GetWithdrawList",
                        dataType: "json",
                        data: parameter
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
        redemptionDs: function (parameter) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/RedemptionManager/GetRedemptionList",
                        dataType: "json",
                        data: parameter
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
        paySpecifiedRecord: function (grid, e, ds, action) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr")),
                recordID = dataItem.id,
                userID = dataItem.user_id,
                amount_principal = dataItem.amount_principal,
                amount_balance = dataItem.amount_balance,
                bankCardNum = dataItem.BankCardInfo.BankCard,
                phone = dataItem.UserInfo.Phone;

            var param = {
                recordID: recordID, userID: userID, amount_principal: amount_principal,
                amount_balance: amount_balance, phone: phone, bankCardNum: bankCardNum
            };

            $.ajax({
                url: action,
                data: param,
                success: function (result) {
                    if (result) {
                        ds.remove(dataItem);
                    }
                    else {
                        alert("打款失败");
                    }
                }
            });
        },        
        payRedemption: function (grid, e, ds, action) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr")),
               recordID = dataItem.id,
               userID = dataItem.user_id,
               amount = dataItem.amount,
               principal = dataItem.atone_benjin,
               interest = dataItem.atone_lixi,
               PenalSum = dataItem.PenalSum,
               orderID = dataItem.order_id,
               bankCardNum = dataItem.BankCardInfo.BankCard,
               phone = dataItem.UserInfo.Phone;

            var param = {
                recordID: recordID, userID: userID, amount: amount, principal: principal,
                interest: interest, penalSum: PenalSum, orderID: orderID, phone: phone, bankCardNum: bankCardNum
            };

            $.ajax({
                url: action,
                data: param,
                success: function (result) {
                    if (result) {
                        ds.remove(dataItem);
                    }
                    else {
                        alert("打款失败");
                    }
                }
            });
        }
    }
});