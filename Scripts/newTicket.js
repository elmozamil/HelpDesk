$(document).ready(function () {
    loadCategories();
    getIssuerEmail();
    loadFileNames();
    //var connection = $.hubConnection();
    //var hub = connection.createHubProxy("pageHitsCounter");

    //hub.on("OnRecordHit", function (hitCount) {
    //    $('#pageHitCount').text(hitCount + " Active User");
    //});
    //connection.start(function () {
    //    hub.invoke("RecordHit");
    //})
})



function loadCategories() {
    $("#CategoryID").prop("disabled", true);
    $("#TypeID").change(function () {
        var selectedType = $("#TypeID option:selected").text();

        if (selectedType !== "Please select a Type") {
            var options = {};
            options.url = "/GetCategories?id=" + $("#TypeID").val();
            options.type = "GET";
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (categories) {
                $("#CategoryID").empty();
                for (var i = 0; i < categories.length; i++) {
                    $("#CategoryID").append("<option value=" + categories[i].CategoryID + ">" + categories[i].CategoryName + "</option>");
                }
                $("#CategoryID").prop("disabled", false);
            };
            options.error = function () { alert("Error retrieving states!"); };
            $.ajax(options);
        }
        else {
            $("#CategoryID").empty();
            $("#CategoryID").prop("disabled", true);
        }
    });
}

function getIssuerEmail() {
    $("#Issuer").change(function () {
        var selectedEmp = $("#Issuer option:selected").text();
        if (selectedEmp !== "Please select issuer") {
            var options = {};
            options.url = "/GetIssuerEmail?empCode=" + $("#Issuer").val();
            options.type = "GET";
            options.dataType = "json";
            options.contentType = "application/json";
            options.success = function (employees) {
                $("#empEmail").text(employees[0].Email);
                $("#empPhone").text(employees[0].PhoneNo);
            };
            options.error = function () { alert("Error retrieving states!"); };
            $.ajax(options);
        }
        else {
            $("#empEmail").text("");
            $("#empPhone").text("");
        }
    });
}


function loadFileNames() {
    var control = document.getElementById("Attachments");
    control.addEventListener("change", function (event) {


        var i = 0,
            files = control.files,
            len = files.length;


        document.getElementById("Selectedfiles").innerHTML = "";

        attachedFiles = "<table class='table table-striped table-bordered table-hover dataTable no-footer'> ";
        attachedFiles += "<thead> <tr> <th># <th> Name <th> Size</tr></thead><tbody>";

        for (; i < len; i++) {
            //var divrow = document.createElement("table");
            //divrow.className = "row";

            attachedFiles += "<tr class ='odd gradeX'>";
            attachedFiles += "<td class='left'>" + String(i + 1) + "</td>";
            attachedFiles += "<td class='left'>" + files[i].name + "</td>";
            attachedFiles += "<td class='left'>" + files[i].size + " bytes" + "</td>";
            attachedFiles += "</tr>";

            //var counter = document.createElement("div");
            //counter.className = "col-md-1 col-md-offset-1";
            //var counterText = document.createTextNode(String(i + 1));
            //counter.appendChild(counterText);
            //divrow.appendChild(counter);


            //var name = document.createElement("div");
            //name.className = "col-md-3";
            //var textname = document.createTextNode(files[i].name);
            //name.appendChild(textname);
            //divrow.appendChild(name);

            

            //var fsize = document.createElement("div");
            //fsize.className = "col-md-3";
            //var textfsize = document.createTextNode(files[i].size + " bytes");
            //fsize.appendChild(textfsize);
            //divrow.appendChild(fsize);



            //document.getElementById("Selectedfiles").appendChild(divrow)
        }
        attachedFiles += "</tbody></table>";
        document.getElementById("Selectedfiles").innerHTML = attachedFiles;

    }, false);
}


