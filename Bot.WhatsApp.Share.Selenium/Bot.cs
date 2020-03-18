using OpenQA.Selenium;
using prmToolkit.Selenium;
using prmToolkit.Selenium.Enum;
using System;
using System.Threading;
using static Bot.WhatsApp.Selenium.Utils;

namespace Bot.WhatsApp.Selenium
{
    public class Bot
    {
        static readonly IWebDriver webDriver = WebDriverFactory.CreateWebDriver(Browser.Chrome, @"C:\Users\marco\Desktop\WppGroupsSpammer\Driver", false);
        static void Main(string[] args)
        {
            LogInfo("Abrindo o Whatsapp Web!");
            LogInfo("Pegue seu celular e leia o QRCODE.");
            webDriver.LoadPage(TimeSpan.FromSeconds(15), "https://web.whatsapp.com");
            Thread.Sleep(TimeSpan.FromSeconds(15));

            string[] links = { };

            foreach (string link in links)
            {
                string message = GetMessageToSend();
                try
                {
                    SendMessageToGroup(link);
                }
                catch (Exception)
                {
                    LogWarning("Convite Inválido, seguindo para o proximo...");
                    continue;
                }
                finally
                {
                    webDriver.Close();
                    webDriver.Dispose();
                }
            }
        }

        private static void SendMessageToGroup(string invite)
        {
            LogInfo("Carregando a página de convite.");
            webDriver.LoadPage(TimeSpan.FromSeconds(25), invite);

            LogInfo("Clicando no botão para entrar na conversa");
            var btnJoin = webDriver.FindElement(By.Id("action-button"));
            btnJoin.Click();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            Console.WriteLine("Clicando no botão para usar o whatsapp web");
            var btnWeb = webDriver.FindElement(By.XPath("//*[@id='fallback_block']/div/div/a"));
            btnWeb.Click();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            Console.WriteLine("Clicando no botão de entrar no grupo");
            var btnJoinGroup = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            btnJoinGroup.Click();
            Thread.Sleep(TimeSpan.FromSeconds(15));

            LogInfo("Escrevendo a mensagem");
            var inputMsg = By.XPath("//*[@id='main']/footer/div[1]/div[2]/div/div[2]");
            webDriver.SetText(inputMsg, GetMessageToSend());

            LogInfo("Enviando a mensagem");
            webDriver.FindElement(inputMsg).SendKeys(Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(20));

            LogInfo("Abrindo o menu para sair do grupo.");
            var opcoesDoMenu = webDriver.FindElement(By.XPath("//*[@id='main']/header"));
            opcoesDoMenu.Click();

            LogInfo("Aguardando para o botão sair aparacer.");
            Thread.Sleep(TimeSpan.FromSeconds(7));

            LogInfo("Clicando pra sair do grupo.");
            var btnSairGrupo = webDriver.FindElement(By.XPath("//*[@id='app']/div/div/div[2]/div[3]/span/div/span/div/div/div[1]/div[6]/div/div[2]/div/span"));
            btnSairGrupo.Click();
            Thread.Sleep(TimeSpan.FromSeconds(5));

            Console.WriteLine("Confirmando saida");
            var confirmExit = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            confirmExit.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            Console.WriteLine("Operação realizada com sucesso!");
        }
    }
}
