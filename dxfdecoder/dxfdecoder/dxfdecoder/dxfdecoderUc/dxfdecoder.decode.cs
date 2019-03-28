using System;
using System.IO;
using System.Text;
using DxfdecoderDef;

namespace Pathdecoder
{
    public class Pathdecoder
    {
        private string  dxfPath;//路径

        /// <summary>
        /// 用路径构造此类
        /// </summary>
        /// <param name="dxfpath"></param>
        public Pathdecoder(string dxfpath)
        {
            this.dxfPath = dxfpath;
        }

        /// <summary>
        /// Dxf文件的路径
        /// </summary>
        public string DxfFilePath
        {
            get { return dxfPath; }
            set { dxfPath = value; }
        }

        /// <summary>
        /// 返回dxf文件里包含的路径信息
        /// </summary>
        /// <param name="dxfPath"></param>
        /// <returns></returns>
        public DxfdecoderDef.Path[] getPaths()
        {
            int pathCount = 0;  //路径统计
            int polyLineN = 0;  //多段线的段数
            bool inSection = false ;//当前指针在Entities段
            bool polyLineNReaded = false;//已经读取到polyline的段数
            bool isClosed = false;//多段线是否闭合
            bool isClosedReaded = false;//是否读到是否闭合属性

            bool inPathLine = false, inPathPolyLine = false , inPathCircle = false, inPathArc = false;//当前所在的多段线类型
            string lineText = "";

            DxfdecoderDef.Path[] paths = new DxfdecoderDef.Path[10000]; //预留的路经集合
        

            for (int i = 0; i<10000; i++)
            {
                paths[i] = new DxfdecoderDef.Path();
            }
            

            StreamReader sr = new StreamReader(dxfPath, Encoding.ASCII);
            do
            {
                lineText = sr.ReadLine();
                if (lineText == "ENTITIES")
                {
                    inSection = true;
                }

                if(inSection == true && lineText  == "AcDbLine")
                {
                    inPathLine = true;inPathArc = false;inPathCircle = false;inPathPolyLine = false;    
                }

                if (inSection == true && lineText == "AcDbPolyline")
                {
                    inPathLine = false ; inPathArc = false; inPathCircle = false; inPathPolyLine = true;
                    polyLineN = 0;
                    isClosed = false;
                    isClosedReaded = false;
                    polyLineNReaded = false;
                }

                if (inSection == true && lineText == "AcDbCircle")
                {
                    inPathLine = false ; inPathArc = false; inPathCircle = true; inPathPolyLine = false;       
                }

                if (inSection == true && lineText == "AcDbArc")
                {
                    inPathLine = false; inPathArc = true; inPathCircle = false; inPathPolyLine = false;              
                }

                
                if (inPathLine == true)
                {
                    paths[pathCount].Type = LineType.Line;
                    if(lineText == " 10")
                    {
                        paths[pathCount].StartPoint.X = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 20")
                    {
                        paths[pathCount].StartPoint.Y = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 11")
                    {
                        paths[pathCount].EndPoint.X = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 21")
                    {
                        paths[pathCount].EndPoint.Y = Convert.ToDouble(sr.ReadLine());
                        pathCount++;
                        inPathLine = false;
                    }  
                }

                if (inPathPolyLine == true)
                {
                    if (lineText == " 90")
                    {
                        polyLineN = Convert.ToInt32(sr.ReadLine());
                        polyLineNReaded = true;
                    }
                    if (lineText == " 70")
                    {
                        if (Convert.ToInt32(sr.ReadLine()) == 1)
                        {
                            isClosed = true;
                        }
                        else
                        {
                            isClosed = false;
                        }
                        isClosedReaded = true;
                    }


                    if (polyLineNReaded == true && isClosedReaded == true )
                    {
                        int i = 0;
                        do
                        {
                            if (i == 0 && lineText == " 10")
                            {
                                paths[pathCount].Type = LineType.Line;
                                lineText = sr.ReadLine();
                                paths[pathCount].StartPoint.X = Convert.ToDouble(lineText);
                            }
                            if (i == 0 && lineText == " 20")
                            {
                                lineText = sr.ReadLine();
                                paths[pathCount].StartPoint.Y = Convert.ToDouble(lineText);
                                i++;
                                lineText = sr.ReadLine();
                            }

                            if (i == 1 && lineText == " 10")
                            {
                                lineText = sr.ReadLine();
                                paths[pathCount].EndPoint.X = Convert.ToDouble(lineText);
                            }
                            if (i == 1 && lineText == " 20")
                            {
                                lineText = sr.ReadLine();
                                paths[pathCount].EndPoint.Y = Convert.ToDouble(lineText);
                                i++;
                                pathCount++;
                            }
                      
                            if (i > 1 && lineText == " 10")
                            {
                                lineText = sr.ReadLine();
                                paths[pathCount].StartPoint.X = paths[pathCount - 1].EndPoint.X;
                                paths[pathCount].EndPoint.X = Convert.ToDouble(lineText);
                            }
                            if (i > 1 && lineText == " 20")
                            {
                                lineText = sr.ReadLine();
                                paths[pathCount].StartPoint.Y = paths[pathCount - 1].EndPoint.Y;
                                paths[pathCount].EndPoint.Y = Convert.ToDouble(lineText);
                                i++;
                                pathCount++;
                                if (isClosed == true && i == polyLineN)
                                {   
                                    paths[pathCount].StartPoint.X = paths[pathCount - 1].EndPoint.X;
                                    paths[pathCount].StartPoint.Y = paths[pathCount - 1].EndPoint.Y;
                                    paths[pathCount].EndPoint.X = paths[pathCount - polyLineN + 1].StartPoint.X;
                                    paths[pathCount].EndPoint.Y = paths[pathCount - polyLineN + 1].StartPoint.Y;
                                    pathCount++;
                                }
                            }     
                            lineText = sr.ReadLine();
                        } while (!(i == polyLineN));
                        inPathPolyLine = false;
                    }     
                }

                if (inPathCircle == true)
                {
                    paths[pathCount].Type = LineType.Circle;
                    if (lineText == " 10")
                    {
                        paths[pathCount].CentrePoint.X = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 20")
                    {
                        paths[pathCount].CentrePoint.Y = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 40")
                    {
                        paths[pathCount].Radium = Convert.ToDouble(sr.ReadLine());
                        pathCount++;
                        inPathCircle = false;
                    }
                }

                if (inPathArc == true)
                {
                    paths[pathCount - 1].Type = LineType.Arc;
                    if (lineText == " 50")
                    {
                        paths[pathCount - 1].StartAngle = Convert.ToDouble(sr.ReadLine());
                    }
                    if (lineText == " 51")
                    {
                        paths[pathCount - 1].EndAngle = Convert.ToDouble(sr.ReadLine());
                        inPathArc = false;
                    }
                }
            } while (!(inSection == true && lineText == "ENDSEC"||lineText == null));

            DxfdecoderDef.Path[] paths1 = new DxfdecoderDef.Path[pathCount];
            Array.Copy(paths, 0,paths1 ,0,paths1.Length);
            return paths1;
        }
    }
}
