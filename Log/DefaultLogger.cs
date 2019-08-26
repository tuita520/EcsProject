using System;

namespace RDLog
{
    public class DefaultLogger:ILog
    {
        public void Trace(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Fatal(string message)
        {
            Console.WriteLine(message);
        }

        public void Trace(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Fatal(string message, params object[] args)
        {
            Console.WriteLine(message);
        }
    }
}