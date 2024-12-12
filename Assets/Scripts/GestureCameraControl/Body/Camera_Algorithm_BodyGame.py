import os
import cv2
import mediapipe as mp
import numpy as np

# Initialize MediaPipe Pose
mp_pose = mp.solutions.pose
mp_drawing = mp.solutions.drawing_utils

def compare_pose(player_pose, reference_pose, threshold=0.1):
    """Compare player's pose with the reference pose."""
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    return np.mean(distance) < threshold

def play_game(reference_pose_filename):
    """Run the game with pose matching."""
    # Construct the full path to the pose file in the Poses folder
    script_dir = os.path.dirname(os.path.abspath(__file__))  # Get script directory
    poses_dir = os.path.join(script_dir, "Poses/Body")  # Poses folder in the same directory
    pose_file_path = os.path.join(poses_dir, reference_pose_filename)

    try:
        reference_pose = np.load(pose_file_path)
        print(f"Reference pose loaded from {pose_file_path}")
    except FileNotFoundError:
        print(f"Error: Pose file '{pose_file_path}' not found!")
        return

    cap = cv2.VideoCapture(0)

    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                print("Failed to grab frame")
                break

            # Flip the frame to correct inversion
            frame = cv2.flip(frame, 1)  # Flip horizontally; use 0 for vertical flip if needed

            # Process frame
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = pose.process(rgb_frame)
            frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)

            if results.pose_landmarks:
                mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

                # Compare current pose to reference pose
                player_pose = np.array([[lm.x, lm.y, lm.z] for lm in results.pose_landmarks.landmark])
                if compare_pose(player_pose, reference_pose):
                    print("Pose Matched!")
                else:
                    print("Keep Trying!")

            cv2.imshow("Pose Game", frame)

            # Exit on 'q'
            if cv2.waitKey(10) & 0xFF == ord('q'):
                break

    cap.release()
    cv2.destroyAllWindows()

def main():
    print("Starting Pose Game...")
    reference_pose_filename = input("Enter reference pose filename (e.g., 'pose_level_1'): ")
    if not reference_pose_filename.endswith('.npy'):
        reference_pose_filename += '.npy'
    play_game(reference_pose_filename)

if __name__ == "__main__":
    main()
