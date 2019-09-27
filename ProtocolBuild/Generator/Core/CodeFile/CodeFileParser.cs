using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RDLog;
using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class CodeFileParser
    {
        public CodeFile ParserFile(string fileFullName)
        {
            var index = fileFullName.LastIndexOf("\\", StringComparison.Ordinal);
            if (index < 0)
            {
                Log.Error($"ParserFile {fileFullName} error : fileFullName format wrong .");
                return null;
            }

            var fileName = fileFullName.Substring(index + 1);

            var lines = FileUtil.ReadLinesFromFile(fileFullName);
            if (lines == null || lines.Count == 0)
            {
                Log.Error($"ParserFile {fileFullName} error : read file wrong .");
                return null;
            }

            var codeFile = new CodeFile
            {
                Name = fileName,
                FullName = fileFullName
            };

            if (CodeFileFormat(lines, codeFile))
            {
                return codeFile;
            }
            Log.Error($"ParserFile {fileFullName} error : format file wrong .");
            return null;
        }

        private bool CodeFileFormat(IEnumerable<string> lines, CodeFile codeFile)
        {
            var syntaxLine = new StringBuilder();
            var packageLines = new StringBuilder();
            var messageLines = new StringBuilder();

            foreach (var line in lines)
            {
                if (line.StartsWith(ConstData.SYNTAX_KEY))
                {
                    if (!GetFormatSyntaxLine(line, codeFile, out var syntaxFormatLine))
                    {
                        return false;
                    }
                    syntaxLine.Append(syntaxFormatLine);
                }
                else if (line.StartsWith(ConstData.PACKAGE_KEY))
                {
                    if (!GetFormatPackageLine(line, codeFile, out var packageFormatLine))
                    {
                        return false;
                    }
                    packageLines.Append(packageFormatLine);
                }
                else if (line.StartsWith(ConstData.ANNOTAION))
                {
                    //The line is annotation 
                }
                else if (line.StartsWith(ConstData.MESSAGE_KEY))
                {
                    if (!GetFormatMessageKeyLine(line, codeFile, out var messageFormatLine))
                    {
                        return false;
                    }
                    messageLines.Append(messageFormatLine);
                    messageLines.Append(Environment.NewLine);
                }
                else
                {
                    if (!GetFormatOtherLine(line,out var otherFormatLine))
                    {
                        return false;
                    }
                    messageLines.Append(otherFormatLine);
                    messageLines.Append(Environment.NewLine);
                }
            }

            codeFile.SetProtoBuffData(messageLines);
            return true;
        }

        private bool GetFormatOtherLine(string line,out string formatLine)
        {
            formatLine = string.Empty;
            var index = line.IndexOf("//", StringComparison.Ordinal);
            formatLine = index == -1 ? line : line.Substring(0, index);
            return true;
        }

        private bool GetFormatMessageKeyLine(string line, CodeFile codeFile,out string formatLine)
        {
            formatLine = string.Empty;
            var formatArr = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (formatArr.Count <= 1)
            {
                Log.Error($"please check the message name {line}!");
                return false;
            }

            var messageKey = formatArr[0];
            if (string.CompareOrdinal(messageKey, ConstData.MESSAGE_KEY) == -1)
            {
                Log.Error($"please check the message key,{messageKey}!");
                return false;
            }

            var messageName = formatArr[1];
            var messageId = string.Empty;
            if (formatArr.Count > 2)
            {
                messageId = formatArr[2];
            }

            if (!codeFile.AddMessageNameId(messageName, messageId))
            {
                return false;
            }

            formatLine = string.Format("{0} {1}", messageKey, messageName);
            return true;
        }


        private bool GetFormatPackageLine(string line, CodeFile codeFile,out string formatLine)
        {
            formatLine = string.Empty;
            var formatArr = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (formatArr.Count <= 1)
            {
                Log.Error("please check the package name!");
                return false;
            }

            var packageKey = formatArr[0];
            if (string.CompareOrdinal(packageKey, ConstData.PACKAGE_KEY) == -1)
            {
                Log.Error($"please check the package key,{packageKey}!");
                return false;
            }

            var packageType = formatArr[1];
            if (codeFile.SetPackageType(packageType))
            {
                formatLine = $"{packageKey} {packageType}";
            }
            return true;
        }

        private bool GetFormatSyntaxLine(string line, CodeFile codeFile,out string formatLine)
        {
            formatLine = string.Empty;
            string tempLine = line.Replace(" ", "");
            var formatArr = tempLine.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (formatArr.Count <= 1)
            {
                Log.Error("please check the package name!");
                return false;
            }

            var syntaxKey = formatArr[0];
            if (string.CompareOrdinal(syntaxKey, ConstData.SYNTAX_KEY) == -1)
            {
                Log.Error($"please check the syntax key,{syntaxKey}!");
                return false;
            }

            var syntax = formatArr[1].Replace(";", "");
            if (string.CompareOrdinal(syntax, ConstData.SYNTAX) == -1)
            {
                Log.Error($"please check the syntax,{syntax}!");
                return false;
            }

            codeFile.SetSyntex(syntax);
            formatLine = $"{syntaxKey}={syntax};";
            return true;
        }
    }
}