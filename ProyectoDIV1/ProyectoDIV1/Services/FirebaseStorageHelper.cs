using Firebase.Storage;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class FirebaseStorageHelper
    {
        private readonly FirebaseStorage firebaseStorage;
        private static readonly string rutaDeStorage = "proyectodiv-d53ed.appspot.com";
        public FirebaseStorageHelper()
        {
            firebaseStorage = new FirebaseStorage(rutaDeStorage);
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName, string carpeta)
        {
            try
            {
                var imageUrl = await firebaseStorage
               .Child(carpeta)
               .Child(fileName)
               .PutAsync(fileStream);
                return imageUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
           
        }
        public async Task<string> GetFile(string fileName, string carpeta)
        {
            try
            {
                return await firebaseStorage
               .Child(carpeta)
               .Child(fileName)
               .GetDownloadUrlAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }      
        }
        public async Task DeleteFile(string fileName, string carpeta)
        {
            try
            {

                await firebaseStorage
                 .Child(carpeta)
                 .Child(fileName)
                 .DeleteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
