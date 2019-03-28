using System;

/// <summary>
/// Dxf文件解码工程相关类
/// </summary>
namespace DxfdecoderDef
{
    /// <summary>
    /// 直角坐标类
    /// </summary>
    public class  Point 
    {
        private double x;
        private double y;
        private double z;

        /// <summary>
        /// 无参数的构造函数，所有坐标赋值为0
        /// </summary>
        public Point()
        {
            this.x = 0.0;
            this.y = 0.0;
            this.z = 0.0;
        }

        /// <summary>
        /// 二维坐标构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(double x,double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 三维坐标构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point(double x, double y,double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;    
        }

        /// <summary>
        /// X坐标
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        /// <summary>
        /// 定义 + 运算符重载，返回值为每个坐标相加之后的新坐标
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Point operator + (Point left,Point right)
        {
            Point p1 = new Point();
            p1.x = left.x + right.x;
            p1.y = left.y + right.y;
            p1.z = left.z + right.z;
            return p1;
        }

        /// <summary>
        /// 定义 - 运算符重载，返回值为每个坐标相减之后的新坐标
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Point operator - (Point left, Point right)
        {
            Point p1 = new Point();
            p1.x = left.x - right.x;
            p1.y = left.y - right.y;
            p1.z = left.z - right.z;
            return p1;
        }

        /// <summary>
        /// 定义 % 运算符重载,返回值为两个坐标的距离
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static double operator % (Point left, Point right)
        {
            return Math.Sqrt((left.x - right.x) * (left.x - right.x) + (left.y - right.y) * (left.y - right.y) + (left.z - right.z) * (left.z - right.z));
        }

     
        ///// <summary>
        ///// 定义 == 运算符的重载，判断两个坐标是否相等
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static bool operator ==(point left, point right)
        //{
        //    if(left.x == right.x && left.y == right.y && left.z == right.z)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// 定义 != 运算符的重载，判断两个坐标是否不等
        ///// </summary>
        ///// <param name="left"></param>
        ///// <param name="right"></param>
        ///// <returns></returns>
        //public static bool operator !=(point left, point right)
        //{
        //    if (left.x != right.x || left.y != right.y || left.z != right.z)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }

