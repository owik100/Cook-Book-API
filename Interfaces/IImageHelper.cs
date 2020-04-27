namespace Cook_Book_API.Interfaces
{
    public interface IImageHelper
    {
        string GetImagePath(string imageName);
        bool CheckCorrectExtension(string extension);
    }
}