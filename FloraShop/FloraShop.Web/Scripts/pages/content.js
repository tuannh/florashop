$(window).load(function () {
    $('.iosSlider').iosSlider({
        desktopClickDrag: true,
        snapToChildren: true,
        infiniteSlider: true,
        snapSlideCenter: true,
        navPrevSelector: '.slideSelectors .prev',
        navNextSelector: '.slideSelectors .next',
        onSliderLoaded: slideLoad,
        onSlideChange: slideChange,
        autoSlide: true,
        scrollbar: true,
        scrollbarMargin: '0',
        scrollbarBorderRadius: '0',
        keyboardControls: true,
        onSliderResize: setHeightBanner
    });
    //$(window).resize(function () {
    //    setHeightBanner();
    // });
});

function slideChange(args) {
    $('.slider .item').removeClass('selected');
    $('.slider .item:eq(' + (args.currentSlideNumber - 1) + ')').addClass('selected');
}

function slideLoad() {
    setHeightBanner();
    $('.slider .item').first().addClass('selected');
}

function setHeightBanner() {

    var item = $('.iosSlider .slider .item img').size();
    switch (item) {
        case 0:
            $('.fluidHeight').css('display', 'none');
            break;
        case 1:
            $('.slideSelectors').css('display', 'none');
            break;
        default:
            var setHeight = $('.slider .item img').height();
            $('.fluidHeight').css('height', setHeight + 'px');
            break;
    }

}
