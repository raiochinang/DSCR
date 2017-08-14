$(document).ready(function () {
    var API = "http://119.93.105.48:83/HOOAPI.svc/";    
    if (location.hostname === "localhost" || location.hostname === "127.0.0.1")
    {
        API = "http://localhost:2518/HOOAPI.svc/";
    } 
    var barcodelength = 11;

    var methods = {
        init: function () {
            $('#LoginWindow').kendoWindow({
                visible: false,
                title: false,
                modal: true,
                height: 180,
                width: 300,
                activate: function () {
                    $('.btnLogin').kendoButton({
                        click: function () {
                            methods.Login();
                        },
                        width: "100%"
                    });
                }
            });

            if ($('#UserBranch').val().length == 0) {
                $('#LoginWindow').data("kendoWindow").center().open();
            }
        
        },
        Login: function () {
            url = API + "Login/";
            parameter = {
                username: $('#LoginWindowUsername').val(),
                password: $('#LoginWindowPassword').val(),
            }
            service.post(url, JSON.stringify(parameter), function (data) {
                $('#UserBranch').val(data.branch.store_code_fld);
                $('#UserBranch').keydown(function (e) {                    
                    e.preventDefault();
                    return false;
                });
                $('#LoginWindow').data('kendoWindow').close();
            });
        },
    }

    var service = {
        post: function (url, parameter, callbackSuccess) {
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
    }
    methods.init();
});