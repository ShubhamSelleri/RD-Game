import os
import cv2 as cv
import mediapipe.python.solutions.hands as mp_hands
import mediapipe.python.solutions.drawing_utils as drawing
import mediapipe.python.solutions.drawing_styles as drawing_styles
import numpy as np
import time

totalTimer = 3
totalFrames = 5

def calculate_average_pose(samples):
    """Calculate the average pose from multiple samples."""
    return np.mean(samples, axis=0)

def train_hand_pose(output_filename):
    """Train a hand pose and save it to a file."""
    # Create the Poses directory if it doesn't exist
    script_dir = os.path.dirname(os.path.abspath(__file__))  # Directory of the script
    poses_dir = os.path.join(script_dir, "Poses/Hand")  # Poses folder in the same directory
    os.makedirs(poses_dir, exist_ok=True)

    # Append the directory to the filename
    output_filepath = os.path.join(poses_dir, output_filename)

    # Initialize the Hands model
    hands = mp_hands.Hands(
        static_image_mode=False,  # Set to False for processing video frames
        max_num_hands=2,          # Maximum number of hands to detect
        min_detection_confidence=0.5  # Minimum confidence threshold for hand detection
    )

    # Open the camera
    cam = cv.VideoCapture(0)

    hand_samples = []

    while cam.isOpened():
        success, frame = cam.read()

        if not success:
            print("Camera Frame not available")
            continue

        # Flip the frame for a mirror effect
        frame = cv.flip(frame, 1)

        # Convert the frame from BGR to RGB
        frame_rgb = cv.cvtColor(frame, cv.COLOR_BGR2RGB)

        # Process the frame for hand detection and tracking
        hands_detected = hands.process(frame_rgb)

        # Convert the frame back to BGR
        frame = cv.cvtColor(frame_rgb, cv.COLOR_RGB2BGR)

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

        # Display the frame
        cv.imshow("Hand Pose Trainer", frame)

        # Start capturing samples when 't' is pressed
        if cv.waitKey(10) & 0xFF == ord('t'):
            print(f"Get ready! Capturing {totalFrames} hand poses in:")
            for i in range(totalTimer, 0, -1):  # Countdown from 5 to 1
                start_time = time.time()
                while time.time() - start_time < 1:
                    success, frame = cam.read()
                    if not success:
                        continue

                    frame = cv.flip(frame, 1)
                    frame_rgb = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
                    hands_detected = hands.process(frame_rgb)
                    frame = cv.cvtColor(frame_rgb, cv.COLOR_RGB2BGR)

                    # Display countdown
                    cv.putText(frame, f"Capturing in {i}...", (50, 50), cv.FONT_HERSHEY_SIMPLEX, 1, (0, 0, 255), 2)
                    cv.imshow("Hand Pose Trainer", frame)

                    if cv.waitKey(1) & 0xFF == ord('q'):
                        cam.release()
                        cv.destroyAllWindows()
                        return

            print(f"Capturing {totalFrames} consecutive hand poses...")
            for j in range(totalFrames):  # Capture 5 frames
                success, frame = cam.read()
                if not success:
                    continue

                frame = cv.flip(frame, 1)
                frame_rgb = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
                hands_detected = hands.process(frame_rgb)

                if hands_detected.multi_hand_landmarks:
                    for hand_landmarks in hands_detected.multi_hand_landmarks:
                        keypoints = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])
                        hand_samples.append(keypoints)
                        print(f"Captured hand pose {j + 1} of 5.")

                # Wait 1 second between captures
                time.sleep(1)

        # Save the average pose when 's' is pressed
        if cv.waitKey(10) & 0xFF == ord('s') and hand_samples:
            reference_pose = calculate_average_pose(hand_samples)
            np.save(output_filepath, reference_pose)
            print(f"Hand pose saved to {output_filepath}!")
            break

        # Exit on 'q'
        if cv.waitKey(10) & 0xFF == ord('q'):
            break

    cam.release()
    cv.destroyAllWindows()

def main():
    print("Starting Hand Pose Trainer...")
    output_filename = input("Enter filename to save hand pose (e.g., 'hand_pose_1.npy'): ")
    if not output_filename.endswith('.npy'):
        output_filename += '.npy'
    train_hand_pose(output_filename)

if __name__ == "__main__":
    main()



