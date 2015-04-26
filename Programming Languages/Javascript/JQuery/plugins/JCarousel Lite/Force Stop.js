var endSlide;
var firstRotation = true;
function select_carousel_control(){
	var l = parseInt($('#carousel_ul').css("left"));
	var w = parseInt($('#carousel_ul').children().css("width"));
	var idx = parseInt(((-1*l/w)  ) % ($('.jcarousel-control').children().length));

	$('.jcarousel-control').each(function() {
		var c = $(this).children();
		c.removeClass('selected');
	});
	$('.jcarousel-control').children(':eq(' + idx + ')').addClass('selected');

	if($(".mycarousel ul").css("left") == (endSlide + "px") && firstRotation){
		//only jump to the fist slide the first time we go through
		firstRotation = false;
		//Move the carousel to the first item
		setTimeout(function() {$(".jcarousel-control .1").trigger('click');}, 10000);
		//Stop the carousel from auto moving
		setTimeout(function() {	
			var x = setTimeout("");
			for (var i = 0 ; i < x ; i++)
			clearTimeout(i); 
		}, 12000);  
	}
}
	
$(function() {   
	$(".mycarousel").jCarouselLite({         
		btnNext: ".c-next",          
		btnPrev: ".c-prev",
		speed: 800,
		auto: 10000,
		visible: 1,
		circular: false,
		afterEnd: select_carousel_control,
		hoverPause: true,
		btnGo:
			[".jcarousel-control .1", ".jcarousel-control .2",
			".jcarousel-control .3", ".jcarousel-control .4",
			".jcarousel-control .5", ".jcarousel-control .6",
			".jcarousel-control .7", ".jcarousel-control .8",
			".jcarousel-control .9", ".jcarousel-control .10",
			".jcarousel-control .11", ".jcarousel-control .12"]
	}); 	
	endSlide = -($(".mycarousel ul").width() - $(".mycarousel ul li").width());
	select_carousel_control();  
}