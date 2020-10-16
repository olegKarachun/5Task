$(document).ready(function () {
    $("#filter").autocomplete({
        source: 'api/Autocomplete/Search'
    })
})

var games = [];
games = document.getElementsByClassName("game_row");
filter = document.getElementById("filter");

function tableSearch() {
    var tiles = [];
    var tags = [];
    var phrase = document.getElementById("filter");
    tiles = document.getElementsByClassName("article");
    tags = document.getElementsByClassName("tag");
    var searchPhrase = new RegExp(phrase.value, 'i');
    var flag = false;
    for (var i = 0; i < tags.length; i++) {
        flag = false;
        flag = searchPhrase.test(tags[i].innerHTML);
        if (flag) {
            tiles[i].style.display = "";
        } else {
            tiles[i].style.display = "none";
        }
    }
}