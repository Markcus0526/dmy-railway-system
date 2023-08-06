function changeProdIage (imgID,img_link) {
		var windowWidth=$(window).width();
		var totalWidthPreview=windowWidth-(200+310);
		//alert (totalWidthPreview);
		if (totalWidthPreview>590) {
			totalWidthPreview=590;
		}
	jQuery ("#"+imgID).html('<img src="'+img_link+'" width="'+totalWidthPreview+'">');
}


function openProd (imgID,img_link) {
		var windowHeight=$(window).height();
		jQuery('#products1').css({'height':(windowHeight-40)+'px'});
		jQuery('#products1').fadeToggle('slow', 'linear');
}