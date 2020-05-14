
var vm = new Vue({
    el: '#app',
    data: {
        user: [{ 'userid': '', 'id': '' }]
    },
    methods: {
        getdata() {
            var _this = this;
            function success(data) {
                _this.user = [];
                for (var i = 0; i < data.data.length; i++) {
                    _this.user.push({ 'userid': data.data[i].userId, 'id': data.data[i].id });
                }
            }
            AjaxHandler.get("/InterFace/GetData", success)
        }
    },
    mounted: function () {
        this.getdata();
    }
});