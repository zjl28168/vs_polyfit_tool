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

        //public void 
#if (false)
            public void gauss_solve(int n, double[] A, double[] x, double[] b)
        {
            int i, j, k, r; double max;
            for (k = 0; k < n - 1; k++)
            {
                max = fabs(A[k * n + k]); /*find maxmum*/
                r = k;
                for (i = k + 1; i < n - 1; i++)
                    if (max < fabs(A[i * n + i]))
                    { max = fabs(A[i * n + i]); r = i; }
                if (r != k) for (i = 0; i < n; i++)         /*change array:A[k]&A[r] */
                    {
                        max = A[k * n + i]; A[k * n + i] = A[r * n + i]; A[r * n + i] = max;
                    }
                max = b[k];                    /*change array:b[k]&b[r]     */
                b[k] = b[r]; b[r] = max;
                for (i = k + 1; i < n; i++)
                for (i = k + 1; i < n; i++)
                for (i = k + 1; i < n; i++)
                for (i = k + 1; i < n; i++)
                {
                    for (j = k + 1; j < n; j++)
                        A[i * n + j] -= A[i * n + k] * A[k * n + j] / A[k * n + k];
                    b[i] -= A[i * n + k] * b[k] / A[k * n + k];
                }
            }
            for (i = n - 1; i >= 0; x[i] /= A[i * n + i], i--)
                for (j = i + 1, x[i] = b[i]; j < n; j++)
                    x[i] -= A[i * n + j] * x[j];
        }
#endif
        private void button1_Click(object sender, EventArgs e)
        {
            string waypoint_info = null;
            //string file_name = null;
            string waypoint_trkpt = null;
            string waypoint_time = null;
            string waypoint_ele = null;
            //bool new_file_is_creat = false;
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
            MessageBox.Show("处理完毕！");
            System.Diagnostics.Process.Start(new_file_path);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt文件|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK && null != dialog.FileName)
            {
                textBox3.Text = dialog.FileName;
            }
            else
            {
                MessageBox.Show("文件路径无效！");
                return;
            }
            //string waypoint_trkpt_lat = null;
            // string waypoint_trkpt_lon = null;
            string[] txt_line_string_arry = new string[4];
            string txt_line_string = null;
            string desc_gpx = null;
            StreamReader fs = new StreamReader(textBox3.Text, Encoding.Default);
            string new_file_path = Path.GetDirectoryName(textBox3.Text) + "\\" + Path.GetFileNameWithoutExtension(textBox3.Text) + ".gpx";
            //StreamReader m_streamWriter = new StreamReader(fs);
            XmlTextWriter writeXml = new XmlTextWriter(new_file_path, Encoding.UTF8);
            writeXml.WriteStartDocument(false);
            //writeXml.WriteStartElement("?xml version="1.0" encoding="UTF - 8" standalone="no" ?");

            // writeXml.WriteStartElement( xsi:schemaLocation="http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensions/v3/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtension/v1/TrackPointExtensionv1.xsd");
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
            writeXml.WriteElementString("name", Path.GetFileNameWithoutExtension(textBox3.Text));
            writeXml.WriteWhitespace("\n");

            //XmlTextReader readerXml = new XmlTextReader(textBox2.Text);

            txt_line_string = fs.ReadLine();
            if(txt_line_string.IndexOf("Area: ")> -1)
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
            writeXml.WriteElementString("desc", desc_gpx );
            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace("  ");
            writeXml.WriteStartElement("trkseg");
            writeXml.WriteWhitespace("\n");


 //           txt_line_string = fs.ReadLine();
            while ( txt_line_string !=null )
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

            //            XmlTextReader readerXml = new XmlTextReader(textBox2.Text);
            /*
                        while (readerXml.Read())
                        {
                            if (readerXml.NodeType == XmlNodeType.Element )
                            {
                                if("desc" == readerXml.Name)
                                {
                                    writeXml.WriteWhitespace("  ");
                                    writeXml.WriteElementString("desc", readerXml.ReadString().Trim());
                                    writeXml.WriteWhitespace("\n");
                                    writeXml.WriteWhitespace("  ");
                                    writeXml.WriteStartElement("trkseg");
                                    writeXml.WriteWhitespace("\n");
                                }
                                if ("trkpt" == readerXml.Name)
                                {
                                    txt_line_string = fs.ReadLine();
                                    txt_line_string_arry = txt_line_string.Split('\t');
                                    writeXml.WriteWhitespace("   ");
                                    writeXml.WriteStartElement("trkpt");
                                    writeXml.WriteAttributeString("lat", txt_line_string_arry[1]);
                                    writeXml.WriteAttributeString("lon", txt_line_string_arry[2]);
                                    writeXml.WriteWhitespace("\n");
                                }
                                else if ("ele" == readerXml.Name)
                                {
                                    writeXml.WriteWhitespace("    ");
                                    writeXml.WriteElementString("ele", txt_line_string_arry[3]);
                                    writeXml.WriteWhitespace("\n");
                                }
                                else if ("time" == readerXml.Name)
                                {
                                    writeXml.WriteWhitespace("    ");
                                    writeXml.WriteElementString("time", txt_line_string_arry[0]);
                                    writeXml.WriteWhitespace("\n");
                                    writeXml.WriteWhitespace("   ");
                                    writeXml.WriteEndElement();//trkpt
                                    writeXml.WriteWhitespace("\n");
                                }
                            }
                        }
                        */
            writeXml.WriteWhitespace("  ");
            writeXml.WriteEndElement();//trkseg
            writeXml.WriteWhitespace("\n");
            writeXml.WriteWhitespace(" ");
            writeXml.WriteEndElement();//trk
            writeXml.WriteWhitespace("\n");
            writeXml.WriteEndElement();//gpx
            //writeXml.WriteEndElement();


            writeXml.Flush();
            writeXml.Close();

            //readerXml.Close();
            fs.Close();

            //m_streamWriter.Flush();
           // m_streamWriter.Close();
            MessageBox.Show("处理完毕！");
            System.Diagnostics.Process.Start(new_file_path);
        }


    }
}
