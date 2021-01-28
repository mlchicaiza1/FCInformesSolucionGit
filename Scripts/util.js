
var isLoading = true;

$(function () {
    __init();
    loadDinamicsPanel();
});

function __init() {

    preparedDate();
    datePickers(); 
    prepateEntero();
    prepateNumber();
    prepateTelefono();
    prepateCurrency();
    

    preparedDateTime();
    dateTimePickers();       

  
}

$.ajaxSetup({
    cache: false
});

$(document).on("click", "#addContainer", function () {
    
    //Asigna FechaCreación ya que valida si existe o no
    var fechaCreacion = $("#FechaCreacion");
    if (fechaCreacion.length > 0) {
        fechaCreacion.val(getDateString(new Date()));
    }
});

var loadDinamicsPanel = function() {
     var dinamicPanels = $(document).find(".x_content_dinamic_load");

    $.each(dinamicPanels, function(index, sender) {
        sender = $(sender);
        var url = sender.data("url");
        var data = {};
        var callback = sender.data("callback");        
        if ($.trim(url) !== "") {
            sender.html("<div class='center-block'><i class='fa fa-spinner fa-pulse fa-2x fa-fw'></i><span class='sr-only'>Loading...</span></div>");
            //data["id"] = 12;
            
            getContent(sender, url, data, callback);    
        }
    });
}

var getContent = function (sender, url, data, fn) {
    $.ajax({
        type: "get",
        url: url,
        data: data,
        dataType: "html",
        success: function (d) {
            if (d) {
                sender.html(d);
                __init();

            }
            if (fn) {
                callFn(fn, data);
            }
        },
        error: function (e) {
            console.debug(e);
        }
    });
}

var getDateString = function(date, format)
{
    if (date.getDate) {
        var datestring = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return datestring;    
    }
    return "";
}

var toDate = function (value) {
    if (typeof value == "undefined" || value == null) {
        return "";
    }
    if (value.indexOf("Date") > -1) {
        value = value.substr(6);
    }
    return new Date(parseInt(value));
}

var getFullDateString = function(date, format)
{
    if (date.getDate) {
        var datestring = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" +
            date.getFullYear() + " " + ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2);
        return datestring;
    }
    return "";
}

$(document).on("change", ".checkbox-true-false", function () {
    $(this).val($(this).prop("checked") ? true : false);
});

$(document).on("hidden.bs.modal", ".modal-maintenance", function () {
    clearFormElem(".modal");
});

$(document).on("shown.bs.modal", ".ajustable", function () {

    return;

    //var intervalId = setInterval(function () {
        
    //    var height = $(this).find(".modal-body").innerHeight();
    //    var aviableHeight = ($(window).height() - 220);

    //    if (aviableHeight < height) {
    //        $(".ajustable > .modal-dialog > .modal-content > .modal-body").height(aviableHeight + "px");
    //    } else {
    //        $(".ajustable > .modal-dialog > .modal-content > .modal-body").height(height + "px");
    //    }

    //}, 1000);

    //$(this).attr("intervalid", intervalId);

});


$(document).on("hidden.bs.modal", ".ajustable", function () {
    var intervalId = $(this).attr("intervalid");
    clearInterval(intervalId);
});

$(document).on("click", ".add-filter", function () {
    var option = $(this).closest('.row').find('select option:selected');
    var formid = $(this).data("formid");
    var value = $(option).val();
    if (value != '-1') {
        var model = {
            type: $(option).data('type'),
            description: $(option).text().trim(),
            name: value,
            url: $(option).data('url')
        }
        addFilter(formid, model);
    }
});

$(document).on('click', '.close-filter', function () {
    var value = $(this).data("value");
    var div = $(this).closest('div');
    var parentDiv = div.parent();

    $('.select-filter option[value=' + value + ']').show();
    resetFilter($(this).closest('form'));
    $(div).remove();

    if ($(parentDiv).find('.control-filter').length == 0) {
        $(parentDiv).find('.btn-filter').addClass('hide');
    }

    resizeAllGrids();
});

