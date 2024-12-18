import os
import cv2
import mediapipe.python.solutions.hands as mp_hands
import mediapipe.python.solutions.drawing_utils as drawing
import mediapipe.python.solutions.drawing_styles as drawing_styles
import numpy as np
import time


def compare_hand_pose(player_pose, reference_pose, threshold=0.15):
    """Compare player's hand pose with the reference pose."""
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    return np.mean(distance) < threshold


def play_hand_game(gesture_files, required_duration=2.0):
    """Run the game with hand gesture matching."""
    # Load all gestures into memory
    script_dir = os.path.dirname(os.path.abspath(__file__))  # Get script directory
    poses_dir = os.path.join(script_dir, "Poses/Hand")  # Poses folder in the same directory
    gestures = {}
    for gesture_name, filename in gesture_files.items():
        file_path = os.path.join(poses_dir, filename)
        try:
            gestures[gesture_name] = np.load(file_path)
            print(f"Loaded gesture '{gesture_name}' from {file_path}")
        except FileNotFoundError:
            print(f"Error: Gesture file '{file_path}' not found!")

    # Initialize the Hands model
    hands = mp_hands.Hands(
        static_image_mode=False,  # Set to False for video frames
        max_num_hands=1,          # Detect up to 1 hand
        min_detection_confidence=0.8,  # Higher detection confidence
        min_tracking_confidence=0.8    # Higher tracking confidence
    )

    cap = cv2.VideoCapture(0)

    # Variables to track gesture match duration
    match_start_time = None
    matched_gesture = None

    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            print("Failed to grab frame")
            break

        frame_height, frame_width, _ = frame.shape

        # Flip the frame to correct inversion
        frame = cv2.flip(frame, 1)

        # Convert the frame to RGB
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        hands_detected = hands.process(rgb_frame)

        # Convert the frame back to BGR
        frame = cv2.cvtColor(rgb_frame, cv2.COLOR_RGB2BGR)

        # Draw landmarks if hands are detected
        if hands_detected.multi_hand_landmarks:
            for hand_landmarks in hands_detected.multi_hand_landmarks:
                drawing.draw_landmarks(
                    frame,
                    hand_landmarks,
                    mp_hands.HAND_CONNECTIONS,
                    drawing_styles.get_default_hand_landmarks_style(),
                    drawing_styles.get_default_hand_connections_style(),
                )

                # Get raw landmark data (no normalization)
                player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])

                # Compare current hand pose to all reference hand poses
                for gesture_name, reference_pose in gestures.items():
                    if compare_hand_pose(player_hand_pose, reference_pose):
                        # If the gesture matches, track the match duration
                        if matched_gesture != gesture_name:  # New gesture matched
                            match_start_time = time.time()
                            matched_gesture = gesture_name
                            print(f"Gesture '{gesture_name}' Matched! Hold the position...")

                        # Check if the gesture is held for the required duration
                        elif time.time() - match_start_time >= required_duration:
                            print(f"Gesture '{gesture_name}' Verified!")
                            cv2.putText(
                                frame, f"{gesture_name} Verified!",
                                (50, 100), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2
                            )
                            break
                        break
                else:
                    # Reset if no gesture matches
                    matched_gesture = None
                    match_start_time = None

        # Provide instructions on the frame
        cv2.putText(
            frame, "Keep hand centered in the frame for better detection",
            (50, frame_height - 50), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2
        )

        # Display the frame
        cv2.imshow("Hand Gesture Game", frame)

        # Exit on 'q'
        if cv2.waitKey(10) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()


def main():
    print("Starting Hand Gesture Game...")

    # Define the gesture files
    gesture_files = {
        "Thumbs Up": "ThumbsUp.npy",
        "Thumbs Down": "ThumbsDown.npy",
        "Middle Finger": "MiddleFinger.npy",
        "Rock": "Rock.npy",
    }

    # Set the required duration for the gesture to be held
    # required_duration = 1.0
    required_duration = 0.5
    play_hand_game(gesture_files, required_duration)


if __name__ == "__main__":
    main()
