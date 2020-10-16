var queue;
var modal = document.getElementById("my_modal");
var wait_modal = document.getElementById("wait_modal");
var btn = document.getElementById("btn_modal_window");
var cells = document.getElementsByClassName("cell");
var span = document.getElementsByClassName("close_modal_window")[0];
const hubConnection = new signalR.HubConnectionBuilder().withUrl("/Game").build();
const TypeOfMove = document.getElementById("move").getAttribute("name");


span.onclick = function () {
    modal.style.display = "none";
}


window.addEventListener("unload", function () {    
    hubConnection.invoke('RemoveFromGame', document.getElementById("exit").getAttribute("name"));
});

hubConnection.on("Move", function (id_cell) {
    var box = document.getElementById(id_cell);
    changeQueue();
    box.textContent = TypeOfMove == "x" ? "o" : "x";
    result(modal, TypeOfMove);
});

hubConnection.on("CheckOpp", function () {
    wait_modal.style.display = "none";
});

function result(modal, typeOfMove) {
    if (checkWinner() == typeOfMove) {
        document.getElementById("winner").textContent = "Вы выиграли))";
        modal.style.display = "block";
    } else if (checkWinner() == (typeOfMove == "x" ? "o" : "x")) {
        document.getElementById("winner").textContent = "Вы проиграли((";
        modal.style.display = "block";
    } else if (checkDraw()) {
        document.getElementById("winner").textContent = "Ничья))";
        modal.style.display = "block";
    }
}

function setQueue() {
    if (document.getElementById("wait_modal").getAttribute("name") == "join") {
        queue = false;
    } else {
        queue = true;
    }
}

function changeQueue() {
    if (queue) {
        queue = false;
        return false;
    } else {
        queue = true;
        return true;
    }
}

function myFunction() {
    var popup = document.getElementById("myPopup");
    popup.classList.toggle("show");
}

function checkDraw() {
    var takenCells = 0;
    for (var i = 0; i < cells.length; i++) {
        if (cells[i].innerHTML != "") {
            takenCells++;
        }
    }
    if (takenCells == 9) return true;
    return false;
}

$(document).ready(function () {
    setQueue();
    $(".cell").on("click", function () {
        var cell_text = jQuery(this).text();
        if (cell_text != "")
            alert("Занято");
        else if (queue) {
            $(this).text(TypeOfMove);
            checkDraw();
            result(modal, TypeOfMove);
            var id_cell = jQuery(this).attr('id');
            id_cell = parseInt(id_cell);
            let groupName = document.getElementById("exit").getAttribute("name");
            hubConnection.invoke("Move", id_cell, groupName);
            changeQueue();
        } else{
            alert("Сейчас ход соперника");
        }
    });
    hubConnection.start();    
})

function checkWinner() {
    var your_symb = TypeOfMove;
    var opp_symb = TypeOfMove == "x" ? "o" : "x";
    var winner = "";
    var j = 0;
    var cell_1 = cells[0].innerHTML;
    var cell_5 = cells[4].innerHTML;
    var cell_9 = cells[8].innerHTML;
    var cell_3 = cells[2].innerHTML;
    var cell_5 = cells[4].innerHTML;
    var cell_7 = cells[6].innerHTML;
    if ((cell_1 && cell_5 && cell_9) || (cell_3 && cell_5 && cell_7)) {
        if (cell_1 == your_symb && cell_5 == your_symb && cell_9 == your_symb) {
            winner = your_symb;
        }
        else if (cell_1 == opp_symb && cell_5 == opp_symb && cell_9 == opp_symb) {
            winner = opp_symb;
        }
        if (cell_3 == your_symb && cell_5 == your_symb && cell_7 == your_symb) {
            winner = your_symb;
        }
        else if (cell_3 == opp_symb && cell_5 == opp_symb && cell_7 == opp_symb) {
            winner = opp_symb;
        }
    }
    if (!winner) {
        for (var i = 0; i < 3; i++) {
            var a1 = cells[i].innerHTML;
            var a2 = cells[i + 3].innerHTML;
            var a3 = cells[i + 6].innerHTML;
            j = 3 * i;
            b1 = cells[j].innerHTML;
            b2 = cells[j + 1].innerHTML;
            b3 = cells[j + 2].innerHTML;
            if (a1 == your_symb && a2 == your_symb && a3 == your_symb) {
                winner = your_symb;
                break;
            }
            else if (a1 == opp_symb && a2 == opp_symb && a3 == opp_symb) {
                winner = opp_symb;
                break;
            }
            if (b1 == your_symb && b2 == your_symb && b3 == your_symb) {
                winner = your_symb;
                break;
            }
            else if (b1 == opp_symb && b2 == opp_symb && b3 == opp_symb) {
                winner = opp_symb;
                break;
            }
        }
    }
    return winner;
}