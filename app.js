var divError = "#divError";
var divSuccess = "#divSuccess";

function SetDatePicker() {
    $(".datePicker").datepicker({
        dateFormat: "dd MM, yy",
        changeMonth: true,
        changeYear: true
    });
}

function CloseAlert(eleId) {
    $(eleId).hide("fade");
    $(eleId + "Text").text("");
}

function BuildMessageContainer(isSuccess, message) {
    if (isSuccess) {
        $(divSuccess).show("fade");
        $(divSuccess + "Text").text(message);
    } else {
        $(divError).show("fade");
        $(divError + "Text").text(message);
    }
}

function ClearBuildMessage() {
    $(divSuccess).hide("fade");
    $(divSuccess + "Text").text("");
    $(divError).hide("fade");
    $(divError + "Text").text("");
}

function ShowModal(result) {
    SetDatePicker();
    if (result) {
        if (result.responseJSON) {
            if (result.responseJSON.Status == "Failed") {
                BuildMessageContainer(false, result.responseJSON.Message);
            }
        }
    }
    ClearBuildMessage();
    $("#EntityModal").modal("show");
}

function HideModal() {
    $("#EntityModal").modal("hide");
    $(".modal-backdrop").remove();
}

function Saved(result) {
    HideModal();
    if (result) {
        if (result.Status == "Success") {
            if (typeof indexUrl != "undefined") {
                $.ajax({
                    url: indexUrl,
                    cache: false,
                    success: function (data) {
                        $("#EntityData").html(data);
                        BuildMessageContainer(true, result.Message);
                    },
                    error: function (jqXHR) {
                        BuildMessageContainer(false, jqXHR.responseText);
                    }
                })
            } else {
                BuildMessageContainer(true, result.Message);
            }
        } else {
            BuildMessageContainer(false, result.Message);
        }
    } else {
        BuildMessageContainer(false, "Server error occured");
    }
}

function Failed(jqXHR) {
    if (jqXHR) {
        BuildMessageContainer(false, jqXHR.responseText);
    } else {
        BuildMessageContainer(false, "Server error occured");
    }
}

function CheckExtension(ele,btnUploadId) {
    var fileName = $(ele).val();
    var ext = fileName.substring(fileName.indexOf("."), fileName.length).toLowerCase();
    var validExt = [".png", ".jpg", ".jpeg"];
    for (var i = 0; i < validExt.length; i++) {
        if (ext == validExt[i]) {
            $(btnUploadId).removeAttr("disabled");
            return;
        }
    }
    $(btnUploadId).attr("disabled", "disabled");
    BuildMessageContainer(false, "Please upload valid photo");
}