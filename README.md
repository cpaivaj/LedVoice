# LedVoice
## Projeto para acender/apagar LED por comando de voz com C# e arduino

### Este projeto envia comandos para o arduino através de comando serial.

#### O que usei para esse Projeto?
- Visual Studio Community
- Arduino IDE
- Uma parte do conteúdo desse site - https://www.embarcados.com.br/comunicacao-serial-c-arduino-parte-1/
- Reconhecimento de voz - Vídeo no meu canal do YouTube - https://www.youtube.com/watch?v=vUGDKknf0Pw
- Esquema dos componentes - https://tecdicas.com/como-acender-e-piscar-um-led-no-arduino/
- Uma placa de Arduino Uno
- 2 fios (que vêm no kit do arduino)
- 1 resistor de 1000 ohms - é o único que eu tinha (pode usar de 220 ohms ou 120, dependendo do LED)
- 1 LED
- 1 protoboard

Abaixo eu detalho os passos que segui durante a construção do projeto.

- Criar projeto windows form
- Inserir um botão, um combobox e um label
	- O botão para conectar na porta serial (btnConecta)
	- O combobox pra selecionar qual porta COM vai usar
	- O label para avisar se o sistema está 'ouvindo' ou não (PODE FALAR | SELECIONE UMA PORTA) (lblStatus)

- Precisa inserir um timer para atualização das portas COM disponíveis (interval = 1000 -> todo segundo)
- Por último, precisa inserir o componente para comunicação serial (SerialPort), vai aparecer do lado do timer
- Adicionar código para atualização das portas COM disponíveis
- Adicionar código do timer acionando o método das portas (2 cliques no timer)
- Quando você conectar o arduino a porta já vai ser reconhecida automaticamente

- Agora tem que fazer a funcionalidade de conectar a porta, através do clique do botão conectar
	- 2 cliques nele para criar o evento

- Precisa colocar uma proteção para liberar a porta quando terminar de usar, pois outros programas não irão conseguir usá-la
	- Dentro do evento formClosed (basta ir no campo de eventos do form e dar 2 cliques em cima de FormClosed)

- Adiciona o código de reconhecimento de voz (lembrando que precisa ter os comandos corretos 'acender' e 'apagar')
- A funcionalidade do botão de conectar pode ser por voz também, tanto faz

- Agora precisamos enviar o comando para o arduino quando o programa 'escutar' o comando
	- Vamos enviar apenas um dígito (1 ou 0) 0 -> apaga e 1 -> acende
		- Criei dois métodos para deixar mais didático o código, mas se não quiser não precisa


### REFERÊNCIAS

- C# usando porta serial - https://www.embarcados.com.br/comunicacao-serial-c-arduino-parte-1/
- Reconhecimento de voz em C# (meu video) - https://www.youtube.com/watch?v=vUGDKknf0Pw
- Acender LED com arduino - https://tecdicas.com/como-acender-e-piscar-um-led-no-arduino/
