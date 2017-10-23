$(document).ready(function () {
      $('#Slide2,#Slide3,#Slide4,#Slide5').hide();
      ValidateContent('up');
});
function ShowNextNavigation(SlideVal) {
    $('#Slide1,#Slide2,#Slide3,#Slide4,#Slide5').hide();

     $("#" + SlideVal).show();
  
    ValidateContent('up');
}
function ShowSkipNavigation(SlideVal) {
    $('#Slide1,#Slide2,#Slide3,#Slide4,#Slide5').hide();
    $("#" + SlideVal).show();
}

localStorage.setItem("returnSession", "guided.html");

