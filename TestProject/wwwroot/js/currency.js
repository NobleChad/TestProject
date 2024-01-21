document.addEventListener("DOMContentLoaded", function () {
    var currencyFilter = document.getElementById("currencyFilter");
    var exchangeRate = 36.92;

    currencyFilter.addEventListener("change", function () {
        var selectedCurrency = currencyFilter.value;
        var priceAmounts = document.getElementsByClassName("price-amount");

        for (var i = 0; i < priceAmounts.length; i++) {
            var priceAmount = priceAmounts[i];
            var price = parseFloat(priceAmount.textContent);

            if (selectedCurrency === "UAH") {
                priceAmount.textContent = (price * exchangeRate).toFixed(2) + "UAH";
            } else {
                priceAmount.textContent = (price / exchangeRate).toFixed(2) + "$";
            }
        }
    });
});