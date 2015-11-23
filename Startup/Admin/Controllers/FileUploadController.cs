using BussinesAccess.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public async Task<ActionResult> Upload()
        {
            bool isSavedSuccessfully = false;

            string fName = "";

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here

                if (file != null && file.ContentLength > 0)
                {

                    var name = $"FeedImages/{AzureHelper.RandomFileName(5, file.FileName)}";
                    isSavedSuccessfully = await AzureHelper.UploadAsync(name, file.InputStream);

                    fName = AzureHelper.GetAzureImageFormat(name);


                }

            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }

        }

    }
}