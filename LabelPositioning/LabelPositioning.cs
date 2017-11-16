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
        private void AddPointToList(float Lng, float Lat)
        {
            LabelPosition labelPosition = new LabelPosition()
            {
                SitePoint = new Coord() { Lat = Lat, Lng = Lng, Ordinal = 0 },
                LabelPoint = new Coord() { Lat = Lat - 1, Lng = Lng + 1, Ordinal = 0 },
                LabelNorthEast = new Coord() { Lat = Lat - LabelHeight, Lng = Lng + LabelWidth, Ordinal = 0 },
                LabelSouthWest = new Coord() { Lat = Lat - 1, Lng = Lng + 1, Ordinal = 0 },
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
                AveragePoint = new Point((int)LabelPositionList.Average(c => c.SitePoint.Lng), (int)LabelPositionList.Average(c => c.SitePoint.Lat));

                foreach (LabelPosition labelPosition in LabelPositionList)
                {
                    labelPosition.LabelPoint = labelPosition.SitePoint;
                    labelPosition.Distance = (float)Math.Sqrt((labelPosition.SitePoint.Lng - AveragePoint.X) * (labelPosition.SitePoint.Lng - AveragePoint.X) + (labelPosition.SitePoint.Lat - AveragePoint.Y) * (labelPosition.SitePoint.Lat - AveragePoint.Y));

                    if ((labelPosition.SitePoint.Lng - AveragePoint.X) >= 0 && (labelPosition.SitePoint.Lat - AveragePoint.Y) <= 0) // first quartier
                    {
                        labelPosition.LabelSouthWest = new Coord() { Lat = labelPosition.SitePoint.Lat - 1, Lng = labelPosition.SitePoint.Lng + 1, Ordinal = 0 };
                        labelPosition.LabelNorthEast = new Coord() { Lat = labelPosition.SitePoint.Lat - LabelHeight - 1, Lng = labelPosition.SitePoint.Lng + LabelWidth + 1, Ordinal = 0 };
                        labelPosition.Position = PositionEnum.LeftBottom;
                    }
                    else if ((labelPosition.SitePoint.Lng - AveragePoint.X) > 0 && (labelPosition.SitePoint.Lat - AveragePoint.Y) > 0) // second quartier
                    {
                        labelPosition.LabelSouthWest = new Coord() { Lat = labelPosition.SitePoint.Lat + LabelHeight + 1, Lng = labelPosition.SitePoint.Lng + 1, Ordinal = 0 };
                        labelPosition.LabelNorthEast = new Coord() { Lat = labelPosition.SitePoint.Lat - 1, Lng = labelPosition.SitePoint.Lng + LabelWidth + 1, Ordinal = 0 };
                        labelPosition.Position = PositionEnum.LeftTop;
                    }
                    else if ((labelPosition.SitePoint.Lng - AveragePoint.X) < 0 && (labelPosition.SitePoint.Lat - AveragePoint.Y) > 0) // third quartier
                    {
                        labelPosition.LabelSouthWest = new Coord() { Lat = labelPosition.SitePoint.Lat + LabelHeight + 1, Lng = labelPosition.SitePoint.Lng - LabelWidth - 1, Ordinal = 0 };
                        labelPosition.LabelNorthEast = new Coord() { Lat = labelPosition.SitePoint.Lat + 1, Lng = labelPosition.SitePoint.Lng + 1, Ordinal = 0 };
                        labelPosition.Position = PositionEnum.RightTop;
                    }
                    else // forth quartier
                    {
                        labelPosition.LabelSouthWest = new Coord() { Lat = labelPosition.SitePoint.Lat - 1, Lng = labelPosition.SitePoint.Lng - LabelWidth - 1, Ordinal = 0 };
                        labelPosition.LabelNorthEast = new Coord() { Lat = labelPosition.SitePoint.Lat - LabelHeight - 1, Lng = labelPosition.SitePoint.Lng - 1, Ordinal = 0 };
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
                            new Coord() { Lat = labelPosition.LabelSouthWest.Lat, Lng = labelPosition.LabelSouthWest.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelSouthWest.Lat, Lng = labelPosition.LabelNorthEast.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelNorthEast.Lat, Lng = labelPosition.LabelNorthEast.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelNorthEast.Lat, Lng = labelPosition.LabelSouthWest.Lng, Ordinal = 0 },
                        };

                        bool PleaseRedo = false;
                        foreach (LabelPosition labelPosition2 in LabelPositionList.Where(c => c.Ordinal != labelPosition.Ordinal))
                        {
                            Coord coord = new Coord()
                            {
                                Lat = labelPosition2.LabelPoint.Lat,
                                Lng = labelPosition2.LabelPoint.Lng,
                                Ordinal = 0,
                            };
                            if (CoordInPolygon(coordList, coord))
                            {
                                float XNew = StepSize;
                                float YNew = StepSize;
                                float dist = (float)Math.Sqrt((AveragePoint.Y - labelPosition.SitePoint.Lat) * (AveragePoint.Y - labelPosition.SitePoint.Lat) + (AveragePoint.X - labelPosition.SitePoint.Lng) * (AveragePoint.X - labelPosition.SitePoint.Lng));
                                float factor = dist / StepSize;
                                float deltX = Math.Abs((AveragePoint.X - labelPosition.LabelPoint.Lng) / factor);
                                float deltY = Math.Abs((AveragePoint.Y - labelPosition.LabelPoint.Lat) / factor);
                                switch (labelPosition.Position)
                                {
                                    case PositionEnum.Error:
                                        break;
                                    case PositionEnum.LeftBottom:
                                        {
                                            XNew = labelPosition.LabelPoint.Lng + deltX;
                                            YNew = labelPosition.LabelPoint.Lat - deltY;
                                            labelPosition.LabelSouthWest = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
                                            labelPosition.LabelNorthEast = new Coord() { Lat = YNew - LabelHeight, Lng = XNew + LabelWidth, Ordinal = 0 };
                                        }
                                        break;
                                    case PositionEnum.RightBottom:
                                        {
                                            XNew = labelPosition.LabelPoint.Lng - deltX;
                                            YNew = labelPosition.LabelPoint.Lat - deltY;
                                            labelPosition.LabelSouthWest = new Coord() { Lat = YNew, Lng = XNew - LabelWidth, Ordinal = 0 };
                                            labelPosition.LabelNorthEast = new Coord() { Lat = YNew - LabelHeight, Lng = XNew, Ordinal = 0 };
                                        }
                                        break;
                                    case PositionEnum.LeftTop:
                                        {
                                            XNew = labelPosition.LabelPoint.Lng + deltX;
                                            YNew = labelPosition.LabelPoint.Lat + deltY;
                                            labelPosition.LabelSouthWest = new Coord() { Lat = YNew + LabelHeight, Lng = XNew, Ordinal = 0 };
                                            labelPosition.LabelNorthEast = new Coord() { Lat = YNew, Lng = XNew + LabelWidth, Ordinal = 0 };
                                        }
                                        break;
                                    case PositionEnum.RightTop:
                                        {
                                            XNew = labelPosition.LabelPoint.Lng - deltX;
                                            YNew = labelPosition.LabelPoint.Lat + deltY;
                                            labelPosition.LabelSouthWest = new Coord() { Lat = YNew + LabelHeight, Lng = XNew - LabelWidth, Ordinal = 0 };
                                            labelPosition.LabelNorthEast = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                labelPosition.LabelPoint = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
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
                            new Coord() { Lat = labelPosition.LabelSouthWest.Lat, Lng = labelPosition.LabelSouthWest.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelSouthWest.Lat, Lng = labelPosition.LabelNorthEast.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelNorthEast.Lat, Lng = labelPosition.LabelNorthEast.Lng, Ordinal = 0 },
                            new Coord() { Lat = labelPosition.LabelNorthEast.Lat, Lng = labelPosition.LabelSouthWest.Lng, Ordinal = 0 },
                        };

                        bool PleaseRedo = false;
                        foreach (LabelPosition labelPosition2 in LabelPositionList.Where(c => c.Ordinal != labelPosition.Ordinal && c.Distance <= labelPosition.Distance))
                        {
                            List<Coord> coordToCompare = new List<Coord>()
                            {
                                new Coord() { Lat = labelPosition2.LabelSouthWest.Lat, Lng = labelPosition2.LabelSouthWest.Lng, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelSouthWest.Lat, Lng = labelPosition2.LabelNorthEast.Lng, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelNorthEast.Lat, Lng = labelPosition2.LabelNorthEast.Lng, Ordinal = 0 },
                                new Coord() { Lat = labelPosition2.LabelNorthEast.Lat, Lng = labelPosition2.LabelSouthWest.Lng, Ordinal = 0 },
                            };
                            for (int i = 0; i < 4; i++)
                            {
                                if (CoordInPolygon(coordList, coordToCompare[i]))
                                {
                                    float XNew = StepSize;
                                    float YNew = StepSize;
                                    float dist = (float)Math.Sqrt((AveragePoint.Y - labelPosition.SitePoint.Lat) * (AveragePoint.Y - labelPosition.SitePoint.Lat) + (AveragePoint.X - labelPosition.SitePoint.Lng) * (AveragePoint.X - labelPosition.SitePoint.Lng));
                                    float factor = dist / StepSize;
                                    float deltX = Math.Abs((AveragePoint.X - labelPosition.LabelPoint.Lng) / factor);
                                    float deltY = Math.Abs((AveragePoint.Y - labelPosition.LabelPoint.Lat) / factor);
                                    switch (labelPosition.Position)
                                    {
                                        case PositionEnum.Error:
                                            break;
                                        case PositionEnum.LeftBottom:
                                            {
                                                XNew = labelPosition.LabelPoint.Lng + deltX;
                                                YNew = labelPosition.LabelPoint.Lat - deltY;
                                                labelPosition.LabelSouthWest = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
                                                labelPosition.LabelNorthEast = new Coord() { Lat = YNew - LabelHeight, Lng = XNew + LabelWidth, Ordinal = 0 };
                                            }
                                            break;
                                        case PositionEnum.RightBottom:
                                            {
                                                XNew = labelPosition.LabelPoint.Lng - deltX;
                                                YNew = labelPosition.LabelPoint.Lat - deltY;
                                                labelPosition.LabelSouthWest = new Coord() { Lat = YNew, Lng = XNew - LabelWidth, Ordinal = 0 };
                                                labelPosition.LabelNorthEast = new Coord() { Lat = YNew - LabelHeight, Lng = XNew, Ordinal = 0 };
                                            }
                                            break;
                                        case PositionEnum.LeftTop:
                                            {
                                                XNew = labelPosition.LabelPoint.Lng + deltX;
                                                YNew = labelPosition.LabelPoint.Lat + deltY;
                                                labelPosition.LabelSouthWest = new Coord() { Lat = YNew + LabelHeight, Lng = XNew, Ordinal = 0 };
                                                labelPosition.LabelNorthEast = new Coord() { Lat = YNew, Lng = XNew + LabelWidth, Ordinal = 0 };
                                            }
                                            break;
                                        case PositionEnum.RightTop:
                                            {
                                                XNew = labelPosition.LabelPoint.Lng - deltX;
                                                YNew = labelPosition.LabelPoint.Lat + deltY;
                                                labelPosition.LabelSouthWest = new Coord() { Lat = YNew + LabelHeight, Lng = XNew - LabelWidth, Ordinal = 0 };
                                                labelPosition.LabelNorthEast = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    labelPosition.LabelPoint = new Coord() { Lat = YNew, Lng = XNew, Ordinal = 0 };
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
                        g.DrawLine(new Pen(Color.Blue, 1.0f), new PointF(labelPosition.SitePoint.Lng, labelPosition.SitePoint.Lat), AveragePoint);
                        PointF p = new PointF();
                        switch (labelPosition.Position)
                        {
                            case PositionEnum.Error:
                                break;
                            case PositionEnum.LeftBottom:
                                {
                                    p = new PointF(labelPosition.LabelSouthWest.Lng, labelPosition.LabelSouthWest.Lat);
                                }
                                break;
                            case PositionEnum.RightBottom:
                                {
                                    p = new PointF(labelPosition.LabelNorthEast.Lng, labelPosition.LabelSouthWest.Lat);
                                }
                                break;
                            case PositionEnum.LeftTop:
                                {
                                    p = new PointF(labelPosition.LabelSouthWest.Lng, labelPosition.LabelNorthEast.Lat);
                                }
                                break;
                            case PositionEnum.RightTop:
                                {
                                    p = new PointF(labelPosition.LabelNorthEast.Lng, labelPosition.LabelNorthEast.Lat);
                                }
                                break;
                            default:
                                break;
                        }
                        g.DrawLine(new Pen(Color.Red, 1.0f), new PointF(labelPosition.SitePoint.Lng, labelPosition.SitePoint.Lat), p);
                        g.DrawRectangle(new Pen(Color.Red, 1.0f), labelPosition.LabelSouthWest.Lng, labelPosition.LabelNorthEast.Lat, labelPosition.LabelNorthEast.Lng - labelPosition.LabelSouthWest.Lng, labelPosition.LabelSouthWest.Lat - labelPosition.LabelNorthEast.Lat);
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
        public Coord SitePoint { get; set; }
        public Coord LabelPoint { get; set; }
        public PositionEnum Position { get; set; }
        public Coord LabelNorthEast { get; set; }
        public Coord LabelSouthWest { get; set; }
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

