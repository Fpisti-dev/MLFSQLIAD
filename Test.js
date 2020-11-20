$(document).ready(function () {

    $('#test_table tfoot th').each(function (i) {

        if (i < ($('#test_table tfoot th').length - 1)) {
            var title = $(this).text();
            $(this).html('<input class="col-sm-12" type="text" placeholder="' + title + '" />');
        }
    });

    // DataTable
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
        "columnDefs": [
            { //this definition is set so the column with the action buttons is not sortable
                "targets": -1, //this references the last column of the tools
                "orderable": false,
                "searchable": false
            }
        ]
    });

    // Apply columns search
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
});

function OpenModal() {

    console.log('Open modal');
    // Open modal windows
    $('#mInsert').modal('toggle');
}

function btnEdit_Click(x) {

    var tRow = $(x).parent().parent();

    var rID = $(tRow).find("td:eq(0)").text();
    var rSQL = $(tRow).find("td:eq(1)").text();
    var rLabel = $(tRow).find("td:eq(2)").text();


    $("#MainContent_txtEditID").val(rID);
    $("#MainContent_hID").val(rID);

    $("#MainContent_txtEditSQL").val(rSQL);
    $("#MainContent_ddlEditLabel").val(rLabel).prop('selected', true);

    // Open modal windows
    $('#mEdit').modal('toggle');
}


function btnDelete_Click(x) {

    var tRow = $(x).parent().parent();

    var rID = $(tRow).find("td:eq(0)").text();
    var rSQL = $(tRow).find("td:eq(1)").text();
    var rLabel = $(tRow).find("td:eq(2)").text();

    $("#MainContent_txtDeleteID").val(rID);
    $("#MainContent_hID").val(rID);

    $("#MainContent_txtDeleteSQL").val(rSQL);
    $("#MainContent_txtDeleteLabel").val(rLabel);

    // Open modal windows
    $('#mDelete').modal('toggle');
}