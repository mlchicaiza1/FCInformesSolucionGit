/**
 * Resize function without multiple trigger
 * 
 * Usage:
 * $(window).smartresize(function(){  
 *     // code here
 * });
 */
(function($,sr){
    // debouncing function from John Hann
    // http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
    var debounce = function (func, threshold, execAsap) {
      var timeout;

        return function debounced () {
            var obj = this, args = arguments;
            function delayed () {
                if (!execAsap)
                    func.apply(obj, args); 
                timeout = null; 
            }

            if (timeout)
                clearTimeout(timeout);
            else if (execAsap)
                func.apply(obj, args);

            timeout = setTimeout(delayed, threshold || 100); 
        };
    };

    // smartresize 
    jQuery.fn[sr] = function(fn){  return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr); };

})(jQuery,'smartresize');
/**
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

var CURRENT_URL = window.location.href.split('?')[0],
    $BODY = $('body'),
    $MENU_TOGGLE = $('#menu_toggle'),
    $SIDEBAR_MENU = $('#sidebar-menu'),
    $SIDEBAR_FOOTER = $('.sidebar-footer'),
    $LEFT_COL = $('.left_col'),
    $RIGHT_COL = $('.right_col'),
    $NAV_MENU = $('.nav_menu'),
    $FOOTER = $('footer');

var toogleClick = function(){};
// Sidebar
$(document).ready(function() {
    // TODO: This is some kind of easy fix, maybe we can improve this
    var setContentHeight = function () {
        // reset height
        $RIGHT_COL.css('min-height', $(window).height());

        var bodyHeight = $BODY.outerHeight(),
            footerHeight = $BODY.hasClass('footer_fixed') ? -10 : $FOOTER.height(),
            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

        // normalize content
        contentHeight -= $NAV_MENU.height() + footerHeight;

        $RIGHT_COL.css('min-height', contentHeight);
    };

    $SIDEBAR_MENU.find('a').on('click', function (ev) {

        var activeClass = $("body").hasClass("nav-sm") ? "active active-sm" : "active";
        var mainUp = false;
        var mainDown = false;

        var $li = $(this).parent();
      
        $('ul', $li.parent()).each(function (index, ul) {
            var $ul = $(ul);
            if (ul != $li.find("ul")[0]) {
                $ul.data("state", "close");
                $ul.slideUp(function () {
                    setContentHeight();
                });
            }
        });


        if (!$li.parent().is('.child_menu')) {
            var state = $li.find("ul").data("state");            
            if (typeof state === "undefined" || state === "close") {
                mainDown = true;
                $('ul:first', $li).slideDown(function () {
                    setContentHeight();
                });
                $li.find("ul:first").data("state", "open");
            } else {
                mainUp = true;
                $SIDEBAR_MENU.find('li ul').slideUp();
                $li.find("ul:first").data("state", "close");
            }
        } else {          
            var state = $li.find("ul").data("state");            
            if (typeof state === "undefined" || state === "close") {
                $('ul:first', $li).slideDown(function () {
                    setContentHeight();
                });                
                $li.find("ul:first").data("state", "open");
            } else {
                $('ul:first', $li).slideUp(function () {
                    setContentHeight();
                });
                $li.find("ul:first").data("state", "close");
            }            
        }
       

        if (typeof $(this).attr("href") !== "undefined") {

            $(".menu_section a").removeClass("active");
            $(".menu_section li").removeClass("active active-sm");
            if ($("body").hasClass("nav-sm")) {
                $('li ul', $SIDEBAR_MENU).each(function (index, ul) {
                    var $ul = $(ul);
                    $ul.data("state", "close");
                    $ul.slideUp(function () {
                        setContentHeight();
                    });
                });
            }           
        }
        if (mainDown) {
            $(".menu_section li").removeClass("active active-sm");
        } 

        $li.addClass(activeClass);

        var $parent = $li.parent();

        while (!$parent.hasClass("menu_section")) {

            $parent.addClass(activeClass);
            $parent = $parent.parent();
        }        

        $(this).addClass('active');
    });

    // toggle small or large menu
    $MENU_TOGGLE.on('click', function() {
        toogleClick();
    });

     toogleClick =  function() {
        if ($BODY.hasClass('nav-md')) {
            $SIDEBAR_MENU.find('li.active ul').hide();
            $SIDEBAR_MENU.find('li.active-sm').addClass('active').removeClass('active-sm');
            
        } else {
            $SIDEBAR_MENU.find('li.active-sm ul').show();
            $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');
        }

        $BODY.toggleClass('nav-md nav-sm');

        setContentHeight();
    }
    
    // //check active menu
    // $SIDEBAR_MENU.find('a[href="' + CURRENT_URL + '"]').parent('li').addClass('current-page');
    
    //var protocol = location.protocol;
    //var slashes = protocol.concat("//");
    //var host = slashes.concat(window.location.hostname) + "/";
    //var baseUrlAux = slashes.concat(window.location.hostname) + (baseUrl.indexOf("/") > -1 ? baseUrl : + baseUrl + "/");
    //var currentUrl = window.location.href;
    //var menuUrl = $("#MenuUrl").val();
    //if (menuUrl !== "") {
    //    currentUrl = slashes.concat(window.location.hostname) + $("#MenuUrl").val();
    //}
    
    //var found = false;
    //var contadorBusquedas = 0;
    //do {
    //    contadorBusquedas++;

    //    if (contadorBusquedas >= 20) {
    //        found = true;
    //        break;
    //    }

    //    found = $SIDEBAR_MENU.find('a').filter(function () { return this.href === currentUrl }).length > 0;
    //    if (found) {
    //        $SIDEBAR_MENU.find('a[href="' + currentUrl + '"]').parent('li').addClass('current-page');
    //        $SIDEBAR_MENU.find('a').filter(function () {
    //            return this.href == currentUrl;
    //        }).parent('li').addClass('current-page').parents('ul').slideDown(function () {
    //            setContentHeight();
    //        }).parent().addClass('active');
    //    } else {
    //        if (currentUrl.indexOf("&") > -1) {
    //            var newUrl = currentUrl.split("&")[0];
    //            var length = currentUrl.split("&").length;
    //            for (var i = 1; i < length - 1; i++) {
    //                newUrl = newUrl + "/" + currentUrl.split("&")[i];
    //            }
    //            currentUrl = newUrl;
    //        }
    //        else if (currentUrl.indexOf("?") > -1) {
    //            currentUrl = currentUrl.split("?")[0];
    //        }
    //        else if (currentUrl === baseUrlAux) {
    //            break;
    //        }
    //        else
    //        {                
    //            var newUrl = currentUrl.split("/")[0];
    //            var length = currentUrl.split("/").length;
    //            for (var i = 1; i < length - 1; i++) {
    //                newUrl = newUrl + "/" + currentUrl.split("/")[i];
    //            }
    //            currentUrl = newUrl;             
    //        }
    //    }
    //} while (!found)

    // var menus = $SIDEBAR_MENU.find('a').filter(function () {

    //     if (this.href === "" || this.href === host || this.href === baseUrlAux) {
    //         return false;
    //     }
         
    //    var contains = CURRENT_URL.indexOf(this.href);
    //    if (contains > -1) {
    //        console.debug(this.href);
    //        return true;
    //    }
    //    return false;
    //});

    //console.debug(menus);

    //$SIDEBAR_MENU.find('a').filter(function () {
    //    if (this.href === "" || this.href === host || this.href === baseUrlAux) {
    //        return false;
    //    }
    //    var contains = CURRENT_URL.indexOf(this.href);
    //    if (contains > -1) {
    //        return true;
    //    }
    //    return false;
    //}).parent('li').addClass('current-page').parents('ul').slideDown(function() {
    //    setContentHeight();
    //}).parent().addClass('active');

    // recompute content when resizing
    $(window).smartresize(function(){  
        setContentHeight();
    });

    setContentHeight();

    // fixed sidebar
    if ($.fn.mCustomScrollbar) {
        $('.menu_fixed').mCustomScrollbar({
            autoHideScrollbar: true,
            theme: 'minimal',
            mouseWheel:{ preventDefault: true }
        });
    }
});
// /Sidebar

// Panel toolbox
$(document).ready(function() {
    $('.collapse-link').on('click', function() {
        var $BOX_PANEL = $(this).closest('.x_panel'),
            $ICON = $(this).find('i'),
            $BOX_CONTENT = $BOX_PANEL.find('.x_content');
        
        // fix for some div with hardcoded fix class
        if ($BOX_PANEL.attr('style')) {
            $BOX_CONTENT.slideToggle(200, function(){
                $BOX_PANEL.removeAttr('style');
            });
        } else {
            $BOX_CONTENT.slideToggle(200); 
            $BOX_PANEL.css('height', 'auto');  
        }

        $ICON.toggleClass('fa-chevron-up fa-chevron-down');
    });

    $('.close-link').click(function () {
        var $BOX_PANEL = $(this).closest('.x_panel');

        $BOX_PANEL.remove();
    });
});
// /Panel toolbox

// Tooltip
$(document).ready(function() {
    $('[data-toggle="tooltip"]').tooltip({
        container: 'body'
    });
});
// /Tooltip

// Progressbar
if ($(".progress .progress-bar")[0]) {
    $('.progress .progress-bar').progressbar();
}
// /Progressbar




// Table
$('table input').on('ifChecked', function () {
    checkState = '';
    $(this).parent().parent().parent().addClass('selected');
    countChecked();
});
$('table input').on('ifUnchecked', function () {
    checkState = '';
    $(this).parent().parent().parent().removeClass('selected');
    countChecked();
});

var checkState = '';

$('.bulk_action input').on('ifChecked', function () {
    checkState = '';
    $(this).parent().parent().parent().addClass('selected');
    countChecked();
});
$('.bulk_action input').on('ifUnchecked', function () {
    checkState = '';
    $(this).parent().parent().parent().removeClass('selected');
    countChecked();
});
$('.bulk_action input#check-all').on('ifChecked', function () {
    checkState = 'all';
    countChecked();
});
$('.bulk_action input#check-all').on('ifUnchecked', function () {
    checkState = 'none';
    countChecked();
});


// NProgress
if (typeof NProgress != 'undefined') {
    $(document).ready(function () {
        NProgress.start();
    });
    $(window).on('load', function () {        
        setTimeout(() => {
            NProgress.done();
        }, 250);
    });    
}