import cv2
import mediapipe as mp
import numpy as np
import json

# Initialize MediaPipe Pose
mp_pose = mp.solutions.pose
mp_drawing = mp.solutions.drawing_utils


def train_pose(output_file="reference_pose.json"):
    """
    Function to train a pose. Captures multiple samples, calculates the average pose,
    and saves it to a file.
    """
    print("Training mode activated. Press 't' to capture a sample, 's' to save the reference pose, or 'q' to quit.")
    
    pose_samples = []
    cap = cv2.VideoCapture(0)

    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                print("Failed to grab frame")
                break

            # Convert the frame to RGB
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            # Process the frame
            results = pose.process(rgb_frame)
            frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)

            # Draw pose landmarks
            if results.pose_landmarks:
                mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

            # Display the frame
            cv2.imshow("Pose Trainer", frame)

            # Capture keypoints when 't' is pressed
            key = cv2.waitKey(10) & 0xFF
            if key == ord('t'):
                if results.pose_landmarks:
                    landmarks = results.pose_landmarks.landmark
                    keypoints = np.array([[lm.x, lm.y, lm.z] for lm in landmarks])
                    pose_samples.append(keypoints)
                    print(f"Sample {len(pose_samples)} captured!")

            # Save the reference pose when 's' is pressed
            elif key == ord('s') and pose_samples:
                reference_pose = np.mean(pose_samples, axis=0).tolist()  # Average pose
                with open(output_file, "w") as f:
                    json.dump(reference_pose, f)
                print(f"Reference pose saved to {output_file}")
                break

            # Quit on 'q'
            elif key == ord('q'):
                print("Exiting training mode.")
                break

    cap.release()
    cv2.destroyAllWindows()


def play_game(reference_file="reference_pose.json", threshold=0.1):
    """
    Function to play the game. Loads the reference pose and compares it with the player's pose in real-time.
    """
    print("Game mode activated. Press 'q' to quit.")

    # Load reference pose
    try:
        with open(reference_file, "r") as f:
            reference_pose = np.array(json.load(f))
        print(f"Reference pose loaded from {reference_file}")
    except FileNotFoundError:
        print(f"Reference pose file '{reference_file}' not found. Please train a pose first.")
        return

    cap = cv2.VideoCapture(0)

    with mp_pose.Pose(min_detection_confidence=0.5, min_tracking_confidence=0.5) as pose:
        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                print("Failed to grab frame")
                break

            # Convert the frame to RGB
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            # Process the frame
            results = pose.process(rgb_frame)
            frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)

            # Draw pose landmarks
            if results.pose_landmarks:
                mp_drawing.draw_landmarks(frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS)

                # Compare player's pose with the reference pose
                player_pose = np.array([[lm.x, lm.y, lm.z] for lm in results.pose_landmarks.landmark])
                distance = np.linalg.norm(player_pose - reference_pose, axis=1)
                if np.mean(distance) < threshold:
                    print("Pose matched! Good job!")
                else:
                    print("Keep trying!")

            # Display the frame
            cv2.imshow("Pose Game", frame)

            # Quit on 'q'
            if cv2.waitKey(10) & 0xFF == ord('q'):
                print("Exiting game mode.")
                break

    cap.release()
    cv2.destroyAllWindows()


def main():
    """
    Main function to choose between training or gameplay modes.
    """
    print("Welcome! Choose an option:")
    print("1: Train a new pose")
    print("2: Play the game")
    print("q: Quit")

    choice = input("Enter your choice: ").strip()
    if choice == "1":
        train_pose()
    elif choice == "2":
        play_game()
    elif choice.lower() == "q":
        print("Goodbye!")
    else:
        print("Invalid choice. Please try again.")


if __name__ == "__main__":
    main()