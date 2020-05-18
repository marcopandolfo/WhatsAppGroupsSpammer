# WhatsAppGroupsSpammer 👾

Bot para enviar mensagens em massa para grupos de whatsapp.

# Funcionamento do bot 🕹️

O bot funciona buscando massivamente links de grupos de whatsapp nesse <a href="https://gruposwhats.app/">site</a>.

Após buscar os links, ele entra em cada grupo, manda a mensagem e sai logo após.

Se o convite for inválido (inexistente, grupo cheio e etc) ele pula para o proximo grupo.

# Como usar 🎮
Para rodar o bot, apenas abra o projeto no seu visual studio e inicie o bot e ele irá buscar automaticamente os convites para entrar.

Após ele pegar todos os links, você deverá pressionar enter e ler o QR code que irá abrir no navegador, após isso o bot vai começar o envio das mensagens.

O driver vem junto com o projeto na pasta Driver/ então basta copiar o path dessa pasta e colar na linha 15.

Altere a mensagem para a que você deseja em Utils.cs linha 39.

# Tecnologias utilizadas 📜
- .NET Core 2.2
- C# 7
- Selenium

# Bot em funcionamento 🚀

Primeira etapa: busca de links

![image](https://user-images.githubusercontent.com/40467826/76956877-99820c80-68f3-11ea-8eb8-543ef302c580.png)

Segunda etapa: leitura do qr code

![image](https://user-images.githubusercontent.com/40467826/76957310-52e0e200-68f4-11ea-9932-186bcf05d840.png)

Terceira etapa: envio nas mensagens

![image](https://user-images.githubusercontent.com/40467826/76957497-ab17e400-68f4-11ea-884d-51dbcfff9bb4.png)



Bot desenvolvido para fins de estudo, não me responsabilizo pelo seu uso do mesmo.
