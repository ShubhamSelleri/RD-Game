import os
import cv2
import mediapipe as mp
import numpy as np
import time

# Initialize MediaPipe Pose
mp_pose = mp.solutions.pose
mp_drawing = mp.solutions.drawing_utils

def calculate_average_pose(samples):
    """Calculate the average pose from multiple samples."""
    return np.mean(samples, axis=0)

def train_pose(output_filename):
    """Train a pose and save it to a file."""
    cap = cv2.VideoCapture(0)
    pose_samples = []
    reference_pose = None

    # Create the Poses directory if it doesn't exist
    script_dir = os.path.dirname(os.path.abspath(__file__))  # Directory of the script
    poses_dir = os.path.join(script_dir, "Poses/Body")
    os.makedirs(poses_dir, exist_ok=True)

    # Append the directory to the filename
    output_filepath = os.path.join(poses_dir, output_filename)

    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                print("Failed to grab frame")
                break

            # Flip the frame to correct inversion
            frame = cv2.flip(frame, 1)  # Flip horizontally; use 0 to flip vertically

            # Process frame
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = pose.process(rgb_frame)
            frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)

            if results.pose_landmarks:
                mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

                # Countdown and capture 5 samples when 't' is pressed
                if cv2.waitKey(10) & 0xFF == ord('t'):
                    print("Get ready! Capturing 5 poses in:")
                    for i in range(5, 0, -1):  # Countdown from 5 to 1
                        start_time = time.time()
                        while time.time() - start_time < 1:  # Display the countdown for 1 second
                            ret, frame = cap.read()
                            if not ret:
                                break

                            # Flip the frame again inside the countdown
                            frame = cv2.flip(frame, 1)

                            # Display countdown on the frame
                            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                            results = pose.process(rgb_frame)
                            frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)
                            cv2.putText(frame, f"Capturing in {i}...", (50, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)
                            mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
                            cv2.imshow("Pose Trainer", frame)

                            # Exit on 'q'
                            if cv2.waitKey(1) & 0xFF == ord('q'):
                                cap.release()
                                cv2.destroyAllWindows()
                                return

                    print("Capturing 5 consecutive poses...")
                    for j in range(5):  # Capture 5 frames
                        ret, frame = cap.read()
                        if not ret:
                            break

                        # Flip and process each frame
                        frame = cv2.flip(frame, 1)
                        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
                        results = pose.process(rgb_frame)

                        if results.pose_landmarks:
                            landmarks = results.pose_landmarks.landmark
                            keypoints = np.array([[lm.x, lm.y, lm.z] for lm in landmarks])
                            pose_samples.append(keypoints)
                            print(f"Captured pose {j + 1} of 5.")

                        # Display the frame
                        frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)
                        mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)
                        cv2.imshow("Pose Trainer", frame)

                        # Wait 1 second between captures
                        time.sleep(1)

                # Save the average pose when 's' is pressed
                if cv2.waitKey(10) & 0xFF == ord('s') and pose_samples:
                    reference_pose = calculate_average_pose(pose_samples)
                    np.save(output_filepath, reference_pose)
                    print(f"Pose saved to {output_filepath}!")
                    break

            cv2.imshow("Pose Trainer", frame)

            # Exit on 'q'
            if cv2.waitKey(10) & 0xFF == ord('q'):
                break

    cap.release()
    cv2.destroyAllWindows()

def main():
    print("Starting Pose Trainer...")
    output_filename = input("Enter filename to save pose (e.g., 'pose_level_1.npy'): ")
    if not output_filename.endswith('.npy'):
        output_filename += '.npy'
    train_pose(output_filename)

if __name__ == "__main__":
    main()
