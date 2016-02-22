

$(document).ajaxStart(function () { $(".row").addClass("loading") });
$(document).ajaxStop(function () { $(".row").removeClass("loading") });


(function () {

    $.get('/Ajax/getUserList')
    .done(function (data) {
        $.each(data, function (item,value) {
            
            value.folder = ko.observable(false);
        });
        UserModel.items(data);      
    })
})();
ko.bindingHandlers.toggleClick = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();

        ko.utils.registerEventHandler(element, "click", function () {
            value(!value());
        });
    }
};


var UserViewModel = function ()
{   var self=this
    self.activeGridUser = ko.observable();
    self.currentUser = ko.observable(currentUser);

    self.items = ko.observableArray([]);
    self.filelist = ko.observableArray([]);
    self.storage = ko.observable(false);
    self.storageFunction = function () {
        self.storage(!self.storage()); $('.storage').toggleClass('fa-folder fa-folder-open');
       
  
    };
    self.getUserFiles = function (type, username) {
        self.activeGridUser(username);
    
        $.get('/Ajax/getFileList', { username: username,type:type })
        .done(function (data) {
            self.filelist(data);
           
        })
    };    
    self.gridViewModel = new ko.simpleGrid.viewModel({
        data: this.filelist,
        columns: [
            {
                headerText: "", rowText: function (data) {
                    if (data.type == 'image')
                        return "<img src='" + data.url + "'height='50' width:'50'  />"
                    else
                        
                        return "<img src='https://lh3.ggpht.com/Yotn7-8_rvZ8FcNowaAH08OnCT7c083hATU7Y69pMIOCWJMNstsIQUWXH9KGqUN0_NF0=w170'height='50' width:'50'  />"
                }},
            { headerText: "File Name", rowText: "name" },
            
            {
                headerText: "Created At", rowText: function (data) {
                    if (data.createdAt) {
                        var time = new Date(data.createdAt.match(/\d+/)[0] * 1);
                        return time.toLocaleString();
                    }
                    else
                        return ('No data info')
                }
            },
            {
                headerText: "Action", rowText: function (data) {
                    if(data.type=='image')
                        return "<a data-url='" + data.url + "' data-toggle='modal' href='#ShowImage'>Show</a>"
                    else
                        return "<a data-url='" + data.url + "'data-name='" + data.name + "' data-toggle='modal' href='#DownloadFile'>Download</a>"

}
            },
            
        ],
        pageSize: 4
    });
    self.deleteElement = function () {
     
        if ($('.activeRow td:nth-child(2)').text())
            $.post('/Ajax/DeleteFile', { filename: $('.activeRow td:nth-child(2)').text() })
        .done(function (data) {
            self.getUserFiles(data, currentUser);
            self.chooseToDelete(false);
        })
       
    }
    self.chooseToDelete = ko.observable($(document).hasClass('activeRow'))
    self.getDeleteText = ko.computed(function () {
      
        if (self.chooseToDelete()) {
           
           
            return "Are you sure you want to delete file   " + $('.activeRow').children(":first").text() + " from your storage "
        }
        else {
           
            return "Please select file to delete by clicking on them"
        }
    },self)
      
}
UserModel = new UserViewModel()

ko.applyBindings(UserModel);




