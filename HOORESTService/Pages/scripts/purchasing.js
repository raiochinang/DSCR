
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

var home_vm = {
    onViewInit: function () { },
    onViewShow: function (e) {
        debugger;
    }
}

var purchasing_master = {
    onViewInit: function () { },
    onViewShow: function () {
        var grid = $("#purchasing_masterGrid").data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }
        var columns = [];
        columns.push({
            command: { text: "View", click: purchasing_master.showDetails }, title: " ", width: "100px"
        });
        columns.push({
            field: 'po_id',
            title: 'Purchase #',
            width: 100,
            hidden: true
        });
        columns.push({
            field: 'transaction_date',
            title: 'Date',
            template: "#= kendo.toString(kendo.parseDate(transaction_date), 'MM/dd/yyyy') #",
            width: 100            
        });        
        columns.push({
            field: 'reference',
            title: 'Reference #'
        });
        columns.push({
            field: 'supplier',
            title: 'Supplier'
        });
        columns.push({
            field: 'note',
            title: 'Note'
        });
        columns.push({
            field: 'created_by',
            title: 'Created By'
        });
        $("#purchasing_masterGrid").kendoGrid({
            dataSource: [],
            pageable: true,
            height: 540,
            columns: columns,
            pageable: true,
        });
    },
    onRefresh: function () {
        var grid = $("#purchasing_masterGrid").data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }

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
            pageSize: _pageSize
        });
        var columns = [];
        columns.push({
            command: { text: "View", click: purchasing_master.showDetails }, title: " ", width: "100px"
        });
        columns.push({
            field: 'po_id',
            title: 'Purchase #',
            template: "#= kendo.toString(po_id, '0000000000') #",
            width: 100,
            hidden: true
        });
        columns.push({
            field: 'transaction_date',
            title: 'Date',
            template: "#= kendo.toString(kendo.parseDate(transaction_date), 'MM/dd/yyyy') #",
            width: 100
        });

        columns.push({
            field: 'reference',
            title: 'Reference #'
        });
        columns.push({
            field: 'supplier',
            title: 'Supplier'
        });
        columns.push({
            field: 'note',
            title: 'Note'
        });
        columns.push({
            field: 'created_by',
            title: 'Created By'
        });
        $("#purchasing_masterGrid").kendoGrid({
            dataSource: dataSource,
            pageable: true,
            height: 540,
            columns: columns,
            pageable: true,
            toolbar: [{ text: "New Record", iconClass: "k-icon k-add" }],
            dataBound: function () {
                $("#purchasing_masterGrid").find(".k-grid-toolbar").off("click");
                $("#purchasing_masterGrid").find(".k-grid-toolbar").on("click", ".k-grid-NewRecord", function (e) {
                    e.preventDefault();
                    purchasing_master.onAddRecord();
                });
            }
        });


    },
    showDetails: function (e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);
        SharedJS.navigateView("#purchasing_view?ID=" + data.po_id);
    },
    onAddRecord: function () {
        SharedJS.navigateView("#purchasing_view?ID=0");
    }
}