function addFilter(formid, model, widthStyle) {

    $('.btn-filter').removeClass('hide');
    var selector = '#' + formid + ' .custom-filters';
    var html = createFilter(model, widthStyle);
    $(selector).prepend(html);
    preparedDate();

    resizeAllGrids();
}

function createFilter(filter, widthStyle) {

    widthStyle = !widthStyle ? "col-md-4" : widthStyle;

    var placeholder = filter.placeholder ? filter.placeholder : filter.description;
    var container = '<div class="form-group '+ widthStyle + ' control-filter">';
    var label = '<label class="control-label" for="id' + filter.name + '">' + filter.description + '</label></br>';
    //var btnClose = '<button type="button" data-value="' + filter.name + '" class="close close-filter"><span aria-hidden="true">×</span><span class="sr-only">Cerrar</span></button>';
    var btnClose = '';
    var componen = '';
    var disabled = filter.requiredValue != null && filter.requiredValue.length > 0 ? ' disabled' : '';
    switch (filter.type) {
        case 1://textbox
            componen = '<input type="text" placeholder="' + placeholder + '" name="' + filter.name + '" id="id' + filter.name + '" class="form-control text-uppercase" value = "' + filter.defaultValue + '" ' + disabled + ' />';
            break;
        case 2://select
            componen = '<select name="' + filter.name + '" id="id' + filter.name + '" class="form-control select2" ' + disabled + '><option value="">-- seleccionar --</option></select>';
            loadSelect('id' + filter.name, filter.url, filter.defaultValue);
            break;
        case 3://date
            componen = '<div class=""><div class="input-group date" id="txtDate" data-date-format="dd/MM/yyy">' +
                '<input type="text" id="id' + filter.name + '" name="' + filter.name + '" readonly="readonly" class="form-control" />' +
                '<span class="input-group-addon"><i class="fa fa-calendar"></i></span></div></div>';
            break;
        case 4://date Range
            container = '<div class="form-group ' + widthStyle + ' control-filter">' + btnClose +
                '<div class="row">' +
                '<div class="col-md-6">' +
                '<label class="control-label" for="idfrom' + filter.name + '">' + filter.description + ' - Desde</label>' +
                '<div data-date-format="dd/MM/yyyy" id="from" class="input-group date">' +
                '<input type="text" class="form-control" readonly="readonly" name="from_' + filter.name + '" id="idfrom' + filter.name + '">' +
                '<span class="input-group-addon"><i class="fa fa-calendar"></i></span>' +
                '</div></div>' +
                '<div class="col-md-6">' +
                '<label class="control-label" for="idto' + filter.name + '">' + filter.description + ' - Hasta</label>' +
                '<div data-date-format="dd/MM/yyyy" id="to" class="input-group date">' +
                '<input type="text" class="form-control" readonly="readonly" id="idTo' + filter.name + '" name="to_' + filter.name + '">' +
                '<span class="input-group-addon">' +
                '<span class="fa-calendar fa"></span>' +
                '</span></div></div></div></div>';
            return container;
        default:

    }

    return container + label + btnClose + componen + '</div>';
}

$(document).on("change", ".reloadChild", function () {
    var child = $(this).data('childid');
    var url = $(this).data('url');
    var value = $(this).val();
    loadSelect(child, url,value);

});

