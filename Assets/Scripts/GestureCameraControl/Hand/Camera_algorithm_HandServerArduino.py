import time
import serial
from flask import Flask, jsonify
import cv2 as cv
import mediapipe.python.solutions.hands as mp_hands
import numpy as np
import os
import threading

# Flask app
app = Flask(__name__)

# Arduino connection settings
arduino_port = "/dev/cu.usbmodem11102"  # Adjust this to match your Arduino's port
baud_rate = 9600                       # Match this to your Arduino's baud rate
arduino = None

# Function to connect to Arduino
def connect_to_arduino(retries=5, delay=2):
    """Attempt to connect to the Arduino with retries."""
    global arduino
    for attempt in range(1, retries + 1):
        try:
            print(f"Attempting to connect to Arduino (Attempt {attempt}/{retries})...")
            arduino = serial.Serial(arduino_port, baud_rate, timeout=1)
            print("Arduino connected successfully.")
            return
        except serial.SerialException as e:
            print(f"Failed to connect to Arduino on attempt {attempt}: {e}")
            time.sleep(delay)
    print("Unable to connect to Arduino after multiple attempts.")

# Function to send command to Arduino
def send_to_arduino(command):
    """Send a command to the Arduino via serial."""
    if arduino and arduino.is_open:
        try:
            arduino.write((command + '\n').encode())
            print(f"Sent to Arduino: {command}")
        except serial.SerialException as e:
            print(f"Error sending to Arduino: {e}")
    else:
        print("Arduino is not connected.")

# MediaPipe Hands for gesture recognition
hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=1,
    min_detection_confidence=0.8,
    min_tracking_confidence=0.8
)

# Gesture files
script_dir = os.path.dirname(os.path.abspath(__file__))
poses_dir = os.path.join(script_dir, os.pardir, "Poses/Hand")
gesture_files = {
    "Thumbs Up": "ThumbsUp.npy",
    "Thumbs Down": "ThumbsDown.npy",
    "Rock": "Rock.npy",
}

# Load gestures
gestures = {}
for gesture_name, gesture_file in gesture_files.items():
    try:
        gesture_path = os.path.join(poses_dir, gesture_file)
        gestures[gesture_name] = np.load(gesture_path)
        print(f"Loaded gesture '{gesture_name}' from {gesture_path}")
    except FileNotFoundError:
        print(f"Warning: Gesture file '{gesture_file}' not found. Skipping.")

# Global variables for state
state_lock = threading.Lock()
capture_running = True
state = {
    "last_verified_gesture": None,
    "last_sent_gesture": None,  # Track last gesture sent to Arduino
    "last_updated": time.time(),
}
required_duration = 0.5  # Seconds to verify gesture

# Compare hand pose with saved gestures
def compare_hand_pose(player_pose, reference_pose, threshold=0.15):
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    return np.mean(distance) < threshold

# Capture frames and recognize gestures
def capture_and_track_gestures():
    global capture_running, state

    cap = cv.VideoCapture(0)
    if not cap.isOpened():
        print("Error: Camera not found.")
        return

    while capture_running:
        ret, frame = cap.read()
        if not ret:
            continue

        # Flip and process the frame
        frame = cv.flip(frame, 1)
        rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
        hands_detected = hands.process(rgb_frame)

        with state_lock:
            if hands_detected.multi_hand_landmarks:
                for hand_landmark in hands_detected.multi_hand_landmarks:
                    player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])

                    for gesture_name, reference_pose in gestures.items():
                        if compare_hand_pose(player_hand_pose, reference_pose):
                            # Verify gesture if held for required duration
                            if (
                                state["last_verified_gesture"] != gesture_name
                                or time.time() - state["last_updated"] >= required_duration
                            ):
                                state["last_verified_gesture"] = gesture_name
                                state["last_updated"] = time.time()

                                # Send to Arduino only if gesture has changed
                                if state["last_sent_gesture"] != gesture_name:
                                    # print(f"New Gesture Detected: {gesture_name}")
                                    if gesture_name == "Thumbs Up":
                                        send_to_arduino("green")
                                    elif gesture_name == "Thumbs Down":
                                        send_to_arduino("red")
                                    # elif gesture_name == "Rock":
                                    #     send_to_arduino("blue")
                                    state["last_sent_gesture"] = gesture_name
                            break
            else:
                # No gesture detected
                if state["last_verified_gesture"] is not None:
                    # print("No Gesture Detected. Clearing Arduino.")
                    send_to_arduino("clear")
                    state["last_verified_gesture"] = None
                    state["last_sent_gesture"] = None

    cap.release()

# API endpoint to check current gesture
@app.route('/check_gesture', methods=['POST'])
def check_gesture():
    global state

    with state_lock:
        if state["last_verified_gesture"]:
            return jsonify({"gesture": state["last_verified_gesture"]})
        else:
            return jsonify({"gesture": "No Gesture"})

if __name__ == "__main__":
    # Connect to Arduino
    connect_to_arduino(retries=5, delay=2)

    # Start gesture tracking thread
    capture_thread = threading.Thread(target=capture_and_track_gestures, daemon=True)
    capture_thread.start()

    try:
        app.run(host="127.0.0.1", port=5000)
    finally:
        # Stop the capture thread
        capture_running = False
        capture_thread.join()
