define(function () {
    return {
        //提醒充值列表
        getReminderList: function (param) {
            var ds = new kendo.data.DataSource({
                type: "json",
                serverPaging: true,
                pageSize: 15,
                transport: {
                    read: {
                        type: "post",
                        url: "/ChargeManager/GetReminderList",
                        dataType: "json",
                        data: param
                    }
                },
                schema: {
                    model: {
                        id: "id"
                    }
                }
            });
            return ds;
        }
    }
});