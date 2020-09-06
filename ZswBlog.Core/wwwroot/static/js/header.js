$(document).ready(function () {
  "use strict";

  var underlineMenuItems = document.querySelectorAll("ul li");
  // underlineMenuItems[1].classList.add("active");
  underlineMenuItems.forEach(function (item) {
    item.addEventListener("click", function () {
      underlineMenuItems.forEach(function (item) { return item.classList.remove("active"); });
      item.classList.add("active");
    });
  });

});