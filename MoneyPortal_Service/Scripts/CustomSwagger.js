﻿$("title").text("Budget Duck API");
(function () {
    var link = document.querySelector("link[rel*='icon']") || document.createElement('link');;
    document.head.removeChild(link);
    link = document.querySelector("link[rel*='icon']") || document.createElement('link');
    document.head.removeChild(link);
    link = document.createElement('link');
    link.type = 'image/x-icon';
    link.rel = 'shortcut icon';
    link.href = '/Images/favicon-32x32.png'; // This is the only part needing to be changed
    document.getElementsByTagName('head')[0].appendChild(link);
})();