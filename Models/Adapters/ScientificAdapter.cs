using System;

namespace SimpleCalculatorMVVM.Models.Adapters
{

    public class ScientificAdapter : IScientificCalculator
    {
        private readonly MathAdapter _mathAdapter;

        public ScientificAdapter()
        {
            _mathAdapter = new MathAdapter();
        }

        public double ComputeSin(double degrees)
        {
            double radians = degrees * Math.PI / 180;
            return _mathAdapter.Sin(radians);
        }

        public double ComputeCos(double degrees)
        {
            double radians = degrees * Math.PI / 180;
            return _mathAdapter.Cos(radians);
        }

        public double ComputeTan(double degrees)
        {
            double radians = degrees * Math.PI / 180;
            return _mathAdapter.Tan(radians);
        }

        public double ComputeLog(double value)
        {
            if (value <= 0)
                throw new ArgumentException("log(x) requires x > 0");
            return _mathAdapter.Log10(value);
        }

        public double ComputeLn(double value)
        {
            if (value <= 0)
                throw new ArgumentException("ln(x) requires x > 0");
            return _mathAdapter.Log(value);
        }

        public double ComputeSqrt(double value)
        {
            if (value < 0)
                throw new ArgumentException("sqrt(x) requires x >= 0");
            return _mathAdapter.Sqrt(value);
        }

        public double ComputePower2(double value)
        {
            return _mathAdapter.Power(value, 2);
        }

        public double ComputeExp(double value)
        {
            return _mathAdapter.Exp(value);
        }
    }

    /// Адаптируемый класс 
    public class MathAdapter
    {
        public double Sin(double radians) => Math.Sin(radians);
        public double Cos(double radians) => Math.Cos(radians);
        public double Tan(double radians) => Math.Tan(radians);
        public double Log(double value) => Math.Log(value);
        public double Log10(double value) => Math.Log10(value);
        public double Sqrt(double value) => Math.Sqrt(value);
        public double Power(double value, double power) => Math.Pow(value, power);
        public double Exp(double value) => Math.Exp(value);
    }
}