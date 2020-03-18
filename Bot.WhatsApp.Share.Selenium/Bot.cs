﻿using OpenQA.Selenium;
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
            List<string> links = GetNewInvites();
            int success = 0;
            int failed = 0;

            Console.Clear();
            LogImportant($"BUSCA DE LINKS FINALIZADA! FORAM ENCONTRADOS [{links.Count}] LINKS");
            LogImportant("PRESSIONE ENTER PARA CONTINUAR E LER O QR CODE");
            Console.ReadKey();

            LogInfo("Abrindo o Whatsapp Web!");
            webDriver.LoadPage(TimeSpan.FromSeconds(15), "https://web.whatsapp.com");
            LogImportant("Pegue seu celular e leia o QRCODE.");
            Thread.Sleep(TimeSpan.FromSeconds(15));
            Console.Clear();

            string message = GetMessageToSend();
            foreach (string link in links)
            {
                Console.Clear();
                LogNumberOfMessagesSent(success, failed);
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

            LogInfo("Confirmando saida");
            var confirmExit = webDriver.FindElement(By.XPath("//*[@id='app']/div/span[2]/div/div/div/div/div/div/div[2]/div[2]"));
            confirmExit.Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            LogInfo("Operação realizada com sucesso!");
        }

        private static List<string> GetNewInvites()
        {
            int pages = 1;
            List<string> links = new List<string>();
            int urlCounter = 1;

            for (int page = 1; page <= pages; page++)
            {
                string url = $"https://gruposwhats.app/?page=8";

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0;)");
                    string htmlCode = client.DownloadString(url);
                    var linkParser = new Regex(@"(https?://gruposwhats\.app\/group\/\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    LogImportant($"BUSCANDO LINKS DE GRUPOS (Pagina ({page}/{pages})");
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

            return links;
        }
    }
}
