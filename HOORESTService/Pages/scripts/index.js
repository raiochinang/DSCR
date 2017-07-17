$(document).ready(function () {
    kendo.mobile.ui.Drawer.current = null;
    var app;
    var url;
    var parameter;
    var todayDate;
    var CheckWindowGridData = [];
    var CreditCardWindowGridData = [];

    //DSCR
    var DSCR = new Object();
    var doctor_id = 0,
        nurse_id = 0,
        patient_id = 0,
        cashier_id = 0,
        branch_id = 0,
        current_user_id = 0;
    var checkdata = [];
    var creditcarddata = [];
    var MyDetails = [];


    //Settings
    var API = "http://localhost:2518/HOOAPI.svc/";
    var PrintURL = "http://localhost:2518/Pages/MyInvoice.aspx?P=";
    var barcodelength = 5; //11


    var methods = {
        init: function () {
            todayDate = kendo.toString(kendo.parseDate(new Date()), 'MM/dd/yyyy');
            app = new kendo.mobile.Application(document.body, { skin: "flat" });
            $('.btnCancel').kendoButton({
                click: function () {
                    methods.Cancel();
                }
            });
            $('.btnSave').kendoButton({
                click: function () {
                    methods.Save();
                }
            });
            $('.btnPrint').kendoButton({
                click: function () {
                    methods.Print();
                }
            });
            $('#dscrDoctor').keyup(function (e) {
                if (e.keyCode == 13 && $(this).val().length > 0) {
                    methods.SearchDoctor();
                }
            });
            $("#btnDoctorSearch").kendoButton({
                icon: "search",
                click: function () {
                    methods.SearchDoctor();
                }
            });
            $('#dscrPAsst').keyup(function (e) {
                if (e.keyCode == 13 && $(this).val().length > 0) {
                    methods.SearchPAsst();
                }
            });
            $("#btnPAsstSearch").kendoButton({
                icon: "search",
                click: function () {
                    methods.SearchPAsst();
                }
            });
            $('#dscrPatient').keyup(function (e) {
                if (e.keyCode == 13 && $(this).val().length > 0) {
                    methods.SearchPatient();
                }
            });
            $("#btnPatientSearch").kendoButton({
                icon: "search",
                click: function () {
                    methods.SearchPatient();
                }
            });
            $("#dscrDate").kendoDatePicker({
                animation: false,
                value: todayDate
            });
            $("#dscrFromSearch").kendoDatePicker({
                animation: false,
                value: todayDate,
            });
            $("#dscrToSearch").kendoDatePicker({
                animation: false,
                value: todayDate,
            });
            $("#dscrTimeFrom").kendoTimePicker({
                animation: false,
                max: new Date(2000, 0, 1, 22, 0, 0),
                min: new Date(2000, 0, 1, 9, 0, 0)
            });
            $("#dscrTimeTo").kendoTimePicker({
                animation: false,
                max: new Date(2000, 0, 1, 22, 0, 0),
                min: new Date(2000, 0, 1, 9, 0, 0)
            });
            $("#dscrCash").kendoNumericTextBox({
                decimals: 2,
                value: 0,
                min: 0,
                spinners: false,
                change: methods.ComputeAR
            });
            $("#btnCheckAdd").kendoButton({
                icon: "plus",
                click: function () {
                    methods.AddCheck();
                }
            });
            $('#dscrRR').keyup(function (e) {
                if (e.keyCode == 13 && $(this).val().length > 0) {
                    methods.SearchDSCR();
                }
            });
            $("#btnRRSearch").kendoButton({
                icon: "search",
                click: function () {

                },
            });
            $("#btnRRSearchSummary").kendoButton({
                icon: "search",
                click: function () {
                    methods.RRSearchSummary();
                }
            });
            $("#btnDateRangeSearchSummary").kendoButton({
                icon: "search",
                click: function () {
                    methods.DateRangeSearchSummary();
                }
            });

            $("#dscrAR").kendoNumericTextBox({
                decimals: 2,
                spinners: false,
            });
            $("#btnCreditCardAdd").kendoButton({
                icon: "plus",
                click: function () {
                    methods.AddCreditCard();
                }
            });
            $("#dscrTotalPayment").kendoNumericTextBox({
                decimals: 2,
                value: 0,
                spinners: false,
            });
            $('#dscrCashier').keyup(function (e) {
                if (e.keyCode == 13 && $(this).val().length > 0) {
                    methods.SearchCashier();
                }
            });
            $("#btnCashierSearch").kendoButton({
                icon: "search",
                click: function () {
                    methods.SearchCashier();
                }
            });
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
            $('#DoctorWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 600,
                activate: function () {
                    $('#btnDoctorWindowCancel').kendoButton({
                        click: function () {
                            $('#DoctorWindow').data("kendoWindow").close();
                        },
                        width: "100%"
                    });
                },
                close: function () {
                    $('#dscrDoctor').focus();
                }
            });
            $('#NurseWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 600,
                activate: function () {
                    $('#btnNurseWindowCancel').kendoButton({
                        click: function () {
                            $('#NurseWindow').data("kendoWindow").close();
                        },
                        width: "100%"
                    });
                },
                close: function () {
                    $('#dscrPAsst').focus();
                }
            });
            $('#PatientWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 600,
                activate: function () {
                    $('#btnPatientWindowCancel').kendoButton({
                        click: function () {
                            $('#PatientWindow').data("kendoWindow").close();
                        },
                        width: "100%"
                    });
                },
                close: function () {
                    $('#dscrPatient').focus();
                }
            });
            $('#CheckWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 700,
                open: function () {
                    $('#CheckWindowGrid').find('.k-grid-add').html('<span class="k-icon k-add"></span>Add Check');
                },
                activate: function () {
                    $('#btnCheckWindowCancel').kendoButton({
                        click: function () {
                            $('#CheckWindow').data("kendoWindow").close();
                        },
                    });
                    $('#btnCheckWindowConfirm').kendoButton({
                        click: function () {
                            var displayedData = $("#CheckWindowGrid").data().kendoGrid.dataSource.view();
                            var json = JSON.stringify(displayedData);
                            var obj = JSON.parse(json);
                            var total = 0;
                            checkdata = [];
                            $(obj).each(function (k, v) {
                                var o = new Object();
                                o.bank_name = v.Bank;
                                o.date_of_check = v.CheckDate.substring(0, 10) + " 00:00:00",
                                o.check_number = v.CheckNumber;
                                o.amount = v.Amount;
                                total += v.Amount;
                                checkdata.push(o);
                            });
                            $('#dscrCheck').val(kendo.toString(total, "n2"));
                            methods.ComputeAR();
                            $('#CheckWindow').data("kendoWindow").close();
                        },
                    });

                }
            });
            $('#CreditCardWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 700,
                open: function () {
                    $('#CreditCardWindow').find('.k-grid-add').html('<span class="k-icon k-add"></span>Add Credit Card');
                },
                activate: function () {
                    $('#btnCreditCardWindowCancel').kendoButton({
                        click: function () {
                            $('#CreditCardWindow').data("kendoWindow").close();
                        },
                    });
                    $('#btnCreditCardWindowConfirm').kendoButton({
                        click: function () {
                            var displayedData = $("#CreditCardWindowGrid").data().kendoGrid.dataSource.view();
                            var json = JSON.stringify(displayedData);
                            var obj = JSON.parse(json);
                            var total = 0;
                            creditcarddata = [];
                            $(obj).each(function (k, v) {
                                var o = new Object();
                                o.name = v.Bank.Text;
                                o.term = v.Terms.Text;
                                o.amount = v.Amount;
                                total += v.Amount;
                                o.note = v.Note;
                                creditcarddata.push(o);

                            });
                            $('#dscrCreditCard').val(kendo.toString(total, "n2"));
                            methods.ComputeAR();
                            $('#CreditCardWindow').data("kendoWindow").close();
                        },
                    });

                }
            });
            $('#CashierWindow').kendoWindow({
                visible: false,
                title: false,
                scrollable: true,
                modal: true,
                height: 320,
                width: 600,
                activate: function () {
                    $('#btnCashierWindowCancel').kendoButton({
                        click: function () {
                            $('#CashierWindow').data("kendoWindow").close();
                        },
                        width: "100%"
                    });
                },
                close: function () {
                    $('#dscrCashier').focus();
                }
            });
            $("#DSCRWindowGross").kendoNumericTextBox({
                decimals: 2,
                spinners: false,
            });
            $("#DSCRWindowQty").kendoNumericTextBox({
                spinners: false,
                format: "n0",
                value: 1,
                change: function () {
                    var value = this.value();
                    $("#DSCRWindowSession").data('kendoNumericTextBox').value(0);
                    var gross = $('#particularName').data('gross');
                    var TotalGross = parseFloat(gross) * value;
                    $("#DSCRWindowGross").data('kendoNumericTextBox').value(TotalGross);
                    methods.ComputeNetSale();
                }
            });
            $("#DSCRWindowSession").kendoNumericTextBox({
                spinners: false,
                format: "n0",
                value: 0,
                change: function () {
                    var value = this.value();
                    $("#DSCRWindowQty").data('kendoNumericTextBox').value(0);
                    var gross = $('#particularName').data('gross');
                    var TotalGross = parseFloat(gross) * value;
                    $("#DSCRWindowGross").data('kendoNumericTextBox').value(TotalGross);
                    methods.ComputeNetSale();
                }
            });
            $("#DSCRWindowGross").kendoNumericTextBox({
                decimals: 2,
                spinners: false,
                value: 0,
                min: 0
            });
            $(".DSCRWindowDecimal").kendoNumericTextBox({
                decimals: 2,
                min: 0,
                value: 0,
                spinners: false,
                change: function () {
                    methods.ComputeNetSale();
                }
            });
            $('#DSCRWindow').kendoWindow({
                visible: false,
                title: false,
                modal: true,
                height: 550,
                width: 400,
                open: function () {
                    $('#btnDSCRWindowPost').kendoButton({
                        click: function () {
                            methods.PostDSCR();
                        },
                        width: "100%"
                    });
                    $('#btnDSCRWindowReset').kendoButton({
                        click: function () {
                            methods.ResetDSCR();
                        },
                        width: "100%"
                    });
                    $('#btnDSCRWindowClose').kendoButton({
                        click: function () {
                            $('#DSCRWindow').data("kendoWindow").close();
                        },
                        width: "100%"
                    });
                },
                activate: function () {
                    //$('.btnLogin').kendoButton({
                    //    click: function () {
                    //        methods.Login();
                    //    },
                    //    width: "100%"
                    //});
                }
            });
            $("#notification").kendoNotification({
                position: {
                    pinned: true,
                    top: 6,
                    left: null,
                    bottom: null,
                    right: 10
                }
            });
            $("#dscrContent").kendoValidator({
                validateOnBlur: false,
                errorTemplate: "<span>#=message#</span>"
            });
            $('#Barcode').on('input', function () {
                if ($('#Barcode').val().length > barcodelength) {
                    methods.Barcode();
                }
            });
            methods.BindGrid();
            $.get(API + "branch", function (data, status) {
                var ds = [];
                $(data.BranchListResult).each(function (k, v) {
                    var item = { value: v.id, text: v.store_name_fld };
                    ds.push(item);
                });

                $('#branchddl').kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: ds,
                    enable: false,
                });
                methods.Styles();

            });
            $('#LoginWindow').data("kendoWindow").center().open();
            methods.BindCheckGrid();
            methods.BindCreditCardGrid();
        },
        Styles: function () {
            $("#dscrDate").css({
                'width': '90%',
                'left': '2px',
            });

            $("#dscrTimeFrom").css({
                'width': '90%',
                'left': '2px',
            });
            $("#dscrTimeTo").css({
                'width': '90%',
                'left': '2px',
            });
        },
        Login: function () {
            url = API + "Login/";
            parameter = {
                username: $('#LoginWindowUsername').val(),
                password: $('#LoginWindowPassword').val(),
            }
            service.post(url, JSON.stringify(parameter), callback.Login);
        },
        SearchDoctor: function () {
            var name = $("#dscrDoctor").val();
            if (name.length > 0) {
                $.get(API + "doctor/" + name, function (data, status) {
                    var ds = [];
                    var template = kendo.template($("#NoRecordTemplate").html());
                    if ($(data.DoctorsResult).length > 0) {
                        $(data.DoctorsResult).each(function (k, v) {
                            var item = { id: v.id, name: v.name };
                            ds.push(item);
                        });
                        template = kendo.template($("#DoctorWindowListTemplate").html());
                    }
                    else {
                        var item = { id: 0, name: "No Record" };
                        ds.push(item);
                    }

                    var dataSource = new kendo.data.DataSource({
                        data: ds
                    });
                    $("#DoctorWindowList").kendoListView({
                        dataSource: ds,
                        template: template,
                        dataBound: function () {
                            $('#DoctorWindow').data("kendoWindow").center().open();
                            $('.DoctorWindowListItem').off("click").on("click", function () {
                                $("#dscrDoctor").val($(this).text());
                                doctor_id = $(this).data('id');
                                $('#DoctorWindow').data("kendoWindow").close();
                            });
                        }
                    });

                });
            }

        },
        SearchPAsst: function () {
            var name = $("#dscrPAsst").val();
            if (name.length > 0) {
                $.get(API + "nurse/" + name, function (data, status) {
                    var ds = [];
                    var template = kendo.template($("#NoRecordTemplate").html());
                    if ($(data.NurseListResult).length > 0) {
                        $(data.NurseListResult).each(function (k, v) {
                            var item = { id: v.id, name: v.name };
                            ds.push(item);
                        });
                        template = kendo.template($("#NurseWindowListTemplate").html());
                    }
                    else {
                        var item = { id: 0, name: "No Record" };
                        ds.push(item);
                    }

                    var dataSource = new kendo.data.DataSource({
                        data: ds
                    });

                    $("#NurseWindowList").kendoListView({
                        dataSource: ds,
                        template: template,
                        dataBound: function () {
                            $('#NurseWindow').data("kendoWindow").center().open();
                            $('.NurseWindowListItem').off("click").on("click", function () {
                                $("#dscrPAsst").val($(this).text());
                                nurse_id = $(this).data('id');
                                $('#NurseWindow').data("kendoWindow").close();
                            });
                        }
                    });

                });
            }
        },
        SearchPatient: function () {
            var name = $("#dscrPatient").val();
            if (name.length > 0) {
                $.get(API + "patient/" + name, function (data, status) {
                    var ds = [];
                    var template = kendo.template($("#NoRecordTemplate").html());
                    if ($(data.PatientListResult).length > 0) {
                        $(data.PatientListResult).each(function (k, v) {
                            var item = { id: v.id, name: v.name };
                            ds.push(item);
                        });

                        template = kendo.template($("#PatientWindowListTemplate").html());
                    }
                    else {
                        var item = { id: 0, name: "No Record" };
                        ds.push(item);
                    }

                    var dataSource = new kendo.data.DataSource({
                        data: ds
                    });
                    $("#PatientWindowList").kendoListView({
                        dataSource: ds,
                        template: template,
                        dataBound: function () {
                            $('#PatientWindow').data("kendoWindow").center().open();
                            $('.PatientWindowListItem').off("click").on("click", function () {
                                $("#dscrPatient").val($(this).text());
                                patient_id = $(this).data('id');
                                $('#PatientWindow').data("kendoWindow").close();
                            });
                        }
                    });

                });
            }
        },
        SearchDSCR: function () {
            var fullRR = $("#dscrRR").val() + "-" + $("#dscrPrefix").val();
            DSCR = new Object();
            DSCR = {
                prefix: $('#dscrPrefix').val(),
                rr_number: $('#dscrRR').val(),
                branch_id: branch_id,
                current_user_id: current_user_id,
            }
            url = API + "DSCR/";
            service.post(url, JSON.stringify(DSCR), callback.SearchDSCR);

        },
        AddCheck: function () {
            $('#CheckWindow').data("kendoWindow").center().open();
        },
        AddCreditCard: function () {
            $('#CreditCardWindow').data("kendoWindow").center().open();
        },
        SearchCashier: function () {
            var name = $("#dscrCashier").val();
            if (name.length > 0) {
                $.get(API + "Cashier/" + name, function (data, status) {
                    var ds = [];
                    var template = kendo.template($("#NoRecordTemplate").html());
                    if ($(data.CashierListResult).length > 0) {
                        $(data.CashierListResult).each(function (k, v) {
                            var item = { id: v.id, name: v.name };
                            ds.push(item);
                        });
                    }
                    else {
                        var item = { id: 0, name: "No Record" };
                        ds.push(item);
                    }

                    var dataSource = new kendo.data.DataSource({
                        data: ds
                    });
                    $("#CashierWindowList").kendoListView({
                        dataSource: ds,
                        template: kendo.template($("#CashierWindowListTemplate").html()),
                        dataBound: function () {
                            $('#CashierWindow').data("kendoWindow").center().open();
                            $('.CashierWindowListItem').off("click").on("click", function () {
                                $("#dscrCashier").val($(this).text());
                                cashier_id = $(this).data('id');
                                $('#CashierWindow').data("kendoWindow").close();
                            });
                        }
                    });

                });
            }
        },
        BindGrid: function () {

            $('#dscrGrid').kendoGrid({
                toolbar: kendo.template($("#dscrGridToolbar").html()),
                columns: [

                    {
                        field: "item_name", title: "Particulars", width: 300, footerTemplate: '<span class="myFooter">Total:</span>'
                    },
                    { field: "quantity", title: "Qty", width: 100, footerTemplate: '<span class="myFooter">0</span>' },
                    { field: "session", title: "Sess", width: 100, footerTemplate: '<span class="myFooter">0</span>' },
                    {
                        field: "gross", title: "Gross", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "discount", title: "Discount", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "deduction", title: "Deduction", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "advance_payment", title: "Adv.Payment", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "program_availed", title: "Prog.Availed", footerTemplate: '<span class="myFooter">0.00</span>', format: "({0:n})",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "add_ons", title: "AddOns", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "net_sales", title: "NetSales", footerTemplate: '<span class="myFooter">0.00</span>', format: "{0:n}",
                        attributes: {
                            style: "text-align: right;"
                        }
                    },
                    {
                        field: "item_id", hidden: true
                    },
                    {
                        field: "explanation", title: "Explanation"
                    },
                    { command: [{ name: "destroy", text: "", width: 30 }], title: " " },
                ],
                editable: {
                    mode: "incell", // mode can be incell/inline/popup with Q1 '12 Beta Release of Kendo UI
                    confirmation: false // the confirmation message for destroy command
                },
                scrollable: true,
                sortable: false,
                pageable: false,
                filterable: false,
                height: 290,
                dataBound: function () {
                    var displayedData = $("#dscrGrid").data().kendoGrid.dataSource.view();
                    var json = JSON.stringify(displayedData);
                    var obj = JSON.parse(json);
                    MyDetails = [];
                    $(obj).each(function (k, v) {
                        MyDetails.push({
                            item_id: v.item_id,
                            item_name: v.item_name,
                            quantity: v.quantity,
                            session: v.session,
                            discount: v.discount,
                            deduction: v.deduction,
                            advance_payment: v.advance_payment,
                            net_sales: v.net_sales,
                            program_availed: v.program_availed,
                            add_ons: v.add_ons,
                            gross: v.gross,
                            explanation: v.explanation
                        });
                    });

                    $('#TotalGrid').text(kendo.toString("0.00", "n"));
                    if (MyDetails.length > 0) {
                        var dataSource = new kendo.data.DataSource({
                            data: MyDetails,
                            aggregate: [
                              { field: "gross", aggregate: "sum" },
                              { field: "quantity", aggregate: "sum" },
                              { field: "session", aggregate: "sum" },
                              { field: "discount", aggregate: "sum" },
                              { field: "deduction", aggregate: "sum" },
                              { field: "advance_payment", aggregate: "sum" },
                              { field: "net_sales", aggregate: "sum" },
                              { field: "program_availed", aggregate: "sum" },
                              { field: "add_ons", aggregate: "sum" }
                            ]
                        });

                        dataSource.fetch(function () {
                            var Gross = dataSource.aggregates().gross;
                            var Qty = dataSource.aggregates().quantity;
                            var Sess = dataSource.aggregates().session;
                            var Discount = dataSource.aggregates().discount;
                            var Deduction = dataSource.aggregates().deduction;
                            var AdvPayment = dataSource.aggregates().advance_payment;
                            var NetSales = dataSource.aggregates().net_sales;
                            var ProgramAvailed = dataSource.aggregates().program_availed;
                            var AddOns = dataSource.aggregates().add_ons;

                            var dom = $('.k-footer-template');
                            dom.find('td:eq(0)').text("Total:").css({ "color": "blue" });;
                            dom.find('td:eq(1)').text(Qty.sum).css({ "color": "blue" });
                            dom.find('td:eq(2)').text(Sess.sum).css({ "color": "blue" });
                            dom.find('td:eq(3)').text(kendo.toString(Gross.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(4)').text(kendo.toString(Discount.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(5)').text(kendo.toString(Deduction.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(6)').text(kendo.toString(AdvPayment.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(7)').text("(" + kendo.toString(ProgramAvailed.sum, "n") + ")").css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(8)').text(kendo.toString(AddOns.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            dom.find('td:eq(9)').text(kendo.toString(NetSales.sum, "n")).css({ "text-align": "right", "color": "blue" });
                            var Total = (NetSales.sum + AdvPayment.sum) - ProgramAvailed.sum;
                            $('#TotalGrid').text(kendo.toString(Total, "n"));
                            $('#dscrAR').data('kendoNumericTextBox').value(Total);
                            methods.ComputeAR();
                        });
                    }
                    else {
                        $('#dscrAR').data('kendoNumericTextBox').value(0);
                    }
                    $("#dscrGridToolbarButton").kendoButton({
                        icon: "plus",
                        click: function () {
                            methods.AddDetails();
                        }
                    });
                },
                aggregate: [
                    { field: "gross", aggregate: "sum" }
                ],
                dataSource: { data: MyDetails }
            });


        },
        BindCheckGrid: function () {
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (e) {
                        e.success(CheckWindowGridData);
                    },
                    update: function (e) {
                        e.success();
                    },
                    create: function (e) {
                        var item = e.data;
                        item.Id = CheckWindowGridData.length + 1;
                        e.success(item);
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Name: { type: "string", validation: { required: true } },
                            CheckNumber: { type: "string", validation: { required: true } },
                            CheckDate: { type: "date" },
                            Amount: { type: "number", validation: { min: 1, required: true } },

                        }
                    }
                }
            });

            var grid = $("#CheckWindowGrid").kendoGrid({
                dataSource: dataSource,
                scrollable: true,
                navigatable: true,
                height: 215,
                editable: {
                    createAt: "bottom"
                },
                navigatable: true,
                toolbar: ["create"],
                columns: [
                            { field: "Bank", title: "Name" },
                            { field: "CheckNumber", title: "Check #" },
                            {
                                field: "CheckDate",
                                title: "Date",
                                format: "{0:d}"
                            },
                            { field: "Amount", format: "{0:n2}" },
                            {
                                command: "destroy", title: "&nbsp;", width: 120
                            }
                ]
            }).data("kendoGrid");

            grid.tbody.on('keydown', function (e) {
                if ($(e.target).closest('td').is(':last-child') && $(e.target).closest('tr').is(':last-child')) {
                    grid.addRow();
                }
            })
        },
        BindCreditCardGrid: function () {
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (e) {
                        e.success(CreditCardWindowGridData);
                    },
                    update: function (e) {
                        e.success();
                    },
                    create: function (e) {
                        var item = e.data;
                        item.Id = CreditCardWindowGridData.length + 1;
                        e.success(item);
                    }
                },
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Bank: {
                                defaultValue: {
                                    Value: 1,
                                    Text: "BPI"
                                }
                            },
                            Terms: {
                                defaultValue: {
                                    Value: 1,
                                    Text: "Straight"
                                }
                            },
                            Amount: {
                                type: "number",
                                validation: {
                                    min: 1,
                                    required: true
                                }
                            },
                            Note: { type: "string" },
                        }
                    }
                }
            });

            var grid = $("#CreditCardWindowGrid").kendoGrid({
                dataSource: dataSource,
                scrollable: true,
                navigatable: true,
                height: 215,
                editable: {
                    createAt: "bottom"
                },
                navigatable: true,
                toolbar: ["create"],
                columns: [
                            { field: "Bank", title: "Bank", editor: methods.BankDropDownEditor, template: "#=Bank.Text#" },
                            { field: "Terms", title: "Terms", editor: methods.TermDropDownEditor, template: "#=Terms.Text#" },
                            { field: "Amount", format: "{0:n2}" },
                            { field: "Note" },
                            {
                                command: "destroy", title: "&nbsp;", width: 120
                            }
                ]
            }).data("kendoGrid");

            grid.tbody.on('keydown', function (e) {
                if ($(e.target).closest('td').is(':last-child') && $(e.target).closest('tr').is(':last-child')) {
                    grid.addRow();
                }
            })
        },
        BankDropDownEditor: function (container, options) {
            $('<input required name="' + options.field + '" class="BankDropDownEditor"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            autoBind: true,
                            dataTextField: "Text",
                            dataValueField: "Value",
                            dataSource: [
                                { Text: "BPI", Value: 1 },
                                { Text: "BDO", Value: 2 },
                                { Text: "RCBC", Value: 3 },
                                { Text: "METROBANK", Value: 4 },
                                { Text: "OTHER", Value: 5 },
                            ]
                        });
        },
        TermDropDownEditor: function (container, options) {
            $('<input required name="' + options.field + '"/>')
                        .appendTo(container)
                        .kendoDropDownList({
                            autoBind: false,
                            dataTextField: "Text",
                            dataValueField: "Value",
                            dataSource: [
                                { Text: "Straight", Value: 1 },
                                { Text: "EPS", Value: 2 },
                                { Text: "DEBIT", Value: 3 },
                                { Text: "SIP-3mos", Value: 4 },
                                { Text: "SIP-6mos", Value: 5 },
                                { Text: "SIP-9mos", Value: 6 },
                                { Text: "SIP-12mos", Value: 7 },
                            ]
                        });
        },
        AddDetails: function () {
            $('#DSCRWindow').data("kendoWindow").center().open();
        },
        PostDSCR: function () {
            if ($('#particularName').text().length > 0) {
                var displayedData = $("#dscrGrid").data().kendoGrid.dataSource.view();
                var json = JSON.stringify(displayedData);
                var obj = JSON.parse(json);
                MyDetails = [];
                $(obj).each(function (k, v) {
                    MyDetails.push({
                        item_id: v.item_id,
                        item_name: v.item_name,
                        quantity: v.quantity,
                        session: v.session,
                        discount: v.discount,
                        deduction: v.deduction,
                        advance_payment: v.advance_payment,
                        net_sales: v.net_sales,
                        program_availed: v.program_availed,
                        add_ons: v.add_ons,
                        gross: v.gross,
                        explanation: v.explanation
                    });
                });
                MyDetails.push({
                    item_id: $('#particularName').data('id'),
                    item_name: $('#particularName').text(),
                    quantity: $('#DSCRWindowQty').data('kendoNumericTextBox').value(),
                    session: $('#DSCRWindowSession').data('kendoNumericTextBox').value(),
                    discount: $('#DSCRWindowDiscount').data('kendoNumericTextBox').value(),
                    deduction: $('#DSCRWindowDeduction').data('kendoNumericTextBox').value(),
                    advance_payment: $('#DSCRWindowAdvancePayment').data('kendoNumericTextBox').value(),
                    net_sales: $('#DSCRWindowNetSale').data('kendoNumericTextBox').value(),
                    program_availed: $('#DSCRWindowProgramAvail').data('kendoNumericTextBox').value(),
                    add_ons: $('#DSCRWindowAddOns').data('kendoNumericTextBox').value(),
                    gross: $('#DSCRWindowGross').data('kendoNumericTextBox').value(),
                    explanation: $('#DSCRWindowExplanation').val()
                });
                methods.BindGrid();
                methods.ResetDSCR();
            }

        },
        ResetDSCR: function () {
            $('#DSCRWindowGross').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowQty').data('kendoNumericTextBox').value(1);
            $('#DSCRWindowSession').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowDiscount').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowDeduction').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowAdvancePayment').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowProgramAvail').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowAddOns').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowNetSale').data('kendoNumericTextBox').value(0);
            $('#DSCRWindowExplanation').val('');
            $('#particularName').attr('data-gross', 0);
            $('#particularName').attr('data-gross', 0);
            $('#particularName').text('');
            $('#Barcode').val('');
        },
        Barcode: function () {
            var barcode = $("#Barcode").val();
            $.get(API + "Particulars/" + barcode, function (data, status) {
                if (data.SearchParticularsResult.length == 0) {
                    $('#particularName').text('');
                    $('#particularName').attr('data-id', 0);
                    $('#particularName').attr('data-gross', 0);
                    $("#DSCRWindowGross").data("kendoNumericTextBox").value(0);
                }
                else {
                    $(data.SearchParticularsResult).each(function (k, v) {
                        $('#particularName').text(v.Name);
                        $('#particularName').attr('data-id', v.Id);
                        $('#particularName').attr('data-gross', v.Gross);
                        $("#DSCRWindowGross").data("kendoNumericTextBox").value(v.Gross);
                    });
                }
                methods.ComputeNetSale();

            });
        },
        ComputeNetSale: function () {
            var advPayment = $('#DSCRWindowAdvancePayment').data('kendoNumericTextBox').value();
            var qty = 0;
            var price = 0;
            var gross = 0;
            var netSale = $('#DSCRWindowNetSale').data('kendoNumericTextBox').value();
            var discount = $('#DSCRWindowDiscount').data('kendoNumericTextBox').value();
            var otherdeduction = $('#DSCRWindowDeduction').data('kendoNumericTextBox').value();
            var advpayment = $('#DSCRWindowAdvancePayment').data('kendoNumericTextBox').value();
            var addOns = $('#DSCRWindowAddOns').data('kendoNumericTextBox').value();
            if (advPayment > 0) {

                if ($('#DSCRWindowQty').data('kendoNumericTextBox').value() > 0) {
                    qty = $('#DSCRWindowQty').data('kendoNumericTextBox').value();
                }
                else if ($('#DSCRWindowSession').data('kendoNumericTextBox').value() > 0) {
                    qty = $('#DSCRWindowSession').data('kendoNumericTextBox').value();
                }

                price = $('#DSCRWindowGross').data('kendoNumericTextBox').value();

                gross = price - (discount + otherdeduction + advpayment);
                gross = gross + addOns;
                netSale = netSale - advpayment;
                $('#DSCRWindowGross').data('kendoNumericTextBox').value(gross);
                $('#DSCRWindowNetSale').data('kendoNumericTextBox').value(netSale);
            }
            else {
                price = parseFloat($('#DSCRWindowGross').data('kendoNumericTextBox').value()) + parseFloat(addOns);
                netSale = price - (otherdeduction + discount);
                $('#DSCRWindowGross').data('kendoNumericTextBox').value(price);
                $('#DSCRWindowNetSale').data('kendoNumericTextBox').value(netSale);
            }
        },
        Save: function () {
            var validator = $("#dscrContent").kendoValidator().data('kendoValidator');
            if (validator.validate() == false) {
                methods.Validate();
            }
            else {
                DSCR = new Object();
                DSCR = {
                    id: 0,
                    prefix: $('#dscrPrefix').val(),
                    rr_number: $('#dscrRR').val(),
                    si_number: $('#dscrSI').val(),
                    or_number: $('#dscrOR').val(),
                    branch_id: branch_id,
                    doctor_id: doctor_id,
                    nurse_id: nurse_id,
                    patient_id: patient_id,
                    trx_date: kendo.toString($("#dscrDate").data('kendoDatePicker').value(), "u"),
                    trx_time_from: kendo.toString($("#dscrTimeFrom").data('kendoTimePicker').value(), "t"),
                    trx_time_to: kendo.toString($("#dscrTimeTo").data('kendoTimePicker').value(), "t"),
                    cash: $('#dscrCash').data('kendoNumericTextBox').value(),
                    ar_amount: $('#dscrAR').data('kendoNumericTextBox').value(),
                    payment: $('#dscrTotalPayment').data('kendoNumericTextBox').value(),
                    cashier_id: cashier_id,
                    current_user_id: current_user_id,
                    details: MyDetails,
                    checks: checkdata,
                    creditcards: creditcarddata,
                }

                url = API + "DSCR/Insert";
                service.post(url, JSON.stringify(DSCR), callback.DSRCInsert);
            }

        },
        Cancel: function () {
            DSCR = new Object();
            $('#dscrRR').val('');
            $('#dscrSI').val('');
            $('#dscrOR').val('');
            doctor_id = 0;
            $('#dscrDoctor').val('');
            nurse_id = 0;
            $('#dscrPAsst').val('');
            patient_id = 0;
            $('#dscrPatient').val('');
            $("#dscrDate").data('kendoDatePicker').value(todayDate);
            $("#dscrTimeFrom").data('kendoTimePicker').value('');
            $("#dscrTimeTo").data('kendoTimePicker').value('');
            $('#dscrCash').data('kendoNumericTextBox').value(0);
            $('#dscrAR').data('kendoNumericTextBox').value(0);
            $('#dscrTotalPayment').data('kendoNumericTextBox').value(0);
            cashier_id = 0;
            $('#dscrCashier').val('');
            checkdata = [];
            $('#dscrCheck').val("0.00");
            methods.BindCheckGrid();
            creditcarddata = [];
            $('#dscrCreditCard').val("0.00");
            methods.BindCreditCardGrid();
            MyDetails = [];
            methods.BindGrid();
        },
        Print: function () {
            var win = window.open(PrintURL + "TRNMA-" + $('#dscrRR').val() + "&pref=" + $('#dscrPrefix').val(), '_blank');
            if (win) {
                //Browser has allowed it to be opened
                win.focus();
            } else {
                //Browser has blocked it
                alert('Please allow popups for this website');
            }
        },
        RRSearchSummary: function () {
            var fullRR = $("#dscrRR").val() + "-" + $("#dscrPrefix").val();
            DSCR = new Object();
            DSCR = {
                prefix: $('#dscrPrefixSearch').val(),
                rr_number: $('#dscrRRSearch').val(),
                branch_id: branch_id,
                current_user_id: current_user_id,
            }
            url = API + "DSCR/";
            service.post(url, JSON.stringify(DSCR), callback.RRSearchSummary);

        },
        DateRangeSearchSummary: function () {
            var from = kendo.toString($("#dscrFromSearch").data('kendoDatePicker').value(), "u");
            var to = kendo.toString($("#dscrToSearch").data('kendoDatePicker').value(), "u");
            from = from.substring(0, 10) + " 00:00:00";
            to = to.substring(0, 10) + " 23:59:59";
            DSCR = new Object();
            DSCR = {
                trx_date_to: to,
                trx_date_from: from,
                branch_id: branch_id,
                current_user_id: current_user_id,
            }
            url = API + "DSCR/ByDateRange";
            service.post(url, JSON.stringify(DSCR), callback.RRSearchSummary);
        },
        FindBank: function (name) {
            var result = new Object();
            switch (name) {
                case "BPI":
                    result = { Text: "BPI", Value: 1 };
                    break;
                case "BDO":
                    result = { Text: "BDO", Value: 2 };
                    break;
                case "RCBC":
                    result = { Text: "RCBC", Value: 3 };
                    break;
                case "METROBANK":
                    result = { Text: "METROBANK", Value: 4 };
                    break;
                default:
                    result = { Text: "OTHER", Value: 5 };
            }
            return result;
        },
        FindTerm: function (name) {
            var result = new Object();
            switch (name) {
                case "Straight":
                    result = { Text: "Straight", Value: 1 };
                    break;
                case "DEBIT":
                    result = { Text: "DEBIT", Value: 3 };
                    break;
                case "SIP-3mos":
                    result = { Text: "SIP-3mos", Value: 4 };
                    break;
                case "SIP-6mos":
                    result = { Text: "SIP-6mos", Value: 5 };
                    break;
                case "SIP-9mos":
                    result = { Text: "SIP-9mos", Value: 6 };
                    break;
                case "SIP-12mos":
                    result = { Text: "SIP-12mos", Value: 7 };
                    break;
                default:
                    result = { Text: "EPS", Value: 2 };
            }
            return result;
        },
        Validate: function () {
            if ($('#dscrRR').is(":empty")) {
                alert('aa');
            }
        },
        ComputeAR: function () {
            var Total = $('#TotalGrid').text().replace(',', '');

            var cash = $('#dscrCash').data('kendoNumericTextBox').value();
            if (cash == null) {
                cash = 0;
            }

            var check = $('#dscrCheck').val();
            if (check == "") {
                check = 0;
            }
            else {
                check = check.replace(',', '');
            }
            var creditcard = $('#dscrCreditCard').val();
            if (creditcard == "") {
                creditcard = 0;
            }
            else {
                creditcard = creditcard.replace(',', '');
            }
            var minus = parseFloat(cash) + parseFloat(check) + parseFloat(creditcard);
            var result = parseFloat(Total) - minus;
            $('#dscrTotalPayment').data('kendoNumericTextBox').value(minus);
            $('#dscrAR').data('kendoNumericTextBox').value(result);
        },
        adminGrid: function () {
            var dataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: API + "admin/",
                        type: "get",
                        dataType: "json"
                    },
                    update: {
                        url: API + "admin/update",
                        type: "post",
                        contentType: "application/json"
                    },
                    parameterMap: function (options, operation) {
                        var x = kendo.stringify(options);
                        if (operation !== "read" && options) {
                            return kendo.stringify(options);
                        }
                        return options;

                    }
                },
                schema: {
                    data: "DSCRUsersResult",
                    total: "itemCount",
                    model: {
                        id: "id",
                        fields: {
                            id: { editable: false },
                            name: { editable: false },
                            username: { editable: false },
                            password: {
                                validation: { //set validation rules
                                    required: true
                                }
                            },
                        }
                    }
                },
                serverPaging: true
            });

            var grid = $("#adminGrid").kendoGrid({
                dataSource: dataSource,
                height: 215,
                toolbar: ["save", "cancel"],
                editable: true,
                columns: [
                          { field: "id", title: "User ID", hidden: true },
                          { field: "name", title: "Full Name" },
                          { field: "username", title: "User Name" },
                          { field: "password", title: "Password" },
                ]
            }).data("kendoGrid");

            //$.get(API + "admin/", function (data) {
            //    callback.adminGrid(data);
            //});
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

    var callback = {
        Login: function (data) {
            if (data.username != null) {
                var code = data.branch.store_code_fld;
                branch_id = data.branch.id;
                current_user_id = data.id
                $('#branchddl').data("kendoDropDownList").value(branch_id);
                $('#dscrPrefix').val(code);
                $('#dscrPrefixSearch').val(code);

                $('#currentUserBranch').html(data.username + "/" + data.branch.store_code_fld);
                $('#LoginWindow').data('kendoWindow').close();
                var d = new Date();

                if (data.role != 3 && data.role != 9) {
                    $('#UserMaintenanceList').hide();
                }
                else {
                    methods.adminGrid();
                }



            }
        },
        DSRCInsert: function (data) {
            methods.Cancel();
            $("#notification").data("kendoNotification").show('DSCR is saved successfully.', "success");
        },
        SearchDSCR: function (data) {
            if (data.resultMessage == true) {
                var trx_date = kendo.toString(kendo.parseDate(data.trx_date), 'MM/dd/yyyy');
                $('#dscrDate').data('kendoDatePicker').value(trx_date);
                $('#dscrTimeFrom').data('kendoTimePicker').value(data.trx_time_from);
                $('#dscrTimeTo').data('kendoTimePicker').value(data.trx_time_to);
                $('#dscrSI').val(data.si_number);
                $('#dscrOR').val(data.or_number);
                doctor_id = data.doctor_id;
                $('#dscrDoctor').val(data.doctor_name);
                nurse_id = data.nurse_id;
                $('#dscrPAsst').val(data.nurse_name);
                patient_id = data.patient_id;
                $('#dscrPatient').val(data.patient_name);
                $('#dscrCash').data('kendoNumericTextBox').value(data.cash);
                $('#dscrAR').data('kendoNumericTextBox').value(data.ar_amount);
                $('#dscrTotalPayment').data('kendoNumericTextBox').value(data.payment);
                cashier_id = data.cashier_id;
                $('#dscrCashier').val(data.cashier_name);
                $('#dscrCreditCard').val(kendo.toString(data.totalcreditcards, "n2"));
                $('#dscrCheck').val(kendo.toString(data.totalchecks, "n2"));
                MyDetails = [];
                $(data.details).each(function (k, v) {
                    MyDetails.push({
                        item_id: v.item_id,
                        item_name: v.item_name,
                        quantity: v.quantity,
                        session: v.session,
                        discount: v.discount,
                        deduction: v.deduction,
                        advance_payment: v.advance_payment,
                        net_sales: v.net_sales,
                        program_availed: v.program_availed,
                        add_ons: v.add_ons,
                        gross: v.gross,
                        explanation: v.explanation
                    });
                });
                methods.BindGrid();

                CheckWindowGridData = [];

                $(data.checks).each(function (k, v) {
                    var o = new Object();
                    o.Bank = v.bank_name;
                    o.CheckDate = v.date_of_check.substring(0, 10) + " 00:00:00",
                    o.CheckNumber = v.check_number;
                    o.Amount = v.amount;
                    CheckWindowGridData.push(o);
                });
                methods.BindCheckGrid();
                CreditCardWindowGridData = [];

                $(data.creditcards).each(function (k, v) {
                    var o = new Object();
                    o.Bank = methods.FindBank(v.name);
                    o.Note = v.note,
                    o.Terms = methods.FindTerm(v.term);
                    o.Amount = v.amount;
                    CreditCardWindowGridData.push(o);
                });
                methods.BindCreditCardGrid();
            }
            else {
                methods.Cancel();
                $("#notification").data("kendoNotification").show('No DSCR found. Search again', "error");
            }
        },
        RRSearchSummary: function (data) {
            var ds = [];
            debugger;
            $(data.details).each(function (k, v) {
                var item = {
                    Prefix: v.prefix,
                    RR: v.rr_number,
                    Particular: v.item_name,
                    Quantity: v.quantity,
                    Session: v.session,
                    Gross: v.gross,
                    Discount: v.discount,
                    Deduction: v.deduction,
                    AdvPayment: v.advance_payment,
                    ProgAvailed: v.program_availed,
                    AddOns: v.add_ons,
                    NetSales: v.net_sales,
                    Explanation: v.explanation
                }
                ds.push(item);
            });


            var grid = $("#SummaryGrid").kendoGrid({
                dataSource: ds,
                scrollable: true,
                navigatable: true,
                height: 215,
                navigatable: true,
                columns: [
                            { field: "Prefix" },
                            { field: "RR", title: "RR #" },
                            { field: "Particular", width: 200 },
                            { field: "Quantity" },
                            { field: "Session" },
                            {
                                field: "Gross", title: "Gross", format: "{0:n}",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "Discount", format: "{0:n}",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "Deduction", format: "{0:n}",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "AdvPayment", format: "{0:n}", title: "Adv.Payment",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "ProgAvailed", format: "{0:n}", title: "Prog.Availed",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "AddOns", format: "{0:n}", title: "Add Ons",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            {
                                field: "NetSales", format: "{0:n}", title: "Net Sales ",
                                attributes: {
                                    style: "text-align: right;"
                                }
                            },
                            { field: "Explanation" },

                ]
            }).data("kendoGrid");
        },
        adminGrid: function (data) {
            debugger;


        }
    }


    methods.init();
});