    /// <summary>
    /// 路径类型的枚举
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// 直线
        /// </summary>
        Line = 0,
        /// <summary>
        /// 整圆
        /// </summary>
        Circle = 1,
        /// <summary>
        /// 圆弧
        /// </summary>
        Arc = 2,
        /// <summary>
        /// 多段线
        /// </summary>
        Polyline = 3 
    }

    /// <summary>
    /// 路径类：定义某种线条的属性，包括与其相关的方法
    /// </summary>
    public class Path
    {
        //private bool isNull;  //是否是空路径
        private LineType type; //路径类型
        //直线参数//
        private Point sPoint; //起点坐标
        private Point ePoint; //终点坐标
        //整圆圆弧共享参数//
        private Point centre; //圆心坐标
        private double radium; //半径
        //圆弧独占参数//
        private double sAngle; //起始角
        private double eAngle; //终止角
        private int ismajorarc; //是否为优弧 ： 0 优弧， 1劣弧  2自身不是圆弧类型

        /// <summary>
        /// 不含参数的构造函数重载
        /// </summary>
        public Path()
        {
            this.type = LineType.Line;
            this.sPoint = new Point();
            this.ePoint = new Point();
            this.centre = new Point();
            this.radium = 0.0;
            this.sAngle = 0.0;
            this.eAngle = 0.0;
        }

        /// <summary>
        /// 直线的构造函数重载
        /// </summary>
        /// <param name="sP"></param>
        /// <param name="eP"></param>
        public Path(Point sP, Point eP)
        {
            this.type = LineType.Line;
            this.sPoint = sP;
            this.ePoint = eP;
            this.centre = new Point();
            this.radium = 0.0;
            this.sAngle = 0.0;
            this.eAngle = 0.0;
        }

        /// <summary>
        /// 整圆的构造函数重载
        /// </summary>
        /// <param name="cet"></param>
        /// <param name="rad"></param>
        public Path(Point cet, double rad)
        {
            this.type = LineType.Circle;
            this.sPoint = new Point();
            this.ePoint = new Point();
            this.centre = cet;
            this.radium = rad;
            this.sAngle = 0.0;
            this.eAngle = 0.0;
        }

        /// <summary>
        /// 圆弧的构造函数重载
        /// </summary>
        /// <param name="cet"></param>
        /// <param name="rad"></param>
        /// <param name="sAng"></param>
        /// <param name="eAng"></param>
        public Path(Point cet, double rad, double sAng, double eAng)
        {
            this.type = LineType.Arc;
            this.sPoint = new Point();
            this.ePoint = new Point();
            this.centre = cet;
            this.radium = rad;
            this.sAngle = sAng;
            this.eAngle = eAng;
        }

        /// <summary>
        /// 路径类型
        /// </summary>
        public LineType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 直线的起点坐标
        /// </summary>
        public Point StartPoint
        {
            get { return sPoint; }
            set { sPoint = value; }
        }

        /// <summary>
        /// 直线的终点坐标
        /// </summary>
        public Point EndPoint
        {
            get { return ePoint; }
            set { ePoint = value; }
        }

        /// <summary>
        /// 圆心（圆弧或者整圆类型）
        /// </summary>
        public Point CentrePoint
        {
            get { return centre; }
            set { centre = value; }
        }

        /// <summary>
        /// 半径（圆弧或者整圆类型）
        /// </summary>
        public double Radium
        {
            get { return radium; }
            set { radium = value; }
        }

        /// <summary>
        /// 圆弧的起始角度（逆时针）
        /// </summary>
        public double StartAngle
        {
            get { return sAngle; }
            set { sAngle = value; }
        }

        /// <summary>
        /// 圆弧的终止角度（逆时针）
        /// </summary>
        public double EndAngle
        {
            get { return eAngle; }
            set { eAngle = value; }
        }

        /// <summary>
        /// 是否为优弧： 0 劣弧， 1优弧，  2自身不是圆弧类型
        /// </summary>
        public int IsMajorArc
        {
            get
            {
                if (this.type == LineType.Arc)
                {
                    if (GetCentrelAngle() > 180.000)
                    {
                        ismajorarc = 1;
                        return ismajorarc;
                    }
                    else
                    {
                        ismajorarc = 0;
                        return ismajorarc;
                    }
                }
                else
                {
                    ismajorarc = 2;
                    return ismajorarc;
                }
            }
        }

        /// <summary>
        /// 计算返回圆弧起点和终点，逆时针（圆弧类型）
        /// </summary>
        /// <returns></returns>
        public Point[] GetArcTeminal()
        {
            Point[] p = new Point[2]; //p[0]起点  p[1]终点  逆时针
            for (int i = 0; i < p.Length; i++)
            {
                p[i] = new Point();
            }
            p[0].X = centre.X + radium * Math.Cos(sAngle / (180 / Math.PI));
            p[0].Y = centre.Y + radium * Math.Sin(sAngle / (180 / Math.PI));
            p[1].X = centre.X + radium * Math.Cos(eAngle / (180 / Math.PI));
            p[1].Y = centre.Y + radium * Math.Sin(eAngle / (180 / Math.PI));

            return p;
        }

        /// <summary>
        /// 计算返回路径长度（所有类型）
        /// </summary>
        /// <returns></returns>
        public double GetPathLength()
        {
            switch (type)
            {
                case LineType.Line: return sPoint % ePoint;
                case LineType.Circle: return 2 * Math.PI * radium;
                case LineType.Arc: return Math.Abs(sAngle - eAngle) / (180.0 / Math.PI) * radium;
                default: return 0.0;
            }
        }

        /// <summary>
        /// 求圆弧的圆心角，角度
        /// </summary>
        /// <returns></returns>
        public double GetCentrelAngle()
        {
            if(sAngle < eAngle)
            {
                return eAngle - sAngle;
            } 
            else
            {
                return 360.000 - sAngle + eAngle;
            }           
        }

        /// <summary>
        /// 判断圆弧与轴线的相交情况
        /// </summary>
        /// <returns></returns>
        public int GetNode()
        {
            int node = 0;
            if(sAngle < eAngle &&       //无交点
                ((sAngle > 0.001 && sAngle < 89.999 && eAngle > 0.001 && eAngle < 89.999))||
                ((sAngle > 90.001 && sAngle < 179.999)&&(eAngle > 90.001 && eAngle < 179.999)) ||
                ((sAngle > 180.001 && sAngle < 269.999)&&(eAngle > 180.001 && eAngle < 269.999)) ||
                ((sAngle > 270.001 && sAngle < 359.999)&&(eAngle > 270.001 && eAngle < 359.999)))
            {
                node = 0000;
                return node;
            }

            if(sAngle > eAngle &&  //一个交点X正
                sAngle > 270.001 && eAngle < 89.999)
            {
                node = 0001;
                return node;
            }

            if (sAngle < eAngle &&  //一个交点Y正
                (sAngle > 0.001 && sAngle < 89.999)&&
                (eAngle > 90.001 && eAngle < 179.999))
            {
                node = 0010;
                return node;
            }

            if (sAngle < eAngle &&  //一个交点X负
                (sAngle > 90.001 && sAngle < 179.999)&&
                (eAngle > 180.001 && eAngle < 269.999))
            {
                node = 0100;
                return node;
            }

            if (sAngle < eAngle &&  //一个交点Y负
                 (sAngle > 180.001 && sAngle < 269.999)&&
                 (eAngle > 270.001 && eAngle < 359.999))
            {
                node = 1000;
                return node;
            }

            if (sAngle > eAngle &&  //两个交点X正，Y正
                 sAngle > 270.001 &&
                 eAngle > 90.001 && eAngle < 179.999)
            {
                node = 0011;
                return node;
            }

            if (sAngle < eAngle &&  //两个交点Y正，X负
                 (sAngle > 0.001 && sAngle < 89.999) &&
                 (eAngle > 180.001 && eAngle < 269.999))
            {
                node = 0110;
                return node;
            }

            if (sAngle < eAngle &&  //两个交点X负，Y负
                 (sAngle > 90.001 && sAngle < 179.999) &&
                 (eAngle > 270.001 && eAngle < 359.999))
            {
                node = 1100;
                return node;
            }

            if (sAngle > eAngle &&  //两个交点Y负，X正
                 (sAngle > 180.001 && sAngle < 269.999) &&
                 (eAngle > 0.001 && eAngle < 89.999))
            {
                node = 1001;
                return node;
            }

            if (sAngle > eAngle &&  //三个交点X正，Y正，X负
                 (sAngle > 270.001 && sAngle < 359.999) &&
                 (eAngle > 180.001 && eAngle < 269.999))
            {
                node = 0111;
                return node;
            }

            if (sAngle < eAngle &&  //三个交点Y正，X负，Y负
                 (sAngle > 0.001 && sAngle < 89.999) &&
                 (eAngle > 270.001 && eAngle < 359.999))
            {
                node = 1110;
                return node;
            }

            if (sAngle > eAngle &&  //三个交点X负，Y负，X正
                (sAngle > 90.001 && sAngle < 179.999) &&
                (eAngle > 0.001 && eAngle < 89.999))
            {
                node = 1101;
                return node;
            }

            if (sAngle > eAngle &&  //三个交点Y负，X正，Y正
                 (sAngle > 180.001 && sAngle < 269.999) &&
                 (eAngle > 90.001 && eAngle < 179.999))
            {
                node = 1011;
                return node;
            }

            if (sAngle > eAngle &&       //四个交点
                ((sAngle > 0.001 && sAngle < 89.999 && eAngle > 0.001 && eAngle < 89.999)) ||
                ((sAngle > 90.001 && sAngle < 179.999) && (eAngle > 90.001 && eAngle < 179.999)) ||
                ((sAngle > 180.001 && sAngle < 269.999) && (eAngle > 180.001 && eAngle < 269.999)) ||
                ((sAngle > 270.001 && sAngle < 359.999) && (eAngle > 270.001 && eAngle < 359.999)))
            {
                node = 1111;
                return node;
            }

            node = 65535;
            return node;
        }
    
        /// <summary>
        /// 清除此路径的数据
        /// </summary>
        public void Clear()
        {
            this.type = LineType.Line;
            this.sPoint = new Point();
            this.ePoint = new Point();
            this.centre = new Point();
            this.radium = 0.0;
            this.sAngle = 0.0;
            this.eAngle = 0.0;
        }

        /// <summary>
        /// 将此类实例里的属性克隆给另一实例
        /// </summary>
        /// <returns></returns>
        public Path Clone()
        {
            Path p1 = new Path();
            p1.type = this.type;
            p1.centre.X = this.centre.X;
            p1.centre.Y = this.centre.Y;
            p1.eAngle = this.eAngle;
            p1.ePoint.X = this.ePoint.X;
            p1.ePoint.Y = this.ePoint.Y;
            p1.radium = this.radium;
            p1.sAngle = this.sAngle;
            p1.sPoint.X = this.sPoint.X;
            p1.sPoint.Y = this.sPoint.Y;
            return p1;
        }
    } 
}
