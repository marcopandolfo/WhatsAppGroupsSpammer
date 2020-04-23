using OpenQA.Selenium;
using prmToolkit.Selenium;
using prmToolkit.Selenium.Enum;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using static Bot.WhatsApp.Selenium.Utils;

namespace Bot.WhatsApp.Selenium
{
    public class Bot
    {
        static readonly IWebDriver webDriver = WebDriverFactory.CreateWebDriver(Browser.Chrome, @"C:\Users\marco\dev\WhatsAppGroupsSpammer\Driver", false);
        static void Main(string[] args)
        {
            try
            {
                Home();
            }
            catch (Exception ex)
            {
                Console.Clear();
                LogWarning("Ocorreu uma exception!");
                Console.WriteLine(ex.Message);
                LogImportant("Pressione enter para voltar para o menu");
                Console.ReadKey();
                Home();
            }
        }

        private static void Home()
        {
            Console.WriteLine(GetAsciiArt());
            LogImportant("O que deseja fazer?\n");
            LogOption(1, "Carregar convites e iniciar SPAM");
            LogOption(2, "Sair");
            Console.Write("> ");
            int option = ParseIntAnswer(Console.ReadLine());

            switch (option)
            {
                case 1:
                    Console.Clear();
                    LogImportant("Você selecionou 'Inciar SPAM'");
                    LogImportant("Deseja setar um limite da quantidade de links para buscar? (Pode demorar alguns minutos)");
                    LogOption(1, "Sim");
                    LogOption(2, "Não");
                    Console.Write("> ");

                    int? qtdePages = null;
                    if (ParseIntAnswer(Console.ReadLine()) == 1)
                    {
                        LogImportant("Quantas paginas? (0 - 9099)");
                        Console.Write("> ");
                        qtdePages = ParseIntAnswer(Console.ReadLine());
                        LogImportant($"Certo, buscando {qtdePages} paginas de links de grupos");
                    }
                    else
                        LogImportant($"Certo, buscando as paginas de links de grupos");

                    List<string> links = qtdePages != null ? GetNewInvites((int)qtdePages) : GetNewInvites();
                    Console.Clear();
                    LogImportant($"BUSCA DE LINKS FINALIZADA! FORAM ENCONTRADOS [{links.Count}] LINKS");
                    LogImportant("Pressione Enter para inserir o QR Code");
                    Console.ReadKey();
                    MakeSpam(links);
                    break;

                // Sair
                case 2:
                    break;

                // Opção não existente
                default:
                    break;
            }
        }

        public static void MakeSpam(List<string> links)
        {
            int success = 0;
            int failed = 0;
            int counter = 0;

            LogInfo("Abrindo o Whatsapp Web!");
            webDriver.LoadPage(TimeSpan.FromSeconds(15), "https://web.whatsapp.com");
            LogImportant("Pegue seu celular e leia o QRCODE.");
            Thread.Sleep(TimeSpan.FromSeconds(15));
            Console.Clear();

            foreach (string link in links)
            {
                counter++;
                Console.Clear();
                LogNumberOfMessagesSent(success, failed);
                Console.Write($" Progresso: {counter}/{links.Count}\n");

                try
                {
                    SendMessageToGroup(link);
                    success++;
                }
                catch (Exception)
                {
                    LogWarning("Convite Inválido, seguindo para o proximo...");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    failed++;
                    continue;
                }
            }
            Console.Clear();
            LogImportant("SPAM Finalizado!");
            Console.WriteLine($"[*] Total de grupos spamados: {success}");
            Console.WriteLine($"[*] Total de convites invalidos: ${failed}");
        }

        private static void SendMessageToGroup(string invite)
        {
            LogInfo("Carregando a página de convite.");
            webDriver.LoadPage(TimeSpan.FromSeconds(25), invite);

            LogInfo("Clicando no botão para entrar na conversa");
            var btnJoin = webDriver.FindElement(By.Id("action-button"));
            btnJoin.Click();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            LogInfo("Clicando no botão para usar o whatsapp web");
            var btnWeb = webDriver.FindElement(By.XPath("//*[@id='fallback_block']/div/div/a"));
            btnWeb.Click();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            LogInfo("Clicando no botão de entrar no grupo");
            var btnJoinGroup = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            btnJoinGroup.Click();
            Thread.Sleep(TimeSpan.FromSeconds(15));

            LogInfo("Escrevendo a mensagem");
            var inputMsg = By.XPath("//*[@id='main']/footer/div[1]/div[2]/div/div[2]");

            webDriver.SetText(inputMsg, GetMessageToSend());

            LogInfo("Enviando a mensagem");
            webDriver.FindElement(inputMsg).SendKeys(Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(25));

            LogInfo("Abrindo o menu para sair do grupo.");
            var opcoesDoMenu = webDriver.FindElement(By.XPath("//*[@id='main']/header/div[2]"));
            opcoesDoMenu.Click();

            LogInfo("Aguardando para o botão sair aparacer.");
            Thread.Sleep(TimeSpan.FromSeconds(3));

            LogInfo("Clicando pra sair do grupo.");
            var btnSairGrupo = webDriver.FindElement(By.XPath("//*[@id='app']/div/div/div[2]/div[3]/span/div/span/div/div/div[1]/div[6]/div"));
            btnSairGrupo.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            LogInfo("Confirmando saida");
            var confirmExit = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            confirmExit.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            LogInfo("Operação realizada com sucesso!");
        }

        private static List<string> GetNewInvites(int maxPages = 999999)
        {
            List<string> links = new List<string>();
            int urlCounter = 1;

            for (int page = 1; page <= maxPages; page++)
            {
                try
                {
                    string url = $"https://gruposwhats.app/?page={page}";
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0;)");
                        string htmlCode = client.DownloadString(url);
                        var linkParser = new Regex(@"(https?://gruposwhats\.app\/group\/\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        LogImportant($"BUSCANDO LINKS DE GRUPOS (Pagina ({page}/{maxPages})");
                        foreach (Match m in linkParser.Matches(htmlCode))
                        {
                            string joinGroupPageHtmlCode = client.DownloadString(m.Value);
                            var groupLinkParser = new Regex(@"(https?://gruposwhats\.app\/join-group\/\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            var joinGroupMatch = groupLinkParser.Matches(joinGroupPageHtmlCode)[0];

                            LogInfo($"Link descoberto -> {joinGroupMatch.Value} (Total: {urlCounter})");
                            links.Add(joinGroupMatch.Value);

                            urlCounter++;
                        }
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }

            return links;
        }
    }
}
