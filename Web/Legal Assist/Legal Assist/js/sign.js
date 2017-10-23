$(document).ready(function () {
    $('#Slide2,#Slide3,#Slide4,#Slide5,#Slide6,#Slide7,#Slide8,#Slide9,#Slide10,#Slide11,#Slide12').hide();
    $("#Slide5textone,#Slide5texttwo,#Slide5textthree").focusout(function () {
        var textone = $("#Slide5textone").val();
        var texttwo = $("#Slide5texttwo").val();
        var textthree = $("#Slide5textthree").val();
        if (textone != "" && texttwo != "" && textthree != "") {
            $("#Slide5button").addClass("active");
            $("#Slide5button").attr("onclick", "ShowNextNavigation('Slide6')");
        }
        else {
            $("#Slide10button").removeClass("active");
            $("#Slide10button").attr("onclick", "return false;");
        }
    });
    $("#Slide10Drp").change(function () {
        if ($(this).val() != "") {
            $("#Slide10button").addClass("active");
            $("#Slide10button").attr("onclick", "ShowNextNavigation('Slide11')");
        }
        else {
            $("#Slide10button").removeClass("active");
            $("#Slide10button").attr("onclick", "return false;");
        }
    });
    $('#tagbargehover').attr("style", "background: #b6b7b8;color: #fff;border: 1px solid #b6b7b8;");
    ValidateContent('down');
});
function ShowNextNavigation(SlideVal) {
    $('#Slide1,#Slide2,#Slide3,#Slide4,#Slide5,#Slide6,#Slide7,#Slide8,#Slide9,#Slide10,#Slide11,#Slide12').hide();

    if (SlideVal == "Slide8") {
        $("#" + SlideVal).show();
        setTimeout(function () { $("#Slide8").hide(); $("#Slide9").show(); }, 1000);
    }
    else if (SlideVal == "Slide6") {
        var textone = $("#Slide5textone").val();
        var texttwo = $("#Slide5texttwo").val();
        var textthree = $("#Slide5textthree").val();
        if (textone != "" && texttwo != "" && textthree != "") {
            $("#" + SlideVal).show();
        }
    }
    else if (SlideVal == "Slide11") {
        if ($("#Slide10Drp").val() != "") {
            $("#" + SlideVal).show();
        }
    }
    else {
        $("#" + SlideVal).show();
    }
    ValidateContent('up');
}
function ShowSkipNavigation(SlideVal) {
    $('#Slide1,#Slide2,#Slide3,#Slide4,#Slide5,#Slide6,#Slide7,#Slide8,#Slide9,#Slide10,#Slide11,#Slide12').hide();
    $("#" + SlideVal).show();
    if (SlideVal == "Slide8") {
        setTimeout(function () { $("#Slide8").hide(); $("#Slide9").show(); }, 1000);
    }
}
function ValidateContent(param) {
    if (param == 'up') {
        $('#Downsection').attr("style", "display:none");
        $('#Upsection').attr("style", "display:''");
    }
    else {
        $('#Upsection').attr("style", "display:none");
        $('#Downsection').attr("style", "display:''");
    }
}

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('glyphicon-plus glyphicon-minus');
}
$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);

