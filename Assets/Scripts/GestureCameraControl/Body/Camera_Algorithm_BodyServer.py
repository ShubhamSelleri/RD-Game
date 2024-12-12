from flask import Flask, jsonify, request
import os
import cv2
import mediapipe as mp
import numpy as np

# Initialize MediaPipe Pose
mp_pose = mp.solutions.pose
mp_drawing = mp.solutions.drawing_utils

app = Flask(__name__)
pose_detector = mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5)

# Directory where pose files are stored
script_dir = os.path.dirname(os.path.abspath(__file__))  # Get script directory
poses_dir = os.path.join(script_dir, "Poses/Body")  # Poses folder in the same directory

# Load multiple poses
pose_files = {
    "pose1": "level1.npy",
    # "pose2": "pose_level_2.npy",
    # "pose3": "pose_level_3.npy"
}
poses = {}
for pose_name, pose_file in pose_files.items():
    pose_file_path = os.path.join(poses_dir, pose_file)  # Construct full path
    try:
        poses[pose_name] = np.load(pose_file_path)
        print(f"{pose_name} loaded from {pose_file_path}")
    except FileNotFoundError:
        print(f"Error: {pose_file_path} not found!")

def compare_pose(player_pose, reference_pose, threshold=0.1):
    """Compare player's pose with the reference pose."""
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    return np.mean(distance) < threshold

@app.route('/check_pose', methods=['POST'])
def check_pose():
    cap = cv2.VideoCapture(0)  # Access MacBook camera
    ret, frame = cap.read()
    if not ret:
        return jsonify({"status": "Camera Error"})

    frame = cv2.flip(frame, 1)  # Flip horizontally
    rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = pose_detector.process(rgb_frame)

    response = {"status": "No Pose Detected"}
    if results.pose_landmarks:
        player_pose = np.array([[lm.x, lm.y, lm.z] for lm in results.pose_landmarks.landmark])

        # Check against all poses
        for pose_name, reference_pose in poses.items():
            if compare_pose(player_pose, reference_pose):
                response = {"status": "Pose Matched", "pose": pose_name}
                break
        else:
            response = {"status": "Keep Trying"}

    cap.release()
    return jsonify(response)

if __name__ == "__main__":
    app.run(host="127.0.0.1", port=5000)
