using OpenQA.Selenium;
using prmToolkit.Selenium;
using prmToolkit.Selenium.Enum;
using System;
using System.Threading;

namespace Bot.WhatsApp.Selenium
{
    public class Bot
    {
        static IWebDriver webDriver = WebDriverFactory.CreateWebDriver(Browser.Chrome, @"C:\Users\marco\Desktop\WppGroupsSpammer\Driver", false);
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Pegue seu celular e leia o QRCODE.");

                Console.WriteLine("Abrindo o WhatsApp Web");
                webDriver.LoadPage(TimeSpan.FromSeconds(15), "https://web.whatsapp.com");
                Thread.Sleep(TimeSpan.FromSeconds(15));

                string[] convites =
                {
                    "https://chat.whatsapp.com/CMtLVDrcFHmHoClNqQl2ym",
                    "https://chat.whatsapp.com/GyC3KAQ3C5S16BjKyB6FBp",
                    "https://chat.whatsapp.com/ELeyOzNT3zz3zDAIjwFiWK",
                    "https://chat.whatsapp.com/BsCUDegnEDrKT1jplyhYZn",
                    "https://chat.whatsapp.com/FmPiqme4FebIUTmR7zhL32",
                    "https://chat.whatsapp.com/GgP8Y7r1VYq8hSuy0Em0m1",
                    "https://chat.whatsapp.com/FoYjkFQAVCc5SFnDuSyOrn",
                    "https://chat.whatsapp.com/JPAykDYzgm369A1BfDi6z2",
                    "https://chat.whatsapp.com/C4i0NFaX6jyEIsNKufsoL3",
                    "https://chat.whatsapp.com/JVRUE4zNHKaKgroCp66BB9",
                    "https://chat.whatsapp.com/IwZ62uiOPZZLNMotqj93BUlwZ62uiOPZZLNMotqj93BU",
                    "https://chat.whatsapp.com/D7V6NBbkdemKdy5jIXI1ej",
                    "https://chat.whatsapp.com/FOhJBVBBJQM47RZgYcoQFM",
                    "https://chat.whatsapp.com/CRBtKw7JnpYBKhGJum4j4P",
                    "https://chat.whatsapp.com/BuKsPSAT5m9HVIYFX4CIYj"
                };
                foreach (var link in convites)
                {
                    try
                    {
                        SendMessageToGroup(link);
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro no convite");
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                webDriver.Close();
                webDriver.Dispose();
            }
            Console.ReadKey();
        }

        private static void SendMessageToGroup(string conviteUrl)
        {
            Console.WriteLine("Carregando a página de convite.");

            webDriver.LoadPage(TimeSpan.FromSeconds(25), conviteUrl);

            //Clicar no botao Entrar na conversa
            Console.WriteLine("Clicando no botao Entrar na conversa.");
            var btnJoin = webDriver.FindElement(By.Id("action-button"));
            btnJoin.Click();

            //Aguardo o botão Entrar no grupo aparecer
            Thread.Sleep(TimeSpan.FromSeconds(10));

            //Clicar no botao para Usar Wpp Web
            Console.WriteLine("Clicando no botao para usar o wpp web.");
            var btnWeb = webDriver.FindElement(By.XPath("//*[@id='fallback_block']/div/div/a"));
            btnWeb.Click();

            //Aguardo o botão Entrar no grupo aparecer
            Thread.Sleep(TimeSpan.FromSeconds(10));

            //Clicar no botão Entrar no grupo
            Console.WriteLine("Clicando no botao entrar no grupo.");
            var btnEntrarNoGrupo = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            btnEntrarNoGrupo.Click();

            Thread.Sleep(TimeSpan.FromSeconds(15));

            //Digitar uma mensagem
            Console.WriteLine("Enviando a mensagem");
            var txtMensagem = By.XPath("//*[@id='main']/footer/div[1]/div[2]/div/div[2]");
            webDriver.SetText(txtMensagem, @"
Cansado de usar sites de torrents cheio de anúncios? Visite nosso site!
Site rápido, moderno e sem anúncios excessivos!
Últimos lançamentos de 2020!
Acesse: https://speckoz.live");

            //Enviamos a mensagem
            webDriver.FindElement(txtMensagem).SendKeys(Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(20));

            Console.WriteLine("Abrindo o menu.");
            var opcoesDoMenu = webDriver.FindElement(By.XPath("//*[@id='main']/header"));
            opcoesDoMenu.Click();

            Console.WriteLine("Aguardando para o botão sair aparacer.");
            Thread.Sleep(TimeSpan.FromSeconds(7));

            Console.WriteLine("Clicando pra sair do grupo.");
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
