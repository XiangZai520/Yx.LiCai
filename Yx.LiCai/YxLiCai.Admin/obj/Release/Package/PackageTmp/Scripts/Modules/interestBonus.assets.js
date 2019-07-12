define(function () {
    return {
        categoryDs: function () {
            var url = "/InterestBonus/GetCategoryList";
            var ds = new kendo.data.DataSource({
                pageSize: 15,
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