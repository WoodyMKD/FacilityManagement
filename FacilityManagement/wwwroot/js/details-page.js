var compressorId = $("#compressor-id").val();
var modalID = "details-modal";
var confirmModalID = "details-confirm-modal";

function reloadGeneralInformationBox() {
    var urlStr = "/InventoryObjects/GetInventoryObjectGeneralInformationPartial/" + compressorId;

    $.ajax({
        type: "GET",
        url: urlStr,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        beforeSend: function () {
            $('#ajax-compressor-info-loading').show();
        },
        success: function (data) {
            $("#box-general-information").html(data);
            $('#ajax-compressor-info-loading').hide();
        },
        failure: function (data) {
            console.log(data.responseText);
            toastr["error"]("Please refresh the page!", "Error occured");
        },
        error: function (data) {
            console.log(data.responseText);
            toastr["error"]("Please refresh the page!", "Error occured");
        }
    });
}

function reloadTypes() {
    var urlStr = "/InventoryObjectTypes/GetInventoryObjectTypesListPartial/" + compressorId;

    $.ajax({
        type: "GET",
        url: urlStr,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        beforeSend: function () {
            $('#ajax-full-details-box-loading').show();
        },
        success: function (data) {
            var previouslySelectedType = $(".nav-pills li.active a").attr("subTypeId");

            $("#box-types-container").html(data);
            $('#ajax-full-details-box-loading').hide();

            var typeExist = ($('.nav-pills li a[subTypeId="' + previouslySelectedType + '"]').length > 0) ? true : false;
            if (typeExist) {
                $(".nav-pills li a[subTypeId=" + previouslySelectedType + "]").parent("li").addClass("active");
                $('#ajax-full-details-box-loading').hide();
            } else {
                firstTypeId = $(".nav-pills li a[subTypeId]:first").attr("subTypeId");
                if (firstTypeId == null) {
                    $(".custom-dropdown-button-group li:not(:first-child)").hide();
                    $('#add-system-btn').remove();
                    $('#ajax-full-details-box-loading').hide();
                } else {
                    $(".custom-dropdown-button-group li:not(:first-child)").show();
                    reloadSystemsAndParts(firstTypeId);
                }
            }
        },
        failure: function (data) {
            console.log(data.responseText);
            toastr["error"]("Please refresh the page!", "Error occured");
        },
        error: function (data) {
            console.log(data.responseText);
            toastr["error"]("Please refresh the page!", "Error occured");
        }
    });
}

function reloadSystemsAndParts(selectedType) {
    if (selectedType == null) {
        toastr["error"]("Please refresh the page!", "Error occured");
        $('#ajax-full-details-box-loading').show();
    }

    var urlStr = "/InventoryObjectSystems/GetInventoryObjectSystemsAndPartsPartial/" + selectedType;

    $.ajax({
        type: "GET",
        url: urlStr,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        beforeSend: function () {
            $('#ajax-full-details-box-loading').show();
        },
        success: function (data) {
            $("#box-systems-container").html(data);

            $(".nav-pills li a[subTypeId]").parent("li").removeClass("active");
            $(".nav-pills li a[subTypeId=" + selectedType + "]").parent("li").addClass("active");
            
            $('table.table-parts').DataTable({
                dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center'<'add-part-button-div'>><'col-sm-3'f>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row'<'col-sm-5'i><'col-sm-7'p>>",
                'columnDefs': [{
                    'targets': [3], 
                    'orderable': false
                }],
                initComplete: function () {
                    var systemId = $(this).parents(".table-responsive").attr("system-id");
                    var button = $("#add-part-button-" + systemId);
                    var buttonContainer = $(this).parents(".table-responsive").find(".add-part-button-div");
                    
                    buttonContainer.html(button.html());
                    button.remove();
                }  
            });

            $('#box-systems-container .box').boxWidget({
                animationSpeed: 300
            });

            $('#ajax-full-details-box-loading').hide();
        },
        failure: function (data) {
            toastr["error"]("Please refresh the page!", "Error occured");
        },
        error: function (data) {
            toastr["error"]("Please refresh the page!", "Error occured");
        }
    });
}

