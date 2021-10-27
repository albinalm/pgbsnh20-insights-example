// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/calculatorHub").build();
connection.start().then(function () {
    console.log("Connected!");
});
function calculate() {
    connection.invoke("Calculate", document.getElementById("txt_input").value);
}
connection.on("OnCalculationCompleted", function (calculationResult) {
    document.getElementById("lbl_result").innerHTML = "Result: " + calculationResult;
});