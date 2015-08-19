$(document).ready(function () {

    var timer = null,
        n = 0,
        searchText = "";

    $("#searchText").on("change cut paste input", function () {

        function cancelTimer() {
            if (timer !== null) {
                clearTimeout(timer);
                timer = null;
            }
        }

        cancelTimer();

        timer = setTimeout(function () {
            var t = $("#searchText").val();
            cancelTimer();
            if (t.length > 0 && t !== searchText) {
                searchText = t;
                $("#info").html("Searching...");
                $.ajax("/find/Users/" + t, {
                    success: function (data, status, xhr) {
                        var table = "<table><thead><tr><th>User</th><th>Account</th><th>Email</th></tr></thead><tbody>",
                            link = "https://developer.xboxlive.com/en-us/workspace/security/Pages/AssignRolesToAContact.aspx?contactid=",
                            r = 0,
                            p;
                        for (n in data) {
                            p = data[n];
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