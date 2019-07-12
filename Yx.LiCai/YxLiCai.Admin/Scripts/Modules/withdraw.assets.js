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
        },
        auditList: function (parameter) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/WithdrawManager/GetUserWithdrawList",
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
        merchantWithdrawDs: function (parameter) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/WithdrawManager/GetMerchantWithdrawList",
                        dataType: "json",
                        data: parameter
                    }
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                },
                schema: {
                    model: {
                        id: 'id',
                        fields: {
                            id: { editable: false },
                            lName: { editable: false },
                            rName: { editable: false },
                            bank: { editable: false },
                            bank_card: { editable: false },
                            phone: { editable: false },
                            amount: { editable: false },
                            c_time: { editable: false },
                            auditor_id: { editable: false },
                            audit_time: { editable: false },
                            oprator_id: { editable: false },
                            pay_time: { editable: false },
                            status: { editable: false },
                            remark: { editable: false },
                            account_id:{editable: false}
                        }
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
    }
});