using api.Helpers;
using api.Helpers.Constants;
using api.Services;

namespace api.Repositories
{
    public static class PictureRepository
    {
        private static readonly string picturesFolderPath = "./wwwroot/pictures/";
        private static readonly string defaultPersonPicturesFolderPath = "./AppData/Pictures/PersonDefault";

        private static IFormFile GetPicture(string folderName, string fileName)
        {
            string pathToFileName = Path.Combine($"{folderName}/{fileName}");

            FileStream fileStream = File.OpenRead(pathToFileName);

            IFormFile picture = new FormFile(fileStream, 0, fileStream.Length, null!, pathToFileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            return picture;
        }

        public static async Task<IFormFile> GetAndDecryptPictureAsync
            (PictureFolders.PictureFolderEntity pictureFolder, string fileName, string encryptionKey)
        {
            string pathToFileName = Path.Combine($"{picturesFolderPath}/{pictureFolder.Path}/{fileName}");

            byte[] encryptedPictureBytes = await File.ReadAllBytesAsync(pathToFileName);
            byte[] decryptedPictureBytes = AesWorker.DecryptPicture(encryptedPictureBytes, encryptionKey);

            MemoryStream decryptedPictureStream = new(decryptedPictureBytes);
            IFormFile decryptedPicture = new FormFile(decryptedPictureStream, 0, decryptedPictureStream.Length, null!, pathToFileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            return decryptedPicture;
        }

        private static async Task<string> UploadPicture
            (PictureFolders.PictureFolderEntity pictureFolder, IFormFile picture)
        {
            string pictureGuidName = Guid.NewGuid().ToString();
            string pictureExtension = Path.GetExtension(picture.FileName);

            using (FileStream fileStream = new(Path.Combine($"{picturesFolderPath}/{pictureFolder.Path}/{pictureGuidName + pictureExtension}"), FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }

            string passportScanFileName = pictureGuidName + pictureExtension;

            return passportScanFileName;
        }

        private static async Task<string> UploadAndEncryptPicture
            (PictureFolders.PictureFolderEntity pictureFolder, IFormFile picture, string encryptionKey)
        {
            string pictureGuidName = Guid.NewGuid().ToString();
            string pictureExtension = Path.GetExtension(picture.FileName);

            using (FileStream fileStream = new(Path.Combine($"{picturesFolderPath}/{pictureFolder.Path}/{pictureGuidName + pictureExtension}"), FileMode.Create))
            {
                byte[] encryptedPictureBytes = AesWorker.EncryptPicture(await picture.ToByteArrayAsync(), encryptionKey);
                await fileStream.WriteAsync(encryptedPictureBytes);
            }

            string passportScanFileName = pictureGuidName + pictureExtension;

            return passportScanFileName;
        }

        public static async Task<string> UploadPassportScanAsync(IFormFile passportScan, string encryptionKey)
        {
            if (passportScan is null) throw new Exception("Passport.Scan is null!");

            string passportScanFileName = await UploadAndEncryptPicture(PictureFolders.Passport, passportScan, encryptionKey);

            return passportScanFileName;
        }

        public static async Task<string> UploadPersonAvatarAsync(IFormFile? personAvatar, int personSex = 0)
        {
            if (personAvatar is null)
            {
                Random random = new();
                int randomMaxValue = 9;

                if (personSex == 1) randomMaxValue = 8;

                string defaultAvatarName = PersonHelper.SexStringByInt(personSex) + "-" + random.Next(1, randomMaxValue) + ".png";

                personAvatar = GetPicture(defaultPersonPicturesFolderPath, defaultAvatarName);
            }

            string personAvatarFileName = await UploadPicture(PictureFolders.Person, personAvatar);

            return personAvatarFileName;
        }

        public static void RemovePicture(PictureFolders.PictureFolderEntity pictureFolder, string pictureName)
        {
            File.Delete($"{picturesFolderPath}/{pictureFolder.Path}/{pictureName}");
        }

        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}