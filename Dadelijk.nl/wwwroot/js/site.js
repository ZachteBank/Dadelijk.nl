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
