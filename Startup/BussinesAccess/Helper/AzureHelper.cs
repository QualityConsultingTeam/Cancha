using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesAccess.Helper
{
    public static class AzureHelper
    {

        static AzureHelper()
        {
            _url = ConfigurationManager.AppSettings["AzureUri"];
            _user = ConfigurationManager.AppSettings["AzureUser"];
            _pass = ConfigurationManager.AppSettings["AzureKey"];
        }



        private static CloudBlobClient GetBlobClient()
        {
            try
            {
                // return AzureHelper.GetBlobClient();
                var credentials = new StorageCredentials(_user, _pass);
                //var storage = new StorageCredentialsAccountAndKey(_user, _pass);

                return new CloudBlobClient(new Uri(_url), credentials);

            }
            catch (Exception e)
            {
                throw new Exception("Error when connecting to Azure , please try again", e);
            }
        }

        public static bool ExistBlob(string packageFileName)
        {

            try
            {
                packageFileName = packageFileName.Replace("\\", "/");

                var blobClient = GetBlobClient();

                // var blob = blobClient.GetBlockBlobReference(packageFileName);

                var blob = blobClient.GetContainerReference(containerName)
                    .GetBlockBlobReference(packageFileName);


                blob.FetchAttributes();

                return blob.Properties.Length > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }


        private static CloudBlobContainer GetContainer(string containerName)
        {
            // containerName = Packages.DecorateName(containerName);
            return GetBlobClient().GetContainerReference(containerName);
        }

        private static List<CloudBlobDirectory> GetCloudBlobDirectories(string containerName, string path = "")
        {

            var container = GetContainer(containerName);

            if (!string.IsNullOrEmpty(path))
            {
                return container.GetDirectoryReference(path).ListBlobs()
                    .ToList().OfType<CloudBlobDirectory>().ToList();
            }

            return container.ListBlobs().ToList().OfType<CloudBlobDirectory>().ToList();
        }




        public static bool UploadImage(string sourceImageFilename)
        {

            try
            {

                var imageFilename = GetAzureImageFormat(sourceImageFilename);

                if (ExistBlob(imageFilename)) return true;

                var blobClient = GetBlobClient();

                var container = blobClient.GetContainerReference(containerName);

                container.CreateIfNotExists();

                var blob = blobClient.GetBlobReferenceFromServer(new Uri(imageFilename));

                var content = File.ReadAllBytes(sourceImageFilename);
                using (var stream = new MemoryStream(content))
                {
                    blob.UploadFromStream(stream);
                }

                return true;
            }
            catch (Exception ex)
            {
                //TODO log Exceptions.
                return false;
            }

        }

        public static async Task DeleteImage(string filepath)
        {


            if (!ExistBlob(filepath))
            {
                filepath = GetAzureImageFormat(filepath);
                if (!ExistBlob(filepath)) return;
            }

            var blobClient = GetBlobClient();

            var container = blobClient.GetContainerReference(containerName);

            var blob = blobClient.GetBlobReferenceFromServer(new Uri(filepath));

            await blob.DeleteAsync();


        }

        #region Async Upload


        public static Task<bool> UploadAsync(string path, Stream stream)
        {
            return Upload(path, stream);
        }

        public static async Task<bool> Upload(string path, Stream stream)
        {

            try
            {
                var imageFilename = GetAzureImageFormat(path);

                if (ExistBlob(imageFilename)) return false;

                var blobClient = GetBlobClient();

                var container = blobClient.GetContainerReference(containerName);

                container.CreateIfNotExists();

                var b = blobClient.GetContainerReference(containerName)
                    .GetBlockBlobReference(path.Replace("\\", "/").ToLower());

                // var blob = blobClient.GetBlobReferenceFromServer(new Uri(imageFilename));
                await b.UploadFromStreamAsync(stream);

                return true;
            }
            catch (Exception ex)
            {
                //TODO log Exceptions.
                return false;
            }

        }

        public static void Upload(string path, byte[] bytes)
        {
            try
            {

                var imageFilename = GetAzureImageFormat(path);

                if (ExistBlob(imageFilename)) return;

                var blobClient = GetBlobClient();

                var container = blobClient.GetContainerReference(containerName);

                container.CreateIfNotExists();

                var b = blobClient.GetContainerReference(containerName).GetBlockBlobReference(imageFilename);


                // var blob = blobClient.GetBlobReferenceFromServer(new Uri(imageFilename));


                using (var stream = new MemoryStream(bytes))
                {
                    b.UploadFromStream(stream);
                }


            }
            catch (Exception ex)
            {
                //TODO log Exceptions.

            }
        }
        #endregion


        public static string GetAzureImageFormat(string filename)
        {
            var file = string.Format("{0}{1}/{2}", _url, containerName, filename);

            return file.Replace("\\", "/").ToLower();
        }

        public static bool ExistsFile(string path)
        {
            var imageFilename = GetAzureImageFormat(path);

            return ExistBlob(imageFilename);
        }

        private static string FriendlyName(this string value)
        {
            return value.Replace("/", "").Replace("-", " ");
        }

        public static string PathCache
        {
            get
            {
                return string.Empty;
                var cache = ConfigurationManager.AppSettings["LocalSourceCache"];



                return string.IsNullOrEmpty(cache) ? cache : "";
            }
        }

        public static string RandomFileName(int length, string fileName)
        {
            var extension = Path.GetExtension(fileName);

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray()) + extension;
        }




        public static string containerName = ConfigurationManager.AppSettings["containerName"];
        //private const string containerName = "parrasStore";
        private static string _user = string.Empty;
        private static string _pass = string.Empty;
        private static string _url = string.Empty;

    }
}
