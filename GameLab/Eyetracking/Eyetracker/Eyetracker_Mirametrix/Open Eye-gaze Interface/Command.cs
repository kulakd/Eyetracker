using System;
using System.Collections.Generic;

namespace GameLab.Eyetracking.OpenEyeGazeInterface
{
    // Object to parse commands
    public static class Command
    {
        public static string Create(string cmd, string id, Dictionary<string, string> additionalParameters = null)
        {
            string command = "<" + cmd + " " + "ID=\"" + id + "\"";
            if (additionalParameters != null)
            {
                foreach (var parameter in additionalParameters)
                {
                    command += " " + parameter.Key + "=\"" + parameter.Value + "\"";
                }
            }
            command += " />\r\n";
            return command;
        }

        public static bool Recreate(string command, out string cmd, out string id)
        {            
            cmd = null;
            id = null;

            if (string.IsNullOrEmpty(command)) return false;

            // check structure
            if (command[0] != '<')
                return false;

            command = command.Remove(0, 1);
            
            if (command.Substring(command.Length - 5, 5) != " />\r\n")
                return false;

            command = command.Remove(command.Length - 5);

            // structure ok
            var items = command.Split(' ');
            if (items.GetLength(0) < 2)
                return false;
            cmd = items[0];

            if (items[1].Substring(0, 4) != "ID=\"")
                return false;
            id = items[1].Substring(5, items[1].Length - 6);

            return true;
        }

        public static bool Recreate(string command, out string cmd, out string id, out Dictionary<string, string> additionalParams)
        {
            cmd = null;
            id = null;
            additionalParams = null;

            if (string.IsNullOrEmpty(command)) return false;

            // check structure
            if (command[0] != '<')
                return false;

            command = command.Remove(0, 1);

            int lastBracketId = command.IndexOf('>');
            if (lastBracketId > -1)
                command = command.Substring(0, lastBracketId - 2);

            // structure ok
            var items = command.Split(' ');
            if (items.GetLength(0) < 3)
                return false;
            cmd = items[0];

            if (items[1].Substring(0, 4) == "ID=\"")
                id = items[1].Substring(4, items[1].Length - 5);

            additionalParams = new Dictionary<string, string>();

            int i = string.IsNullOrEmpty(id) ? 1 : 2;
            for (; i < items.Length; i++)
            {
                var itemParam = items[i].Split('=');
                if (itemParam.GetLength(0) != 2)
                    return false;
                additionalParams.Add(itemParam[0], itemParam[1].Substring(1, itemParam[1].Length - 2));
            }

            return true;
        }

        public static bool RecreateREC(string answers, out List<Dictionary<string, string>> additionalParams)
        {
            additionalParams = new List<Dictionary<string, string>>();

            if (string.IsNullOrEmpty(answers)) return false;

            var commands = answers.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string commandTmp;
            foreach (var command in commands)
            {
                if (string.IsNullOrEmpty(command)) continue;

                // check structure
                if (command[0] != '<')
                    return false;

                commandTmp = command;
                commandTmp = commandTmp.Remove(0, 1);

                int lastBracketId = commandTmp.IndexOf('>');
                if (lastBracketId > -1)
                    commandTmp = commandTmp.Substring(0, lastBracketId - 2);

                // structure ok
                var items = commandTmp.Split(' ');
                if (items.GetLength(0) < 3)
                    return false;

                if (items[0] != "REC") continue;

                var dictionaryTmp = new Dictionary<string, string>();

                for (int i=1; i < items.Length; i++)
                {
                    var itemParam = items[i].Split('=');
                    if (itemParam.GetLength(0) != 2)
                        return false;
                    dictionaryTmp.Add(itemParam[0], itemParam[1].Substring(1, itemParam[1].Length - 2));
                }

                additionalParams.Add(dictionaryTmp);
            }

            return true;
        }

        public static bool IsCompatible(string commandSend, string commandReceive)
        {
            string firstCmd;
            string firstId;
            string secondCmd;
            string secondId;

            if (!Recreate(commandSend, out firstCmd, out firstId))
                return false;
            if (!Recreate(commandReceive, out secondCmd, out secondId))
                return false;

            if (secondCmd != "ACK")
                return false;

            if (firstId != secondId)
                return false;

            return true;
        }

    }
}
                                                                                                                                                                                      