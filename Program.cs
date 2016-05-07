using HashListComparer.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashListComparer
{
    class Program
    {
        static void Main(string[] args)
        {
            //int StartingPoint = 0;
            string checksumfile = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).Directory.FullName;
            string fakepath = @"C:\";
            string output1 = "", output2 = "", output0 = "";
            List<string> CandidateList = new List<string>();
            List<FileStruct> outputlist1 = null;
            List<FileStruct> outputlist2 = null;
            //List<FileStruct> newlist = null;
            //args = new string[] { @"C:\Users\Dev\Source\Repos\HashListComparer\bin\Debug\State_20160403.md5", @"C:\Users\Dev\Source\Repos\HashListComparer\bin\Debug\State_20160403-Sorted.md5" };
            //args = new string[] { @"C:\Users\Dev\Source\Repos\HashListComparer\bin\Debug\output_2016-05-07.md5" };
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
            switch (Path.GetExtension(CandidateList[0]).ToLowerInvariant())
            {
                //case ".fva":
                //    XDocument xmlDoc = XDocument.Load(CandidateList[i]);
                //    outputlist = xmlDoc.Descendants("fvx")
                //        .Elements().Select(entry => new FileStruct
                //        {
                //            Name = Path.Combine(fakepath, entry.Attribute("name").Value),
                //            hash = entry.Element("hash").Value.Trim(),
                //        })
                //        .OrderBy(r => Path.GetDirectoryName(r.Name))
                //        .Select(entry => new FileStruct
                //        {
                //            Name = entry.Name.Replace(fakepath, ""),
                //            hash = entry.hash,
                //        }).ToList();
                //    output = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[i]) + ".md5");
                //    break;
                case ".md5":
                    if (CandidateList.Count == 2)
                    {
                        if (Path.GetExtension(CandidateList[1]) == ".md5")
                        {
                            Console.WriteLine((CandidateList[0]) + " vs " + (CandidateList[1]));
                            List<string> md5lines = null;
                            //for (int i = 0; i < CandidateList.Count; i++)
                            //{
                            md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                            Console.WriteLine(md5lines.Count);
                            md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                            md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                            Console.WriteLine(md5lines.Count);
                            outputlist1 = md5lines.Select(entry => new FileStruct
                            {
                                Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                                hash = entry.Trim().Split('*')[0].Trim(),
                            })
                            .OrderBy(r => Path.GetDirectoryName(r.Name))
                            .Select(entry => new FileStruct
                            {
                                Name = entry.Name.Replace(fakepath, ""),
                                hash = entry.hash.ToLowerInvariant(),
                            }).ToList();
                            md5lines = File.ReadAllLines(CandidateList[1]).ToList();
                            Console.WriteLine(md5lines.Count);
                            md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                            Console.WriteLine(md5lines.Count);
                            outputlist2 = md5lines.Select(entry => new FileStruct
                            {
                                Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                                hash = entry.Trim().Split('*')[0].Trim(),
                            })
                            .OrderBy(r => Path.GetDirectoryName(r.Name))
                            .Select(entry => new FileStruct
                            {
                                Name = entry.Name.Replace(fakepath, ""),
                                hash = entry.hash.ToLowerInvariant(),
                            }).ToList();
                            output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Intersected.txt");
                            output1 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-orphan.txt");
                            output2 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[1]) + "-orphan.txt");
                            // matched elements from both lists
                            List<FileStruct> r1 = outputlist1.Intersect<FileStruct>(outputlist2, new ListComparer()).ToList();
                            // elements from l1 not in l2
                            List<FileStruct> firstNotSecond = outputlist1.Except<FileStruct>(outputlist2, new ListComparer()).ToList();
                            // elements from l2 not in l1
                            List<FileStruct> secondNotFirst = outputlist2.Except<FileStruct>(outputlist1, new ListComparer()).ToList();
                            if (r1.Any())
                            {
                                StringBuilder builder = new StringBuilder();
                                using (FileStream file = File.Create(output0))
                                { }
                                foreach (var fss in r1)
                                {
                                    //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                                    //{
                                    builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                                    //}
                                }
                                if (builder.Length > 0)
                                {
                                    using (TextWriter writer = File.CreateText(output0))
                                    {
                                        writer.Write(builder.ToString());
                                    }
                                    builder.Clear();
                                }
                            }
                            if (firstNotSecond.Any())
                            {
                                StringBuilder builder = new StringBuilder();
                                using (FileStream file = File.Create(output1))
                                { }
                                foreach (var fss in firstNotSecond)
                                {
                                    //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                                    //{
                                    builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                                    //}
                                }
                                if (builder.Length > 0)
                                {
                                    using (TextWriter writer = File.CreateText(output1))
                                    {
                                        writer.Write(builder.ToString());
                                    }
                                    builder.Clear();
                                }
                            }
                            if (secondNotFirst.Any())
                            {
                                StringBuilder builder = new StringBuilder();
                                using (FileStream file = File.Create(output2))
                                { }
                                foreach (var fss in secondNotFirst)
                                {
                                    //if (!fss.Name.ToLowerInvariant().Contains("thumbs.db"))
                                    //{
                                    builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                                    //}
                                }
                                if (builder.Length > 0)
                                {
                                    using (TextWriter writer = File.CreateText(output2))
                                    {
                                        writer.Write(builder.ToString());
                                    }
                                    builder.Clear();
                                }
                            }
                            //newlist = firstNotSecond.Concat(secondNotFirst).ToList();
                            //}
                        }
                    }
                    else if (CandidateList.Count == 1) // find duplicated
                    {
                        List<string> md5lines = null;
                        //for (int i = 0; i < CandidateList.Count; i++)
                        //{
                        md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        Console.WriteLine(md5lines.Count);
                        md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                        md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        Console.WriteLine(md5lines.Count);
                        outputlist1 = md5lines.Select(entry => new FileStruct
                        {
                            Name = Path.Combine(fakepath, entry.Trim().Split('*')[1].Trim()),
                            hash = entry.Trim().Split('*')[0].Trim(),
                        })
                        .OrderBy(r => Path.GetDirectoryName(r.Name))
                        .Select(entry => new FileStruct
                        {
                            Name = entry.Name.Replace(fakepath, ""),
                            hash = entry.hash.ToLowerInvariant(),
                        }).ToList();

                        List<FileStruct> duplicateItems = outputlist1
                            .GroupBy(x => x.hash)
                            .Where(x => x.Count() > 1)
                            .SelectMany(x => x).ToList();
                        output0 = Path.Combine(checksumfile, Path.GetFileNameWithoutExtension(CandidateList[0]) + "-Duplicated.txt");
                        if (duplicateItems.Any())
                        {
                            StringBuilder builder = new StringBuilder();
                            using (FileStream file = File.Create(output0))
                            { }
                            foreach (var fss in duplicateItems)
                            {
                                builder.Append(fss.hash + " *" + fss.Name).AppendLine();
                            }
                            if (builder.Length > 0)
                            {
                                using (TextWriter writer = File.CreateText(output0))
                                {
                                    writer.Write(builder.ToString());
                                }
                                builder.Clear();
                            }
                        }
                    }
                    break;
                case ".sfv":
                    if (CandidateList.Count == 1)
                    {
                        List<string> md5lines = null;
                        //for (int i = 0; i < CandidateList.Count; i++)
                        //{
                        md5lines = File.ReadAllLines(CandidateList[0]).ToList();
                        md5lines = md5lines.Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
                        outputlist1 = md5lines.Select(entry => new FileStruct
                        {
                            Name = entry.Substring(0, entry.IndexOfAny(new char[] { ' ', '\t' }, entry.LastIndexOf("."))).Trim(),
                            hash = entry.Substring(entry.LastIndexOfAny(new char[] { ' ', '\t' })).Trim(),
                        }).ToList();
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
                        //newlist = firstNotSecond.Concat(secondNotFirst).ToList();
                        //}
                    }
                    break;

            }
            //int subfolderindex = outputlist1.FindIndex(a => a.Name.Contains("\\"));
            //var firsthalf = outputlist1.Take(subfolderindex).OrderBy(x => x.Name).ToList();
            //var Secondhalf = outputlist1.Skip(subfolderindex).OrderBy(x => x.Name).ToList();
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
            //}
            //Console.ReadKey();
        }
    }
}
