using System;
using System.Linq;
using HandBook.Shared;
using System.Collections.Generic;
using HandBook.Domain.PersonManagement.Events;
using HandBook.Domain.PersonManagement.ValueObjects;

namespace HandBook.Domain.PersonManagement
{
    public class Person : AggregateRoot
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string IdentificationNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public int CityId { get; private set; }

        public virtual Photo Photo { get; private set; }

        public Gender Gender { get; private set; }

        public virtual ICollection<RelatedPerson> RelatedPersons { get; private set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; private set; }

        public Person()
        {

        }

        public Person(string firstName,
                      string lastName,
                      string identificationNumber,
                      DateTime birthDate,
                      int cityId,
                      Photo photo,
                      Gender gender,
                      ICollection<PhoneNumber> phoneNumbers)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            BirthDate = birthDate;
            CityId = cityId;
            Photo = photo;
            Gender = gender;
            PhoneNumbers = phoneNumbers;
            CreateDate = DateTimeOffset.Now;

            Raise(new PersonPlacedEvent(this));
        }

        public void ChangeDetails(string firstName,
                                 string lastName,
                                 string identificationNumber,
                                 DateTime birthDate,
                                 int cityId, Photo photo,
                                 Gender gender,
                                 IList<PhoneNumber> phoneNumbers)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            BirthDate = birthDate;
            CityId = cityId;
            Photo = photo;
            Gender = gender;
            PhoneNumbers = phoneNumbers;
            LastChangeDate = DateTimeOffset.Now;

            Raise(new PersonUpdatedEvent(this));
        }

        public void SetRelatedPerson(IList<RelatedPerson> relatedPersons)
        {
            if (relatedPersons?.Any() ?? true)
            {
                RelatedPersons = relatedPersons;

                Raise(new RelatedPersonAddedEvent(this));
            }
        }

        public void RaiseDeletedEvent()
            => Raise(new PersonDeletedEvent(Id));
    }
}
