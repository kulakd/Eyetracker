using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RafałLinowiecki.OpenEyeGazeInterface
{
    /// <summary>
    ///  Objects to calibrate process
    /// </summary>
    public class Calibration
    {
        public struct CALIB_PT_STRUCT
        {
            public int PT; // Calibration point started/completed
            public float CALX; // Calibration X coordinate for PT
            public float CALY; // Calibration Y coordinate for PT
        };

        public struct CALIB_RESULT_STRUCT
        {
            public float CALX; // Calibration X coordinate for point ?
            public float CALY; // Calibration Y coordinate for point ?
            public float LX; // Left eye point-of-gaze X for point ?
            public float LY; // Left eye point-of-gaze Y for point ?
            public int LV; // Left eye valid flag for point ?
            public float RX; // Right eye point-of-gaze X for point ?
            public float RY; // Right eye point-of-gaze Y for point ?
            public int RV; // Right eye valid flag for point ?
        };

        public struct CALIB_RESULT_SUMMARY_STRUCT
        {
            public float AVE_ERROR; // Average error in pixels over all calibration points
            public int VALID_POINTS; // Number of calibration points with valid data
        };

        /// <summary>
        /// Start or stop the calibration procedure.
        /// </summary>
        /// <returns>True if success</returns>
        public bool CALIBRATE_START()
        {
            var id = "CALIBRATE_START";
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

            if (answerParam.ElementAt(0).Value != "0")
            {
                if (answerParam.ElementAt(0).Value == "1")
                    return true;
                else
                    return false;
            }

            // SET
            var sendParam = new Dictionary<string, string>();
            sendParam.Add(answerParam.ElementAt(0).Key, "1");
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
        /// Show or hide the calibration window.
        /// </summary>
        /// <returns>True if success</returns>
        public bool CALIBRATE_SHOW()
        {
            var id = "CALIBRATE_SHOW";
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

            if (answerParam.ElementAt(0).Value != "0")
            {
                if (answerParam.ElementAt(0).Value == "1")
                    return true;
                else
                    return false;
            }

            // SET
            var sendParam = new Dictionary<string, string>();
            sendParam.Add(answerParam.ElementAt(0).Key, "1");
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
        /// Set the delay before a calibration point in started seconds, which provides time for calibration point animation.
        /// </summary>
        /// <param name="value">delay before a calibration point in started seconds</param>
        /// <returns>True if success</returns>
        public bool CALIBRATE_DELAY(float value)
        {
            var id = "CALIBRATE_DELAY";
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
        /// Set the maximum duration of a calibration point in seconds after the calibration delay
        /// </summary>
        /// <param name="value">maximum duration of a calibration point in seconds after the calibration delay</param>
        /// <returns>True if success</returns>
        public bool CALIBRATE_TIMEOUT(float value)
        {
            var id = "CALIBRATE_TIMEOUT";
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
        /// If enabled the calibration will jump to the next point as soon as sufficient data has been collected at the current calibration point, rather than waiting until the completion of the CALIBRATE_TIMEOUT value.
        /// </summary>
        /// <param name="state">Enable fast calibration</param>
        /// <returns>True if CALIBRATE_FAST is enabled</returns>
        public bool CALIBRATE_FAST(bool state)
        {
            int value = state == true ? 1 : 0;

            var id = "CALIBRATE_FAST";
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

        /// <summary>
        /// While calibrating, information about the status of each calibration point is transmit. At the start of each calibration point, the CALIB_START_PT record is sent along with the X and Y coordinates of the target point. After the completion the calibration point the CALIB_RESULT_PT record is sent, immediately followed by the subsequent CALIB_START_PT record for the next calibration point.
        /// </summary>
        /// <param name="calibPtStruct">Struct of calibration point</param>
        /// <returns>True is success</returns>
        public bool CALIB_START_PT(out CALIB_PT_STRUCT calibPtStruct)
        {
            var answer = Connection.Receive();
            calibPtStruct = new CALIB_PT_STRUCT();

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerCmd != "CAL")
                return false;

            if (answerId != "CALIB_START_PT")
                return false;

            if (answerParam.Count != 3)
                return false;

            if (answerParam.ElementAt(0).Key != "PT")
                return false;
            calibPtStruct.PT = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (answerParam.ElementAt(1).Key != "CALX")
                return false;
            calibPtStruct.CALX = float.Parse(answerParam.ElementAt(1).Value,
                CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(2).Key != "CALY")
                return false;
            calibPtStruct.CALY = float.Parse(answerParam.ElementAt(2).Value,
                CultureInfo.InvariantCulture);

            return true;
        }

        /// <summary>
        /// While calibrating, information about the status of each calibration point is transmit. At the start of each calibration point, the CALIB_START_PT record is sent along with the X and Y coordinates of the target point. After the completion the calibration point the CALIB_RESULT_PT record is sent, immediately followed by the subsequent CALIB_START_PT record for the next calibration point.
        /// </summary>
        /// <param name="calibPtStruct">Struct of calibration point</param>
        /// <returns>True is success</returns>
        public bool CALIB_RESULT_PT(out CALIB_PT_STRUCT calibPtStruct)
        {
            var answer = Connection.Receive();
            calibPtStruct = new CALIB_PT_STRUCT();

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerCmd != "CAL")
                return false;

            if (answerId != "CALIB_RESULT_PT")
                return false;

            if (answerParam.Count != 3)
                return false;

            if (answerParam.ElementAt(0).Key != "PT")
                return false;
            calibPtStruct.PT = Convert.ToInt32(answerParam.ElementAt(0).Value);

            if (answerParam.ElementAt(1).Key != "CALX")
                return false;
            calibPtStruct.CALX = float.Parse(answerParam.ElementAt(1).Value,
                CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(2).Key != "CALY")
                return false;
            calibPtStruct.CALY = float.Parse(answerParam.ElementAt(2).Value,
                CultureInfo.InvariantCulture);

            return true;
        }

        /// <summary>
        /// When the calibration procedure completes, the results for each point are collected in an XML string and transmit. An example of the calibration return data is shown below for a single point.
        /// </summary>
        /// <param name="calibResultStruct">Struct of calibration point</param>
        /// <returns>True is success</returns>
        public bool CALIB_RESULT(out List<CALIB_RESULT_STRUCT> calibResultStruct)
        {
            var answer = Connection.Receive();
            calibResultStruct = new List<CALIB_RESULT_STRUCT>();

            string answerCmd;
            string answerId;
            Dictionary<string, string> answerParam;

            if (!Command.Recreate(answer, out answerCmd, out answerId, out answerParam))
                return false;

            if (answerCmd != "CAL")
                return false;

            if (answerId != "CALIB_RESULT")
                return false;

            if (answerParam.Count%8 != 0)
                return false;

            var tmpStruct = new CALIB_RESULT_STRUCT();
            int j = 1;

            for (int i = 0; i < answerParam.Count; i++)
            {
                // CALX?
                if (answerParam.ElementAt(i).Key != "CALX" + j)
                    return false;
                tmpStruct.CALX = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // CALY?
                if (answerParam.ElementAt(i).Key != "CALY" + j)
                    return false;
                tmpStruct.CALY = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // LX?
                if (answerParam.ElementAt(i).Key != "LX" + j)
                    return false;
                tmpStruct.LX = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // LY?
                if (answerParam.ElementAt(i).Key != "LY" + j)
                    return false;
                tmpStruct.LY = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // LV?
                if (answerParam.ElementAt(i).Key != "LV" + j)
                    return false;
                tmpStruct.LV = Convert.ToInt32(answerParam.ElementAt(i).Value);

                i++;
                // RX?
                if (answerParam.ElementAt(i).Key != "RX" + j)
                    return false;
                tmpStruct.RX = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // RY?
                if (answerParam.ElementAt(i).Key != "RY" + j)
                    return false;
                tmpStruct.RY = float.Parse(answerParam.ElementAt(i).Value,
                    CultureInfo.InvariantCulture);

                i++;
                // RV?
                if (answerParam.ElementAt(i).Key != "RV" + j)
                    return false;
                tmpStruct.RV = Convert.ToInt32(answerParam.ElementAt(i).Value);

                j++;
                calibResultStruct.Add(tmpStruct);
            }
            return true;
        }

        /// <summary>
        /// When the calibration procedure completes, a summary of the results can be queried using the CALIB_RESULT_SUMMARY identifier.
        /// </summary>
        /// <param name="calibResultSummaryStruct">Struct with result parameters after calibration procedure</param>
        /// <returns>True if success</returns>
        public bool CALIB_RESULT_SUMMARY(out CALIB_RESULT_SUMMARY_STRUCT calibResultSummaryStruct)
        {
            calibResultSummaryStruct = new CALIB_RESULT_SUMMARY_STRUCT();

            var id = "CALIB_RESULT_SUMMARY";
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

            if (answerParam.ElementAt(0).Key != "AVE_ERROR")
                return false;

            calibResultSummaryStruct.AVE_ERROR = float.Parse(answerParam.ElementAt(0).Value,
                CultureInfo.InvariantCulture);

            if (answerParam.ElementAt(1).Key != "VALID_POINTS")
                return false;

            calibResultSummaryStruct.VALID_POINTS = Convert.ToInt32(answerParam.ElementAt(1).Value);

            return true;
        }
    }
}
