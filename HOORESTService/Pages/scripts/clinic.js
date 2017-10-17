﻿
var app;
var SharedJS;
var _pageSize = 10;
var barcodelength = 11;
var todayDate = kendo.toString(kendo.parseDate(new Date()), 'MM/dd/yyyy');
var API, url;
$(document).ready(function () {
    kendo.mobile.ui.Drawer.current = null;
    app = new kendo.mobile.Application(
        $(document.body),
        {
            //skin: "flat",
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
    //$('#LoginWindow').data("kendoWindow").center().open();
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

var home_vm = {
    onViewInit: function () { },
    onViewShow: function (e) {
        
    }
}

var request_master_dataSource;
var request_master_grid_column = [];
var request_master = {
    onViewInit: function () { },
    onViewShow: function (e) {
        var grid = $("#request_masterGrid").data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }        
        var url = API + "Request/";
        request_master_dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: url,
                    dataType: "json",
                    type: "post",
                }
            },
            pageSize: _pageSize
        });
                        
        request_master_grid_column.push({
            field: 'branch_name',
            title: 'Branch',
            width: 50
        });
        request_master_grid_column.push({            
            field: 'transaction_date',
            title: 'Date',
            template: "#= kendo.toString(kendo.parseDate(transaction_date), 'MM/dd/yyyy') #",
            width: 50
        });
        request_master_grid_column.push({
            field: 'created_by_name',
            title: 'Created By',            
            width: 100
        });
        //request_master_grid_column.push({
        //    field: 'approved_by_name',
        //    title: 'Approved By',
        //    width: 100
        //});
        request_master_grid_column.push({
            command: { text: "View", click: request_master.showDetails }, title: " ", width: "50px"
        });

        $("#request_masterGrid").kendoGrid({
            dataSource: request_master_dataSource,
            pageable: true,
            height: 450,
            columns: request_master_grid_column,
            dataBound: function () {
                $("#request_masterGrid").find(".k-grid-toolbar").off("click");
                $("#request_masterGrid").find(".k-grid-toolbar").on("click", ".k-grid-CreateNewRequest", function (e) {
                    e.preventDefault();                    
                    SharedJS.navigateView("#request_view?ID=0");
                });

            },
            toolbar: [{ text: "Create New Request", iconClass: "k-icon k-add" }],
        });
    },
    showDetails: function (e) {
        e.preventDefault();        
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);        
        SharedJS.navigateView("#request_view?ID=" + data.request_id);
    }
}

var request_view_grid_window_items = [];
API = "http://119.93.105.48:83/HOOAPI.svc/";
if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
    API = "http://localhost:2518/HOOAPI.svc/";
}
var request_view_grid_window = kendo.observable({
    onWindowClose: function () {
        $("#request_view_grid_window").data('kendoWindow').close();
    },
    onWindowInsert: function () {
        var toUpper = $('#request_view_grid_window_note').val();        
        var item = {
            item_id: $('#request_view_grid_window_item').data("kendoComboBox").value(),
            item_name_fld: $('#request_view_grid_window_item').data("kendoComboBox").text(),
            quantity: $('#request_view_grid_window_quantity').data("kendoNumericTextBox").value(),
            note: toUpper.toUpperCase()
        };
        request_view_dataSource.add(item);
        $('#request_view_grid_window_item').data("kendoComboBox").value("");
        $('#request_view_grid_window_item').data("kendoComboBox").text("");
        $('#request_view_grid_window_quantity').data("kendoNumericTextBox").value("1");
        $('#request_view_grid_window_note').val("");
    },
    items: new kendo.data.DataSource({
        transport: {
            read: {
                url: API + "Particulars/",
                dataType: "json"
            },
        },
        sort: { field: "Name", dir: "asc" },
        schema: {
            data: function (data) {
                return data.GetParticularListResult;
            }
        }
    })
});

var request_view_dataSource;
var request_view = {
    onViewInit: function () {

        $("#request_view_grid_window").kendoWindow({
            actions: [],
            visible: false,
            title: false,
            modal: true,
            activate: function () {
                $('#request_view_grid_window_item').data("kendoComboBox").value("");
                $('#request_view_grid_window_item').data("kendoComboBox").text("");
                $('#request_view_grid_window_quantity').data("kendoNumericTextBox").value("1");
                $('#request_view_grid_window_note').val("");
            }
        });
        kendo.bind($("#request_view_grid_window"), request_view_grid_window);        
    },
    onViewShow: function (e) {
        var grid = $("#request_view_grid").data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }        
        var request_id = e.view.params.ID;     
        request_view_dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: API + "Request/" + request_id,
                    type: "get",
                    dataType: "json"
                }
            },
            schema: {
                data: function (data) {
                    debugger;
                    purchasing_view_po_id = data.request_id;
                    return [];
                    //if (data.po_id > 0) {
                    //    $("#purchasing_view_reference").val(data.reference);
                    //    $("#purchasing_view_supplier").val(data.supplier);
                    //    $("#purchasing_view_note").val(data.note);
                    //    $("#purchasing_view_trans_date").data("kendoDatePicker").value(kendo.toString(kendo.parseDate(data.transaction_date), 'MM/dd/yyyy'));
                    //    return data.details;
                    //}
                    //else {
                    //    $("#purchasing_view_reference").val('');
                    //    $("#purchasing_view_supplier").val('');
                    //    $("#purchasing_view_note").val('');
                    //    $("#purchasing_view_trans_date").data("kendoDatePicker").value(todayDate);
                    //    return [];
                    //}


                },
                total: "total_count"
            },
            pageSize: _pageSize

        });

        var request_view_column = [];
        request_view_column.push({
            field: 'item_id',
            title: 'Item #',
            hidden: true
        });
        request_view_column.push({
            field: 'item_name_fld',
            title: 'Item',
            width: 400
        });
        request_view_column.push({
            field: 'quantity',
            title: 'Quantity',
            width: 100
        });
        request_view_column.push({
            field: 'note',
            title: 'Note'            
        });
        request_view_column.push({            
            command: { text: " ", iconClass: "k-icon k-delete", click: request_view.deleteRow }, title: " ", width: "80px"
        });

        $("#request_view_grid").kendoGrid({
            dataSource: request_view_dataSource,
            pageable: true,
            height: 450,            
            columns: request_view_column,
            dataBound: function () {
                $("#request_view_grid").find(".k-grid-toolbar").off("click");
                $("#request_view_grid").find(".k-grid-toolbar").on("click", ".k-grid-AddDetails", function (e) {
                    e.preventDefault();
                    $("#request_view_grid_window").data('kendoWindow').open().center();
                });

            },
            toolbar: [{ text: "Add Details", iconClass: "k-icon k-add" }],
        });
    },
    deleteRow: function (e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = this.dataItem(tr);
        if (confirm('Do you really want to delete this record?')) {
            var dataSource = $("#request_view_grid").data("kendoGrid").dataSource;
            dataSource.remove(dataItem);
            dataSource.sync();
        }
    },
    onApproved: function (e) { },
    onSave: function (e) { },
    onCancel: function (e) {
        SharedJS.navigateView("#request_master");
    },

}
