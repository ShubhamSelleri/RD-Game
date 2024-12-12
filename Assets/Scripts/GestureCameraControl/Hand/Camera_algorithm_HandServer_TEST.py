# from flask import Flask, jsonify, request
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os

# # Initialize Flask app
# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=2,
#     min_detection_confidence=0.5
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# # gesture_files = {"hand1": "ThumbsDown.npy", 
# #                  "hand2": "hand_pose_2.npy",}
# gesture_files = {"hand1": "ThumbsDown.npy"}
# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# def compare_hand_pose(player_pose, reference_pose, threshold=0.1):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check hand gestures."""
#     cap = cv.VideoCapture(0)
#     ret, frame = cap.read()
#     if not ret:
#         return jsonify({"status": "Camera Error"})

#     frame = cv.flip(frame, 1)
#     rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#     hands_detected = hands.process(rgb_frame)
#     cap.release()

#     response = {"status": "No Gesture"}
#     if hands_detected.multi_hand_landmarks:
#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])
#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     response = {"status": "Gesture Matched", "gesture": gesture_name}
#                     break

#     return jsonify(response)

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check hand gestures."""
#     cap = cv.VideoCapture(0)

#     while cap.isOpened():
#         ret, frame = cap.read()
#         if not ret:
#             return jsonify({"status": "Camera Error"})

#         # Flip and process the frame
#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Prepare response
#         response = {"status": "No Gesture"}
#         if hands_detected.multi_hand_landmarks:
#             for hand_landmarks in hands_detected.multi_hand_landmarks:
#                 player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])
#                 for gesture_name, reference_pose in gestures.items():
#                     if compare_hand_pose(player_hand_pose, reference_pose):
#                         response = {"status": "Gesture Matched", "gesture": gesture_name}
#                         break

#         # Display the video feed with landmarks
#         cv.imshow("Hand Gesture Detection", frame)

#         # Break the loop if 'q' is pressed
#         if cv.waitKey(10) & 0xFF == ord('q'):
#             break

#         # Respond to the client request
#         return jsonify(response)

#     cap.release()
#     cv.destroyAllWindows()




# from flask import Flask, jsonify, request
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os

# # Initialize Flask app
# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=2,
#     min_detection_confidence=0.5
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# # gesture_files = {"hand1": "ThumbsDown.npy", 
# #                  "hand2": "hand_pose_2.npy",}
# gesture_files = {"hand1": "ThumbsDown.npy"}
# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# def compare_hand_pose(player_pose, reference_pose, threshold=0.1):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check hand gestures."""
#     cap = cv.VideoCapture(0)
#     ret, frame = cap.read()
#     if not ret:
#         cap.release()
#         return jsonify({"status": "Camera Error"})

#     # Flip and process the frame
#     frame = cv.flip(frame, 1)
#     rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#     hands_detected = hands.process(rgb_frame)
#     cap.release()

#     # Prepare response
#     response = {"status": "No Gesture"}
#     if hands_detected.multi_hand_landmarks:
#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])
#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     response = {"status": "Gesture Matched", "gesture": gesture_name}
#                     break

#     return jsonify(response)


# if __name__ == "__main__":
#     app.run(host="127.0.0.1", port=5000)




# from flask import Flask, jsonify, request
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import time

# # Initialize Flask app
# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")

# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     "Middle Finger": "middleFinger.npy"
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# # Compare hand pose with a reference pose
# def compare_hand_pose(player_pose, reference_pose, threshold=0.1):
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check hand gestures."""
#     cap = cv.VideoCapture(0)
#     ret, frame = cap.read()
#     if not ret:
#         cap.release()
#         return jsonify({"status": "Camera Error"})

#     # Flip and process the frame
#     frame = cv.flip(frame, 1)
#     rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#     hands_detected = hands.process(rgb_frame)
#     cap.release()

#     # Variables to track gesture match duration
#     global match_start_time, matched_gesture
#     if 'match_start_time' not in globals():
#         match_start_time = None
#     if 'matched_gesture' not in globals():
#         matched_gesture = None

#     required_duration = 2.0  # Seconds to verify a gesture
#     response = {"status": "No Gesture"}

#     if hands_detected.multi_hand_landmarks:
#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])
#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     # If the gesture matches, track the match duration
#                     if matched_gesture != gesture_name:  # New gesture matched
#                         match_start_time = time.time()
#                         matched_gesture = gesture_name
#                         print(f"Gesture '{gesture_name}' Matched! Hold the position...")
#                     elif time.time() - match_start_time >= required_duration:
#                         # Gesture verified
#                         response = {"status": "Gesture Verified", "gesture": gesture_name}
#                         print(f"Gesture '{gesture_name}' Verified!")
#                         return jsonify(response)
#                     break
#             else:
#                 # Reset if no gesture matches
#                 matched_gesture = None
#                 match_start_time = None
#     else:
#         # Reset if no hands detected
#         matched_gesture = None
#         match_start_time = None

#     return jsonify(response)

# if __name__ == "__main__":
#     app.run(host="127.0.0.1", port=5000)





# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# # Initialize Flask app
# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     "Middle Finger": "middleFinger.npy"
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# # Shared state for the latest gesture detection
# latest_gesture = {"status": "No Gesture"}
# lock = threading.Lock()  # To synchronize access to shared data

# # Compare hand pose with a reference pose
# def compare_hand_pose(player_pose, reference_pose, threshold=0.1):
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_and_process():
#     """Continuously capture frames and process gestures."""
#     global latest_gesture

#     # Open the camera
#     cap = cv.VideoCapture(0)

#     # Variables to track gesture match duration
#     match_start_time = None
#     matched_gesture = None
#     required_duration = 2.0  # Seconds to verify a gesture

#     while True:
#         ret, frame = cap.read()
#         if not ret:
#             print("Failed to grab frame")
#             break

#         # Flip the frame to correct inversion
#         frame = cv.flip(frame, 1)

#         # Convert the frame to RGB
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Reset gesture if no hands are detected
#         if not hands_detected.multi_hand_landmarks:
#             matched_gesture = None
#             match_start_time = None
#             with lock:
#                 latest_gesture = {"status": "No Gesture"}
#             continue

#         # Process detected hands
#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])

#             # Compare current hand pose to all reference hand poses
#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     # If the gesture matches, track the match duration
#                     if matched_gesture != gesture_name:  # New gesture matched
#                         match_start_time = time.time()
#                         matched_gesture = gesture_name

#                     elif time.time() - match_start_time >= required_duration:
#                         # Gesture verified
#                         with lock:
#                             latest_gesture = {"status": "Gesture Verified", "gesture": gesture_name}
#                         break
#             else:
#                 # Reset if no gesture matches
#                 matched_gesture = None
#                 match_start_time = None
#                 with lock:
#                     latest_gesture = {"status": "No Gesture"}

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check the latest hand gesture."""
#     with lock:
#         return jsonify(latest_gesture)

# if __name__ == "__main__":
#     # Start the camera processing thread
#     processing_thread = threading.Thread(target=capture_and_process, daemon=True)
#     processing_thread.start()

#     # Start the Flask server
#     app.run(host="127.0.0.1", port=5000)




# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# # Initialize Flask app
# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     "Middle Finger": "middleFinger.npy"
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# # Shared state for the latest gesture detection
# latest_gesture = {"status": "No Gesture"}
# lock = threading.Lock()  # To synchronize access to shared data

# # Compare hand pose with a reference pose
# def compare_hand_pose(player_pose, reference_pose, threshold=0.1):
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_and_process():
#     """Continuously capture frames and process gestures."""
#     global latest_gesture

#     # Open the camera
#     cap = cv.VideoCapture(0)

#     # Variables to track gesture match duration
#     match_start_time = None
#     matched_gesture = None
#     required_duration = 2.0  # Seconds to verify a gesture

#     while True:
#         ret, frame = cap.read()
#         if not ret:
#             print("Failed to grab frame")
#             break

#         # Flip the frame to correct inversion
#         frame = cv.flip(frame, 1)

#         # Convert the frame to RGB
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Reset gesture if no hands are detected
#         if not hands_detected.multi_hand_landmarks:
#             matched_gesture = None
#             match_start_time = None
#             with lock:
#                 latest_gesture = {"status": "No Gesture"}
#             continue

#         # Process detected hands
#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])

#             # Compare current hand pose to all reference hand poses
#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     # If the gesture matches, track the match duration
#                     if matched_gesture != gesture_name:  # New gesture matched
#                         match_start_time = time.time()
#                         matched_gesture = gesture_name

