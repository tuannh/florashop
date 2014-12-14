
;(function($) {
	// DOM ready
	$(function() {
		
		// Append the mobile icon nav
		$('.nav').append($('<div class="nav-mobile"></div>'));
		$('.nav').append($('<span class="name">Menu</span>'));
		
		
		
		// Add a <span> to every .nav-item that has a <ul> inside
		$('.nav-item').has('ul').prepend('<span class="nav-click"><i class="nav-arrow"></i></span>');
		$('.nav-submenu-item').has('ul.nav-submenu2').prepend('<span class="nav-click"><i class="nav-arrow"></i></span>');
		
		// Click to reveal the nav
		$('.nav-mobile').click(function(){
			$('.nav-list').slideToggle();
		});
		$('.name').click(function(){
			$('.nav-list').slideToggle();
		});
		
		
		// Dynamic binding to on 'click'
		$('.nav-list').on('click', '.nav-click', function(){
		
			// Toggle the nested nav
			$(this).siblings('.nav-submenu').slideToggle();
			$(this).siblings('.nav-submenu2').slideToggle();
			
			// Toggle the arrow using CSS3 transforms
			$(this).children('.nav-arrow').toggleClass('nav-rotate');
			
		});
	});
	
})(jQuery);


//Category	
$(document).ready(function(){
jQuery("#menu-icon").on("click", function(){
  jQuery(".sf-menu-phone").slideToggle(function() {
		  if(jQuery(".sf-menu-phone").is(":hidden")) {
  			jQuery("#menu-icon").css('background','#9f1500 url(userfiles/image/graphics/iconClickSale.png) no-repeat 97% 12px')	
  
		  }else{
			  	jQuery("#menu-icon").css('background','#9f1500 url(userfiles/image/graphics/iconClickSale-in.png) no-repeat 97% 12px')	
		}
 });
});

  jQuery('.sf-menu-phone').find('li.parent').append('<i class="icon-angle-down"></i>');
  jQuery('.sf-menu-phone li.parent i').on("click", function(){
   if (jQuery(this).hasClass('icon-angle-up')) { jQuery(this).removeClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle(); } 
    else {
     jQuery(this).addClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle();
    }
  });


 });