var purchasing_view_dataSource;
var purchasing_view_po_id;
var purchasing_view = {
    onViewInit: function () {
        kendo.bind($("#purchasing_view_grid_window"), purchasing_view_grid_window);
    },
    onViewShow: function (e) {
        var grid = $("#purchasing_view_grid").data("kendoGrid");
        if (grid != undefined) {
            grid.destroy();
        }

        var po_id = e.view.params.ID;        
        var url = API + "Purchasing/" + po_id;
        purchasing_view_dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: url,
                    type: "get",
                    dataType: "json"
                }
            },
            schema: {
                data: function (data) {
                    purchasing_view_po_id = data.po_id;
                    if (data.po_id > 0) {                        
                        $("#purchasing_view_reference").val(data.reference);
                        $("#purchasing_view_supplier").val(data.supplier);
                        $("#purchasing_view_note").val(data.note);
                        $("#purchasing_view_trans_date").data("kendoDatePicker").value(kendo.toString(kendo.parseDate(data.transaction_date), 'MM/dd/yyyy'));
                        return data.details;
                    }
                    else {
                        $("#purchasing_view_reference").val('');
                        $("#purchasing_view_supplier").val('');
                        $("#purchasing_view_note").val('');
                        $("#purchasing_view_trans_date").data("kendoDatePicker").value(todayDate);
                        return [];
                    }


                },
                total: "total_count"
            },
            pageSize: _pageSize

        });
        var purchasing_view_grid_column = [];

        purchasing_view_grid_column.push({
            field: 'item_id',
            title: 'Item ID',
            hidden: true
        });
        purchasing_view_grid_column.push({
            field: 'item_name_fld',
            title: 'Item',
            width: 400
        });
        purchasing_view_grid_column.push({
            field: 'lot_number',
            title: 'Lot #'
        });
        purchasing_view_grid_column.push({
            field: 'expiration_date',
            title: 'Expiration Date',
            template: "#= kendo.toString(kendo.parseDate(expiration_date), 'MM/dd/yyyy') #",            
        });
        purchasing_view_grid_column.push({
            field: 'currency',
            title: 'Currency'
        });
        purchasing_view_grid_column.push({
            field: 'quantity',
            title: 'Quantity',
            format: '{0:n0}',
            attributes: {
                style: "text-align: right;"
            }
        });
        purchasing_view_grid_column.push({
            field: 'md_price',
            title: 'MD Price',
            format: '{0:n}',
            attributes: {
                style: "text-align: right;"
            }
        });
        purchasing_view_grid_column.push({
            field: 'actual_cost',
            title: 'Actual Cost',
            format: '{0:n}',
            attributes: {
                style: "text-align: right;"
            }
        });
        purchasing_view_grid_column.push({
            command: { text: " ", iconClass: "k-icon k-delete", click: purchasing_view.deleteRow }, title: " ", width: "80px"
        });


        $("#purchasing_view_grid").kendoGrid({
            dataSource: purchasing_view_dataSource,
            pageable: true,
            height: 450,
            columns: purchasing_view_grid_column,
            dataBound: function () {
                $("#purchasing_view_grid").find(".k-grid-toolbar").off("click");
                $("#purchasing_view_grid").find(".k-grid-toolbar").on("click", ".k-grid-AddDetail", function (e) {
                    e.preventDefault();
                    purchasing_view.onAddRow();
                });

            },
            toolbar: [{ text: "Add Detail", iconClass: "k-icon k-add" }],
        });
    },
    transDateSelectedDate: todayDate,
    onChangeTransDate: function () {
        purchasing_view_TransDate = kendo.toString(this.get("transDateSelectedDate"), "MM/dd/yyyy");
    },
    onSave: function (e) {
        e.preventDefault();        
        var details = purchasing_view_dataSource.data();
        var param = {
            po_id: purchasing_view_po_id,
            transaction_date: kendo.toString($("#purchasing_view_trans_date").data('kendoDatePicker').value(), "u").replace('Z', ''),
            reference: $("#purchasing_view_reference").val(),
            supplier: $("#purchasing_view_supplier").val(),
            note: $("#purchasing_view_note").val(),
            created_by: 12,//current_user_id,
            details: details
        }
        if (purchasing_view_po_id > 0) {
            //update
            url = API + "Purchasing/Update";
        }
        else {
            //insert
            url = API + "Purchasing/Insert";            
        }
        SharedJS.postService(url, JSON.stringify(param), purchasing_view.onSaveCallBack);
        
    },
    onSaveCallBack: function (data) {
        $("#notification").data("kendoNotification").show("Saving Successfully.", "success");
    },
    deleteRow: function (e) {
        e.preventDefault();
        var tr = $(e.target).closest("tr");
        var dataItem = this.dataItem(tr);
        if (confirm('Do you really want to delete this record?')) {
            var dataSource = $("#purchasing_view_grid").data("kendoGrid").dataSource;
            dataSource.remove(dataItem);
            dataSource.sync();
        }
    },
    onAddRow: function () {        
        $("#purchasing_view_grid_window").data("kendoWindow").open().center();
        
    },
    onCancel: function () {
        SharedJS.navigateView("#purchasing_master");
    },
}

