var searchFormId = "#search_form", gridId = "GridMain";

$(window).bind('resize', function () {
    resizeAllGrids();
});

function resizeAllGrids(container) {
    $(container || document).find('.ui-jqgrid-btable').each(function (index, element) {
        var width = $(element).closest('.ui-jqgrid').parent().width();
        resizeGrid(element, width);
    });
}

function resizeGrid(grid, width) {
    $(grid).setGridWidth(width);
}


$(document).on("submit", searchFormId, function () {    
    reloadFilters(this);
    return false;
});

$(document).on("reset", searchFormId, function () {
    resetFilter(this);
});

function updateGridInfo(grid) {    
    preparePager(grid);
    var $grid = $(grid);
    var id = $grid.attr("id");
    var totalRecords = $grid.jqGrid('getGridParam', 'records');
    var perpageRecords = $grid.jqGrid('getGridParam', 'rowNum');
    var actualPage = $grid.jqGrid('getGridParam', 'page');
    var actualRecords = $grid.jqGrid('getGridParam', 'reccount');
    var record1 = (totalRecords == 0) ? 0 : perpageRecords * (actualPage - 1) + 1;
    var record2 = perpageRecords * (actualPage - 1) + actualRecords;
    var sep = " " + "de" + " ";
    $(".lblInfo[grid='" + id + "']").html("(" + record1 + " - " + record2 + sep + totalRecords + ")");
    if (actualPage < $grid.jqGrid('getGridParam', 'lastpage')) {
        $(".btnNext[grid='" + id + "']").removeClass("disabled");
        $(".btnEnd[grid='" + id + "']").removeClass("disabled");
    }
    else {
        $(".btnNext[grid='" + id + "']").addClass("disabled");
        $(".btnEnd[grid='" + id + "']").addClass("disabled");
    }
    if (actualPage > 1) {
        $(".btnPrev[grid='" + id + "']").removeClass("disabled");
        $(".btnHome[grid='" + id + "']").removeClass("disabled");
    }
    else {
        $(".btnPrev[grid='" + id + "']").addClass("disabled");
        $(".btnHome[grid='" + id + "']").addClass("disabled");
    }

    $(".ui-jqgrid").removeClass("ui-widget ui-widget-content");
    $(".ui-jqgrid-view").children().removeClass("ui-widget-header ui-state-default");
    $(".ui-jqgrid").removeClass("ui-widget-content");
    // add classes
    $(".ui-jqgrid-htable").addClass("table table-bordered  table-hover");
    $(".ui-jqgrid-btable").addClass("table table-bordered table-striped");
}

function gridLoadComplete(grid) {
    var $grid = $(grid);
    var $selector = $(".emptyMsgDiv", $grid.closest(".ui-jqgrid").parent());
    $selector.html($grid.jqGrid('getGridParam', 'emptyrecords'));
    if (grid.p.reccount === 0) {
        $grid.hide();
        $selector.show();
    } else {
        $grid.show();
        $selector.hide();
    }

   
}

function preparePager(gridp) {
    var id = $(gridp).attr("id");
    $(".btnNext[grid='" + id + "']").each(function (index, domEle) {
        // domEle == this
        var grid = $(domEle).attr("grid");
        $(domEle).on("click", function () {
            var page = $("#" + grid).jqGrid('getGridParam', 'page');
            var lastpage = $("#" + grid).jqGrid('getGridParam', 'lastpage');
            if (page < lastpage)
                reloadGrid(grid, page == lastpage ? page : page + 1);
        });
    });
    $(".btnPrev[grid='" + id + "']").each(function (index, domEle) {
        // domEle == this
        var grid = $(domEle).attr("grid");
        $(domEle).on("click", function () {
            var page = $("#" + grid).jqGrid('getGridParam', 'page');
            if (page > 1)
                reloadGrid(grid, page == 1 ? page : page - 1);
        });
    });
    $(".btnHome[grid='" + id + "']").each(function (index, domEle) {
        // domEle == this
        var grid = $(domEle).attr("grid");
        $(domEle).on("click", function () {
            var page = $("#" + grid).jqGrid('getGridParam', 'page');
            if (page > 1)
                reloadGrid(grid, 1);
        });
    });
    $(".btnEnd[grid='" + id + "']").each(function (index, domEle) {
        // domEle == this
        var grid = $(domEle).attr("grid");
        $(domEle).on("click", function () {
            var page = $("#" + grid).jqGrid('getGridParam', 'page');
            var lastpage = $("#" + grid).jqGrid('getGridParam', 'lastpage');
            if (page < lastpage)
                reloadGrid(grid, lastpage);
        });
    });
}

function reloadGrid(grid, page) {
    if (typeof page === "undefined") {
        $("#" + grid).trigger("reloadGrid");
    } else {
        $("#" + grid).trigger("reloadGrid", [{ page: page }]);    
    }
}

///
///example  rules = [{ Field: "accountname", Data: "7"},{ Field: "id", Data: "6"}];
/// op = 0 --equal 1 --delete
function execFilter(idgrid, filterArray) {
    var filterJson = JSON.stringify(filterArray);
    //console.debug(filterJson);
    var grid = $("#" + idgrid);
    var postdata = grid.jqGrid('getGridParam', 'postData');
    console.log(idgrid)
    $.extend(postdata, {
        filters: filterJson
    });
    grid.jqGrid('setGridParam', { search: true, postData: postdata });
    reloadGrid(idgrid, 1);
}

function AddParanIdGrid(idgrid, value) {
    var grid = $("#" + idgrid);
    var postdata = grid.jqGrid('getGridParam', 'postData');
    $.extend(postdata, {
        IdFilter: value
    });
    grid.jqGrid('setGridParam', { postData: postdata });
}

function clearFilter(idgrid) {
    var grid = $("#" + idgrid);
    var postdata = grid.jqGrid('getGridParam', 'postData');
    $.extend(postdata, {
        filters: null
    });
    grid.jqGrid('setGridParam', { search: true, postData: postdata });

}

function getDataGrid(ele) {
    var result = [];
    var dataGrid = $(ele).data("grid");
    if (dataGrid) {
        result = dataGrid.split(',');
    } else {
        result.push(gridId);
    }
    //return $(ele).data("grid") || gridId;
    return result;
}

function getFilterArray(formId) {
    var form = $(formId);
    var arr = form.serializeArray();
    arr = $.grep(arr, function (n) {
        return (n.value.length > 0);
    });

    var prefix = form.data("prefix");
    if ($.trim(prefix).length) {
        prefix += ".";
    }
    var data = $.map(arr, function (o) { return { field: o.name.replace(prefix, ""), data: o.value }; });

    return { rules: data };
}

function reloadFilters(form) {

    var $form = $(form),
    data = getFilterArray($form),
    grids = getDataGrid($form);
    
    $.each(grids, function () {        
        execFilter(this, data);
    });


};

function resetFilter(form) {
    var $form = $(form),
        grids = getDataGrid($form);
    $.each(grids, function () {
        clearFilter(this);
        reloadGrid(this, 1);
    });
}