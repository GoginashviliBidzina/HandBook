﻿namespace HandBook.Shared
{
    public class ValueObject
    {
        public void ThrowDomainException(string message)
        {
            throw new DomainException(message);
        }
    }
}
