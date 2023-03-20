using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameLab.Eyetracking.OpenEyeGazeInterface
{
    /// <summary>
    /// Options for the control of the Tracker program through the API.
    /// </summary>
    public class TrackerProgramControl
    {
        /// <summary>
        /// Command the Tracker program to exit.
        /// </summary>
        /// <returns>True if success</returns>
        public bool TRACKER_EXIT()
        {
            var id = "TRACKER_EXIT";
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
        /// Command the Tracker to change the current selected monitor. This command is also used to notify client applications whenever the user selected a different monitor within the Tracker user interface.
        /// </summary>
        /// <param name="value">Sets/gets the current selected monitor using a 0-based index for multiple monitors</param>
        /// <returns>True if success</returns>
        public bool SCREEN_SELECTED(int value)
        {
            var id = "SCREEN_SELECTED";
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

            if (answerParam.Count < 1)
                return false;

            if (answerParam.ElementAt(0).Key != "VALUE")
                return false;

            float valueAnswer = float.Parse(answerParam.ElementAt(0).Value, CultureInfo.InvariantCulture);

            //if (valueAnswer == value)
            //    return true;

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
        /// Set the Tracker program to show or hide the user display image.
        /// </summary>
        /// <param name="state">Show/Hide the Tracker display image</param>
        /// <returns>True if success</returns>
        public bool TRACKER_DISPLAY(bool state)
        {
            int value = state == true ? 1 : 0;

            var id = "TRACKER_DISPLAY";
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
