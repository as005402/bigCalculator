using System;
using System.Linq;

namespace bigCalculator
{
    class calculator
    {
        string val1, val2;
        byte[] v1;
        byte[] v2;
        char op;
        string res;

        public calculator(string exp)
        {
            if (exp.IndexOf('+') != -1) op = '+';
            else if (exp.IndexOf('-') != -1) op = '-';
            else if (exp.IndexOf('*') != -1) op = '*';
            else if (exp.IndexOf('/') != -1) op = '/';

            val1 = exp.Split(op)[0].Trim();
            val2 = exp.Split(op)[1].Trim();

            v1 = strToByteArr(val1);
            v2 = strToByteArr(val2);

            calc();
        }

        void calc()
        {
            switch (op)
            {
                case '+':
                    res = compare(v1, v2) != -1 ? byteArrToStr(add(v1, v2)) : byteArrToStr((add(v2, v1)));
                    break;
                case '-':
                    res = compare(v1, v2) != -1 ? byteArrToStr(sub(v1, v2)) : '-' + byteArrToStr((sub(v2, v1)));
                    break;
                case '*':
                    res = compare(v1, v2) != -1 ? byteArrToStr(mult(v1, v2)) : byteArrToStr((mult(v2, v1)));
                    break;
                case '/':
                    res = div(v1, v2);
                    break;
            }
        }

        byte[] add(byte[] v1, byte[] v2)
        {
            byte temp = 0;
            byte[] res = new byte[0];

            for (int i = 0; i < v1.Length; i++)
            {
                if (i < v2.Length)
                {
                    Array.Resize(ref res, res.Length + 1);
                    res[i] = (byte)((v1[i] + v2[i] + temp) % 100);
                    temp = (byte)((v1[i] + v2[i]) / 100);

                } else
                {
                    Array.Resize(ref res, res.Length + 1);
                    res[i] = (byte)((temp + v1[i]) % 100);
                    temp = (byte)((temp + v1[i]) / 100);
                }
                
            }
            if (temp != 0)
            {
                Array.Resize(ref res, res.Length + 1);
                res[res.Length - 1] = temp;
            }
            return res;
        }

        byte[] sub(byte[] v1, byte [] v2)
        {
            byte[] res = new byte[0];
            byte temp = 0;

            for (int i = 0; i < v1.Length; i++)
            {
                if (i < v2.Length)
                {
                    Array.Resize(ref res, res.Length + 1);

                    res[i] = (byte)(v1[i] >= v2[i] ? v1[i] - v2[i] - temp : v1[i] + 100 - v2[i] - temp);
                    temp = (byte)(v1[i] >= v2[i] ? 0 : 1);

                }
                else
                {
                    Array.Resize(ref res, res.Length + 1);

                    res[i] = (byte)(v1[i] >= temp ? v1[i] - temp : v1[i] + 100 - temp);
                    temp = (byte)(v1[i] >= temp ? 0 : 1);

                }

            }
            return res;
        }

        byte[] mult(byte[] v1, byte[] v2)
        {
            byte[] temp1, temp2;
            byte[] t1, t2;

            string zero = "";
            int z = 0;

            {
                zero = String.Concat(Enumerable.Repeat("0", z++));
                t1 = strToByteArr(byteArrToStr(simpleMult(v1, (byte)(v2[0] % 10))) + zero);

                zero = String.Concat(Enumerable.Repeat("0", z++));
                t2 = strToByteArr(byteArrToStr(simpleMult(v1, (byte)(v2[0] / 10))) + zero);

                temp1 = add(t2, t1);
            }
            
            for (int i = 1; i < v2.Length; i++)
            {
                zero = String.Concat(Enumerable.Repeat("0", z++));
                t1 = strToByteArr(byteArrToStr(simpleMult(v1, (byte)(v2[i] % 10))) + zero);

                zero = String.Concat(Enumerable.Repeat("0", z++));
                t2 = strToByteArr(byteArrToStr(simpleMult(v1, (byte)(v2[i] / 10))) + zero);

                temp2 = add(t2, t1);
                temp1 = add(temp2, temp1);
            }
            return temp1;

        }

