var SharedJS;
$(document).ready(function () {
    kendo.mobile.ui.Drawer.current = null;
    var app = new kendo.mobile.Application(
        $(document.body),
        {
            skin: "flat",         
        });

    SharedJS = {
        postService: function (url, parameter, callbackSuccess) {
            var request = $.ajax({
                type: 'POST',
                url: url,
                data: parameter,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == "") {
                        data = "";
                    }
                    if ($.isFunction(callbackSuccess)) { callbackSuccess(data); }
                },
                error: function (data) {
                    $("#notification").data("kendoNotification").show(data.statusText, "error");
                }
            });
        },
        navigateView: function (viewId) {
            app.navigate(viewId);
        },
        onMenuInit: function (role) {
            //hide show menu
        },
        login: function () {
            url = API + "Login/";
            parameter = {
                username: $('#LoginWindowUsername').val(),
                password: $('#LoginWindowPassword').val(),
            }
            SharedJS.postService(url, JSON.stringify(parameter), SharedJS.loginCallback);
        },
        loginCallback: function (data) {
            if (data.username != null) {
                branch_id = data.branch.id;
                current_user_id = data.id;
                $('#LoginWindow').data('kendoWindow').close();
                var d = new Date();
                SharedJS.onMenuInit(data.role);
            }
        }
    }

   
    API = "http://119.93.105.48:83/HOOAPI.svc/";
    if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
        API = "http://localhost:2518/HOOAPI.svc/";
    }

    $('#LoginWindow').kendoWindow({
        visible: false,
        title: false,
        modal: true,
        height: 180,
        width: 300,
        activate: function () {
            $('.btnLogin').kendoButton({
                click: function () {
                    SharedJS.login();
                },
                width: "100%"
            });
        }
    });
    $('#LoginWindow').data("kendoWindow").center().open();
    $("#notification").kendoNotification({
        position: {
            pinned: true,
            top: 6,
            left: null,
            bottom: null,
            right: 10
        }
    });
});

var API, url;
var purchasing_master = {   
    onViewInit: function () {
        
    },
    onViewShow: function () {
        
    },
    onClick: function () {        
        var isProcess = 'Y';
        var url = API + "Purchasing/";
        var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: url,
                    dataType: "json",
                    type: "post",
                }
            },
            //schema: {
            //    data: "GetParticularListResult",
            //    total: function () {
            //        return 100;
            //    }
            //},
            pageSize: 10
        });
        var columns = [
            {field: 'note', title: 'Note'}
        ];
        $("#purchasing_masterGrid").kendoGrid({
            dataSource: dataSource,
            pageable: true,
            //columns: columns,
        });
    },
}

var purchasing_view = { 
    onViewInit: function () {

    },
    onViewShow: function () {
    },
}