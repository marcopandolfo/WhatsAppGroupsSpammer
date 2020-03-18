using System;

namespace Bot.WhatsApp.Selenium
{
    public static class Utils
    {
        public static void LogImportant(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[>>>>] {message}");
            Console.ResetColor();
        }
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[INFO] {message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[WARN] {message}");
            Console.ResetColor();
        }

        public static void LogNumberOfMessagesSent(int success, int failed)
        {
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Mensagens enviadas (grupos): {success} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Convites Invalidos: {failed}");
            Console.ResetColor();
        }

        public static string GetMessageToSend()
        {
            return @"
                Cansado de usar sites de torrents cheio de anúncios? Visite nosso site!
                Site rápido, moderno e sem anúncios excessivos!
                Últimos lançamentos de 2020!
                Acesse: https://speckoz.live";
        }
    }
}
