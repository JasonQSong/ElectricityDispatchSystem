using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectricityWcfService
{
    public class Controller
    {
        public void Forecast(int StationID, DateTime TargetDate)
        {
            TargetDate = TargetDate.Date;
            DatabaseConnector dc = new DatabaseConnector();
            if (dc.SelectForecastDayStationData(StationID, TargetDate, 1).Count > 0)
                return;
            List<List<RuntimeStationData>> HistoryData = GetPredictData(StationID, TargetDate);
            List<ForecastDayStationData> DataList = dayforecastslot(HistoryData);
            foreach (ForecastDayStationData Record in DataList)
            {
                Record.StationID = StationID;
                Record.Time = TargetDate + TimeSpan.FromMinutes(Record.ID * 15);
                dc.AddForecastDayStationData(Record);
            }
        }

        private List<List<RuntimeStationData>> GetPredictData(int StationID, DateTime TargetDate)
        {
            ElectricityService es = new ElectricityService();
            TargetDate = TargetDate.Date;
            DateTime StartDate = TargetDate - TimeSpan.FromDays(14);
            List<List<RuntimeStationData>> HistoryData = new List<List<RuntimeStationData>>();
            for (int i = 0; i < 14; i++)
            {
                List<RuntimeStationData> DayHistoryData = es.SelectRuntimeStationData(StationID, StartDate + TimeSpan.FromDays(i)).ToList();
                HistoryData.Add(DayHistoryData);
            }
            return HistoryData;
        }
        List<ForecastDayStationData> dayforecastslot(List<List<RuntimeStationData>> HistoryData)//进行预测
        {
            double[][] lp = new double[14][];
            for (int day = 0; day < 14; day++)//读入数据
            {
                lp[day] = new double[96];
                bool[] hasvalue = new bool[96];
                for (int ii = 0; ii < 96; ii++)
                {
                    lp[day][ii] = 0;
                    hasvalue[ii] = false;
                }
                for (int i = 0; i < HistoryData[day].Count; i++)
                {
                    int ii = (int)((HistoryData[day][i].Time.Subtract(HistoryData[day][i].Time.Date).TotalMinutes) / (TimeSpan.FromMinutes(15).TotalMinutes));
                    lp[day][ii] = HistoryData[day][i].ActivePower;
                    hasvalue[ii] = true;
                }
                for (int ii = 1; ii < 96; ii++)
                    if (!hasvalue[ii])
                        lp[day][ii] = lp[day][ii - 1];
            }
            double[][] germane = new double[14][];//相关负荷集合，将负荷重新排序
            for (int i = 0; i < 14; i++)
                germane[i] = new double[96];
            for (int i = 0; i < 96; i++)
            {
                germane[0][i] = lp[6][i];
                germane[1][i] = lp[13][i];
                for (int j = 2; j < 8; j++)
                {
                    germane[j][i] = lp[j - 2][i];
                }
                for (int j = 8; j < 14; j++)
                {
                    germane[j][i] = lp[j - 1][i];
                }
            }
            double[] pf1 = new double[96];//正常日点对点倍比法预测结果
            for (int t = 0; t < 96; t++)
            {
                pf1[t] = pointtopointradio(germane, t);
            }

            double[] pf2 = new double[96];//正常日倍比平滑法预测结果
            for (int t = 0; t < 96; t++)
            {
                pf2[t] = smooth(germane, t);
            }

            double[] pf3 = new double[96];//灰色预测
            for (int t = 0; t < 96; t++)
            {
                pf3[t] = daygrey(germane, t);
            }

            double[] pf4 = new double[96];
            for (int t = 0; t < 96; t++)
            {
                pf4[t] = variationcoefficient(lp, t);//简单直观的变化系数法
            }
            List<ForecastDayStationData> DataList = new List<ForecastDayStationData>();
            ForecastDayStationData Record;
            for (int ii = 0; ii < 96; ii++)
            {
                Record = new ForecastDayStationData()
                {
                    ID=ii,
                    ForecastType = 1,
                    ActivePower = pf1[ii],
                };
                DataList.Add(Record);
                Record = new ForecastDayStationData()
                {
                    ID = ii,
                    ForecastType = 2,
                    ActivePower = pf2[ii] 
                };
                DataList.Add(Record);
                Record = new ForecastDayStationData()
                {
                    ID = ii,
                    ForecastType = 3,
                    ActivePower = pf3[ii] 
                };
                DataList.Add(Record);
                Record = new ForecastDayStationData()
                {
                    ID = ii,
                    ForecastType = 4,
                    ActivePower = pf4[ii] 
                };
                DataList.Add(Record);
            }
            return DataList;
        }

        double pointtopointradio(double[][] germane, int t)//正常日点对点倍比法
        {
            double a = 0.5;//负荷平滑系数
            double a1, a2;
            double aa;
            double pf1;//正常日点对点倍比法预测结果
            a1 = 0;
            aa = a;
            for (int i = 2; i < 8; i++)
            {
                a1 += aa * germane[i][t];
                aa = aa - aa * a;
            }

            a2 = 0;
            aa = a;
            for (int i = 8; i < 14; i++)
            {
                a2 += aa * germane[i][t];
                aa = aa - aa * a;
            }
            pf1 = a1 * germane[0][t] / a2;
            return pf1;
        }

        double smooth(double[][] germane, int t)//正常日倍比平滑法
        {
            double a = 0.75;//负荷平滑系数
            double[] pf2 = new double[96];//正常日倍比平滑法预测结果
            double[] A = new double[14];//日平均负荷,作为基值，即基值选取采用均荷方式
            double[][] L = new double[14][];//负荷的标幺值
            for (int i = 0; i < 14; i++)
                L[i] = new double[96];

            for (int i = 0; i < 14; i++)	//求出各点负荷的标幺值,基值选取采用均荷方式
            {
                for (int j = 0; j < 96; j++)
                {
                    A[i] += germane[i][j];
                }
                A[i] = A[i] / 96;
                for (int j = 0; j < 96; j++)
                {
                    L[i][j] = germane[i][j] / A[i];
                }
            }

            double[] lf = new double[96];//标幺预测值
            double LL;//LL为方便编程引入的变量
            //标幺曲线预测
            lf[t] = a * L[0][t];
            LL = a;
            for (int j = 1; j < 14; j++)
            {
                LL = (1 - a) * LL;
                lf[t] += LL * L[j][t];
            }

            double A0;//基值的预测值
            double A1, A2, AA;//A1,A2分别为第一、第二周期中不同类型日的基值平滑值，AA1,AA2为方便编程引入的变量
            A1 = a * A[2];
            AA = a;
            for (int i = 3; i < 6; i++)//本应循环到i<8，但结果超出double精度范围，故采用i<6
            {
                AA = (1 - a) * AA;
                A1 += AA * A[i];
            }
            A2 = a * A[8];
            AA = a;
            for (int i = 9; i < 12; i++)
            {
                AA = (1 - a) * AA;
                A2 += AA * A[i];
            }
            A0 = A1 * A[0] / A2;

            pf2[t] = lf[t] * A0;//预测的有名值为标幺值乘以基值

            return pf2[t];
            //return A2;
        }

        double daygrey(double[][] germane, int t)//灰色预测
        {
            double[] lp = new double[14];
            for (int i = 0; i < 14; i++)
            {
                lp[i] = germane[i][t];
            }
            double[] lp1 = new double[14];


            for (int j = 0; j < 14; j++)
            {
                lp1[j] = lp[j];
            }
            for (int j = 1; j < 14; j++)
            {
                lp1[j] += lp1[j - 1];
            }

            double[][] B ={//13,2
                new double[]{-(lp1[0]+lp1[1])/2,1},
                new double[]{-(lp1[1]+lp1[2])/2,1},
                new double[]{-(lp1[2]+lp1[3])/2,1},
                new double[]{-(lp1[3]+lp1[4])/2,1},
                new double[]{-(lp1[4]+lp1[5])/2,1},
                new double[]{-(lp1[5]+lp1[6])/2,1},
                new double[]{-(lp1[6]+lp1[7])/2,1},
                new double[]{-(lp1[7]+lp1[8])/2,1},
                new double[]{-(lp1[8]+lp1[9])/2,1},
                new double[]{-(lp1[9]+lp1[10])/2,1},
                new double[]{-(lp1[10]+lp1[11])/2,1},
                new double[]{-(lp1[11]+lp1[12])/2,1},
                new double[]{-(lp1[12]+lp1[13])/2,1}
	        };
            double[][] A = new double[2][];
            for (int i = 0; i < 2; i++)
                A[i] = new double[13];

            for (int r = 0; r < 13; r++)//矩阵的转置
            {
                for (int tt = 0; tt < 2; tt++)
                {
                    A[tt][r] = B[r][tt];
                }
            }
            double[][] C = new double[2][];
            for (int i = 0; i < 2; i++)
                C[i] = new double[2];
            double[][] G = new double[2][];
            for (int i = 0; i < 2; i++)
                G[i] = new double[1];
            double de;
            double[][] D = new double[2][];
            for (int i = 0; i < 2; i++)
                D[i] = new double[2];
            double[][] Y = new double[13][];
            for (int i = 0; i < 13; i++)
                Y[i] = new double[1] { lp[i + 1] };

            for (int i = 0; i < 2; i++)//矩阵的运算
            {
                for (int m = 0; m < 2; m++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        C[i][m] += A[i][j] * B[j][m];
                        A[i][j] = A[i][j];
                        B[j][m] = B[j][m];
                    }
                }
            }

            de = 1 / (C[0][0] * C[1][1] - C[0][1] * C[1][0]);
            D[0][0] = de * C[1][1]; D[0][1] = -de * C[0][1]; D[1][0] = -de * C[1][0]; D[1][1] = de * C[0][0];

            double[][] aaa = new double[2][];//矩阵的运算
            for (int i = 0; i < 2; i++)
                aaa[i] = new double[13];
            for (int i = 0; i < 2; i++)
            {
                for (int m = 0; m < 13; m++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        aaa[i][m] += D[i][j] * A[j][m];
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int m = 0; m < 1; m++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        G[i][m] += aaa[i][j] * Y[j][m];
                    }
                }
            }

            double w = G[0][0];//得到拟合方程的参数
            double u = G[1][0];

            double pyuce = ((lp1[0] - u / w) * Math.Exp(-w * 14) + u / w) - ((lp1[0] - u / w) * Math.Exp(-w * 13) + u / w);

            return pyuce;
        }


        double variationcoefficient(double[][] lp, int t)//简单直观的变化系数法
        {
            double pave = 0;//历史各日各时的负荷均值
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 96; j++)
                {
                    pave += lp[i][j];
                }
            }
            pave = pave / (14 * 96);

            double[] pave2 = new double[14];//各日的平均负荷
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 96; j++)
                {
                    pave2[i] += lp[i][j];
                }
                pave2[i] = pave2[i] / 96;
            }

            double[] p = new double[96];//各时段的周期系数
            for (int j = 0; j < 96; j++)
            {
                for (int i = 0; i < 14; i++)
                {
                    p[j] += lp[i][j] / pave2[i];
                }
                p[j] = p[j] / 14;
            }

            double pf4;//预测结果
            pf4 = pave * p[t];
            return pf4;
        }
    }
}