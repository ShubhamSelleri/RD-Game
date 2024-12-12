import os
import numpy as np

def merge_gesture_files(files):
    """Merge multiple gesture files into a single reference pose."""
    all_poses = []
    for file in files:
        try:
            pose = np.load(file)
            all_poses.append(pose)
        except FileNotFoundError:
            print(f"Error: File '{file}' not found. Skipping.")
    if not all_poses:
        raise ValueError("No valid pose files were found to merge.")
    merged_pose = np.mean(all_poses, axis=0)  # Average pose
    return merged_pose

def main():
    print("Starting merging files")

    # Get the project root and Poses/Hand directory
    project_root = os.path.dirname(os.path.abspath(__file__))
    poses_dir = os.path.join(project_root, "Poses", "Hand")

    # Define gesture files to merge
    gesture_files = {
        # "Thumbs Up": ["ThumbsUp_1.npy", "ThumbsUp_2.npy", "ThumbsUp_3.npy"],
        "Thumbs Down": ["ThumbsDown.npy", "ThumbsDown2.npy"]
    }

    merged_gestures = {}
    for gesture, files in gesture_files.items():
        # Prepare full file paths for each gesture
        full_paths = [os.path.join(poses_dir, file) for file in files]
        try:
            merged_gestures[gesture] = merge_gesture_files(full_paths)
            print(f"Successfully merged gesture: {gesture}")
        except ValueError as e:
            print(f"Skipping gesture '{gesture}': {e}")

    # Save merged gestures for reuse
    for gesture, pose in merged_gestures.items():
        output_file = os.path.join(poses_dir, f"{gesture}_merged.npy")
        np.save(output_file, pose)
        print(f"Merged gesture saved to: {output_file}")

    print("Ending merging files")

if __name__ == "__main__":
    main()
