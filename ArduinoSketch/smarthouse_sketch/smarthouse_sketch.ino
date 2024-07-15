#include <SPI.h>
#include <WiFiS3.h>
#include "Adafruit_Keypad.h"
#include <Servo.h> // BIBLIOTECA SERVO

#define ledPin 10
#define pirPin 9

int calibrationTime = 30;
long unsigned int lowIn;
long unsigned int pause = 5000;
boolean lockLow = true;
boolean takeLowTime;
int PIRValue = 0;

///////colocar dados sensíveis em ficheiro separado/arduino_secrets.h
// dados Wi-Fi 
char ssid[] = "Galaxy-RC";          //  SSID (name) / Galaxy-RC
char pass[] = "ttqo3746";   //  senha ttqo3746
int status = WL_IDLE_STATUS;                     // Wifi radio's status
//  site URL para onde serão enviados os dados
char* host = "46.105.31.193";
const int postPorta = 8081;
// Variáveis globais que irão armazenar os valores dos sensores
int porta;
int movimento;
const long postDuracao = 10000; //intervalo entre cada envio para a base de dados
unsigned long ultimoPost = 0;
bool conectado = false;
  WiFiClient client;
//---------------------

Servo servo_Motor; //OBJETO DO TIPO SERVO
char* password = "1234"; //SENHA CORRETA PARA DESTRANCAR A FECHADURA
int posicao = 0; //VARIÁVEL PARA LEITURA DE POSIÇÃO DA TECLA PRESSIONADA
const byte LINHAS = 4; //NUMERO DE LINHAS DO TECLADO
const byte COLUNAS = 3; //NUMERO DE COLUNAS DO TECLADO
char keys[LINHAS][COLUNAS] = { //DECLARAÇÃO DOS NUMEROS, LETRAS E CARACTERES DO TECLADO
{'1','2','3'},
{'4','5','6'},
{'7','8','9'},
{'*','0','#'}
};

byte rowPins[LINHAS] = { 8, 7, 6, 2 }; // PINOS DE CONEXÃO DAS LINHAS DO TECLADO
byte colPins[COLUNAS] = { 5, 4, 3}; //PINOS DE CONEXÃO DAS COLUNAS DO TECLADO
Adafruit_Keypad mykeypad = Adafruit_Keypad( makeKeymap(keys), rowPins, colPins, LINHAS, COLUNAS );//AS VARIÁVEIS rowPins E colPins RECEBEM O VALOR DE LEITURA DOS PINOS DAS LINHAS E COLUNAS RESPECTIVAMENTE
const int ledVermelho = 12; //PINO ONDE QUE ESTÁ CONECTADO O LED VERMELHO
const int ledVerde = 13; //PINO ONDE QUE ESTÁ CONECTADO O LED VERDE


/*
 * Método WIFICONNECT para efetuar a ligação a uma determinada rede
 * wi-fi através de ssid e password
 */
void wificonnect(char ssid[], char pass[]) {
  //Serial.begin(9600); //INICIALIZA A SERIAL
    //while (!Serial) {
   // ; // espera pela porta série para conectar. 
  //}
  //delay(500);

  // verificar de existe o módulo wifi:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communicação com o módulo wifi falhou!");
    // não continúa
    while (true);
  }
  // verificar se o firmware do módulo wireless está atualizado:
  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }
  // Tentativa de ligação à Rede Wifi:
  while (status != WL_CONNECTED) {
    Serial.print("Tentativa de ligação à Rede Wifi, SSID: ");
    Serial.println(ssid);
    status = WiFi.begin(ssid, pass);
    // espera 10 segundos para tentar ligar de novo:
    delay(10000);
  }

Serial.println(WiFi.localIP());
  // definimos a duração do último post e fazemos o post imediatamente
  // desde que o main loop inicía
  ultimoPost = postDuracao;

  Serial.println("Está conectado à rede");
}


bool leitura_movimento() {
   if(digitalRead(pirPin) == HIGH) {  
      if(lockLow) {
         PIRValue = 1;
         movimento= 1;
         lockLow = false;
         Serial.println("Movimento detectado.");
         digitalWrite(ledPin, HIGH); //ACENDE O LED
         delay(50);
      }
      takeLowTime = true;
   }
   if(digitalRead(pirPin) == LOW) {
      if(takeLowTime){
         lowIn = millis();takeLowTime = false;
      }
      if(!lockLow && millis() - lowIn > pause) {
         PIRValue = 0;
         movimento= 0;
         lockLow = true;
         Serial.println("Movimento finalizado.");
         digitalWrite(ledPin, LOW); //ACENDE O LED
         delay(50);

      }
    
   }

  return movimento ;

}

