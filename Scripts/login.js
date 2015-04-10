$(document).ready(function () {
    Logout();
})
function Logout() {
    //$("#empEmail").prop("disabled")
    $("#logout").click(function () {
        var options = {};
        options.url = "Account/LogOff"
        options.type = "GET";
        options.contentType = "application/json";
        //options.error = function () { alert("Error retrieving states!"); };
        $.ajax(options);
    });
}