$(document).on("click", "a.delete:not(.disabled),li.delete:not(.disabled)", function (event, preventConfirm) {
    
    var $element = $(this),
        grids = $element.data("grid") ? $element.data("grid").split(' ') : undefined,
        url = $element.data('action'),
        id = $element.data('id'),
        callback = $element.data('callback');

    var cont = function (data) {
        if (grids) {
            for (var i = 0; i < grids.length; i++) {
                reloadGrid(grids[i], 1);
            }
        }

        callFn(callback, data);
    };

    confirm("Estás seguro de eliminar ?", function () {
        if (url) {
            $.ajax({
                type: "get",
                url: url,
                data: {
                    'id': id
                },
                success: function (d) {
                    if (d.success) {
                        success(d.message);
                        cont(d);
                    }
                },
                error: function (err) {
                    error(err);
                },
                dataType: 'json'
            });
        } else {
            cont(id);
        }
    });
    event.preventDefault();
    event.stopPropagation();
});

$(document).on("submit", "form.modal-Crud", function () {
    var $form = $(this),
        data = getCrudFields($form),
        grids = $form.data("grid") ? $form.data("grid").split(' ') : undefined,
        modal = $form.data("modal"),
        url = $form.attr('action'),
        callback = $form.data("callback");

    var cont = function (d) {
        if (grids && grids != '') {
            for (var i = 0; i < grids.length; i++) {
                reloadGrid(grids[i], 1);
            }
        }

        if (callback && callback != '') {
            callFn(callback, data, d);
        }

        if (modal && modal != '') {
            $('#' + modal).modal('hide');
        }
    };

    if (url && url != '') {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: 'json',
            success: function (d) {
                if (d.success == true) {
                    success(d.message || "Grabado Correctamente !!");
                    cont(d);
                } else {
                    error(d.message.length == 0 ? "Ocurrió un error. Por favor vuelva a interntarlo" : d.message);
                }
            },
            error: function (e) {
                error("Ocurrió un error. Por favor vuelva a interntarlo");
            },
        });
    } else {
        cont();
    }

    return false;
});

$(document).on("click", ".load-modal", function () {
    var $element = $(this),
        modal = $element.data("modal") || $element.closest("ul").data("modal"),
        url = $element.data('action'),
        id = $element.data('id'),
        hiddenId = $element.data('hidden'),
        callback = $element.data('callback'),
        viewUrl = $element.data('viewurl');

    if (hiddenId) {
        $("#" + hiddenId).val(id);
    }

    var data = { 'id': id };
    var dataType = "json";
    if (viewUrl) {
        data = "";
        dataType = "html";
    }

    if (url) {
        $.ajax({
            type: "get",
            url: url,
            data: data,
            dataType: dataType,
            success: function (d) {
                if (viewUrl) {
                    $('#' + modal).find(".modal-content").html(d);

                    if (hiddenId) {
                        $("#" + hiddenId).val(id);
                    }

                    jQuery.validator.unobtrusive.parse('#' + modal);
                    callFn(callback, id);
                } else {
                    callFn(callback, d);
                }

                if (modal) {
                    $('#' + modal).modal('show');
                }
            },
            error: function () {
                error("Error");
            }
        });
    } else {
        if (modal) {
            $('#' + modal).modal('show');
        }

        callFn(callback, id);
    }

    return false;
});

$(document).ajaxSend(function (e, response, options) {
    var preventLoading = false;
    if (!isLoading) {
        preventLoading = true;
        isLoading = true;
    } else {
        if (options.port) {

        } else if (options.url) {
            if (options.url.toString().indexOf("sidx") != -1) {
                preventLoading = true;
            }
        }
    }
    if (!preventLoading) {
        $("#loading-panel").show();
        $("#loading-panel").animate({ "right": "0px" }, "slow");
    }
});

$(document).ajaxComplete(function (event, xhr, options) {
    $("#loading-panel").animate({ "right": "-=300px" }, "slow", function () {
        $("#loading-panel").hide();
    });
    //if (xhr && xhr.responseJSON && xhr.responseJSON.success != undefined && !xhr.responseJSON.success) {
    //    var msg = xhr.responseJSON.message ? xhr.responseJSON.message : "Oops, it was an error, please try again";
    //    error(msg);
    //}
});

