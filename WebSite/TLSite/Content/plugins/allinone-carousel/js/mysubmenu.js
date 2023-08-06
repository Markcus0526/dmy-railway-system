$(document).ready(function () {
    var animComplete=true;
	$('#nav li a').mouseover(function () {
		var currentId = $(this).parent().attr('id');
		if ( String(currentId)!="") {
			currentId = '#'+currentId+' .sub_menu';
			$(currentId).slideDown(300, 'linear');
		}		
    });
    $('#nav li').mouseleave(function () {
		var currentId = $(this).attr('id');
		if ( String(currentId)!="") {
			currentId = '#'+currentId+' .sub_menu';
			$(currentId).slideUp(300, 'swing');
		}
		
    });	
});