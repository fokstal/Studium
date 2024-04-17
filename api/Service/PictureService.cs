namespace api.Service
{
    public static class PictureService
    {
        public static readonly string picturesFolderPath = "./AppData/Pictures/";

        public static async Task<string> UploadPassportScanAsync(IFormFile passportScan)
        {
            if (passportScan is null) throw new Exception("Passport.Scan is null!");

            string passportScanFileName = await UploadPictureToFolder(passportScan, "Passport");

            return passportScanFileName;
        }

        public static async Task<string> UploadPersonAvatarAsync(IFormFile? personAvatar, int personSex = 0)
        {
            if (personAvatar is null)
            {
                Random random = new();

                string defaultAvatarName = PersonService.SexStringByInt(personSex) + "-" + random.Next(1, 9) + ".png";

                personAvatar = GetPictureByFolderAndFileName("Person/Default", defaultAvatarName);
            }

            string personAvatarFileName = await UploadPictureToFolder(personAvatar, "Person");

            return personAvatarFileName;
        }

        public static async Task<string> UploadPictureToFolder(IFormFile picture, string folderName)
        {
            string pictureGuidName = Guid.NewGuid().ToString();
            string pictureExtension = Path.GetExtension(picture.FileName);

            using (FileStream fileStream = new(Path.Combine(picturesFolderPath + folderName + "/", pictureGuidName + pictureExtension), FileMode.Create))
            {
                await picture.CopyToAsync(fileStream);
            }

            string passportScanFileName = pictureGuidName + pictureExtension;

            return passportScanFileName;
        }

        public static IFormFile GetPictureByFolderAndFileName(string folderName, string fileName)
        {
            string pathToFileName = Path.Combine(picturesFolderPath + folderName + "/", fileName);

            FileStream fileStream = File.OpenRead(pathToFileName);

            IFormFile picture = new FormFile(fileStream, 0, fileStream.Length, null!, pathToFileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            return picture;
        }
    }
}