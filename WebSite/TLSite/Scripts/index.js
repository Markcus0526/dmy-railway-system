
$(document).ready(function () {

    var oTable = $('#myDataTable').dataTable({
    	"bServerSide": true,
    	"sAjaxSource": "AjaxHandler", 
    	"bProcessing": true,
    });
});
