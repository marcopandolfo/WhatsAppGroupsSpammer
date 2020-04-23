using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.WhatsApp.Selenium.Exceptions
{
    class InvalidAnswerException : Exception
    {
        private static readonly string msg = "Opção inválida! Certifique-se de ter inserido uma opção válida";

        public InvalidAnswerException() : base(msg)  {}
    }
}
