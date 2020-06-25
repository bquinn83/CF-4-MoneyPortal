$(".logo__title").text("Money Portal");
(function () {
    var link = document.querySelector("link[rel*='icon']") || document.createElement('link');;
    document.head.removeChild(link);
    link = document.querySelector("link[rel*='icon']") || document.createElement('link');
    document.head.removeChild(link);
    link = document.createElement('link');
    link.type = 'image/x-icon';
    link.rel = 'shortcut icon';
    link.href = '/Content/favicon.ico'; // This is the only part needing to be changed
    document.getElementsByTagName('head')[0].appendChild(link);
})();