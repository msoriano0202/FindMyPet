
/*
    @
    @   CNOTE RESPONSIVE TEMPLATE JS
    @   1.  scrollTo
    @   2.  scrollUpPage
    @   3.  sectionScroll
    @   4.  stickyNavbar
    @   5.  changeLogoSrc
    @   6.  changeCurrency
    @   7.  dropdownSubmenu
    @   8.  focusInputAddons
    @   9.  magnificPopups
    @   10. masonryInit
    @   11. tooltipsInit
    @   12. popversInit
    @   13. counterUp
    @   14. counterDown
    @   15. rangeSliders
    @   16. quantity
    @   17. progressBars
    @   18. progressCircles
    @   19. customFile
    @   20. owlThemeCarousels
    @
*/

// Preloader
$(window).on('load', function () {
    $('#preloader').fadeOut(200);
});

$(window).resize(function() {
    fullScreen();
});
fullScreen();

// Fullscreen Banner
function fullScreen() {
    var header = $("#header");
    var banner = $('.banner');
    var windowWidth = $(window).width();
    
    if(banner.hasClass('fullscreen')) {
        banner.height($(window).height());
        header.css('position','absolute');
        if((banner.hasClass('fullscreen-height-auto')) && (windowWidth < 975 )) {
            banner.css({
                'height': 'auto'
            });
        }
    }
}

