from flask import Flask, jsonify
import cv2 as cv
import mediapipe.python.solutions.hands as mp_hands
import numpy as np
import os
import threading
import time

# Flask app
app = Flask(__name__)

# MediaPipe Hands
hands = mp_hands.Hands(
    static_image_mode=False,
    max_num_hands=1,
    min_detection_confidence=0.7
)

# Gesture files
script_dir = os.path.dirname(os.path.abspath(__file__))
poses_dir = os.path.join(script_dir, "Poses/Hand")
gesture_files = {
    "Thumbs Up": "ThumbsUp.npy",
    # "Thumbs Down": "ThumbsDown.npy",
    "Thumbs Down": "ThumbsDown_merged.npy",
    # "Middle Finger": "middleFinger.npy",
}

gestures = {}
for gesture_name, gesture_file in gesture_files.items():
    try:
        gesture_path = os.path.join(poses_dir, gesture_file)
        gestures[gesture_name] = np.load(gesture_path)
        print(f"Loaded gesture: {gesture_name} from {gesture_path}")
    except FileNotFoundError:
        print(f"Error: Gesture file '{gesture_file}' not found!")

# Global variables for shared state
latest_frame = None
latest_hand_landmarks = None
state_lock = threading.Lock()
capture_running = True
state = {
    "matched_gesture": None,
    "match_start_time": None
}
required_duration = 1.0  # Seconds

def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
    """Compare player's hand pose with the reference pose."""
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    return np.mean(distance) < threshold

def capture_frames():
    """Continuously capture frames from the camera."""
    global latest_frame, latest_hand_landmarks, capture_running

    cap = cv.VideoCapture(0)
    while capture_running:
        ret, frame = cap.read()
        if not ret:
            continue

        # Flip and process the frame
        frame = cv.flip(frame, 1)
        rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
        hands_detected = hands.process(rgb_frame)

        # Store the frame and landmarks in shared state
        with state_lock:
            latest_frame = frame
            latest_hand_landmarks = hands_detected.multi_hand_landmarks

    cap.release()

@app.route('/check_gesture', methods=['POST'])
def check_gesture():
    """API endpoint to detect and validate hand gestures."""
    global state

    # Access the latest frame and landmarks
    with state_lock:
        frame = latest_frame
        hand_landmarks = latest_hand_landmarks

    if frame is None or hand_landmarks is None:
        # Reset state if no hands are detected
        state["matched_gesture"] = None
        state["match_start_time"] = None
        return jsonify({"status": "No Gesture"})

    for hand_landmark in hand_landmarks:
        player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])

        for gesture_name, reference_pose in gestures.items():
            if compare_hand_pose(player_hand_pose, reference_pose):
                if state["matched_gesture"] != gesture_name:
                    # New gesture detected
                    state["matched_gesture"] = gesture_name
                    state["match_start_time"] = time.time()
                    return jsonify({"status": "Gesture Matched", "gesture": gesture_name, "holding": 0})
                else:
                    # Gesture is still being held
                    holding_duration = time.time() - state["match_start_time"]
                    if holding_duration >= required_duration:
                        return jsonify({"status": "Gesture Verified", "gesture": gesture_name})
                    else:
                        return jsonify({"status": "Gesture Matched", "gesture": gesture_name, "holding": holding_duration})

    # Reset state if no gesture matches
    state["matched_gesture"] = None
    state["match_start_time"] = None
    return jsonify({"status": "No Gesture"})

if __name__ == "__main__":
    # Start the frame capture thread
    capture_thread = threading.Thread(target=capture_frames, daemon=True)
    capture_thread.start()

    try:
        app.run(host="127.0.0.1", port=5000)
    finally:
        # Stop the frame capture thread
        capture_running = False
        capture_thread.join()

