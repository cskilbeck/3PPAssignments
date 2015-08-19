$(document).ready(function () {

    var timer = null;
    var n = 0;

    $("#searchText").on("change keydown cut paste input", function () {

        function cancelTimer() {
            if (timer !== null) {
                clearTimeout(timer);
                timer = null;
            }
        }

        cancelTimer();

        timer = setTimeout(function () {
            cancelTimer();
            var t = $("#searchText").val();
            if (t.length > 0) {
                $("#info").html("Searching...");
                $.ajax("/find/Users/" + t, {
                    success: function (data, status, xhr) {
                        var table = "<table><thead><tr><th>User</th><th>Account</th><th>Email</th></tr></thead><tbody>";
                        var link = "https://developer.xboxlive.com/en-us/workspace/security/Pages/AssignRolesToAContact.aspx?contactid="
                        var r = 0;
                        for (n in data) {
                            var p = data[n];
                            table += "<tr><td><a href=" + link + "{" + p.ID + "}>" + p.Name + "</a></td><td>" + "" + p.Account + "</td><td>" + p.Email + "</td></tr>";
                            ++r;
                        }
                        table += "</tbody></table>";
                        $("#info").html(r + " results found");
                        $("#results").html(table);
                    },
                    error: function (xhr, status, error) {
                        $("#info").html("Error - can't get results!?");
                        $("#results").html("");
                    }
                });
            }
        }, 500);
    });

});