$(document).ajaxError(function (event, xhr, options, exception) {
    if (xhr.status === 0) {
        xhr.abort();
    } else {
        if (xhr.status !== 200) {
            error(xhr.status === 500 ? xhr.responseText : xhr.statusText);    
        }
    }
});

function clearForm(id) {
    clearFormElem('#' + id);
}

function clearFormElem(elem) {
    $(elem).find('input, textarea, select').removeClass('input-validation-error').val('');
}

//$(document).on("keypress", ".number,input[data-val-number]", function (event, elem) {
//    if (!event.charCode) return true;
//    var key = event.which;

//    var decimalSeparator =  $CULTURA.separadorDecimal.charCodeAt(0);

//    if (key >= 48 && key <= 57 // 0-9
//        || key == decimalSeparator 
//        || key == 45 //-Minus
//    ) {
//        return true;
//    }
//    return false;
//});

function prepateCurrency() {
    //console.debug($CULTURA)
    $(".currency,.money").inputmask('currency',
        {
            radixPoint: $CULTURA.separadorDecimal,
            groupSeparator: $CULTURA.separadorMiles,
            digits: $CULTURA.numeroDecimales,
            autoGroup: true,
            prefix: $CULTURA.prefijo,
            rightAlign: true
        });
}

function prepateNumber() {

    $(".number").inputmask('decimal',
        {
            radixPoint: $CULTURA.separadorDecimal,
            groupSeparator: $CULTURA.separadorMiles,
            digits: $CULTURA.numeroDecimales,
            autoGroup: true,
            rightAlign: true
        });
}

function prepateEntero() {

    $(".integer").inputmask('numeric',
        {
            radixPoint: "",
            groupSeparator: "",
            digits: 0,
            autoGroup: true,
            rightAlign: true
        });
}

function prepateTelefono() {
    $(".telefono").inputmask("([0]9) 9999-999");
    $(".celular").inputmask("([0]\\9) 9999-9999");
}

function preparedDate() {

    $('.calendar').datetimepicker({
        pickTime: false,
        language: 'es',
        format: $CULTURA.formatoFecha.toUpperCase(),
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    });
}

function datePickers() {

    var input = $("input.calendar");
    $(input).removeClass("calendar").wrap("<div><div class='input-group date' data-date-format='" + $CULTURA.formatoFecha.toUpperCase() + "'></div>");
    $(input).parent().append("<span class='input-group-addon'><i class='fa fa-calendar'></i></span>");

    preparedDate();
}


function preparedDateTime() {
    
    $('.calendar-time').datetimepicker({
        pickTime: true,
        language: 'es',
        format: $CULTURA.formatoFecha.toUpperCase(),
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    });
}


function dateTimePickers() {

    var input = $("input.calendar-time");
    $(input).removeClass("calendar-time").wrap("<div><div class='input-group calendar-time' data-date-format='" + $CULTURA.formatoFechaLarga + "'></div>");
    $(input).parent().append("<span class='input-group-addon'><i class='fa fa-calendar'></i></span>");

    preparedDateTime();
}

function callFn(fn, object, object2) {
    if (typeof window[fn] === "function") {
        var func = window[fn];
        return func.call(name, object, object2);
    } else {
        console.debug(fn);
        eval(fn).call("", object, object2);
    }

    return null;
};

function getCrudFields(formId) {
    var form = $(formId);
    var arr = form.serializeArray({ checkboxesAsBools : true });
    //arr = $.grep(arr, function (n) {
    //    return (n.value.length > 0);
    //});

    return arr;
}

function error(message) {
    showNotification('error', message);
}
function warning(message) {
    showNotification('warning', message);
}
function info(message) {
    showNotification('info', message);
}
function success(message) {
    showNotification('success', message);
}

