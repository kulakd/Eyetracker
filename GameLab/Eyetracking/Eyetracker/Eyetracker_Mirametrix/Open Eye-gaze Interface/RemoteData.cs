using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameLab.Eyetracking.OpenEyeGazeInterface
{
    /// <summary>
    /// The GET and SET commands operate on the remote data specified by the DATA_ID identifier, along with optional parameters.
    /// </summary>
    public class RemoteData
    {
        public struct ENABLE_SEND_COUNTER_STRUCT
        {
            public int CNT; // Sequence counter for data packets
        };

        public struct ENABLE_SEND_TIME_STRUCT
        {
            public float TIME; // Elapsed time in seconds since last system initialization or calibration
        };

        public struct ENABLE_SEND_TIME_TICK_STRUCT
        {
            public long TIME_TICK; // Tick count (signed 64-bit integer)
        };

        public struct ENABLE_SEND_POG_LEFT_STRUCT
        {
            public float LPOGX; // Left point-of-gaze X
            public float LPOGY; // Left point-of-gaze Y
            public int LPOGV; // Left point-of-gaze valid flag
        };

        public struct ENABLE_SEND_POG_RIGHT_STRUCT
        {
            public float RPOGX; // Right point-of-gaze X
            public float RPOGY; // Right point-of-gaze Y
            public int RPOGV; // Right point-of-gaze valid flag
        };

        public struct ENABLE_SEND_POG_BEST_STRUCT
        {
            public float BPOGX; // Best point-of-gaze X
            public float BPOGY; // Best point-of-gaze Y
            public int BPOGV; // Best point-of-gaze valid flag
        };

        public struct ENABLE_SEND_POG_FIX_STRUCT
        {
            public float FPOGX; // Fixation point-of-gaze X
            public float FPOGY; // Fixation point-of-gaze Y
            public float FPOGS; // Fixation start (seconds)
            public float FPOGD; // Fixation duration (elapsed time since fixation start (seconds))
            public int FPOGID; // Fixation number ID
            public int FPOGV; // Fixation point-of-gaze valid flag
        };

        public struct ENABLE_SEND_PUPIL_LEFT_STRUCT
        {
            public float LPCX; // Left eye pupil center X
            public float LPCY; // Left eye pupil center Y
            public float LPD; // Left eye pupil diameter (pixels)
            public float LPS; // Left eye pupil distance (unit less, from calibration position)
            public int LPV; // Left eye pupil image valid
        };

        public struct ENABLE_SEND_PUPIL_RIGHT_STRUCT
        {
            public float RPCX; // Right eye pupil center X
            public float RPCY; // Right eye pupil center Y
            public float RPD; // Right eye pupil diameter (pixels)
            public float RPS; // Right eye pupil distance (unit less, from calibration position)
            public int RPV; // Right eye pupil image valid
        };

        public struct ENABLE_SEND_EYE_LEFT_STRUCT
        {
            public float LEYEX; // Left eye position in X -left/+right (cm)
            public float LEYEY; // Left eye position in Y -down/+up (cm)
            public float LEYEZ; // Left eye position in Z -away/+toward (cm)
            public int LEYEV; // Left eye data valid
            public float LPUPILD; // Left eye pupil diameter (mm)
            public int LPUPILV; // Left pupil data valid 0 - Invalid pupil data; 1 - Valid pupil data; 2 - Valid pupil data, but old position data
        };

        public struct ENABLE_SEND_EYE_RIGHT_STRUCT
        {
            public float REYEX; // Right eye position in X -left/+right (cm)
            public float REYEY; // Right eye position in Y -down/+up (cm)
            public float REYEZ; // Right eye position in Z -away/+toward (cm)
            public int REYEV; // Right eye data valid
            public float RPUPILD; // Right eye pupil diameter (mm)
            public int RPUPILV; // Right pupil data valid 0 - Invalid pupil data; 1 - Valid pupil data; 2 - Valid pupil data, but old position data
        };

        public struct ENABLE_SEND_CURSOR_STRUCT
        {
            public float CX; // Cursor X position
            public float CY; // Cursor Y position
            public int CS; // Cursor button state 0 - No press; 1 - Left button down; 2 - Left button up; 3 - Left double click; 4 - Right button down; 5 - Right button up; 6 - Right double click
        };

        public struct ENABLE_SEND_GPI_STRUCT
        {
            public string GPI1; // GPI? value (where ? is 1 to 10)
            public string GPI2;
            public string GPI3;
            public string GPI4;
            public string GPI5;
            public string GPI6;
            public string GPI7;
            public string GPI8;
            public string GPI9;
            public string GPI10;
        };



        /// <summary>
        /// Enable or disable the sending of the eye-gaze data records. Enabling this variable begins the continuous sending of the eye-gaze data records.
        /// </summary>
        /// <param name="state">Start or stop data streaming</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_DATA(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_DATA", state);
        }

        /// <summary>
        /// Enable or disable the sending of the packet counter. The packet counter is incremented each time a packet is sent from the server. The packet counter provides a means for determining if packets are being lost.
        /// </summary>
        /// <param name="state">Enable counter data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_COUNTER(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_COUNTER", state);
        }

        /// <summary>
        /// Enable or disable the elapsed time variable. The elapsed time variable identifies the elapsed time since the last calibration.
        /// </summary>
        /// <param name="state">Enable time stamp data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_TIME(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_TIME", state);
        }
    
        /// <summary>
        /// Enable or disable the high resolution timer tick. The high resolution timer tick returns the output of the QueryPerformanceCounter (Win32 API) function and can be used to synchronize eye-gaze with other data recorded on the same computer. To convert to seconds divide this value by the output of the TIME_TICK_FREQUENCY variable.
        /// </summary>
        /// <param name="state">Enable high resolution timer tick</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_TIME_TICK(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_TIME_TICK", state);
        }

        /// <summary>
        /// Get the timer tick frequency for high resolution timing. This returns the output of QueryPerformanceFrequency in the Win32 API.
        /// </summary>
        /// <param name="value">Tick frequency (signed 64-bit integer)</param>
        /// <returns>True if success</returns>
        public bool TIME_TICK_FREQUENCY(out long value)
        {
            value = 0;

            var id = "TIME_TICK_FREQUENCY";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "FREQ")
                return false;

            value = Convert.ToInt64(answerParam.ElementAt(0).Value);

            return true;
        }

        /// <summary>
        /// Enable or disable the sending of the point-of-gaze data from the left eye.
        /// </summary>
        /// <param name="state">Enable left eye point-of-gaze data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_POG_LEFT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_POG_LEFT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the point-of-gaze data from the right eye.
        /// </summary>
        /// <param name="state">Enable right eye point-of-gaze data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_POG_RIGHT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_POG_RIGHT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the “best” point-of-gaze data. The “best” POG is simply the average of the left and right POG estimates if both the left and right estimates are valid. If either the left or right POG is invalid, then the “best” POG is equal to the remaining valid POG.
        /// </summary>
        /// <param name="state">Enable best eye point-of-gaze data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_POG_BEST(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_POG_BEST", state);
        }

        /// <summary>
        /// Enable or disable the sending of the fixation point-of-gaze data.
        /// </summary>
        /// <param name="state">Enable fixation point-of-gaze data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_POG_FIX(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_POG_FIX", state);
        }

        /// <summary>
        /// Enable or disable the sending of the left pupil image data.
        /// </summary>
        /// <param name="state">Enable left eye image data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_PUPIL_LEFT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_PUPIL_LEFT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the right pupil image data
        /// </summary>
        /// <param name="state">Enable right eye image data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_PUPIL_RIGHT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_PUPIL_RIGHT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the left eye data.
        /// </summary>
        /// <param name="state">Enable left eye data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_EYE_LEFT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_EYE_LEFT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the right eye data.
        /// </summary>
        /// <param name="state">Enable right eye data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_EYE_RIGHT(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_EYE_RIGHT", state);
        }

        /// <summary>
        /// Enable or disable the sending of the right mouse cursor position.
        /// </summary>
        /// <param name="state">Enable mouse cursor data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_CURSOR(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_CURSOR", state);
        }

        /// <summary>
        /// Enable or disable the sending of up to 10 general purpose input data values which can be used to set user defined values in the eye-gaze data stream. For example, if various stimulus are being displayed, the stimulus ID could be saved into one of the GPI values.
        /// </summary>
        /// <param name="state">Enable general purpose input data</param>
        /// <returns>True if success</returns>
        public bool ENABLE_SEND_GPI(bool state)
        {
            return ENABLE_STATE("ENABLE_SEND_GPI", state);
        }

        /// <summary>
        /// The number of general purpose input data values is set with the GPI_NUMBER parameter
        /// </summary>
        /// <param name="value">Set the number of general input data variables (0 to 10)</param>
        /// <returns>True if success</returns>
        public bool GPI_NUMBER(int value)
        {
            var id = "GPI_NUMBER";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "VALUE")
                return false;

            float valueAnswer = float.Parse(answerParam.ElementAt(0).Value, CultureInfo.InvariantCulture);

            if (valueAnswer == value)
                return true;

            // SET
            var sendParam = new Dictionary<string, string>();
            sendParam.Add(answerParam.ElementAt(0).Key, value.ToString());
            var secondCommand = Command.Create("SET", id, sendParam);
            if (!Connection.Send(secondCommand))
                return false;

            // ACK
            answer = Connection.Receive();

            if (!Command.IsCompatible(secondCommand, answer))
                return false;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "VALUE")
                return false;

            valueAnswer = float.Parse(answerParam.ElementAt(0).Value, CultureInfo.InvariantCulture);

            if (valueAnswer != value)
                return false;

            return true;
        }

        /// <summary>
        /// Get the size of the screen in pixels.
        /// </summary>
        /// <param name="width">Screen width (pixels)</param>
        /// <param name="height">Screen height (pixels)</param>
        /// <returns>True if success</returns>
        public bool SCREEN_SIZE(out int width, out int height)
        {
            width = 0;
            height = 0;

            var id = "SCREEN_SIZE";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 2)
                return false;

            if (answerParam.ElementAt(0).Key != "WIDTH")
                return false;

            width = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (answerParam.ElementAt(1).Key != "HEIGHT")
                return false;

            height = Convert.ToInt32(answerParam.ElementAt(1).Value);

            return true;
        }

        /// <summary>
        /// Get the size of the camera image in pixels.
        /// </summary>
        /// <param name="width">Camera image width (pixels)</param>
        /// <param name="height">Camera image height (pixels)</param>
        /// <returns>True if success</returns>
        public bool CAMERA_SIZE(out int width, out int height)
        {
            width = 0;
            height = 0;

            var id = "CAMERA_SIZE";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 2)
                return false;

            if (answerParam.ElementAt(0).Key != "WIDTH")
                return false;

            width = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (answerParam.ElementAt(1).Key != "HEIGHT")
                return false;

            height = Convert.ToInt32(answerParam.ElementAt(1).Value);

            return true;
        }

        /// <summary>
        /// Get the size of the position and size of the tracking rectangle.
        /// </summary>
        /// <param name="x">X coordinate of tracking rectangle</param>
        /// <param name="y">Y coordinate of tracking rectangle</param>
        /// <param name="width">Width of tracking rectangle</param>
        /// <param name="height">Height of tracking rectangle</param>
        /// <returns>True if success</returns>
        public bool TRACK_RECT(out float x, out float y, out float width, out float height)
        {
            x = 0;
            y = 0;
            width = 0;
            height = 0;

            var id = "TRACK_RECT";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 4)
                return false;

            if (answerParam.ElementAt(0).Key != "X")
                return false;

            x = float.Parse(answerParam.ElementAt(0).Value, CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(1).Key != "Y")
                return false;

            y = float.Parse(answerParam.ElementAt(1).Value, CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(2).Key != "WIDTH")
                return false;

            width = float.Parse(answerParam.ElementAt(2).Value, CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(3).Key != "HEIGHT")
                return false;

            height = float.Parse(answerParam.ElementAt(3).Value, CultureInfo.InvariantCulture);

            return true;
        }

        /// <summary>
        /// Product identifier.
        /// </summary>
        /// <param name="value">Product identifier</param>
        /// <returns>True if success</returns>
        public bool PRODUCT_ID(out string value)
        {
            value = "";

            var id = "PRODUCT_ID";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "VALUE")
                return false;

            value = answerParam.ElementAt(0).Value; 

            return true;
        }

        /// <summary>
        /// Product serial number.
        /// </summary>
        /// <param name="value">Device serial number</param>
        /// <returns>True if success</returns>
        public bool SERIAL_ID(out string value)
        {
            value = "";

            var id = "SERIAL_ID";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "VALUE")
                return false;

            value = answerParam.ElementAt(0).Value;

            return true;
        }

        /// <summary>
        /// Manufacturer identifier.
        /// </summary>
        /// <param name="value">Manufacturer identity</param>
        /// <returns>True if success</returns>
        public bool COMPANY_ID(out string value)
        {
            value = "";

            var id = "COMPANY_ID";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            //Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId))
                return false;

            var startIndex = answer.IndexOf("VALUE=\"");
 
            answer = answer.Substring(startIndex+8);
            answer = answer.Replace(" />", "");

            value = answer;

            return true;
        }

        /// <summary>
        /// API identity.
        /// </summary>
        /// <param name="vendor">API vendor identity</param>
        /// <param name="version">API version number</param>
        /// <returns>True if success</returns>
        public bool API_ID(out string vendor, out string version)
        {
            vendor = "";
            version = "";

            var id = "API_ID";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 2)
                return false;

            if (answerParam.ElementAt(0).Key != "MFG_ID")
                return false;

            vendor = answerParam.ElementAt(0).Value;

            if (answerParam.ElementAt(1).Key != "VER_ID")
                return false;

            version = answerParam.ElementAt(1).Value;

            return true;
        }

        /// <summary>
        /// Select the API to use for further communication.
        /// </summary>
        /// <param name="state">API selected</param>
        /// <param name="vendor">List of vendors supported</param>
        /// <param name="version">List of versions supported</param>
        /// <returns>True if success</returns>
        public bool API_SELECT(int state, out List<string> vendor, out List<string> version)
        {
            vendor = new List<string>();
            version = new List<string>();

            var id = "API_SELECT";
            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count % 2 != 0)
                return false;

            int j = 0;
            for (int i = 0; i < answerParam.Count; i++)
            {
                // MFG_ID
                if (answerParam.ElementAt(i).Key != "MFG_ID" + j)
                    return false;
                vendor.Add(answerParam.ElementAt(i).Value);

                i++;
                // VER_ID
                if (answerParam.ElementAt(i).Key != "VER_ID" + j)
                    return false;
                version.Add(answerParam.ElementAt(i).Value);

                j++;
            }

            // SET
            var sendParam = new Dictionary<string, string>();
            sendParam.Add("STATE", "1");
            var secondCommand = Command.Create("SET", id, sendParam);
            if (!Connection.Send(secondCommand))
                return false;

            // ACK
            answer = Connection.Receive();

            if (!Command.IsCompatible(secondCommand, answer))
                return false;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "STATE")
                return false;

            if (answerParam.ElementAt(0).Value != "1")
                return false;

            return true;
        }

        /// <summary>
        /// Helper method to other methods
        /// </summary>
        /// <param name="id">DataID</param>
        /// <param name="state">Boolean parameter</param>
        /// <returns>True if success</returns>
        private bool ENABLE_STATE(string id, bool state)
        {
            int value = state == true ? 1 : 0;

            var firstCommand = Command.Create("GET", id);

            // GET
            if (!Connection.Send(firstCommand))
                return false;

            // ACK
            var answer = Connection.Receive();

            if (!Command.IsCompatible(firstCommand, answer))
                return false;

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "STATE")
                return false;

            int valueAnswer = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (valueAnswer == value)
                return true;

            // SET
            var sendParam = new Dictionary<string, string>();
            sendParam.Add(answerParam.ElementAt(0).Key, value.ToString());
            var secondCommand = Command.Create("SET", id, sendParam);
            if (!Connection.Send(secondCommand))
                return false;

            // ACK
            answer = Connection.Receive();

            if (!Command.IsCompatible(secondCommand, answer))
                return false;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerParam.Count != 1)
                return false;

            if (answerParam.ElementAt(0).Key != "STATE")
                return false;

            valueAnswer = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (valueAnswer != value)
                return false;

            return true;
        }
    }
}