$(document).on('submit', '#' + confirmModalID + ' form:first', function (event) {
    event.preventDefault();

    $.ajax({
        type: "POST",
        url: $(this)[0].action,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#modal-overlay-loading").show();
        },
        statusCode: {
            200: function (response) {
                $('#' + confirmModalID).modal('hide');

                if (response.redirectToUrl == null) {
                    if (response.deleteModel == "type") {
                        reloadTypes();
                    } else if (response.deleteModel == "system" || response.deleteModel == "part") {
                        var subTypeId = $(".nav-pills li.active a").attr("subTypeId");
                        reloadSystemsAndParts(subTypeId);
                    }
                    console.log(response.toastMessage)
                    toastr["success"](response.toastMessage, "Successful operation");
                } else {
                    window.location.href = response.redirectToUrl;
                }
            },
            400: function (response) {
                console.log(data.responseText);
                toastr["error"]("Please refresh the page!", "Error occured");
            },
            404: function (response) {
                console.log(data.responseText);
                toastr["error"]("Please refresh the page!", "Error occured");
            }
        },
    });
})
.on("click", ".nav-pills li a[subTypeId]", function () {
    var selectedType = $(this).attr("subTypeId");
    reloadSystemsAndParts(selectedType);
})
.on("click", "#delete-part-btn", function () {
    var partId = $(this).attr("part-id");

    $.ajax({
        type: "GET",
        url: "/InventoryObjectParts/DeleteInventoryObjectPartAjaxAsync/" + partId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#modal-overlay-loading").show();
        },
        statusCode: {
            200: function (response) {
                $("#" + confirmModalID + " .modal-content").html(response.responseText);
                $("#" + confirmModalID).modal("show");
            },
            400: function (response) {
                console.log(data.responseText);
                toastr["error"]("Please refresh the page!", "Error occured");
            },
            404: function (response) {
                console.log(data.responseText);
                toastr["error"]("Please refresh the page!", "Error occured");
            }
        },
    });
})
.ready(function () {
    reloadGeneralInformationBox();
    reloadTypes();

    $("#delete-compressor-btn").click(function () {
        $.ajax({
            type: "GET",
            url: "/InventoryObjects/DeleteInventoryObjectAjaxAsync/" + compressorId,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $("#modal-overlay-loading").show();
            },
            statusCode: {
                200: function (response) {
                    $("#" + confirmModalID + " .modal-content").html(response.responseText);
                    $("#" + confirmModalID).modal("show");
                },
                400: function (response) {
                    console.log(data.responseText);
                    toastr["error"]("Please refresh the page!", "Error occured");
                },
                404: function (response) {
                    console.log(data.responseText);
                    toastr["error"]("Please refresh the page!", "Error occured");
                }
            },
        });
    });
});

onUpdateCompressorComplete = function (data) {
    if (data.responseText == "success") {
        $('#' + modalID).modal('hide');
        reloadGeneralInformationBox();
        toastr["success"]("The unit was successfully updated", "Successful operation");
    }
};

onAddCompressorTypeComplete = function (data) {
    if (data.responseText == "success") {
        $('#' + modalID).modal('hide');
        reloadTypes();
        toastr["success"]("A new type was successfully added", "Successful operation");
    }
};

onEditCompressorTypeComplete = function (data) {
    if (data.responseText == "success") {
        $('#' + modalID).modal('hide');
        reloadTypes();
        toastr["success"]("The type was successfully updated", "Successful operation");
    }
};

onDeleteCompressorTypeComplete = function (data) {
    var errors = $("#" + modalID + " .validation-summary-errors li").length;

    if (errors == 0) {
        if (data.responseJSON.status == "success") {
            var selectedCompressorType = data.responseJSON.selectedID;

            $.ajax({
                type: "GET",
                url: "/InventoryObjectTypes/DeleteInventoryObjectTypeAjaxAsync/" + selectedCompressorType,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#modal-overlay-loading").show();
                },
                statusCode: {
                    200: function (response) {
                        $('#' + modalID).modal('hide');
                        $("#" + confirmModalID + " .modal-content").html(response.responseText);
                        $("#" + confirmModalID).modal("show");
                    },
                    400: function (response) {
                        console.log(data.responseText);
                        toastr["error"]("Please refresh the page!", "Error occured");
                    },
                    404: function (response) {
                        console.log(data.responseText);
                        toastr["error"]("Please refresh the page!", "Error occured");
                    }
                },
            });
        }
    }
};

onAddCompressorSystemComplete = function (data) {
    if (data.responseText == "success") {
        var subTypeId = $(".nav-pills li.active a").attr("subTypeId");
        $('#' + modalID).modal('hide');
        reloadSystemsAndParts(subTypeId);
        toastr["success"]("A new system was successfully added", "Successful operation");
    }
};

onEditCompressorSystemComplete = function (data) {
    if (data.responseText == "success") {
        var subTypeId = $(".nav-pills li.active a").attr("subTypeId");
        $('#' + modalID).modal('hide');
        reloadSystemsAndParts(subTypeId);
        toastr["success"]("The system was successfully updated", "Successful operation");
    }
};

onDeleteCompressorSystemComplete = function (data) {
    var errors = $("#" + modalID + " .validation-summary-errors li").length;

    if (errors == 0) {
        if (data.responseJSON.status == "success") {
            var selectedCompressorSystem = data.responseJSON.selectedID;

            $.ajax({
                type: "GET",
                url: "/InventoryObjectSystems/DeleteInventoryObjectSystemAjaxAsync/" + selectedCompressorSystem,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $("#modal-overlay-loading").show();
                },
                statusCode: {
                    200: function (response) {
                        $('#' + modalID).modal('hide');
                        $("#" + confirmModalID + " .modal-content").html(response.responseText);
                        $("#" + confirmModalID).modal("show");
                    },
                    400: function (response) {
                        console.log(data.responseText);
                        toastr["error"]("Please refresh the page!", "Error occured");
                    },
                    404: function (response) {
                        console.log(data.responseText);
                        toastr["error"]("Please refresh the page!", "Error occured");
                    }
                },
            });
        }
    }
};

onAddPartComplete = function (data) {
    if (data.responseText == "success") {
        var subTypeId = $(".nav-pills li.active a").attr("subTypeId");
        $('#' + modalID).modal('hide');
        reloadSystemsAndParts(subTypeId);
        toastr["success"]("A new component was successfully added", "Successful operation");
    }
};

onEditPartComplete = function (data) {
    if (data.responseText == "success") {
        var subTypeId = $(".nav-pills li.active a").attr("subTypeId");
        $('#' + modalID).modal('hide');
        reloadSystemsAndParts(subTypeId);
        toastr["success"]("The component was successfully updated", "Successful operation");
    }
};