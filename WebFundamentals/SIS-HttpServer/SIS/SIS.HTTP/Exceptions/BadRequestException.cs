namespace SIS.HTTP.Exceptions
{
    using System;

    public class BadRequestException : Exception
    {
        private const string InvalidRequestMessage = "The Request was malformed or contains unsupported elements.";

        public static object ThrowFromInvalidRequest()
            => throw new BadRequestException(InvalidRequestMessage);

        public BadRequestException(string message)
            : base(message) { }
    }
}
