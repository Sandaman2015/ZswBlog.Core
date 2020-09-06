$(document).ready(function () {
  var $wrap = $("#wrap"),
    $pages = $("#product_list").find(".product_box"),
    $controlBox = $("#control_box"),
    $productBtns = $("#product_btns"),
    $controls = $controlBox.find("a"),
    $icLine = $("#ic_line"),
    $listTops = $("#list_top").find("li"),
    $hNav = $("#h_nav"),
    $hNavli = $hNav.find("li"),
    $hSubnav = $("#h_subnav"),
    $hSubDD = $hSubnav.find("dd"),
    $hBdot = $("#h_bdot"),
    $hSdot = $("#h_sdot"),
    $footer = $("#footer"),
    $bgs = $pages.find(".bg_box img"),
    $mainBoxs = $pages.find(".main_box"),
    $moreNav = $("#more_nav"),
    $rptBg = $("#rpt_bg");
  var data = {
    pLength: $pages.length,
    curP: 0,
    isCan: true,
    isOnbtn: false,
    fColor: [2, 2, 1, 1, 1],
    dur: 3500,
    cNum: 0
  };
  var aDD = $controls.eq(0).width() + parseInt($controls.eq(0).css("margin-right").slice(0, -2)) * 2;
  var cId;
  var isCss3 = function () {
    var style = document.createElement("div").style;
    for (var k in style) {
      if (k.toLowerCase().indexOf("animation") > 0) {
        return true;
      }
    }
    return false;
  }();

  var isIE6 = navigator.userAgent.indexOf("MSIE 6.0") > 0;
  //页面自缩放
  var resize = function () {
    var w = $(window).width(),
      h = $(window).height();
    $wrap.height(h);
    $moreNav.height(h);
    if (w / h < 1920 / 1080) {
      $bgs.height(h).css({ width: "auto", margin: -.5 * h + "px 0 0 " + -.5 * 1920 / 1080 * h + "px" });
    } else {
      $bgs.width(w).css({ height: "auto", margin: -.5 * w * 1080 / 1920 + "px 0 0 " + -.5 * w + "px" });
    }
    var imgH = $bgs.height();
    $mainBoxs.height(imgH).css("margin-top", -.5 * imgH + "px");
    var cls;
    if (isCss3) {
      // $("#warp").attr("class", "wrap big_view xbig");
      document.getElementById("wrap").className = "wrap big_view xbig";
      switch (true) {
        case w >= 1920: { cls = ""; break; }
        case w < 1920 && w >= 1680: { cls = "small9"; break; }
        case w < 1680 && w >= 1600: { cls = "small8"; break; }
        case w < 1600 && w >= 1536: { cls = "small7"; break; }
        case w < 1536 && w >= 1440: { cls = "small6"; break; }
        case w < 1440 && w >= 1366: { cls = "small5"; break; }
        case w < 1366 && w >= 1280: { cls = "small4"; break; }
        case w < 1280 && w >= 1024: { cls = "small3"; break; }
        case w < 1024: { cls = "small2"; break; }
      }
      for (var i = 0; i < data.pLength; i++) {
        $pages.eq(i).find(".content").attr("class", "content " + cls);
      }
      $productBtns.find(".content").attr("class", "content " + cls);
    } else {
      switch (true) {
        case w >= 1600: { cls = "big_view"; break; }
        case w < 1600 && w >= 1440: { cls = "mid_view"; break; }
        case w < 1440: { cls = "small_view"; break; }
      }
      var ws = $wrap[0].className;
      if (isIE6 && (ws.indexOf("big") != -1 || ws.indexOf("mid") != -1 || ws.indexOf("small") != -1) && ws.indexOf(cls) == -1)
        location.reload();
      wrap.className = "wrap " + cls;
    }
    aDD = $controls.eq(0).width() + parseInt($controls.eq(0).css("margin-right").slice(0, -2)) * 2;
  };
  $(window).resize(resize);
  resize();
  var pageChange = function (idx) {
    if (data.isOnbtn)
      return;
    if (idx >= -1 && idx < data.pLength && idx != data.curP && data.isCan) {
      data.isCan = false;
      data.cNum++;
      clearInterval(cId);
      idx = idx == -1 ? data.pLength - 1 : idx;
      $controls.removeClass("cur").eq(idx).addClass("cur");
      $pages.eq(data.curP).css({ zIndex: 0 });
      $icLine.css({ left: 100 * idx });
      // $footer.attr("class","footer f_color"+data.fColor[idx]);
      $pages.eq(idx).addClass("show").css({ opacity: 0, zIndex: 1 }).animate({ opacity: 1 }, 400, function () {
        $pages.eq(data.curP).removeClass("show");
        $(this).addClass("show");
        data.isCan = true;
        data.curP = idx;
        data.cNum--;
        if (data.cNum == 0) {
          cId = setInterval(function () {
            pageChange((data.curP + 1) % data.pLength);
          }, data.dur);
        }
      });
    }
  }
  $controls.on("mouseenter", function () {
    data.isCan = true;
    pageChange($controls.index(this));
    $icLine.css({ left: $controls.index(this) * 100 });
    data.isOnbtn = true;
  });
  $controls.on("mouseleave", function () {
    data.isOnbtn = false;
  });

  var isBottom = false;
  var bAni;
  var bottomHide = function () {
    if (isBottom) {
      isBottom = false;
      if (bAni)
        clearTimeout(bAni), bAni = null;
      $footer.removeClass("show");
      $productBtns.removeClass("show");
      $rptBg.removeClass("show");
    }
  }
  var bottomShow = function () {
    if (!isBottom) {
      isBottom = true;
      $footer.addClass("show");
      $productBtns.addClass("show");
      $rptBg.addClass("show");
      if (bAni)
        clearTimeout(bAni);
      bAni = setTimeout(bottomHide, 2000);
    }
  }
  $footer.on("mouseenter", function () {
    if (bAni)
      clearTimeout(bAni), bAni = null;
  });
  $footer.on("mouseleave", function () {
    if (bAni)
      clearTimeout(bAni);
    bAni = setTimeout(bottomHide, 2000);
  });
  $(document).on("mousewheel DOMMouseScroll", function (event) {
    var sd = event.originalEvent.wheelDelta || event.originalEvent.detail * -1;
    if (sd > 0) {
      bottomHide();
    }
    else {
      bottomShow();
    }
  });
  cId = setInterval(function () {
    pageChange((data.curP + 1) % data.pLength);
  }, data.dur);

  //导航事件

  $("#nav_more").on("mouseenter", function () {
    $moreNav.addClass("show");
  });
  $moreNav.on("mouseleave", function () {
    $moreNav.removeClass("show");
    $hBdot.css({ left: -9999, top: 106 });
  })
  $hNavli.on("mouseenter", function () {
    $hBdot.css({ left: 18, top: 106 + $hNavli.index(this) * 69 });
  });
  $hNav.on("mouseleave", function () {
    $hBdot.css({ left: -9999 });
  });
  $hSubnav.find("dt").on("mouseenter", function () {
    $hBdot.css({ left: 18 });
    $hSdot.css({ left: -9999 });
  });
  $hSubDD.on("mouseenter", function () {
    $hSdot.css({ left: 18, top: 53 + $hSubDD.index(this) * 44 });
    $hBdot.css({ left: -9999 });
  });
  $hSubnav.on("mouseleave", function () {
    $hSdot.css({ left: -9999 });
  });
})
