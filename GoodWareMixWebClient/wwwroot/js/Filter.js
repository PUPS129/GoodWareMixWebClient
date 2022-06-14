function SessionHelperIndexButton() {
    var Attributes = document.getElementById('Attribute').value   //PageSize
    var Searchs = document.getElementById('Search').value
    var PageSizes = document.getElementById('PageSize').value
    $.ajax({
        type: "GET",
        url: "@Url.Action("SessionHelperIndex")",
        data: { Search: Searchs, PageSize: PageSizes, Attribute: Attributes },
        error: function (req, status, error) {
            alert(error);
        }
    });

function addFilter() {
    if (document.getElementById('autocomplete').value != "") {
        var elem1 = document.createElement("div");
        var val = document.getElementById('autocomplete').value
        var elemText = document.createTextNode(val);
        elem1.id = "AttributeKey";
        var articleDiv = document.getElementById("FilterBody");
        elem1.appendChild(elemText);

        $.ajax({
            type: "GET",
            url: "@Url.Action("ValueAttribut")",
            data: { number1: val },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                var Divcontainer = document.createElement("div"); // 1

                Divcontainer.className = "container";
                Divcontainer.style = "padding:0px";
                Divcontainer.id = val;
                var DivRow = document.createElement("div"); // 1
                DivRow.className = "row";

                var FilterBody = document.createElement("div"); // 1.1
                FilterBody.className = "input-group mb-3";


                var elem = document.createElement("select"); // 1.1.1
                elem.className = "form-select";
                elem.id = "Attribute";
                elem.name = "Attribute";


                var DivButton = document.createElement("div"); // 1.1.2
                var Button = document.createElement("button");
                Button.className = "btn btn-danger";
                Button.type = "button";
                Button.onclick = function () { Divcontainer.remove(); }

                var ButtonI = document.createElement("i");
                ButtonI.className = "bi bi-x-lg";
                Button.appendChild(ButtonI);
                DivButton.appendChild(Button);
                FilterBody.appendChild(DivButton);

                //console.log(meetup.Value);

                for (let i = 0; i < msg.length; i++) {
                    elem.options[elem.options.length] = new Option(msg[i], val + ";" + msg[i]);
                }

                FilterBody.appendChild(elem); //1.1.1 => 1.1
                FilterBody.appendChild(DivButton); // 1.1.2 => 1.1

                DivRow.appendChild(elem1);
                DivRow.appendChild(FilterBody);
                Divcontainer.appendChild(DivRow);
                articleDiv.appendChild(Divcontainer);
                document.getElementById('autocomplete').value = "";

            },
            error: function (req, status, error) {
                alert(error);
            }
        });
    }
}
function GetNameAttribute() {
    let arr = [];

    var valueinput = document.getElementById('autocomplete').value;
    $.ajax({
        type: "GET",
        url: "@Url.Action("GetAttribute")",
        data: { Search: valueinput },
        //contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            for (let i = 0; i < msg.data.length; i++) {
                console.log(msg.data[i].nameAttribute);
                arr.push(msg.data[i].nameAttribute);
            }
            console.log("-------------------");
        },
        error: function (req, status, error) {
            alert(error);
        }
    });

    return arr;
}