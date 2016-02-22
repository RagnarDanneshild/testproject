$('#ShowImage').on('show.bs.modal', function (e) {

    //get data-id attribute of the clicked element
    var href = $(e.relatedTarget).data('url');
    
   
    $(e.currentTarget).find('#showImg').attr('src', href);
    $(e.currentTarget).find('a').attr('href', href);
});
$('#DownloadFile').on('show.bs.modal', function (e) {

    //get data-id attribute of the clicked element
    var href = $(e.relatedTarget).data('url');
    var name = $(e.relatedTarget).data('name');

    $(e.currentTarget).find('#text').text( " "+name);
    $(e.currentTarget).find('a').attr('href', href);
    $(e.currentTarget).find('#link').val( href);
});
