using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabelPositioning
{
    public partial class LabelPositioning : Form
    {
        #region Variables
        float LabelWidth = 50.0f;
        float LabelHeight = 12.0f;
        float StepSize = 5;
        #endregion Variables

        #region Properties
        private List<LabelPosition> LabelPositionList { get; set; }
        private PointF AveragePoint { get; set; }
        #endregion Properties

        #region Constructors
        public LabelPositioning()
        {
            InitializeComponent();
            LabelPositionList = new List<LabelPosition>();
            AveragePoint = new PointF(0, 0);
        }
        #endregion Constructors

        #region Events
        private void butClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        private void pictureBoxLabelPosition_MouseClick(object sender, MouseEventArgs e)
        {
            AddPointToList(e.Location.X, e.Location.Y);
        }

        #endregion Events

        #region Functions public
        #endregion Functions public

        #region Functions private
        private void AddPointToList(float x, float y)
        {
            LabelPosition labelPosition = new LabelPosition()
            {
                SitePoint = new PointF(x, y),
                LabelPoint = new PointF(x, y),
                LabelRectangle = new RectangleF(x, y, 5, 5),
                Position = PositionEnum.LeftBottom,
                Distance = 0.0f,
                Ordinal = LabelPositionList.Count(),
            };
            LabelPositionList.Add(labelPosition);
            RedrawPointsAndLabels();
        }
        private void ClearAll()
        {
            LabelPositionList.Clear();
            AveragePoint = new Point(0, 0);

            RedrawPointsAndLabels();
        }
        private void RedrawPointsAndLabels()
        {
            //Font font = new Font("Arial", 10, FontStyle.Regular);
            //Brush brush = new SolidBrush(Color.LightBlue);

            if (LabelPositionList.Count > 0)
            {
                AveragePoint = new Point((int)LabelPositionList.Average(c => c.SitePoint.X), (int)LabelPositionList.Average(c => c.SitePoint.Y));

                foreach (LabelPosition labelPosition in LabelPositionList)
                {
                    labelPosition.LabelPoint = labelPosition.SitePoint;
                    labelPosition.Distance = (float)Math.Sqrt((labelPosition.SitePoint.X - AveragePoint.X) * (labelPosition.SitePoint.X - AveragePoint.X) + (labelPosition.SitePoint.Y - AveragePoint.Y) * (labelPosition.SitePoint.Y - AveragePoint.Y));

                    if ((labelPosition.SitePoint.X - AveragePoint.X) >= 0 && (labelPosition.SitePoint.Y - AveragePoint.Y) <= 0) // first quartier
                    {
                        labelPosition.LabelRectangle = new RectangleF(labelPosition.SitePoint.X + 1, labelPosition.SitePoint.Y - LabelHeight - 1, LabelWidth, LabelHeight);
                        labelPosition.Position = PositionEnum.LeftBottom;
                    }
                    else if ((labelPosition.SitePoint.X - AveragePoint.X) > 0 && (labelPosition.SitePoint.Y - AveragePoint.Y) > 0) // second quartier
                    {
                        labelPosition.LabelRectangle = new RectangleF(labelPosition.SitePoint.X + 1, labelPosition.SitePoint.Y + 1, LabelWidth, LabelHeight);
                        labelPosition.Position = PositionEnum.LeftTop;
                    }
                    else if ((labelPosition.SitePoint.X - AveragePoint.X) < 0 && (labelPosition.SitePoint.Y - AveragePoint.Y) > 0) // third quartier
                    {
                        labelPosition.LabelRectangle = new RectangleF(labelPosition.SitePoint.X - LabelWidth - 1, labelPosition.SitePoint.Y + 1, LabelWidth, LabelHeight);
                        labelPosition.Position = PositionEnum.RightTop;
                    }
                    else // forth quartier
                    {
                        labelPosition.LabelRectangle = new RectangleF(labelPosition.SitePoint.X - LabelWidth - 1, labelPosition.SitePoint.Y - LabelHeight - 1, LabelWidth, LabelHeight);
                        labelPosition.Position = PositionEnum.RightBottom;
                    }
                }
                foreach (LabelPosition labelPosition in LabelPositionList.OrderBy(c => c.Distance))
                {
                    bool HidingPoint = true;
                    while (HidingPoint)
                    {
                        List<Coord> coordList = new List<Coord>()
                        {
                            new Coord() { Lat = labelPosition.LabelRectangle.Y, Lng = labelPosition.LabelRectangle.X, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height, Lng = labelPosition.LabelRectangle.X, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height, Lng = labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y, Lng = labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, Ordinal = 0 },
                        };

                        bool PleaseRedo = false;
                        foreach (LabelPosition labelPosition2 in LabelPositionList.Where(c => c.Ordinal != labelPosition.Ordinal))
                        {
                            Coord coord = new Coord()
                            {
                                Lat = labelPosition2.LabelPoint.Y,
                                Lng = labelPosition2.LabelPoint.X,
                                Ordinal = 0,
                            };
                            if (CoordInPolygon(coordList, coord))
                            {
                                float XNew = StepSize;
                                float YNew = StepSize;
                                float dist = (float)Math.Sqrt((AveragePoint.Y - labelPosition.SitePoint.Y) * (AveragePoint.Y - labelPosition.SitePoint.Y) + (AveragePoint.X - labelPosition.SitePoint.X) * (AveragePoint.X - labelPosition.SitePoint.X));
                                float factor = dist / StepSize;
                                float deltX = Math.Abs((AveragePoint.X - labelPosition.LabelPoint.X) / factor);
                                float deltY = Math.Abs((AveragePoint.Y - labelPosition.LabelPoint.Y) / factor);
                                switch (labelPosition.Position)
                                {
                                    case PositionEnum.Error:
                                        break;
                                    case PositionEnum.LeftBottom:
                                        {
                                            XNew = labelPosition.LabelPoint.X + deltX;
                                            YNew = labelPosition.LabelPoint.Y - deltY;
                                            labelPosition.LabelRectangle = new RectangleF(XNew, YNew - LabelHeight, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                        }
                                        break;
                                    case PositionEnum.RightBottom:
                                        {
                                            XNew = labelPosition.LabelPoint.X - deltX;
                                            YNew = labelPosition.LabelPoint.Y - deltY;
                                            labelPosition.LabelRectangle = new RectangleF(XNew - LabelWidth, YNew - LabelHeight, LabelWidth, LabelHeight);
                                        }
                                        break;
                                    case PositionEnum.LeftTop:
                                        {
                                            XNew = labelPosition.LabelPoint.X + deltX;
                                            YNew = labelPosition.LabelPoint.Y + deltY;
                                            labelPosition.LabelRectangle = new RectangleF(XNew, YNew, LabelWidth, LabelHeight);
                                        }
                                        break;
                                    case PositionEnum.RightTop:
                                        {
                                            XNew = labelPosition.LabelPoint.X - deltX;
                                            YNew = labelPosition.LabelPoint.Y + deltY;
                                            labelPosition.LabelRectangle = new RectangleF(XNew - LabelWidth - 1, YNew, LabelWidth, LabelHeight);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                labelPosition.LabelPoint = new PointF(XNew, YNew);
                                PleaseRedo = true;
                                break;
                            }
                        }
                        if (!PleaseRedo)
                        {
                            HidingPoint = false;
                        }
                    }

                    HidingPoint = true;
                    while (HidingPoint)
                    {
                        List<Coord> coordList = new List<Coord>()
                        {
                            new Coord() { Lat = labelPosition.LabelRectangle.Y, Lng = labelPosition.LabelRectangle.X, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height, Lng = labelPosition.LabelRectangle.X, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height, Lng = labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelRectangle.Y, Lng = labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, Ordinal = 0 },
                        };

                        bool PleaseRedo = false;
                        foreach (LabelPosition labelPosition2 in LabelPositionList.Where(c => c.Ordinal != labelPosition.Ordinal && c.Distance <= labelPosition.Distance))
                        {
                            List<Coord> coordToCompare = new List<Coord>()
                            {
                                new Coord() { Lat = labelPosition2.LabelRectangle.Y, Lng = labelPosition2.LabelRectangle.X, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelRectangle.Y + labelPosition2.LabelRectangle.Height, Lng = labelPosition2.LabelRectangle.X, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelRectangle.Y + labelPosition2.LabelRectangle.Height, Lng = labelPosition2.LabelRectangle.X + labelPosition2.LabelRectangle.Width, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelRectangle.Y, Lng = labelPosition2.LabelRectangle.X + labelPosition2.LabelRectangle.Width, Ordinal = 0 },
                            };
                            for (int i = 0; i < 4; i++)
                            {
                                if (CoordInPolygon(coordList, coordToCompare[i]))
                                {
                                    float XNew = StepSize;
                                    float YNew = StepSize;
                                    float dist = (float)Math.Sqrt((AveragePoint.Y - labelPosition.SitePoint.Y) * (AveragePoint.Y - labelPosition.SitePoint.Y) + (AveragePoint.X - labelPosition.SitePoint.X) * (AveragePoint.X - labelPosition.SitePoint.X));
                                    float factor = dist / StepSize;
                                    float deltX = Math.Abs((AveragePoint.X - labelPosition.LabelPoint.X) / factor);
                                    float deltY = Math.Abs((AveragePoint.Y - labelPosition.LabelPoint.Y) / factor);
                                    switch (labelPosition.Position)
                                    {
                                        case PositionEnum.Error:
                                            break;
                                        case PositionEnum.LeftBottom:
                                            {
                                                XNew = labelPosition.LabelPoint.X + deltX;
                                                YNew = labelPosition.LabelPoint.Y - deltY;
                                                labelPosition.LabelRectangle = new RectangleF(XNew, YNew - LabelHeight, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                            }
                                            break;
                                        case PositionEnum.RightBottom:
                                            {
                                                XNew = labelPosition.LabelPoint.X - deltX;
                                                YNew = labelPosition.LabelPoint.Y - deltY;
                                                labelPosition.LabelRectangle = new RectangleF(XNew - LabelWidth, YNew - LabelHeight, LabelWidth, LabelHeight);
                                            }
                                            break;
                                        case PositionEnum.LeftTop:
                                            {
                                                XNew = labelPosition.LabelPoint.X + deltX;
                                                YNew = labelPosition.LabelPoint.Y + deltY;
                                                labelPosition.LabelRectangle = new RectangleF(XNew, YNew, LabelWidth, LabelHeight);
                                            }
                                            break;
                                        case PositionEnum.RightTop:
                                            {
                                                XNew = labelPosition.LabelPoint.X - deltX;
                                                YNew = labelPosition.LabelPoint.Y + deltY;
                                                labelPosition.LabelRectangle = new RectangleF(XNew - LabelWidth - 1, YNew, LabelWidth, LabelHeight);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    labelPosition.LabelPoint = new PointF(XNew, YNew);
                                    PleaseRedo = true;
                                    break;
                                }
                                if (PleaseRedo)
                                {
                                    break;
                                }
                            }
                        }
                        if (!PleaseRedo)
                        {
                            HidingPoint = false;
                        }
                    }
                }
                using (Graphics g = pictureBoxLabelPosition.CreateGraphics())
                {
                    g.Clear(Color.White);

                    foreach (LabelPosition labelPosition in LabelPositionList)
                    {
                        g.DrawLine(new Pen(Color.Blue, 1.0f), labelPosition.SitePoint, AveragePoint);
                        switch (labelPosition.Position)
                        {
                            case PositionEnum.Error:
                                break;
                            case PositionEnum.LeftBottom:
                                {
                                    PointF p = new PointF(labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height);
                                    g.DrawLine(new Pen(Color.Red, 1.0f), labelPosition.SitePoint, p);
                                    g.DrawRectangle(new Pen(Color.Red, 1.0f), labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                }
                                break;
                            case PositionEnum.RightBottom:
                                {
                                    PointF p = new PointF(labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Y + labelPosition.LabelRectangle.Height);
                                    g.DrawLine(new Pen(Color.Red, 1.0f), labelPosition.SitePoint, p);
                                    g.DrawRectangle(new Pen(Color.Red, 1.0f), labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                }
                                break;
                            case PositionEnum.LeftTop:
                                {
                                    PointF p = new PointF(labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y);
                                    g.DrawLine(new Pen(Color.Red, 1.0f), labelPosition.SitePoint, p);
                                    g.DrawRectangle(new Pen(Color.Red, 1.0f), labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                }
                                break;
                            case PositionEnum.RightTop:
                                {
                                    PointF p = new PointF(labelPosition.LabelRectangle.X + labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Y);
                                    g.DrawLine(new Pen(Color.Red, 1.0f), labelPosition.SitePoint, p);
                                    g.DrawRectangle(new Pen(Color.Red, 1.0f), labelPosition.LabelRectangle.X, labelPosition.LabelRectangle.Y, labelPosition.LabelRectangle.Width, labelPosition.LabelRectangle.Height);
                                }
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
        }

        public bool CoordInPolygon(List<Coord> poly, Coord pnt)
        {
            int i, j;
            int nvert = poly.Count;
            bool InPoly = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((poly[i].Lat > pnt.Lat) != (poly[j].Lat > pnt.Lat)) &&
                 (pnt.Lng < (poly[j].Lng - poly[i].Lng) * (pnt.Lat - poly[i].Lat) / (poly[j].Lat - poly[i].Lat) + poly[i].Lng))
                    InPoly = !InPoly;
            }
            return InPoly;
        }
        #endregion Functions private

    }
    public enum PositionEnum
    {
        Error = 0,
        LeftBottom = 1,
        RightBottom = 2,
        LeftTop = 3,
        RightTop = 4,
    }

    public class LabelPosition
    {
        public PointF SitePoint { get; set; }
        public PointF LabelPoint { get; set; }
        public PositionEnum Position { get; set; }
        public RectangleF LabelRectangle { get; set; }
        public float Distance { get; set; }
        public int Ordinal { get; set; }

    }
    public class Coord
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
        public int Ordinal { get; set; }
    }
}

