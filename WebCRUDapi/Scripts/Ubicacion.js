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
        IdRecord = data.IdUbicacion;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.IdUbicacion;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Ubicacion/Lista",
        order: [],
        columns: [
            { "data": "IdUbicacion" },
            { "data": "IdMaestro" },
            { "data": "Nombre" },
            { "data": "Tipo" }
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
                width: "24%",
                targets: 0,
                data: "IdUbicacion"
            },
            {
                width: "24%",
                targets: 1,
                data: "IdMaestro"
            },
            {
                width: "25%",
                targets: 2,
                data: "Nombre"
            },
            {
                width: "25",
                targets: 3,
                data: "Tipo"
            },
            {
                width: "1%",
                targets: 4,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 5,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'
            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Registrar Ubicacion");

    $('#txtIdMaestro').val('');
    $('#txtNombre').val('');
    $('#txtTipo').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Ubicacion");

    $('#txtIdMaestro').val(data.IdMaestro);
    $('#txtNombre').val(data.Nombre);
    $('#txtTipo').val(data.Tipo);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'IdUbicacion':" + IdRecord;
    record += ",'IdMaestro':'" + $.trim($('#txtIdMaestro').val()) + "'";
    record += ",'Nombre':'" + $.trim($('#txtNombre').val()) + "'";
    record += ",'Tipo':'" + $.trim($('#txtTipo').val()) + "'";

    $.ajax({
        type: 'POST',
        url: '/Ubicacion/Guardar',
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
        url: '/Ubicacion/Eliminar/?IdUbicacion=' + IdRecord,
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