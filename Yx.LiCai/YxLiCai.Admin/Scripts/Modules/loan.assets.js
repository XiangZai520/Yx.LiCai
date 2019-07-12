define(function () {
    return {
        getPageList: function (param) {
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
        getLoanFailedList: function (param) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "POST",
                        url: "/LoanManager/GetLoanFailedList",
                        dataType: "json",
                        data: param
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
                            pjid: { editable: false },
                            pjName: { editable: false },
                            loanPeriod: { editable: false },
                            pjAmount: { editable: false },
                            amount: { editable: false },
                            mID: { editable: false },
                            mName: { editable: false },
                            rName: { editable: false },
                            phone: { editable: false },
                            bkName: { editable: false },
                            bkCard: { editable: false },
                            adminID: { editable: false },
                            loanTime: { editable: false },
                            remark: { editable: false }
                        }
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        getLoanLog: function (param) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/LoanManager/GetLoanLog",
                        dataType: "json",
                        data: param
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
                            pjid: { editable: false },
                            pjName: { editable: false },
                            loanPeriod: { editable: false },
                            pjAmount: { editable: false },
                            amount: { editable: false },
                            mID: { editable: false },
                            mName: { editable: false },
                            rName: { editable: false },
                            phone: { editable: false },
                            bkName: { editable: false },
                            bkCard: { editable: false },
                            adminID: { editable: false },
                            loanTime: { editable: false },
                            remark: { editable: false }
                        }
                    },
                    data: "DataResult",
                    total: "TotalCount"
                }
            });
            return ds;
        },
        pay: function (grid, e) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr")),
               recordID = dataItem.id,
               merchantID = dataItem.account_id,
               amount = dataItem.amount,
               projectID = dataItem.pj_id;

            var param = {
                recordID: recordID, merchantID: merchantID,
                amount: amount, projectID: projectID
            };

            return param;
        },
        rePay: function (grid, e) {
            e.preventDefault();
            var dataItem = grid.dataItem($(e.currentTarget).closest("tr")),
               recordID = dataItem.id,
               merchantID = dataItem.mID,
               amount = dataItem.amount,
               projectID = dataItem.pjid;

            var param = {
                recordID: recordID, merchantID: merchantID,
                amount: amount, projectID: projectID
            };

            return param;
        },
    }
});