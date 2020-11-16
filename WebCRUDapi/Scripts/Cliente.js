﻿var tableHdr = null;
var IdCliente = 0;

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        IdCliente = 0;
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
        IdRecord = data.IdCliente;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.IdCliente;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Cliente/Lista",
        order: [],
        columns: [
            { "data": "IdCliente" },
            { "data": "Nombre" },
            { "data": "Apellido" },
            { "data": "Telefono" },
            { "data": "Tipo" },
            { "data": "Estado" }
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
                width: "10%",
                targets: 0,
                data: "IdCliente"
            },
            {
                width: "20%",
                targets: 1,
                data: "Nombre"
            },
            {
                width: "20%",
                targets: 2,
                data: "Apellido"
            },
            {
                width: "20%",
                targets: 3,
                data: "Telefono"
            },
            {
                width: "10%",
                targets: 4,
                data: "Tipo"
            },
            {
                width: "10%",
                targets: 5,
                data: "Estado"
            },
            {
                width: "5%",
                targets: 6,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "5%",
                targets: 7,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Registrar Cliente");

    $('#txtNombre').val('');
    $('#txtApellido').val('');
    $('#txtTelefono').val('');
    $('#txtTipo').val('');
    $('#txtEstado').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    $(".modal-header h3").text("Editar Cliente");

    $('#txtIdCliente').val(data.IdCliente);
    $('#txtNombre').val(data.Nombre);
    $('#txtApellido').val(data.Apellido);
    $('#txtTelefono').val(data.Telefono);
    $('#txtTipo').val(data.Tipo);
    $('#txtEstado').val(data.Estado);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'IdCliente
    ':" + IdCliente;
    record += ",'CardType':'" + $.trim($('#txtCardType').val()) + "'";
    record += ",'CardNumber':'" + $.trim($('#txtCardNumber').val()) + "'";
    record += ",'ExpMonth':'" + $.trim($('#txtExpMonth').val()) + "'";
    record += ",'ExpYear':" + $.trim($('#txtExpYear').val());

    $.ajax({
        type: 'POST',
        url: '/CreditCard/Guardar',
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
        url: '/CreditCard/Eliminar/?CreditCardID=' + IdRecord,
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