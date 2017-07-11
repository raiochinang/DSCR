$(document).ready(function () {
    var app;
    var DSCRGridData = [];
    
    var dscrVM = kendo.observable({
        RR: "RR",
        SI: "",
        OR: "",
        BranchID: 0,
        Branch: "",
        DoctorID: 0,
        Doctor: "",
        Date: null,
        Time: null,
        Asst: "Asst",
        Cash: "0.00",
        Check: "0.00",
        AR: "0.00",
        CC: "0.00",
        Total: "0.00",
        DSCRDoctorSearch: function () {
            debugger;
        },
        DSCRBarcodeSearch: function () {
            debugger;
        },
        onClick: function (e) {
            var x = kendo.toString(this.DateTime, "MM/dd/yyyy hh:mm tt");

            debugger;
        },
        GridSave: function () {
            debugger;
        },
        products: new kendo.data.DataSource({
            schema: {
                model: {
                    fields: {
                        name: { type: "string" },
                        price: { type: "number", 'format': '{2:n2}' }
                    }
                }
            }
        }),
        CreditCard: new kendo.data.DataSource({
            schema: {
                model: {
                    fields: {
                        Name: { type: "string", template: "<input type='checkbox' class='checkbox' />" },
                        Amount: { type: "number", 'format': '{2:n2}' }
                    }
                }
            }
        })
    });

    var reportVM = kendo.observable({
        Text1: ""
    });

  
    var DSCRWindowVM = kendo.observable({
        Close: function (e) {
            var dialog = $("#DSCRWindow").data("kendoMobileModalView");
            dialog.close();
        },
        Post: function (e) {
            var dialog = $("#DSCRWindow").data("kendoMobileModalView");
            dialog.close();
        },
        Reset: function (e) {
            var dialog = $("#DSCRWindow").data("kendoMobileModalView");
            dialog.close();
        },
        Particular: "Particular1",
        Qty: "322",
        Gross: "11.1"
    });



    var methods = {
        Init: function () {
            app = new kendo.mobile.Application(document.body, { skin: "flat" });
            kendo.bind("#mainview", dscrVM);
            kendo.bind("#DSCRWindow", DSCRWindowVM);
          
            var header = $('#header').height();
            var row1 = $('#row1').height();
            var footer = $('#footer').height();

            $("#DSCRTime").kendoTimePicker({
                animation: false,
                format: "h:mm tt",
                interval: 10,
                min: new Date(2017, 0, 1, 10, 0, 0),
                max: new Date(2099, 0, 1, 22, 0, 0),
                parseFormats: ["HH:mm"]
            });

            $("#DSCRDate").kendoDatePicker({
                min: new Date(2017, 0, 1), // sets min date to Jan 1st, 2017
                max: new Date(2020, 0, 1) // sets min date to Jan 1st, 2020
            });

            $(".r-numeric-textbox").kendoNumericTextBox({
                decimals: 2,
                spinners: false,
                min: 0
            });


            $("#paymentGrid").kendoGrid({
                columns: [
                            { field: "Amt", title: "Amount", format: "{0:n}", width: "80px", attributes: { style: "text-align:right;" } },
                            { field: "CC", title: "Credit Card", width: "60px", editor: methods.categoryDropDownEditor, template: "#=CC.CategoryName#" },
                            { command: [{ name: "destroy", text: "", width: 20 }], title: " ", width: 55 }
                ],
                dataSource: new kendo.data.DataSource({
                    data: [],
                    schema: {
                        model: {
                            fields: {
                                CC: { defaultValue: { CategoryID: 1, CategoryName: "BPI" } },
                                Amt: { type: "number", validation: { required: true, min: 1 } }
                            }
                        }
                    }
                }),
                toolbar: [{ name: "create", text: "", width: 20 }],
                editable: true,
                height: 200,
                save: function (e) {
                    setTimeout(function () {
                        var data = $("#paymentGrid").data('kendoGrid').dataSource.data();
                        var total = 0;
                        $(data).each(function (k, v) {
                            total += v.Amt;
                        });
                        $("#DSCRTotal").data("kendoNumericTextBox").value(total);
                        $("#DSCRTotal").data("kendoNumericTextBox").trigger("change");
                    }, 100);
                },
                remove: function () {
                    setTimeout(function () {
                        var data = $("#paymentGrid").data('kendoGrid').dataSource.data();
                        var total = 0;
                        $(data).each(function (k, v) {
                            total += v.Amt;
                        });
                        $("#DSCRTotal").data("kendoNumericTextBox").value(total);
                        $("#DSCRTotal").data("kendoNumericTextBox").trigger("change");
                    }, 100);
                },
            });

            $("#DSCRGrid").kendoGrid({
                editable: "inline",
                toolbar: kendo.template($("#DSCRGridToolbarTemplate").html()),
                columns: [
                            { field: "Particular", title: "Particular", width: 300, footerTemplate: "Total:" },
                            { field: "Qty", title: "Qty/Sess", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:center'>#=sum #</div>", attributes: { style: "text-align:center;" } },
                            { field: "Gross", title: "Gross", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "Discount", title: "Discount", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "Deduction", title: "Deduction", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "AdvPayment", title: "Adv.Payment", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "ProgAvail", title: "Prog.Avail", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "AddOns", title: "AddOns", width: 80, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "NetSale", title: "NetSale", width: 90, aggregates: ["sum"], footerTemplate: "<div style='text-align:right'>#=kendo.toString(sum, 'N2') #</div>", attributes: { style: "text-align:right;" }, format: "{0:n2}" },
                            { field: "Explanation", title: "Explanation", width: 300 },
                            { command: [{ name: "destroy", text: "", width: 20 }], title: " ", width: 55 }
                ],
                height: 450,
                scrollable: true,
                dataSource: new kendo.data.DataSource({
                    data: DSCRGridData,
                    aggregate: [
                            { field: "Qty", aggregate: "sum" },
                            { field: "Gross", aggregate: "sum" },
                            { field: "Discount", aggregate: "sum" },
                            { field: "Deduction", aggregate: "sum" },
                            { field: "AdvPayment", aggregate: "sum" },
                            { field: "ProgAvail", aggregate: "sum" },
                            { field: "AddOns", aggregate: "sum" },
                            { field: "NetSale", aggregate: "sum" },
                    ]
                }),
                dataBound: function () {
                    $('#DSCRBarcode').off('keyup').on('keyup', function () {
                        var dialog = $("#DSCRWindow").data("kendoMobileModalView");
                        dialog.open();
                    });
                }
            });

            $('#DSCRBarcodeSearch').off('click').on('click', function () {
                debugger;
            });
        },        
        categoryDropDownEditor: function (container, options) {
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoComboBox({
                    autoBind: false,
                    dataTextField: "CategoryName",
                    dataValueField: "CategoryID",
                    dataSource: [
                        { "CategoryID": 1, "CategoryName": "BPI" },
                        { "CategoryID": 2, "CategoryName": "BDO" }
                    ]
                });
        }
    }

    var Service = {
        
    }


    methods.Init();
});