#                     elif time.time() - match_start_time >= required_duration:
#                         # Gesture verified
#                         with lock:
#                             latest_gesture = {"status": "Gesture Verified", "gesture": gesture_name}
#                         break
#             else:
#                 # Reset if no gesture matches
#                 matched_gesture = None
#                 match_start_time = None
#                 with lock:
#                     latest_gesture = {"status": "No Gesture"}

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """Endpoint to check the latest hand gesture."""
#     with lock:
#         return jsonify(latest_gesture)

# if __name__ == "__main__":
#     # Start the camera processing thread
#     processing_thread = threading.Thread(target=capture_and_process, daemon=True)
#     processing_thread.start()

#     # Start the Flask server
#     app.run(host="127.0.0.1", port=5000)




# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# app = Flask(__name__)

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Load hand gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     "Middle Finger": "middleFinger.npy"
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"{gesture_name} loaded from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: {gesture_file} not found!")

# # Shared state for latest gesture detection
# latest_gesture = {"status": "No Gesture"}
# lock = threading.Lock()

# def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_and_process():
#     global latest_gesture

#     cap = cv.VideoCapture(0)
#     match_start_time = None
#     matched_gesture = None
#     required_duration = 2.0

#     while True:
#         ret, frame = cap.read()
#         if not ret:
#             print("Failed to grab frame")
#             break

#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         if not hands_detected.multi_hand_landmarks:
#             # No hands detected
#             matched_gesture = None
#             match_start_time = None
#             with lock:
#                 latest_gesture = {"status": "No Gesture"}
#             continue

#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])

#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     # If gesture matches, track its duration
#                     if matched_gesture != gesture_name:
#                         match_start_time = time.time()
#                         matched_gesture = gesture_name
#                         print(f"Gesture Matched: {gesture_name}. Start holding...")

#                     # Check if gesture is held long enough
#                     elif time.time() - match_start_time >= required_duration:
#                         print(f"Gesture Verified: {gesture_name} (Held for {required_duration} seconds)")
#                         with lock:
#                             latest_gesture = {"status": "Gesture Verified", "gesture": gesture_name}
#                         break
#                     else:
#                         print(f"Holding gesture '{gesture_name}' for {time.time() - match_start_time:.2f} seconds")
#                 else:
#                     # Reset if gesture does not match
#                     matched_gesture = None
#                     match_start_time = None
#                     with lock:
#                         latest_gesture = {"status": "No Gesture"}

#         # Display the frame for debugging
#         cv.imshow("Hand Gesture Debug", frame)

#         # Exit on 'q'
#         if cv.waitKey(10) & 0xFF == ord('q'):
#             break

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     with lock:
#         return jsonify(latest_gesture)

# if __name__ == "__main__":
#     processing_thread = threading.Thread(target=capture_and_process, daemon=True)
#     processing_thread.start()
#     app.run(host="127.0.0.1", port=5000)




# import os
# import cv2 as cv
# import time
# import numpy as np
# import threading
# from flask import Flask, jsonify
# import mediapipe.python.solutions.hands as mp_hands

# app = Flask(__name__)

# # Set TensorFlow log level to suppress warnings
# os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

# # Initialize MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Load gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     "Middle Finger": "middleFinger.npy"
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"Loaded gesture: {gesture_name} from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: Gesture file '{gesture_file}' not found!")

# # Shared state for gesture recognition
# latest_gesture = {"status": "No Gesture"}
# lock = threading.Lock()

# def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_and_process():
#     """Continuously process frames for hand gesture detection."""
#     global latest_gesture

#     cap = cv.VideoCapture(0)
#     match_start_time = None
#     matched_gesture = None
#     required_duration = 2.0  # Hold gesture for 2 seconds

#     while True:
#         ret, frame = cap.read()
#         if not ret:
#             print("Failed to grab frame")
#             break

#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         if not hands_detected.multi_hand_landmarks:
#             # Reset if no hands are detected
#             matched_gesture = None
#             match_start_time = None
#             with lock:
#                 latest_gesture = {"status": "No Gesture"}
#             continue

#         for hand_landmarks in hands_detected.multi_hand_landmarks:
#             player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmarks.landmark])

#             for gesture_name, reference_pose in gestures.items():
#                 if compare_hand_pose(player_hand_pose, reference_pose):
#                     # Track match duration
#                     if matched_gesture != gesture_name:
#                         match_start_time = time.time()
#                         matched_gesture = gesture_name
#                         print(f"Gesture Matched: {gesture_name}. Start holding...")

