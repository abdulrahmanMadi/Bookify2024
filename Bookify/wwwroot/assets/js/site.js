var updatedRow;
var table;
var datatable;
var exportedCols = [];

function showSuccessMessage(message = 'Done successfully!') {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: message,
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });
}

function showErrorMessage(message = 'Something went wrong!') {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message,
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });
}
function DisableSuccessMessage()
{
    $('body :submit').attr('disabled', 'disabled').attr('data-kt-indicator', 'on');
}

function onModalSuccess() {
    showSuccessMessage();
    $('#Modal').modal('hide');
    if (updatedRow === undefined) {
        $('tbody').append(item);
    } else {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }
    KTMenu.init();
    KTMenu.initHandlers();
}
var headers = $('th');
$.each(headers, function (i) {
    if (!$(this).hasClass("print-no-columns")) {
        exportedCols.push(i);
    }
});
var KTDatatables = function () {
    var initDatatable = function () {
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
        });
    }
    // Hook export buttons


    var exportButtons = () => {
        const documentTitle = $(".js-DataTables").data("document-title");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols

                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-DataTables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();


$(document).ready(function () {
    var message = $('#Message').text();
    //Disable Submit Button
    //$("form").on('submit', function () {
    //    var isValid = $(this).isValid();
    //    if (isValid) {
    //        DisableSuccessMessage();
    //    }
    //});
    //Show SuccessMessage
    if (message !== '') {
        showSuccessMessage(message);
    }
    //TinyMCE
    if ($(".js-TinyMce").length > 0)
    {
        tinymce.init({
            selector: ".js-TinyMce", height: "466"
        });
        $.validator.setDefaults({
            ignore: []
        });
        if (KTThemeMode.getMode() === "dark") {
            Option["skin"] = "oxide-dark";
            Option["content_css"] = "dark";

        }
    }
   
    //Datepicker
    $(".js-Datepicker").daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops:"up"
    });

    //$(".js-Datepicker").flatpickr({
    //    enableTime: true,
    //});

   
    //Select2
    $(".js-select2").select2({
        //   placeholder:"Select Category",
    });
    $(".js-select2").on('select2:select', function (e)
    {
        var select = $(this);
        var selsect = $(form).validate().element('#' + select.attr('Id'));
        console.log(selsect);
    }
    );
    // DataTables
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });


    // Handle bootstrap modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');

        modal.find('#ModalTitle').text(btn.data('title'));

        if (btn.data('update') !== undefined) {
            updatedRow = btn.parents('tr');
        }

        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            }
        });

        modal.modal('show');
    });
    //Toggle Status
    $('.js-toggle-status').on('click', function () {
        var btn = $(this);
        bootbox.confirm({
            title: 'Delete item',
            message: 'Are you sure to delete this item?',
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel',
                    className: 'btn-success'

                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm',
                    className: 'btn-danger'

                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (lastUpdatedOn) {
                            var row = btn.parents('tr');
                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Deleted' ? 'Available' : 'Deleted';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');
                            row.find('.js-updated-on').html(lastUpdatedOn);
                            row.addClass('animated flash');

                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });

    });
});