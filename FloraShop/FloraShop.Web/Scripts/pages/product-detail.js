
$(document).ready(function () {
    var owl = $("#owl1");
    owl.owlCarousel({
        items: 4, //10 items above 1000px browser width
        itemsDesktop: [1000, 3], //5 items between 1000px and 901px
        itemsDesktopSmall: [767, 3], // betweem 900px and 601px
        itemsTablet: [700, 2], //2 items between 600 and 0
        itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
        navigation: true,
    });
});