        string div(byte[] v1, byte[] v2)
        {
            int d1 = v1[v1.Length - 1] > 9 ? v1.Length * 2 : v1.Length * 2 - 1;
            int d2 = v2[v2.Length - 1] > 9 ? v2.Length * 2 : v2.Length * 2 - 1;

            byte i = 0; //to find num for answ string
            string val1 = byteArrToStr(v1);
            string answ = "";

            byte[] valToDiv = strToByteArr(val1.Substring(0, d2));
            byte[] checkZero = { 0 }; //just 'cause im too lazy
            val1 = val1.Substring(d2);
            if (compare(valToDiv, v2) == -1) { valToDiv = strToByteArr(byteArrToStr(valToDiv) + val1[0]); val1 = val1.Substring(1); }

            while (val1 != "" || compare(valToDiv, v2) != -1)
            {
                while (compare(valToDiv, simpleMult(v2, i)) != -1) i++;
                answ += (--i).ToString();

                valToDiv = strToByteArr(byteArrToStr(sub(valToDiv, simpleMult(v2, i))) + (val1 != "" ? val1[0] : ""));
                val1 = val1 != "" ? val1.Substring(1) : "";

                i = 0;
            }
            int current = answ.Length;

            if (compare(valToDiv, checkZero) != 0)
            {
                answ += '.';
                Console.WriteLine("Number of decimal places:");
                int stop = Convert.ToInt32(Console.ReadLine());

                while (compare(valToDiv, checkZero) != 0)
                {
                    valToDiv = strToByteArr(byteArrToStr(valToDiv) + '0');

                    while (compare(valToDiv, simpleMult(v2, i)) != -1) i++;
                    answ += (--i).ToString();

                    valToDiv = strToByteArr(byteArrToStr(sub(valToDiv, simpleMult(v2, i))) + '0');

                    i = 0;

                    if (answ.Length - current > stop) break;
                }
                
            }

            return answ;
        }

        int compare(byte[] v1, byte[] v2) //1 - v1 > v2;    -1 - v1 < v2;    0 - v1 == v2
        {
            if (v1.Length > v2.Length) return 1;
            else if (v1.Length < v2.Length) return -1;
            else
            {
                for (int i = v1.Length - 1; i >= 0; i--)
                {
                    if (v1[i] > v2[i]) return 1;
                    else if (v1[i] < v2[i]) return -1;
                }
                return 0;
            }
        }

        byte[] simpleMult(byte[] v, byte n)
        {
            byte temp = 0;
            byte[] res = new byte[0];

            for (int i = 0; i < v.Length; i++)
            {
                Array.Resize(ref res, res.Length + 1);
                res[i] = (byte)((v[i] * n + temp) % 100);
                temp = (byte)((v[i] * n + temp) / 100);

            }
            if (temp != 0)
            {
                Array.Resize(ref res, res.Length + 1);
                res[res.Length - 1] = temp;
            }

            return res;
        }

        byte[] strToByteArr(string val)
        {
            byte[] v = new byte[(int)Math.Ceiling(val.Length / 2.0)];

            for (int i = 0; i < val.Length; i += 2)
                v[i / 2] = Convert.ToByte(((i == val.Length - 1) ? "0" : val[val.Length - i - 2].ToString()) + val[val.Length - i - 1].ToString());
            return v;
        }

        string byteArrToStr(byte[] v)
        {
            string var = "";
            for (int i = v.Length - 1; i >= 0; i--)
            {
                var += v[i] < 10 ? "0" + v[i].ToString() : v[i].ToString();
            }
            while (var[0].Equals('0') && var.Length != 1) var = var.Substring(1);
            return var;
        }

        public override string ToString()
        {
            return res;
        }
    }
}
