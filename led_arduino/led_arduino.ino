void setup() {
  Serial.begin(9600); // inicia a portal serial com velocidade definida para 9600 bps (bits por segundo - velocidade de transmissao)
  //pinMode(LED_BUILTIN, OUTPUT);
  pinMode(12, OUTPUT);
}

void loop() {
  if(Serial.available())
  {
    switch(Serial.read())
    {
      case '0':
        //digitalWrite(LED_BUILTIN, LOW);
        digitalWrite(12, LOW);
        break;
      case '1':
        //digitalWrite(LED_BUILTIN, HIGH);
        digitalWrite(12, HIGH);
        break;
    }
  }
}
