#define BUTTON_COUNT 9

//the last pin is mode button
const int BUTTON_PIN[] = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
bool button_state[BUTTON_COUNT];

void setup()
{
  Serial.begin(9600);
  for(int i=0 ; i<BUTTON_COUNT ; i++)
  {
    pinMode(BUTTON_PIN[i], INPUT_PULLUP);
    button_state[i] = false;
  }
}

void loop()
{
  for(byte i=0 ; i<BUTTON_COUNT ; i++)
  {
    bool stat = digitalRead(BUTTON_PIN[i]);
    if(stat != button_state[i])
    {
      if(stat)
        Serial.print(i+1);
      else if(i != BUTTON_COUNT-1)
        Serial.print(i+BUTTON_COUNT);
      button_state[i] = stat;
    }
  }
}
