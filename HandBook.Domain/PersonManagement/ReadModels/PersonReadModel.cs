using System;

namespace HandBook.Domain.PersonManagement.ReadModels
{
    public class PersonReadModel
    {
        public int Id { get; private set; }

        public int AggregateRootId { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string IdentificationNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public int CityId { get; private set; }

        public string FilePath { get; private set; }

        public int PhotoHeight { get; private set; }

        public int PhotoWidth { get; private set; }

        public string Gender { get; private set; }

        public string RelatedPersonJson { get; private set; }

        public string HomePhoneNumber { get; private set; }

        public string OfficePhoneNumber { get; private set; }

        public string MobilePhoneNumber { get; private set; }

        public PersonReadModel(int aggregateRootId,
                               string firstName,
                               string lastName,
                               string identificationNumber,
                               DateTime birthDate,
                               int cityId,
                               string filePath,
                               int photoHeight,
                               int photoWidth,
                               string gender,
                               string relatedPersonJson,
                               string homePhoneNumber,
                               string officePhoneNumber,
                               string mobilePhoneNumber)
        {
            AggregateRootId = aggregateRootId;
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            BirthDate = birthDate;
            CityId = cityId;
            FilePath = filePath;
            PhotoHeight = photoHeight;
            PhotoWidth = photoWidth;
            Gender = gender;
            RelatedPersonJson = relatedPersonJson;
            HomePhoneNumber = homePhoneNumber;
            OfficePhoneNumber = officePhoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
        }

        public void ChangeDetails(int aggregateRootId,
            string firstName,
            string lastName,
            string identificationNumber,
            DateTime birthDate,
            int cityId,
            string filePath,
            int photoHeight,
            int photoWidth,
            string gender,
            string relatedPersonJson,
            string homePhoneNumber,
            string officePhoneNumber,
            string mobilePhoneNumber)
        {
            AggregateRootId = aggregateRootId;
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            BirthDate = birthDate;
            CityId = cityId;
            FilePath = filePath;
            PhotoHeight = photoHeight;
            PhotoWidth = photoWidth;
            Gender = gender;
            RelatedPersonJson = relatedPersonJson;
            HomePhoneNumber = homePhoneNumber;
            OfficePhoneNumber = officePhoneNumber;
            MobilePhoneNumber = mobilePhoneNumber;
        }

        public void ChangeRelatedPersonJson(string relatedPersonJson)
        {
            RelatedPersonJson = relatedPersonJson;
        }
    }
}
