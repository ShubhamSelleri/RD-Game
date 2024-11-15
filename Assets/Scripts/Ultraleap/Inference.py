import numpy as np
import joblib

# Function to recognize gesture
def recognize_gesture(model, scaler, gesture_data):
    # Flatten the input gesture data for scaling
    gesture_data_flattened = np.array(gesture_data).flatten().reshape(1, -1)  # Reshape for single prediction
    gesture_data_scaled = scaler.transform(gesture_data_flattened)  # Scale the input
    prediction = model.predict(gesture_data_scaled)
    return prediction[0]

# Load the model and scaler
model = joblib.load('Assets/Scripts/Ultraleap/gesture_recognition_model.joblib')
scaler = joblib.load('Assets/Scripts/Ultraleap/gesture_scaler.joblib')

# Example input gesture data for inference (replace with actual gesture data)
# example_gesture = [(0.03, -0.43, 0.90), (0.37, 0.80, 0.47), (0.35, 0.15, 0.93), (-0.01, 0.99, -0.17), (-0.06, 0.99, 0.15)]

# Read gesture data from file
# Read the data from the text file
with open('Assets/Scripts/Ultraleap/gesture_data.txt', 'r') as file:
    data = file.read()

# Remove any unwanted characters and split the string into individual tuples
data = data.strip().split('), (')
data = [tuple(map(float, item.replace('(', '').replace(')', '').split(', '))) for item in data]

# Convert to array of tuples
example_gesture = data
print(example_gesture)


# Run inference
predicted_gesture = recognize_gesture(model, scaler, example_gesture)

# Output the predicted gesture
print(f"Predicted Gesture: {predicted_gesture}")

### Write the predicted gesture to a txt file ###
output_path = 'Assets/Scripts/Ultraleap/predicted_gesture.txt'

# Open the file in write mode ('w'), which overwrites existing content
with open(output_path, 'w') as file:
    # Write the new string to the file
    file.write(predicted_gesture)
