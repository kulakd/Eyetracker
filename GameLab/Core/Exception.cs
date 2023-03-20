using System;

namespace GameLab
{
    public class GameLabException : Exception
    {
        public GameLabException(string message)
            :base(message)
        {
        }

        public GameLabException(string message, Exception innerExteption)
            :base(message,innerExteption)
        {
        }
    }
}
