using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LightToHeaven.util
{
    //Class for solve path of single photon
    class Solve
    {
        static public double[] run(double ax, double ay, double az)
        {
            try {
            Data data = Data.getInstance();

            LineTO lt = new LineTO(ax, ay, az, data.init_x, data.init_y, data.init_z);
            lt.solve();
            LineFrom lf;
            LineTO oldLt;
            while(true){
                oldLt = lt;
                lf = new LineFrom();
                lf.solve(lt);
                lt = new LineTO(lf.res_anglex, lf.res_angley, lf.res_anglez, lt.res_x, lt.res_y, lt.res_z);
                lt.solve();
                if (lt.res_z >= data.c) {
                    double[] res = findCross(data, lf.res_cosx, lf.res_cosy, lf.res_cosz, lt.res_x, lt.res_y, lt.res_z);
                    return res;
                }
            }
            return new double[] { };            
        }catch(Exception e){
            throw new Exception("ee");
         }
        }

        public static double[] findCross(Data data,double a,double b,double c,double x,double y,double z){
            double res_x = ((data.c - z) / c) * a + x;
            double res_y = ((data.c - z) / c) * b + y;
            double res_z = z;
            return new double[] { res_x, res_y, res_z };
        }
    }

    class Data
    {
        public double init_x, init_y, init_z;
        public double a, b;
        public double e, c;
        public double delta_x, delta_y;
        public double scale;
        public Label label_status;

        static private Data instance = null;
        private Data() { }
        public static Data getInstance(){
            if (instance == null)
            {
                instance = new Data();
            }
            return instance;
        }
    }

    class LineTO
    {
        public double res_x, res_y, res_z;
        public double t;
        public double angle_a, angle_b, angle_c;
        public double A,B,C,D;
        public double init_x, init_y, init_z;  

        public Data d;

        public LineTO(double a,double b,double c,double x,double y,double z)
        {
            d =  Data.getInstance();

            //Angle line lights
            angle_a = a;
            angle_b = b;
            angle_c = c;

            //Point from line light
            init_x = x;
            init_y = y;
            init_z = z;
        }

        //Solve roots of quadratic equation, and new position of lights
        public void solve()
        {
            solveD();
            solveT();
            res_x = t * Math.Cos(angle_a) + init_x;
            res_y = t * Math.Cos(angle_b) + init_y;
            res_z = t * Math.Cos(angle_c) + init_z; 
        }

        public void solveD(){
            A = Math.Pow(d.b, 2) * (Math.Pow(Math.Cos(angle_a), 2) + Math.Pow(Math.Cos(angle_b), 2)) + Math.Pow(d.a, 2) * Math.Pow(Math.Cos(angle_c), 2);
            B = 2 * Math.Pow(d.b, 2) * (Math.Cos(angle_a) * init_x + Math.Cos(angle_b) * init_y) + 2 * Math.Pow(d.a, 2) * Math.Cos(angle_c) * init_z;
            C = Math.Pow(d.b, 2) * (Math.Pow(init_x, 2) + Math.Pow(init_y, 2)) + Math.Pow(d.a, 2) * (Math.Pow(init_z, 2) - Math.Pow(d.b, 2));
            D = Math.Pow(B, 2) - 4 * A * C; 
        }

        public void solveT()
        {
            if (Math.Cos(angle_c) > 0)
            {
                t = (-B + Math.Sqrt(D))/(2*A);
            }
            else if(Math.Cos(angle_c) < 0){
                t = (-B - Math.Sqrt(D))/(2*A);
            }
            else
            {
                throw new Exception("cos(y) = 0 "+angle_c.ToString());
            }
            /*if (angle_c < (Math.PI))
            {
                t = (-B + Math.Sqrt(D)) / (2 * A);
            }
            else if (angle_c > Math.PI)
            {
                t = (-B - Math.Sqrt(D)) / (2 * A);
            }
            else
            {
                throw new Exception("cos(y) = 0");
            }*/
        }
    }

    class LineFrom{
        double xn, yn, zn;
        LineTO lt;
        Data d;

        double norm_n,norm_r,cos_phi;
        double vec_x,vec_y,vec_z;
        double res_x, res_y, res_z;
        public double res_cosx, res_cosy, res_cosz;
        public double res_anglex, res_angley, res_anglez;

        public void solve(LineTO lt){
            d = Data.getInstance();
            this.lt = lt;
            initN();
            solveVectorLine();
            solveNormN();
            solveNormR();
            solveCosPhi();
            solveResult();

        }

        public void initN(){
            xn = (lt.res_x)/Math.Pow(d.a,2);
            yn = (lt.res_y)/Math.Pow(d.a,2);
            zn = (lt.res_z)/Math.Pow(d.b,2);
        }

        public void solveNormN()
        {
            norm_n = Math.Sqrt(Math.Pow(xn,2)+Math.Pow(yn,2)+Math.Pow(zn,2));
        }
        public void solveNormR()
        {
            norm_r = Math.Sqrt(Math.Pow(vec_x,2)+Math.Pow(vec_y,2)+Math.Pow(vec_z,2));
        }

        public void solveCosPhi(){
            cos_phi = (xn*vec_x+yn*vec_y+zn*vec_z)/(norm_n*norm_r);
        }

        public void solveVectorLine()
        {
            vec_x = lt.res_x - lt.init_x;
            vec_y = lt.res_y - lt.init_y;
            vec_z = lt.res_z - lt.init_z;
        }

        public void solveResult()
        {
            double cos = (2 * cos_phi * norm_r) / norm_n;
            res_x = vec_x - cos * xn;
            res_y = vec_y - cos * yn;
            res_z = vec_z - cos * zn;

            double S = Math.Sqrt(Math.Pow(res_x, 2) + Math.Pow(res_y, 2) + Math.Pow(res_z,2));

            res_cosx = res_x / S;
            res_cosy = res_y / S;
            res_cosz = res_z / S;

           /* if(res_cosx == ){
                
            }*/

            res_anglex = Math.Acos(res_cosx);
            res_angley = Math.Acos(res_cosy);
            res_anglez = Math.Acos(res_cosz);

            double check = Math.Pow(res_cosx, 2) + Math.Pow(res_cosy, 2) + Math.Pow(res_cosz, 2); 
            double delta = 0.01;
            if ((check < (1 - delta))||(check > (delta + 1))) {
                throw new Exception("Result cos dont validate!");
            }
        }
    }
}