var delivery_receipt_master = {
    onViewInit: function () { },
    onViewShow: function () { }
}

var clinic_request_master = {
    onViewInit: function () { },
    onViewShow: function () { }
}

var purchasing_view_grid_window = kendo.observable({
    onOpen: function () {
        $('#purchasing_view_grid_window_barcode').off('input');
        $('#purchasing_view_grid_window_barcode').on('input', function () {
            if ($('#purchasing_view_grid_window_barcode').val().length > barcodelength) {
                setTimeout(purchasing_view_grid_window.onBarcode($('#purchasing_view_grid_window_barcode').val()), 500);
            }
        });
    },
    isVisible: false,
    isEnabled: true,
    onWindowClose: function (e) {
        $("#purchasing_view_grid_window").data("kendoWindow").close();
    },
    onWindowInsert: function () {
        if ($("#purchasing_view_grid_window_item").text() > "") {
            var date = $("#purchasing_view_grid_window_expiration_date").data('kendoDatePicker').value();
            if (date == null) {
                date = todayDate;
            }
            else {
                date = kendo.toString(date, "u").replace("Z", "");
            }
            var details = {
                item_id: $("#purchasing_view_grid_window_item").data("item_id"),
                item_name_fld: $("#purchasing_view_grid_window_item").text(),
                lot_number: $("#purchasing_view_grid_window_lotnum").text(),
                quantity: $("#purchasing_view_grid_window_quantity").data("kendoNumericTextBox").value(),
                currency: $("#purchasing_view_grid_window_currency").data("kendoComboBox").value(),
                expiration_date: date,
                md_price: $("#purchasing_view_grid_window_md_price").data("kendoNumericTextBox").value(),
                actual_cost: $("#purchasing_view_grid_window_actual_cost").data("kendoNumericTextBox").value(),
            }
            purchasing_view_dataSource.add(details);
            $("#purchasing_view_grid_window_item").attr("data-item_id", 0);
            $("#purchasing_view_grid_window_item").text('');
            $("#purchasing_view_grid_window_lotnum").text('');
            $("#purchasing_view_grid_window_quantity").data("kendoNumericTextBox").value(0);
            $("#purchasing_view_grid_window_currency").data("kendoComboBox").value('');
            $("#purchasing_view_grid_window_expiration_date").data('kendoDatePicker').value('');
            $("#purchasing_view_grid_window_md_price").data("kendoNumericTextBox").value(0);
            $("#purchasing_view_grid_window_actual_cost").data("kendoNumericTextBox").value(0)
            $("#purchasing_view_grid_window_barcode").val("").focus();
        }
    },
    Currencys: ["USD", "PHP", "EUR", "CNY", "JPY"],
    onBarcode: function (barcode) {
        $.get(API + "Particulars/" + barcode, function (data, status) {
            if (data.SearchParticularsResult.length > 0) {
                var lotnum = barcode.substring(5, 12);
                $("#purchasing_view_grid_window_item").text(data.SearchParticularsResult[0].Name);
                $("#purchasing_view_grid_window_item").attr("data-item_id", data.SearchParticularsResult[0].Id);
                $("#purchasing_view_grid_window_lotnum").text(lotnum);
            }
            else {
                $("#purchasing_view_grid_window_item").text('');
                $("#purchasing_view_grid_window_lotnum").text('');
                $("#purchasing_view_grid_window_item").attr("data-item_id", "0");
            }
        });
    },    
});