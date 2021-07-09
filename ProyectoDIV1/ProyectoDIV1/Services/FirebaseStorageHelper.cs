using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage;
        private static string rutaDeStorage = "proyectodiv-d53ed.appspot.com";
        public FirebaseStorageHelper()
        {
            firebaseStorage = new FirebaseStorage(rutaDeStorage);
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName, string carpeta)
        {
            var imageUrl = await firebaseStorage
                .Child(carpeta)
                .Child(fileName)
                .PutAsync(fileStream);
            return imageUrl;
        }
        public async Task<string> GetFile(string fileName, string carpeta)
        {
            return await firebaseStorage
                .Child(carpeta)
                .Child(fileName)
                .GetDownloadUrlAsync();
        }
        public async Task DeleteFile(string fileName, string carpeta)
        {
            await firebaseStorage
                 .Child(carpeta)
                 .Child(fileName)
                 .DeleteAsync();

        }
    }
}