bool leitura_porta(){
mykeypad.tick();

  while(mykeypad.available()){
  keypadEvent key = mykeypad.read();
  if(key.bit.EVENT == KEY_JUST_PRESSED) 
  {
    Serial.print("Tecla pressionada : "); //IMPRIME O TEXTO NO MONITOR SERIAL
    Serial.println((char)key.bit.KEY); //IMPRIME NO MONITOR SERIAL A TECLA PRESSIONADA

if (key.bit.KEY == '*' || key.bit.KEY == '#'){ //SE A TECLA PRESSIONADA POR IGUAL A CARACTERE "*" OU "#", FAZ
posicao = 0; //POSIÇÃO DE LEITURA DA TECLA PRESSIONADA INICIA EM 0
setLocked(true); //PORTA FECHADA
 porta = 0;
}

if (key.bit.KEY == password[posicao]){ //SE A TECLA PRESSIONADA CORRESPONDER A SEQUÊNCIA DA SENHA, FAZ
posicao ++;//PULA PARA A PRÓXIMA POSIÇÃO
}
if (posicao == 4){ // SE VARIÁVEL FOR IGUAL A 3 FAZ (QUANDO AS TECLAS PRESSIONADAS CHEGAREM A 4 POSIÇÕES, SIGNIFICA QUE A SENHA ESTÁ CORRETA)
setLocked(false); //PORTA ABERTA
porta = 1;
}

}
}
return porta;
}


/* Método que realiza a tarefa de enviar as leituras 
 *para um serviço externo neste caso um servidor web  
 */
void SaveDados(int porta, int movimento) {
  //leituras(porta, movimento);

  Serial.println("Envio de Dados - Início ");
  Serial.print(" - A conectar-se a ");
  Serial.println(host);
  Serial.print(" - na porta ");
  Serial.println(postPorta);  
  
  // Criar o URI para o request/pedido
  String url = String("/Home/SaveDados") + String("?porta=") + porta +
  String("&movimento=") + movimento;
  Serial.println(" - A solicitar o URL: ");
  Serial.print("     ");
  Serial.println(url);
  
  // envia o request/pedido para o servidor
  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" + 
               "Connection: close\r\n\r\n");
  //delay(500);

  // Lê todas as linhas de resposta que vem do servidor web 
  // e escreve no serial monitor
  Serial.println(" - Resposta do SERVIDOR: ");
  while(client.available()){
    String line = client.readStringUntil('\r');
    Serial.print(line);
  }
  Serial.println("");
  Serial.println(" - Fechar a conexão");
  Serial.println("");

  Serial.println("Envio de Dados - FIM");
  Serial.println("");
}

void setup(){
  Serial.begin(9600); //INICIALIZA A SERIAL

    //chama a função para conectar ao wifi
  wificonnect(ssid, pass);

   mykeypad.begin();
  Serial.println("Aperte uma tecla..."); //IMPRIME O TEXTO NO MONITOR SERIAL
  Serial.println(); //QUEBRA UMA LINHA NO MONITOR SERIAL

pinMode(ledPin, OUTPUT); //DEFINE O PINO COMO SAÍDA

  pinMode(pirPin, INPUT);//DECLARA O PINO DO SENSOR MOVIEMNTO COMO ENTRADA
  servo_Motor.attach(11); //PINO DE CONTROLE DO SERVO MOTOR
  setLocked(true); //ESTADO INICIAL DA FECHADURA (FECHADA)


}

void setLocked(int locked){ //MANUSEAR O ESTADO DA FECHADURA
  if (locked){ //SE FECHADURA TRANCADA, FAZ
  digitalWrite(ledVermelho, HIGH);// LED VERMELHO ACENDE
  digitalWrite(ledVerde, LOW);// LED VERDE APAGA
  servo_Motor.write(0); //POSIÇÃO DO SERVO FICA EM 0º (FECHADURA TRANCADA)
}
else{ //CASO CONTRÁRIO, FAZ
  digitalWrite(ledVerde, HIGH);
  digitalWrite(ledVermelho, LOW);
  servo_Motor.write(180);// SERVO GIRA A 90º (FECHADURA DESTRANCADA)

}
}

void loop(){
  int movimento = leitura_movimento();
  int porta = leitura_porta();

 
 if (client.connect(host, postPorta)) {
    unsigned long diff = millis() - ultimoPost;
   
      SaveDados(porta,movimento);
      ultimoPost = millis();
 
  } else {
    Serial.println(" - Não conseguiu conectar-se ao host!");
    delay(100);
  }
  delay(100);
  }


