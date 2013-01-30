using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace xsltr
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: xsltr.exe inputXmlFilePath xslTransformationFilePath outputFilePath");
                return;
            }
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string xmlfile = args[0];
            string xslfile = args[1];
            string outfile = args[2];

            XPathDocument doc = null;
            try
            {
                doc = new XPathDocument(xmlfile);
            }
            catch (XmlException e)
            {
                Console.WriteLine(Path.GetFullPath(xmlfile));
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            try
            {
                XslCompiledTransform transform = new XslCompiledTransform(true);
                transform.Load(xslfile);

                XmlTextWriter writer = new XmlTextWriter(outfile, null);

                transform.Transform(doc, null, writer);

                Console.WriteLine(String.Format("{0} in {1}ms", Path.GetFullPath(outfile), stopwatch.ElapsedMilliseconds));
            }
            catch (XsltException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(String.Format("{0}({1},{2})", e.SourceUri, e.LineNumber, e.LinePosition));
                Console.WriteLine(e.InnerException.Message);
                Environment.Exit(1);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                Environment.Exit(1);
            }
        }
    }
}