#                     elif time.time() - match_start_time >= required_duration:
#                         print(f"Gesture Verified: {gesture_name} (Held for {required_duration} seconds)")
#                         with lock:
#                             latest_gesture = {"status": "Gesture Verified", "gesture": gesture_name}
#                         break
#                     else:
#                         print(f"Holding gesture '{gesture_name}' for {time.time() - match_start_time:.2f} seconds")
#                 else:
#                     matched_gesture = None
#                     match_start_time = None
#                     with lock:
#                         latest_gesture = {"status": "No Gesture"}

#         # Save frame for debugging (Optional)
#         cv.imwrite(f"debug_frame_{int(time.time())}.png", frame)

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """API endpoint to retrieve the latest gesture."""
#     with lock:
#         return jsonify(latest_gesture)

# if __name__ == "__main__":
#     processing_thread = threading.Thread(target=capture_and_process, daemon=True)
#     processing_thread.start()
#     app.run(host="127.0.0.1", port=5000)




# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# # Flask app
# app = Flask(__name__)

# # MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown.npy",
#     # "Middle Finger": "middleFinger.npy",
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"Loaded gesture: {gesture_name} from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: Gesture file '{gesture_file}' not found!")

# # Global variables for shared state
# latest_frame = None
# latest_hand_landmarks = None
# state_lock = threading.Lock()
# capture_running = True
# state = {
#     "matched_gesture": None,
#     "match_start_time": None
# }
# required_duration = 1.0  # Seconds

# def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_frames():
#     """Continuously capture frames from the camera."""
#     global latest_frame, latest_hand_landmarks, capture_running

#     cap = cv.VideoCapture(0)
#     while capture_running:
#         ret, frame = cap.read()
#         if not ret:
#             continue

#         # Flip and process the frame
#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Store the frame and landmarks in shared state
#         with state_lock:
#             latest_frame = frame
#             latest_hand_landmarks = hands_detected.multi_hand_landmarks

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """API endpoint to detect and validate hand gestures."""
#     global state

#     # Access the latest frame and landmarks
#     with state_lock:
#         frame = latest_frame
#         hand_landmarks = latest_hand_landmarks

#     if frame is None or hand_landmarks is None:
#         # Reset state if no hands are detected
#         state["matched_gesture"] = None
#         state["match_start_time"] = None
#         return jsonify({"status": "No Gesture"})

#     for hand_landmark in hand_landmarks:
#         player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])

#         for gesture_name, reference_pose in gestures.items():
#             if compare_hand_pose(player_hand_pose, reference_pose):
#                 if state["matched_gesture"] != gesture_name:
#                     # New gesture detected
#                     state["matched_gesture"] = gesture_name
#                     state["match_start_time"] = time.time()
#                     return jsonify({"status": "Gesture Matched", "gesture": gesture_name, "holding": 0})
#                 else:
#                     # Gesture is still being held
#                     holding_duration = time.time() - state["match_start_time"]
#                     if holding_duration >= required_duration:
#                         return jsonify({"status": "Gesture Verified", "gesture": gesture_name})
#                     else:
#                         return jsonify({"status": "Gesture Matched", "gesture": gesture_name, "holding": holding_duration})

#     # Reset state if no gesture matches
#     state["matched_gesture"] = None
#     state["match_start_time"] = None
#     return jsonify({"status": "No Gesture"})

# if __name__ == "__main__":
#     # Start the frame capture thread
#     capture_thread = threading.Thread(target=capture_frames, daemon=True)
#     capture_thread.start()

#     try:
#         app.run(host="127.0.0.1", port=5000)
#     finally:
#         # Stop the frame capture thread
#         capture_running = False
#         capture_thread.join()


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
    "Middle Finger": "middleFinger.npy",
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
    "match_start_time": None,
    "last_response_time": 0,
}
response_interval = 0.5
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



# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# # Flask app
# app = Flask(__name__)

# # MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     # "Thumbs Down": "ThumbsDown.npy",
#     "Thumbs Down": "ThumbsDown_merged.npy",
#     "Middle Finger": "middleFinger.npy",
# }

# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"Loaded gesture: {gesture_name} from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: Gesture file '{gesture_file}' not found!")

