var tableHdr = null;
var IdRecord = 0;

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        IdRecord = 0;
        NewRecord();
    });

    $('#btnguardar').on('click', function (e) {
        e.preventDefault();
        Guardar();
    });

    $('#dt-records').on('click', 'button.btn-edit', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        loadDtl(data);
        IdRecord = data.IdFactura;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.IdFactura;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Factura/Lista",
        order: [],
        columns: [
            { "data": "IdFactura" },
            { "data": "Numero" },
            { "data": "IdCliente" },
            { "data": "Fecha" },
            { "data": "IdZonaCliente" },
            { "data": "Total" }
        ],
        processing: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                width: "15%",
                targets: 0,
                data: "IdFactura"
            },
            {
                width: "20%",
                targets: 1,
                data: "Numero"
            },
            {
                width: "20%",
                targets: 2,
                data: "IdCliente"
            },
            {
                width: "15%",
                targets: 3,
                data: "Fecha"
            },
            {
                width: "14%",
                targets: 4,
                data: "IdZonaCliente"
            },
            {
                width: "14%",
                targets: 5,
                data: "Total"
            },
            {
                width: "1%",
                targets: 6,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 7,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Registrar Factura");

    $('#txtNumero').val('');
    $('#txtIdCliente').val('');
    $('#txtFecha').val('');
    $('#txtIdZonaCliente').val('');
    $('#txtTotal').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Factura");

    $('#txtNumero').val(data.Numero);
    $('#txtIdCliente').val(data.IdCliente);
    $('#txtFecha').val(data.Fecha);
    $('#txtIdZonaCliente').val(data.IdZonaCliente);
    $('#txtTotal').val(data.Total);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'IdFactura':" + IdRecord;
    record += ",'Numero':'" + $.trim($('#txtNumero').val()) + "'";
    record += ",'IdCliente':'" + $.trim($('#txtIdCliente').val()) + "'";
    record += ",'Fecha':'" + $.trim($('#txtFecha').val()) + "'";
    record += ",'IdZonaCliente':'" + $.trim($('#txtIdZonaCliente').val()) + "'";
    record += ",'Total':'" + $.trim($('#txtTotal').val()) + "'";

    $.ajax({
        type: 'POST',
        url: '/Factura/Guardar',
        data: eval('({' + record + '})'),
        success: function (response) {
            if (response.success) {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    $.ajax({
        type: 'POST',
        url: '/Factura/Eliminar/?IdFactura=' + IdRecord,
        success: function (response) {
            if (response.success) {
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}