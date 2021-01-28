var cultura = function (parms) {

    this.separadorMiles = parms && parms.separadorMiles ? parms.separadorMiles : ",";
    this.separadorDecimal = parms && parms.separadorDecimal ? parms.separadorDecimal : ".";
    this.formatoFecha = parms && parms.formatoFecha ? parms.formatoFecha : "dd/mm/yyyy";
    this.formatoFechaLarga = parms && parms.formatoFechaLarga ? parms.formatoFechaLarga : "dd/mm/yyyy hh:mm";
    this.zonaHoraria = "-5";
    this.numeroDecimales = parms && parms.numeroDecimales ? parms.numeroDecimales : 2;
    this.prefijo = parms && parms.prefijo ? parms.prefijo + " " : "$ ";

    this.cero = "0" + this.separadorDecimal + "00";
};

$CULTURA = new cultura(cultureParams);


Number.prototype.formatCurrentMoney = function () {
    return this.formatMoney(2, $CULTURA.separadorDecimal, $CULTURA.separadorMiles);
};

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = $CULTURA.numeroDecimales,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return $CULTURA.prefijo + s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

Number.prototype.formatNumber = function (c, d, t) {
    var n = this,
        c = $CULTURA.numeroDecimales,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = String(parseInt(n = Math.abs(Number(n) || 0).toFixed(c))),
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

Number.prototype.roundMoney = function () {
    var n = this;
    return parseFloat(n.toFixed($CULTURA.numeroDecimales));
};

var getMoney = function (value) {

    if (typeof value === "string") {
        value = value.replaceAll(",", "");
        value = value.replaceAll(".", "");
        value = value.replaceAll($CULTURA.prefijo.trim(), "");
        value = value.replaceAll(" ", "");
        console.log(value);
        return parseInt(value) / getFactor();
    }
    else if (typeof value === "number") {
        return parseFloat(parseFloat(value).toFixed($CULTURA.numeroDecimales));
    }
};

var getNumber = function (value) {

    if (typeof value === "string") {
        value = value.replaceAll(",", "");
        value = value.replaceAll(".", "");
        value = value.replaceAll($CULTURA.prefijo.trim(), "");
        value = value.replaceAll(" ", "");

        return parseFloat(value);
    }
    else if (typeof value === "number") {
        return parseFloat(parseFloat(value).toFixed($CULTURA.numeroDecimales));
    }
};

var getDecimal = function (value) {

    if (typeof value === "string") {

        var numeroDecimales = 0;

        if (value.indexOf($CULTURA.separadorDecimal) > -1) {
            numeroDecimales = value.split($CULTURA.separadorDecimal)[1].length;
        }
        value = value.replaceAll(",", "");
        value = value.replaceAll(".", "");
        value = value.replaceAll($CULTURA.prefijo.trim(), "");
        value = value.replaceAll(" ", "");

        var divisor = 1;
        for (var i = 0; i < numeroDecimales; i++) {
            divisor *= 10;
        }


        return parseInt(value) / divisor;
    }
    else if (typeof value == "number") {
        return parseFloat(parseFloat(value).toFixed($CULTURA.numeroDecimales));
    }
};

var getFactor = function () {
    var f = "1";
    for (var i = 0; i < $CULTURA.numeroDecimales; i++) {
        f += "0";
    }
    return parseInt(f);
};

var formatDate = function (date) {
    var d = new Date(date);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }

    var dateString = $CULTURA.formatoFecha.toUpperCase();

    dateString = dateString.replace("DD", day);
    dateString = dateString.replace("MM", month);
    dateString = dateString.replace("YYYY", year);


    //var dateString = day + "/" + month + "/" + year;

    return dateString;
};