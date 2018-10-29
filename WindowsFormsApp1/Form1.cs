using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void write_to_gpx(string file_path)
        {
            string[] txt_line_string_arry = new string[4];
            string txt_line_string = null;
            string desc_gpx = null;
            StreamReader fs = new StreamReader(file_path, Encoding.Default);

            string new_file_path = Path.GetDirectoryName(file_path) + "\\" + Path.GetFileNameWithoutExtension(file_path) + ".gpx";
            XmlTextWriter writeXml = new XmlTextWriter(new_file_path, Encoding.UTF8);
            writeXml.WriteStartDocument(false);
            writeXml.WriteWhitespace("\n");
            writeXml.WriteStartElement("gpx");
            writeXml.WriteAttributeString("xmlns", "http://www.topografix.com/GPX/1/1");
            writeXml.WriteAttributeString("xmlns:gpxx", "http://www.garmin.com/xmlschemas/GpxExtensions/v3");
            writeXml.WriteAttributeString("xmlns:gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");
            writeXml.WriteAttributeString("creator", "Garmin Colorado");
            writeXml.WriteAttributeString("version", "1.1");
            writeXml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            writeXml.WriteAttributeString("xsi:schemaLocation", "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensions/v3/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtension/v1/TrackPointExtensionv1.xsd");

            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace(" ");
            writeXml.WriteStartElement("trk");
            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace("  ");
            writeXml.WriteElementString("name", Path.GetFileNameWithoutExtension(file_path));
            writeXml.WriteWhitespace("\n");

            //XmlTextReader readerXml = new XmlTextReader(textBox2.Text);

            txt_line_string = fs.ReadLine();
            if (txt_line_string.IndexOf("Area: ") > -1)
            {
                desc_gpx = txt_line_string + "\n";
                txt_line_string = fs.ReadLine();
            }

            if (txt_line_string.IndexOf("Length: ") > -1)
            {
                desc_gpx += txt_line_string + "\n";
                txt_line_string = fs.ReadLine();
            }

            if (txt_line_string.IndexOf("Circumference: ") > -1)
            {
                desc_gpx += txt_line_string;
                txt_line_string = fs.ReadLine();
            }

            if (txt_line_string.IndexOf("Angle: ") > -1)
            {
                desc_gpx += txt_line_string;
                txt_line_string = fs.ReadLine();
            }

            writeXml.WriteWhitespace("  ");
            writeXml.WriteElementString("desc", desc_gpx);
            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace("  ");
            writeXml.WriteStartElement("trkseg");
            writeXml.WriteWhitespace("\n");


            //           txt_line_string = fs.ReadLine();
            while (txt_line_string != null)
            {
                txt_line_string_arry = txt_line_string.Split('\t');
                writeXml.WriteWhitespace("   ");
                writeXml.WriteStartElement("trkpt");
                writeXml.WriteAttributeString("lat", txt_line_string_arry[1]);
                writeXml.WriteAttributeString("lon", txt_line_string_arry[2]);
                writeXml.WriteWhitespace("\n");

                writeXml.WriteWhitespace("    ");
                writeXml.WriteElementString("ele", txt_line_string_arry[3]);
                writeXml.WriteWhitespace("\n");

                writeXml.WriteWhitespace("    ");
                writeXml.WriteElementString("time", txt_line_string_arry[0]);
                writeXml.WriteWhitespace("\n");
                writeXml.WriteWhitespace("   ");
                writeXml.WriteEndElement();//trkpt
                writeXml.WriteWhitespace("\n");

                txt_line_string = fs.ReadLine();

            }

            writeXml.WriteWhitespace("  ");
            writeXml.WriteEndElement();//trkseg
            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace(" ");
            writeXml.WriteEndElement();//trk
            writeXml.WriteWhitespace("\n");
            writeXml.WriteEndElement();//gpx
                    writeXml.Flush();
            writeXml.Close();

            fs.Close();

            MessageBox.Show("处理完毕！");
            System.Diagnostics.Process.Start(new_file_path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string waypoint_info = null;
            string waypoint_trkpt = null;
            string waypoint_time = null;
            string waypoint_ele = null;
            OpenFileDialog dialog  = new OpenFileDialog();
            dialog.Filter = "gpx文件|*.gpx";
            if (dialog.ShowDialog() == DialogResult.OK && null != dialog.FileName)
            {
                textBox1.Text = dialog.FileName;
            }
            else
            {
                MessageBox.Show("文件路径无效！");
                return;
            }

            string new_file_path = Path.GetDirectoryName(dialog.FileName) + "\\" + Path.GetFileNameWithoutExtension(dialog.FileName) + ".txt";
            FileStream fs = new FileStream( new_file_path,FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.Flush();
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);

            XmlTextReader readerXml = new XmlTextReader(dialog.FileName);
            while (readerXml.Read())
            {
                if (readerXml.NodeType == XmlNodeType.Element)
                {
                    if ("desc" == readerXml.Name)
                    {
                        waypoint_info = readerXml.ReadString().Trim() + "\n";
                        m_streamWriter.Write(waypoint_info);
                    }
                    else if ("trkpt" == readerXml.Name )
                    {
                        waypoint_trkpt = readerXml.GetAttribute("lat") + "\t" + readerXml.GetAttribute("lon");
                    }
                    else if("ele" == readerXml.Name )
                    {
                        waypoint_ele = readerXml.ReadString().Trim();
                    }
                    else if ("time" == readerXml.Name)
                    {
                        waypoint_time = readerXml.ReadString().Trim();
                        if( waypoint_time != null && waypoint_trkpt != null && waypoint_ele != null)
                        {
                            waypoint_info = waypoint_time + "\t" + waypoint_trkpt + "\t" + waypoint_ele + "\n";
                            m_streamWriter.Write( waypoint_info );
                        }
                    }
                }
            }
            readerXml.Close();
            m_streamWriter.Flush();
            m_streamWriter.Close();
            //var proc = System.Diagnostics.Process.Start("C:\\Users\\zhaobruce\\Desktop\\fix_length\\polyfit_vscode\\polyfit.c.exe", new_file_path);
            var proc = System.Diagnostics.Process.Start(System.Environment.CurrentDirectory + "\\polyfit.c.exe", new_file_path);
            proc.WaitForExit();

            new_file_path = Path.GetDirectoryName(new_file_path) + "\\" + Path.GetFileNameWithoutExtension(new_file_path) + "_new.txt";
            write_to_gpx( new_file_path );

        }

    }
}
