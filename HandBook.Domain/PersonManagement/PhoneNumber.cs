using HandBook.Shared;

namespace HandBook.Domain.PersonManagement
{
    public class PhoneNumber : Entity
    {
        public string Number { get; private set; }

        public int PersonId { get; private set; }

        public virtual Person Person { get; private set; }

        public PhoneNumberType PhoneNumberType { get; private set; }

        public PhoneNumber(string number,
                           PhoneNumberType phoneNumberType)
        {
            Number = number;
            PhoneNumberType = phoneNumberType;
        }

        public void ChangeNumber(string number)
        {
            Number = number;
        }
    }
}
