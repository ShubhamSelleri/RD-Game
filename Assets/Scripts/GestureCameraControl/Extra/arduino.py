import serial.tools.list_ports

def find_arduino_port():
    """Find the port to which the Arduino is connected."""
    ports = serial.tools.list_ports.comports()
    for port in ports:
        if "usbmodem" in port.device or "usbserial" in port.device:
            print(f"Arduino detected on port: {port.device}")
            return port.device
    print("No Arduino found.")
    return None

def main():
    print("Checking for Arduino connection...")
    arduino_port = find_arduino_port()
    if arduino_port:
        try:
            with serial.Serial(arduino_port, 9600, timeout=1) as arduino:
                print(f"Successfully connected to Arduino on {arduino_port}")
        except serial.SerialException as e:
            print(f"Error connecting to Arduino: {e}")
    else:
        print("Arduino not detected. Make sure it is connected.")

if __name__ == "__main__":
    main()
