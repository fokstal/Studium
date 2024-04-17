namespace api.Service
{
    public static class PictureService
    {
        public static readonly string picturesFolderPath = "./AppData/Pictures/";

        public static async Task<string> UploadPassportScanAsync(IFormFile passportScan)
        {
            if (passportScan is null) throw new Exception("Passport.Scan is null!");

            string pictureGuidName = Guid.NewGuid().ToString();
            string pictureExtension = Path.GetExtension(passportScan.FileName);

            using (FileStream fileStream = new(Path.Combine(picturesFolderPath + "Passport/", pictureGuidName + pictureExtension), FileMode.Create))
            {
                await passportScan.CopyToAsync(fileStream);
            }

            string passportScanFileName = pictureGuidName + pictureExtension;

            return passportScanFileName;
        }

        public static async Task<string> UploadPersonAvatarAsync(IFormFile? personAvatar, int personSex = 0)
        {
            if (personAvatar is null)
            {
                string sex;
                Random random = new();

                switch (personSex)
                {
                    case 0:
                        {
                            sex = "Woman";
                            break;
                        }
                    case 1:
                        {
                            sex = "Man";
                            break;
                        }
                    default:
                        {
                            throw new Exception("Sex is incorrect!");
                        }
                }

                string defaultAvatarName = sex + "-" + random.Next(1, 9) + ".png";

                personAvatar = GetPictureByFolderAndFileName("Person/" + "Default/", defaultAvatarName);
            }

            string pictureGuidName = Guid.NewGuid().ToString();
            string pictureExtension = Path.GetExtension(personAvatar.FileName);

            using (FileStream fileStream = new(Path.Combine(picturesFolderPath + "Person/", pictureGuidName + pictureExtension), FileMode.Create))
            {
                await personAvatar.CopyToAsync(fileStream);
            }

            string passportScanFileName = pictureGuidName + pictureExtension;

            return passportScanFileName;
        }

        public static IFormFile GetPictureByFolderAndFileName(string folder, string fileName)
        {
            string pathToFileName = Path.Combine(picturesFolderPath + folder, fileName);

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