function showNotification(type, message) {
    var title, color, icon;
    if (type == 'warning') {
        title = 'Warning';
        color = '#C79121';
        icon = 'fa fa-warning';
    } else if (type == 'error') {
        title = 'Error';
        color = '#CD3C54';
        icon = 'fa fa-thumbs-down';
    } else if (type == 'success') {
        title = 'Success';
        color = '#437C6A';
        icon = 'fa fa-thumbs-up';
    } else {
        title = 'Info';
        color = '#547989';
        icon = 'fa fa-info-circle';
    }
    toastr[type](message);
}

function confirm(message, confirmCallback, cancelCallback, confirmButtonText, cancelButtonText) {
    confirmButtonText = confirmButtonText || "Aceptar";
    cancelButtonText = cancelButtonText || "Cancelar";
    var options = {
        animation: 400,
        buttons: {

            cancel: {
                text: cancelButtonText,
                className: "btn btn-danger btn-xs",
                action: function () {
                    Apprise("close");
                    if ($.isFunction(cancelCallback)) {
                        cancelCallback.call(this);
                    }
                }
            },
            confirm: {
                text: confirmButtonText,
                className: "btn btn-primary btn-xs",
                action: function () {
                    Apprise("close");
                    if ($.isFunction(confirmCallback)) {
                        confirmCallback.call(this);
                    }
                }
            },
        }
    };
    Apprise(message, options);
}

function loadSelect(id, url, selectedItem, filterId, callback) {
    if (url) {
        $.ajax({
            type: "get",
            url: url,
            data:{id :filterId===''?'0':filterId},
            success: function (d) {

                try {
                    if (d.success) {

                        var select;
                        if (typeof id == "string") {
                            if (id.indexOf("#") === -1) {
                                select = $('#' + id);
                            } else {
                                select = $(id);
                            }
                        } else {
                            select = id;
                        }

                        var first = $(select).find('option').first();
                        $(select).html('');

                        var stringOptions = '';
                        for (var i = 0; i < d.values.length; i++) {
                            var item = d.values[i];

                            var selected = item.value == selectedItem ? ' selected' : '';

                            var option = '<option value="' + item.value + '" ' + selected + '>' + item.text + '</option>';
                            stringOptions += option;
                        }
                        $(select).append(first);
                        $(select).append(stringOptions);

                        if (callback) {
                            callback($(select));
                        }

                    } else {
                        error("Error");
                    }
                } catch (e) {

                } 
            },
            error: function (err) {
               error(err);
            },
            dataType: 'json'
        });
    }
}


$(document).on("click", ".dropdown-menu", function(event) {
    event.stopPropagation();
});

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for(var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).on("click", ".viewable.active", function () {

    var leftInit = $(".btn-editar").position().left;
    $(".btn-editar").css("position", "relative");

    var vibrate = function (count) {
        if (count % 2 === 0) {
            $(".btn-editar").css({ "left": "-=10px" });
        } else {
            $(".btn-editar").css({ "left": "+=10px" });
        }
        if (count > 0) {
            setTimeout(function () {
                vibrate(count-1);
            }, 10);
        } 
    }
    setTimeout(function() {
        vibrate(11);
    }, 11);
    
});

var ___resizeModal = function (height) {

    if (typeof height === "undefined") {
        height = $(".ajustable > .modal-dialog > .modal-content > .modal-body").innerHeight();
    }

    var aviableHeight = ($(window).height() - 220);
   
    if (aviableHeight < height) {
        $(".ajustable > .modal-dialog > .modal-content > .modal-body").height(aviableHeight + "px");
    } else {
        $(".ajustable > .modal-dialog > .modal-content > .modal-body").height(height + "px");
    }
    
}



var downloadFile = function (url, callback) {

    var iframe = document.createElement('iframe');
    iframe.id = "IFRAMEID";
    iframe.style.display = 'none';
    document.body.appendChild(iframe);
    iframe.src =url;
    iframe.addEventListener("load", function () {
        if (callback) {
            callback();    
        }
    });
}

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

$(document).ajaxStop($.unblockUI); 