# # Global variables for shared state
# latest_frame = None
# latest_hand_landmarks = None
# state_lock = threading.Lock()
# capture_running = True
# state = {
#     "matched_gesture": None,
#     "match_start_time": None,
#     "last_response_time": 0,
# }
# response_interval = 0.5
# required_duration = 1.0  # Seconds

# def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     return np.mean(distance) < threshold

# def capture_frames():
#     """Continuously capture frames from the camera."""
#     global latest_frame, latest_hand_landmarks, capture_running

#     cap = cv.VideoCapture(0)
#     while capture_running:
#         ret, frame = cap.read()
#         if not ret:
#             continue

#         # Flip and process the frame
#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Store the frame and landmarks in shared state
#         with state_lock:
#             latest_frame = frame
#             latest_hand_landmarks = hands_detected.multi_hand_landmarks

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """API endpoint to detect and validate hand gestures."""
#     global state

#     # Access the latest frame and landmarks
#     with state_lock:
#         frame = latest_frame
#         hand_landmarks = latest_hand_landmarks

#     if frame is None or hand_landmarks is None:
#         # Reset state if no hands are detected
#         state["matched_gesture"] = None
#         state["match_start_time"] = None
#         return throttle_response({"status": "No Gesture"})

#     for hand_landmark in hand_landmarks:
#         player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])

#         for gesture_name, reference_pose in gestures.items():
#             if compare_hand_pose(player_hand_pose, reference_pose):
#                 if state["matched_gesture"] != gesture_name:
#                     # New gesture detected
#                     state["matched_gesture"] = gesture_name
#                     state["match_start_time"] = time.time()
#                     return throttle_response({"status": "Gesture Matched", "gesture": gesture_name})
#                 else:
#                     # Gesture is still being held
#                     holding_duration = time.time() - state["match_start_time"]
#                     if holding_duration >= required_duration:
#                         state["matched_gesture"] = None  # Reset after verifying
#                         state["match_start_time"] = None
#                         return throttle_response({"status": "Gesture Verified", "gesture": gesture_name})
#                     else:
#                         return throttle_response({"status": "Gesture Matched", "gesture": gesture_name, "holding": holding_duration})

#     # Reset state if no gesture matches
#     state["matched_gesture"] = None
#     state["match_start_time"] = None
#     return throttle_response({"status": "No Gesture"})

# def throttle_response(response):
#     """Ensure responses are sent at a limited rate to avoid spamming."""
#     current_time = time.time()
#     time_since_last_response = current_time - state["last_response_time"]

#     if time_since_last_response >= response_interval:
#         state["last_response_time"] = current_time
#         return jsonify(response)
#     return jsonify({"status": "No Change"})  # Avoid flooding Unity with redundant data


# if __name__ == "__main__":
#     # Start the frame capture thread
#     capture_thread = threading.Thread(target=capture_frames, daemon=True)
#     capture_thread.start()

#     try:
#         app.run(host="127.0.0.1", port=5000)
#     finally:
#         # Stop the frame capture thread
#         capture_running = False
#         capture_thread.join()




# from flask import Flask, jsonify
# import cv2 as cv
# import mediapipe.python.solutions.hands as mp_hands
# import numpy as np
# import os
# import threading
# import time

# # Flask app
# app = Flask(__name__)

# # MediaPipe Hands
# hands = mp_hands.Hands(
#     static_image_mode=False,
#     max_num_hands=1,
#     min_detection_confidence=0.7
# )

# # Gesture files
# script_dir = os.path.dirname(os.path.abspath(__file__))
# poses_dir = os.path.join(script_dir, "Poses/Hand")
# gesture_files = {
#     "Thumbs Up": "ThumbsUp.npy",
#     "Thumbs Down": "ThumbsDown_merged.npy",
# }

# # Load gestures
# gestures = {}
# for gesture_name, gesture_file in gesture_files.items():
#     try:
#         gesture_path = os.path.join(poses_dir, gesture_file)
#         gestures[gesture_name] = np.load(gesture_path)
#         print(f"Loaded gesture: {gesture_name} from {gesture_path}")
#     except FileNotFoundError:
#         print(f"Error: Gesture file '{gesture_file}' not found!")

# # Global variables for shared state
# latest_frame = None
# latest_hand_landmarks = None
# state_lock = threading.Lock()
# capture_running = True
# state = {
#     "matched_gesture": None,
#     "match_start_time": None,
# }
# required_duration = 1.0  # Seconds

