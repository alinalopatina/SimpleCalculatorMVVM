namespace SimpleCalculatorMVVM.Models.Adapters
{
    /// <summary>
    /// Интерфейс для научных вычислений (ожидаемый клиентом)
    /// </summary>
    public interface IScientificCalculator
    {
        double ComputeSin(double degrees);
        double ComputeCos(double degrees);
        double ComputeTan(double degrees);
        double ComputeLog(double value);
        double ComputeLn(double value);
        double ComputeSqrt(double value);
        double ComputePower2(double value);
        double ComputeExp(double value);
    }
}