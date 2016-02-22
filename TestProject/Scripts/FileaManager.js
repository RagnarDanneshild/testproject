
    
$('#file-input').on('change', function (e) {
    if (e.target.files.length > 0) {
        file = e.target.files[0];
        fileHandler(file)
    }
      
    });
var fileHandler = function (file) {
    var data = new FormData();
    
        data.append('image', file);
    
        $.ajax({
        type: "POST",
        url: "/Ajax/uploadFile",
        contentType: false,
        processData: false,
        data: data
        })
       .done(function (data) {
           if (file.type.match('image.*')) 
               UserModel.getUserFiles('image', currentUser)
           else UserModel.getUserFiles('other',currentUser)
              

       });
}

$(document).on('click', '.ko-grid tr',function () {
    UserModel.chooseToDelete(true);
    $('.activeRow').removeClass('activeRow');
    $(this).addClass('activeRow');
});

