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
                else
                {
                    $("#Slide10button").removeClass("active");
                    $("#Slide10button").attr("onclick", "return false;");}
            });
            $("#Slide10Drp").change(function () {
                if ($(this).val() != "") {
                    $("#Slide10button").addClass("active");
                    $("#Slide10button").attr("onclick", "ShowNextNavigation('Slide11')");
                }
                else
                {
                    $("#Slide10button").removeClass("active");
                    $("#Slide10button").attr("onclick", "return false;");
                }
            });
            $('#tagbargehover').attr("style", "background: #b6b7b8;color: #fff;border: 1px solid #b6b7b8;");
            //ValidateContent('down');
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


$('.tagBadges > span').click(function () {

    var active = $('span.activeBadge')
    active.removeClass('activeBadge');
    $(this).toggleClass('activeBadge');

    $('#lnkText').empty();
    if (this.id == "link_1")
        $('#lnkText').append('In this section you will find general legal information, self-help packets and videos on your rights as a <br />renter and what to do if you have a problem with your landlord.<a> http://www.washingtonlawhelp.org/issues/housing/tenants-rights</a>');
    else if (this.id == "link_2")
        $('#lnkText').append('Browse the information below to find information about foreclosure prevention, foreclosure mediation and other foreclosure issues. <a>http://www.washingtonlawhelp.org/issues/housing/foreclosure-1</a>');
    else if (this.id == "link_3")
        $('#lnkText').append('Browse the resources below to find general information and resources on emergency shelter and assistance in Washington state.  <a>http://www.washingtonlawhelp.org/issues/housing/emergency-shelter-assistance</a>');
    else if (this.id == "link_4")
        $('#lnkText').append('In this section of Washington LawHelp you will find general information, self-help packets and resources for mobile home tenants. <a>http://www.washingtonlawhelp.org/issues/housing/mobile-home-park-tenants</a>');
    else
        $('#lnkText').append('Sed elementum felis velit, et euismod nibh pellentesque sed.Curabitur nec cursus orci, non faucibus nisl. Ut vel gravida arcu.Quisque malesuada ut nibh vitae auctor. Pellentesque et massaaugue. Ut felis orci, condimentum ut pulvinar ut, posuere vitae erat.Nulla laoreet pulvinar diam non condimentum. Proin non liberoconvallis augue sodales malesuada. Quisque consectetur, magnaeget ultricies dictum, mauris ipsum ultrices ante, in aliquet nuncodio vel orci. Nam tristique fringilla est, et iaculis nisi fermentum a.Sed vel magna sit amet urna convallis sagittis at eu enim. Sedgravida diam a purus dictum, et faucibus lacus facilisis. In porttitorsollicitudin convallis. Sed blandit, tellus nec faucibus facilisis, urnanisi fringilla sapien, id condimentum dui velit imperdiet lectus.Quisque quam mi, molestie ac eros in, porta pharetra magna.liquam posuere porta pulvinar. Morbi mattis dolor mi, sit amet rhoncus sem volutpat eget. Vestibulum et nisl in erat posuere tristique in sedaugue. Quisque lorem elit, consequat nec commodo quis, faucibus non urna. Nulla facilisi. In gravida ultricies ante a hendrerit. Etiamaccumsan nisl massa, nec tincidunt justo mattis eu. In mattis sem libero, id euismod neque congue a. Pellentesque consequat in leo sedhendrerit. Suspendisse fringilla ligula in sem porttitor congue. Proin nec est non mi fringilla venenatis in at nulla. Nullam lectus urna,');

    return false;
});

$('#modalSearch').click(function () {
    $('#divSnippet').removeClass('display-none')
});
$('#btnSearchClose').click(function () {
    $('#divSnippet').addClass('display-none')
});

//localStorage.setItem("returnSession", "Home.html");

$('#modalReturn').click(function () {
    if (localStorage.getItem("returnSession") != null)
        document.location.href = localStorage.getItem("returnSession");
});


//setTimeout(function () {
//    $('#searchModal').modal('show');
//}, 15000);



            