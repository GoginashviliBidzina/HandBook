using System.Linq;
using Newtonsoft.Json;
using HandBook.Domain.PersonManagement;
using HandBook.Infrastructure.DataBase;
using HandBook.Domain.PersonManagement.Events;
using HandBook.Infrastructure.EventDispatching;
using HandBook.Domain.PersonManagement.ReadModels;

namespace HandBook.Application.EventHandlers.Person
{
    public class PersonEventHandler : IHandleEvent<PersonPlacedEvent>,
                                      IHandleEvent<PersonUpdatedEvent>,
                                      IHandleEvent<PersonDeletedEvent>,
                                      IHandleEvent<RelatedPersonAddedEvent>
    {
        public void Handle(PersonPlacedEvent @event, DatabaseContext db)
        {
            var aggregate = @event.Person;

            var homePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Home);
            var officePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Office);
            var mobilePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Mobile);

            var personReadModel = new PersonReadModel(aggregate.Id,
                                                      aggregate.FirstName,
                                                      aggregate.LastName,
                                                      aggregate.IdentificationNumber,
                                                      aggregate.BirthDate,
                                                      aggregate.CityId,
                                                      aggregate.Photo.FilePath,
                                                      aggregate.Photo.Height,
                                                      aggregate.Photo.Width,
                                                      aggregate.Gender.ToString(),
                                                      "",
                                                      homePhoneNumber?.Number,
                                                      officePhoneNumber?.Number,
                                                      mobilePhoneNumber?.Number);

            db.Set<PersonReadModel>().Add(personReadModel);
        }

        public void Handle(PersonUpdatedEvent @event, DatabaseContext db)
        {
            var aggregate = @event.Person;
            var personReadModel = db.Set<PersonReadModel>()
                .FirstOrDefault(readModel => readModel.AggregateRootId == aggregate.Id);

            if (personReadModel != null)
            {
                var homePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Home);
                var officePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Office);
                var mobilePhoneNumber = aggregate.PhoneNumbers?.FirstOrDefault(phone => phone.PhoneNumberType == PhoneNumberType.Mobile);

                var relatedPersons = aggregate.RelatedPersons == null ? string.Empty :
                                                                              JsonConvert.SerializeObject(aggregate.RelatedPersons);

                personReadModel.ChangeDetails(aggregate.Id,
                                              aggregate.FirstName,
                                              aggregate.LastName,
                                              aggregate.IdentificationNumber,
                                              aggregate.BirthDate,
                                              aggregate.CityId,
                                              aggregate.Photo.FilePath,
                                              aggregate.Photo.Height,
                                              aggregate.Photo.Width,
                                              aggregate.Gender.ToString(),
                                              relatedPersons,
                                              homePhoneNumber?.Number,
                                              officePhoneNumber?.Number,
                                              mobilePhoneNumber?.Number);

                db.Set<PersonReadModel>().Update(personReadModel);
            }
        }

        public void Handle(PersonDeletedEvent @event, DatabaseContext db)
        {
            var personReadModel = db.Set<PersonReadModel>()
                                    .FirstOrDefault(readModel => readModel.AggregateRootId == @event.Id);

            if (personReadModel != null)
                db.Set<PersonReadModel>().Remove(personReadModel);
        }

        public void Handle(RelatedPersonAddedEvent @event, DatabaseContext db)
        {
            var aggregate = @event.Person;
            var personReadModel = db.Set<PersonReadModel>()
                .FirstOrDefault(readModel => readModel.AggregateRootId == aggregate.Id);

            if (personReadModel != null)
            {
                var relatedPersonsJson = aggregate.RelatedPersons == null ? string.Empty : JsonConvert.SerializeObject(aggregate.RelatedPersons);

                personReadModel.ChangeRelatedPersonJson(relatedPersonsJson);

                db.Set<PersonReadModel>().Update(personReadModel);
            }
        }
    }
}
