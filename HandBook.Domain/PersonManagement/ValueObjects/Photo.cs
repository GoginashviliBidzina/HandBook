using HandBook.Shared;

namespace HandBook.Domain.PersonManagement.ValueObjects
{
    public class Photo : ValueObject
    {
        public string FilePath { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Photo(string filePath,
                     int width,
                     int height)
        {
            FilePath = filePath;
            Width = width;
            Height = height;
        }
    }
}
