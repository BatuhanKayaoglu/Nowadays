using System.Runtime.Serialization;

namespace Nowadays.API.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public UserCreateFailedException() : base("Kullanıcı oluşturulurken beklenmeyen bir hata meydana geldi.")
        {
        }

        public UserCreateFailedException(string? message) : base(message)
        {
        }

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserCreateFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
