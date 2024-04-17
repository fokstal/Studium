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