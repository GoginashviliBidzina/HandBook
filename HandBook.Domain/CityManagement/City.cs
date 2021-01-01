using System;
using HandBook.Shared;

namespace HandBook.Domain.CityManagement
{
    public class City : AggregateRoot
    {
        public string Name { get; private set; }

        public City(string name)
        {
            Name = name;
            CreateDate = DateTimeOffset.Now;
        }
    }
}
