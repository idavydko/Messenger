function deleteUser(btn) {
    let data = $(btn).data();

    $.ajax({
        type: "POST",
        url: "/admin/delete",
        data: data,
        success: function (response) {
            $(btn).parent().remove();
        },
        error: function (response) {
            alert("Something went wrong");
        }
    });
}

function copyContentToMessenger() {
    let text = $("#MessageAreaSMS").val(),
        imagesToSelect = $("#InsertImageModal img");

    if (document.imagesToCopy && document.imagesToCopy.length > 0) {
        for (let i = 0; i < document.imagesToCopy.length; i++) {
            let src = document.imagesToCopy[i];
            text = text.replace(src, '');

            for (let y = 0; y < imagesToSelect.length; y++) {
                if (src == $(imagesToSelect[y]).attr("src")) {
                    $(imagesToSelect[y]).parent().addClass("img-selected");
                }
            }
        }

        insertImages();
    }

    $("#Message").val(text);
    $("#SMS, #SMS-tab").removeClass("active");
    $("#Messenger, #Messenger-tab").addClass("active");
}

function submitSmsForm() {
    if (!$("#MessageAreaSMS").val()) {
        alert("Message is required.");

        return false;
    }

    $("#SmsForm").submit();
}

function insertUrls() {
    let selectedImages = $("#InsertUrlModal .img-container.img-selected"),
        text = $("#MessageAreaSMS").val();

    if (selectedImages.length <= 0) {
        return;
    }

    if (!document.imagesToCopy) {
        document.imagesToCopy = [];
    }

    for (let i = 0; i < selectedImages.length; i++) {
        let src = $(selectedImages[i]).find("img").attr("src");

        document.imagesToCopy.push(src);
        text = text + " " + src + " ";

        $(selectedImages[i]).click();
    }
    $("#MessageAreaSMS").val(text);

    $('#InsertUrlModal').modal('hide');
}

function insertImages() {
    let selectedImages = $("#InsertImageModal .img-container.img-selected");

    if (selectedImages.length <= 0) {
        return;
    }

    let root = $("div.inserted-images");

    for (let i = 0; i < selectedImages.length; i++) {
        let div = document.createElement("div");
        let a = document.createElement("a");

        $(a).on("click", function (e) {
            $(e.target).parent().parent().remove();
        });

        $(root).append(
            $(div).attr("class", "col-md-3 framed-block img-container").append(
                $(a).attr("class", "detach-link").append(
                    $(selectedImages[i]).find("img").clone())));

        $(selectedImages[i]).click();
    }

    $('#InsertImageModal').modal('hide');
}

function toggleInsertButtons() {
    let insert = $(".insert-button");

    if (insert && insert.length > 0) {
        $(insert).after('<button class="btn btn-default remove-button" onclick="toggleInsertButtons(); return false;">Remove answer buttons</button>');
        $(".inserted-buttons-message").append("<h4>BUTTONS INSERTED</h4>");
        insert.remove();
    } else {
        let remove = $(".remove-button");
        $(remove).after('<button class="btn btn-default insert-button" onclick="toggleInsertButtons(); return false;">Insert answer buttons</button>');
        $(".inserted-buttons-message").empty();
        remove.remove();
    }

    return false;
}

function sumbitMessengerForm() {
    let model = {},
        text = $("#Message").val(),
        images = $("div.inserted-images img"),
        buttonsInserted = $(".remove-button");

    if (!text) {
        alert("Message is required.");

        return false;
    }

    if (buttonsInserted.length > 0 || images.length > 0) {
        model.attachment = {
            "type": "template",
            "payload": {
                "template_type": "generic",
                "elements": []
            }
        };

        if (images.length > 0) {
            generateMessengerImages(images, text, model.attachment.payload.elements);
        }

        if (buttonsInserted.length > 0) {
            generateButtonsAttachment(text, model.attachment.payload.elements);
        }

    } else {
        model.text = text;
    }

    $.ajax({
        type: "POST",
        url: "/admin/SendToAllMessenger",
        data: model,
        success: function (response) {
            location.href = "/admin/success";
        },
        error: function (response) {
            console.log("Something went wrong");
        }
    });
}

function generateMessengerImages(images, text, elements) {
    if (images && images.length > 0) {
        for (let i = 0; i < images.length; i++) {
            elements.push({
                "title": text,
                'image_url': $(images[i]).attr("src")
            });
        }
    }

    return elements;
}

function generateButtonsAttachment(text, elements) {
    elements.push({
        "title": text,
        "buttons": [
            {
                "type": "postback",
                "title": "Yes",
                "payload": "yes",
            },
            {
                "type": "postback",
                "title": "No",
                "payload": "no",
            }
        ]
    });
}

$("#InsertUrlModal .img-container, #InsertImageModal .img-container").on("click", function () {
    $(this).toggleClass("img-selected");
});