$(function () {
    'use strict';
    
    var blackLogo = 'http://via.placeholder.com/150x50/eee/777';
    var whiteLogo = 'http://via.placeholder.com/150x50/2c343a/fff';
    
    scrollTo();
    scrollUpPage();
    sectionScroll();
    stickyNavbar();
    changeLogoSrc();
    changeCurrency();
    dropdownSubmenu();
    focusInputAddons();
    magnificPopups();
    masonryInit();
    tooltipsInit();
    popversInit();
    counterUp();
    counterDown();
    rangeSliders();
    quantity();
    progressBars();
    progressCircles();
    customFile();
    owlThemeCarousels();
    
    
    // 1.
    // Scroll to Next Element
    function scrollTo(){
        
        var mainNavbar = $(".cn-navbar-wrapper");
        var scrollButton = $('.scroll-next');
        
        scrollButton.on('click', function(e) {
            e.preventDefault();
            var scrollToNext = scrollButton.closest('section').next();
            if (scrollButton.data('scroll-to')) {
                scrollToNext = $(scrollButton.data('scroll-to'));
            }
            
            if((mainNavbar).hasClass("navbar-scrollspy")) {
                $('html, body').animate({
                    scrollTop: scrollToNext.offset().top - 40
                }, 1000);
            } else {
                $('html, body').animate({
                    scrollTop: scrollToNext.offset().top
                }, 1000);
            }
         
        });
    }
    
    // 2.
    //  Scroll Up Page
    function scrollUpPage() {
        $(window).scroll(function() {
            if ($(this).scrollTop() > 500) {
                $('#scrollUp:hidden').stop(true, true).fadeIn();
            } else {
                $('#scrollUp').stop(true, false).fadeOut();
            }
        });
        
        $("#scrollUp a").on("click", function() { //  a[href='#top']
            $("html, body").animate({ scrollTop: 0 }, 600);
            return false;
        });

    }
    
    // 3.
    // Window scroll animate used for onepages
    function sectionScroll() {
        var headerHeight = $(".navbar-scrollspy").innerHeight();
        $(".navbar-scrollspy .nav-link").on('click', function(e) {
            // prevent default anchor click behavior
            e.preventDefault();
            
            var _this = $(this);
            var hash = _this.attr('href');
            
            $('html, body').stop().animate({
                scrollTop: $(hash).offset().top - 40
            }, 500);
        });
    }
    
    // 4.
    // Sticky Navbar
    function stickyNavbar() {
        var navbar = $(".navbar-scrollspy");
        
        $(window).scroll(function() {
            var windowpos = $(window).scrollTop();
            if (windowpos > 50) {
                navbar.addClass("sticky-navbar");
                if(navbar.hasClass("sticky-navbar")) {
                    $("img.logo").attr("src", blackLogo);
                    // $("img.logo").attr("src", "../../assets/images/"+blackLogo);
                }
            } else if  (windowpos == 0 ){
                navbar.removeClass("sticky-navbar");
                if(navbar.hasClass("cn-navbar-white-color")) {
                    $("img.logo").attr("src", whiteLogo);
                }
            }
        });
    }
    
    // 5.
    // Change logo src ( black / white ) according to background header
    function changeLogoSrc() {
        var mainHeader = $('.cn-navbar-wrapper');
        
        if(mainHeader.hasClass("cn-navbar-white-color")) {
            $("img.logo").attr("src", whiteLogo);
        } else if (mainHeader.hasClass("cn-navbar-dark-color")) {
            $("img.logo").attr("src", blackLogo);
        } else {
            $("img.logo").attr("src", blackLogo);
        }
    }
    
    // 6.
    // Change Currency ( E-Commerce Topbar )
    function changeCurrency() {
        
        var _currentCurrency = $(".current-currency");
        var _currencyListOptions = $(".currency-list a");
    
        _currencyListOptions.on("click", function (e) {
            e.preventDefault();
            
            var _currencyVal = $(this).data("currency");
            _currentCurrency.text(_currencyVal);
            
        })
        
    }
    
    // 7.
    // Dropdown Submenu
    function dropdownSubmenu() {
        $('.dropdown-menu a.dropdown-toggle').on('click', function(e) {
            if (!$(this).next().hasClass('show')) {
                $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
            }
            var $subMenu = $(this).next(".dropdown-menu");
            $subMenu.toggleClass('show');
            
            $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
                $('.dropdown-submenu .show').removeClass("show");
            });
            return false;
        });
        
        // Prevents the dropdown menu closing when you click inside.
        $('.dropdown-menu').on('click', function(event) {
            event.stopPropagation();
        });
    }
    
    // 8.
    // FocusInputAddons
    function focusInputAddons() {
        // This will help to style input-group-prepend / append when an input it's focused
        $( ".input-group .form-control" ).focus(function() {
            $(this).parent('.input-group').addClass('focused');
        });
        $( ".input-group .form-control" ).focusout(function() {
            $(this).parent('.input-group').removeClass('focused');
        });
    }
    
    // 9.
    // Magnific Popups
    function magnificPopups() {
        $('.images-gallery').each(function () {
            $(this).magnificPopup({
                delegate: '.popup-img',
                type: 'image',
                mainClass: 'mfp-fade',
                removalDelay: 400,
                callbacks: {
                    beforeOpen: function() {
                        // just a hack that adds mfp-anim class to markup
                        this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
                        this.st.mainClass = this.st.el.attr('data-effect');
                    }
                },
                gallery: {
                    enabled: true,
                    navigateByImgClick: true,
                    arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>', // markup of an arrow button
                    tPrev: 'Previous',
                    tNext: 'Next',
                    tCounter: '<span class="mfp-counter">%curr% of %total%</span>' // markup of counter
                }
            })
        });
    
        $('.images-gallery-zoom').magnificPopup({
            delegate: '.popup-img',
            type: 'image',
            mainClass: 'mfp-with-zoom',
            zoom: {
                enabled: true,
                duration: 300,
                easing: 'ease-in',
                opener: function(openerElement) {
                    return openerElement.is('img') ? openerElement : openerElement.find('img');
                }
            },
            gallery: {
                enabled: true,
                navigateByImgClick: true,
                arrowMarkup: '<button title="%title%" type="button" class="mfp-arrow mfp-arrow-%dir%"></button>', // markup of an arrow button
                tPrev: 'Previous',
                tNext: 'Next',
                tCounter: '<span class="mfp-counter">%curr% of %total%</span>' // markup of counter
            }
        });
    }
    
    // 10.
    // Masonry
    function masonryInit() {
        var $grid = $('.grid').imagesLoaded( function() {
            // init Masonry after all images have loaded
            $grid.masonry({
                columnWidth: '.grid-sizer',
                itemSelector: '.grid-item'
            });
        });
    }
    
    // 11.
    // Tooltips
     function tooltipsInit() {
         $('[data-toggle="tooltip"]').tooltip({
             trigger : 'hover'
         })
     }
    
    // 12.
    // Popovers
    function popversInit() {
        $('[data-toggle="popover"]').popover()
    }
    
    // 13.
    // CounterUp
    function counterUp() {
        var options = {
            useEasing : true,
            useGrouping : true,
            separator : ',',
            decimal : '.',
            prefix : '',
            suffix : ''
        };
        $(".counters .countup").each(function() {
            var countVal = $(this).data('counter');
            var countup = new CountUp(this, 0, countVal, 0, 2.5, options);
        
            $(this).appear(function() {
                countup.start();
            }, {accX: 0, accY: -35})
        });
    }
    
    // 14.
    // Countdown
    function counterDown() {
        $(".countdown").each(function () {
            var countdownValue = $(this).data('date');
            $(this).countdown(countdownValue, function (event) {
                $(this).html(event.strftime(
                    '<div class="date-block"><div class="date-time">%D </div><span class="date-text">day%!d </span></div>' +
                    '<div class="date-block"><div class="date-time">%H </div><span class="date-text">Hr </span></div>' +
                    '<div class="date-block"><div class="date-time">%M </div><span class="date-text">Min </span></div>' +
                    '<div class="date-block"><div class="date-time">%S </div><span class="date-text">Sec </span></div>'
                ));
            })
        })
    }
    
    // 15.
    // Range Sliders
    function rangeSliders() {
        $( ".slider-range" ).slider({
            range: true,
            min: 0,
            step: 1,
            max: 1000,
            values: [ 100, 750 ],
            slide: function( event, ui ) {
                $(".slider-amount").text( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
            }
        });
        $( ".slider-amount" ).text( "$" + $( ".slider-range" )
                .slider( "values", 0 ) + " - $" + $( ".slider-range" )
                .slider( "values", 1 )
        );
    
        $( ".slider-range-2" ).slider({
            range: true,
            min: 0,
            step: 1,
            max: 1000,
            values: [ 100, 750 ],
            slide: function( event, ui ) {
                $(".slider-amount-2").text( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
            }
        });
        $( ".slider-amount-2" ).text( "$" + $( ".slider-range-2" )
                .slider( "values", 0 ) + " - $" + $( ".slider-range-2" )
                .slider( "values", 1 )
        );
    
        $( ".slider-range-3" ).slider({
            range: "min",
            min: 0,
            step: 50,
            max: 1000,
            value: 100,
            slide: function( event, ui ) {
                $( ".slider-amount-3" ).text( "$" + ui.value );
            }
        });
        $( ".slider-amount-3" ).text( "$" + $( ".slider-range-3" ).slider( "value" ));
    }
    
    // 16.
    // Quantity Inputs
    function quantity() {
        $(".quantity-minus").on("click", function(e) {
            e.preventDefault();
            var fieldName = $(this).attr('data-field');
            var currentVal = parseInt($('input[id='+fieldName+']').val());
            if (!isNaN(currentVal) && currentVal > 0) {
                $('input[id='+fieldName+']').val(currentVal - 1);
            } else {
                $('input[id='+fieldName+']').val(0);
            }
        });
    
        $('.quantity-plus').on("click", function(e){
            e.preventDefault();
            var fieldName = $(this).attr('data-field');
            var currentVal = parseInt($('input[id='+fieldName+']').val());
            if (!isNaN(currentVal)) {
                $('input[id='+fieldName+']').val(currentVal + 1);
            } else {
                $('input[id='+fieldName+']').val(0);
            }
        });
    }
    
    // 17.
    // ProgressBars
    function progressBars() {
        $('.progress-bar').each(function() {
            var _this = $(this);
            var perc = _this.attr("aria-valuenow");
            var current_perc = 0;
            
            $(this).appear(function(){
                var progress = setInterval(function() {
                    if (current_perc >= perc) {
                        clearInterval(progress);
                    } else {
                        current_perc +=1;
                        _this.css('width', (current_perc)+'%');
                    }
                    _this.find(".tooltiptext").css('opacity', '1').text((current_perc)+'%');
                    
                }, 20);
            }, {accX: 0, accY: -50})
        });
    }
    
    // 18.
    // ProgressCircles
    function progressCircles() {
        $('.progress-circle').each(function () {
            $(this).appear(function() {
                var circleColor = $(this).find('.circle').attr('data-fill-color');
                var percent = $(this).find('.circle').attr('data-percent');
                var percentage = parseInt(percent, 10) / parseInt(100, 10);
                var animate = $(this).data('animate');
            
                $(this).data('animate', true);
                $(this).find('.circle').circleProgress({
                    startAngle: -Math.PI / 2,
                    value: percent / 100,
                    size: 150,
                    emptyFill: "#eeeeee",
                    lineCap: 'round',
                    fill: {
                        color: circleColor
                    }
                }).on('circle-animation-progress', function (event, progress, stepValue) {
                    $(this).find('.circle-percentage').html(( stepValue * 100 ).toFixed(0) + "<span class='perc'>%</span>");
                }).stop();
            
            }, {accX: 0, accY: -50})
        });
    }
    
    // 19.
    // Custom File Input
    function customFile() {
        $( '.cn-custom-file-input' ).each( function() {
            var $input	 = $( this ),
                $label	 = $input.next( 'label' ),
                labelVal = $label.html();
        
            $input.on( 'change', function( e ) {
                var fileName = '';
            
                if( this.files &&  e.target.value  )
                    fileName = e.target.value.split( '\\' ).pop();
            
                if( fileName )
                    $label.find( 'span' ).html( fileName );
                else
                    $label.html( labelVal );
            });
        });
    }
    
    // 20.
    // OwlThemeCarousels
    function owlThemeCarousels() {
        $('.feature-carousel').owlCarousel({
            autoplay:true,
            autoplayTimeout: 5000,
            nav: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: true,
            center: false,
            touchDrag: true,
            responsive:{
                0:{
                    items:1,
                    margin:15,
                    nav: false
                },
                768: {
                    items:2,
                    margin:15,
                    nav: false
                },
                992:{
                    items:2,
                    margin:15
                },
                1200:{
                    items:3,
                    margin:20
                }
            }
        });
        $('.feature-carousel-no-gutter').owlCarousel({
            nav: false,
            center: false,
            autoplay: true,
            autoplayTimeout: 4000,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: true,
            touchDrag: true,
            responsive:{
                0:{
                    items:1
                },
                992:{
                    items:2
                },
                1200:{
                    items:3
                }
            }
        });
        $('.images-carousel').owlCarousel({
            nav: true,
            center: true,
            autoplay: true,
            autoplayTimeout:5000,
            autoplayHoverPause: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: true,
            margin: 15,
            touchDrag: true,
            responsive:{
                0:{
                    items:1,
                    nav: false
                },
                992:{
                    items:2
                },
                1200:{
                    items:3
                }
            }
        });
        $('.images-carousel-no-gutter').owlCarousel({
            nav: true,
            center: true,
            autoplay: true,
            autoplayTimeout:5000,
            autoplayHoverPause: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: true,
            touchDrag: true,
            responsive:{
                0:{
                    items:1,
                    nav: false
                },
                992:{
                    items:2
                },
                1200:{
                    items:3
                }
            }
        });
        $('.team-carousel').owlCarousel({
            nav: true,
            center: false,
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: false,
            touchDrag: true,
            responsive:{
                0:{
                    items:1,
                    margin:10,
                    nav: false
                },
                762: {
                    items:2,
                    margin:10
                },
                992:{
                    items:3,
                    margin:15
                }
            }
        });
        $('.about-carousel').owlCarousel({
            nav: true,
            center: true,
            autoplay: true,
            autoplayTimeout: 3000,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots: true,
            touchDrag: true,
            responsive:{
                0:{
                    items:1,
                    margin:10,
                    nav: false
                },
                992:{
                    items:2,
                    margin:15
                },
                1200:{
                    items:3,
                    margin:15
                }
            }
        });
        $('.testimonials-carousel.style1').owlCarousel({
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            nav: false,
            loop:true,
            checkVisibility: true,
            dots:true,
            items: 1,
            touchDrag: true,
            margin: 10
        });
        $('.testimonials-carousel.style2').owlCarousel({
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            nav: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            touchDrag: true,
            checkVisibility: true,
            dots:true,
            items: 1,
            margin: 10,
            center: true
        });
        $('.testimonials-carousel.style4').owlCarousel({
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            nav: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            checkVisibility: true,
            dots:true,
            touchDrag: true,
            items: 3,
            margin: 10,
            center: true,
            responsive:{
                0:{
                    items:1,
                    margin:10,
                    nav: false
            },
                992:{
                    items:2,
                    margin:15
                },
                1200:{
                    items:3,
                    margin:15
                }
            }
        });
        $('.popular-products-carousel').owlCarousel({
            nav: false,
            navContainer: '.owl-carousel-custom-nav',
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            loop: true,
            checkVisibility: true,
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            touchDrag: true,
            items: 4,
            margin: 10,
            dots:true,
            responsive:{
                0:{
                    items:1,
                    margin:10,
                    nav: false
                },
                480:{
                    items:2,
                    margin:10
                },
                768:{
                    items:3,
                    margin:10
                },
                1170:{
                    items:4,
                    margin:10
                }
            }
        });
        $('.product-gallery-carousel').owlCarousel({
            nav: false,
            dots:false,
            loop:true,
            checkVisibility: true,
            autoplay: true,
            autoplayTimeout: 5000,
            autoplayHoverPause: true,
            touchDrag: true,
            items: 1,
            margin: 10,
            center: true,
            thumbs: true,
            // When only using images in your slide (like the demo) use this option to dynamicly create thumbnails without using the attribute data-thumb.
            thumbImage: true,
            // Enable this if you have pre-rendered thumbnails in your html instead of letting this plugin generate them. This is recommended as it will prevent FOUC
            thumbsPrerendered: false,
            // Class that will be used on the thumbnail container
            thumbContainerClass: 'owl-thumbs',
            // Class that will be used on the thumbnail item's
            thumbItemClass: 'owl-thumb-item'
        });
        $('.partner-carousel').owlCarousel({
            nav: false,
            center: true,
            autoplay: true,
            autoplayTimeout: 3000,
            loop:true,
            checkVisibility: true,
            touchDrag: true,
            dots: false,
            margin:50,
            responsive:{
                0:{
                    items:2
                },
                992:{
                    items:4
                },
                1200:{
                    items:6
                }
            }
        });
        $('.banner-carousel').owlCarousel({
            autoplay: true,
            autoplayTimeout: 5000,
            nav: false,
            loop:true,
            checkVisibility: true,
            touchDrag: true,
            dots: false,
            items:1
        });
        $(".portfolio-carousel").owlCarousel({
            autoplay: true,
            autoplayTimeout: 3000,
            nav: true,
            navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
            loop:true,
            touchDrag: true,
            checkVisibility: true,
            dots: false,
            margin:10,
            responsive:{
                0:{
                    items:1
                },
                762: {
                    items:2
                },
                992:{
                    items:3
                },
                1200:{
                    items:4
                }
            }

        });
    }
    
});






