namespace ClinicManagement.Blazor.Utilities
{
  public static class Utility
  {
    public static string APIBase;
    public static string PatientPictureFolderToUpload;
    public static string PatientPictureFolderToGet;

    public static string PatientPictureUrl(string fileName) => APIBase + PatientPictureFolderToGet + fileName;
  }
}
