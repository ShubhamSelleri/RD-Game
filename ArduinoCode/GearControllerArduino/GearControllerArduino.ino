#include <ESP32RotaryEncoder.h>

// Change these to the actual pin numbers that you've connected your rotary encoder to
const uint8_t DI_ENCODER_A   = 25;
const uint8_t DI_ENCODER_B   = 27;
const int8_t  DI_ENCODER_SW  = 26;
const int8_t  DO_ENCODER_VCC = 13;

// Instantiate the rotary encoder object
RotaryEncoder rotaryEncoder(DI_ENCODER_A, DI_ENCODER_B, DI_ENCODER_SW, DO_ENCODER_VCC);

// Initialize a global variable to track the current value
long currentValue = 5;  // Starting value between 1 and 20

// Variable to store the last sent value (initially set to an invalid value)
long lastSentValue = -1;

// This function will be called whenever the rotary encoder is turned
void knobCallback(long value) {
    // Update the current value based on the knob turn
    currentValue = value;
}

// This function will be called whenever the button is pressed
void buttonCallback(unsigned long duration) {
    // Print "boop!" and the button press duration to the serial console
    Serial.println("boop! button was pressed.");
}

// Setup function
void setup() {
    Serial.begin(115200);  // Start Serial communication

    // This tells the library that the encoder has its own pull-up resistors
    rotaryEncoder.setEncoderType(EncoderType::HAS_PULLUP);

    // Set boundaries for the encoder's value between 1 and 20, with wraparound
    rotaryEncoder.setBoundaries(1, 24, true);

    // Assign callback functions for turning the knob and pressing the button
    rotaryEncoder.onTurned(&knobCallback);
    // rotaryEncoder.onPressed(&buttonCallback);

    // Initialize the encoder input pins and attach interrupts
    rotaryEncoder.begin();

    // Print the initial value to the serial console
    Serial.println("connected");

    // Set the initial last sent value to the current value
    lastSentValue = currentValue;
}

// Loop function
void loop() {
    // Keep the rotary encoder running and detecting changes
    rotaryEncoder.loop();

    // If the current value has changed and is different from the last sent value
    if (currentValue != lastSentValue) {

        if(currentValue > lastSentValue && !(currentValue == 24 && lastSentValue == 1)||(currentValue == 1 && lastSentValue == 24))
        Serial.println("0");
      
        else
        Serial.println("1");
        // Update the last sent value to the current value
        lastSentValue = currentValue;
    }
}