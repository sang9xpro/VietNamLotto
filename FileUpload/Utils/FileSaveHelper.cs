using BusinessDatabase.CommonObj;
using BusinessDatabase.DataObj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Utilities;

namespace FileUpload.Utils
{
    public class FileSaveHelper
    {

        private static void SaveFileAsPath(string path, List<FileAndMimeType> listFileNew)
        {
   
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                new FileInfo(HttpContext.Current.Server.MapPath(path)).Directory.Create();
            }

            DirectoryInfo fileInfor = new FileInfo(HttpContext.Current.Server.MapPath(path)).Directory;

            var count = fileInfor.GetFiles().Length;

            foreach (var file in listFileNew)
            {
                count = count + 1;
                var fileType = "." + StringUtils.splitStringByChar(file.fileName, '.', false);
                var fileName = StringUtils.splitStringByChar(file.fileName, '-', true) + "_" + StringUtils.pad(true, count.ToString(), 2, '0') + fileType; 
                var filePath = HttpContext.Current.Server.MapPath(path + fileName.Replace("\"", String.Empty));
                File.WriteAllBytes(filePath, file.file);
            }
        }

        public static string SaveFileToLocal(List<FileAndMimeType> listFileNew, CustomerDto data)
        {
            var folderCustomer = data.Name.ToString().Replace(" ", String.Empty).RemoveCharacterUnicode().Trim() + data.IdNumber.ToString().Trim();

            var folderStaff = data.StaffId.ToString().Trim();

            var httpRequest = HttpContext.Current.Request;
            // Get files and save to server
            if (listFileNew.Count > 0)
            {
                FileSaveHelper.SaveFileAsPath("~/Uploaded/" + folderStaff + "/" + folderCustomer + "/", listFileNew);

                //Save  file to backup folder as Maketing Team request
                var customerPath = data.IdNumber.ToString().Replace(" ", String.Empty).RemoveCharacterUnicode().Trim() + "_" + data.CIF.ToString().Trim();
                DateTime dateTime = DateTime.UtcNow.Date;
                string datePath = dateTime.ToString("yyyyMMdd");
                FileSaveHelper.SaveFileAsPath("~/Backup/" + datePath + "/" + folderStaff + "/" + customerPath + "/", listFileNew);
                return  "/Uploaded/" + folderStaff + "/" + folderCustomer + "/"; 
            }
            else
            {
                return null;
            }
        }
    }
}