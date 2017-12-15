// Write your Javascript code.

$(document).on("click", ".full-link", function () {
    var $t = $(this);
    var url = $t.find("a").attr("href");
    console.log(url);
    if ($t.hasClass("full-link-confirm")) {
         if (!confirm('Weet u het zeker?')) {
              return false;
         }
    }
    window.location.href = url;
});

$(document).on("click", ".confirmClick", function (e) {
     if (!confirm('Weet u het zeker?')) {
         e.preventDefault();
         return false;
     }
});

$(document).on("click",
    ".reply",
    function(e) {
        $(".reactionUsername").html(" - " + $(this).data("username"));
        $(".reactionId").val(Number($(this).data("reactionid")));
});

$(document).ready(function () {
    var elm = $(".halfOffset");
    elm.each(function(index) {
        $(this).css("margin-left", ($(this).css("margin-left").replace("px", "") / 2) + "px");
        console.log("Set margin-left to half");
        console.log($(this).css("margin-left"));
    });

});
