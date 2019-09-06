using ChecksumFileHandler.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ChecksumFileHandler
{
    class Program
    {
        private static string fakepath = @"C:\", checksumfile = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.FullName;
        private readonly static string reservedCharacters = "*'();@&=+$,/%#[]- ";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<string> CandidateList = new List<string>();
            //args = new string[] { @"C:\Users\Daniel\Source\Repos\ChecksumFileHandler\bin\Debug\20181117_Downloaded.md5" };
            if (args.Length > 0 && args.Length <= 2)
            {
                foreach (var s in args)
                {
                    CandidateList.Add(s);
                }
                checksumfile = Path.GetDirectoryName(args[0]);
            }
            else
            {
                return;
            }
            Console.WriteLine("No. of inputs : " + CandidateList.Count);
            string output1 = "", output2 = "", output0 = "", outputD = "", output3 = "";
            List<FileStruct> outputlist0 = null;
            List<FileStruct> outputlist1 = null;
            List<FileStruct> outputlist2 = null;
            switch (Path.GetExtension(CandidateList[0]).ToLowerInvariant())
            {
                case ".fva":
                    for (int i = 0; i < CandidateList.Count; i++)
                    {
                        if (Path.GetExtension(CandidateList[i]).ToLowerInvariant() == ".fva")
                        {
                            outputlist0 = Prepare_Source_Data(CandidateList[i], Path.GetExtension(CandidateList[i]).ToLowerInvariant());
                            output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[i]) + "-Converted_from_FVA.md5");

                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[i]) + "-Converted_from_FVA-Duplicated.txt");
                            Generate_Duplicated_ItemList(outputlist0, output1);
                            Output_result(Sort(outputlist0), output0, false, false);
                        }
                    }
                    break;
                case ".md5":
                    if (CandidateList.Count == 2)
                    {
                        if (Path.GetExtension(CandidateList[1]) == ".md5")
                        {
                            Console.WriteLine((CandidateList[0]) + " compare with " + (CandidateList[1]));
                            outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());
                            outputlist2 = Prepare_Source_Data(CandidateList[1], Path.GetExtension(CandidateList[1]).ToLowerInvariant());

                            output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Intersected.txt");
                            outputD = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-damaged.txt");
                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-orphan.txt");
                            output2 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[1]) + "-orphan.txt");
                            // matched elements from both lists
                            List<FileStruct> Intersected = outputlist1.Intersect<FileStruct>(outputlist2, new ListComparer()).ToList();
                            List<FileStruct> Damaged = outputlist1.Intersect<FileStruct>(outputlist2, new DamagedListComparer()).ToList();
                            // elements from l1 not in l2
                            List<FileStruct> firstNotSecond = outputlist1.Except<FileStruct>(outputlist2, new ListComparer()).ToList();
                            // elements from l2 not in l1
                            List<FileStruct> secondNotFirst = outputlist2.Except<FileStruct>(outputlist1, new ListComparer()).ToList();
                            Output_result(Intersected, output0, true, false);
                            Output_result(Damaged, outputD, true, false);
                            Output_result(firstNotSecond, output1, true, false);
                            Output_result(secondNotFirst, output2, true, false);
                        }
                    }
                    else if (CandidateList.Count == 1) // find duplicated and sort
                    {
                        outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());
                        output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Sorted.md5");
                        output2 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Dedupped.md5");
                        output3 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Unsorted.md5");
                        output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Duplicated.txt");
                        if (CandidateList[0].Contains("dedup"))
                        {
                            Generate_Distinct_ItemList(Sort(outputlist1), output2);
                        }
                        Generate_Duplicated_ItemList(outputlist1, output0);                        
                        Output_result(Sort(outputlist1), output1, false, false, output3);
                    }
                    break;
                case ".sfv":
                    if (CandidateList.Count == 1)
                    {
                        outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());
                        StringBuilder builder = new StringBuilder();
                        bool Mismatch = false;
                        foreach (var fss in outputlist1)
                        {
                            if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                            {
                                int pos1 = fss.Name.LastIndexOf(']');
                                int pos2 = fss.Name.LastIndexOf(')');
                                if (pos1 > pos2) //[]
                                {
                                    string hashinname = fss.Name.Substring(fss.Name.LastIndexOf('[') + 1);
                                    hashinname = hashinname.Remove(hashinname.LastIndexOf(']')).ToLowerInvariant();
                                    if (hashinname.Contains("_"))
                                        hashinname = hashinname.Substring(hashinname.LastIndexOf('_') + 1);
                                    if (hashinname == fss.hash)
                                        builder.Append(fss.Name + "\t" + fss.hash + "\t" + "OK").AppendLine();
                                    else
                                    {
                                        builder.Append(fss.Name + "\t" + fss.hash + "\t" + "Mismatch").AppendLine();
                                        Mismatch = true;
                                    }
                                }
                                else //()
                                {
                                    string hashinname = fss.Name.Substring(fss.Name.LastIndexOf('(') + 1);
                                    hashinname = hashinname.Remove(hashinname.LastIndexOf(')')).ToLowerInvariant();
                                    if (hashinname.Contains("_"))
                                        hashinname = hashinname.Substring(hashinname.LastIndexOf('_') + 1);
                                    if (hashinname == fss.hash)
                                        builder.Append(fss.Name + "\t" + fss.hash + "\t" + "OK").AppendLine();
                                    else
                                    {
                                        builder.Append(fss.Name + "\t" + fss.hash + "\t" + "Mismatch").AppendLine();
                                        Mismatch = true;
                                    }
                                }
                            }
                        }
                        if (builder.Length > 0)
                        {
                            if (!Mismatch)
                                output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-comparedSFV-OK.txt");
                            else
                                output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-comparedSFV-Mismatch.txt");
                            using (TextWriter writer = File.CreateText(output1))
                            {
                                writer.Write(builder.ToString());
                            }
                            builder.Clear();
                        }
                    }
                    break;
                case ".txt":
                    if (CandidateList.Count == 1) // reverse and sort
                    {
                        if (CandidateList[0].Contains("VideoCorruptionChecks"))
                        {
                            List<string> loglines = File.ReadAllLines(CandidateList[0]).ToList();
                            loglines = loglines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                            Console.WriteLine(Path.GetFileNameWithoutExtension(CandidateList[0]) + " records : " + loglines.Count);
                            string output_sanitized = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Sanitized.txt");
                            int pointer = 0;
                            loglines = loglines.Where(s => !s.Contains("Error:     Last message repeated")).ToList();
                            while (pointer < loglines.Count - 1)
                            {
                                string filename = loglines[pointer].Split(new string[] { "checking" }, StringSplitOptions.None)[0];
                                if (loglines[pointer].Contains("checking...") && loglines[pointer + 1].Contains(filename) && loglines[pointer + 1].Contains("checks Completed"))
                                {
                                    loglines[pointer] = "";
                                    loglines[pointer + 1] = "";
                                    pointer += 2;
                                }
                                else
                                {
                                    pointer += 1;
                                }
                            }
                            loglines = loglines.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            using (TextWriter tw = new StreamWriter(output_sanitized))
                            {
                                foreach (String s in loglines)
                                {
                                    tw.WriteLine(s);
                                    if (s.Contains("checks Completed"))
                                    {
                                        tw.WriteLine("");
                                    }
                                }
                            }
                        }
                        else if (CandidateList[0].Contains("DeleteIntersected") || CandidateList[0].Contains("DeleteDuplicated"))
                        {
                            try
                            {
                                List<FileStruct> filesTodelete = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());

                                foreach (var file in filesTodelete)
                                {
                                    Console.WriteLine("Delete " + file.Name);
                                    try
                                    {
                                        File.Delete(Path.Combine(Path.GetDirectoryName(CandidateList[0]), file.Name));
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine(Path.Combine(Path.GetDirectoryName(CandidateList[0]), file.Name) + " delete failed or missing");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                            Console.ReadKey();
                        }
                        else
                        {
                            outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());

                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Reversed.txt");

                            Output_result(Sort(outputlist1), output1, false, true);
                        }
                    }
                    break;
            }
            //Console.ReadKey();
        }
        static private List<string> GetFiles(string path) //, string pattern
        {
            var files = new List<string>();

            try
            {
                if (!path.Contains("$RECYCLE.BIN") && !path.Contains("#recycle"))
                {
                    files.AddRange(Directory.GetFiles(path).Where(file => file.ToLower().EndsWith("fva") || file.ToLower().EndsWith("md5")).ToList());
                    foreach (var directory in Directory.GetDirectories(path))
                        files.AddRange(GetFiles(directory));
                }
            }
            catch (UnauthorizedAccessException) { }

            return files;
        }
        static private void Generate_Distinct_ItemList(List<FileStruct> t, string output_filename)
        {
            try
            {
                List<FileStruct> distinctItems = t
                                    .GroupBy(x => x.hash)
                                    .Select(x => x.First()).ToList();

                if (distinctItems.Any())
                {
                    StringBuilder builder = new StringBuilder();
                    using (FileStream file = File.Create(output_filename))
                    { }
                    foreach (var fss in distinctItems)
                    {
                        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                    }
                    if (builder.Length > 0)
                    {
                        using (TextWriter writer = File.CreateText(output_filename))
                        {
                            writer.Write(builder.ToString());
                        }
                        builder.Clear();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        static private void Generate_Duplicated_ItemList(List<FileStruct> t, string output_filename)
        {
            try
            {
                List<FileStruct> duplicateItems = t
                                    .GroupBy(x => x.hash)
                                    .Where(x => x.Count() > 1)
                                    .SelectMany(x => x).ToList();

                if (duplicateItems.Any())
                {
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builder2 = new StringBuilder();
                    using (FileStream file = File.Create(output_filename))
                    { }
                    using (FileStream file = File.Create(output_filename.Replace("-Duplicated", "-DeleteDuplicated")))
                    { }
                    string previousHash = "";
                    foreach (var fss in duplicateItems)
                    {
                        if (!string.IsNullOrEmpty(previousHash))
                        {
                            if (fss.hash != previousHash)
                            {
                                builder.AppendLine();
                                builder2.AppendLine();
                            }
                        }
                        if (fss.Name.Contains("\\"))
                        {
                            builder.Append("\"file://" + checksumfile + "\\" + fss.Name.Substring(0, fss.Name.LastIndexOf("\\") + 1) + "\" " + fss.Name.Substring(fss.Name.LastIndexOf("\\") + 1)).AppendLine();
                        }
                        else
                        {
                            builder.Append("\"file://" + checksumfile + "\" " + fss.Name).AppendLine();
                        }
                        builder2.Append(fss.hash + " *" + fss.Name).AppendLine();
                        previousHash = fss.hash;
                    }
                    if (builder.Length > 0)
                    {
                        using (TextWriter writer = File.CreateText(output_filename))
                        {
                            writer.Write(builder.ToString());
                        }
                        builder.Clear();
                        using (TextWriter writer = File.CreateText(output_filename.Replace("-Duplicated", "-DeleteDuplicated")))
                        {
                            writer.Write(builder2.ToString());
                        }
                        builder2.Clear();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        static private void Generate_Damaged_ItemList(List<FileStruct> t, string output_filename)
        {
            try
            {
                List<FileStruct> duplicateItems = t
                                    .GroupBy(x => x.hash)
                                    .Where(x => x.Count() > 1)
                                    .SelectMany(x => x).ToList();

                if (duplicateItems.Any())
                {
                    StringBuilder builder = new StringBuilder();
                    using (FileStream file = File.Create(output_filename))
                    { }
                    foreach (var fss in duplicateItems)
                    {
                        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                    }
                    if (builder.Length > 0)
                    {
                        using (TextWriter writer = File.CreateText(output_filename))
                        {
                            writer.Write(builder.ToString());
                        }
                        builder.Clear();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        static private List<FileStruct> Prepare_Source_Data(string filepath, string source)
        {
            List<string> md5lines = null;
            switch (source)
            {
                case ".fva":
                    XDocument xmlDoc = XDocument.Load(filepath);
                    return new List<FileStruct>(xmlDoc.Descendants("fvx")
                        .Elements().Select(entry => new FileStruct
                        {
                            Name = Path.Combine(fakepath, entry.Attribute("name").Value),
                            hash = entry.Element("hash").Value.Trim(),
                        })
                        .OrderBy(r => Path.GetDirectoryName(r.Name))
                        .Select(entry => new FileStruct
                        {
                            Name = entry.Name.Replace(fakepath, ""),
                            hash = entry.hash.ToLowerInvariant(),
                        }).ToList());
                case ".sfv":
                    md5lines = File.ReadAllLines(filepath).ToList();
                    md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                    return new List<FileStruct>(md5lines.Select(entry => new FileStruct
                    {
                        Name = entry.Substring(0, entry.IndexOfAny(new char[] { ' ', '\t' }, entry.LastIndexOf("."))).Trim(),
                        hash = entry.Substring(entry.LastIndexOfAny(new char[] { ' ', '\t' })).Trim(),
                    }).ToList());
                case ".md5":
                case ".txt":
                default:
                    md5lines = File.ReadAllLines(filepath).ToList();
                    Console.WriteLine(filepath + " Raw Rows : " + md5lines.Count);
                    md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                    Console.WriteLine(filepath + " Valid Rows : " + md5lines.Count);

                    if (md5lines[0].Contains('*'))
                    {
                        return new List<FileStruct>(md5lines.Select(entry => new FileStruct
                        {
                            Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                            hash = entry.Trim().Split('*')[0].Trim(),
                        })
                        .OrderBy(r => Path.GetDirectoryName(r.Name))
                        .Select(entry => new FileStruct
                        {
                            Name = entry.Name.Replace(fakepath, ""),
                            hash = entry.hash.ToLowerInvariant(),
                        }).ToList());
                    }
                    else
                    {
                        return new List<FileStruct>(md5lines.Select(entry => new FileStruct
                        {
                            Name = Path.Combine(fakepath, entry.Trim().Split('\t')[1].Trim()),
                            hash = entry.Trim().Split('\t')[0].Trim(),
                        })
                        .OrderBy(r => Path.GetDirectoryName(r.Name))
                        .Select(entry => new FileStruct
                        {
                            Name = entry.Name.Replace(fakepath, ""),
                            hash = entry.hash.ToLowerInvariant(),
                        }).ToList());
                    }
            }
        }
        static private List<FileStruct> Sort(List<FileStruct> t)
        {
            int subfolderindex = t.FindIndex(a => a.Name.Contains("\\"));
            List<FileStruct> firsthalf = t.Take(subfolderindex).OrderBy(x => x.Name).ToList();
            List<FileStruct> Secondhalf = t.Skip(subfolderindex).OrderBy(x => x.Name).ToList();
            return new List<FileStruct>(Secondhalf.Concat(firsthalf).ToList());
        }
        static private void Output_result(List<FileStruct> t, string output_filename, bool isCompare, bool isReversed, string original_filename = null)
        {
            if (t.Any())
            {
                StringBuilder builder = new StringBuilder();
                using (FileStream file = File.Create(output_filename))
                { }
                foreach (var fss in t)
                {
                    if (isCompare)
                    {
                        if (isReversed)
                        {
                            builder.Append(fss.Name + " *" + fss.hash).AppendLine();
                        }
                        else
                        {
                            builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                        }
                    }
                    else
                    {
                        if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                        {
                            if (isReversed)
                            {
                                builder.Append(fss.Name + " *" + fss.hash).AppendLine();
                            }
                            else
                            {
                                builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            }
                        }
                    }
                }
                if (builder.Length > 0)
                {
                    using (TextWriter writer = File.CreateText(output_filename))
                    {
                        writer.Write(builder.ToString());
                    }
                    builder.Clear();
                }
                if (!string.IsNullOrEmpty(original_filename))
                {
                    string source_filename = original_filename.Replace("-Unsorted.md5", ".md5");
                    File.Move(source_filename, original_filename);
                    File.Move(output_filename, source_filename);
                }
            }
        }
        static string UrlEncode(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            var sb = new StringBuilder();
            foreach (char @char in value)
            {
                if (reservedCharacters.IndexOf(@char) == -1)
                    sb.Append(@char);
                else
                    sb.AppendFormat("%{0:X2}", (int)@char);
            }
            return sb.ToString();
        }
    }
}
