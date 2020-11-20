$(document).ready(function () {

    // Test table
    $('#test_table tfoot th').each(function (i) {

        if (i < ($('#test_table tfoot th').length - 1)) {
            var title = $(this).text();
            $(this).html('<input class="col-sm-12" type="text" placeholder="' + title + '" />');
        }
    });

    // Test DataTable
    var table = $('#test_table').DataTable({
        dom: 'lBfrtip',
        buttons: [
            'copyHtml5',
            'print',
            'csvHtml5',
            'pdfHtml5'
        ],
        "oLanguage": {
            "sZeroRecords": "No records to display",
            "sSearchPlaceholder": "Search in all columns",
            "sSearch": "Search "
        },
        "aLengthMenu": [[10, 20, 50, 100, 150, 250], [10, 20, 50, 100, 150, 250]],
        "iDisplayLength": 10,
        "bSortClasses": false,
        "bStateSave": false,
        "bPaginate": true,
        "bAutoWidth": false,
        "bProcessing": true,
        "bDestroy": true,
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bDeferRender": true,
        "columnDefs": [{ "type": 'date-euro', "targets": -1 }]
    });

    // Apply columns search for test table
    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });

    // URL table
    $('#url_table tfoot th').each(function (i) {

        if (i < ($('#url_table tfoot th').length - 1)) {
            var title = $(this).text();
            $(this).html('<input class="col-sm-12" type="text" placeholder="' + title + '" />');
        }
    });

    // URL DataTable
    var table2 = $('#url_table').DataTable({
        dom: 'lBfrtip',
        buttons: [
            'copyHtml5',
            'print',
            'csvHtml5',
            'pdfHtml5'
        ],
        "oLanguage": {
            "sZeroRecords": "No records to display",
            "sSearchPlaceholder": "Search in all columns",
            "sSearch": "Search "
        },
        "aLengthMenu": [[10, 20, 50, 100, 150, 250], [10, 20, 50, 100, 150, 250]],
        "iDisplayLength": 10,
        "bSortClasses": false,
        "bStateSave": false,
        "bPaginate": true,
        "bAutoWidth": false,
        "bProcessing": true,
        "bDestroy": true,
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bDeferRender": true,
        "columnDefs": [{ "type": 'date-euro', "targets": -1 }]
    });

    // Apply columns search for URL table
    table2.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });


    // Input table
    $('#input_table tfoot th').each(function (i) {

        if (i < ($('#input_table tfoot th').length - 1)) {
            var title = $(this).text();
            $(this).html('<input class="col-sm-12" type="text" placeholder="' + title + '" />');
        }
    });

    // Input DataTable
    var table3 = $('#input_table').DataTable({
        dom: 'lBfrtip',
        buttons: [
            'copyHtml5',
            'print',
            'csvHtml5',
            'pdfHtml5'
        ],
        "oLanguage": {
            "sZeroRecords": "No records to display",
            "sSearchPlaceholder": "Search in all columns",
            "sSearch": "Search "
        },
        "aLengthMenu": [[10, 20, 50, 100, 150, 250], [10, 20, 50, 100, 150, 250]],
        "iDisplayLength": 10,
        "bSortClasses": false,
        "bStateSave": false,
        "bPaginate": true,
        "bAutoWidth": false,
        "bProcessing": true,
        "bDestroy": true,
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bDeferRender": true,
        "columnDefs": [{ "type": 'date-euro', "targets": -1 }]
    });

    // Apply columns search for input table
    table2.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
});