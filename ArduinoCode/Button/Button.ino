constexpr int buttonPin = 2;     // Compile-time constant for the button pin
volatile bool buttonPressed = false; // Flag to indicate button press
unsigned long lastDebounceTime = 0;  // Last time the button was toggled
constexpr unsigned long debounceDelay = 300; // Debounce delay in milliseconds

void setup() {
  pinMode(buttonPin, INPUT_PULLUP); // Set button pin as input with internal pull-up resistor
  attachInterrupt(digitalPinToInterrupt(buttonPin), buttonISR, FALLING); // Attach interrupt on FALLING edge
  Serial.begin(9600);              // Start serial communication
}

// Interrupt Service Routine (ISR) for the button press
void buttonISR() {
  if (millis() - lastDebounceTime > debounceDelay) { // Check if enough time has passed for debounce
    buttonPressed = true; // Set the flag to indicate a valid button press
    lastDebounceTime = millis(); // Update the last debounce time
  }
}

// Function to handle button actions
void handleButtonPress() {
  if (buttonPressed) {
    Serial.println('p'); // Print 'p' to the console
    buttonPressed = false; // Reset the button press flag
  }
}

void loop() {
  handleButtonPress(); // Call the function to handle the button press
}
