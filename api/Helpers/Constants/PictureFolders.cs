namespace api.Helpers.Constants
{
    public static class PictureFolders
    {
        private static readonly PictureFolderEntity passport = new(0, "passport");
        private static readonly PictureFolderEntity person = new(1, "person");

        public static PictureFolderEntity Passport => passport;
        public static PictureFolderEntity Person => person;

        public record class PictureFolderEntity(int Id, string Path);
    }
}