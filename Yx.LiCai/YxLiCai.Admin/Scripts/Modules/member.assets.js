define(function () {
    return {
        memberDs: function (status) {
            var url = "/Member/GetMemberList/" + status;
            var ds = new kendo.data.DataSource({
                batch: true,
                pageSize: 10,
                transport: {
                    read: {
                        url: url,
                        dataType: "json"
                    }
                },
                schema: {
                    model: {
                        id: "Id"
                    }
                }
            });
            return ds;
        }
    }
});