# def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
#     """Compare player's hand pose with the reference pose."""
#     distance = np.linalg.norm(player_pose - reference_pose, axis=1)
#     mean_distance = np.mean(distance)
#     print(f"Gesture Comparison: Mean Distance = {mean_distance}, Threshold = {threshold}")
#     return mean_distance < threshold

# def capture_frames():
#     """Continuously capture frames from the camera."""
#     global latest_frame, latest_hand_landmarks, capture_running

#     cap = cv.VideoCapture(0)
#     while capture_running:
#         ret, frame = cap.read()
#         if not ret:
#             continue

#         # Flip and process the frame
#         frame = cv.flip(frame, 1)
#         rgb_frame = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
#         hands_detected = hands.process(rgb_frame)

#         # Store the frame and landmarks in shared state
#         with state_lock:
#             latest_frame = frame
#             latest_hand_landmarks = hands_detected.multi_hand_landmarks

#     cap.release()

# @app.route('/check_gesture', methods=['POST'])
# def check_gesture():
#     """API endpoint to detect and validate hand gestures."""
#     global state

#     # Access the latest frame and landmarks
#     with state_lock:
#         frame = latest_frame
#         hand_landmarks = latest_hand_landmarks

#     if frame is None or hand_landmarks is None:
#         print("No hands detected.")
#         state["matched_gesture"] = None
#         state["match_start_time"] = None
#         return "", 204  # No Content

#     for hand_landmark in hand_landmarks:
#         player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])
#         print(f"Detected Hand Pose: {player_hand_pose}")

#         for gesture_name, reference_pose in gestures.items():
#             if compare_hand_pose(player_hand_pose, reference_pose):
#                 if state["matched_gesture"] != gesture_name:
#                     # New gesture detected
#                     print(f"New Gesture Detected: {gesture_name}")
#                     state["matched_gesture"] = gesture_name
#                     state["match_start_time"] = time.time()
#                 else:
#                     # Gesture is still being held
#                     holding_duration = time.time() - state["match_start_time"]
#                     if holding_duration >= required_duration:
#                         print(f"Gesture Verified: {gesture_name}")
#                         return jsonify({"status": "Gesture Verified", "gesture": gesture_name})
#     return "", 204  # No Content

# if __name__ == "__main__":
#     # Start the frame capture thread
#     capture_thread = threading.Thread(target=capture_frames, daemon=True)
#     capture_thread.start()

#     try:
#         app.run(host="127.0.0.1", port=5000)
#     finally:
#         # Stop the frame capture thread
#         capture_running = False
#         capture_thread.join()



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
    "Thumbs Down": "ThumbsDown_merged.npy",
    "Middle Finger": "middleFinger.npy",
}

# Load gestures
gestures = {}
for gesture_name, gesture_file in gesture_files.items():
    try:
        gesture_path = os.path.join(poses_dir, gesture_file)
        gestures[gesture_name] = np.load(gesture_path)
    except FileNotFoundError:
        pass  # Skip missing gesture files

# Global variables for shared state
latest_frame = None
latest_hand_landmarks = None
state_lock = threading.Lock()
capture_running = True
state = {
    "matched_gesture": None,
    "match_start_time": None,
}
required_duration = 0.5  # Seconds

def compare_hand_pose(player_pose, reference_pose, threshold=0.2):
    """Compare player's hand pose with the reference pose."""
    distance = np.linalg.norm(player_pose - reference_pose, axis=1)
    mean_distance = np.mean(distance)
    return mean_distance < threshold

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
        state["matched_gesture"] = None
        state["match_start_time"] = None
        return "", 204  # No Content

    for hand_landmark in hand_landmarks:
        player_hand_pose = np.array([[lm.x, lm.y, lm.z] for lm in hand_landmark.landmark])

        for gesture_name, reference_pose in gestures.items():
            if compare_hand_pose(player_hand_pose, reference_pose):
                if state["matched_gesture"] != gesture_name:
                    # New gesture detected
                    state["matched_gesture"] = gesture_name
                    state["match_start_time"] = time.time()
                else:
                    # Gesture is still being held
                    holding_duration = time.time() - state["match_start_time"]
                    if holding_duration >= required_duration:
                        return jsonify({"status": "Gesture Verified", "gesture": gesture_name})
    return "", 204  # No Content

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