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
        static void Main(string[] args)
        {
            List<string> CandidateList = new List<string>();
            //args = new string[] { @"C:\Users\Dev\Source\Repos\ChecksumFileHandler\bin\Debug\State_20160403.md5", @"C:\Users\Dev\Source\Repos\ChecksumFileHandler\bin\Debug\State_20160403-Sorted.md5" };
            //args = new string[] { @"C:\Users\Dev\Source\Repos\ChecksumFileHandler\bin\Debug\output_2016-05-07.md5" };
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
            //for (int i = StartingPoint; i < CandidateList.Count; i++)
            //{
            string output1 = "", output2 = "", output0 = "";
            //int subfolderindex = 0;
            List<FileStruct> outputlist0 = null;
            List<FileStruct> outputlist1 = null;
            List<FileStruct> outputlist2 = null;
            //List<FileStruct> firsthalf = null;
            //List<FileStruct> Secondhalf = null;
            //List<FileStruct> newlist = null;
            switch (Path.GetExtension(CandidateList[0]).ToLowerInvariant())
            {
                case ".fva":
                    for (int i = 0; i < CandidateList.Count; i++)
                    {
                        if (Path.GetExtension(CandidateList[i]).ToLowerInvariant() == ".fva")
                        {
                            //XDocument xmlDoc = XDocument.Load(CandidateList[i]);
                            //outputlist0 = xmlDoc.Descendants("fvx")
                            //    .Elements().Select(entry => new FileStruct
                            //    {
                            //        Name = Path.Combine(fakepath, entry.Attribute("name").Value),
                            //        hash = entry.Element("hash").Value.Trim(),
                            //    })
                            //    .OrderBy(r => Path.GetDirectoryName(r.Name))
                            //    .Select(entry => new FileStruct
                            //    {
                            //        Name = entry.Name.Replace(fakepath, ""),
                            //        hash = entry.hash.ToLowerInvariant(),
                            //    }).ToList();
                            outputlist0 = Prepare_Source_Data(CandidateList[i], Path.GetExtension(CandidateList[i]).ToLowerInvariant());
                            output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[i]) + "-Converted_from_FVA.md5");

                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[i]) + "-Converted_from_FVA-Duplicated.txt");
                            Generate_Duplicated_ItemList(outputlist0, output1);
                            //List<FileStruct> duplicateItems = outputlist0
                            //    .GroupBy(x => x.hash)
                            //    .Where(x => x.Count() > 1)
                            //    .SelectMany(x => x).ToList();

                            //if (duplicateItems.Any())
                            //{
                            //    StringBuilder builder = new StringBuilder();
                            //    using (FileStream file = File.Create(output1))
                            //    { }
                            //    foreach (var fss in duplicateItems)
                            //    {
                            //        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            //    }
                            //    if (builder.Length > 0)
                            //    {
                            //        using (TextWriter writer = File.CreateText(output1))
                            //        {
                            //            writer.Write(builder.ToString());
                            //        }
                            //        builder.Clear();
                            //    }
                            //}
                            Output_result(Sort(outputlist0), output0, false);
                            //subfolderindex = outputlist0.FindIndex(a => a.Name.Contains("\\"));
                            //firsthalf = outputlist0.Take(subfolderindex).OrderBy(x => x.Name).ToList();
                            //Secondhalf = outputlist0.Skip(subfolderindex).OrderBy(x => x.Name).ToList();
                            //newlist = Secondhalf.Concat(firsthalf).ToList();
                            //if (newlist.Any())
                            //{
                            //    StringBuilder builder = new StringBuilder();
                            //    using (FileStream file = File.Create(output0))
                            //    { }
                            //    foreach (var fss in newlist)
                            //    {
                            //        if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                            //        {
                            //            builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            //        }
                            //    }
                            //    if (builder.Length > 0)
                            //    {
                            //        using (TextWriter writer = File.CreateText(output0))
                            //        {
                            //            writer.Write(builder.ToString());
                            //        }
                            //        builder.Clear();
                            //    }
                            //}
                        }
                    }
                    break;
                case ".md5":
                    if (CandidateList.Count == 2)
                    {
                        if (Path.GetExtension(CandidateList[1]) == ".md5")
                        {
                            Console.WriteLine((CandidateList[0]) + " compare with " + (CandidateList[1]));
                            //List<string> md5lines = null;
                            //md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                            //Console.WriteLine((CandidateList[0]) + " Raw Rows : " + md5lines.Count);
                            //md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                            ////md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                            //Console.WriteLine((CandidateList[0]) + " Valid Rows : " + md5lines.Count);
                            //outputlist1 = md5lines.Select(entry => new FileStruct
                            //{
                            //    Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                            //    hash = entry.Trim().Split('*')[0].Trim(),
                            //})
                            //.OrderBy(r => Path.GetDirectoryName(r.Name))
                            //.Select(entry => new FileStruct
                            //{
                            //    Name = entry.Name.Replace(fakepath, ""),
                            //    hash = entry.hash.ToLowerInvariant(),
                            //}).ToList();
                            outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());

                            //md5lines = File.ReadAllLines(CandidateList[1]).ToList();
                            //Console.WriteLine((CandidateList[1]) + " Raw Rows : " + md5lines.Count);
                            //md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                            //Console.WriteLine((CandidateList[1]) + " Valid Rows : " + md5lines.Count);
                            //outputlist2 = md5lines.Select(entry => new FileStruct
                            //{
                            //    Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                            //    hash = entry.Trim().Split('*')[0].Trim(),
                            //})
                            //.OrderBy(r => Path.GetDirectoryName(r.Name))
                            //.Select(entry => new FileStruct
                            //{
                            //    Name = entry.Name.Replace(fakepath, ""),
                            //    hash = entry.hash.ToLowerInvariant(),
                            //}).ToList();

                            outputlist2 = Prepare_Source_Data(CandidateList[1], Path.GetExtension(CandidateList[1]).ToLowerInvariant());

                            output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Intersected.txt");
                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-orphan.txt");
                            output2 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[1]) + "-orphan.txt");
                            // matched elements from both lists
                            List<FileStruct> Intersected = outputlist1.Intersect<FileStruct>(outputlist2, new ListComparer()).ToList();
                            // elements from l1 not in l2
                            List<FileStruct> firstNotSecond = outputlist1.Except<FileStruct>(outputlist2, new ListComparer()).ToList();
                            // elements from l2 not in l1
                            List<FileStruct> secondNotFirst = outputlist2.Except<FileStruct>(outputlist1, new ListComparer()).ToList();
                            Output_result(Intersected, output0, true);
                            //if (Intersected.Any())
                            //{
                            //    StringBuilder builder = new StringBuilder();
                            //    using (FileStream file = File.Create(output0))
                            //    { }
                            //    foreach (var fss in Intersected)
                            //    {
                            //        //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                            //        //{
                            //        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            //        //}
                            //    }
                            //    if (builder.Length > 0)
                            //    {
                            //        using (TextWriter writer = File.CreateText(output0))
                            //        {
                            //            writer.Write(builder.ToString());
                            //        }
                            //        builder.Clear();
                            //    }
                            //}
                            Output_result(firstNotSecond, output1, true);
                            //if (firstNotSecond.Any())
                            //{
                            //    StringBuilder builder = new StringBuilder();
                            //    using (FileStream file = File.Create(output1))
                            //    { }
                            //    foreach (var fss in firstNotSecond)
                            //    {
                            //        //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                            //        //{
                            //        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            //        //}
                            //    }
                            //    if (builder.Length > 0)
                            //    {
                            //        using (TextWriter writer = File.CreateText(output1))
                            //        {
                            //            writer.Write(builder.ToString());
                            //        }
                            //        builder.Clear();
                            //    }
                            //}
                            Output_result(secondNotFirst, output2, true);
                            //if (secondNotFirst.Any())
                            //{
                            //    StringBuilder builder = new StringBuilder();
                            //    using (FileStream file = File.Create(output2))
                            //    { }
                            //    foreach (var fss in secondNotFirst)
                            //    {
                            //        //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                            //        //{
                            //        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            //        //}
                            //    }
                            //    if (builder.Length > 0)
                            //    {
                            //        using (TextWriter writer = File.CreateText(output2))
                            //        {
                            //            writer.Write(builder.ToString());
                            //        }
                            //        builder.Clear();
                            //    }
                            //}
                        }
                    }
                    else if (CandidateList.Count == 1) // find duplicated and sort
                    {
                        //List<string> md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        //Console.WriteLine(CandidateList[0] + " Raw Rows : " + md5lines.Count);
                        //md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                        //md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        //Console.WriteLine(CandidateList[0] + " Valid Rows : " + md5lines.Count);
                        //outputlist1 = md5lines.Select(entry => new FileStruct
                        //{
                        //    Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                        //    hash = entry.Trim().Split('*')[0].Trim(),
                        //})
                        //.OrderBy(r => Path.GetDirectoryName(r.Name))
                        //.Select(entry => new FileStruct
                        //{
                        //    Name = entry.Name.Replace(fakepath, ""),
                        //    hash = entry.hash.ToLowerInvariant(),
                        //}).ToList();

                        outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());
                        //outputlist2 = md5lines.Select(entry => new FileStruct
                        //{
                        //    Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                        //    hash = entry.Trim().Split('*')[0].Trim(),
                        //})
                        //.OrderBy(r => Path.GetDirectoryName(r.Name))
                        //.Select(entry => new FileStruct
                        //{
                        //    Name = entry.Name.Replace(fakepath, ""),
                        //    hash = entry.hash,
                        //}).ToList();
                        output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Sorted.md5");

                        //List<FileStruct> duplicateItems = outputlist1
                        //    .GroupBy(x => x.hash)
                        //    .Where(x => x.Count() > 1)
                        //    .SelectMany(x => x).ToList();
                        output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Duplicated.txt");
                        Generate_Duplicated_ItemList(outputlist1, output0);

                        //if (duplicateItems.Any())
                        //{
                        //    StringBuilder builder = new StringBuilder();
                        //    using (FileStream file = File.Create(output0))
                        //    { }
                        //    foreach (var fss in duplicateItems)
                        //    {
                        //        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                        //    }
                        //    if (builder.Length > 0)
                        //    {
                        //        using (TextWriter writer = File.CreateText(output0))
                        //        {
                        //            writer.Write(builder.ToString());
                        //        }
                        //        builder.Clear();
                        //    }
                        //}

                        Output_result(Sort(outputlist1), output1, false);
                        //subfolderindex = outputlist1.FindIndex(a => a.Name.Contains("\\"));
                        //firsthalf = outputlist1.Take(subfolderindex).OrderBy(x => x.Name).ToList();
                        //Secondhalf = outputlist1.Skip(subfolderindex).OrderBy(x => x.Name).ToList();
                        //newlist = Secondhalf.Concat(firsthalf).ToList();
                        //if (newlist.Any())
                        //{
                        //    StringBuilder builder = new StringBuilder();
                        //    using (FileStream file = File.Create(output1))
                        //    { }
                        //    foreach (var fss in newlist)
                        //    {
                        //        if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                        //        {
                        //            builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                        //        }
                        //    }
                        //    if (builder.Length > 0)
                        //    {
                        //        using (TextWriter writer = File.CreateText(output1))
                        //        {
                        //            writer.Write(builder.ToString());
                        //        }
                        //        builder.Clear();
                        //    }
                        //}
                    }
                    break;
                case ".sfv":
                    if (CandidateList.Count == 1)
                    {
                        //List<string> md5lines = null;
                        //for (int i = 0; i < CandidateList.Count; i++)
                        //{
                        //md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        //md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                        //outputlist1 = md5lines.Select(entry => new FileStruct
                        //{
                        //    Name = entry.Substring(0, entry.IndexOfAny(new char[] { ' ', '\t' }, entry.LastIndexOf("."))).Trim(),
                        //    hash = entry.Substring(entry.LastIndexOfAny(new char[] { ' ', '\t' })).Trim(),
                        //}).ToList();
                        outputlist1 = Prepare_Source_Data(CandidateList[0], Path.GetExtension(CandidateList[0]).ToLowerInvariant());
                        StringBuilder builder = new StringBuilder();
                        bool Mismatch = false;
                        //using (FileStream file = File.Create(output1))
                        //{ }
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
            //if (newlist.Any())
            //{
            //    StringBuilder builder = new StringBuilder();
            //    using (FileStream file = File.Create(output_filename))
            //    { }
            //    foreach (var fss in newlist)
            //    {
            //        if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
            //        {
            //            builder.Append(fss.hash + " *" + fss.Name).AppendLine();
            //        }
            //    }
            //    if (builder.Length > 0)
            //    {
            //        using (TextWriter writer = File.CreateText(output_filename))
            //        {
            //            writer.Write(builder.ToString());
            //        }
            //        builder.Clear();
            //    }
            //}
        }
        static private void Output_result(List<FileStruct> t, string output_filename, bool isCompare)
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
                        builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                    }
                    else
                    {
                        if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                        {
                            builder.Append(fss.hash + " *" + fss.Name).AppendLine();
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
            }
        }
    }
}
