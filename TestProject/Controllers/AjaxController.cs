using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TestProject.Controllers
{     
    public class AjaxController : Controller
    {    
        private FileSystem db = new FileSystem();
        private CloudBlobContainer container = BlobStorage.getContainer();
        private string baseStorageUrl = "https://arhiptest.blob.core.windows.net/deyou";
        [HttpGet]
        public JsonResult getUserList()
        {
            var allUser = db.AspNetUsers.Select( u => new { username = u.UserName }).ToList();
           
            return Json( allUser , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getFileList(string username,string type)
        {
            var files = db.FileNotes.Where(p => p.AspNetUsers.UserName == username && p.Type == type).Select(u => new { name = u.Name, createdAt = u.CreatedAt,type = u.Type,  url = u.Url}).OrderByDescending(d => d.createdAt).ToList();
            return Json(files, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void uploadFile()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            { 
                var httpPostedFile = System.Web.HttpContext.Current.Request.Files["image"];
                if (httpPostedFile != null)
                {                    
                    var fileSavePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content"), httpPostedFile.FileName);
                    httpPostedFile.SaveAs(fileSavePath);
                    var blobPath = Path.Combine(User.Identity.GetUserName(), BlobStorage.IsImage(httpPostedFile), httpPostedFile.FileName);
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobPath);
                    using (var fileStream = System.IO.File.OpenRead(fileSavePath))
                    {
                        blockBlob.UploadFromStream(fileStream);
                    }

                    FileNote newFile = new FileNote();
                    newFile.UserId = User.Identity.GetUserId();
                    newFile.AspNetUsers = db.AspNetUsers.SingleOrDefault(e => e.Id == newFile.UserId);
                    newFile.Url = Path.Combine(baseStorageUrl,blobPath);
                    newFile.Name = httpPostedFile.FileName;
                    newFile.CreatedAt = System.DateTime.Now;
                    newFile.Type = BlobStorage.IsImage(httpPostedFile);
                    db.FileNotes.Add(newFile);
                    db.SaveChanges();

                    System.IO.File.Delete(fileSavePath);

                }
            }
        }
        [HttpPost]
        public string deleteFile(string filename)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                var currentUser = User.Identity.GetUserId();
                var deletingFile = db.FileNotes.First(k => k.Name == filename && k.UserId == currentUser);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(Path.Combine(deletingFile.AspNetUsers.UserName, deletingFile.Type, deletingFile.Name));
                if (blockBlob.Exists())
                {
                    blockBlob.Delete();
                    db.FileNotes.Remove(deletingFile);
                    db.SaveChanges();
                }
                return deletingFile.Type;
            }

            return "";
        }
    }
}
