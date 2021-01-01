using System;
using HandBook.Shared;

namespace HandBook.Domain.PhotoManagement
{
    public class Photo : AggregateRoot
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public string FilePath { get; private set; }

        public Photo(int width,
                     int height,
                     string filePath)
        {
            Width = width;
            Height = height;
            FilePath = filePath;
            CreateDate = DateTimeOffset.Now;
        }
    }
}
